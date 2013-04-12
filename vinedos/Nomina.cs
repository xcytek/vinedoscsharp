using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.IO;

namespace vinedos
{
    public partial class Nomina : Form
    {   
        Funciones funciones = new Funciones();

        public Nomina()
        {
            InitializeComponent();
        }         

        //***Variables para Mostrar en Nomina***\\
        int TotalEmpleados = 0;
        double TotalSemanal = 0;
        double TotalOtros = 0;
        double TotalHorasExtra = 0;
        double TotalPrima = 0;
        double TotalSubsidio = 0;
        double TotalImss = 0;
        double TotalIsr = 0;
        double TotalAjuste = 0;
        double TotalNeto = 0;
        
        //*** ALMACENA REGISTRO POR REGISTRO EN LA NOMINA ***\\
        public void insertNomina(int IdNomina, string Rancho, int NoEmpleado, string Nombre, string Puesto, 
            double SalarioDiario, double SalSemanal, int FaltasDias, int FaltasHoras, double HEDobles,
            double HETriples, double Aguinaldos, double DiasVacaciones, double PrimaDominical, double DiaFestivos,
            DateTime FechaBaja, double CD, double Otros, double Subsidio, double IMSS, double ISR, double Ajuste, double PercepcionNeta)
        {
            try
            {
                clConexion objConexion = new clConexion();
                string query = "INSERT INTO Nomina (IdNomina,Rancho,NoEmpleado,Nombre,Puesto,SalarioDiario,SalarioSemanal," +
                    "FaltasDias,FaltasHoras,HEDobles,HETriples, Aguinaldos,DiasVacaciones,PDominical,FestivosTrabajados," +
                    "FechaBaja,CD,Otros,Subsidios,IMSS,ISR,Ajuste,PercepcionNeta) VALUES (@IdNomina,@Rancho,@NoEmpleado,@Nombre," +
                    "@Puesto,@SalarioDiario,@SalSemanal,@FaltasDias,@FaltasHoras,@HEDobles,@HETriples,@Aguinaldos,@DiasVacaciones," +
                    "@PrimaDominical,@DiaFestivos,@FechaBaja,@CD,@Otros,@Subsidio,@IMSS,@ISR,@Ajuste,@PercepcionNeta)";
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@IdNomina", IdNomina);
                    cmd.Parameters.AddWithValue("@Rancho", Rancho);
                    cmd.Parameters.AddWithValue("@NoEmpleado", NoEmpleado);
                    cmd.Parameters.AddWithValue("@Nombre", Nombre);
                    cmd.Parameters.AddWithValue("@Puesto", Puesto);
                    cmd.Parameters.AddWithValue("@SalarioDiario", SalarioDiario);
                    cmd.Parameters.AddWithValue("@SalSemanal", SalSemanal);
                    cmd.Parameters.AddWithValue("@FaltasDias", FaltasDias);
                    cmd.Parameters.AddWithValue("@FaltasHoras", FaltasHoras);
                    cmd.Parameters.AddWithValue("@HEDobles", HEDobles);
                    cmd.Parameters.AddWithValue("@HETriples", HETriples);
                    cmd.Parameters.AddWithValue("@Aguinaldos", Aguinaldos);
                    cmd.Parameters.AddWithValue("@DiasVacaciones", DiasVacaciones);
                    cmd.Parameters.AddWithValue("@PrimaDominical", PrimaDominical);
                    cmd.Parameters.AddWithValue("@DiaFestivos", DiaFestivos);
                    cmd.Parameters.AddWithValue("@FechaBaja", FechaBaja);
                    cmd.Parameters.AddWithValue("@CD", CD);
                    cmd.Parameters.AddWithValue("@Otros", Otros);
                    cmd.Parameters.AddWithValue("@Subsidio", Subsidio);
                    cmd.Parameters.AddWithValue("@IMSS", IMSS);
                    cmd.Parameters.AddWithValue("@ISR", ISR);
                    cmd.Parameters.AddWithValue("@Ajuste", Ajuste);
                    cmd.Parameters.AddWithValue("@PercepcionNeta", PercepcionNeta);
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error con registro de nomina", "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void muestraEtiquetas()
        {
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            textBox6.Visible = true;
            textBox7.Visible = true;            
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            textBox1.Text = "" + TotalEmpleados;
            textBox2.Text = TotalSemanal.ToString("###,###.##");
            double Otros = TotalOtros + TotalPrima;
            textBox3.Text = Otros.ToString("###,###.##");
            textBox4.Text = TotalSubsidio.ToString("###,###.##");
            textBox5.Text = TotalImss.ToString("###,###.##");
            textBox6.Text = TotalHorasExtra.ToString("###,###.##");
            textBox8.Text = TotalAjuste.ToString("###,###.##");
            textBox7.Text = TotalNeto.ToString("###,###.##");
        }

        public void wait()
        {
            Wait frmWait = new Wait();
            frmWait.ShowDialog();            
        }

        public int numeroSemana()
        {
            int Año = Convert.ToInt32(DateTime.Today.Year);
            int Mes = Convert.ToInt32(DateTime.Today.Month);
            int Dia = Convert.ToInt32(DateTime.Today.Day);
            DateTime date = new DateTime(Año, Mes, Dia);
            System.Globalization.CultureInfo norwCultura = System.Globalization.CultureInfo.CreateSpecificCulture("es");
            System.Globalization.Calendar cal = norwCultura.Calendar;
            int numSemana = cal.GetWeekOfYear(date, norwCultura.DateTimeFormat.CalendarWeekRule, norwCultura.DateTimeFormat.FirstDayOfWeek);
            return numSemana;
        }

        public void crea_Nomina(int IdNom)
        {
            string query = "INSERT INTO Reg_Nomina (IdNomina,Fecha1,Fecha2,NumEmpleados,SalOrdinario,Otros,Subsidios,Imss,Isr,Monto) values (@IdNomina,@Fecha1,@Fecha2,@numEmpleados,@SalOrdinario,@Otros,@Subsidios,@Imss,@Isr,@Monto)";            
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion())) 
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@IdNomina", IdNom);
                    cmd.Parameters.AddWithValue("@Fecha1",DateTime.Now);
                    cmd.Parameters.AddWithValue("@Fecha2", DateTime.Now);
                    cmd.Parameters.AddWithValue("@NumEmpleados", Convert.ToInt32(textBox1.Text));
                    cmd.Parameters.AddWithValue("@SalOrdinario", Convert.ToDouble(textBox2.Text));
                    cmd.Parameters.AddWithValue("@Otros", Convert.ToDouble(textBox3.Text) + Convert.ToDouble(textBox6.Text) + Convert.ToDouble(textBox8.Text));
                    cmd.Parameters.AddWithValue("@Subsidios", 0);
                    cmd.Parameters.AddWithValue("@Imss", 0);
                    cmd.Parameters.AddWithValue("@Isr", 0);
                    cmd.Parameters.AddWithValue("@Monto", Convert.ToDouble(textBox7.Text));
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Nomina Guardada Correctamente");
                    cmd.Connection.Close();                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+" Error al guardar la nomina");
            }
        }
        //*** VACIA TABLAS DE ASISTENCIA PLANTA, EVENTUAL, OD ***\\
        public void vaciaAsistencias()
        {
            string query = "";            
            clConexion objConexion = new clConexion();
            try
            {
                for (int i = 0; i <6; i++)
                {
                    if (i == 0)
                        query = "TRUNCATE TABLE Asistencia_Planta";
                    else if (i == 1)
                        query = "TRUNCATE TABLE Asistencia_Eventual";
                    else if (i == 2)
                        query = "TRUNCATE TABLE Asistencia_OD";
                    else if (i == 3)
                        query = "TRUNCATE TABLE Cobro_Externos";
                    using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                    {
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = objConexion.conexion();
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+" Error al vaciar asistencias");
            }
        }
        // *** RESPALDA TABLAS ASISTENCIA UNA VEZ CALCULADA LA NOMINA  *** \\
        public void respTabla()
        {
            //Respalda empleados de PLANTA, EVENTUALES            
            string query="";
            int bandera = 0;
            clConexion objConexion = new clConexion();
            try
            {                
                for (int i = 0; i < 6; i++)//cambiar i=0
                {
                    if (i == 0) //cambiar i=0 para que lo respalde!
                        query = "INSERT INTO Asistencia_Planta_Resp (Id_Empleado, Fecha, Asist, HE, Asistencias, Faltas, Actividad, Lote1, Lote2, Rancho) SELECT Id_Empleado, fecha, Asist, HE, Asistencias, Faltas, Actividad, Lote1, Lote2, Rancho FROM Asistencia_Planta";
                    else if (i == 1)
                        query = "INSERT INTO Asistencia_Eventual_Resp (Id_Empleado, Fecha, Asist, Rendimiento, Asistencia, faltas, Actividad, Lote1) SELECT Id_Empleado, fecha, Asist, Rendimiento, Asistencia, faltas, Actividad, Lote1 FROM Asistencia_Eventual";
                    else if (i == 2)
                        query = "INSERT INTO Asistencia_OD_Resp (Id_Empleado, Fecha, Plantas, Precio, Asistencias, faltas, Actividad, Rancho, Lote1) SELECT Id_Empleado, Fecha, Plantas, Precio, Asistencias, faltas, Actividad, Rancho, Lote1 FROM Asistencia_OD";
                    else if (i == 3)
                        query = "INSERT INTO Cobro_Externos_Resp (Id_Empleado, Fecha, Descripcion, Importe, Rancho) SELECT Id_Empleado, Fecha, Descripcion, Importe, Rancho FROM Cobro_Externos";
                    using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                    {
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = objConexion.conexion();
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }
                bandera = 1;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error al respaldar asistencias", "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (bandera == 1)
                vaciaAsistencias();
                MessageBox.Show("El respaldo se ha realizado con Exito!","Sistema de Viñedos",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        //*** RESPALDA LA NOMINA EN LA DB***\\
        public void respaldaNomina(int idNom)
        {
            int n = 0;
            int IdNomina = idNom, NoEmpleado, FaltasDias, FaltasHoras;
            string Nombre, Puesto, Rancho;
            double SalDiario, SalSemanal, HEDobles, HETriples, Aguinaldos, DiasVacaciones,
                PDominical, Festivos,CD, Otros, Subsidio, IMSS, ISR, Ajustes, Sueldo;
            DateTime FechaBaja = new DateTime(1900, 1, 1);
            foreach (DataGridViewRow row in gridNomina.Rows)
            {
                Rancho = gridNomina.Rows[n].Cells[0].Value.ToString();
                NoEmpleado = Convert.ToInt32(gridNomina.Rows[n].Cells[1].Value);
                Nombre = Convert.ToString(gridNomina.Rows[n].Cells[2].Value);
                Puesto = Convert.ToString(gridNomina.Rows[n].Cells[3].Value);                
                if (Convert.ToString(gridNomina.Rows[n].Cells[4].Value) == "")//***VALIDA SI LA CELDA SE ENCUENTRA VACIA, LE ASIGNA UN 0 POR DEFAULT***
                    SalDiario = 0;
                else
                    SalDiario = Convert.ToDouble(gridNomina.Rows[n].Cells[4].Value);//**DE LO CONTRARIO ASIGNA EL VALOR A LA VARIABLE PARA ALMACENAR
                if (Convert.ToString(gridNomina.Rows[n].Cells[5].Value) == "")//SEMANAL
                    SalSemanal = 0;
                else
                    SalSemanal = Convert.ToDouble(gridNomina.Rows[n].Cells[5].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[6].Value) == "")//FALTAS
                    FaltasDias = 0;
                else
                    FaltasDias = Convert.ToInt32(gridNomina.Rows[n].Cells[6].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[7].Value) == "")//FALTAS HRS
                    FaltasHoras = 0;
                else
                    FaltasHoras = Convert.ToInt32(gridNomina.Rows[n].Cells[7].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[8].Value) == "")//HE DOBLES
                    HEDobles = 0;
                else
                    HEDobles = Convert.ToDouble(gridNomina.Rows[n].Cells[8].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[9].Value) == "")//HE TRIPLES
                    HETriples = 0;
                else
                    HETriples = Convert.ToDouble(gridNomina.Rows[n].Cells[9].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[10].Value) == "")//AGUINALDOS
                    Aguinaldos = 0;
                else
                    Aguinaldos = Convert.ToDouble(gridNomina.Rows[n].Cells[10].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[11].Value) == "")//VACACIONES
                    DiasVacaciones = 0;
                else
                    DiasVacaciones = Convert.ToDouble(gridNomina.Rows[n].Cells[11].Value);//PRIMA DOMINICAL
                if (Convert.ToString(gridNomina.Rows[n].Cells[12].Value) == "")
                    PDominical = 0;
                else
                    PDominical = Convert.ToDouble(gridNomina.Rows[n].Cells[12].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[13].Value) == "")//FESTIVOS TRABAJADOS
                    Festivos = 0;
                else
                    Festivos = Convert.ToDouble(gridNomina.Rows[n].Cells[13].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[14].Value) != "")//FECHA BAJA
                    FechaBaja = Convert.ToDateTime(gridNomina.Rows[n].Cells[14].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[15].Value) == "")//CD
                    CD = 0;
                else
                    CD = Convert.ToDouble(gridNomina.Rows[n].Cells[15].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[16].Value) == "")//OTROS
                    Otros = 0;
                else
                    Otros = Convert.ToDouble(gridNomina.Rows[n].Cells[16].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[17].Value) == "")//SUBSIDIO
                    Subsidio = 0;
                else
                    Subsidio = Convert.ToDouble(gridNomina.Rows[n].Cells[17].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[18].Value) == "")//IMSS
                    IMSS = 0;
                else
                    IMSS = Convert.ToDouble(gridNomina.Rows[n].Cells[18].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[19].Value) == "")//ISR
                    ISR = 0;
                else
                    ISR = Convert.ToDouble(gridNomina.Rows[n].Cells[19].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[20].Value) == "")//AJUSTES
                    Ajustes = 0;
                else
                    Ajustes = Convert.ToDouble(gridNomina.Rows[n].Cells[20].Value);
                if (Convert.ToString(gridNomina.Rows[n].Cells[21].Value) == "")//PERCEPCION NETA
                    Sueldo = 0;
                else
                    Sueldo = Convert.ToDouble(gridNomina.Rows[n].Cells[21].Value);
                insertNomina(IdNomina, Rancho, NoEmpleado, Nombre, Puesto, SalDiario, SalSemanal, FaltasDias, FaltasHoras, HEDobles, HETriples,
                    Aguinaldos, DiasVacaciones, PDominical, Festivos, FechaBaja, CD, Otros, Subsidio, IMSS, ISR, Ajustes, Sueldo);
                n += 1;
            }
            crea_Nomina(idNom);
        }        
        //*******************************************************************************************************
        //*******************************************************************************************************
        //*******************************************************************************************************
        //*************FUNCIONES PARA EL CALCULO DE NOMINA PARA PODA, COSECHA, RENDIMIENTO Y JUBILADOS***********
        //*******************************************************************************************************
        //*******************************************************************************************************
        //*******************************************************************************************************
        public void NominaPoda()
        {
            clConexion objConexion = new clConexion();
            List<Empleado> objEmpleados = new List<Empleado>();
            List<Puesto> Puesto = new List<Puesto>();
            string QueryPoda = "SELECT Id_Empleado, SUM(Plantas * Precio) FROM Asistencia_OD WHERE Observacion = 'Poda' GROUP BY Id_Empleado";
            using (SqlCommand cmd = new SqlCommand(QueryPoda, objConexion.conexion()))
            {
                SqlDataReader dr;
                cmd.Connection = objConexion.conexion();
                cmd.Connection.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows == true)                    
                {
                    string nombre = "";
                    string puesto = "";
                    string rancho = "";
                    int NoEmpleado = 0;
                    double percepcionNeta = 0, SalSemanal = 0;                    
                    objEmpleados = funciones.llenarListaEmpleado(objEmpleados);
                    Puesto = funciones.llenarListaPuesto(Puesto);
                    while (dr.Read())
                    {
                        int n = gridNomina.Rows.Add();
                        TotalEmpleados += 1;
                        NoEmpleado = int.Parse(dr[0].ToString());
                        foreach (Empleado empleados in objEmpleados)//Compara Id de empleados y obtiene el nombre y puesto\\
                            if (NoEmpleado == empleados.Id)
                            {
                                nombre = empleados.Paterno + " " + empleados.Materno + " " + empleados.Nombre;
                                puesto = empleados.Puesto;
                                rancho = empleados.Rancho;
                                break;
                            }
                        percepcionNeta = Convert.ToDouble(dr[1].ToString());
                        SalSemanal = Convert.ToDouble(dr[1].ToString());
                        TotalSemanal += SalSemanal;
                        TotalNeto += percepcionNeta;                        
                        //****************************************************\\
                        //***********MUESTRA EN EL GRID************************\\
                        gridNomina.Rows[n].Cells[0].Value = rancho;
                        gridNomina.Rows[n].Cells[1].Value = NoEmpleado;
                        gridNomina.Rows[n].Cells[2].Value = nombre;
                        gridNomina.Rows[n].Cells[3].Value = puesto;
                        gridNomina.Rows[n].Cells[4].Value = "";
                        gridNomina.Rows[n].Cells[5].Value = SalSemanal;
                        gridNomina.Rows[n].Cells[6].Value = "";
                        gridNomina.Rows[n].Cells[7].Value = "";
                        gridNomina.Rows[n].Cells[8].Value = "";
                        gridNomina.Rows[n].Cells[9].Value = "";
                        gridNomina.Rows[n].Cells[10].Value = "";
                        gridNomina.Rows[n].Cells[11].Value = "";
                        gridNomina.Rows[n].Cells[12].Value = "";
                        gridNomina.Rows[n].Cells[13].Value = "";
                        gridNomina.Rows[n].Cells[14].Value = "";
                        gridNomina.Rows[n].Cells[15].Value = "";
                        gridNomina.Rows[n].Cells[16].Value = "";
                        gridNomina.Rows[n].Cells[17].Value = "";
                        gridNomina.Rows[n].Cells[18].Value = "";
                        gridNomina.Rows[n].Cells[19].Value = "";
                        gridNomina.Rows[n].Cells[20].Value = "";
                        gridNomina.Rows[n].Cells[21].Value = percepcionNeta;

                    }
                }
            }
        }
        //CALCULA LA NOMINA EN TEMPORADA DE COSECHA!!!
        public void NominaCosecha()
        {
            clConexion objConexion = new clConexion();
            List<Empleado> objEmpleados = new List<Empleado>();
            List<Puesto> Puesto = new List<Puesto>();
            string QueryPoda = "SELECT Id_Empleado, SUM(Plantas * Precio) FROM Asistencia_OD WHERE Observacion = 'Cosecha' GROUP BY Id_Empleado";
            using (SqlCommand cmd = new SqlCommand(QueryPoda, objConexion.conexion()))
            {
                SqlDataReader dr;
                cmd.Connection = objConexion.conexion();
                cmd.Connection.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    string nombre = "";
                    string puesto = "";
                    string rancho = "";
                    int NoEmpleado = 0;
                    double percepcionNeta = 0, SalSemanal = 0;
                    objEmpleados = funciones.llenarListaEmpleado(objEmpleados);
                    Puesto = funciones.llenarListaPuesto(Puesto);
                    while (dr.Read())
                    {
                        int n = gridNomina.Rows.Add();
                        TotalEmpleados += 1;
                        NoEmpleado = int.Parse(dr[0].ToString());
                        foreach (Empleado empleados in objEmpleados)//Compara Id de empleados y obtiene el nombre y puesto\\
                            if (NoEmpleado == empleados.Id)
                            {
                                nombre = empleados.Paterno + " " + empleados.Materno + " " + empleados.Nombre;
                                puesto = empleados.Puesto;
                                rancho = empleados.Rancho;
                                break;
                            }
                        percepcionNeta = Convert.ToDouble(dr[1].ToString());
                        SalSemanal = Convert.ToDouble(dr[1].ToString());
                        TotalSemanal += SalSemanal;
                        TotalNeto += percepcionNeta;
                        //****************************************************\\
                        //***********MUESTRA EN EL GRID************************\\
                        gridNomina.Rows[n].Cells[0].Value = rancho;
                        gridNomina.Rows[n].Cells[1].Value = NoEmpleado;
                        gridNomina.Rows[n].Cells[2].Value = nombre;
                        gridNomina.Rows[n].Cells[3].Value = puesto;
                        gridNomina.Rows[n].Cells[4].Value = "";
                        gridNomina.Rows[n].Cells[5].Value = SalSemanal;
                        gridNomina.Rows[n].Cells[6].Value = "";
                        gridNomina.Rows[n].Cells[7].Value = "";
                        gridNomina.Rows[n].Cells[8].Value = "";
                        gridNomina.Rows[n].Cells[9].Value = "";
                        gridNomina.Rows[n].Cells[10].Value = "";
                        gridNomina.Rows[n].Cells[11].Value = "";
                        gridNomina.Rows[n].Cells[12].Value = "";
                        gridNomina.Rows[n].Cells[13].Value = "";
                        gridNomina.Rows[n].Cells[14].Value = "";
                        gridNomina.Rows[n].Cells[15].Value = "";
                        gridNomina.Rows[n].Cells[16].Value = "";
                        gridNomina.Rows[n].Cells[17].Value = "";
                        gridNomina.Rows[n].Cells[18].Value = "";
                        gridNomina.Rows[n].Cells[19].Value = "";
                        gridNomina.Rows[n].Cells[20].Value = "";
                        gridNomina.Rows[n].Cells[21].Value = percepcionNeta;

                    }
                }
            }
        }

        public void NominaRendimiento()
        {

            clConexion objConexion = new clConexion();
            List<Empleado> objEmpleados = new List<Empleado>();
            List<Puesto> Puesto = new List<Puesto>();
            string QueryPoda = "SELECT Id_Empleado, SUM(Asistencia) FROM Asistencia_Eventual GROUP BY Id_Empleado";
            using (SqlCommand cmd = new SqlCommand(QueryPoda, objConexion.conexion()))
            {
                SqlDataReader dr;
                cmd.Connection = objConexion.conexion();
                cmd.Connection.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    string nombre = "";
                    string puesto = "";
                    string rancho = "";
                    int NoEmpleado = 0;
                    int asistencias = 0;
                    double salarioSemanal = 0, otrosIngresos = 0, descIMSS = 0, subsidio = 0;                    
                    double percepcionNeta = 0;
                    double Hdoble = 0, Htriple = 0;
                    int cont = 0, horasExtra = 0, rendimientos = 0;
                    objEmpleados = funciones.llenarListaEmpleado(objEmpleados);                    
                    Puesto = funciones.llenarListaPuesto(Puesto);
                    while (dr.Read())
                    {                        
                        Hdoble = 0;//Inicializamos las Variables de Horas Extra para noafectar a quienes no aplica (Regadores, Jornales ETC)
                        Htriple = 0;
                        //Llenamos el Grid con la informacion requerida para la nomina\\
                        int n = gridNomina.Rows.Add();
                        TotalEmpleados += 1;
                        NoEmpleado = Convert.ToInt32(dr[0].ToString());
                        asistencias = Convert.ToInt32(dr[1].ToString());                       
                        foreach (Empleado empleados in objEmpleados)//Compara Id de empleados y obtiene el nombre y puesto\\
                            if (NoEmpleado == empleados.Id)
                            {
                                nombre = empleados.Paterno + " " + empleados.Materno + " " + empleados.Nombre;
                                puesto = empleados.Puesto;
                                rancho = empleados.Rancho;
                                break;                                
                            }
                        if (puesto == "Jornalero" || puesto == "Regadores")
                            rendimientos = funciones.calculaRendimietos(0,puesto);//***1=Asistencia | 2=Rendimiento | 3=Asistencia+Rendimiento***\\                                
                        for (int j = 0; j < Puesto.Count; j++)
                            if (puesto == Puesto[j].NomPuesto)//compara puestos y obtiene el indice dentro de la lista Puesto\\ 
                            {                       
                                cont = j;
                                break;
                            }
                        //MessageBox.Show("Hora: "+horasExtra+" Asist: " + asistencias+" Sal Diario: "+ Puesto[cont].Sueldo);
                        salarioSemanal = Math.Round(funciones.calculoSalarioSemanal(asistencias, Puesto[cont].Sueldo), 2);//asistencias * Puesto[cont].Sueldo + Puesto[cont].BonoAsist + Puesto[cont].BonoRend;                                                                                             
                        otrosIngresos = Math.Round(funciones.calculoOtrosIngresos(asistencias, Puesto[cont].Otros) + (2.333333 * asistencias), 2);
                        Hdoble = Math.Round(funciones.calculoHorasExtra(horasExtra, Puesto[cont].Sueldo, 1), 2);
                        Htriple = Math.Round(funciones.calculoHorasExtra(horasExtra, Puesto[cont].Sueldo, 2), 2);
                        double PrimaDominical = funciones.PrimaDomicial(funciones.HExtraDomingo(NoEmpleado), Puesto[cont].Sueldo);//Calcula la prima Dominical
                        PrimaDominical = Math.Round(PrimaDominical, 2);//Redondea la Prima Dominical para la suma                                                                      
                        percepcionNeta = Math.Round(salarioSemanal + Hdoble + Htriple + PrimaDominical + otrosIngresos + subsidio - descIMSS, 2);
                        TotalSemanal += salarioSemanal;//*** ACUMULA SALARIO SEMANAL ***\\                               
                        TotalOtros += otrosIngresos;//*** ACUMULA OTROS INGRESOS ***\\                                                        
                        double ajuste = 0;
                        if (puesto == "Regadores" && (percepcionNeta > 1000 && percepcionNeta < 1005))//ajuste para los REGADORES QUE SE PASEN POR $3 MXN O $4 MXN DE SU SUELDO
                            ajuste = percepcionNeta - 1000;
                        else
                            ajuste = Math.Round(Math.Round(percepcionNeta, 0) - percepcionNeta, 2);
                        //******* MUESTRA DATOS EN LA NOMINA ||GRID||*********\\\
                        gridNomina.Rows[n].Cells[0].Value = rancho;//Rancho para ordenar\\
                        gridNomina.Rows[n].Cells[1].Value = NoEmpleado; //No Empleado\\
                        gridNomina.Rows[n].Cells[2].Value = nombre;//Nombre empleado\\
                        gridNomina.Rows[n].Cells[3].Value = puesto;//puesto empleado\\
                        gridNomina.Rows[n].Cells[4].Value = Puesto[cont].Sueldo;//sueldo diario\\
                        gridNomina.Rows[n].Cells[5].Value = salarioSemanal.ToString("###,###.##"); //**MUESTRA EL SUELDO SEMANAL ORDINARIO**\\
                        int AsistenciaParaMuestra = 0;
                        if (asistencias == 6)
                            AsistenciaParaMuestra = 0;
                        else if (asistencias == 7)
                            AsistenciaParaMuestra = 0;
                        else if (asistencias < 6)
                            AsistenciaParaMuestra = 6 - asistencias;
                        gridNomina.Rows[n].Cells[6].Value = AsistenciaParaMuestra;//****FALTAS EN LA SEMANA***\\\
                        gridNomina.Rows[n].Cells[8].Value = Hdoble.ToString("###,###.##");//***Horas Dobles***\\
                        gridNomina.Rows[n].Cells[9].Value = Htriple.ToString("###,###.##");//***Horas Triples***\\
                        gridNomina.Rows[n].Cells[12].Value = PrimaDominical.ToString("###,###.##");//***Prima Dominical***\\
                        gridNomina.Rows[n].Cells[16].Value = otrosIngresos;//Otros ingresos\\                     
                        gridNomina.Rows[n].Cells[18].Value = ""; //Muestra descuento del IMSS\\
                        gridNomina.Rows[n].Cells[20].Value = ajuste;
                        gridNomina.Rows[n].Cells[21].Value = Math.Round(percepcionNeta + ajuste, 0);//.ToString("###,###"); //****SALARIO TOTAL****\\
                        TotalNeto += Math.Round(percepcionNeta + ajuste, 0);//***ACUMULA LA PERCEPCION NETA***\\
                    }
                }
            }
        }

        public void NominaJubilados()
        {
            clConexion objConexion = new clConexion();
            List<Empleado> objEmpleados = new List<Empleado>();
            List<Puesto> Puesto = new List<Puesto>();
            string queryPlanta = "SELECT Id_Empleado, SUM(Asistencia), SUM(Rendimiento) FROM Asistencia_Eventual WHERE Observacion = 'Jubilados' GROUP BY Id_Empleado";
            using (SqlCommand cmd = new SqlCommand(queryPlanta, objConexion.conexion()))
            {
                SqlDataReader dr;
                cmd.Connection = objConexion.conexion();
                cmd.Connection.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows == false)
                    MessageBox.Show("No hay registros de las fechas", "Jubilados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    string nombre = "";
                    string puesto = "";
                    string rancho = "";
                    int NoEmpleado = 0;
                    int asistencias = 0;
                    double salarioSemanal = 0, otrosIngresos = 0, descIMSS = 0, subsidio = 0;
                    double factorIntegracion = 0;
                    double percepcionNeta = 0;
                    double Hdoble = 0, Htriple = 0;
                    int cont = 0, horasExtra = 0, rendimientos = 0;
                    objEmpleados = funciones.llenarListaEmpleado(objEmpleados);
                    Puesto = funciones.llenarListaPuesto(Puesto);
                    while (dr.Read())
                    {
                        Hdoble = 0;//Inicializamos las Variables de Horas Extra para noafectar a quienes no aplica (Regadores, Jornales ETC)
                        Htriple = 0;
                        //Llenamos el Grid con la informacion requerida para la nomina\\
                        int n = gridNomina.Rows.Add();
                        TotalEmpleados += 1;
                        NoEmpleado = Convert.ToInt32(dr[0].ToString());
                        asistencias = Convert.ToInt32(dr[1].ToString());
                        horasExtra = Convert.ToInt32(dr[2].ToString());//***HORAS EXTRAS**\\                                
                        foreach (Empleado empleados in objEmpleados)//Compara Id de empleados y obtiene el nombre y puesto\\
                            if (Convert.ToInt32(dr[0].ToString()) == empleados.Id)
                            {
                                nombre = empleados.Paterno + " " + empleados.Materno + " " + empleados.Nombre;
                                puesto = empleados.Puesto;
                                rancho = empleados.Rancho;
                                break;
                            }
                        if (puesto == "Jornalero" || puesto == "Regadores")
                            rendimientos = funciones.calculaRendimietos(0,puesto);//***1=Asistencia | 2=Rendimiento | 3=Asistencia+Rendimiento***\\                                
                        for (int j = 0; j < Puesto.Count; j++)
                            if (puesto == Puesto[j].NomPuesto)//compara puestos y obtiene el indice dentro de la lista Puesto\\ 
                            {
                                //gridNomina.Rows[n].Cells[5].Value = arrIntPuesto[1, i];//Salario diario
                                cont = j;
                                break;
                            }
                        //MessageBox.Show("Hora: "+horasExtra+" Asist: " + asistencias+" Sal Diario: "+ Puesto[cont].Sueldo);
                        salarioSemanal = Math.Round(funciones.calculoSalarioSemanal(asistencias, Puesto[cont].Sueldo), 2);//asistencias * Puesto[cont].Sueldo + Puesto[cont].BonoAsist + Puesto[cont].BonoRend;                                                                                             
                        otrosIngresos = Math.Round(funciones.calculoOtrosIngresos(asistencias, Puesto[cont].Otros), 2);
                        Hdoble = Math.Round(funciones.calculoHorasExtra(horasExtra, Puesto[cont].Sueldo, 1), 2);
                        Htriple = Math.Round(funciones.calculoHorasExtra(horasExtra, Puesto[cont].Sueldo, 2), 2);
                        double PrimaDominical = funciones.PrimaDomicial(funciones.HExtraDomingo(NoEmpleado), Puesto[cont].Sueldo);//Calcula la prima Dominical
                        PrimaDominical = Math.Round(PrimaDominical, 2);//Redondea la Prima Dominical para la suma
                        factorIntegracion = Math.Round(funciones.calculaFactorIntegracion(NoEmpleado, objEmpleados), 2);//Factor Integracion por medio de la funcion
                        descIMSS = Math.Round(funciones.calculaIMSS(salarioSemanal, Htriple, otrosIngresos, factorIntegracion), 2);//funcion para calcular el descuento de IMSS\\
                        subsidio = Math.Round(funciones.calculaISRCuota(salarioSemanal, Hdoble / 2, Htriple, otrosIngresos), 2);//Calcula subsidios\\
                        percepcionNeta = Math.Round(salarioSemanal + Hdoble + Htriple + PrimaDominical + otrosIngresos + subsidio - descIMSS, 2);
                        TotalSemanal += salarioSemanal;//*** ACUMULA SALARIO SEMANAL ***\\                               
                        TotalOtros += otrosIngresos;//*** ACUMULA OTROS INGRESOS ***\\                                
                        TotalImss += descIMSS;//***ACUMULA DESCUENTO DE IMSS***\\  
                        double ajuste = 0;
                        if (puesto == "Regadores" && (percepcionNeta > 1000 && percepcionNeta < 1005))//ajuste para los REGADORES QUE SE PASEN POR $3 O $4 DE SU SUELDO
                            ajuste = percepcionNeta - 1000;
                        else
                            ajuste = Math.Round(Math.Round(percepcionNeta, 0) - percepcionNeta, 2);
                        //******* MUESTRA DATOS EN LA NOMINA ||GRID||*********\\\
                        gridNomina.Rows[n].Cells[0].Value = rancho;//Rancho para ordenar\\
                        gridNomina.Rows[n].Cells[1].Value = NoEmpleado; //No Empleado\\
                        gridNomina.Rows[n].Cells[2].Value = nombre;//Nombre empleado\\
                        gridNomina.Rows[n].Cells[3].Value = puesto;//puesto empleado\\
                        gridNomina.Rows[n].Cells[4].Value = Puesto[cont].Sueldo;//sueldo diario\\
                        gridNomina.Rows[n].Cells[5].Value = salarioSemanal.ToString("###,###.##"); //**MUESTRA EL SUELDO SEMANAL ORDINARIO**\\
                        int AsistenciaParaMuestra = 0;
                        if (asistencias == 6)
                            AsistenciaParaMuestra = 0;
                        else if (asistencias == 7)
                            AsistenciaParaMuestra = 0;
                        else if (asistencias < 6)
                            AsistenciaParaMuestra = 6 - asistencias;
                        gridNomina.Rows[n].Cells[6].Value = AsistenciaParaMuestra;//****FALTAS EN LA SEMANA***\\\
                        gridNomina.Rows[n].Cells[8].Value = Hdoble.ToString("###,###.##");//***Horas Dobles***\\
                        gridNomina.Rows[n].Cells[9].Value = Htriple.ToString("###,###.##");//***Horas Triples***\\
                        gridNomina.Rows[n].Cells[12].Value = PrimaDominical.ToString("###,###.##");//***Prima Dominical***\\
                        gridNomina.Rows[n].Cells[16].Value = otrosIngresos;//Otros ingresos\\
                        if (subsidio < 0)
                        {
                            subsidio = subsidio * -1; //***LO CONVIERTE EN POSITIVO PARA MOSTRARLO COMO TAL***\\
                            gridNomina.Rows[n].Cells[19].Value = subsidio.ToString("##.##");//Muestra Retenciones de ISR\\
                            TotalIsr += subsidio;//***ACUMULA RETENCIONES ISR***\\
                            gridNomina.Rows[n].Cells[17].Value = ("");
                        }
                        else
                        {
                            gridNomina.Rows[n].Cells[19].Value = ("");
                            gridNomina.Rows[n].Cells[17].Value = subsidio.ToString("##.##");//Muestra Subsidios a favor\\
                            TotalSubsidio += subsidio; //***ACUMULA SUBSIDIOS***\\
                        }
                        gridNomina.Rows[n].Cells[18].Value = descIMSS.ToString("##.##"); //Muestra descuento del IMSS\\
                        gridNomina.Rows[n].Cells[20].Value = ajuste;
                        gridNomina.Rows[n].Cells[21].Value = Math.Round(percepcionNeta + ajuste, 0);//.ToString("###,###"); //****SALARIO TOTAL****\\
                        TotalNeto += Math.Round(percepcionNeta + ajuste, 0);//***ACUMULA LA PERCEPCION NETA***\\
                    }
                }
            }
        }

        //**** CALCULA LA NOMINA ****\\
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = true;
            Thread Hilo1 = new Thread(new ThreadStart(wait));        
            Hilo1.Start();
            string queryPlanta = "SELECT Id_Empleado, SUM(Asistencias), SUM(Faltas), SUM(HE) FROM Asistencia_Planta GROUP BY Id_Empleado";            
            clConexion objConexion = new clConexion();
            List<Empleado> empleadoPlanta = new List<Empleado>();   
            List<Empleado> empleadoExterno = new List<Empleado>();
            List<Puesto> Puesto = new List<Puesto>();            
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
                            MessageBox.Show("No hay registros de las fechas", "PLANTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            string nombre = "";
                            string puesto = "";
                            string rancho = "";
                            double SalarioDiario = 0;
                            int NoEmpleado = 0;
                            int asistencias = 0;
                            double salarioSemanal = 0, otrosIngresos = 0, descIMSS = 0, subsidio = 0;                            
                            double percepcionNeta = 0;
                            double Hdoble = 0, Htriple = 0;
                            int cont = 0, horasExtra = 0;
                            empleadoPlanta = funciones.llenarListaEmpleado(empleadoPlanta);
                            Puesto = funciones.llenarListaPuesto(Puesto);                            
                            while (dr.Read())
                            {
                                int rendimientos = 0;
                                Hdoble = 0;//Inicializamos las Variables de Horas Extra para noafectar a quienes no aplica (Regadores, Jornales ETC)
                                Htriple = 0;
                                //Llenamos el Grid con la informacion requerida para la nomina\\
                                int n = gridNomina.Rows.Add();
                                TotalEmpleados += 1;
                                NoEmpleado = Convert.ToInt32(dr[0].ToString());
                                asistencias = Convert.ToInt32(dr[1].ToString());
                                horasExtra = Convert.ToInt32(dr[3].ToString());//***HORAS EXTRAS**\\                                
                                foreach (Empleado empleados in empleadoPlanta)//Compara Id de empleados y obtiene el nombre y puesto\\
                                    if (Convert.ToInt32(dr[0].ToString()) == empleados.Id)
                                    {
                                        nombre = empleados.Paterno + " " + empleados.Materno + " " + empleados.Nombre;
                                        puesto = empleados.Puesto;
                                        rancho = empleados.Rancho;
                                        SalarioDiario = empleados.SalarioDiario;                                        
                                        break;
                                    }
                               
                                    if ((puesto == "Jornalero" || puesto == "Regadores") && asistencias >= 6)
                                        rendimientos = funciones.calculaRendimietos(3, puesto);//***1=Asistencia | 2=Rendimiento | 3=Asistencia+Rendimiento***\\                                
                               
        
                                for (int j = 0; j < Puesto.Count; j++)
                                    if (puesto == Puesto[j].NomPuesto)//compara puestos y obtiene el indice dentro de la lista Puesto\\ 
                                    {
                                        //gridNomina.Rows[n].Cells[5].Value = arrIntPuesto[1, i];//Salario diario
                                        cont = j;
                                        break;
                                    }

                                if (SalarioDiario == 0)
                                {
                                    SalarioDiario = Puesto[cont].Sueldo;
                                }
                                //MessageBox.Show("Hora: "+horasExtra+" Asist: " + asistencias+" Sal Diario: "+ Puesto[cont].Sueldo);
                                salarioSemanal = Math.Round(funciones.calculoSalarioSemanal(asistencias, SalarioDiario), 2);//asistencias * Puesto[cont].Sueldo + Puesto[cont].BonoAsist + Puesto[cont].BonoRend;                                                                                                                                                            
                                otrosIngresos = Math.Round(funciones.calculoOtrosIngresos(asistencias, Puesto[cont].Otros), 2) + rendimientos;
                                if (puesto == "Tractorista" && horasExtra < 16)
                                    otrosIngresos = 0;
                                Hdoble = Math.Round(funciones.calculoHorasExtra(horasExtra, SalarioDiario, 1), 2);
                                Htriple = Math.Round(funciones.calculoHorasExtra(horasExtra, SalarioDiario, 2), 2);
                                TotalHorasExtra += (Hdoble + Htriple);
                                double PrimaDominical = funciones.PrimaDomicial(funciones.HExtraDomingo(NoEmpleado), SalarioDiario);//Calcula la prima Dominical
                                PrimaDominical = Math.Round(PrimaDominical, 2);//Redondea la Prima Dominical para la suma
                                //****SE DESHABILITAN EN BASE AL NUEVO FORMATO****\\                               
                                //factorIntegracion = Math.Round(funciones.calculaFactorIntegracion(NoEmpleado, empleadoPlanta), 2);//Factor Integracion por medio de la funcion
                                //descIMSS = Math.Round(funciones.calculaIMSS(salarioSemanal, Htriple, otrosIngresos, factorIntegracion), 2);//funcion para calcular el descuento de IMSS\\
                                //subsidio = Math.Round(funciones.calculaISRCuota(salarioSemanal, Hdoble / 2, Htriple, otrosIngresos), 2);//Calcula subsidios\\
                                percepcionNeta = Math.Round(salarioSemanal + Hdoble + Htriple + PrimaDominical + otrosIngresos + subsidio - descIMSS, 2);                                     
                                TotalSemanal += salarioSemanal;//*** ACUMULA SALARIO SEMANAL ***\\                               
                                TotalOtros += otrosIngresos;//*** ACUMULA OTROS INGRESOS ***\\     
                                TotalPrima += PrimaDominical;
                                TotalImss += descIMSS;//***ACUMULA DESCUENTO DE IMSS***\\  
                                double ajuste = 0;
                                if (puesto == "Regadores" && (percepcionNeta > 1000 && percepcionNeta < 1005))//ajuste para los REGADORES QUE SE PASEN POR $3 O $4 DE SU SUELDO
                                    ajuste = Math.Round(percepcionNeta - 1000, 2);
                                else
                                    ajuste = Math.Round(Math.Round(percepcionNeta, 0) - percepcionNeta, 2);
                                TotalAjuste += ajuste;
                                //********MUESTRA DATOS EN LA NOMINA ||GRID||*********\\\
                                gridNomina.Rows[n].Cells[0].Value = rancho;//Rancho para ordenar\\
                                gridNomina.Rows[n].Cells[1].Value = NoEmpleado; //No Empleado\\
                                gridNomina.Rows[n].Cells[2].Value = nombre;//Nombre empleado\\
                                gridNomina.Rows[n].Cells[3].Value = puesto;//puesto empleado\\
                                gridNomina.Rows[n].Cells[4].Value = SalarioDiario;//sueldo diario\\
                                gridNomina.Rows[n].Cells[5].Value = salarioSemanal.ToString("###.##"); //**MUESTRA EL SUELDO SEMANAL ORDINARIO**\\
                                int AsistenciaParaMuestra = 0;
                                if (asistencias == 6 && AsistenciaParaMuestra == 7)
                                    AsistenciaParaMuestra = 0;
                                else if (asistencias < 6)
                                    AsistenciaParaMuestra = 6 - asistencias;
                                gridNomina.Rows[n].Cells[6].Value = AsistenciaParaMuestra;//****FALTAS EN LA SEMANA***\\\
                                gridNomina.Rows[n].Cells[8].Value = Hdoble.ToString("###,###.##");//***Horas Dobles***\\
                                gridNomina.Rows[n].Cells[9].Value = Htriple.ToString("###,###.##");//***Horas Triples***\\
                                gridNomina.Rows[n].Cells[12].Value = PrimaDominical.ToString("###,###.##");//***Prima Dominical***\\
                                gridNomina.Rows[n].Cells[16].Value = otrosIngresos;//Otros ingresos\\
                                if (subsidio < 0)
                                {
                                    subsidio = subsidio * -1; //***LO CONVIERTE EN POSITIVO PARA MOSTRARLO COMO TAL***\\
                                    gridNomina.Rows[n].Cells[19].Value = subsidio.ToString("##.##");//Muestra Retenciones de ISR\\
                                    TotalIsr += subsidio;//***ACUMULA RETENCIONES ISR***\\
                                    gridNomina.Rows[n].Cells[17].Value = ("");
                                }
                                else
                                {
                                    gridNomina.Rows[n].Cells[19].Value = ("");
                                    gridNomina.Rows[n].Cells[17].Value = subsidio.ToString("##.##");//Muestra Subsidios a favor\\
                                    TotalSubsidio += subsidio; //***ACUMULA SUBSIDIOS***\\
                                }
                                gridNomina.Rows[n].Cells[18].Value = descIMSS.ToString("##.##"); //Muestra descuento del IMSS\\
                                gridNomina.Rows[n].Cells[20].Value = ajuste;
                                gridNomina.Rows[n].Cells[21].Value = Math.Round(percepcionNeta + ajuste, 0);//.ToString("###,###"); //****SALARIO TOTAL****\\
                                TotalNeto += Math.Round(percepcionNeta + ajuste, 0);//***ACUMULA LA PERCEPCION NETA***\\
                            }
                        }
                    }
                    //****************************\\
                    //***NOMINA PARA EVENTUALES***\\
                    //****************************\\
                    int contador1 = 0, contador2 = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        string QueryBusqueda = "";                        
                        if (i == 0)
                            QueryBusqueda = "SELECT * FROM Asistencia_OD WHERE Observacion = 'Poda'";
                        if (i == 1)
                            QueryBusqueda = "SELECT * FROM Asistencia_OD WHERE Observacion = 'Cosecha'";                       

                        using (SqlCommand cmd = new SqlCommand(QueryBusqueda, objConexion.conexion()))
                        {
                            SqlDataReader dr;
                            cmd.Connection = objConexion.conexion();
                            cmd.Connection.Open();
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows == true)
                            {
                                while (dr.Read())
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            contador1 += 1;
                                            NominaPoda();
                                            break;
                                        case 1:
                                            contador2 += 1;
                                            NominaCosecha();
                                            break;                                        
                                        default:
                                            MessageBox.Show("No Hay Registros de trabajadores Eventuales", "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    //MessageBox.Show("Empleados en Poda: "+contador1+", Empleados en Cosecha: "+contador2+", Empleados por Rendimiento: "+contador3);

                    //*********MUESTRA A LOS JUBILADOS EN LA NOMINA**********\\
                    NominaJubilados();                   
                    string queryExternos = "SELECT Id_Empleado, SUM(Importe) FROM Cobro_Externos GROUP BY Id_Empleado";
                    using (SqlCommand cmd = new SqlCommand(queryExternos, objConexion.conexion()))
                    {
                        SqlDataReader dr;
                        cmd.Connection = objConexion.conexion();
                        cmd.Connection.Open();
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows == false)
                            MessageBox.Show("No hay registros de las fechas", "EXTERNOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            string Nombre = "";
                            string puesto = "";
                            string Rancho = "";
                            empleadoExterno = funciones.llenarListaEmpleado(empleadoExterno);
                            Puesto = funciones.llenarListaPuesto(Puesto);
                            while (dr.Read())
                            {
                                int n = gridNomina.Rows.Add();
                                TotalEmpleados += 1;
                                int NoEmpleado = Convert.ToInt32(dr[0].ToString());
                                double Importe = Convert.ToDouble(dr[1].ToString());
                                foreach (Empleado empleados in empleadoExterno)//Compara Id de empleados y obtiene el nombre y puesto\\
                                    if (Convert.ToInt32(dr[0].ToString()) == empleados.Id)
                                    {
                                        Nombre = empleados.Paterno + " " + empleados.Materno + " " + empleados.Nombre;
                                        puesto = empleados.Puesto;
                                        Rancho = empleados.Rancho;
                                        break;
                                    }
                                TotalOtros += Importe;
                                TotalNeto += Importe;
                                //***MUESTRA DATOS DE LOS EXTERNOS EN EL GRID***\\
                                gridNomina.Rows[n].Cells[0].Value = Rancho;
                                gridNomina.Rows[n].Cells[1].Value = NoEmpleado;
                                gridNomina.Rows[n].Cells[2].Value = Nombre;
                                gridNomina.Rows[n].Cells[3].Value = puesto;
                                gridNomina.Rows[n].Cells[4].Value = "";
                                gridNomina.Rows[n].Cells[5].Value = "";
                                gridNomina.Rows[n].Cells[6].Value = "";
                                gridNomina.Rows[n].Cells[7].Value = "";
                                gridNomina.Rows[n].Cells[8].Value = "";
                                gridNomina.Rows[n].Cells[9].Value = "";
                                gridNomina.Rows[n].Cells[10].Value = "";
                                gridNomina.Rows[n].Cells[11].Value = "";
                                gridNomina.Rows[n].Cells[12].Value = "";
                                gridNomina.Rows[n].Cells[13].Value = "";
                                gridNomina.Rows[n].Cells[14].Value = "";
                                gridNomina.Rows[n].Cells[15].Value = "";
                                gridNomina.Rows[n].Cells[16].Value = Importe;
                                gridNomina.Rows[n].Cells[17].Value = "";
                                gridNomina.Rows[n].Cells[18].Value = "";
                                gridNomina.Rows[n].Cells[19].Value = "";
                                gridNomina.Rows[n].Cells[20].Value = "";
                                gridNomina.Rows[n].Cells[21].Value = Importe;
                            }
                        }
                    }
                    muestraEtiquetas();
                    button1.Enabled = true;
                    btnCalcular.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Error al generar Nomina", "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }            
            //CERRAR WAIT Form...
            Hilo1.Abort();
            button1.Enabled = true;
        }

        public void contarAsistencias(int id)
        {            
        }

        private void Nomina_Load(object sender, EventArgs e)
        {
            //vaciaAsistencias();
            toolStripStatusLabel1.Text = "Usuario: " + LogUsers.getUserName();
           /*
            * Vacia tabla de Nomina
            string query = "";
            clConexion objConexion = new clConexion();
            try
            {               
                    query = "TRUNCATE TABLE Nomina";
                    using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                    {
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = objConexion.conexion();
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error al vaciar Nomina");
            }
            // Vacia Respaldos de Asistencias
            string query = "";
            clConexion objConexion = new clConexion();
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    if (i == 0)
                        query = "TRUNCATE TABLE Asistencia_Planta";
                    else if (i == 1)
                        query = "TRUNCATE TABLE Asistencia_Eventual_Resp";
                    else if (i == 2)
                        query = "TRUNCATE TABLE Asistencia_OD_Resp";
                    using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                    {
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = objConexion.conexion();
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error al vaciar asistencias");
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int IdNomina = (Convert.ToInt32(DateTime.Today.Year) * 100) + numeroSemana();
            respaldaNomina(IdNomina);
            respTabla();           
        }

        private void button2_Click(object sender, EventArgs e)            
        {
            //EXPORTA LA INFORMACION DE PRENOMINA A UN ARCHIVO COMPATIBLE CSV CON EL SISTEMA TRESS EN TIJUANA
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                && saveFileDialog1.FileName.Length > 0)
            {
                int ban = 0;               
                try
                {
                    FileInfo t = new FileInfo(saveFileDialog1.FileName);
                    StreamWriter Tex = t.CreateText();                    
                    int n = 0;
                    foreach (DataGridViewRow row in gridNomina.Rows)
                    {

                        string Campo1Tress = gridNomina.Rows[n].Cells[1].Value.ToString();
                        string Campo2Tress = ",10,";
                        string Campo3Tress = DateTime.Now.Date.ToString().Substring(0, 10); ;
                        string Campo4Tress = ",CB_NETO,";
                        string Campo5Tress = gridNomina.Rows[n].Cells[21].Value.ToString();
                                                
                        string cadena = Campo1Tress + Campo2Tress +                            
                            Campo3Tress + Campo4Tress + Campo5Tress;

                        Tex.WriteLine(cadena);
                        n += 1;

                    }
                    Tex.Write(Tex.NewLine);
                    Tex.Close();
                }
                catch (Exception ex)
                {
                    ban = 1;
                    MessageBox.Show("No se pudo crear el archivo de piramidacion. Intente de Nuevo", "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (ban == 0)
                    MessageBox.Show("El Archivo " + saveFileDialog1.FileName + " ha sido creado", "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Information);               
            }           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //EXPORTA EL ARCHIVO DE NOMINA A CSV 
            string[] Cabecera = new string[gridNomina.Columns.Count];
            try
            {
                for (int i = 0; i < gridNomina.Columns.Count; i++)
                    Cabecera[i] = gridNomina.Columns[i].HeaderText;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Exportar.Nomina(gridNomina, Cabecera);
            }
        }

    }
}
