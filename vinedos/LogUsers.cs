using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    public class LogUsers
    {
        public static string _username;
        public static string _password;
        public static int _nivel;
        public static string _computername;
        public static string _ip;
       

        public static void NuevoUsers(string username, string password, int nivel, string computername, string ip)
        {
            LogUsers._username = username;
            LogUsers._password = password;
            LogUsers._nivel = nivel;
            LogUsers._computername = computername;
            LogUsers._ip = ip;
        }
        public static int Deleteuser(string username)
        {
            int ban;
            ban = Funciones.DeleteSesion(username);
            if (ban == 0)
            {
                LogUsers._username = "";
                LogUsers._password = ""; 
                LogUsers._nivel = 0;
                LogUsers._computername = "";
                LogUsers._ip = "";
            }
            return ban;
        }
        public static string getUserName()
        {
            return LogUsers._username;
        }
        public static string getPassword()
        {
            return LogUsers._password;
        }
        public static int getNivel()
        {
            return LogUsers._nivel;
        }
        public static string getComputerName()
        {
            return LogUsers._computername;
        }
        public static string getIp()
        {
            return LogUsers._ip;     
        }        
    }
}