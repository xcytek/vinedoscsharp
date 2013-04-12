using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace vinedos
{
    class Exportar
    {
        /// <summary>
        /// Estrucutura de la Cabecera
        /// 1.-Mano Obra Costos LOTE-ACTIVIDAD
        /// 2.-Mano Obra Costo Planta
        /// 3.-Mano Obra Costo Hectarea
        /// 4.-Mano Obra Costo Planta-Actividad        
        /// </summary>
        

        public static void excel(DataGridView dataGrid, string[] Cabecera)
        {            
            List<ListActividades> objActividad = new List<ListActividades>();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
            saveFileDialog1.Title = Cabecera[1];
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                && saveFileDialog1.FileName.Length > 0)
            {
                int ban = 0;                
                try
                {
                    string Cadena = "";
                    Funciones fnFunciones = new Funciones();
                    objActividad = fnFunciones.cargaListaActividades(objActividad);
                    FileInfo t = new FileInfo(saveFileDialog1.FileName);
                    StreamWriter Tex = t.CreateText();

                    for (int i = 1; i < Cabecera.Length; i++)
                        Tex.WriteLine(Cabecera[i]);

                    switch (int.Parse(Cabecera[0]))
                    {
                        case 1://COSTO MANO DE OBRA LOTE-ACTIVIDAD
                            int contActividad = 0;
                            foreach (ListActividades actividad in objActividad)
                            {
                                if (contActividad == 0)
                                    Cadena += "Lotes,";
                                Cadena += actividad.Actividad + ",";
                                contActividad = 1;
                            }
                            Cadena += "TOTAL";
                            Tex.WriteLine(Cadena);
                            for (int i = 0; i < dataGrid.Rows.Count; i++)
                            {
                                Cadena = "";
                                for (int x = 0; x < dataGrid.Columns.Count; x++)
                                    Cadena += dataGrid.Rows[i].Cells[x].Value + ",";
                                Tex.WriteLine(Cadena);
                            }
                            break;
                        case 2://Mano Obra Costo Planta
                            break;
                        case 3://Mano Obra Costo Hectarea

                            string[] Array_Cadena = new string[] { "","Costos,Montos"};
                            for (int j = 0; j < 2; j++)                            
                                Tex.WriteLine(Array_Cadena[j]);
                            
                            for (int i = 0; i < dataGrid.Rows.Count; i++)
                            {
                                Cadena = "";
                                for (int x = 0; x < dataGrid.Columns.Count; x++)
                                    Cadena += dataGrid.Rows[i].Cells[x].Value + ",";
                                Tex.WriteLine(Cadena);
                            }
                            break;
                        case 4://Mano Obra Costo Planta-Actividad
                            break;
                        case 5:
                            break;
                    } 
                    Tex.Write(Tex.NewLine);
                    Tex.Close();
                }
                catch (Exception ex)
                {
                    ban = 1;
                }
                if (ban == 0)
                    MessageBox.Show("El Archivo " + saveFileDialog1.FileName + " ha sido creado",
                        "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("No se pudo crear el archivo de Costos. Intente de Nuevo", 
                        "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void pdf()
        {
        }
        public static void html(DataGridView dataGrid, string[] Cabecera)
        {
            int ban = 0;
            string Cadena = "";
            List<ListActividades> objActividad = new List<ListActividades>();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "HTML files (*.html)|*.html";
            saveFileDialog1.Title = Cabecera[1];
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                && saveFileDialog1.FileName.Length > 0)
            {
                try
                {
                    Funciones fnFunciones = new Funciones();
                    objActividad = fnFunciones.cargaListaActividades(objActividad);
                    //CREAMOS EL ARCHIVO CON EL NOMBRE Y RUTA PARA GUARDARLO
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                    //Se escribe el codigo HTML
                    sw.WriteLine("<html><title>" + saveFileDialog1.Title + "</title>");//Se abre el HTML y El TITULO se Establece
                    sw.WriteLine("<body>");//Se abre la etiqueta <BODY>

                    for (int i = 1; i < Cabecera.Length; i++)
                        sw.WriteLine(Cabecera[i]+"<br />");
                    sw.WriteLine("<table cellspacing='0' cellspading='0'>");//Se abre la etiqueta <TABLE>
                    switch (int.Parse(Cabecera[0]))
                    {
                        case 1://COSTO MANO DE OBRA LOTE-ACTIVIDAD
                            int contActividad = 0;
                            foreach (ListActividades actividad in objActividad)
                            {
                                if (contActividad == 0)
                                    Cadena += "<tr bgcolor='#25C0F3'><td>Lotes</td>";
                                Cadena += "<td>" + actividad.Actividad + "</td>";
                                contActividad = 1;
                            }
                            Cadena += "<td>TOTAL</td></tr>";
                            sw.WriteLine(Cadena);//ESTABLECE EL ENCABEZADO DE LA TABLA
                            int intercalado = 0;//Variable de control para el intercalado de color de las filas del reporte
                            for (int i = 0; i < dataGrid.Rows.Count; i++)
                            {
                                Cadena = "";                                
                                string tr_Color = "";
                                for (int x = 0; x < dataGrid.Columns.Count; x++)                                
                                    Cadena += "<td>" + dataGrid.Rows[i].Cells[x].Value + "</td>";
                                //*********INTERCALA EL COLOR DE LAS FILAS*************
                                if (intercalado == 0)
                                { tr_Color = ""; intercalado = 1; }
                                else if (intercalado == 1)
                                { tr_Color = " bgcolor='#A7EAFF'"; intercalado = 0; }
                                sw.WriteLine("<tr" + tr_Color + ">" + Cadena + "</tr>");
                            }
                            break;
                        case 2://Mano Obra Costo Planta
                            break;
                        case 3://Mano Obra Costo Hectarea

                            string[] Array_Cadena = new string[] { "", "Costos,Montos" };
                            for (int j = 0; j < 2; j++)
                                sw.WriteLine(Array_Cadena[j]);

                            for (int i = 0; i < dataGrid.Rows.Count; i++)
                            {
                                Cadena = "";
                                for (int x = 0; x < dataGrid.Columns.Count; x++)
                                    Cadena += dataGrid.Rows[i].Cells[x].Value + ",";
                                sw.WriteLine(Cadena);
                            }
                            break;
                        case 4://Mano Obra Costo Planta-Actividad
                            break;
                        case 5:
                            break;
                    }
                                  
                    sw.WriteLine("</table></body></html>");//SE CIERRAN LAS ETIQUETAS TABLE BODY Y HTML
                    //CERRAMOS EL ARCHIVO </HTML>
                    sw.Close();
                }
                catch (Exception ex)
                { ban = 1; }
                finally
                {
                    if (ban == 0)
                        MessageBox.Show("El Archivo " + saveFileDialog1.FileName + " ha sido creado",
                            "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("No se pudo crear el archivo de Costos. Intente de Nuevo", 
                            "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        /// <summary>
        /// Exclusivo para el Archivo de Piramidacion del Sistema Tress de Recursos Humanos
        /// </summary>
        public void piramidacion()
        {
        }
        public static void Nomina(DataGridView dataGrid, string[] Cabecera)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
            saveFileDialog1.Title = Cabecera[1];
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                && saveFileDialog1.FileName.Length > 0)
            {
                int ban = 0;
                try
                {
                    FileInfo t = new FileInfo(saveFileDialog1.FileName);
                    StreamWriter Tex = t.CreateText();
                    string Cadena = "";
                    for (int i = 0; i < Cabecera.Length; i++)
                        Cadena += Cabecera[i] + ",";
                    Tex.WriteLine(Cadena);
                    for (int i = 0; i < dataGrid.Rows.Count; i++)
                    {
                        Cadena = "";
                        for (int x = 0; x < dataGrid.Columns.Count; x++)
                            Cadena += dataGrid.Rows[i].Cells[x].Value + ",";
                        Tex.WriteLine(Cadena);
                    }
                }
                catch (Exception ex)
                { ban = 1; }
                if (ban == 0)
                    MessageBox.Show("El Archivo " + saveFileDialog1.FileName + " ha sido creado",
                        "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("No se pudo crear el archivo de Nomina. Intente de Nuevo",
                        "Sistema de Viñedos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
