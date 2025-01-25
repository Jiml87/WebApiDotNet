using Microsoft.EntityFrameworkCore;
using System.Services; 
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;

// using System.Linq;

using WebApi.Models;
using WebApi.Data;

namespace WebApi.Services
{
    
    public class ConfigurationService : IConfigurationService
    {
        private readonly AppDbContext _dbContext;

        public ConfigurationService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<ConfigurationItem>> GetItems()
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
            var existingItem = await _dbContext.ConfigurationItems
                                            .FirstOrDefaultAsync(c => c.Key == key);

            if (existingItem == null)
            {
                return null;
            }

            existingItem.Value = value;

            await _dbContext.SaveChangesAsync();
            return existingItem;
        }

        public async Task<ConfigurationItem> GetItemByKey(string key)
        {
            var existingItem = await _dbContext.ConfigurationItems
                                            .FirstOrDefaultAsync(c => c.Key == key);

            if (existingItem == null)
            {
                return null;
            }

            return existingItem;
        }
    }
}