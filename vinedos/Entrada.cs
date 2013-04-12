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
    public partial class Entrada : Form
    {
        public Entrada()
        {
            InitializeComponent();
            string query3 = "SELECT * from Articulos";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query3, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    Producto.DataSource = dt;
                    Producto.ValueMember = dt.Columns[0].ToString();
                    Producto.DisplayMember = dt.Columns[1].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("¿Seguro que deseas salir?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr.ToString() == "Yes")
            {
                this.Close();
            }
            else
            {
            }
        }
    }
}
