using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Interceptador.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly RalmsContext _db;

        public TestController( RalmsContext sampleContext)
        {
            _db = sampleContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _ = _db.People.Count();
            _ = _db.People.ToList();

            return Ok();
        }
    }
}
