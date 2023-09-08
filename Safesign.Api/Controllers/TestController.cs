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
                 var errorResponse = new {
                Message = "The request is invalid. Message has the wrong type.",
                AdditionalInfo = "Only type 1 messages are taken at this stage."
                };
            
                return BadRequest(errorResponse);
            }

            return Ok(result);
        }
    }
}
