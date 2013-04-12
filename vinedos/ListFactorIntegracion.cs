using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    class ListFactorIntegracion
    {
        private int _tiempo_inicio;
        private int _tiempo_fin;
        private int _vacaciones;
        private double _prima_vacacional;
        private int _aguinaldo;
        private int _sueldo;
        private double _factor_vacacional;
        private double _factor_aguinaldo;
        private double _factor;

        public int Tiempo_Inicio
        {
            get { return _tiempo_inicio; }
            set { _tiempo_inicio = value; }
        }
        public int Tiempo_Fin
        {
            get { return _tiempo_fin; }
            set { _tiempo_fin = value; }
        }
        public int Vacaciones
        {
            get { return _vacaciones; }
            set { _vacaciones = value; }
        }
        public double Prima_Vacacional
        {
            get { return _prima_vacacional; }
            set { _prima_vacacional = value; }
        }
        public int Aguinaldo
        {
            get { return _aguinaldo; }
            set { _aguinaldo = value; }
        }
        public int Sueldo
        {
            get { return _sueldo; }
            set { _sueldo = value; }
        }
        public double Factor_Vacacional
        {
            get { return _factor_vacacional; }
            set { _factor_vacacional = value; }
        }
        public double Factor_Aguinaldo
        {
            get { return _factor_aguinaldo; }
            set { _factor_aguinaldo = value; }
        }
        public double Factor
        {
            get { return _factor; }
            set { _factor = value; }
        }

        public ListFactorIntegracion(int tiempo_inicio,int tiempo_fin,int vacaciones,
            double prima_vacacional, int aguinaldo, int sueldo, double factor_vacacional, 
            double factor_aguinaldo, double factor)
        {
            this.Tiempo_Inicio = tiempo_inicio;
            this.Tiempo_Fin = tiempo_fin;
            this.Vacaciones = vacaciones;
            this.Prima_Vacacional = prima_vacacional;
            this.Aguinaldo = aguinaldo;
            this.Sueldo = sueldo;
            this.Factor_Vacacional = factor_vacacional;
            this.Factor_Aguinaldo = Factor_Aguinaldo;
            this.Factor = factor;
        }
   
    }
}
