using System.Collections.Generic;

using WebApi.Models;

namespace System.Services
{
    public interface IConfigurationService
    {
        Task<IEnumerable<ConfigurationItem>> GetItems();
        Task<ConfigurationItem> AddItem(ConfigurationItem item);
        Task<ConfigurationItem> UpdateItemByKey(string key, string value);
        Task<ConfigurationItem> GetItemByKey(string key);
        Task<ConfigurationItem> DeleteItemByKey(string key);
    }
}