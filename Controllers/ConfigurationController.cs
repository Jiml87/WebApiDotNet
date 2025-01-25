using Microsoft.AspNetCore.Mvc;
using System.Services;

using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;
        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }
    
        [HttpGet]
        public IActionResult GetItems()
        {
            var items = _configurationService.GetItems();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ConfigurationItem item)
        {
            if (item == null)
            {
                return BadRequest("Item cannot be null.");
            }

            try
            {
                var addedItem = await _configurationService.AddItem(item);
          
                return StatusCode(201, addedItem);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Bad request: {ex.Message}");
            }
        }
    }

}
