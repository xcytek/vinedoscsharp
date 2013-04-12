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
    public partial class Productos : Form
    {
        public Productos()
        {
            InitializeComponent();
        }

        private void Productos_Load(object sender, EventArgs e)
        {
           
        }
        private void cbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCategoria.Text == "Agroquimicos")
            {
                cbSubcategoria.Items.Clear();
                cbSubcategoria.Items.Add("Fungicidas");
                cbSubcategoria.Items.Add("Insecticidas");
                cbSubcategoria.Items.Add("Nematicidas");
                cbSubcategoria.Items.Add("Herbicidas");
                cbSubcategoria.Items.Add("Fertilizantes Foliares");
                cbSubcategoria.Items.Add("Adeherentes");
                cbSubcategoria.Items.Add("Bactericidas");
                cbSubcategoria.Items.Add("Fertilizantes");
                cbSubcategoria.Items.Add("Mejoradores de Suelo");
            }

            if (cbCategoria.Text == "Mantenimiento y Suministros")
            {
                cbSubcategoria.Items.Clear();
                cbSubcategoria.Items.Add("Mantenimiento");
                cbSubcategoria.Items.Add("Accesorios de PVC");
                cbSubcategoria.Items.Add("Suministros");
            }
        }

        private void registrarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbLogo.Visible = false;
            gbNuevo.Visible = true;
            btnCancelar.Visible = true;
            btnGuardar.Visible = true;
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

        
    }
}
