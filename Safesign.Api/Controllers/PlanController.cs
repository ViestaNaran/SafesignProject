using Safesign.Core;
using Safesign.Services;
using Microsoft.AspNetCore.Mvc;

namespace Safesign.Api;

[ApiController]
[Route("[controller]")]
public class PlanController : ControllerBase 
{
    private readonly PlanService _planService; 

    public PlanController(PlanService planService) {
        _planService = planService;
    }

    [HttpGet]
    public async Task<IActionResult> Get() {
        return Ok(await _planService.GetAll());
    }
    // GET by Id action
    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(string id)
    {
        var plan = _planService.Get(id);

        if (plan == null)
        {
            return NotFound();
        }
        return Ok(await _planService.Get(id));
    }

    [HttpGet("{Id}/signs")]
    public async Task<IActionResult> GetPlanSigns(string Id)
    {
        var signs = await _planService.GetPlanSigns(Id);
        if(signs == null || signs.Count == 0) 
        {
            return NotFound();
        }

        return Ok(signs);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlan([FromBody] PlanRequestModel model)
    {
        if (ModelState.IsValid)
        {
            Plan plan = new Plan(model.Id, model.CSId, model.Responsible);
            var createdPlan = await _planService.Add(plan);
            return CreatedAtAction(nameof(Get), new { id = createdPlan.Id }, createdPlan);
        }
        return BadRequest(ModelState);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Plan plan)
    {
        await _planService.Update(id, plan);
        return CreatedAtAction(nameof(Get), new { id = plan.Id }, plan);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _planService.Delete(id);
        return NoContent();
    }
}
