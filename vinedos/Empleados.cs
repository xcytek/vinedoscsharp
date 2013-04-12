using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace vinedos
{
    public partial class Empleados : Form
    {
        int status = 1, maximo, bandera = 1, idmod;
        char sexo;
        string fotostring = "", Tipo = "";

        public Empleados()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Seguro que deseas salir?", "Mensaje", 
                MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (dr.ToString() == "Yes")           
                this.Close();                     
        }
        
        public void cargaRanchos()
        {
            string queryRancho = "";
            clConexion objConexion = new clConexion();
            if (rbOD.Checked)
            { queryRancho = "Select * from Cuadrillas"; label10.Text = "Cuadrilla"; }
            else
            { queryRancho = "Select Clave,Rancho from R"; label10.Text = "Rancho"; }
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(queryRancho, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cbRancho2.DataSource = dt;
                    cbRancho2.ValueMember = dt.Columns[0].ToString();
                    cbRancho2.DisplayMember = dt.Columns[1].ToString();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }                        
        }

        public void guardar(int Id)
        {
            string query = "Insert into empleados (Id,Paterno,Materno,Nombre,Direccion,Telefono,Sexo,Fechanac,Estado,Ciudad,Nss,Rfc,Curp,Status,Fechaing,Puesto,Tipo,Rancho,Foto)" +
                "values (@Numero,@Paterno,@Materno,@Nombre,@Direccion,@Telefono,@Sexo,@Fechanac,@Estado,@Ciudad,@Nss,@Rfc,@Curp,@Status,@Fechaing,@Puesto,@Tipo,@Rancho,@Foto)";
           clConexion objConexion = new clConexion();
           try
           {
               using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
               {
                   cmd.CommandText = query;
                   cmd.CommandType = CommandType.Text;
                   ///cmd.Parameters.AddWithValue("@Numero", maximo);Cambiar con la linea de abajo
                   ///cuando ya se hayan registrado todos los empleados actuales
                   ///para que se genere automaticamente el numero de empleado
                   cmd.Parameters.AddWithValue("@Numero", Id);
                   cmd.Parameters.AddWithValue("@Paterno", txtPaterno.Text);
                   cmd.Parameters.AddWithValue("@Materno", txtMaterno.Text);
                   cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                   cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                   cmd.Parameters.AddWithValue("@Telefono", mtxtTelefono.Text);
                   cmd.Parameters.AddWithValue("@Sexo", sexo);
                   cmd.Parameters.AddWithValue("@Fechanac", dtFechanac.Value);
                   cmd.Parameters.AddWithValue("@Estado", txtEstado.Text);
                   cmd.Parameters.AddWithValue("@Ciudad", txtCiudad.Text);
                   cmd.Parameters.AddWithValue("@Nss", txtNss.Text);
                   cmd.Parameters.AddWithValue("@Rfc", txtRfc.Text);
                   cmd.Parameters.AddWithValue("@Curp", txtCurp.Text);
                   cmd.Parameters.AddWithValue("@Status", status);
                   cmd.Parameters.AddWithValue("@Fechaing", dtIngreso.Value);
                   cmd.Parameters.AddWithValue("@Puesto", cbPuesto.Text);
                   cmd.Parameters.AddWithValue("@Tipo", Tipo);
                   cmd.Parameters.AddWithValue("@Rancho", cbRancho2.Text);
                   cmd.Parameters.AddWithValue("@Foto", fotostring);
                   cmd.Connection = objConexion.conexion();
                   cmd.Connection.Open();
                   cmd.ExecuteNonQuery();
               }
           }
           catch (Exception ex)
           { MessageBox.Show(ex.Message); }

           MessageBox.Show("Registro guardado exitosamente!", "Mensaje", MessageBoxButtons.OK);
           limpiar();
           registro();
        }

        private void altasToolStripMenuItem_Click(object sender, EventArgs e)
        { }

        private void groupBox1_Enter(object sender, EventArgs e)
        { }

        public void limpiar()
        {
            txtCiudad.Clear();
            txtCurp.Clear();
            txtDireccion.Clear();
            txtEstado.Clear();
            txtMaterno.Clear();
            txtNombre.Clear();
            txtNss.Clear();
            txtPaterno.Clear();
            txtRfc.Clear();
            mtxtTelefono.Clear();
            dtFechanac.Value = DateTime.Today;
            dtIngreso.Value = DateTime.Today;
            pbFoto.ImageLocation = null;
            rbMasculino.Checked = false;
            rbFemenino.Checked = false;            
        }

        public void traer_Maximo(String query)
        {           
            clConexion objConexion = new clConexion();
            cargaRanchos();
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
            { MessageBox.Show(ex.Message); }
            maximo += 1;
            txtNumero.Text = maximo.ToString();
        }

        public void registro()
        {
            limpiar();
            gbBusqueda.Visible = false;
            grid1.Visible = false;
            tcRegistro.Visible = true;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            btnGuardar.Text = "Guardar";
            pbLogo.Visible = false;
            rbPlanta.Checked = true;
            string query = "SELECT MAX(id) FROM empleados WHERE Id < 50000 ";
            traer_Maximo(query);            
            string queryPuesto = "SELECT Clave,Puesto FROM Puestos";
            clConexion objConexion = new clConexion();

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(queryPuesto, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cbPuesto.DataSource = dt;
                    cbPuesto.ValueMember = dt.Columns[0].ToString();
                    cbPuesto.DisplayMember = dt.Columns[1].ToString();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }          
 
        }
        
        public void modifica(int id)
        {
            string update = "UPDATE Empleados "+
                "SET Id = @NoEmpleado, Paterno = @Paterno, Materno = @Materno, Nombre = @Nombre, Direccion = @Direccion, Telefono = @Telefono, Fechanac = @Fechanac, Estado = @Estado, Ciudad = @Ciudad, Sexo = @Sexo, Foto = @Foto, Status = @Status, Fechabaja = @Fechabaja, Fechaing = @Fechaing, Nss = @Nss, Rfc = @Rfc, Curp = @Curp, Puesto = @Puesto, Rancho = @Rancho " +
                "WHERE Id = " + id + "";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand updatecmd = new SqlCommand(update, objConexion.conexion()))
                {
                    updatecmd.Parameters.AddWithValue("@NoEmpleado", txtNumero.Text);
                    updatecmd.Parameters.AddWithValue("@Paterno", txtPaterno.Text);
                    updatecmd.Parameters.AddWithValue("@Materno", txtMaterno.Text);
                    updatecmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    updatecmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                    updatecmd.Parameters.AddWithValue("@Telefono", mtxtTelefono.Text);
                    updatecmd.Parameters.AddWithValue("@Fechanac", dtFechanac.Value);
                    updatecmd.Parameters.AddWithValue("@Estado", txtEstado.Text);
                    updatecmd.Parameters.AddWithValue("@Ciudad", txtCiudad.Text);
                    updatecmd.Parameters.AddWithValue("@Sexo", sexo);
                    updatecmd.Parameters.AddWithValue("@Foto", fotostring);
                    updatecmd.Parameters.AddWithValue("@Status", status);
                    updatecmd.Parameters.AddWithValue("@Fechabaja", dtFechaBaja.Value);
                    updatecmd.Parameters.AddWithValue("@Fechaing", dtIngreso.Value);
                    updatecmd.Parameters.AddWithValue("@Nss", txtNss.Text);
                    updatecmd.Parameters.AddWithValue("@Rfc", txtRfc.Text);
                    updatecmd.Parameters.AddWithValue("@Curp", txtCurp.Text);
                    updatecmd.Parameters.AddWithValue("@Puesto", cbPuesto.Text);
                    updatecmd.Parameters.AddWithValue("@Rancho", cbRancho2.Text);
                    updatecmd.Connection = objConexion.conexion();
                    updatecmd.Connection.Open();
                    updatecmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

            MessageBox.Show("Registro modificado exitosamente!", "Mensaje", MessageBoxButtons.OK);            
        }

        private void registroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registro();
            bandera = 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (btnGuardar.Text == "Guardar")
                guardar(Convert.ToInt32(txtNumero.Text));
            else
                modifica(idmod);
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gbBusqueda.Visible = true;
            grid1.Visible = false;
            rbplanta2.Checked=true;
            rbEventual2.Checked = false;
            tcRegistro.Visible = false;
            btnCancelar.Visible = false;
            btnGuardar.Visible = false;
            pbLogo.Visible = false;
            bandera = 1;
        }

        private void rbPlanta_CheckedChanged(object sender, EventArgs e)
        {
            if (bandera == 1)
            { }
            else
            {
                string query = "SELECT MAX(id) FROM Empleados WHERE Id<50000 ";
                traer_Maximo(query);
                Tipo = "P";
            }                   
        }

        private void rbEventual_CheckedChanged(object sender, EventArgs e)
        {
            if (bandera == 1)
            { }
            else
            {
                string query = "SELECT MAX(id) FROM Empleados WHERE Id>49999 AND Id<60000 ";
                traer_Maximo(query);
                Tipo = "E";
            }            
        }

        private void rbMasculino_CheckedChanged(object sender, EventArgs e)
        {
            sexo = 'M';
        }

        private void rbFemenino_CheckedChanged(object sender, EventArgs e)
        {
            sexo = 'F';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            DialogResult dlgRes = dlg.ShowDialog();
            if (dlgRes != DialogResult.Cancel)
            {
                pbFoto.ImageLocation = dlg.FileName;
                fotostring = dlg.FileName;
            }
        }

        private void rbplanta2_CheckedChanged(object sender, EventArgs e)
        {
            grid1.Visible = true;
            label17.Visible = true;
            cbRancho.Visible = true;
            string query = "SELECT Id,Paterno,Materno,Nombre,Puesto,Rancho,Status "+
                "FROM Empleados WHERE Id>39999 AND Id<50000 ORDER BY Paterno ASC";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    grid1.DataSource = dt;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void rbEventual2_CheckedChanged(object sender, EventArgs e)
        {
            grid1.Visible = true;
            label17.Visible = true;
            cbRancho.Visible = true;
            string query = "SELECT Id,Paterno,Materno,Nombre,Puesto,Rancho,Status"+
                " FROM Empleados WHERE Id>49999 and Id<60000 ORDER BY Paterno ASC";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    grid1.DataSource = dt;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void grid1_DoubleClick(object sender, EventArgs e)
        {          
            int id = (Convert.ToInt32(grid1[0, grid1.SelectedCells[0].RowIndex].Value.ToString()));
            cargaDatos(id);
        }

        public void cargaDatos(int id)
        {
            idmod = id;
            tcRegistro.Visible = true;
            btnGuardar.Visible = true;
            btnGuardar.Text = "Modificar";
            btnCancelar.Visible = true;
            gbBusqueda.Visible = false;
            grid1.Visible = false;
            string query = "SELECT * FROM EMPLEADOS WHERE Id=" + id + "";

            if (id > 4999)            
                rbEventual.Checked = true;            
            else            
                rbPlanta.Checked = true;            

            clConexion objConexion = new clConexion();
            DataTable dt = new DataTable();
            limpiar();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    adapter.Fill(dt);
                    txtNumero.Text = dt.Rows[0]["Id"].ToString();
                    txtPaterno.Text = dt.Rows[0]["Paterno"].ToString();
                    txtMaterno.Text = dt.Rows[0]["Materno"].ToString();
                    txtNombre.Text = dt.Rows[0]["Nombre"].ToString();
                    mtxtTelefono.Text = dt.Rows[0]["Telefono"].ToString();
                    txtDireccion.Text = dt.Rows[0]["Direccion"].ToString();
                    dtFechanac.Value = Convert.ToDateTime(dt.Rows[0]["Fechanac"]);
                    txtEstado.Text = dt.Rows[0]["Estado"].ToString();
                    txtCiudad.Text = dt.Rows[0]["Ciudad"].ToString();
                    txtNss.Text = dt.Rows[0]["Nss"].ToString();
                    txtRfc.Text = dt.Rows[0]["Rfc"].ToString();
                    txtCurp.Text = dt.Rows[0]["Curp"].ToString();
                    if (Convert.ToChar(dt.Rows[0]["Sexo"].ToString()) == 'M')
                        rbMasculino.Checked = true;
                    else if (Convert.ToChar(dt.Rows[0]["Sexo"].ToString()) == 'F')
                        rbFemenino.Checked = true;
                    pbFoto.ImageLocation = dt.Rows[0]["foto"].ToString();
                    cbPuesto.Text = dt.Rows[0]["Puesto"].ToString();
                    cbRancho2.Text = dt.Rows[0]["Rancho"].ToString();
                    dtFechaBaja.Value = Convert.ToDateTime(dt.Rows[0]["FechaBaja"].ToString());
                    dtIngreso.Value = Convert.ToDateTime(dt.Rows[0]["Fechaing"].ToString());
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void asistenciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Asistencia asistencia = new Asistencia();
            asistencia.ShowDialog();
        }

        private void nominaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nomina nomina = new Nomina();
            nomina.ShowDialog();
        }

        private void rbOD_CheckedChanged(object sender, EventArgs e)
        {
            if (bandera == 1)
            { }
            else
            {
                string query = "SELECT MAX(id) FROM EMPLEADOS WHERE Id > 5999  ";
                traer_Maximo(query);
                Tipo = "OD";
            }
        }
        
        private void rbOD2_CheckedChanged(object sender, EventArgs e)
        {
            grid1.Visible = true;
            label17.Visible = false;
            cbRancho.Visible = false;
            string query = "SELECT Id, Paterno, Materno, Nombre, Puesto, Rancho, Status" +
                " FROM Empleados WHERE Id > 59999 ORDER BY Paterno ASC";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    grid1.DataSource = dt;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void avanzadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CambiaRancho frmCambiaRancho = new CambiaRancho();
            frmCambiaRancho.ShowDialog();
        }

        private void kardexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KardexEmpleado frmKardex = new KardexEmpleado();
            frmKardex.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EmpleadoArchivo frmEmpleadoArchivo = new EmpleadoArchivo();
            frmEmpleadoArchivo.ShowDialog();
        }

        private void listadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Puesto> objPuesto = new List<Puesto>();
            Funciones objFuncion = new Funciones();
            objPuesto = objFuncion.llenarListaPuesto(objPuesto);

            string[,] ArrayEmpleados = 
            {
            { "P", "E" }, { "Empleados de Planta", "Empleados Eventuales" } 
            };

            for (int i = 0; i < 2; i++)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
                saveFileDialog1.Title = ArrayEmpleados[1, i];
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                    && saveFileDialog1.FileName.Length > 0)
                {
                    int ban = 0;
                    try
                    {
                        FileInfo t = new FileInfo(saveFileDialog1.FileName);
                        StreamWriter Tex = t.CreateText();
                        clConexion objConexion = new clConexion();
                        string Cadena;
                        string queryExternos = "SELECT Id,Paterno,Materno,Nombre,Puesto, Rancho FROM Empleados WHERE Tipo = '"
                            + ArrayEmpleados[0, i] + "' AND Status = 1 AND Rancho != '-' ORDER BY Paterno ASC";
                        using (SqlCommand cmd = new SqlCommand(queryExternos, objConexion.conexion()))
                        {
                            SqlDataReader dr;
                            cmd.Connection = objConexion.conexion();
                            cmd.Connection.Open();
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows == true)
                                while (dr.Read())
                                {
                                    double SalDiario = 0;
                                    foreach (Puesto puesto in objPuesto)
                                        if (dr[4].ToString() == puesto.NomPuesto)
                                            SalDiario = puesto.Sueldo;
                                    Cadena = dr[0].ToString() + "," + dr[1].ToString() + "," + dr[2].ToString() +
                                        "," + dr[3].ToString() + "," + SalDiario + "," + dr[4].ToString() + "," + dr[5].ToString();
                                    Tex.WriteLine(Cadena);
                                }
                        }
                        Tex.Write(Tex.NewLine);
                        Tex.Close();
                    }
                    catch (Exception ex)
                    { ban = 1; }
                    if (ban == 0)
                        MessageBox.Show("El Archivo " + saveFileDialog1.FileName +
                            " ha sido creado", "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("No se pudo crear el Padron de Empleados. Intente de Nuevo",
                            "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Empleados_Load(object sender, EventArgs e)
        {
            this.Text = "L.A. CETTO ::Sistema de Viñedos::";
            toolStripStatusLabel1.Text = "Usuario: " +
                LogUsers.getUserName();
            //MessageBox.Show("Usuario: " + LogUsers.getUserName() + " Nivel: " + LogUsers.getNivel());
            registroToolStripMenuItem.Enabled = false;
            modificarToolStripMenuItem.Enabled = false;
            avanzadoToolStripMenuItem.Enabled = false;
            asistenciaToolStripMenuItem.Enabled = false;
            nominaToolStripMenuItem.Enabled = false;
            kardexToolStripMenuItem.Enabled = false;
            listadoToolStripMenuItem.Enabled = false;

            if (LogUsers.getNivel() >= 2)
            {                
                registroToolStripMenuItem.Enabled = true;
                asistenciaToolStripMenuItem.Enabled = true;
                kardexToolStripMenuItem.Enabled = true;
                listadoToolStripMenuItem.Enabled = true;
            }

            if (LogUsers.getNivel() >= 3)
            {
                modificarToolStripMenuItem.Enabled = true;
                avanzadoToolStripMenuItem.Enabled = true;
                nominaToolStripMenuItem.Enabled = true;
            }
        }

        public void generaHtml(DataGridView grid1, string titulo)
        {
            int ban = 0;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "HTML files (*.html)|*.html";
            saveFileDialog1.Title = titulo;
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                && saveFileDialog1.FileName.Length > 0)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);                    
                    //Se escribe el codigo HTML
                    sw.WriteLine("<html><title>" + saveFileDialog1.Title + "</title>");//Se abre el HTML y El TITULO se Establece
                    sw.WriteLine("<body>");//Se abre la etiqueta <BODY>
                    sw.WriteLine("<br /><div style='text-align:center;'>" +
                        "CATALOGO DE EMPLEADOS</div><br /><br />");//Cabecera con DIV
                    sw.WriteLine("<table border ='1' cellspacing = '0' cellpadding = '0' align = 'center'>");
                    sw.WriteLine("<tr>");
                    for (int contador = 0; contador < grid1.Columns.Count; contador++)
                    {
                        string _name_emp = grid1.Rows[0].Cells[contador].Value.ToString();
                        sw.WriteLine("<td>" + _name_emp + "</td>");
                    }
                    sw.WriteLine("</tr></table><br />");
                    sw.WriteLine("Usuario: " + LogUsers._username + "<br />" +
                        DateTime.Now + "<br />IP:" + LogUsers._ip + "</body></html>");
                    //CERRAMOS EL ARCHIVO </HTML>
                    sw.Close();
                }
                catch (Exception ex)
                {
                    ban = 1;
                    MessageBox.Show("No se pudo crear el " + titulo + ". Intente de Nuevo. " + ex.Message,
                        "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (ban == 0)
                        MessageBox.Show("El Archivo " + saveFileDialog1.FileName + " ha sido creado",
                            "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

               

        private void concentradoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ///*************************
            ///FUNCION PARA CREAR LOS ARCHIVOS PARA TODOS LOS EMPLEADOS!!!
            ///************************
            ///
            Funciones objFuncion = new Funciones();
            List<ListRanchos> objListRanchos = new List<ListRanchos>();
            List<Puesto> objListPuestos = new List<Puesto>();
            List<Empleado> objListEmpleado = new List<Empleado>();            
            objListEmpleado = objFuncion.llenarListaEmpleado(objListEmpleado);
            objListPuestos = objFuncion.llenarListaPuesto(objListPuestos);
            objListRanchos = objFuncion.cargaRanchos(objListRanchos);
            //CREACION DEL DATATABLE
            DataTable tabla_catalogo = new DataTable();
            DataColumn columna;
            DataRow renglon;
            //*************************************************
            //Primero CREAMOS LA COLUMNA DONDE IRAN LOS RANCHOS

            columna = new DataColumn();
            columna.DataType = Type.GetType("System.String");
            columna.ColumnName = "Rancho";
            columna.AutoIncrement = false;
            columna.Caption = "Ranchos";
            columna.ReadOnly = true;
            columna.Unique = true;
            tabla_catalogo.Columns.Add(columna);
            //**********************************************
            //CREAMOS LAS COLUMNAS DE LOS PUESTOS SEGUN LA RELACION: MAYORDOMOS, TRACTORISTAS, REGADORES, JORNALES, OTROS.
            foreach (Puesto puesto in objListPuestos)
            {
                if (puesto.NomPuesto == "Jornalero")
                {
                    columna = new DataColumn();
                    columna.DataType = Type.GetType("System.String");
                    columna.ColumnName = puesto.NomPuesto;
                    columna.AutoIncrement = false;
                    columna.Caption = puesto.NomPuesto;
                    columna.ReadOnly = true;
                    columna.Unique = true;
                    tabla_catalogo.Columns.Add(columna);
                }
            }

            DataGridView grid1 = new DataGridView();
            grid1.DataSource = tabla_catalogo;
            generaHtml(grid1, "Catalogo de Empleados");
        }

        private void cargarSueldosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filepath = "";
            string[] campos;
            int renglon = 0;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "csv files (*.csv)|*.csv";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                filepath = openFileDialog1.FileName;
            String linea;
            StreamReader f = new StreamReader(filepath);

            while ((linea = f.ReadLine()) != null)
            {
                renglon++;
                campos = linea.Split(',');
                string NoEmpleado = campos[0];
                string SalarioDiario = campos[1];

                try
                {
                    //Actualizar la tabla empleados con los nuevos sueldos a partir del archivo csv que debe ser cargado                   

                    string update = "UPDATE Empleados SET SalarioDiario=@SalarioDiario WHERE Id = " + NoEmpleado + "";
                    clConexion objConexion = new clConexion();

                    using (SqlCommand updatecmd = new SqlCommand(update, objConexion.conexion()))
                    {
                        updatecmd.Parameters.AddWithValue("@SalarioDiario", SalarioDiario);
                        updatecmd.Connection = objConexion.conexion();
                        updatecmd.Connection.Open();
                        updatecmd.ExecuteNonQuery();                        
                    }
                }
                catch (Exception ex)
                {
                    renglon = renglon - 1;
                }               
            }

            MessageBox.Show("Se actualizaron " + renglon + " registros satisfactoriamente");
                    
                    
           
        }
    }
}
