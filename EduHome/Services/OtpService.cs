using EduHome.Services.Interfaces;
using System.Security.Cryptography;

namespace EduHome.Services
{
    public class OtpService : IOtpService
    {
        public string GenerateOTP()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[4]; 
                rng.GetBytes(data);
                int otp = BitConverter.ToUInt16(data, 0) % 1000000; 
                return otp.ToString("D6"); 
            }
        }
    }
}

