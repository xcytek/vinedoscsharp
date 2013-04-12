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
    public partial class Asistencia : Form
    {        
        public Asistencia()
        {
            InitializeComponent();
        }

        int ban = 0;
        int reloj = 0;
        public void ActualizaAsistencia(int NoEmpleado, DateTime Fecha)
        {
        }
        public int ValidaAsistenciaRepetida(int NoEmpleado, DateTime Fecha, string Tabla)
        {
            int Bandera = 1;
            string Dia_Semana = Fecha.DayOfWeek.ToString(); 
            string query = "SELECT * FROM " + Tabla + " WHERE Id_Empleado = " + NoEmpleado;
            clConexion objConexion = new clConexion();
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
                        {
                            DateTime FechaDB = Convert.ToDateTime(dr[1].ToString());
                            string Dia_SemanaDB = FechaDB.DayOfWeek.ToString();
                            if (Dia_Semana == Dia_SemanaDB)
                            { Bandera = 1; break; }
                            else
                                Bandera = 0;
                        }
                    else
                        Bandera = 0;
                }
            }
            catch (Exception ex)
            { Bandera = 1; }
            return Bandera;
        }       
        /// <summary>
        /// ***********************************************************************************************************************************
        /// CARGA INFORMACION DE CADA EMPLEADO EN EL GRID DE ASISTENCIAS, PARA TENER REFERENCIA DE CUALES DIAS SON LOS QUE SE LE HAN REGISTRADO
        /// Y VERIFICAR QUE NO SE REPITA LA INFORMACION.***************************************************************************************
        /// ***********************************************************************************************************************************
        /// </summary>
        public void ShowGridData(int NoEmpleado)
        {            
            string query = "";
            dataGridView2.Rows.Clear();
            for (int i = 0; i < 3; i++)
            { dataGridView2.Rows.Add(); }
            string[] DiasSemana = { "Thursday", "Friday", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday" };
            if (plantaToolStripMenuItem.Checked == true || desbroteMasToolStripMenuItem.Checked == true)
                query = "SELECT * FROM Asistencia_Planta WHERE Id_Empleado =" + NoEmpleado;
            else if (podaToolStripMenuItem.Checked == true || cosechaToolStripMenuItem.Checked == true)
                query = "SELECT * FROM Asistencia_OD WHERE Id_Empleado =" + NoEmpleado;
            else if (jubiladosToolStripMenuItem.Checked == true)
            { query = "SELECT * FROM Asistencia_Planta WHERE Id_Empleado =" + NoEmpleado; }
            else if (externosToolStripMenuItem.Checked == true)
            { query = "SELECT * FROM Cobro_Externos WHERE Id_Empleado = " + NoEmpleado; }            
            clConexion objConexion = new clConexion();
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
                        {
                            DateTime Fecha = Convert.ToDateTime(dr[1].ToString());
                            string Dia_Semana = Fecha.DayOfWeek.ToString();
                            for (int j = 0; j < 7; j++)
                                if (Dia_Semana == DiasSemana[j])
                                {
                                    dataGridView2.Rows[0].Cells[j].Value = dr[2].ToString();
                                    dataGridView2.Rows[1].Cells[j].Value = dr[3].ToString();
                                    dataGridView2.Rows[2].Cells[j].Value = dr[7].ToString();
                                    dataGridView2.Rows[3].Cells[j].Value = dr[6].ToString();
                                }                                
                        }
                    else                    
                        for (int j = 0; j < 7; j++)                        
                            for (int i = 0; i < 4; i++)                            
                                dataGridView2.Rows[i].Cells[j].Value = "";                                                                                                                            
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void CargaRanchoGeneral()
        {
            string query = "SELECT Clave,Rancho from R";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cbRancho.DataSource = dt;
                    cbRancho.ValueMember = dt.Columns[0].ToString();
                    cbRancho.DisplayMember = dt.Columns[1].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void habilitaPanel(int Num, int Opc)
        {
            switch (Num)
            {
                case 1:
                    PanelPlanta.Visible = true;
                    PanelExternos.Visible = false;
                    PanelJubilados.Visible = false;                    
                    PanelPodaCosecha.Visible = false;
                    PanelRendimiento.Visible = false;                    
                    break;
                case 2:
                    if (Opc == 1)
                    { label23.Text = "Plantas"; label24.Text = "Precio por Planta"; }
                    else if (Opc == 2)
                    { label23.Text = "Toneladas"; label24.Text = "Precio por Tonelada"; }                    
                    PanelPodaCosecha.Visible = true;
                    PanelPlanta.Visible = false;
                    PanelExternos.Visible = false;
                    PanelJubilados.Visible = false;                                     
                    PanelRendimiento.Visible = false;   
                    break;
                case 3:
                    PanelRendimiento.Visible = true;   
                    PanelPlanta.Visible = false;
                    PanelExternos.Visible = false;
                    PanelJubilados.Visible = false;                    
                    PanelPodaCosecha.Visible = false;                    
                    break;
                case 4:
                    PanelJubilados.Visible = true;   
                    PanelPlanta.Visible = false;
                    PanelExternos.Visible = false;                                     
                    PanelPodaCosecha.Visible = false;
                    PanelRendimiento.Visible = false;   
                    break;
                case 5:
                    PanelExternos.Visible = true;
                    PanelPlanta.Visible = false;                    
                    PanelJubilados.Visible = false;                    
                    PanelPodaCosecha.Visible = false;
                    PanelRendimiento.Visible = false;   
                    break;
                default:
                    break;
            }
        }
        public void checkMenu(int Opc)
        {
            switch (Opc)
            {
                case 1:
                    plantaToolStripMenuItem.Checked = true;
                    podaToolStripMenuItem.Checked = false;                    
                    cosechaToolStripMenuItem.Checked = false;
                    desbroteMasToolStripMenuItem.Checked = false;
                    jubiladosToolStripMenuItem.Checked = false;
                    externosToolStripMenuItem.Checked = false;
                    break;
                case 2:
                    podaToolStripMenuItem.Checked = true;
                    plantaToolStripMenuItem.Checked = false;
                    cosechaToolStripMenuItem.Checked = false;
                    desbroteMasToolStripMenuItem.Checked = false;
                    jubiladosToolStripMenuItem.Checked = false;
                    externosToolStripMenuItem.Checked = false;
                    break;
                case 3:
                    cosechaToolStripMenuItem.Checked = true;
                    podaToolStripMenuItem.Checked = false;
                    plantaToolStripMenuItem.Checked = false;                    
                    desbroteMasToolStripMenuItem.Checked = false;
                    jubiladosToolStripMenuItem.Checked = false;
                    externosToolStripMenuItem.Checked = false;
                    break;
                case 4:
                    desbroteMasToolStripMenuItem.Checked = true;                    
                    podaToolStripMenuItem.Checked = false;
                    plantaToolStripMenuItem.Checked = false;
                    cosechaToolStripMenuItem.Checked = false;
                    jubiladosToolStripMenuItem.Checked = false;
                    externosToolStripMenuItem.Checked = false;
                    break;
                case 5:
                    jubiladosToolStripMenuItem.Checked = true;                    
                    podaToolStripMenuItem.Checked = false;
                    plantaToolStripMenuItem.Checked = false;
                    cosechaToolStripMenuItem.Checked = false;
                    desbroteMasToolStripMenuItem.Checked = false;                    
                    externosToolStripMenuItem.Checked = false;
                    break;
                case 6:
                    externosToolStripMenuItem.Checked = true;
                    cosechaToolStripMenuItem.Checked = false;
                    podaToolStripMenuItem.Checked = false; 
                    plantaToolStripMenuItem.Checked = false;                    
                    desbroteMasToolStripMenuItem.Checked = false;
                    jubiladosToolStripMenuItem.Checked = false;                    
                    break;
            }
        }
        public void creaColumna(string Nombre, string Etiqueta)
        {
            DataGridViewColumn column = new DataGridViewColumn();
            column.Name = Nombre;
            column.HeaderText = Etiqueta;
            DataGridViewCell cell = new DataGridViewTextBoxCell();
            cell.Style.BackColor = Color.White;
            column.CellTemplate = cell;
            grid1.Columns.Add(column);
        }
        public void cargaEmpleadosGrid(string TipoEmpleado, string Rancho)
        {
            grid1.Rows.Clear();
            grid1.Columns.Clear();
            creaColumna("Numero", "Numero");
            creaColumna("Nombre", "Nombre");
            creaColumna("Puesto", "Puesto");
            string query = "";
            if (TipoEmpleado == "EX" || TipoEmpleado == "J")
                query = "SELECT * From Empleados WHERE Tipo = '" + TipoEmpleado + "'";
            else if (TipoEmpleado == "P" || TipoEmpleado == "E")
                query = "SELECT * From Empleados WHERE Rancho = '" + Rancho + "' AND Tipo = '" + TipoEmpleado + "'";
            clConexion objConexion = new clConexion();
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
                        {
                            int NoEmpleado = int.Parse(dr[0].ToString());
                            string Nombre = dr[1].ToString() + " " + dr[2].ToString() + " " + dr[3].ToString();
                            string Puesto = dr[17].ToString();
                            int n = grid1.Rows.Add();
                            grid1.Rows[n].Cells[0].Value = NoEmpleado;
                            grid1.Rows[n].Cells[1].Value = Nombre;
                            grid1.Rows[n].Cells[2].Value = Puesto;

                        }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void cargaEmpleadosGrid(string TipoEmpleado)
        {
            grid1.Rows.Clear();
            grid1.Columns.Clear();
            creaColumna("Numero", "Numero");
            creaColumna("Nombre", "Nombre");
            creaColumna("Puesto", "Puesto");
            string query = "";
            if (TipoEmpleado == "P")
                query = "SELECT * From Empleados WHERE Tipo != '" + TipoEmpleado + "' ORDER BY Paterno ASC";            
            clConexion objConexion = new clConexion();
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
                        {
                            int NoEmpleado = int.Parse(dr[0].ToString());
                            string Nombre = dr[1].ToString() + " " + dr[2].ToString() + " " + dr[3].ToString();
                            string Puesto = dr[16].ToString();
                            int n = grid1.Rows.Add();
                            grid1.Rows[n].Cells[0].Value = NoEmpleado;
                            grid1.Rows[n].Cells[1].Value = Nombre;
                            grid1.Rows[n].Cells[2].Value = Puesto;

                        }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       

        public void almacenaExternos(int IdEmpleado, DateTime Fecha, string Descripcion, double Importe, string Rancho)
        {
            clConexion objConexion = new clConexion();
            string query = "Insert into Cobro_Externos (Id_Empleado,Fecha,Descripcion,Importe,Rancho) values (@Id_Empleado,@Fecha,@Descripcion,@Importe,@Rancho)";
            using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id_Empleado", IdEmpleado);
                cmd.Parameters.AddWithValue("@Fecha", Fecha);
                cmd.Parameters.AddWithValue("@Descripcion", Descripcion);
                cmd.Parameters.AddWithValue("@Importe", Importe);
                cmd.Parameters.AddWithValue("@Rancho", Rancho);
                cmd.Connection = objConexion.conexion();
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public void almacenaPoda(int Id_Empleado, DateTime Fecha, double Plantas, double Precio, string Actividad, string Rancho,
            string Lote, string Observacion)
        {
            clConexion objConexion = new clConexion();
            string query = "Insert into Asistencia_OD (Id_Empleado,Fecha,Plantas,Precio,Actividad,Rancho,Lote1,Observacion)"+
                " values (@Id,@Fecha,@Plantas,@Precio,@Actividad,@Rancho,@Lote,@Observacion)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", Id_Empleado);
                    cmd.Parameters.AddWithValue("@Fecha", Fecha);
                    cmd.Parameters.AddWithValue("@Plantas", Plantas);
                    cmd.Parameters.AddWithValue("@Precio", Precio);
                    cmd.Parameters.AddWithValue("@Actividad", Actividad);
                    cmd.Parameters.AddWithValue("@Rancho", Rancho);
                    cmd.Parameters.AddWithValue("@Lote", Lote);
                    cmd.Parameters.AddWithValue("@Observacion", Observacion);                    
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }        

        public void almacenaRendimiento(int Id_Empleado, DateTime Fecha, int Rendimiento, int Asistencia, string Actividad,
            string Lote, string Rancho, string Observacion)
        {
            clConexion objConexion = new clConexion();
            string query = "Insert into Asistencia_Eventual (Id_Empleado,Fecha,Rendimiento,Asistencia,Actividad,Lote1,Rancho,Observacion)" +
                " values (@Id,@Fecha,@Rendimiento,@Asistencia,@Actividad,@Lote,@Rancho,@Observacion)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", Id_Empleado);
                    cmd.Parameters.AddWithValue("@Fecha", Fecha);
                    cmd.Parameters.AddWithValue("@Rendimiento", Rendimiento);
                    cmd.Parameters.AddWithValue("@Asistencia", Asistencia);
                    //cmd.Parameters.AddWithValue("@Faltas", Faltas);
                    cmd.Parameters.AddWithValue("@Actividad", Actividad);
                    cmd.Parameters.AddWithValue("@Lote", Lote);
                    cmd.Parameters.AddWithValue("@Rancho", Rancho);
                    cmd.Parameters.AddWithValue("@Observacion", Observacion);
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void almacenaJubilados(int Id_Empleado,DateTime Fecha, bool Asist, int HorasExtra, string Actividad, string Lote, string Observacion)
        {
            int Asistencia, Falta;
            if (Asist == true)
            { Asistencia = 1; Falta = 0; }
            else
            { Asistencia = 0; Falta = 1; }
            clConexion objConexion = new clConexion();
            string query = "Insert into Asistencia_Eventual (Id_Empleado,Fecha,Rendimiento,Asistencia,faltas,Actividad,Lote1,Observacion)" +
                " values (@Id,@Fecha,@Rendimiento,@Asistencia,@Faltas,@Actividad,@Lote,@Observacion)";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", Id_Empleado);
                    cmd.Parameters.AddWithValue("@Fecha", Fecha);
                    cmd.Parameters.AddWithValue("@Rendimiento", HorasExtra);
                    cmd.Parameters.AddWithValue("@Asistencia", Asistencia);
                    cmd.Parameters.AddWithValue("@Faltas", Falta);
                    cmd.Parameters.AddWithValue("@Actividad", Actividad);
                    cmd.Parameters.AddWithValue("@Lote", Lote);                    
                    cmd.Parameters.AddWithValue("@Observacion", Observacion);
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void CargaExternos()
        {
           
            

        }

        public void CargaDatosExternos(int IdEmpleado)
        {
            
        }

        public double SumaColumnaExternos()
        {
            return 0;
        }       
        
        private void cbRancho_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid1.Rows.Clear();
            if (plantaToolStripMenuItem.Checked == true)
                cargaEmpleadosGrid("P", cbRancho.Text);
                //MessageBox.Show(cbRancho.Text);
            else if (podaToolStripMenuItem.Checked == true || cosechaToolStripMenuItem.Checked == true || desbroteMasToolStripMenuItem.Checked == true)
                cargaEmpleadosGrid("E", cbRancho.Text);           
            btnGuardar.Visible = true;
            btnCancelar.Visible = true;
            cargaLotePlanta(cbRancho.SelectedValue.ToString());
            cargaLoteRendimiento(cbRancho.SelectedValue.ToString());
            cargaLotePodaCosecha(cbRancho.SelectedValue.ToString());
        }
        
        private void plantaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void cbRancho_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }
        ///GUARDA ASISTENCIAS EMPLEADOS PLANTA Y JUBILADOS(eventuales)
        public int guardaAsistenciaPlanta(int id,DateTime fecha, string asi, int he, int Asist, int faltas, int act,string lote1, string rancho)
        {
            int Error = 0;
             clConexion objConexion = new clConexion();
             try
             {                 
                 string query = "Insert into Asistencia_Planta (Id_Empleado,Fecha,Asist,HE,Asistencias,Faltas,Actividad,Lote1,Rancho) values (@Id,@Fecha,@Asist,@HE,@Asistencia,@Faltas,@Actividad,@Lote1,@Rancho)";
                 using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                 {
                     cmd.CommandText = query;
                     cmd.CommandType = CommandType.Text;
                     cmd.Parameters.AddWithValue("@Id", id);
                     cmd.Parameters.AddWithValue("@Fecha", fecha);
                     cmd.Parameters.AddWithValue("@Asist", asi);
                     cmd.Parameters.AddWithValue("@HE", he);
                     cmd.Parameters.AddWithValue("@Asistencia", Asist);
                     cmd.Parameters.AddWithValue("@Faltas", faltas);
                     cmd.Parameters.AddWithValue("@Actividad", act);
                     cmd.Parameters.AddWithValue("@Lote1", lote1);
                     cmd.Parameters.AddWithValue("@Rancho", rancho);
                     cmd.Connection = objConexion.conexion();
                     cmd.Connection.Open();
                     cmd.ExecuteNonQuery();
                 }
             }
             catch (Exception ex)
             { Error = 1; }
            return Error;
        }
        public int guardaAsistenciaPodaCosecha(int id,DateTime fecha, int Plantas, double Precio, int Asistencias, int Faltas, int Actividad, string Rancho, string Lote)
        {
            int Error = 0;
            try
            { 
                clConexion objConexion = new clConexion();
                string query = "Insert into Asistencia_OD (Id_Empleado,Fecha,Plantas,Precio,Asistencias,faltas,Actividad,Lote1,Rancho) values (@Id,@Fecha,@Plantas,@Precio,@Asistencia,@Faltas,@Actividad,@Lote1,@Rancho)";
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Fecha", fecha);
                    cmd.Parameters.AddWithValue("@Plantas", Plantas);
                    cmd.Parameters.AddWithValue("@Precio", Precio);
                    cmd.Parameters.AddWithValue("@Asistencia", Asistencias);
                    cmd.Parameters.AddWithValue("@Faltas", Faltas);
                    cmd.Parameters.AddWithValue("@Actividad", Actividad);
                    cmd.Parameters.AddWithValue("@Lote1", Lote);
                    cmd.Parameters.AddWithValue("@Rancho", Rancho);
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            { Error = 1; }
            return Error;
        }
        public int guardaAsistenciaRendimiento(int id, DateTime Fecha, string Asist, int Rendimiento, int Asistencia, int Falta, string Actividad, string Lote1, string Rancho)
        {
            int Error = 0;
            try
            {
                clConexion objConexion = new clConexion();
                string query = "Insert into Asistencia_Eventual (Id_Empleado,Fecha,Asist,Rendimiento,Asistencia,faltas,Actividad,Lote1,Rancho) values (@Id,@Fecha,@Asist,@Rendimiento,@Asistencia,@Faltas,@Actividad,@Lote1,@Rancho)";
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Fecha", Fecha);
                    cmd.Parameters.AddWithValue("@Asist", Asist);
                    cmd.Parameters.AddWithValue("@Rendimiento", Rendimiento);
                    cmd.Parameters.AddWithValue("@Asistencia", Asistencia);
                    cmd.Parameters.AddWithValue("@Faltas", Falta);
                    cmd.Parameters.AddWithValue("@Actividad", Actividad);
                    cmd.Parameters.AddWithValue("@Lote1", Lote1);
                    cmd.Parameters.AddWithValue("@Rancho", Rancho);
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            { Error = 1; }
            return Error;
        }        
        public int guardaAsistenciaExterno(int id, DateTime Fecha, string Descripcion, double Importe, string Rancho)
        {
            int Error = 0;
            try
            {
                clConexion objConexion = new clConexion();
                string query = "Insert into Cobro_Externos (Id_Empleado,Fecha,Descripcion,Importe,Rancho) values (@Id,@Fecha,@Descripcion,@Importe,@Rancho)";
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Fecha", Fecha);
                    cmd.Parameters.AddWithValue("@Descripcion", Descripcion);
                    cmd.Parameters.AddWithValue("@Importe", Importe);
                    cmd.Parameters.AddWithValue("@Rancho", Rancho);                    
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            { Error = 1; }
            return Error;
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            ///Asistencia para empleados de PLANTA (TODOS)
            if (plantaToolStripMenuItem.Checked == true || desbroteMasToolStripMenuItem.Checked == true)
            {
                int NoEmpleado = int.Parse(label15.Text);
                DateTime Fecha = dateTimePicker2.Value;
                string Asistencia;
                int asistencia;
                int falta;
                int HE = 0;
                string Lote = comboBox1.Text;
                int Actividad = (int)comboBox2.SelectedValue;
                try { HE = int.Parse(textBox7.Text); }
                catch (Exception ex) { HE = 0; }
                if (checkBox1.Checked == true)
                { Asistencia = "Asistencia"; asistencia = 1; falta = 0; }
                else
                { Asistencia = "Falta"; asistencia = 0; falta = 1; Lote = "N/A"; Actividad = 0; HE = 0; }
                string Rancho = cbRancho.Text;
                if (ValidaAsistenciaRepetida(NoEmpleado, Fecha, "Asistencia_Planta") == 0)
                {
                    //ALMACENA LA ASISTENCIA CON LOS DATOS CORRESPONDIENTES
                    if (guardaAsistenciaPlanta(NoEmpleado, Fecha, Asistencia, HE, asistencia, falta, Actividad, Lote, Rancho) == 0)
                    { label2.Visible = true; timer1.Start(); }
                    else
                        MessageBox.Show("Hubo un error por favor revisa la informacion", "Sistema de Viñedos");
                }
                else
                {
                    DialogResult dr = MessageBox.Show("Ya ha guardado la asistencia del dia " + Fecha.ToString().Substring(0, 10) + ". Desea actualizarla?",
                        "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr.ToString() == "Yes")
                        ActualizaAsistencia(NoEmpleado, Fecha);
                }
            }
            ///Asistencia de Eventuales en el esquema de PODA
            else if (podaToolStripMenuItem.Checked == true)
            {
                int NoEmpleado = int.Parse(label15.Text);
                DateTime Fecha = dateTimePicker2.Value;
                int Plantas;
                double Precio = double.Parse(textBox10.Text);
                string Rancho = cbRancho.Text;
                string Lote = comboBox4.Text;
                int Actividad = int.Parse(textBox1.Text);
                int Asistencia, Falta;
                try { Plantas = int.Parse(textBox9.Text); Precio = double.Parse(textBox10.Text); }
                catch (Exception ex)
                { Plantas = 0; Precio = 0; }
                if (Plantas != 0)
                { Asistencia = 1; Falta = 0; }
                else
                { Asistencia = 0; Falta = 1; }
                if (ValidaAsistenciaRepetida(NoEmpleado, Fecha, "Asistencia_OD") == 0)
                {
                    if (guardaAsistenciaPodaCosecha(NoEmpleado, Fecha, Plantas, Precio, Asistencia, Falta, Actividad, Rancho, Lote) == 0)
                        MessageBox.Show("La asistencia del dia " + Fecha + " del empleado " +
                            NoEmpleado + " " + label16.Text + " ha sido almacenada correctamente", "Sistema de Viñedos");
                    else
                        MessageBox.Show("Hubo un error por favor revisa la informacion", "Sistema de Viñedos");
                }
                else
                    MessageBox.Show("Ocurrio un error en la validacion de datos, Por favor revise su informacion", "Sistema de Viñedos");

            }
            ///Asistencia para eventuales en el esquema de COSECHA
            else if (cosechaToolStripMenuItem.Checked == true)
            {
                int NoEmpleado = int.Parse(label15.Text);
                DateTime Fecha = dateTimePicker2.Value;
                int Toneladas;
                double Precio = double.Parse(textBox10.Text);
                string Rancho = cbRancho.Text;
                string Lote = comboBox4.Text;
                int Actividad = int.Parse(textBox1.Text);
                int Asistencia, Falta;
                try { Toneladas = int.Parse(textBox9.Text); Precio = double.Parse(textBox10.Text); }
                catch (Exception ex)
                { Toneladas = 0; Precio = 0; }
                if (Toneladas != 0)
                { Asistencia = 1; Falta = 0; }
                else
                { Asistencia = 0; Falta = 1; }
                if (ValidaAsistenciaRepetida(NoEmpleado, Fecha, "Asistencia_OD") == 0)
                {
                    if (guardaAsistenciaPodaCosecha(NoEmpleado, Fecha, Toneladas, Precio, Asistencia, Falta, Actividad, Rancho, Lote) == 0)
                        MessageBox.Show("La asistencia del dia " + Fecha + " del empleado " +
                            NoEmpleado + " " + label16.Text + " ha sido almacenada correctamente", "Sistema de Viñedos");
                    else
                        MessageBox.Show("Hubo un error por favor revisa la informacion", "Sistema de Viñedos");
                }
                else
                    MessageBox.Show("Ocurrio un error en la validacion de datos, Por favor revise su informacion", "Sistema de Viñedos");
            }
            ///Asistencias de Eventuales en el esquema de Rendimientos
            else if (desbroteMasToolStripMenuItem.Checked == true)
            {
                int NoEmpleado = int.Parse(label15.Text);
                DateTime Fecha = dateTimePicker2.Value;
                int Asistencia, Falta, Rendimiento, HE;
                string Rancho = cbRancho.Text, Lote = comboBox6.Text, Actividad = comboBox7.Text, Asist;
                try
                { HE = int.Parse(textBox12.Text); }
                catch (Exception ex)
                { HE = 0; }
                if (checkBox2.Checked == true)
                    Rendimiento = 1;
                else
                    Rendimiento = 0;
                if (checkBox3.Checked == true)
                { Asistencia = 1; Falta = 0; Asist = "Asistencia"; }
                else
                { Asistencia = 0; Falta = 1; Rendimiento = 0; HE = 0; Lote = ""; Actividad = ""; Asist = "Falta"; }
                if (ValidaAsistenciaRepetida(NoEmpleado, Fecha, "Asistencia_OD") == 0)
                {
                    if (guardaAsistenciaRendimiento(NoEmpleado, Fecha, Asist, Rendimiento, Asistencia, Falta, Actividad, Lote, Rancho) == 0)
                        MessageBox.Show("La asistencia del dia " + Fecha + " del empleado " +
                            NoEmpleado + " " + label16.Text + " ha sido almacenada correctamente", "Sistema de Viñedos");
                    else
                        MessageBox.Show("Hubo un error por favor revisa la informacion", "Sistema de Viñedos");
                }
                else
                    MessageBox.Show("Ocurrio un error en la validacion de datos, Por favor revise su informacion", "Sistema de Viñedos");
            }
            else if (jubiladosToolStripMenuItem.Checked == true)
            {
                int NoEmpleado = int.Parse(label15.Text);
                DateTime Fecha = dateTimePicker2.Value;
                int Asistencia, Falta, HE, Actividad = (int)comboBox10.SelectedValue;
                string Asist, Rancho = comboBox8.Text, Lote = comboBox9.Text;
                try
                { HE = int.Parse(textBox14.Text); }
                catch (Exception ex)
                { HE = 0; }
                if (checkBox4.Checked == true)
                { Asistencia = 1; Falta = 0; Asist = "Asistencia"; }
                else
                { Asistencia = 0; Falta = 1; Asist = "Falta"; HE = 0; Lote = ""; Actividad = 0; }
                if (ValidaAsistenciaRepetida(NoEmpleado, Fecha, "Asistencia_OD") == 0)
                {
                    if (guardaAsistenciaPlanta(NoEmpleado, Fecha, Asist, HE, Asistencia, Falta, Actividad, Lote, Rancho) == 0)
                    { label2.Visible = true; timer1.Start(); }
                    else
                        MessageBox.Show("Hubo un error por favor revisa la informacion", "Sistema de Viñedos");
                }
                else
                    MessageBox.Show("Ocurrio un error en la validacion de datos, Por favor revise su informacion", "Sistema de Viñedos");
            }

            else if (externosToolStripMenuItem.Checked == true)
            {
                int NoEmpleado = int.Parse(label15.Text);
                DateTime Fecha = dateTimePicker2.Value;
                string Rancho = comboBox3.Text;
                string Descripcion = "";
                double Importe = 0;
                try
                {
                    Importe = double.Parse(textBox8.Text);
                    Descripcion = richTextBox1.Text;
                }
                catch (Exception ex)
                { MessageBox.Show("Hubo un error, revisa tu informacion", "Sistema de Viñedos"); }
                if (ValidaAsistenciaRepetida(NoEmpleado, Fecha, "Cobro_Externos") == 0)
                {
                    if (guardaAsistenciaExterno(NoEmpleado, Fecha, Descripcion, Importe, Rancho) == 0)
                        MessageBox.Show("La asistencia del dia " + Fecha + " del empleado " +
                            NoEmpleado + " " + label16.Text + " ha sido almacenada correctamente", "Sistema de Viñedos");
                    else
                        MessageBox.Show("No fue posible almacenar la informacion, por favor revisala", "Sistema de Viñedos");
                }
                else
                    MessageBox.Show("Ocurrio un error en la validacion de datos, Por favor revise su informacion", "Sistema de Viñedos");
            }
               
      
            /*if (btnGuardar.Text == "Guardar")Mensaje de Aviso de Exito en el almacenamiento de la informacion en la DB
            {             
                MessageBox.Show("Lista Guardada Exitosamente!", "Mensaje", MessageBoxButtons.OK);                
            }*/
            ShowGridData(int.Parse(label15.Text));
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Seguro que deseas salir?", "Mensaje",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dr.ToString() == "Yes")            
                this.Close();           
        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int NumeroEmpleado = (Convert.ToInt32(grid1[0, grid1.SelectedCells[0].RowIndex].Value.ToString()));
                string NombreEmpleado = grid1[1, grid1.SelectedCells[0].RowIndex].Value.ToString();
                label15.Text = NumeroEmpleado.ToString();
                label16.Text = NombreEmpleado;
            }
            catch (Exception ex)
            { }
            finally
            {
                ShowGridData(int.Parse(label15.Text));
            }
        }

        private void eventualesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Para Eventuales           
           
            label4.Text = "Eventuales:Rendimiento:";
            label1.Text = "Rancho";           
            this.cbRancho.Size = new System.Drawing.Size(155, 21);           
            ban = 2;            
            grid1.Visible = false;           
            string query = "SELECT Clave,Rancho from R";
            clConexion objConexion = new clConexion();            
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cbRancho.DataSource = dt;
                    cbRancho.ValueMember = dt.Columns[0].ToString();
                    cbRancho.DisplayMember = dt.Columns[1].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void obraDeterminadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Asistencia_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Usuario: " + LogUsers.getUserName();            
            label4.Text = "";
            label2.Visible = false;
            obraDeterminadaToolStripMenuItem1.Visible = false;            
        }

        private void button1_Click(object sender, EventArgs e)
        {          
                                                            
        }
        /// <summary>
        ///Funciones Planta 
        /// </summary>
        public void cargaLotePlanta(string Rancho)
        {            
            string query = "SELECT Clave From Lotes where Rancho = '" + Rancho + "'";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox1.DataSource = dt;
                    comboBox1.ValueMember = dt.Columns[0].ToString();
                    comboBox1.DisplayMember = dt.Columns[0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void cargaActividadPlanta()
        {
            string query = "SELECT * From Actividad";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox2.DataSource = dt;
                    comboBox2.ValueMember = dt.Columns[0].ToString();
                    comboBox2.DisplayMember = dt.Columns[1].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        ///Termina Funciones Planta
        private void plantaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Para trabajadores de planta.            
            checkMenu(1);
            panel1.Visible = true;
            CargaRanchoGeneral();
            cargaLotePlanta(cbRancho.SelectedValue.ToString());
            cargaActividadPlanta();
            cbRancho.Visible = true;
            PanelAsistencia.Visible = true;
            habilitaPanel(1, 0);
            label4.Text = "Planta";
            label1.Text = "Rancho";                   
            this.cbRancho.Size = new System.Drawing.Size(155, 21);       
            ban = 1;            
            grid1.Visible = true;                 
            
        }

        private void eventualesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void obraDeterminadaToolStripMenuItem1_Click(object sender, EventArgs e)
        {           
        }


        /// FUNCIONES PODA Y COSECHA
        public void cargaLotePodaCosecha(string Rancho)
        {
            string query = "SELECT Clave From Lotes where Rancho = '" + Rancho + "'";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox4.DataSource = dt;
                    comboBox4.ValueMember = dt.Columns[0].ToString();
                    comboBox4.DisplayMember = dt.Columns[0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
        }
        public void cargaActividadPodaCosecha()
        {
            string query = "SELECT * From Actividad";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox5.DataSource = dt;
                    comboBox5.ValueMember = dt.Columns[0].ToString();
                    comboBox5.DisplayMember = dt.Columns[1].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// FIN DE FUNCIONES PODA Y COSECHA


        private void podaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkMenu(2);
            panel1.Visible = true;
            CargaRanchoGeneral();
            cargaActividadPodaCosecha();
            PanelAsistencia.Visible = true;
            habilitaPanel(2, 1);
            cbRancho.Visible = true;           
            //obraDeterminadaToolStripMenuItem1_Click(null, null);            
            ban = 2;
            grid1.Visible = true;     
        }

        private void cosechaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkMenu(3);
            panel1.Visible = true;
            CargaRanchoGeneral();
            cargaActividadPodaCosecha();
            PanelAsistencia.Visible = true;
            habilitaPanel(2, 2);
            cbRancho.Visible = true;            
            obraDeterminadaToolStripMenuItem1_Click(null, null);
            cosechaToolStripMenuItem.Checked = true;
            ban = 3;
            grid1.Visible = true;     
        }

        
        /// FUNCIONES PARA RENDIMIENTOS
        public void cargaLoteRendimiento(string Rancho)
        {
            string query = "SELECT Clave From Lotes where Rancho = '" + Rancho + "'";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox6.DataSource = dt;
                    comboBox6.ValueMember = dt.Columns[0].ToString();
                    comboBox6.DisplayMember = dt.Columns[0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void cargaActividadRendimiento()
        {
            string query = "SELECT * From Actividad";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox7.DataSource = dt;
                    comboBox7.ValueMember = dt.Columns[0].ToString();
                    comboBox7.DisplayMember = dt.Columns[1].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// Finaliza FUNCIONES RENDIMENTO
        

        private void desbroteMasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkMenu(4);
            panel1.Visible = true;
            CargaRanchoGeneral();
            cargaLotePlanta(cbRancho.SelectedValue.ToString());
            cargaActividadPlanta();
            cbRancho.Visible = true;
            PanelAsistencia.Visible = true;
            habilitaPanel(1, 0);
            label4.Text = "Eventuales";
            label1.Text = "Rancho";
            this.cbRancho.Size = new System.Drawing.Size(155, 21);
            ban = 1;
            grid1.Visible = true;      
            /*checkMenu(4);
            panel1.Visible = true;
            PanelAsistencia.Visible = true;
            habilitaPanel(3, 0);
            eventualesToolStripMenuItem_Click(null, null);
            desbroteMasToolStripMenuItem.Checked = true;            
            cargaActividadRendimiento();
            ban = 4;
            grid1.Visible = true;  */   
        }
        
        /// Funciones Externos        
        public void cargaRanchoExternos()
        {
            string query = "SELECT Clave,Rancho from R";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox3.DataSource = dt;
                    comboBox3.ValueMember = dt.Columns[0].ToString();
                    comboBox3.DisplayMember = dt.Columns[1].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        
        /// Termina Funciones Externos
        
        
        

        private void externosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkMenu(6);
            panel1.Visible = false;
            if (jubiladosToolStripMenuItem.Checked == true)
                cargaEmpleadosGrid("J", "");
            else if (externosToolStripMenuItem.Checked == true)
                cargaEmpleadosGrid("EX", "");
            cargaRanchoExternos();
            PanelAsistencia.Visible = true;
            habilitaPanel(5, 0);
            CargaExternos();
            btnGuardar.Visible = true;
            btnCancelar.Visible = true;                       
            ban = 6;
            grid1.Visible = true;     
        }

        private void cbExternos_SelectedIndexChanged(object sender, EventArgs e)
        { }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        { }
        
        /// Funciones de Jubilados        
        public void cargaRanchoJubilados()
        {
            string query = "SELECT Clave,Rancho from R";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox8.DataSource = dt;
                    comboBox8.ValueMember = dt.Columns[0].ToString();
                    comboBox8.DisplayMember = dt.Columns[1].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void cargaLoteActividad(string Rancho)
        {
            string query = "SELECT Clave From Lotes where Rancho = '" + Rancho + "'";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox9.DataSource = dt;
                    comboBox9.ValueMember = dt.Columns[0].ToString();
                    comboBox9.DisplayMember = dt.Columns[0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void cargaActividades()
        {
            string query = "SELECT Clave,Actividad from Actividad";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox10.DataSource = dt;
                    comboBox10.ValueMember = dt.Columns[0].ToString();
                    comboBox10.DisplayMember = dt.Columns[1].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        
        /// Termina Funciones de Jubilados        
        private void jubiladosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkMenu(5);
            panel1.Visible = false;
            if (jubiladosToolStripMenuItem.Checked == true)
                cargaEmpleadosGrid("P");       //Cargara a los empleados que SEAN DIFERENTES A PLANTA    
            PanelAsistencia.Visible = true;
            habilitaPanel(4, 0);
            cargaRanchoJubilados();
            cargaActividades();
            grid1.Visible = true;     
        }        

        private void radioButton5_CheckedChanged_1(object sender, EventArgs e)
        {
            checkBox4.Enabled = true;
            radioButton6.Checked = false;
            textBox13.Text = "";
            textBox13.Enabled = false;
        }

        private void radioButton6_CheckedChanged_1(object sender, EventArgs e)
        {
            radioButton5.Checked = false;
            checkBox4.Enabled = false;
            textBox13.Enabled = true;
            textBox13.Text = "";
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargaLoteActividad(comboBox8.SelectedValue.ToString());
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked = false;
            checkBox1.Enabled = true;
            textBox6.Enabled = false;
            textBox6.Text = "";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            checkBox1.Enabled = false;
            textBox6.Enabled = true;
            textBox6.Text = "";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Enabled = true;
            radioButton4.Checked = false;
            textBox11.Text = "";
            textBox11.Enabled = false;            
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Enabled = false;
            radioButton3.Checked = false;
            textBox11.Text = "";
            textBox11.Enabled = true;
        }

        private void grid1_SelectionChanged(object sender, EventArgs e)
        {
            grid1_CellContentClick(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (reloj == 2)
            {
                reloj = 0;
                label2.Visible = false;
                timer1.Stop();
            }
            reloj += 1;
        }    
    }
}
