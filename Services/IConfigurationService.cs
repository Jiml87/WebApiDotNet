using System.Collections.Generic;

using WebApi.Models;

namespace System.Services
{
    public interface IConfigurationService
    {
        Task<IEnumerable<ConfigurationItem>> GetItems();
        Task<ConfigurationItem> AddItem(ConfigurationItem item);
    }
}