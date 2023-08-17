using System.ComponentModel.DataAnnotations;


namespace Safesign.Core;
public class PlanRequestModel
{
    [Required]
    public string Id { get; set; }
    
    [Required]
    public string Responsible { get; set; }
}