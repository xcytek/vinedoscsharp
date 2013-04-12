using System;

namespace vinedos
{
    class PadronEmpleados
    {
        private string _noempleado;
        private string _paterno;
        private string _materno;
        private string _nombre;
        private string _direccion;
        private string _residencia;
        private string _sexo;
        private string _fechanac;
        private string _lugarnacimiento;
        private string _imss;
        private string _rfc;
        private string _curp;
        private string _fechaingreso;
        private string _puesto;


        public string NoEmpleado
        {
            set { _noempleado = value; }
            get { return _noempleado; }
        }
        public string Paterno
        {
            set { _paterno = value; }
            get { return _paterno; }
        }
        public string Materno
        {
            set { _materno = value; }
            get { return _materno; }
        }
        public string Nombre
        {
            set { _nombre = value; }
            get { return _nombre; }
        }
        public string Direccion
        {
            set { _direccion = value; }
            get { return _direccion; }
        }
        public string Residencia
        {
            set { _residencia = value; }
            get { return _residencia; }
        }
        public string Sexo
        {
            set { _sexo = value; }
            get { return _sexo; }
        }
        public string FechaNacimiento
        {
            set { _fechanac = value; }
            get { return _fechanac; }
        }
        public string LugarNacimiento
        {
            set { _lugarnacimiento = value; }
            get { return _lugarnacimiento; }
        }
        public string IMSS
        {
            set { _imss = value; }
            get { return _imss; }
        }
        public string RFC
        {
            set { _rfc = value; }
            get { return _rfc; }
        }
        public string CURP
        {
            set { _curp = value; }
            get { return _curp; }
        }
        public string FechaIngreso
        {
            set { _fechaingreso = value; }
            get { return _fechaingreso; }
        }
        public string Puesto
        {
            set { _puesto = value; }
            get { return _puesto; }
        }

        public PadronEmpleados(string noempleado, string paterno, string materno, string nombre, string direccion, string residencia, string sexo, string fechanacimiento, string lugarnacimiento,
            string imss, string rfc, string curp, string fechaingreso, string puesto)
        {
            this.NoEmpleado = noempleado;
            this.Paterno = paterno;
            this.Materno = materno;
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Residencia = residencia;
            this.Sexo = sexo;
            this.FechaNacimiento = fechanacimiento;
            this.LugarNacimiento = lugarnacimiento;
            this.IMSS = imss;
            this.RFC = rfc;
            this.CURP = curp;
            this.FechaIngreso = fechaingreso;
            this.Puesto = puesto;
        }
    }
}
