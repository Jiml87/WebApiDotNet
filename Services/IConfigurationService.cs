using System.Collections.Generic;

using WebApi.Models;

namespace System.Services
{
    public interface IConfigurationService
    {
        IEnumerable<ConfigurationItem> GetItems();
    }
}