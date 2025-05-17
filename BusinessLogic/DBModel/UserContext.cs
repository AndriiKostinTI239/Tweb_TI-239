using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using labTW.Domain.Entities.Users;

namespace BusinessLogic.DBModel
{
    public class UserContext : DbContext
    {
        public UserContext() : 
            base("name=DefaultConnection")
        {  
        }
        public virtual DbSet<UDbTable> Users { get; set; }
    }
}
