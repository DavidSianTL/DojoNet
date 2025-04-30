using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorCrudApp.Models;

namespace RazorCrudApp.Services
{
    public class ItemService
    {
        private readonly JsonFileService _jsonFileService;
        private const string FileName = "items.json";
        
        public ItemService(JsonFileService jsonFileService)
        {
            _jsonFileService = jsonFileService;
            InitializeDataAsync().Wait();
        }
        
        private async Task InitializeDataAsync()
        {
            var items = await _jsonFileService.ReadAsync<Item>(FileName);
            
            if (items.Count == 0)
            {
                // Add sample data if no items exist
                items.Add(new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Laptop",
                    Description = "High-performance laptop with 16GB RAM",
                    Category = "Electronics",
                    Price = 1299.99m,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                
                items.Add(new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Desk Chair",
                    Description = "Ergonomic office chair with lumbar support",
                    Category = "Home",
                    Price = 249.99m,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                
                await _jsonFileService.WriteAsync(FileName, items);
            }
        }
        
        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _jsonFileService.ReadAsync<Item>(FileName);
        }
        
        public async Task<Item> GetItemByIdAsync(string id)
        {
            var items = await _jsonFileService.ReadAsync<Item>(FileName);
            return items.FirstOrDefault(i => i.Id == id);
        }
        
        public async Task<Item> CreateItemAsync(Item item)
        {
            var items = await _jsonFileService.ReadAsync<Item>(FileName);
            
            item.Id = Guid.NewGuid().ToString();
            item.CreatedAt = DateTime.Now;
            item.UpdatedAt = DateTime.Now;
            
            items.Add(item);
            await _jsonFileService.WriteAsync(FileName, items);
            
            return item;
        }
        
        public async Task<Item> UpdateItemAsync(string id, Item updatedItem)
        {
            var items = await _jsonFileService.ReadAsync<Item>(FileName);
            var existingItem = items.FirstOrDefault(i => i.Id == id);
            
            if (existingItem == null)
            {
                return null;
            }
            
            existingItem.Name = updatedItem.Name;
            existingItem.Description = updatedItem.Description;
            existingItem.Category = updatedItem.Category;
            existingItem.Price = updatedItem.Price;
            existingItem.UpdatedAt = DateTime.Now;
            
            await _jsonFileService.WriteAsync(FileName, items);
            
            return existingItem;
        }
        
        public async Task<bool> DeleteItemAsync(string id)
        {
            var items = await _jsonFileService.ReadAsync<Item>(FileName);
            var item = items.FirstOrDefault(i => i.Id == id);
            
            if (item == null)
            {
                return false;
            }
            
            items.Remove(item);
            await _jsonFileService.WriteAsync(FileName, items);
            
            return true;
        }
    }
}
