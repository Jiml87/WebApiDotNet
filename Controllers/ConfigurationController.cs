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
    }

}
