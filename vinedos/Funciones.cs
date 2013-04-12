using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace vinedos
{
    class Funciones
    {
        //***Arreglos para ISR***//
        double[,] arrISR = new double[4, 8];
        double[,] arrSubsidio = new double[3, 11];
        FuncionesEmpleado fnEmpleado = new FuncionesEmpleado();
        //***--------|||-------***\\ 
        /****LLENA ARREGLOS CON TABLAS DE ISR****/
        public void llenaArrISR()
        {
            clConexion objConexion = new clConexion();
            string queryEmpleado = "SELECT * FROM Isr_Cuota";
            try
            {
                using (SqlCommand cmd = new SqlCommand(queryEmpleado, objConexion.conexion()))
                {//Carga los datos de ISR
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)                    
                        MessageBox.Show("Error con datos de ISR", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                    else
                    {
                        int i = 0;
                        while (dr.Read())
                        {
                            //guarda Limite inferior, superior, cuota fija y porcentaje
                            arrISR[0, i] = Convert.ToDouble(dr[0].ToString());
                            arrISR[1, i] = Convert.ToDouble(dr[1].ToString());
                            arrISR[2, i] = Convert.ToDouble(dr[2].ToString());
                            arrISR[3, i] = Convert.ToDouble(dr[3].ToString());
                            i++;
                        }

                    }
                    cmd.Connection.Close();
                }
                queryEmpleado = "SELECT * FROM Isr_Subsidio";
                using (SqlCommand cmd = new SqlCommand(queryEmpleado, objConexion.conexion()))
                {//Carga los datos de subsidio para ISR
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)                    
                        MessageBox.Show("Error con datos de ISR", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                    else
                    {
                        int j = 0;
                        while (dr.Read())
                        {
                            //inferior, superior y subsidio
                            arrSubsidio[0, j] = Convert.ToDouble(dr[0].ToString());
                            arrSubsidio[1, j] = Convert.ToDouble(dr[1].ToString());
                            arrSubsidio[2, j] = Convert.ToDouble(dr[2].ToString());
                            j++;
                        }
                    }
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " No se puede cargar datos de ISR");
            }
        }
        //*** LLENA LA LISTA DE FACTOR INTEGRACION DESDE DB ***\\
        public List<ListFactorIntegracion> llenaFactorIntegracion(List<ListFactorIntegracion> objFactorIntegracion)
        {
            clConexion objConexion = new clConexion();            
            string query = "SELECT * FROM Factor_Integracion";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {//Carga los datos de ISR
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)
                        MessageBox.Show("Error con datos de Factor de integracion", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        while (dr.Read())
                            //guarda la info en la lista
                            objFactorIntegracion.Add(new ListFactorIntegracion(Convert.ToInt32(dr[0].ToString()), Convert.ToInt32(dr[1].ToString()),
                                Convert.ToInt32(dr[2].ToString()), Convert.ToDouble(dr[3].ToString()), Convert.ToInt32(dr[4].ToString()),
                                Convert.ToInt32(dr[5].ToString()), Convert.ToDouble(dr[6].ToString()), Convert.ToDouble(dr[7].ToString()),
                                Convert.ToDouble(dr[8].ToString())));                        
                    }
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                       
            //Regresa el objeto Lista
            return objFactorIntegracion;            
        }
        //*** OBTIENE EL FACTOR DE INTEGRACION SEGUN EL TIEMPO LABORADO EN LA EMPRESA ***\\
        public double calculaFactorIntegracion(int NoEmpleado, List<Empleado> objEmpleado)
        {
            List<ListFactorIntegracion> objFactorIntegracion = new List<ListFactorIntegracion>();
            DateTime Fecha=new DateTime(01/01/1900);
            double FactorIntegracion = 0;
            foreach (Empleado empleado in objEmpleado)
                if (empleado.Id == NoEmpleado)
                    Fecha = empleado.FechaIngreso;
            int Vejez = fnEmpleado.calculaVejez(Fecha);
            objFactorIntegracion = llenaFactorIntegracion(objFactorIntegracion);
            foreach (ListFactorIntegracion listFactor in objFactorIntegracion)
            {
                if (Vejez == 0)
                { FactorIntegracion = 1.0452; break; }
                if (Vejez < 5)
                    if (Vejez == listFactor.Tiempo_Fin)
                    { FactorIntegracion = listFactor.Factor; break; }                    
                if(Vejez >= 5)
                    if (Vejez >= listFactor.Tiempo_Inicio && Vejez <= listFactor.Tiempo_Fin)
                    { FactorIntegracion = listFactor.Factor; break; }                    
            }
            return FactorIntegracion;
        }
        //****CALCULA EL DESCUENTO DEL IMSS****\\
        public double calculaIMSS(double salarioSemanal, double Htriple, double otrosIngresos, double factorIntegracion)
        {
            double salDiarioIntegrado = 0, descIMSS = 0, ptjexcedente = 0, salMinimo = 59.82;
            double prestacionesDinero = 0, gastosMedicos = 0, invalidezVida = 0, censatiaVejez = 0;
            salDiarioIntegrado = ((salarioSemanal + otrosIngresos + Htriple) / 7) * factorIntegracion;
            if (salDiarioIntegrado > (salMinimo * 3))
                ptjexcedente = (salDiarioIntegrado - (salMinimo * 3)) * 0.004;
            prestacionesDinero = salDiarioIntegrado * 0.0025;
            gastosMedicos = salDiarioIntegrado * 0.00375;
            invalidezVida = salDiarioIntegrado * 0.00625;
            censatiaVejez = salDiarioIntegrado * 0.01125;
            descIMSS = (ptjexcedente + prestacionesDinero + gastosMedicos + invalidezVida + censatiaVejez) * 7;
            return descIMSS;
        }
        /****CALCULA LOS MONTO DE ISR****/
        public double calculaISRCuota(double salarioSemanal,double Hdoble, double Htriple, double otrosIngresos)
        {
            double salarioBase = 0, excedente = 0, impuestoMarginal = 0, impuesto = 0, subsidioEmpleo = 0;
            double cuotaFija = 0, Ptj = 0, limInferior = 0, limSuperior = 0;
            double subsidio = 0;
            llenaArrISR();
            salarioBase = salarioSemanal + otrosIngresos + Hdoble + Htriple;
            for (int i = 0; i < arrISR.Length; i++)
                if (salarioBase >= arrISR[0, i] && salarioBase <= arrISR[1, i])
                {
                    limInferior = arrISR[0, i];
                    limSuperior = arrISR[1, i];
                    cuotaFija = arrISR[2, i];
                    Ptj = arrISR[3, i];
                    break;//una vez encontrado se rompe la busqueda
                }
            excedente = salarioBase - limInferior;
            impuestoMarginal = excedente * Ptj;
            impuesto = impuestoMarginal + cuotaFija;
            subsidioEmpleo = traeSubsidio(salarioBase);
            subsidio = subsidioEmpleo - impuesto;
            return subsidio;
        }
        //**** BUSCA EL SUBSIDIO SEGUN EL SALARIO BASE ****\\
        public double traeSubsidio(double salarioBase)
        {
            double subsidio = 0;
            for (int i = 0; i < arrSubsidio.Length; i++)            
                if (salarioBase >= arrSubsidio[0, i] && salarioBase <= arrSubsidio[1, i])
                {
                    subsidio = arrSubsidio[2, i];
                    break;//una vez encontrado el subsidio se rompe la busqueda
                }            
            return subsidio;
        }
        /****CARGA LA LISTA EMPLEADO DESDE LA TABLA****/
        public List<Empleado> llenarListaEmpleado(List<Empleado> empleado)
        {
            string queryEmpleado = "SELECT Id, Paterno, Materno,Nombre, Puesto,Tipo, Rancho, Fechaing, SalarioDiario FROM Empleados WHERE Status = 1 ORDER BY Id";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand(queryEmpleado, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)                    
                        MessageBox.Show("No Hay Empleados Registrados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
                    else                    
                        while (dr.Read())                        
                            //guarda Id, Nombre paterno materno, puesto, tipo empleado y rancho
                            empleado.Add(new Empleado(Convert.ToInt32(dr[0].ToString()), Convert.ToString(dr[1].ToString()), 
                                Convert.ToString(dr[2].ToString()), Convert.ToString(dr[3].ToString()), Convert.ToString(dr[4].ToString()),
                                Convert.ToString(dr[5].ToString()), Convert.ToString(dr[6].ToString()),Convert.ToDateTime(dr[7].ToString()),
                                Convert.ToDouble(dr[8].ToString()))); 
                    //Agregar campo SalarioDiario   
                        //sizeEmpleados = i;                    
                    cmd.Connection.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error al cargar datos de empleados");
            }
            return empleado;
        }
        /****LLENA LA LISTA PUESTO CON LA TABLA****/
        public List<Puesto> llenarListaPuesto(List<Puesto> puesto)
        {            
            clConexion objConexion = new clConexion();
            string queryPuesto = "SELECT * FROM Puestos ORDER BY clave";
            try
            {
                using (SqlCommand cmd = new SqlCommand(queryPuesto, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)
                        MessageBox.Show("No Hay Empleados Registrados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        while (dr.Read())
                            //llena arreglo con la tabla Puestos **TODOS LOS CAMPOS**                           
                            puesto.Add(new Puesto(Convert.ToInt32(dr[0].ToString()), Convert.ToString(dr[1].ToString()), 
                                Convert.ToDouble(dr[2].ToString()), Convert.ToDouble(dr[3].ToString()), Convert.ToDouble(dr[4].ToString()), 
                                Convert.ToDouble(dr[5].ToString()), Convert.ToDouble(dr[6].ToString()), Convert.ToString(dr[7].ToString())));
                    cmd.Connection.Close();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error del sistema");
            }
            return puesto;
        }
        //***CALCULA SALARIO SEMANAL PLANTA CON CALCULO DE HORAS EXTRAS***\\
        public double calculoSalarioSemanal(int asistencias, double salarioDiario)
        {
            double bonoAsistencia = 0,  salarioSemanal = 0;
            /*if (asistencias == 6)
                bonoAsistencia = salarioDiario;         
            else if (asistencias == 7)//***SI TRABAJA EL DIA DOMINGO SEGUN LA LEY SE LE AGREGA UN SALARIO DOBLE***\\
            { bonoAsistencia = (salarioDiario * 2); asistencias = 6; }*/
            bonoAsistencia = (asistencias * 0.1666666) * salarioDiario;//Bonificacion diaria por el 7mo dia!!
            salarioSemanal = (asistencias * salarioDiario) + bonoAsistencia;// +calculoHorasExtra(horaExtra, salarioDiario);
            return salarioSemanal;
        }
        //***CALCULA OTROS INGRESOS***\\
        public double calculoOtrosIngresos(int asistencias, double otrosIngresos)
        {
            double MontoOtrosIngresos = 0;
            MontoOtrosIngresos = otrosIngresos * asistencias + (otrosIngresos * asistencias * 0.1666666);
            return MontoOtrosIngresos;
        }
        //***CALCULO DE MONTO POR HORAS EXTRA***\\
        public double calculoHorasExtra(int horaExtra, double salarioDiario, int bandera)
        {
            int horaExtraTriple = 0, horaExtraDoble = 0;
            double salarioxHora = 0, montoHorasExtra = 0;
            if (horaExtra > 9)
            { 
                horaExtraDoble = 9; 
                horaExtraTriple = horaExtra - horaExtraDoble; 
            }
            else if (horaExtra <= 9 && horaExtra > 0)
                horaExtraDoble = horaExtra;
            salarioxHora = salarioDiario / 8;
            if (bandera == 1)//**ENTRA SI SE SOLICITA EL MONTO DE HORAS DOBLES***\\
                montoHorasExtra = (salarioxHora * horaExtraDoble * 2);
            else if (bandera == 2)//**ENTRA SI SE SOLICITA EL MONTO DE HORAS TRIPLES***\\
                montoHorasExtra = (salarioxHora * horaExtraTriple * 3);
            else if (bandera == 3)//**ENTRA SI SE SOLICITA EL MONTO DE HORAS EXTRA (DOBLES Y TRIPLES)***\\
                montoHorasExtra = (salarioxHora * horaExtraDoble * 2) + (salarioxHora * horaExtraTriple * 3);
            return montoHorasExtra;
        }
        /****TRAE EL MAXIMO DE NOMINA****/
        public int maximo_Nomina()
        {
            int maximo=0;
            string query = "select max(idNomina) from Reg_NominaPlanta";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    maximo = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            maximo += 1;
            return maximo;
        }

        //***--------------------- RENDIMIENTOS JORNALES Y REGADORES-----------------------------       

        public int calculaRendimietos(int Rendimiento, string puesto)
        {
            int costRendimiento=0;            
            if (puesto == "Jornalero")
            {
                switch (Rendimiento)
                {
                    case 1:
                        costRendimiento = 107;
                        break;
                    case 2:
                        costRendimiento = 107;
                        break;
                    case 3:
                        costRendimiento = 214;
                        break;
                    default:
                        costRendimiento = 0;
                        break;
                }
            }
            else if (puesto == "Regadores")
            {
                switch (Rendimiento)
                {
                    case 1:
                        costRendimiento = 100;
                        break;
                    case 2:
                        costRendimiento = 100;
                        break;
                    case 3:
                        costRendimiento = 200;
                        break;
                    default:
                        costRendimiento = 0;
                        break;
                }
            }
            else
            {
                costRendimiento = 0;
            }

            return costRendimiento;                    
        }
        //**** OBTIENE LAS HORAS EXTRA EN DOMINGO PARA CALCULAR PRIMA DOMINICAL****\\
        public int HExtraDomingo(int NoEmpleado)
        {
            int HExtraDomingo = 0;            
            clConexion objConexion = new clConexion();
            string queryEmpleado = "SELECT * FROM Asistencia_Planta WHERE Id_Empleado = "+NoEmpleado;
            try
            {
                using (SqlCommand cmd = new SqlCommand(queryEmpleado, objConexion.conexion()))
                {//Carga los datos de la asistencia de planta segun el NoEmpleado
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)
                        MessageBox.Show("Error al cargar datos del Empleado: " + NoEmpleado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        while (dr.Read())
                        {
                            //busca el dia Domingo en la asistencia del empleado y obtiene las horas extra para sacar la prima dominical

                            DateTime dt = Convert.ToDateTime(dr[1].ToString());
                            string DiaSemana = dt.DayOfWeek.ToString();
                            if (DiaSemana == "Sunday" && Convert.ToInt32(dr[3].ToString()) != 0)//Verifica si el dia de asistencia del empleado es Domingo (Sunday)
                                HExtraDomingo = Convert.ToInt32(dr[3].ToString());//Extrae las horas extra trabajas si es Domingo     
                        }
                    }
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return HExtraDomingo;
        }
        //**** CALCULA LA PRIMA DOMINICAL****\\
        public double PrimaDomicial(int HExtraDomingo,double SueldoDiario)
        {
            double PrimaDominical = (((SueldoDiario / 8) * HExtraDomingo) * 2) * 0.25;
            return PrimaDominical;
        }
        //**** LLENA LISTA DE CUADRILLAS***\\
        public List<ListCuadrillas> CargaInfoCuadrilla(List<ListCuadrillas> objCuadrillas)
        {
            string query = "SELECT * From Cuadrillas ";
            clConexion objConexion = new clConexion();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows != false)
                        while (dr.Read())
                        {                                                       
                            int Id = int.Parse(dr[0].ToString());
                            int Cuadrilla = int.Parse(dr[1].ToString());
                            int Supervisor = int.Parse(dr[2].ToString());
                            string Rancho = dr[3].ToString();
                            string SupNombre = dr[4].ToString();
                            int SupervisorEventual = int.Parse(dr[5].ToString());
                            string SupnombreEventual = dr[6].ToString();
                            string RanchoEventual = dr[7].ToString();
                            objCuadrillas.Add(new ListCuadrillas(Id, Cuadrilla, Supervisor, Rancho, SupNombre, SupervisorEventual, SupnombreEventual,RanchoEventual));
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return objCuadrillas;
        }
        //carga la lista actividades desde la DB
        public List<ListActividades> cargaListaActividades(List<ListActividades> objActividades)
        {
            string query = "SELECT * FROM Actividad ORDER BY Clave";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)
                        MessageBox.Show("No Hay Actividades Registradas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        while (dr.Read())
                            objActividades.Add(new ListActividades(int.Parse(dr[0].ToString()), dr[1].ToString()));
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return objActividades;
        }
        //carga la lista Lotes de la DB
        public List<ListLotes> cargaListalotes(List<ListLotes> objLotes, string RanchoValue)
        {
            string query = "SELECT * FROM Lotes WHERE Rancho ='" + RanchoValue + "'";
            clConexion objConexion = new clConexion();
            int index = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)                        
                        while (dr.Read())
                            objLotes.Add(new ListLotes(index++, dr[0].ToString(), dr[1].ToString(),
                                dr[2].ToString(), Convert.ToDouble(dr[3].ToString()),
                                dr[4].ToString(), Convert.ToDouble(dr[5].ToString())));
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LOTES");
            }
            return objLotes;
        }



        //Crea tabla previa a calculo de costos
        public List<ListCostoManoObra> GeneraTablaCosto(List<ListCostoManoObra> objCostoManoObra, string queryPlanta)
        {           
            
            List<Puesto> objPuesto = new List<Puesto>();
            List<Empleado> objEmpleado = new List<Empleado>();
            objPuesto = llenarListaPuesto(objPuesto);
            objEmpleado = llenarListaEmpleado(objEmpleado);
            clConexion objConexion = new clConexion();
            try
            {
                //Listar Empleados de Planta en la Nomina\\ 
                using (SqlCommand cmd = new SqlCommand(queryPlanta, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)
                        MessageBox.Show("No hay registros de Asistencia", "PLANTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        while (dr.Read())
                        {
                            int Id_Empleado = int.Parse(dr[0].ToString());
                            DateTime Fecha = Convert.ToDateTime(dr[1].ToString());
                            int HorasExtra = int.Parse(dr[2].ToString());
                            int Asistencias = int.Parse(dr[3].ToString());
                            string Actividad = dr[4].ToString();
                            string Lote = dr[5].ToString();
                            string Rancho = dr[6].ToString();
                            string Puesto = "";
                            double SalarioDiario = 0, Otros = 0;
                            foreach (Empleado empleado in objEmpleado)//busca puesto de empleado
                                if (empleado.Id == Id_Empleado)
                                { Puesto = empleado.Puesto; break; }
                            foreach (Puesto puesto in objPuesto)//busca salario y otros ingresos del empleado segun su puesto
                                if (puesto.NomPuesto == Puesto)
                                {
                                    SalarioDiario = puesto.Sueldo;
                                    Otros = puesto.Otros;
                                    break;
                                }
                            objCostoManoObra.Add(new ListCostoManoObra(Id_Empleado, SalarioDiario, Otros, Rancho, Lote, Actividad, HorasExtra, Fecha));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return objCostoManoObra;
        }

        /////////////////////////////////////////////////////
        ////////////////////////////////////////////////////

        public static List<Consultas.EmpleadoCosto> fnEmpleadoCosto(List<Consultas.EmpleadoCosto> objEmpleadoCosto, string query)
        {        
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)
                        MessageBox.Show("No hay registros de Asistencia", "PLANTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else                                                                
                        while (dr.Read())                        
                            objEmpleadoCosto.Add(new Consultas.EmpleadoCosto(int.Parse(dr[0].ToString()), 0));                                           
                }
            }
            catch (Exception ex)
            { }
            return objEmpleadoCosto;                
        }


        //carga datos de Ranchos en lista
        public List<ListRanchos> cargaRanchos(List<ListRanchos> objRanchos)
        {
            string queryPlanta = "SELECT * FROM R"; 
            clConexion objConexion = new clConexion();
            try
            {
                //Listar Empleados de Planta en la Nomina\\ 
                using (SqlCommand cmd = new SqlCommand(queryPlanta, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)
                        MessageBox.Show("No hay registros de Ranchos", "Ranchos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        while (dr.Read())
                            objRanchos.Add(new ListRanchos(dr[0].ToString(), dr[1].ToString(), 
                                dr[2].ToString(), Convert.ToDouble(dr[3].ToString())));                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return objRanchos;
        }
        //*************************************************************
        //*************************************************************
        //*************************************************************
        //CARGA LISTA DE SEARCHFUNCTION PARA LA BUSQUEDA DE PALABRASSSS
        //*************************************************************
        //*************************************************************        
        //*************************************************************
        public List<SearchFunction> LoadEmp (List<SearchFunction> objSearchFunction)
        {
            clConexion objConexion = new clConexion();
            string query = "SELECT Id, Paterno, Materno, Nombre FROM Empleados";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                        while (dr.Read())
                            objSearchFunction.Add(new SearchFunction(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString()));
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return objSearchFunction;
        }
        //DIRECTORIO TELEFONICO
        public static List<DirTelefonico> Fill_DirTelefonico_Class(List<DirTelefonico> objDirTelefonico)
        {
            clConexion objConexion = new clConexion();
            string queryEmpleado = "SELECT * FROM DirectorioTelefonico ORDER BY Nombre ASC";
            try
            {
                using (SqlCommand cmd = new SqlCommand(queryEmpleado, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)
                        MessageBox.Show("No hay contactos telefonicos almacenados", "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else                    
                        while (dr.Read())                        
                            objDirTelefonico.Add(new DirTelefonico(int.Parse(dr[0].ToString()), dr[1].ToString(), dr[2].ToString(),
                                dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString()));                                             
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            { }
            return objDirTelefonico;
        }
        public static int AddContactoTelefonico(int Id, string nombre, string numero, string direccion, string ciudad, string estado, string email, string web)
        {
            string query = "INSERT INTO DirectorioTelefonico (Id,Nombre,Numero,Direccion,Ciudad,Estado,Email,Web) VALUES (@Id,@Nombre,@Numero,@Direccion,@Ciudad,@Estado,@Email,@Web)";
            clConexion objConexion = new clConexion();
            int ban = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Numero", numero);
                    cmd.Parameters.AddWithValue("@Direccion", direccion);
                    cmd.Parameters.AddWithValue("@Ciudad", ciudad);
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Web", web);
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            { ban = 1; }

            return ban;
        }
        public static int traer_Maximo(String query)
        {
            int maximo = 1;
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    maximo = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return maximo += 1;
        }
        public static int UpdateContactoTelefonico(int Id, string nombre, string numero, string direccion, string ciudad, string estado, string email, string web)
        {
            int ban = 0;
            string update = "UPDATE DirectorioTelefonico SET Nombre = @Nombre, Numero = @Numero, Direccion = @Direccion, Ciudad = @Ciudad, Estado = @Estado, Email = @Email, Web = @Web " + "WHERE Id = " + Id + "";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand updatecmd = new SqlCommand(update, objConexion.conexion()))
                {                    
                    updatecmd.Parameters.AddWithValue("@Nombre", nombre);
                    updatecmd.Parameters.AddWithValue("@Numero", numero);
                    updatecmd.Parameters.AddWithValue("@Direccion", direccion);
                    updatecmd.Parameters.AddWithValue("@Ciudad", ciudad);
                    updatecmd.Parameters.AddWithValue("@Estado", estado);
                    updatecmd.Parameters.AddWithValue("@Email", email);
                    updatecmd.Parameters.AddWithValue("@Web", web);                    
                    updatecmd.Connection = objConexion.conexion();
                    updatecmd.Connection.Open();
                    updatecmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }              
            return ban;
        }
        public static int DeleteContactoTelefonico(int Id)
        {
            int ban = 0;
            string query = "DELETE FROM DirectorioTelefonico WHERE Id = " + Id;
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();                    
                }
            }
            catch (Exception ex)
            { ban = 1; }
            return ban;
        }
        public static int DeleteSesion(string username)
        {
            int ban = 0;
            string query = "DELETE FROM Sesion WHERE Username = '" + username + "'";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    AddCloseSesion_to_HistorialSesion(LogUsers._username, LogUsers._nivel, DateTime.Now, LogUsers._computername, "Salida");        
                }
            }
            catch (Exception ex)
            { ban = 1; MessageBox.Show(ex.Message); }
            return ban;
        }

        public static int AddCloseSesion_to_HistorialSesion(string username, int nivel, DateTime horafecha, string pc, string observaciones)
        {
            string query = "INSERT INTO HistorialSesion (Username,Nivel,HoraFecha,PC,Observaciones) VALUES (@Username,@Nivel,@HoraFecha,@PC,@Observaciones)";
            clConexion objConexion = new clConexion();
            int ban = 0;
            try
            {                
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Nivel", nivel);
                    cmd.Parameters.AddWithValue("@HoraFecha", horafecha);
                    cmd.Parameters.AddWithValue("@PC", pc);
                    cmd.Parameters.AddWithValue("@Observaciones", observaciones);
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            { ban = 1; }

            return ban;
        }

        //FUNCION PARA DETERMINAR LA VERSION DEL SISTEMA OPERATIVO
        public static string GetOS()
        {
            string name = "Desconocido";
            // Obtener la información de OperatingSystem del espacio de nombres system.
            System.OperatingSystem osInfo = System.Environment.OSVersion;            
            switch (osInfo.Platform)
            {
                case System.PlatformID.Win32Windows:
                    switch (osInfo.Version.Minor)
                    {
                        case 0:
                            name = "Windows 95";
                            break;

                        case 10:
                            name = "Windows 98";
                            break;

                        case 90:
                            name = "Windows ME";
                            break;
                    }
                    break;

                case System.PlatformID.Win32NT:
                    switch (osInfo.Version.Major)
                    {
                        case 3:
                            name = "Windws NT 3.51";
                            break;

                        case 4:
                            name = "Windows NT 4";
                            break;

                        case 5:
                            switch (osInfo.Version.Minor)
                            {
                                case 0:
                                    name = "Windows 2000";
                                    break;
                                case 1:
                                    name = "Windows XP";
                                    break;
                                case 2:
                                    name = "Windows Server 2003";
                                    break;
                            }
                            break;

                        case 6:
                            switch (osInfo.Version.Minor)
                            {
                                case 0:
                                    name = "Windows Vista";
                                    break;
                                case 1:
                                    name = "Windows 7";
                                    break;

                            }
                            break;
                    }
                    break;
            }
            return name;
        }

        //FUNCION PARA LLENAR OBJSALIDAS DE COSTO 

        public List<Consultas.SalidaCosto> fillSalidas(List<Consultas.SalidaCosto> objSalida, string producto)
        {
            string query =
                "SELECT Producto, UM, Rancho, Importe FROM salidas WHERE Producto = '" + producto + "'";
            clConexion objConexion = new clConexion();
            try
            {
                //Listar Empleados de Planta en la Nomina\\ 
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                    {
                        while (dr.Read())                            
                            objSalida.Add(new Consultas.SalidaCosto(dr[0].ToString(), dr[1].ToString(),
                                dr[2].ToString(), Convert.ToDouble(dr[3].ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return objSalida;
        }

        public List<Consultas.SalidaCosto> fillSalidas(List<Consultas.SalidaCosto> objSalida, string rancho, string lote)
        {
            string query =
                "SELECT Producto, UM, Rancho, Importe FROM salidas WHERE Rancho = '" + rancho + "' AND Lote = '" + lote + "'";
            clConexion objConexion = new clConexion();
            try
            {
                //Listar Empleados de Planta en la Nomina\\ 
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                    {
                        while (dr.Read())
                            objSalida.Add(new Consultas.SalidaCosto(dr[0].ToString(), dr[1].ToString(),
                                dr[2].ToString(), Convert.ToDouble(dr[3].ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return objSalida;
        }
    }
}
