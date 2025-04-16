using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using labTW.BusinessLogic.Interfaces; // Правильный namespace для ISession

namespace labTW.BusinessLogic
{
    public class BusinessLogic
    {
        public ISession GetSession()
        {
            return new SessionBL();
        }
    }
}
