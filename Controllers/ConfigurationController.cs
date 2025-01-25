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

        [HttpGet("search")]
        public IActionResult GetConfigurationItems(
            [FromQuery] string keyPattern,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest("PageNumber and PageSize must be greater than 0.");
                }
                
                var items = _configurationService.GetItems(keyPattern, pageNumber, pageSize);
                return Ok(items);
            }
            catch (Exception)
            {
                // TODO: log error
                return BadRequest();
            }
        }

        [HttpGet("item/{key}")]
        public async Task<IActionResult> GetConfigurationItem(string key)
        {
            try 
            {
                var item = await _configurationService.GetItemByKey(key);

                return Ok(item);
            }
            catch (InvalidOperationException exc)
            {
                return NotFound($"ConfigurationItem with Key {key} not found.");
            }
            catch(Exception ex)
            {
                // TODO: log error
                return BadRequest();
            }

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

                return Ok(updatedItem);
            }
            catch (InvalidOperationException exc)
            {
                return NotFound($"ConfigurationItem with Key {key} not found.");
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

                return NoContent();
            }
            catch (InvalidOperationException exc)
            {
                return NotFound($"ConfigurationItem with Key {key} not found.");
            }
            catch(Exception ex)
            {
                // TODO: log error
                return BadRequest();
            }

        }
    }

}
