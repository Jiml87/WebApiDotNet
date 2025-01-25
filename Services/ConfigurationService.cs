using Microsoft.EntityFrameworkCore;
// using System.Services; 
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using System.Collections.Generic;

// using System.Linq;

using WebApi.Models;
using WebApi.Data;

namespace WebApi.Services
{
    public interface IConfigurationService
    {
        Task<IEnumerable<ConfigurationItem>> GetItems(string keyPattern);
        Task<ConfigurationItem> AddItem(ConfigurationItem item);
        Task<ConfigurationItem> UpdateItemByKey(string key, string value);
        Task<ConfigurationItem> GetItemByKey(string key);
        Task<ConfigurationItem> DeleteItemByKey(string key);
    }
    
    public class ConfigurationService : IConfigurationService
    {
        private readonly AppDbContext _dbContext;

        public ConfigurationService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<ConfigurationItem>> GetItems(string keyPattern)
        {
            var items = await _dbContext.ConfigurationItems.ToListAsync();
            return items;
        }

        public async Task<ConfigurationItem> AddItem(ConfigurationItem item)
        {
            try
            {
                _dbContext.ConfigurationItems.Add(item);
                await _dbContext.SaveChangesAsync();
                
                return item;
            }
            catch (DbUpdateException dbEx)
            {
                // Check SQLite UNIQUE constraint violation
                if (dbEx.InnerException is SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19)
                {
                   
                    throw new InvalidOperationException("A duplicate value was found for a unique field.", dbEx);
                }
                throw;
            }
        }

        public async Task<ConfigurationItem> UpdateItemByKey(string key, string value)
        {
            var existingItem = await _dbContext.ConfigurationItems.SingleAsync(c => c.Key == key);
            existingItem.Value = value;
            await _dbContext.SaveChangesAsync();

            return existingItem;
        }

        public async Task<ConfigurationItem> GetItemByKey(string key)
        {
            var existingItem = await _dbContext.ConfigurationItems.SingleAsync(c => c.Key == key);

            return existingItem;
        }
        public async Task<ConfigurationItem> DeleteItemByKey(string key)
        {
            var existingItem = await _dbContext.ConfigurationItems.SingleAsync(c => c.Key == key);
            _dbContext.ConfigurationItems.Remove(existingItem); 
            await _dbContext.SaveChangesAsync();

            return existingItem;
        }
    }
}