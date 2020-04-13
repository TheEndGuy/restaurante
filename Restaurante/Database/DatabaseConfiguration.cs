using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.Database
{
    public class DatabaseConfiguration
    {
        public DatabaseConfiguration()
        {
        }

        public string Server
        {
            get;
            set;
        }

        public string Database
        {
            get;
            set;
        }

        public string UserId
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("Server=" + Server + "; database=" + Database + "; UID=" + UserId + "; password=" + Password);
        }
    }
}

