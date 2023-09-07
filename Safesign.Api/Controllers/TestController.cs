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

      [HttpPut("{id}")]
       public async Task<IActionResult> CreateRandom(JsonObject randomObject) {

            return Ok(await _sService.CreateRandom1(randomObject));
        }
    }
}
