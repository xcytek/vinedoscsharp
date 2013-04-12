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
    public partial class Salida : Form
    {
        public Salida()
        {
            InitializeComponent();
            string query = "SELECT Clave,Rancho from R";
            string query3 = "SELECT * from Articulos";
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("¿Seguro que deseas salir?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr.ToString() == "Yes")            
                this.Close();
        }

        private void cbRancho_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query2 = "SELECT Clave From Lotes where Rancho = '" + cbRancho.SelectedValue.ToString() + "'";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query2, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    Lote1.DataSource = dt;
                    Lote1.ValueMember = dt.Columns[0].ToString();
                    Lote1.DisplayMember = dt.Columns[0].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void insertar()
        {
            //string query = "";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (btnGuardar.Text == "Guardar")
            {
                int n = 0;
                foreach (DataGridViewRow row in dgSalida.Rows)
                {
                    int id = Convert.ToInt32(dgSalida.Rows[n].Cells[0].Value);
                    int a = Convert.ToInt32(dgSalida.Rows[n].Cells[3].Value);
                    int r = Convert.ToInt32(dgSalida.Rows[n].Cells[4].Value);
                    int act = Convert.ToInt32(dgSalida.Rows[n].Cells[5].Value);
                    int hrs = Convert.ToInt32(dgSalida.Rows[n].Cells[8].Value);
                    string lote1 = Convert.ToString(dgSalida.Rows[n].Cells[6].Value);
                    //insertar(id, a, r, act, lote1, hrs);
                    n = n + 1;
                }
                MessageBox.Show("!Lista Guardada Exitosamente!", "Mensaje", MessageBoxButtons.OK);

            }
        }
    }
}
