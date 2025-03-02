﻿using Sportify.Models;

namespace Sportify.Services.Interfaces
{
    public interface IUserService
    {
        Task<RegistrationResult> Register(User user);
        Task<LoginResult> Login(string email, string password);
        Task<User?> GetUserProfile(int id);
        Task<bool> DeleteUser(int id);
        Task<UpdateProfileResult> UpdateUserProfile(int id, User updatedUser);
        Task<IEnumerable<User>> GetAllUsers();
        Task<UpdateProfileResult> UpdateUserRole(int id, string role);
    }
}
