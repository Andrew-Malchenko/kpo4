using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication7
{
    public class user
    {
        public int? id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password_hash { get; set; }
        public string role { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }

    public class session
    {
        public int? id { get; set; }
        public int user_id { get; set; }
        public string session_token { get; set; }
        public DateTime expires_at { get; set; }
    }
    public class DbContexts: DbContext
    {
        public DbSet<user> user { get; set; }
        public DbSet<session> session { get; set; }
        public DbContexts() { }
        public DbContexts(DbContextOptions<DbContexts> options): base(options) { }
    }
}
