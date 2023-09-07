using Safesign.Core;
using Safesign.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;



namespace Safesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly SensorService _sService;

        public TestController(SensorService sService)
        {
            _sService = sService;
        }

      [HttpPost]
       public async Task<IActionResult> CreateRandom(JsonObject randomObject) {

            var result = await _sService.CreateRandom1(randomObject);

            if(result == null) {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
