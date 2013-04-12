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
    public partial class KardexEmpleado : Form
    {
        public KardexEmpleado()
        {
            InitializeComponent();            
        }

        //FUNCION PARA EXTRAER LA INFO DEL EMPLEADO (TRABAJO)
        public void LoadEmpInfoRanch(int NoEmpleado)
        {
            string[] queryPart = {"Fecha, Asist, HE, Actividad, Lote1", "Fecha, Asist, Rendimiento, Actividad, Lote1","Fecha, Plantas, Precio, Actividad, Lote1","Fecha, Descripcion, Importe, Rancho"};
            string[] queryPart2 = { "Asistencia_Planta_Resp", "Asistencia_Eventual_Resp", "Asistencia_OD_Resp", "Cobro_Externos_Resp" };            
            clConexion objConexion = new clConexion();
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    string query = "SELECT " + queryPart[i] + " FROM " + queryPart2[i] + " WHERE Id_Empleado = " + NoEmpleado+" ORDER BY Fecha";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                    {                        
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                        if (dt.Rows.Count != 0)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        ///*****************************************************************
        ///*****************************************************************
        ///*****************************************************************
        ///***********FUNCION PARA CARGAR INFORMACION EN EL KARDEX**********
        ///*****************************************************************
        ///*****************************************************************
        ///*****************************************************************
        ///*****************************************************************
        public void LoadKardexInfo(int NoEmpleado)
        {
            string query = "SELECT Rancho, Fechaing, Puesto, Foto, Zona, Sexo FROM Empleados WHERE Id = " + NoEmpleado;
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
                            DateTime FechaIngreso = new DateTime();
                            string Fecha_Ingreso = "";                           
                            txtRancho.Text = "Rancho: " + dr[0].ToString();
                            FechaIngreso = Convert.ToDateTime(dr[1].ToString());
                            Fecha_Ingreso = FechaIngreso.ToString("F", new System.Globalization.CultureInfo("es-ES"));                            
                            txtFechaIngreso.Text = "Fecha Ingreso: " + Fecha_Ingreso.Substring(0, Fecha_Ingreso.Length - 7);
                            txtPuesto.Text = "Puesto: " + dr[2].ToString();
                            txtZona.Text = "Zona: " + dr[4].ToString();
                            if (dr[3].ToString() == "")
                                if (dr[5].ToString() == "M")
                                    pictureBox1.ImageLocation = @"C:/VinedosImages/male-icon.jpg";
                                else if (dr[5].ToString() == "F")
                                    pictureBox1.ImageLocation = @"C:/VinedosImages/female-icon.jpg";
                            LoadEmpInfoRanch(NoEmpleado);
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //BUSQUEDA DE EMPLEADO ATRAVES DE PALABRAS
        public void WordSearchFunction(string SearchQuery)
        {
            Funciones objFunciones = new Funciones();
            List<SearchFunction> objSearchFunction = new List<SearchFunction>();
            List<SearchFunction> AuxSearchFunction = new List<SearchFunction>();
            objSearchFunction = objFunciones.LoadEmp(objSearchFunction);
            foreach (SearchFunction objSF in objSearchFunction)
            {
                try
                {                    
                    if (SearchQuery == objSF.NoEmpleado || SearchQuery == objSF.Paterno || SearchQuery == objSF.Materno || SearchQuery == objSF.Nombre
                        || SearchQuery == objSF.NoEmpleado + " " + objSF.Paterno || SearchQuery == objSF.NoEmpleado + " " + objSF.Materno || SearchQuery == objSF.NoEmpleado + " " + objSF.Nombre
                        || SearchQuery == objSF.Paterno + " " + objSF.Materno || SearchQuery == objSF.Paterno + " " + objSF.Nombre || SearchQuery == objSF.Materno + " " + objSF.Nombre
                        || SearchQuery == objSF.NoEmpleado + " " + objSF.Paterno + " " + objSF.Materno || SearchQuery == objSF.NoEmpleado + " " + objSF.Paterno + " " + objSF.Nombre
                        || SearchQuery == objSF.NoEmpleado + " " + objSF.Materno + " " + objSF.Nombre || SearchQuery == objSF.Paterno + " " + objSF.Materno + " " + objSF.Nombre
                        || SearchQuery == objSF.NoEmpleado + " " + objSF.Paterno + " " + objSF.Materno + " " + objSF.Nombre)
                        //SI LO ESCRITO EN EL TEXTBOX COINCIDE CON CUALQUIER COMBINACION MUESTRA EN EL GRID LOS RESULTADOS QUE COINCIDAN
                        AuxSearchFunction.Add(new SearchFunction(objSF.NoEmpleado, objSF.Paterno, objSF.Materno, objSF.Nombre));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            dataGridView2.DataSource = AuxSearchFunction;               
        }

        public void cargaEmpleados()
        {
            string query = "SELECT Id,Paterno, Materno, Nombre FROM EMPLEADOS ORDER BY Paterno ASC";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            
        }

        private void KardexEmpleado_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Usuario: " + LogUsers.getUserName();
            cargaEmpleados();
            textBox1.Select();
            radioButton1.Checked = true;
            radioButton1_CheckedChanged(null, null);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //ACTUALIZA EL GRID A PARTIR DE LA BUSQUEDA
            if (textBox1.Text == "")
                cargaEmpleados();
            else
                WordSearchFunction(textBox1.Text);
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           //CARGA LA INFO A PARTIR DEL NUMERO DE EMPLEADO QUE SEA SELACCIONADO EN LA LISTA.
            int NumeroEmpleado = 0;
            try
            {
                NumeroEmpleado = (Convert.ToInt32(dataGridView2[0, dataGridView2.SelectedCells[0].RowIndex].Value.ToString()));
                string NombreEmpleado = dataGridView2[1, dataGridView2.SelectedCells[0].RowIndex].Value.ToString() + " " + dataGridView2[2, dataGridView2.SelectedCells[0].RowIndex].Value.ToString()
                    + " " + dataGridView2[3, dataGridView2.SelectedCells[0].RowIndex].Value.ToString();
                txtNoEmpleado.Text = NumeroEmpleado.ToString();
                txtNombre.Text = NombreEmpleado;
            }
            catch (Exception ex)
            { }
            finally
            {
                //FUNCION PARA CARGAR LA INFORMACION EN EL KARDEX DEL EMPLEADO
                LoadKardexInfo(NumeroEmpleado);
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            //INVOCA AL EVENTO dataGridView2_CellContentClick() 
            dataGridView2_CellContentClick(null, null);
        }

        private void KardexEmpleado_Click(object sender, EventArgs e)
        {
            textBox1_Click(null, null);
        }

        private void actividadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DescripcionActividades frmActividades = new DescripcionActividades();
            frmActividades.ShowDialog();            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            comboBox1.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = false;
            comboBox1.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
            comboBox1.Enabled = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Select();
            textBox1.SelectAll();
        }
    }
}
