﻿using Safesign.Core;
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

            [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
        var constructionSite = await _cSService.Get(id);
        
            if (constructionSite == null)
            {
            return NotFound();
            }
        
            return Ok(constructionSite);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ConstructionSite constructionSite)
        {
            var createdConstructionSite = await _cSService.CreateCSsite(constructionSite);
            return CreatedAtAction(nameof(Get), new { id = createdConstructionSite.Id }, createdConstructionSite);
        }

        [HttpPost("fullPackage")]
        public async Task<IActionResult> CreateWithSigns(ConstructionSiteModel csModel) {

            var createdConstructionSite = await _cSService.CreateCSSiteWithSigns(csModel.CSSite, csModel.Signs);
            return CreatedAtAction(nameof(Get), new { id = createdConstructionSite.Id }, createdConstructionSite);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ConstructionSite constructionSite)
        {
            var updatedConstructionSite = await _cSService.Update(id, constructionSite);
            
            if (updatedConstructionSite == null)
            {
                return NotFound();
            }
            
            return Ok(updatedConstructionSite);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _cSService.Delete(id);
            
            return NoContent();
        }
    
    }
}