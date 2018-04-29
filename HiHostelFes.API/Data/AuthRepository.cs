using System;
using System.Threading.Tasks;
using HiHostelFes.API.Models;
using Microsoft.EntityFrameworkCore;
namespace HiHostelFes.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            this._context = context;
            

        }
        public async Task<Admin> Login(string username, string password)
        {
            var user = await _context.Admins.FirstOrDefaultAsync(x=>x.Username.Equals(username));
            if(user==null)
                return null;
            if(!VerifyHashedPassword(password,user.PasswordHash,user.PasswordSalt))
                return null;
            //authentication successfull
            return user;
        }

        private bool VerifyHashedPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
             using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                //Conveting the password string to array of bytes[]
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0;i<computedHash.Length;i++){
                    if(computedHash[i]!=passwordHash[i])
                        return false;
                }
            }
            return true;
        }

        public async Task<Admin> Register(Admin user, string password)
        {
            byte[] passwordHash, passwordSalt;
            //out because we want to pass a reference of variables that are not initialized yet
            CreatePasswordHash(password,out passwordHash,out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.Admins.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                //Conveting the password string to array of bytes[]
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); 
            }
        }

        public async Task<bool> UserExists(string username)
        {
           if(await _context.Admins.AnyAsync(x=>x.Username.Equals(username)))
                return true;
        
        return false;
        }
    }
}