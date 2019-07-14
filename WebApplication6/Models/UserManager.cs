using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApplication6.ViewModels;

namespace WebApplication6.Models
{
    public class UserManager:IUserManager
    {
        private IUserRepository repository;
        public UserManager(IUserRepository repo)
        {
            repository = repo;
        }

        public User FindUser(string username,string password)
        {
            string hashed_pass = repository.GetPassword(username);
            if (hashed_pass != null)
            {

                bool isAuthenticated = VerifyPassword(password, hashed_pass);
                if (isAuthenticated)
                {
                   
                    return repository.GetUser(username);
                    
                }
            }
            return null;

        }
        private string HashPassword(string PlainPassword)
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(PlainPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }
        private bool VerifyPassword(string password,string savedPasswordHash)
        {
           
            /* Extract the bytes */ 
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public int? VerifyAuthCredentials(string selector,string validator)
        {
            Auth_tokens auth = repository.GetTokens(selector);
            var result = 0;
            if (auth != null)
            {

                var cookie_validator = Sha256(validator);
                if (auth.Validator.Length != cookie_validator.Length)
                    return null;
                for (int i = 0; i < auth.Validator.Length; i++)
                {
                    result = result | (auth.Validator[i] ^ cookie_validator[i]);
                }

                if (result == 0)
                {
                    return auth.UserId;
                }
            }
            
            return null;
            
        }
        public void SetAuthCredentials(string selector,string validator,int userid,DateTime expires)
        {
            
            repository.SetTokens(selector, Sha256(validator), userid,expires);
        }
        public string GenerateSelector()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] token = new byte[9];
                rng.GetBytes(token);
                string token_str = Convert.ToBase64String(token);
                return token_str;
            }
        }
        public string GenerateToken()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] token = new byte[32];
                rng.GetBytes(token);
                string token_str = Convert.ToBase64String(token);
                return token_str;
            }
        }
        public string Sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

    }
}
