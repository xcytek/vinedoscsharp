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
    public partial class DirectorioTelefonico : Form
    {
        public DirectorioTelefonico()
        {
            InitializeComponent();
        }
        int Id = 0;
        public void SearchFunction(string Query)
        {
            List<DirTelefonico> objDirTelefonico = new List<DirTelefonico>();
            List<DirTelefonico> aux_ObjDirTelefonico = new List<DirTelefonico>();
            objDirTelefonico = Funciones.Fill_DirTelefonico_Class(objDirTelefonico);
            foreach (DirTelefonico directorio in objDirTelefonico)
            {
                if (Query == Convert.ToString(directorio.Id) || Query == directorio.Nombre || Query == directorio.Numero || Query == directorio.Direccion || Query == directorio.Ciudad
                    || Query == directorio.Estado || Query == directorio.Email || Query == directorio.Web)                
                    aux_ObjDirTelefonico.Add(new DirTelefonico(directorio.Id, directorio.Nombre, directorio.Numero, directorio.Direccion, directorio.Ciudad, directorio.Estado, directorio.Email, directorio.Web));                
            }
            dataGridView1.DataSource = aux_ObjDirTelefonico;
        }
        public void LimpiaCampos()
        {
            textBox1.Text = "";
            maskedTextBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }
        private void DirectorioTelefonico_Load(object sender, EventArgs e)
        {
            this.Text = "L.A. CETTO ::Sistema de Viñedos::";
            toolStripStatusLabel1.Text = "Usuario: " + LogUsers.getUserName();            
        }

        private void buscarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DirTelefonico> objDirTelefonico = new List<DirTelefonico>();
            objDirTelefonico = Funciones.Fill_DirTelefonico_Class(objDirTelefonico);
            dataGridView1.Visible = true;
            textBox7.Visible = true;
            panel1.Visible = false;
            button1.Visible = true;
            button1.Text = "Modificar";
            button2.Visible = true;
            button2.Text = "Eliminar";
            dataGridView1.DataSource = objDirTelefonico;
        }

        private void añadirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            textBox7.Visible = false;
            panel1.Visible = true;
            button1.Visible = true;
            button1.Text = "Agregar";
            button2.Visible = true;
            button2.Text = "Limpiar";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Agregar")
            {
                int ban = Funciones.AddContactoTelefonico(Funciones.traer_Maximo("SELECT MAX(Id) FROM DirectorioTelefonico"),textBox1.Text, maskedTextBox1.Text, textBox2.Text,
                    textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text);
                if (ban == 0)
                {
                    MessageBox.Show("Registro Almacenado");
                    LimpiaCampos();
                }
                else if (ban == 1)
                    MessageBox.Show("Registro no almacenado");
            }
            else if (button1.Text == "Modificar")
            {
                
                if (dataGridView1.Visible)
                {
                    Id = (Convert.ToInt32(dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString()));
                    textBox1.Text = dataGridView1[1, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
                    maskedTextBox1.Text = dataGridView1[2, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
                    textBox2.Text = dataGridView1[3, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
                    textBox3.Text = dataGridView1[4, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
                    textBox4.Text = dataGridView1[5, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
                    textBox5.Text = dataGridView1[6, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
                    textBox6.Text = dataGridView1[7, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
                    dataGridView1.Visible = false;
                    textBox7.Visible = false;
                    panel1.Visible = true;
                }
                else
                {
                    int ban = Funciones.UpdateContactoTelefonico(Id, textBox1.Text, maskedTextBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text);
                    if (ban == 0)
                    { MessageBox.Show("Registro Modificado"); buscarToolStripMenuItem_Click(null, null); }
                    else if (ban == 1)
                    { MessageBox.Show("Registro no modificado"); }
                }
                
                
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            Id = (Convert.ToInt32(dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString()));
            textBox1.Text = dataGridView1[1, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
            maskedTextBox1.Text = dataGridView1[2, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
            textBox2.Text = dataGridView1[3, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
            textBox3.Text = dataGridView1[4, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
            textBox4.Text = dataGridView1[5, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
            textBox5.Text = dataGridView1[6, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
            textBox6.Text = dataGridView1[7, dataGridView1.SelectedCells[0].RowIndex].Value.ToString();
            dataGridView1.Visible = false;
            panel1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Limpiar")
            {
            }
            else if (button2.Text == "Eliminar")
            {
                if (Funciones.DeleteContactoTelefonico((Convert.ToInt32(dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString()))) == 0)
                { MessageBox.Show("Registro Eliminado"); }
                else
                { MessageBox.Show("Registro no eliminado"); }
                buscarToolStripMenuItem_Click(null, null);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text == "")
            {
                List<DirTelefonico> objDirTelefonico = new List<DirTelefonico>();
                objDirTelefonico = Funciones.Fill_DirTelefonico_Class(objDirTelefonico);
                dataGridView1.DataSource = objDirTelefonico;
            }
            else
            {
                SearchFunction(textBox7.Text);
            }
        }
    }
}
