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
    // POST action
    [HttpPost]
    public async Task<IActionResult> CreatePlan(Plan plan)
    {
        return CreatedAtAction(nameof(Get), new {id = plan.Id}, await _planService.Add(plan));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Plan plan)
    {
        await _planService.Update(id, plan);
        return CreatedAtAction(nameof(Get), new { id = plan.Id }, plan);
    }

    [HttpPut("{id}/sign")]
    public async Task<IActionResult> AddSign(string id, Plan plan)
    {
        await _planService.AddSignToPlan(id);
        return CreatedAtAction(nameof(Get), new {id = plan.Id}, plan);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _planService.Delete(id);
        return NoContent();
    }
}
