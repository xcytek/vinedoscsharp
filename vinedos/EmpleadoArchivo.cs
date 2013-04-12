using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Data.Sql;

namespace vinedos
{
    public partial class EmpleadoArchivo : Form
    {
        public EmpleadoArchivo()
        {
            InitializeComponent();
        }

        public void GuardaListaEmpleados(int NoEmpleado, string Paterno, string Materno, string Nombre, string Direccion, string Residencia, string Sexo, DateTime FechaNacimiento, string LugarNacimiento,
            string IMSS, string RFC, string CURP, DateTime FechaIngreso, string Puesto, string Tipo, string Rancho)
        {
            clConexion objConexion = new clConexion();
            string query = "INSERT INTO Empleados (Id, Paterno, Materno, Nombre, Direccion, Sexo, Fechanac, Estado, Ciudad, Nss, Rfc, Curp, Fechaing, Puesto, Tipo, Rancho) VALUES " +
                "(@Id, @Paterno, @Materno, @Nombre, @Direccion, @Sexo, @Fechanac, @Estado, @Ciudad, @Nss, @Rfc, @Curp, @Fechaing, @Puesto, @Tipo, @Rancho)";
            using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", NoEmpleado);
                cmd.Parameters.AddWithValue("@Paterno", Paterno);
                cmd.Parameters.AddWithValue("@Materno", Materno);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Direccion", Direccion);
                cmd.Parameters.AddWithValue("@Sexo", Sexo);
                cmd.Parameters.AddWithValue("@Fechanac", FechaNacimiento);
                cmd.Parameters.AddWithValue("@Estado", LugarNacimiento);
                cmd.Parameters.AddWithValue("@Ciudad", Residencia);
                cmd.Parameters.AddWithValue("@Nss", IMSS);
                cmd.Parameters.AddWithValue("@Rfc", RFC);
                cmd.Parameters.AddWithValue("@Curp", CURP);
                cmd.Parameters.AddWithValue("@Fechaing", FechaIngreso);
                cmd.Parameters.AddWithValue("@Puesto", Puesto);
                cmd.Parameters.AddWithValue("@Tipo", Tipo);
                cmd.Parameters.AddWithValue("@Rancho", Rancho);
                cmd.Connection = objConexion.conexion();
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
            

        public void CreaColumna(string NameColumna,string HeaderColumna)
        {           
            DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
            column.Name = NameColumna;
            column.HeaderText = HeaderColumna;
            //DataGridViewCell cell = new DataGridViewTextBoxCell();
            DataGridViewCell cell = new DataGridViewComboBoxCell();
            cell.Style.BackColor = Color.White;             
            column.CellTemplate = cell;
            column.FlatStyle = FlatStyle.Popup; 
            dataGridView1.Columns.Add(column);
            if (NameColumna == "NewPuesto")
                cargaPuestos(column);
            else if (NameColumna == "Rancho")
                cargaRancho(column);
            else
                column.Items.AddRange("P", "E", "J", "EX");
            
        }

        public void cargaRancho(DataGridViewComboBoxColumn column)
        {
            string queryPuesto = "Select Clave,Rancho from R";
            clConexion objConexion = new clConexion();

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(queryPuesto, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    column.DataSource = dt;
                    column.ValueMember = dt.Columns[1].ToString();
                    column.DisplayMember = dt.Columns[1].ToString();
                }

            }
            catch (Exception ex)
            { }
        }



