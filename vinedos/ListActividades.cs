using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    class ListActividades
    {
        private int _clave;
        private string _actividad;

        public int Clave
        {
            get { return _clave; }
            set { _clave = value; }
        }
        public string Actividad
        {
            get { return _actividad; }
            set { _actividad = value; }
        }
        public ListActividades(int clave, string actividad)
        {
            this.Clave = clave;
            this.Actividad = actividad;
        }
    }
}
