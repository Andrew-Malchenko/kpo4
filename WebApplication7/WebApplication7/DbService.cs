using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication7
{
    public class DbService
    {
        private readonly DbContexts context;

        public DbService(DbContexts context)
        {
            this.context = context;
        }

        public List<user> GetUsers()
        {
            return context.user.ToList();
        }

        public void InsertUser(string email, string username, string password)
        {
            context.user.Add(new user { email = email, password_hash = CreateMD5(password), role = "chef", created_at = DateTime.Now, updated_at = DateTime.Now, username = username });
        }

        public bool CheckEmail(string email)
        {
            return context.user.Any(c => c.email == email);
        }

        public bool CheckUserName(string username)
        {
            return context.user.Any(c => c.username == username);
        }

        public bool CheckPassword(string email, string password)
        {
            return context.user.Any(c => c.email == email && c.password_hash == CreateMD5(password));
        }

        public int GetIdByEmail(string email)
        {
            return context.user.FirstOrDefault(c => c.email == email).id.Value;
        }

        public bool ActiveSession(int user_id)
        {
            return context.session.Any(c => c.user_id == user_id && c.expires_at > DateTime.Now);
        }

        public void InsertSession(int user_id, string token, DateTime expires_to)
        {
            context.session.Add(new session { user_id = user_id, session_token = token, expires_at = expires_to });
        }
        
        public void SaveChange()
        {
            context.SaveChanges();
        }

        private static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes);
            }
        }

    }
}
