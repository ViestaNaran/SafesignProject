using Safesign.Core;
using Safesign.Services;
using Microsoft.AspNetCore.Mvc;

namespace Safesign.Api;

[ApiController]
[Route("[Controller]")]

public class SignController : ControllerBase 
{
    private readonly SignService _signService;

    public SignController(SignService signService){
        _signService = signService;
    }

    [HttpGet]
    public async Task<IActionResult>Get(){

        return Ok(await _signService.GetAll());
    }

    // GET by Id action
    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(string id)
    {
        var plan = _signService.Get(id);

        if (plan == null)
        {
            return NotFound();
        }
        return Ok(await _signService.Get(id));
    }
    
    // POST action
    // [HttpPost]
    // public async Task<IActionResult> CreateSign(Sign sign)
    // {
    //     return Ok(await _signService.Add(sign));
    // }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Sign sign)
    {
        await _signService.Update(id, sign);
        return CreatedAtAction(nameof(Get), new { id = sign.Id }, sign);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _signService.Delete(id);
        return NoContent();
    }
}