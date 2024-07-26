using EduHome.Services.Interfaces;

namespace EduHome.Services
{
    public class UserService : IUserService
    {
        // Örnek olarak, bu metodun nasıl implement edileceği burada yer almaktadır
        public async Task<bool> CreateUserAsync(string fullName, string userName, string email, string password)
        {
            // Kullanıcı oluşturma işlemini burada yapın
            // Bu bir örnek olduğu için gerçek uygulamanızda veri tabanı işlemleri vs. eklemelisiniz

            return true;
        }
    }
}
