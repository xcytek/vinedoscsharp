using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    class ListCuadrillas
    {
        private int _id;
        private int _cuadrilla;
        private int _supervisor;
        private string _rancho;
        private string _supnombre;
        private int _supervisor_eventual;
        private string _supnombre_eventual;
        private string _rancho_eventual;
       

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public int Cuadrilla
        {
            get { return _cuadrilla; }
            set { _cuadrilla= value; }
        }
        public int Supervisor
        {
            get { return _supervisor; }
            set { _supervisor = value; }
        }
        public string Rancho
        {
            get { return _rancho; }
            set { _rancho = value; }
        }
        public string SupNombre
        {
            get { return _supnombre; }
            set { _supnombre = value; }
        }
        public int SupervisorEventual
        {
            get { return _supervisor_eventual; }
            set { _supervisor_eventual = value; }
        }
        public string SupNombreEventual
        {
            get { return _supnombre_eventual; }
            set { _supnombre_eventual = value; }
        }
        public string RanchoEventual
        {
            get { return _rancho_eventual; }
            set { _rancho_eventual = value; }
        }
        public ListCuadrillas(int id, int cuadrilla, int supervisor, string rancho, string supnombre,int supervisoreventual,string supnombreeventual,string ranchoeventual)
        {
            this.Id = id;
            this.Cuadrilla = cuadrilla;
            this.Supervisor = supervisor;
            this.Rancho = rancho;
            this.SupNombre = supnombre;
            this.SupervisorEventual = supervisoreventual;
            this.SupNombreEventual = supnombreeventual;
            this.RanchoEventual = ranchoeventual;
        }
    }
}
