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
using System.Net;
using System.Net.Sockets;

namespace vinedos
{
    public partial class Principal : Form
    {       
        public Principal()
        {
            InitializeComponent();
            label1.Text = DateTime.Today.ToLongDateString();
            timer1.Start();
        }
        
        ToolStripStatusLabel fecha = new ToolStripStatusLabel();
        int bandera = 0;
        
        public void loadComboSesionUsers()
        {
            string query = "SELECT username from Sesion";
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
                comboBox1.Items.Add("No hay usuarios conectados");                
            }
        }

        public void EncenderServidor(bool Variable)
        {
            iniciarToolStripMenuItem.Enabled = !Variable;
            detenerToolStripMenuItem.Enabled = Variable;
        }        
        /// <summary>
        /// Funcion para obtener la IP del servidor!!!!
        /// </summary>
        /// <returns></returns>
        ///         
        public static string GetIP(string OS)
        {
            string IP = "None";
            try
            {
                String strHostName = Dns.GetHostName();
                IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

                if (OS == "Windows 7" || OS == "Windows Vista")
                    IP = iphostentry.AddressList[2].ToString();
                else if (OS == "Windows XP")
                    IP = iphostentry.AddressList[0].ToString();
            }
            catch (Exception ex)
            { IP = "None"; }

            return IP;
        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {            

        }

        private void empleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Empleados empleados = new Empleados();
            empleados.ShowDialog();            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongTimeString();            
        }

