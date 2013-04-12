using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    class ListRanchos
    {
        private string _zona;
        private string _clave;
        private string _rancho;
        private double _hectareaje;        

        public string Zona
        {
            get { return _zona; }
            set { _zona = value; }
        }
        public string Clave
        {
            get { return _clave; }
            set { _clave = value; }
        }
        public string Rancho
        {
            get { return _rancho; }
            set { _rancho = value; }
        }
        public double Hectareaje
        {
            get { return _hectareaje; }
            set { _hectareaje = value; }
        }
        
        public ListRanchos (string zona, string clave, string rancho, double hectareaje)
        {
            this.Zona = zona;
            this.Clave = clave;
            this.Rancho = rancho;
            this.Hectareaje = hectareaje;

        }
    }
}
