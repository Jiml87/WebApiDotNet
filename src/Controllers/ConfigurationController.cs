using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.Extensions.Logging;

using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogger<ConfigurationController> _logger;
        public ConfigurationController(IConfigurationService configurationService, ILogger<ConfigurationController> logger)
        {
            _configurationService = configurationService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a paginated list of configuration items matching the key pattern.
        /// </summary>
        /// <param name="keyPattern">The key pattern to search for.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A list of matching configuration items.</returns>
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching configuration items.");
                return BadRequest();
            }
        }

        /// <summary>
        /// Retrieves a configuration item by its key.
        /// </summary>
        /// <param name="key">The unique key of the configuration item to retrieve.</param>
        /// <returns>
        /// Returns a status code of:
        /// <list type="bullet">
        /// <item>
        /// <description><c>200 OK</c>: If the configuration item is found.</description>
        /// </item>
        /// <item>
        /// <description><c>404 Not Found</c>: If no configuration item with the specified key is found.</description>
        /// </item>
        /// <item>
        /// <description><c>400 Bad Request</c>: If an error occurs during the operation.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpGet("item/{key}")]
        public async Task<IActionResult> GetConfigurationItem(string key)
        {
            try 
            {
                var item = await _configurationService.GetItemByKey(key);

                return Ok(item);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"Configuration item with key \"{key}\" not found: {ex.Message}");
                return NotFound($"ConfigurationItem with Key \"{key}\" not found.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving configuration item.");
                return BadRequest();
            }

        }
    
        
        /// <summary>
        /// Adds a new configuration item.
        /// </summary>
        /// <param name="data">The data transfer object containing the key and value of the configuration item to be added.</param>
        /// <returns>
        /// Returns a status code of:
        /// <list type="bullet">
        /// <item>
        /// <description><c>201 Created</c>: If the configuration item is successfully created.</description>
        /// </item>
        /// <item>
        /// <description><c>409 Conflict</c>: If a configuration item with the same key already exists.</description>
        /// </item>
        /// <item>
        /// <description><c>400 Bad Request</c>: If an error occurs during the operation.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] CreateConfigurationItemDto data)
        {
            try
            {
                var configurationItem = new ConfigurationItem
                {
                    Key = data.Key,
                    Value = JsonSerializer.Serialize(data.Value)
                };
                
                var addedItem = await _configurationService.AddItem(configurationItem);
          
                return Created("v1/api/[controller]", addedItem);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"Conflict occurred while adding configuration item with key \"{data.Key}\": {ex.Message}");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "An error occurred while adding configuration item.");
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates an existing configuration item by its key.
        /// </summary>
        /// <param name="key">The unique key of the configuration item to update.</param>
        /// <param name="data">The new value for the configuration item.</param>
        /// <returns>
        /// Returns a status code of:
        /// <list type="bullet">
        /// <item>
        /// <description><c>200 OK</c>: If the configuration item is successfully updated, returns the updated item.</description>
        /// </item>
        /// <item>
        /// <description><c>404 Not Found</c>: If no configuration item with the specified key is found.</description>
        /// </item>
        /// <item>
        /// <description><c>400 Bad Request</c>: If an error occurs during the operation.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpPut("{key}")]
        public async Task<IActionResult> UpdateConfigurationItem(string key, [FromBody] object data)
        {
            try 
            {
                var updatedItem = await _configurationService.UpdateItemByKey(key, JsonSerializer.Serialize(data));

                return Ok(updatedItem);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"ConfigurationItem with Key \"{key}\" not found: {ex.Message}");
                return NotFound($"ConfigurationItem with Key \"{key}\" not found.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating configuration item.");
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes a configuration item by its key.
        /// </summary>
        /// <param name="key">The unique key of the configuration item to delete.</param>
        /// <returns>
        /// Returns a status code of:
        /// <list type="bullet">
        /// <item>
        /// <description><c>204 No Content</c>: If the configuration item is successfully deleted.</description>
        /// </item>
        /// <item>
        /// <description><c>404 Not Found</c>: If no configuration item with the specified key is found.</description>
        /// </item>
        /// <item>
        /// <description><c>400 Bad Request</c>: If an error occurs during the operation.</description>
        /// </item>
        /// </list>
        /// </returns>
        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteConfigurationItem(string key)
        {
            try 
            {
                var item = await _configurationService.DeleteItemByKey(key);

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"Configuration item with key \"{key}\" not found for deletion: {ex.Message}");
                return NotFound($"ConfigurationItem with Key \"{key}\" not found.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting configuration item.");
                return BadRequest();
            }

        }
    }

}
