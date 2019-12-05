using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Interceptador
{
    public class RalmsInterceptor : DbCommandInterceptor
    {
        private static readonly Regex _tableAliasRegex =
            new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))",
                RegexOptions.Multiline |
                RegexOptions.IgnoreCase |
                RegexOptions.Compiled);

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            if (!command.CommandText.Contains("WITH (NOLOCK)"))
            {
                command.CommandText =
                    _tableAliasRegex.Replace(command.CommandText,
                    "${tableAlias} WITH (NOLOCK)");
            }

            return result;
        }
    }

    public class RalmsContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFCORE31;Trusted_Connection=True;Connect Timeout=5;ConnectRetryCount=0")
                .AddInterceptors(new RalmsInterceptor());
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
    }
}
