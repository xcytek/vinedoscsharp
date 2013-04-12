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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }        
        clConexion objConexion = new clConexion();
        Principal frmPrincipal = new Principal();
        string UserSecurity = "Administrator";
        string ContraSecurity = "default";
        int tipoUserSecurity = 4;
        public int AddUserSesion(string table, string username, int nivel, DateTime horafecha, string pc,string ip, string observaciones)
        {
            string query = "INSERT INTO " + table + " (Username,Nivel,HoraFecha,PC,Ip,Observaciones) VALUES (@Username,@Nivel,@HoraFecha,@PC,@IP,@Observaciones)";
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
                    cmd.Parameters.AddWithValue("@IP", ip);
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

        
        public int valida()
        {
            int i = 1;
            if (txtUsuario.Text == "")
            {
                MessageBox.Show("Introduce un Usuario", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                laUs.Visible = true;
                laPa.Visible = false;
                txtUsuario.Focus();
                i = 0;
            }
            else if (txtPassword.Text == "")
            {
                MessageBox.Show("Introduce una Contraseña", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                laUs.Visible = false;
                laPa.Visible = true;
                txtPassword.Focus();
                i = 0;
            }
            else if (txtUsuario.Text == UserSecurity && txtPassword.Text == ContraSecurity)
            {
                i = 2;
            }
            return i;
        }

        public void entrar()
        {                       
            string query = "SELECT username, password, tipo FROM users WHERE username='" + txtUsuario.Text + "'";
            if (valida() == 1)
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                    {
                        SqlDataReader dr;
                        cmd.Connection = objConexion.conexion();
                        cmd.Connection.Open();
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows == false)                        
                            MessageBox.Show("El Usuario no existe", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);                        
                        else                        
                            while (dr.Read())
                            {
                                if (dr["username"].ToString() == txtUsuario.Text)
                                {
                                    if (dr["password"].ToString() == txtPassword.Text)
                                    {
                                        //SE BORRAN LOS DATOS DE LOS TEXTBOX
                                        txtUsuario.Text = "";
                                        txtPassword.Text = "";
                                        //CREAMOS LA SESION DEL USUARIO QUE HIZO LOGIN
                                        LogUsers.NuevoUsers(dr["username"].ToString(), dr["password"].ToString(), int.Parse(dr["tipo"].ToString()), cmd.Connection.WorkstationId, Principal.GetIP(Funciones.GetOS()));//"tipo" se refiere al NIVEL que tiene el usuario
                                        //REGISTRAMOS EL HISTORIAL Y LA SESION EN LA DB DE QUIEN HIZO LOGIN
                                        string[] Sesion = { "Sesion", "HistorialSesion" };
                                        for (int i = 0; i < Sesion.Length; i++)
                                            AddUserSesion(Sesion[i], dr["username"].ToString(), int.Parse(dr["tipo"].ToString()), DateTime.Now, cmd.Connection.WorkstationId, Principal.GetIP(Funciones.GetOS()), "Acceso");                                    
                                        Principal a = new Principal();
                                        a.ShowDialog();                                                      
                                    }
                                    else                                    
                                        MessageBox.Show("Contraseña Incorrecta", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);                                 
                                }
                                else                                
                                    MessageBox.Show("Usuario Incorrecto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                                
                            }                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "algomas");
                }
            }
            if (valida() == 2)//ACCESSO CON EL USUARIO ADMINISTRATOS
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {                    
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    txtUsuario.Text = "";
                    txtPassword.Text = "";
                    //CREAMOS LA SESION DEL USUARIO QUE HIZO LOGIN
                    LogUsers.NuevoUsers(UserSecurity, ContraSecurity, tipoUserSecurity, cmd.Connection.WorkstationId, Principal.GetIP(Funciones.GetOS()));//"tipo" se refiere al NIVEL que tiene el usuario
                    //REGISTRAMOS EL HISTORIAL Y LA SESION EN LA DB DE QUIEN HIZO LOGIN
                    string[] Sesion = { "Sesion", "HistorialSesion" };
                    for (int i = 0; i < Sesion.Length; i++)
                        AddUserSesion(Sesion[i], UserSecurity, tipoUserSecurity, DateTime.Now, cmd.Connection.WorkstationId, Principal.GetIP(Funciones.GetOS()), "Acceso");
                    Principal a = new Principal();
                    a.ShowDialog();
                    //this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            entrar();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)            
                entrar();                       
        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)            
                entrar();           
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Salir del Sistema?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr.ToString() == "Yes")            
                this.Close();            
            else            
                txtUsuario.Focus();            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", "http://www.xcytekc.com");            
        }

        private void Login_Load(object sender, EventArgs e)
        {
            clConexion objConexion = new clConexion();
            string queryEmpleado = "SELECT * FROM Sesion";
            try
            {
                using (SqlCommand cmd = new SqlCommand(queryEmpleado, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                            if (dr[3].ToString() == cmd.Connection.WorkstationId)
                            {
                                MessageBox.Show("Hay una sesion que no fue cerrada.\nUsuario: '" + dr[0].ToString() + "'", 
                                    "Sistema de Viñedos", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Information
                                    );
                                LogUsers._username = dr[0].ToString();
                                LogUsers._nivel = int.Parse(dr[1].ToString());
                                LogUsers._computername = dr[3].ToString();
                                LogUsers._ip = Principal.GetIP(Funciones.GetOS());                               
                                frmPrincipal.ShowDialog();                                
                            }
                    cmd.Connection.Close();                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar sesion. Intenta mas tarde. " + ex.Message);
            }
        }
    }
}
