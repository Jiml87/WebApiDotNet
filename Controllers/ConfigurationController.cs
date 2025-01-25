using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        public IActionResult GetConfigurationItems()
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
                // TODO: log error
                return BadRequest();
            }
        }

        [HttpPut("{key}")]
        public async Task<IActionResult> UpdateConfigurationItem(string key, [FromBody] object data)
        {
            try 
            {
                var updatedItem = await _configurationService.UpdateItemByKey(key, JsonSerializer.Serialize(data));

                if (updatedItem == null)
                {
                    return NotFound($"ConfigurationItem with Key {key} not found.");
                }

                return Ok(updatedItem);
            }
            catch(Exception ex)
            {
                // TODO: log error
                return BadRequest();
            }

        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetConfigurationItem(string key)
        {
            try 
            {
                var item = await _configurationService.GetItemByKey(key);

                if (item == null)
                {
                    return NotFound($"ConfigurationItem with Key {key} not found.");
                }

                return Ok(item);
            }
            catch(Exception ex)
            {
                // TODO: log error
                return BadRequest();
            }

        }
        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteConfigurationItem(string key)
        {
            try 
            {
                var item = await _configurationService.DeleteItemByKey(key);

                if (item == null)
                {
                    return NotFound($"ConfigurationItem with Key {key} not found.");
                }

                return NoContent();
            }
            catch(Exception ex)
            {
                // TODO: log error
                return BadRequest();
            }

        }
    }

}
