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
        var sign = await _signService.Get(id);

        if (sign == null)
        {
            return NotFound(new {message = "Sign not found"});
        }
        return Ok(sign);
    }
    
    [HttpGet("cs/{Id}")]
    public async Task<IActionResult> GetByCSId(string id)
    {
        var signs = await _signService.GetSignsByCSId(id);

        if (signs == null)
        {
            return NotFound(new {message = "Sign not found"});
        }
        return Ok(signs);
    }

    [HttpGet("plan/{Id}")]
    public async Task<IActionResult> GetByPlanId(string id)
    {
        var signs = await _signService.GetSignsByPlanId(id);

        if (signs == null)
        {
            return NotFound(new {message = "Sign not found"});
        }
        return Ok(signs);
    }

    //POST action
    [HttpPost]
    public async Task<IActionResult> CreateSign(Sign sign)
    {
        return Ok(await _signService.Add(sign));
    }

    [HttpPost("sensor")]
    public async Task<IActionResult> CreateSignWithSensor(string csId, string planId, string macId)
    {
        return Ok(await _signService.CreateSignWithSensor(csId, planId, macId));
    }

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
    
    [HttpGet("checksignangle/{signId}")]
    public async Task<IActionResult> CheckSignAngle(string signId)
    {
        bool isAngleValid = await _signService.CheckSignAngle(signId);

        return Ok(isAngleValid);
    }

    [HttpPut("{Id}/updatesensorid")]
    public async Task<IActionResult> UpdateSensorId(string signId, string newSensorId)
    {
    // Call the service method to update the "SensorId" value.
    var updatedSign = await _signService.UpdateSensorId(signId, newSensorId);

    if (updatedSign == null)
    {
        // Handle the case where the Sign with the specified ID doesn't exist.
        return NotFound($"Sign with ID {signId} not found.");
    }
        return Ok(updatedSign);
    }






}