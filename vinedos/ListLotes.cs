using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    class ListLotes
    {
        private int _index;
        private string _clave;
        private string _rancho;
        private string _descripcion;
        private double _hectareaje;
        private string _referencia;
        private double _porcentaje;

        public int Index
        {
            get { return _index; }
            set { _index = value; }
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
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }
        public double Hectareaje
        {
            get { return _hectareaje; }
            set { _hectareaje = value; }
        }
        public string Referencia
        {
            get { return _referencia; }
            set { _referencia = value; }
        }
        public double Porcentaje
        {
            get { return _porcentaje; }
            set { _porcentaje = value; }
        }

        //Constructor
        public ListLotes (int index, string clave, string rancho, 
            string descripcion, double hectareaje, string referencia, double porcentaje)
        {
            this.Index = index;
            this.Clave = clave;
            this.Rancho = rancho;
            this.Descripcion = descripcion;
            this.Hectareaje = hectareaje;
            this.Referencia = referencia;
            this.Porcentaje = porcentaje;
        }
    }
}
