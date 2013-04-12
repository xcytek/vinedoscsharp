using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vinedos
{
    class FuncionesEmpleado
    {
        public int calculaVejez(DateTime fecha)
        {
            int Vejez = 0;
            string fechaString = "";
            fechaString = Convert.ToString(fecha);//convierte tipo fecha a string
            fechaString = fechaString.Substring(0, 10);//obtiene los primeros 10 caracteres que son la fecha "01/01/2000"
            fecha = Convert.ToDateTime(fechaString);//convierte los 10 caracteres a tipo fecha
            TimeSpan ts = DateTime.Now.Date - fecha;
            DateTime d = DateTime.MinValue + ts;
            Vejez = d.Year - 1;
            return Vejez;
        }
    }
}
