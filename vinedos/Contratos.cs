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
    public partial class Contratos : Form
    {
        int maximo;
        public Contratos()
        {
            InitializeComponent();
            string query2 = "SELECT * From Actividad";
            clConexion objConexion = new clConexion();

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query2, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cbActividad.DataSource = dt;
                    cbActividad.ValueMember = dt.Columns[0].ToString();
                    cbActividad.DisplayMember = dt.Columns[1].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void nuevoContratoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbLogo.Visible = false;
            groupBox1.Visible = true;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
            traer_Maximo();
        }

        public void traer_Maximo()
        {

            clConexion objConexion = new clConexion();
            string query = "select max(id) from Contratos";
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
            {
                MessageBox.Show(ex.Message);
            }

            maximo += 1;
            txtFolio.Text = maximo.ToString();
        }

        public void nuevo()
        {
            string query = "insert into Contratos (Id,FechaInicio,Actividad) values (@Id,@Fecha,@Actividad)";
            clConexion objConexion = new clConexion();

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtFolio.Text));
                    cmd.Parameters.AddWithValue("@Fecha", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@Actividad", cbActividad.Text);
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("Contrato Registrado Exitosamente!", "Mensaje", MessageBoxButtons.OK);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Seguro que deseas salir?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr.ToString() == "Yes")
            {
                this.Close();
            }
            else
            {
            }
        }

        public void limpiar()
        {
            txtFolio.Clear();
            traer_Maximo();
            dateTimePicker1.Value = DateTime.Today;
            cbActividad.Text = "";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            nuevo();
            limpiar();
        }

        private void verContratosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbLogo.Visible = false;
            gbLista.Visible = true;
            string query = "SELECT * From Contratos";
            clConexion objConexion = new clConexion();

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgLista.DataSource = dt;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void buscaContrato(int id)
        {
            gbLista.Visible = false;
            gbInfo.Visible = true;
            string query = "SELECT * From Ref_Contratos where Id = "+id+"";
            string query2 = "SELECT * From Contratos where Id = "+id+"";
            clConexion objConexion = new clConexion();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)
                    {

                    }
                    else
                    {
                      //Traer los Nombres de los empleados que estan registrados en ese contrato y ponerlos en el grid.
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand(query2, objConexion.conexion()))
                {
                    SqlDataReader dr;
                    cmd.Connection = objConexion.conexion();
                    cmd.Connection.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)
                    {
                        
                    }
                    else
                    {
                        while (dr.Read())
                        {
                            label7.Text = dr[0].ToString();
                            label8.Text = dr[1].ToString();
                            label9.Text = dr[3].ToString();
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgLista_DoubleClick(object sender, EventArgs e)
        {
            int id = (Convert.ToInt32(dgLista[0, dgLista.SelectedCells[0].RowIndex].Value.ToString()));
            buscaContrato(id);
        }


    }
}
