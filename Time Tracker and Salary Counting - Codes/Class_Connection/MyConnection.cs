using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMTTS_Feb_10_2023.Class_Connection
{
    internal class MyConnection
    {
        public SqlConnection con;

        public MyConnection() 
        { 
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["CC"].ConnectionString);;
        }

    }
}