        private void salidasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Salida salida = new Salida();
            salida.ShowDialog();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos productos = new Productos();
            productos.ShowDialog();
        }
        private void entradasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Entrada entrada = new Entrada();
            entrada.ShowDialog();
        }
        private void contratosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contratos contrato = new Contratos();
            contrato.ShowDialog();
        }
        private void salirDelSistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Seguro que deseas cerrar sesion?", "Sistema de Viñedos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr.ToString() == "Yes")
            {
                string username = LogUsers._username;
                if (LogUsers.Deleteuser(LogUsers._username) == 0)
                { MessageBox.Show("Sesion ha sido cerrada. Hasta Pronto " + username); }
                bandera = 1;
                this.Close();
            }
        }
        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
        private void consultasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Consultas frmConsulta = new Consultas();
            frmConsulta.ShowDialog();               
        }
        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        { }

        private void iniciarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //EncenderServidor(true);
            //IniciarServidor(true);
        }
        private void detenerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //EncenderServidor(false);
            //IniciarServidor(false);
        }
        private void Principal_Load(object sender, EventArgs e)
        {
            //ESTILOS PARA LAS CELDAS DEL DATAGRIDVIEW DEL HISTORIAL
            this.dataGridView1.RowsDefaultCellStyle.BackColor = Color.Bisque;
            this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;

            this.Text = "L.A. CETTO ::Sistema de Viñedos::";
            toolStripStatusLabel1.Text = "Usuario: " + LogUsers.getUserName();
            //APAGA TODOS LOS MENUS Y SE ENCENDERAN SEGUN EL USUARIO QUE INICIE SESION, TODO DEPENDE DEL NIVEL DE ACCESO QUE POSEA!!
            empleadosToolStripMenuItem.Enabled = false;            
            almacenToolStripMenuItem.Enabled = false;            
            contratosToolStripMenuItem.Enabled = false;
            consultasToolStripMenuItem.Enabled = false;
            servidorToolStripMenuItem.Enabled = false;                        
            //VALDICACION DE PERMISOS A USUARIO SEGUN EL NIVEL DE ACCESO RESTRINGIDO QUE POSEAN
            //Y EL NIVEL DE LOS PROCESOS O MENUS
            if (LogUsers.getNivel() >= 1)
            {
                empleadosToolStripMenuItem.Enabled = true;         
                almacenToolStripMenuItem.Enabled = true;                
                contratosToolStripMenuItem.Enabled = true;                
            }
            if (LogUsers.getNivel() == 3)
            {
                empleadosToolStripMenuItem.Enabled = true;
                almacenToolStripMenuItem.Enabled = true;
                contratosToolStripMenuItem.Enabled = true;
                consultasToolStripMenuItem.Enabled = true;
            }
            if (LogUsers.getNivel() == 4)
            {
                empleadosToolStripMenuItem.Enabled = true;
                almacenToolStripMenuItem.Enabled = true;
                contratosToolStripMenuItem.Enabled = true;
                consultasToolStripMenuItem.Enabled = true;
                servidorToolStripMenuItem.Enabled = true;
                panel2.Visible = true;
                loadComboSesionUsers();
            }
        }
        private void almacenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowsFormsApplication1.Almacen frmAlmace = new WindowsFormsApplication1.Almacen();
            frmAlmace.ShowDialog();
        }
        private void directorioTelefonicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectorioTelefonico frmDirectorio = new DirectorioTelefonico();
            frmDirectorio.Show();
        }
        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sistema Viñedos L.A. CETTO\nVersion Beta 1.0 2012\nPara cualquier aclaracion consulte al administrador del sistema", "L.A. CETTO ::Sistema de Viñedos::");
        }
        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Seguro que deseas cerrar sesion?", "Sistema de Viñedos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr.ToString() == "Yes")
            {
                string username = LogUsers._username;
                if (LogUsers.Deleteuser(LogUsers._username) == 0)
                { MessageBox.Show("Sesion ha sido cerrada. Hasta Pronto " + username); }
                bandera = 1;
                this.Close();
            }
        }
        private void Principal_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (bandera == 0)
            {
                DialogResult dr = MessageBox.Show("Seguro que deseas cerrar sesion?", "Sistema de Viñedos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr.ToString() == "Yes")
                {
                    string username = LogUsers._username;
                    if (LogUsers.Deleteuser(LogUsers._username) == 0)
                    { MessageBox.Show("Sesion ha sido cerrada. Hasta Pronto " + username); }
                }
            }
        }

        private void label7_MouseMove(object sender, MouseEventArgs e)
        {
            label7.Cursor = Cursors.Hand;
        }

        DateTime inicio_sesion = new DateTime();
        public int tiempodeSesion(int horas_inicio_sesion)
        {
            int horas_sesion_actual = DateTime.Now.Hour;
            return horas_sesion_actual - horas_inicio_sesion;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel3.Visible = true;
            string ip = "";
            string pcname = "";
            string query = "SELECT * FROM Sesion WHERE Username = '" + comboBox1.Text + "'";
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
                            ip = dr[4].ToString();
                            pcname = dr[3].ToString();
                            inicio_sesion = Convert.ToDateTime(dr[2].ToString());
                            break;
                        }
                }
            }
            catch (Exception ex)
            { }

            label4.Text = "Usuario: " + comboBox1.Text;
            label5.Text = "Nombre PC: " + pcname;
            label6.Text = "IP: " + ip;
            int horas_inicio_sesion = inicio_sesion.Hour;

            label8.Text = "Tiempo sesion: " + tiempodeSesion(horas_inicio_sesion);
        }

        private void button1_Click(object sender, EventArgs e)
        { loadComboSesionUsers(); }

        private void label7_Click(object sender, EventArgs e)
        {
            if (!dataGridView1.Visible)
            {
                label7.Text = "Ocultar Historial";
                dataGridView1.Visible = true;
                string query = "SELECT * FROM HistorialSesion ORDER BY HoraFecha DESC";
                clConexion objConexion = new clConexion();
                try
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception ex)
                { comboBox1.Items.Add("No hay historial registrado"); }
            }
            else
            {
                label7.Text = "Mostrar Historial";
                dataGridView1.Visible = false;
                dataGridView1.DataSource = null;
            }
        }

         
    }
}
