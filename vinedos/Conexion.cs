using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace vinedos
{
    class clConexion
    {
        //string servidor = "MXVID73T94D1";
        string servidor = "CETTO";
        string servidorIP = "192.168.0.102,1433";        
        public SqlConnection conexion()
        {
            SqlConnection miConexion = new SqlConnection("Server=(local)\\SQLEXPRESS;database=GCETTO;integrated security=yes");            
            if (miConexion.WorkstationId != servidor)
                //miConexion = new SqlConnection("Data Source=" + servidorIP + ";Network Library=DBMSSOCN;Initial Catalog=GCETTO;User ID=sa;Password=1234");
                miConexion = new SqlConnection("Data Source=" + servidor + "\\SQLEXPRESS;Initial Catalog=GCETTO;User ID=sa;Password=1234");
            //string hola = "Data Source=" + servidorIP + ";Network Library=DBMSSOCN;Initial Catalog=GCETTO;User ID=sa;Password=1234";
            return miConexion;
        }
    }    
}
