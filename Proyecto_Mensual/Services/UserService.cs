using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorCrudApp.Models;

namespace RazorCrudApp.Services
{
    public class UserService
    {
        private readonly JsonFileService _jsonFileService;
        private const string FileName = "users.json";
        
        public UserService(JsonFileService jsonFileService)
        {
            _jsonFileService = jsonFileService;
            InitializeDataAsync().Wait();
        }
        
        private async Task InitializeDataAsync()
        {
            var users = await _jsonFileService.ReadAsync<User>(FileName);
            
            if (users.Count == 0)
            {
                // Add default admin user if no users exist
                users.Add(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = "admin",
                    Password = "admin123", // In a real app, this would be hashed
                    Name = "Administrator"
                });
                
                await _jsonFileService.WriteAsync(FileName, users);
            }
        }
        
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var users = await _jsonFileService.ReadAsync<User>(FileName);
            return users.FirstOrDefault(u => 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
                u.Password == password);
        }
        
        public async Task<User> GetUserByIdAsync(string id)
        {
            var users = await _jsonFileService.ReadAsync<User>(FileName);
            return users.FirstOrDefault(u => u.Id == id);
        }
    }
}
