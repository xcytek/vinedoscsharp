using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    public class SearchFunction
    {
        private string _no_empleado;
        private string _paterno;
        private string _materno;
        private string _nombre;

        public string NoEmpleado
        {
            get { return _no_empleado; }
            set { _no_empleado = value; }
        }
        public string Paterno
        {
            get { return _paterno; }
            set { _paterno = value; }                                     
        }
        public string Materno
        {
            get { return _materno; }
            set { _materno = value; }
        }
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public SearchFunction(string NoEmp, string paterno, string materno, string nombre)
        {
            this.NoEmpleado = NoEmp;
            this.Paterno = paterno;
            this.Materno = materno;
            this.Nombre = nombre;
        }
        
        
    }
}
