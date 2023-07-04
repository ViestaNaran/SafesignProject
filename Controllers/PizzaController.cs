using SafesignProject.Models;
using SafesignProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace SafesignProject.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    private readonly PizzaService _pService;

    public PizzaController(PizzaService pService)
    {
        _pService = pService;
    }

    // GET all action
    [HttpGet]
    public async Task<IActionResult> GetAll() 
    {
        return Ok(await _pService.GetAll());
    }
        
    // GET by Id action
    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(string id) {
        var pizza = _pService.Get(id);

        if(pizza == null){
            return NotFound();
        }
        return Ok(await _pService.Get(id));
    }
    // POST action
    [HttpPost]
    public async Task<IActionResult> CreatePizza(Pizza pizza) {
        
        return Ok(await _pService.Add(pizza));
        //return CreatedAtAction(nameof(Get), new { id = pizza.Id}, pizza);
    }

   [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Pizza pizza)
    {
        // if (id != pizza.Id)
        //      return BadRequest();
           
        // var existingPizza = _pService.Get(id);
        // if(existingPizza == null)
        //     return NotFound();
   
        await _pService.Update(id,pizza);           
   
        return CreatedAtAction(nameof(Get),new {id = pizza.Id}, pizza);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) {
    
        await _pService.Delete(id);
        return NoContent();
    }
}