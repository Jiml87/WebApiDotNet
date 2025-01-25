using Microsoft.AspNetCore.Mvc;
using System.Services;

using WebApi.Models;
using WebApi.Services;
using System.Text.Json;

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
        public async Task<IActionResult> AddItem([FromBody] ConfigurationItemDto dto)
        {
            try
            {
                var configurationItem = new ConfigurationItem
                {
                    Key = dto.Key,
                    Value = JsonSerializer.Serialize(dto.Value)
                };
                
                var addedItem = await _configurationService.AddItem(configurationItem);
          
                return Created("v1/api/[controller]", addedItem);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("!!!!!!!!!!!!!!!!!");
                Console.WriteLine(ex.ToString());
                // TODO: log error
                return BadRequest();
            }
        }
    }

}