        public void cargaPuestos(DataGridViewComboBoxColumn column)
        {
            string queryPuesto = "Select Clave,Puesto from Puestos";
            clConexion objConexion = new clConexion();

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(queryPuesto, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    column.DataSource = dt;
                    column.ValueMember = dt.Columns[1].ToString();
                    column.DisplayMember = dt.Columns[1].ToString();
                }

            }
            catch (Exception ex)
            { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            int bandera = 0;
            int contador = 0;
            string[] campos;
            string filepath = "";
            List<PadronEmpleados> objPadron = new List<PadronEmpleados>();            
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "csv files (*.csv)|*.csv";
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    filepath = openFileDialog1.FileName;
                String linea;
                StreamReader f = new StreamReader(filepath);
                while ((linea = f.ReadLine()) != null)
                {
                    campos = linea.Split(',');
                    try
                    {
                        objPadron.Add(new PadronEmpleados(campos[0], campos[1], campos[2], campos[3], campos[4], campos[5], campos[6], campos[7], campos[8], campos[9]
                            , campos[10], campos[11], campos[12], campos[13]));                        
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex)
            { bandera = 1; }
            finally
            {
                if (bandera == 0)
                {                    
                    ///
                    //cargar el Grid con el List Padron Empleados                         
                    ///PONER UN FOREACH DONDE SE COMPARE CON UN DATAREADER DE LA BASE DE DATOS DE LA TABLA EMPLEADOS
                    ///PARA VALIDAR QUE NO CARGUE A EMPLEADOS QUE YA ESTAN DADOS DE ALTA EN EL SISTEMA 
                    ///CAMPOS A VALIDAR:
                    ///No Empleado (Primordial), Paterno Materno Nombre
                    ///

                    clConexion objConexion = new clConexion();
                    List<PadronEmpleados> objPadronAux = new List<PadronEmpleados>();
                    List<PadronEmpleados> objPadronAux2 = new List<PadronEmpleados>();
                    string[] PadronArr = new string[500];                    
                    string Query = "SELECT Id FROM Empleados";
                    int j = 0;                
                    using (SqlCommand cmd = new SqlCommand(Query, objConexion.conexion()))
                    {
                        SqlDataReader dr;
                        cmd.Connection = objConexion.conexion();
                        cmd.Connection.Open();
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows == true)
                            while (dr.Read())
                            { PadronArr[j] = dr[0].ToString(); j += 1; }
                    }                    
                    foreach (PadronEmpleados empleado in objPadron)
                    {
                        int Repeticiones = 0;
                        for (int i = 0; i < PadronArr.Length; i++)
                            if (empleado.NoEmpleado == PadronArr[i])
                            { Repeticiones += 1; break; }
                        if (Repeticiones == 0)
                        {
                            objPadronAux.Add(new PadronEmpleados(empleado.NoEmpleado, empleado.Paterno, empleado.Materno, empleado.Nombre, empleado.Direccion, empleado.Residencia, empleado.Sexo,
                               empleado.FechaNacimiento, empleado.LugarNacimiento, empleado.IMSS, empleado.RFC, empleado.CURP, empleado.FechaIngreso, empleado.Puesto));
                            contador++;
                        }
                    }
                    MessageBox.Show("Archivo cargado correctamente. Total de Registros: " + (contador), "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.DataSource = objPadronAux;
                    CreaColumna("NewPuesto", "Nuevo Puesto");
                    CreaColumna("Tipo", "Tipo Empleado");
                    CreaColumna("Rancho", "Rancho");
                    button2.Enabled = true;
                }
                else if (bandera == 1)
                    MessageBox.Show("No se puede leer el archivo", "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int n = 0, bandera = 0;
             foreach (DataGridViewRow row in dataGridView1.Rows)
             {
                 try
                 {
                     GuardaListaEmpleados(int.Parse(dataGridView1.Rows[n].Cells[0].Value.ToString()), dataGridView1.Rows[n].Cells[1].Value.ToString(), dataGridView1.Rows[n].Cells[2].Value.ToString(), dataGridView1.Rows[n].Cells[3].Value.ToString(),
                         dataGridView1.Rows[n].Cells[4].Value.ToString(), dataGridView1.Rows[n].Cells[5].Value.ToString(), dataGridView1.Rows[n].Cells[6].Value.ToString(), new DateTime(1900, 01, 01),
                         dataGridView1.Rows[n].Cells[8].Value.ToString(), dataGridView1.Rows[n].Cells[9].Value.ToString(), dataGridView1.Rows[n].Cells[10].Value.ToString(), dataGridView1.Rows[n].Cells[11].Value.ToString(),
                         new DateTime(1900, 01, 01), dataGridView1.Rows[n].Cells[14].Value.ToString(), "P", "-");
                     bandera += 1;
                 }
                 catch (Exception ex)
                 { }
                 finally
                 { n += 1; }
             }
             MessageBox.Show("Se registraron " + bandera + " nuevos empleados", "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
