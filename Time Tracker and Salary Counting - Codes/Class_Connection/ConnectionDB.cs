using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMTTS_Feb_10_2023.Class_Connection
{
    class ConnectionDB
    {
        public string GetConnection()
        {
            string conn1 = "Data Source=DESKTOP-L0FUGSM;Initial Catalog=PMTTS_SERVER_ROOM;Integrated Security=True";
            return conn1;
        }
    }
}
