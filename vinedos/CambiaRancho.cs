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
    public partial class CambiaRancho : Form
    {
        public CambiaRancho()
        {
            InitializeComponent();
            
        }
        Funciones fn = new Funciones();
        private void CambiaRancho_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Usuario: " + LogUsers.getUserName();
            cargaCombo_Activar_Baja();
            CargaComboRancho();            
        }
        //*******************************************************************************************************\\
        //FUNCIONES PARA PESTAÑA "CUADRILLA"
        /// Variables
        
        
        /// Metodos
        /// 
        public void CargaComboRancho()
        {
            clConexion objConexion = new clConexion();
            string query = "SELECT Id, Cuadrilla from Cuadrillas";            
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

        public void CargaGridCuadrillas()
        {
            dataGridView1.Rows.Clear();
            string query = "SELECT Id,Paterno,Materno,Nombre,Rancho FROM Empleados WHERE Cuadrilla='" + cbRancho.Text + "' AND "+varQuery+" Status = 1 ORDER BY Paterno ASC";
            string query2 = "SELECT Rancho From R ";
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
                            int n;
                            string nomCompleto = dr[1].ToString() + " " + dr[2].ToString() + " " + dr[3].ToString();
                            n = dataGridView1.Rows.Add();
                            dataGridView1.Rows[n].Cells[0].Value = dr[0].ToString();
                            dataGridView1.Rows[n].Cells[1].Value = nomCompleto;
                            dataGridView1.Rows[n].Cells[2].Value = dr[4].ToString();
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query2, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    Column3.DataSource = dt;
                    Column3.ValueMember = dt.Columns[0].ToString();
                    Column3.DisplayMember = dt.Columns[0].ToString();
                    cbRDestino.DataSource = dt;
                    cbRDestino.ValueMember = dt.Columns[0].ToString();
                    cbRDestino.DisplayMember = dt.Columns[0].ToString();
                }
            }

            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void ActualizaRancho(int Id,string Rancho)
        {
            string update = "UPDATE Empleados SET Rancho = @Rancho WHERE Id = " + Id;
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand updatecmd = new SqlCommand(update, objConexion.conexion()))
                {
                    updatecmd.Parameters.AddWithValue("@Rancho", Rancho);
                    updatecmd.Connection = objConexion.conexion();
                    updatecmd.Connection.Open();
                    updatecmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }        
        }

        public void ActualizaRanchodeCuadrilla(int Cuadrilla, string Rancho)
        {
            string update = "";
            if (eventualesToolStripMenuItem.Checked == true)
                update = "UPDATE Cuadrillas SET RanchoEventual = @Rancho WHERE Id = " + Cuadrilla;
            if (plantaToolStripMenuItem.Checked == true)
                update = "UPDATE Cuadrillas SET Rancho = @Rancho WHERE Id = " + Cuadrilla;
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand updatecmd = new SqlCommand(update, objConexion.conexion()))
                {
                    updatecmd.Parameters.AddWithValue("@Rancho", Rancho);
                    updatecmd.Connection = objConexion.conexion();
                    updatecmd.Connection.Open();
                    updatecmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }     
        }

        //Eventos
        private void cbRancho_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CargaGridCuadrillas();
            List<ListCuadrillas> objCuadrillas = new List<ListCuadrillas>();
            objCuadrillas = fn.CargaInfoCuadrilla(objCuadrillas);
            foreach (ListCuadrillas cuadrilla in objCuadrillas)
            {
                string cuadri = Convert.ToString(cuadrilla.Cuadrilla);
                if(eventualesToolStripMenuItem.Checked==true)
                    if (cuadri == cbRancho.Text)
                    {
                        lbRancho.Text = cuadrilla.RanchoEventual;
                        lbSupervisor.Text = Convert.ToString(cuadrilla.SupNombreEventual);
                    }
                    else { }
                 else if(plantaToolStripMenuItem.Checked==true)
                    if (cuadri == cbRancho.Text)
                    {
                        lbRancho.Text = cuadrilla.Rancho;
                        lbSupervisor.Text = Convert.ToString(cuadrilla.SupNombre);
                    }
                    else { }

            }
        }

        private void btnAsignSup_Click(object sender, EventArgs e)
        {
            try
            {
                int Id = 0;
                string Nombre = "";
                Id = (Convert.ToInt32(dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString()));
                Nombre = dataGridView1[1, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
                if (Id == 0)
                    MessageBox.Show("Debe seleccionar a un empleado para asignarlo como supervisor", "Aviso");
                else
                    //Asigna al Empleado como supervisor de la cuadrilla
                    asignaSupervisor(Id, Nombre, int.Parse(cbRancho.Text));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioIndividual_CheckedChanged(object sender, EventArgs e)
        {
            cbRDestino.Enabled = false;
            Column3.ReadOnly = false;
        }

        private void radioTodos_CheckedChanged(object sender, EventArgs e)
        {
            cbRDestino.Enabled = true;
            Column3.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioIndividual.Checked == true)
            {
                int n = 0, x = 0;
                string Rancho = "";
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int id = int.Parse(dataGridView1.Rows[n].Cells[0].Value.ToString());
                    if (dataGridView1.Rows[n].Cells[3].Value != null)
                    {
                        Rancho = dataGridView1.Rows[n].Cells[3].Value.ToString();
                        x += 1;
                        ActualizaRancho(id, Rancho);
                    }                   
                    n += 1;
                }
                MessageBox.Show("Se Actualizaron " + x + " Registros");
            }
            else if (radioTodos.Checked == true)
            {
                int n = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int id = int.Parse(dataGridView1.Rows[n].Cells[0].Value.ToString());
                    string Rancho = cbRDestino.Text;
                    ActualizaRancho(id, Rancho);
                    ActualizaRanchodeCuadrilla(int.Parse(cbRancho.Text), Rancho);
                    lbRancho.Text = Rancho;
                    n += 1;
                }
                MessageBox.Show("Se Actualizaron " + n + " Registros");
            }
            else
                MessageBox.Show("Debe Seleccionar una Opcion", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        //*******************************************************************************************************\\
        ///FUNCIONES PARA PESTAÑA "CAMBIO RANCHOS"
        ///
        ///Variables
        string queryGral = "";
        string varQuery = "Id = 0 AND";
        int intRancho = 0;
        int intCuadrilla = 0;
        //Metodos
        public void fnActualizarListas(int NoEmpleado,string Rancho)
        {
            string update="";            
            int cuad;
            string ranchito="";
            if (intRancho == 1)            
                update = "UPDATE Empleados SET Rancho = @Rancho, Cuadrilla = 0 WHERE Id = " + NoEmpleado;
            if (intCuadrilla == 1)
            {//Verifica a cual cuadrilla se mandara y obtiene el rancho para tambien actualizarlo
                List<ListCuadrillas> objCuadrillas = new List<ListCuadrillas>();
                objCuadrillas = fn.CargaInfoCuadrilla(objCuadrillas);
                foreach (ListCuadrillas cuadrilla in objCuadrillas)
                {
                    string cuadri = Convert.ToString(cuadrilla.Cuadrilla);
                    if (cuadri == Rancho)
                        ranchito = cuadrilla.Rancho;
                }
                update = "UPDATE Empleados SET Rancho = '" + ranchito + "', Cuadrilla = @Rancho WHERE Id = " + NoEmpleado;
                cuad = int.Parse(Rancho);
            }
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand updatecmd = new SqlCommand(update, objConexion.conexion()))
                {
                    updatecmd.Parameters.AddWithValue("@Rancho", Rancho);                    
                    updatecmd.Connection = objConexion.conexion();
                    updatecmd.Connection.Open();
                    updatecmd.ExecuteNonQuery();     
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }        
        }
        //Eventos
        private void radioRancho1_CheckedChanged(object sender, EventArgs e)
        {
            intRancho = 1;
            intCuadrilla = 0;
            if (listCambio2.Text != "")
                btnIzquierda.Enabled = true;
            queryGral = "SELECT Id, Paterno, Materno, Nombre from Empleados WHERE Status = 1 AND " + varQuery + " Rancho=";
            clConexion objConexion = new clConexion();
            string query = "SELECT Clave,Rancho from R";
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cbCambio1.DataSource = dt;
                    cbCambio1.ValueMember = dt.Columns[0].ToString();
                    cbCambio1.DisplayMember = dt.Columns[1].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void radioCuadrilla1_CheckedChanged(object sender, EventArgs e)
        {
            intRancho = 0;
            intCuadrilla = 1;
            if (listCambio2.Text != "")
                btnIzquierda.Enabled = true;
            queryGral = "SELECT Id, Paterno, Materno, Nombre from Empleados WHERE Status = 1 AND " + varQuery + " Cuadrilla=";
            clConexion objConexion = new clConexion();
            string query = "SELECT Id, Cuadrilla from Cuadrillas";
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);                    
                    cbCambio1.DataSource = dt;
                    cbCambio1.ValueMember = dt.Columns[0].ToString();
                    cbCambio1.DisplayMember = dt.Columns[1].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioRancho2_CheckedChanged(object sender, EventArgs e)
        {
            intRancho = 1;
            intCuadrilla = 0;
            if (ListCambio1.Text != "")
                btnDerecha.Enabled = true;
            queryGral = "SELECT Id, Paterno, Materno, Nombre from Empleados WHERE Status = 1 AND " + varQuery + " Rancho=";
            clConexion objConexion = new clConexion();
            string query = "SELECT Clave,Rancho from R";
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cbCambio2.DataSource = dt;
                    cbCambio2.ValueMember = dt.Columns[0].ToString();
                    cbCambio2.DisplayMember = dt.Columns[1].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void radioCuadrilla2_CheckedChanged(object sender, EventArgs e)
        {
            intRancho = 0;
            intCuadrilla = 1;
            if (ListCambio1.Text != "")
                btnDerecha.Enabled = true;
            queryGral = "SELECT Id, Paterno, Materno, Nombre from Empleados WHERE Status = 1 AND " + varQuery + " Cuadrilla=";
            clConexion objConexion = new clConexion();
            string query = "SELECT Id, Cuadrilla from Cuadrillas";
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cbCambio2.DataSource = dt;
                    cbCambio2.ValueMember = dt.Columns[0].ToString();
                    cbCambio2.DisplayMember = dt.Columns[1].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void cbCambio1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListCambio1.Items.Clear();
            clConexion objConexion = new clConexion();
            try
            {
                //Listar Empleados segun el Rancho seleccionado\\ 
                using (SqlCommand cmd = new SqlCommand(queryGral + "'" + cbCambio1.Text + "' ORDER BY Id", objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                        while (dr.Read())
                        {
                            //ListCambio1.Items.AddRange(new object[] { dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString() });
                            string cadena = dr[0].ToString() + "  " + dr[1].ToString() + "  " + dr[2].ToString() + "  " + dr[3].ToString();
                            ListCambio1.Items.Add(cadena);

                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }               

        private void cbCambio2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listCambio2.Items.Clear();
            clConexion objConexion = new clConexion();
            try
            {
                //Listar Empleados segun el Rancho seleccionado\\ 
                using (SqlCommand cmd = new SqlCommand(queryGral + "'" + cbCambio2.Text + "' ORDER BY Id", objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                        while (dr.Read())
                        {
                            string cadena = dr[0].ToString() + "  " + dr[1].ToString() + "  " + dr[2].ToString() + "  " + dr[3].ToString();
                            listCambio2.Items.Add(cadena);

                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListCambio1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnBorrar.Enabled = true;
            if (cbCambio2.Text != "")
                btnDerecha.Enabled = true;
        }

        private void listCambio2_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnBorrar2.Enabled = true;
            if (cbCambio1.Text != "")
                btnIzquierda.Enabled = true;
        }

        private void btnDerecha_Click(object sender, EventArgs e)
        {
            if (cbCambio1.Text != cbCambio2.Text)
            {
                listCambio2.Items.Add(ListCambio1.Text);
                ListCambio1.Items.Remove(ListCambio1.SelectedItem);
                btnDerecha.Enabled = false;
            }
        }

        private void btnIzquierda_Click(object sender, EventArgs e)
        {
            if (cbCambio1.Text != cbCambio2.Text)
            {
                ListCambio1.Items.Add(listCambio2.Text);
                listCambio2.Items.Remove(listCambio2.SelectedItem);
                btnIzquierda.Enabled = false;
            }
        }

        private void plantaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            varQuery = "Id < 50000 AND";
            tabControl1.Enabled = true;
            plantaToolStripMenuItem.Checked = true;
            eventualesToolStripMenuItem.Checked = false;
        }

        private void eventualesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            varQuery = "Id >= 50000 AND";
            tabControl1.Enabled = true;
            eventualesToolStripMenuItem.Checked = true;
            plantaToolStripMenuItem.Checked = false;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string cadena = "";
            try
            {
                foreach (string item in ListCambio1.Items)
                {
                    cadena = item.Substring(0, 5);
                    fnActualizarListas(int.Parse(cadena), cbCambio1.Text);
                }
                foreach (string item in listCambio2.Items)
                {
                    cadena = item.Substring(0, 5);
                    fnActualizarListas(int.Parse(cadena), cbCambio2.Text);
                }
                MessageBox.Show("Hecho");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            ListCambio1.Items.Remove(ListCambio1.SelectedItem);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listCambio2.Items.Remove(listCambio2.SelectedItem);
        }
        //*******************************************************************************************************\\
        //FUNCIONES PARA PESTAÑA "ACTIVAR"
        //variables               

        //Metodos
        public void cargaGridActivar_Baja(string radioVar)
        {
            dataGridView2.Rows.Clear();
            string query = "SELECT Id,Paterno,Materno,Nombre,Status FROM Empleados WHERE "+varQuery+" Rancho='" + comboRancho.Text + "'"+ radioVar +"ORDER BY Paterno ASC";            
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
                            int n;
                            string nomCompleto = dr[1].ToString() + " " + dr[2].ToString() + " " + dr[3].ToString();

                            n = dataGridView2.Rows.Add();
                            dataGridView2.Rows[n].Cells[0].Value = dr[0].ToString();
                            dataGridView2.Rows[n].Cells[1].Value = nomCompleto;
                            dataGridView2.Rows[n].Cells[2].Value = dr[4].ToString();
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void actulizaEstado(int id, string Rancho, int status)
        {
            string update = "UPDATE Empleados SET Status = @Status WHERE Id = " + id+" AND Rancho = '"+Rancho+"'";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand updatecmd = new SqlCommand(update, objConexion.conexion()))
                {
                    updatecmd.Parameters.AddWithValue("@Status", status);
                    updatecmd.Connection = objConexion.conexion();
                    updatecmd.Connection.Open();
                    updatecmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }        
        }

        public int validaCeldasEstado()
        {
            int n = 0, band = 0;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                int id = int.Parse(dataGridView2.Rows[n].Cells[0].Value.ToString());
                int status = int.Parse(dataGridView2.Rows[n].Cells[2].Value.ToString());
                if (status == 1 || status == 0)
                    band = 1;
                else
                { band = 0; break; }
                n += 1;
            }
            return band;
        }

        public void cargaCombo_Activar_Baja()
        {
            clConexion objConexion = new clConexion();
            string query = "SELECT Clave,Rancho from R";
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboRancho.DataSource = dt;
                    comboRancho.ValueMember = dt.Columns[0].ToString();
                    comboRancho.DisplayMember = dt.Columns[1].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void asignaSupervisor(int Id,string Nombre, int Cuadrilla)
        {
            string update="";
            if (Id < 50000)
                //Cuadrilla Planta
                update = "UPDATE Cuadrillas SET Supervisor = @Id, SupNombre = @Nombre WHERE Id = " + Cuadrilla;
            else if (Id >= 50000)
                //Cuadrilla Eventual
                update = "UPDATE Cuadrillas SET SupervisorEventual = @Id, SupNombreEventual = @Nombre WHERE Id = " + Cuadrilla;

                
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand updatecmd = new SqlCommand(update, objConexion.conexion()))
                {
                    updatecmd.Parameters.AddWithValue("@Id", Id);
                    updatecmd.Parameters.AddWithValue("@Nombre", Nombre);
                    updatecmd.Connection = objConexion.conexion();
                    updatecmd.Connection.Open();
                    updatecmd.ExecuteNonQuery();
                }
                MessageBox.Show("Supervisor Asignado");
                lbSupervisor.Text = Nombre;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }    
        }

        //Eventos
        private void tabPage2_Click(object sender, EventArgs e)
        {          
        }

        private void comboRancho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioActivo.Checked == true)
                cargaGridActivar_Baja(" AND Status = 1 ");
            else if (radioInactivo.Checked == true)
                cargaGridActivar_Baja(" AND Status = 0 ");
            else
                cargaGridActivar_Baja("");//manda "" si no esta seleccionado ningun radiobutton, muestra empleados Activos e Inactivos
                                      
        }  

        private void radioInactivo_CheckedChanged(object sender, EventArgs e)
        {
            string radioVar = " AND Status = 0 ";
            cargaGridActivar_Baja(radioVar);
        }

        private void radioActivo_CheckedChanged(object sender, EventArgs e)
        {
            string radioVar = " AND Status = 1 ";
            cargaGridActivar_Baja(radioVar);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int n = 0;
            int validacion = validaCeldasEstado();
            if (validacion == 0)
                MessageBox.Show("Uno o mas empleados tienen un Estado no valido, verifique que sea Activo = 1 o Inactivo = 0","Error Estado Empleados");
            else
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    int id = int.Parse(dataGridView2.Rows[n].Cells[0].Value.ToString());
                    int status = int.Parse(dataGridView2.Rows[n].Cells[2].Value.ToString());
                    actulizaEstado(id, comboRancho.Text, status);
                    n += 1;
                }
                if (n == 1)
                    MessageBox.Show("Estado Actualizado");
                else if (n > 1)
                    MessageBox.Show("Estados Actualizados");
                dataGridView2.Rows.Clear();
            }
        }
    }
}
