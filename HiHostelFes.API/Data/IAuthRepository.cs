using System.Threading.Tasks;
using HiHostelFes.API.Models;

namespace HiHostelFes.API.Data
{
    public interface IAuthRepository
    {
         Task<Admin> Register(Admin user, string password);
         Task<Admin> Login(string username,string password);
         Task<bool> UserExists(string username);
    }
}