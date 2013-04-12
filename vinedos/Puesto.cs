using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    class Puesto
    {
        private int clave;
        private string puesto;
        private double sueldoDiario;
        private double sueldoMax;
        private double bonoAsist;
        private double bonoRend;
        private double otrosIng;
        private string observaciones;       

        public int Clave
        {
            get { return clave; }
            set { clave = value; }
        }
        public string NomPuesto
        {
            get { return puesto; }
            set { puesto = value; }
        }
        public double Sueldo
        {
            get { return sueldoDiario; }
            set { sueldoDiario = value; }
        }
        public double SueldoMax
        {
            get { return sueldoMax; }
            set { sueldoMax = value; }
        }
        public double BonoAsist
        {
            get { return bonoAsist; }
            set { bonoAsist = value; }
        }
        public double BonoRend
        {
            get { return bonoRend; }
            set { bonoRend = value; }
        }
        public double Otros
        {
            get { return otrosIng; }
            set { otrosIng = value; }
        }
        public string Observacion
        {
            get { return observaciones; }
            set { observaciones = value; }
        }

        public Puesto(int clave, string puesto, double sueldo, double sueldomax, double bonoasis, double bonorend, double otros, string observacion)
        {
            this.Clave = clave;
            this.NomPuesto = puesto;
            this.Sueldo = sueldo;
            this.SueldoMax = sueldomax;
            this.BonoAsist = bonoasis;
            this.BonoRend = bonorend;
            this.Otros = otros;
            this.Observacion = observacion;
        }
    }
}