using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace vinedos
{
    public partial class Clima : Form
    {
        public Clima()
        {
            InitializeComponent();
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
                    comboBox1.DataSource = dt;
                    comboBox1.ValueMember = dt.Columns[0].ToString();
                    comboBox1.DisplayMember = dt.Columns[1].ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Clima_Load(object sender, EventArgs e)
        {
            CargaRanchoGeneral();
        }
    }
}
