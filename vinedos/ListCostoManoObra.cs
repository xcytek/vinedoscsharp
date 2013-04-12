using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    class ListCostoManoObra
    {
        private int _id_empleado;
        private double _sueldodiario;
        private double _otros;
        private string _rancho;
        private string _lote;
        private string _actividad;
        private double _he;
        private DateTime _fecha;       

        public int IdEmpleado
        {
            get { return _id_empleado; }
            set { _id_empleado = value; }
        }
        public double SueldoDiario
        {
            get { return _sueldodiario; }
            set { _sueldodiario= value; }
        }
        public double Otros
        {
            get { return _otros; }
            set { _otros = value; }
        }
        public string Rancho
        {
            get { return _rancho; }
            set { _rancho = value; }
        }
        public string Lote
        {
            get { return _lote; }
            set { _lote = value; }
        }
        public string Actividad
        {
            get { return _actividad; }
            set { _actividad = value; }
        }
        public double HE
        {
            get { return _he; }
            set { _he = value; }
        }
        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }
        public ListCostoManoObra(int idempleado, double sueldodiario, double otros, string rancho, string lote, string actividad, double he, DateTime fecha)
        {
            this.IdEmpleado = idempleado;
            this.SueldoDiario = sueldodiario;
            this.Otros = otros;
            this.Rancho = rancho;
            this.Lote = lote;
            this.Actividad = actividad;
            this.HE = he;
            this.Fecha = fecha;
        }
    }
}
