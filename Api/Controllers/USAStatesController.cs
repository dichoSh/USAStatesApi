using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace EsriStatesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class USAStatesController(IUSAStatesService statesService) : ControllerBase
    {
        [HttpGet(Name = "GetStates")]
        public async Task<IActionResult> Get(string? stateName, CancellationToken ct)
        {
            var result = await statesService.GetStates(stateName, ct);

            if (result is null || !result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }
    }
}
