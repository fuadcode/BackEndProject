namespace EduHome.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(string fullName, string userName, string email, string password);
    }
}
