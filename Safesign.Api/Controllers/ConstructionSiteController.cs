using Safesign.Core;
using Safesign.Services;
using Microsoft.AspNetCore.Mvc;



namespace Safesign.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConstructionSiteController : ControllerBase
    {
            private readonly ConstructionSiteService _cSService;

            public ConstructionSiteController(ConstructionSiteService cSSercice)
            {
                _cSService = cSSercice;
            }

            // GET all action
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                return Ok(await _cSService.GetAll());
            }
        }
}
