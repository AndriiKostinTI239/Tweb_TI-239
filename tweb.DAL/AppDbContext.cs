using labTW.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBModel;

namespace tweb.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=DefaultConnection") { }

        public virtual DbSet<UDbTable> Users { get; set; }

    }

}
