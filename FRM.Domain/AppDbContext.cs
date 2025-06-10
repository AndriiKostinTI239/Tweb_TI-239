using System.Data.Entity;
using FRM.Core.Entities;

namespace FRM.Domain
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=AppDbContext") { }

        public DbSet<UserEf> Users { get; set; }

        public DbSet<ThreadEf> Threads { get; set; }
        public DbSet<CommentEf> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Конфигурация для PostgreSQL
            modelBuilder.HasDefaultSchema("public");
        }
    }
}