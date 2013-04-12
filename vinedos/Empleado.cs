using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    public class Empleado
    {
        private int _id;
        private string _paterno;
        private string _materno;
        private string _nombre;
        private string _puesto;
        private string _tipo;
        private string _rancho;
        private DateTime _fechaingreso;
        private double _salariodiario;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
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
        public string Puesto
        {
            get { return _puesto; }
            set { _puesto = value; }
        }
        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }
        public string Rancho
        {
            get { return _rancho; }
            set { _rancho = value; }
        }
        public DateTime FechaIngreso
        {
            get { return _fechaingreso; }
            set { _fechaingreso = value; }
        }
        public double SalarioDiario
        {
            get { return _salariodiario; }
            set { _salariodiario = value; }
        }

        public Empleado(int id, string paterno, string materno, string nombre, string puesto,string tipo, string rancho, DateTime fechaingreso, double salariodiario)
        {
            this.Id = id;
            this.Paterno = paterno;
            this.Materno = materno;
            this.Nombre = nombre;
            this.Puesto = puesto;
            this.Tipo = tipo;
            this.Rancho = rancho;
            this.FechaIngreso = fechaingreso;
            this.SalarioDiario = salariodiario;
        }
        public Empleado(int id, string paterno, string materno, string nombre, string puesto, string tipo, string rancho, DateTime fechaingreso)
        {
            this.Id = id;
            this.Paterno = paterno;
            this.Materno = materno;
            this.Nombre = nombre;
            this.Puesto = puesto;
            this.Tipo = tipo;
            this.Rancho = rancho;
            this.FechaIngreso = fechaingreso;
        }
   

    }
}
