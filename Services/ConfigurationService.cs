using System.Services; 
using System.Collections.Generic;
using System.Linq;

using WebApi.Models;

namespace WebApi.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly List<ConfigurationItem> _items = new();

        public IEnumerable<ConfigurationItem> GetItems()
        {
            return _items;
        }
    }
}