using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.Office.Interop;
using System.IO;
using WindowsFormsApplication1;


namespace vinedos
{
    public partial class Consultas : Form
    {
        //Clase para almacenar las HE acumuladas en los Costos
        public class EmpleadoCosto
        {
            private int _id;
            private int _hestored;

            public int Id
            {
                set { _id = value; }
                get { return _id; }
            }
            public int HEstored
            {
                set { _hestored = value; }
                get { return _hestored; }
            }            

            public EmpleadoCosto(int id, int hestored)
            {
                this.Id = id;
                this.HEstored= hestored;
            }
        }

        //
        public class SalidaCosto
        {
            private string _producto;
            private string _um;
            private string _rancho;
            private double _importe;


            public string Producto
            {
                set { _producto = value; }
                get { return _producto; }
            }

            public string UM
            {
                set { _um = value; }
                get { return _um; }
            }

            public string Rancho
            {
                set { _rancho = value; }
                get { return _rancho; }
            }

            public double Importe
            {
                set { _importe = value; }
                get { return _importe; }
            }

            public SalidaCosto(string producto, string um, string rancho, double importe)
            {
                this.Producto = producto;
                this.UM = um;
                this.Rancho = rancho;
                this.Importe = importe;
            }
        }


        public Consultas()
        {
            InitializeComponent();
        }

        Funciones fnFunciones = new Funciones();
        static List<EmpleadoCosto> objEmpleadoCosto = new List<EmpleadoCosto>();
        static string _Rancho = "";
        ///
        /// ***********************************************************************
        /// Diferencia de Horas Extra en caso de tener dobles y triples en la misma
        /// ***********************************************************************
        ///
        public static double fnDiffHE(int _hestored,int _he,double _horadiaria)
        {
            int _diffhe = 9 - _hestored;
            return ((_diffhe * 2) * _horadiaria) + ((_he - _diffhe) * 3 * _horadiaria);
        }
        ///
        /// ***********************************************************************
        /// **************CALCULO DE LAS HORA EXTRA PARA LOS COSTOS****************
        /// ***********************************************************************
        ///
        public static double fnCostoHoraExtra(int _he, int _idemp, double _costohora)
        {                   
            double _costo = 0;
            try
            {
                foreach (EmpleadoCosto emp in objEmpleadoCosto)
                {
                    if (emp.Id == _idemp)
                    {
                        int _hestored = emp.HEstored;
                        if (_hestored > 9)
                        {
                            emp.HEstored += _he;
                            _costo = _he * 3 * _costohora;
                        }
                        else if (_hestored <= 9)
                        {
                            if ((_hestored + _he) > 9)
                            {
                                _costo = fnDiffHE(_hestored, _he, _costohora);
                                emp.HEstored += _he;
                            }
                            else
                            {
                                emp.HEstored += _he;
                                _costo = _he * 2 * _costohora;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }                                               
            return _costo;
        }

        //Crea una nueva columna con los parametros (Nombre,numero de columna) 
        public void showColumn(string Tipo, int numero)
        {
            DataGridViewColumn column = new DataGridViewColumn();
            column.Name = "Cell" + numero;
            if (Tipo != "Act-")
                column.HeaderText = Tipo;
            else
                column.HeaderText = Tipo + (numero + 1);
            DataGridViewCell cell = new DataGridViewTextBoxCell();
            cell.Style.BackColor = Color.White;
            column.CellTemplate = cell;
            dataGridView1.Columns.Add(column);            
        }
        //Simula un proceso de ESPERA**************
        public void wait()
        {
            Wait frmWait = new Wait();
            frmWait.ShowDialog();
        }
        //Carga combo Ranchos Inicial
        public void CargaComboRancho()
        {            
            clConexion objConexion = new clConexion();
            string query = "SELECT Clave, Rancho from R";
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
        }                     
        //******Costo Mano Obra->Costo Planta->Actividad******\\
        public void CostoManoObra_CostoPlanta_Actividad()
        {
            dataGridView1.Columns.Clear();
            Thread Hilo1 = new Thread(new ThreadStart(wait));
            Hilo1.Start();
            dataGridView1.Rows.Clear();
            double[,] CostoHectarea = new double[100, 10];
            this.WindowState = FormWindowState.Maximized;
            List<ListCostoManoObra> objCostoManoObra = new List<ListCostoManoObra>();
            List<ListLotes> objLotes = new List<ListLotes>();
            List<ListRanchos> objRanchos = new List<ListRanchos>();
            string RanchoValue = "";
            string query = "";//cambiar "" por query de busqueda en la DB
            button1.Enabled = false;
            objRanchos = fnFunciones.cargaRanchos(objRanchos);
            objCostoManoObra = fnFunciones.GeneraTablaCosto(objCostoManoObra, query);
            foreach (ListRanchos rancho in objRanchos)
                if (cbRancho.Text == rancho.Rancho)
                { RanchoValue = rancho.Clave; break; }
            objLotes = fnFunciones.cargaListalotes(objLotes, RanchoValue);
            showColumn("Costos", 0);
            int Columnas = 0;
            for (Columnas = 0; Columnas < objLotes.Count; Columnas++)
                showColumn(objLotes[Columnas].Clave, Columnas);
            showColumn("TOTAL", Columnas + 1);
            //int Renglon = dataGridView1.Rows.Add();
            int n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[0].Value = "Costo Mano Obra";
            n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[0].Value = "Hectareas";
            n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[0].Value = "Costo por Hectarea";
            Hilo1.Abort();
        }
        //*******Costo Mano Obra->Costo Hectarea**********\\
        /// <summary>**************************************************************************************************************
        /// COSTO POR HECTAREA POR MANO DE OBRA ***********************************************************************************
        /// </summary>*************************************************************************************************************
        public void CostoManoObra_CostoHectarea()
        {
            try
            {
                Thread Hilo1 = new Thread(new ThreadStart(wait));
                Hilo1.Start();
                dataGridView1.Rows.Clear();
                double CostoManoObra = 0;
                this.WindowState = FormWindowState.Maximized;
                List<ListCostoManoObra> objCostoManoObra = new List<ListCostoManoObra>();
                List<ListLotes> objLotes = new List<ListLotes>();
                List<ListRanchos> objRanchos = new List<ListRanchos>();
                List<ListActividades> objActividades = new List<ListActividades>();
                string queryCost = "SELECT Id_Empleado FROM Asistencia_Planta_Resp WHERE Rancho ='" +
                    _Rancho + "' GROUP BY Id_Empleado";
                objEmpleadoCosto = Funciones.fnEmpleadoCosto(objEmpleadoCosto, queryCost);
                string query = "SELECT Id_Empleado, Fecha, HE, Asistencias, Actividad, Lote1, Rancho " +
                    "FROM Asistencia_Planta_Resp WHERE Asistencias = 1 AND Rancho = '" + _Rancho + "'";
                float Rancho_Ha = 0;
                button1.Enabled = false;
                objRanchos = fnFunciones.cargaRanchos(objRanchos);
                objCostoManoObra = fnFunciones.GeneraTablaCosto(objCostoManoObra, query);//cambiar "" por query de busqueda en la DB
                objActividades = fnFunciones.cargaListaActividades(objActividades);
                showColumn("Costos", 0);
                showColumn("Montos", 1);                
                foreach (ListRanchos rancho in objRanchos)
                    if (cbRancho.Text == rancho.Rancho)
                    { Rancho_Ha = (float)rancho.Hectareaje; break; }                
                //ASIGNA LOS COSTOS AL ARREGLO
                int banderaInterna = 0;

                foreach (ListCostoManoObra costo in objCostoManoObra)
                {
                    CostoManoObra += Math.Round(costo.SueldoDiario + (costo.SueldoDiario * 0.1666666) +
                        costo.Otros + (costo.Otros * 0.1666666) + fnCostoHoraExtra((int)costo.HE, costo.IdEmpleado, costo.SueldoDiario / 8), 2);
                }
                
                
                ///
                ///
                /// GENERA LAS FILAS Y COLUMNAS EN EL GRID PARA POSTERIORMENTE COLOCAR LOS COSTOS SEGUN CORRESPONDA, APARTIR DEL ARREGLO
                /// CostoManoObra[];
                ///
                ///                               
                dataGridView1.Rows[dataGridView1.Rows.Add()].Cells[0].Value = "Mano Obra";
                dataGridView1.Rows[dataGridView1.Rows.Add()].Cells[0].Value = "Hectareas";
                dataGridView1.Rows[dataGridView1.Rows.Add()].Cells[0].Value = "Costo por Ha";               
               
                /**************************************************************************   
                  **   REALIZA CALCULOS E INSERTA EL TOTAL DEL RANCHO (ACTIVIDAD                             **
                  **************************************************************************/

                dataGridView1.Rows[0].Cells[1].Value = Math.Round(CostoManoObra, 2);
                dataGridView1.Rows[1].Cells[1].Value = Math.Round(Rancho_Ha, 2);
                dataGridView1.Rows[2].Cells[1].Value = Math.Round(CostoManoObra / Rancho_Ha, 2);

                label2.Text = cbRancho.Text;
                Hilo1.Abort();
            }
            //catch
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //*******Costo Mano Obra->Costo Planta*******\\
        public void CostoManoObra_CostoPlanta()
        {
            dataGridView1.Columns.Clear();
            Thread Hilo1 = new Thread(new ThreadStart(wait));
            Hilo1.Start();
            dataGridView1.Rows.Clear();
            double[,] CostoHectarea = new double[100, 10];
            this.WindowState = FormWindowState.Maximized;
            List<ListCostoManoObra> objCostoManoObra = new List<ListCostoManoObra>();
            List<ListLotes> objLotes = new List<ListLotes>();
            List<ListRanchos> objRanchos = new List<ListRanchos>();
            string RanchoValue = "";
            string query = "";/////cambiar "" por query de busqueda en la DB
            button1.Enabled = false;
            objRanchos = fnFunciones.cargaRanchos(objRanchos);
            objCostoManoObra = fnFunciones.GeneraTablaCosto(objCostoManoObra, query);
            foreach (ListRanchos rancho in objRanchos)
                if (cbRancho.Text == rancho.Rancho)
                { RanchoValue = rancho.Clave; break; }
            objLotes = fnFunciones.cargaListalotes(objLotes, RanchoValue);
            showColumn("Costos", 0);
            int Columnas = 0;
            for (Columnas = 0; Columnas < objLotes.Count; Columnas++)
                showColumn(objLotes[Columnas].Clave, Columnas);
            showColumn("TOTAL", Columnas + 1);
            //int Renglon = dataGridView1.Rows.Add();
            int n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[0].Value = "Costo Mano Obra";
            n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[0].Value = "Plantas";
            n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[0].Value = "Costo por Planta";
            Hilo1.Abort();
        }
        //Costo de Actividades x Lote x Rancho
        public void CostoRanchoLoteActividad()
        {
            try
            {
                Thread Hilo1 = new Thread(new ThreadStart(wait));
                Hilo1.Start();
                dataGridView1.Rows.Clear();
                double[,] CostoManoObra = new double[50, 100];
                this.WindowState = FormWindowState.Maximized;
                List<ListCostoManoObra> objCostoManoObra = new List<ListCostoManoObra>();
                List<ListLotes> objLotes = new List<ListLotes>();
                List<ListRanchos> objRanchos = new List<ListRanchos>();
                List<ListActividades> objActividades = new List<ListActividades>();
                string queryCost = "SELECT Id_Empleado FROM Asistencia_Planta_Resp WHERE Rancho ='" +
                    _Rancho + "' GROUP BY Id_Empleado";
                objEmpleadoCosto = Funciones.fnEmpleadoCosto(objEmpleadoCosto, queryCost);
                string query = "SELECT Id_Empleado, Fecha, HE, Asistencias, Actividad, Lote1, Rancho" +
                    " FROM Asistencia_Planta_Resp WHERE Asistencias = 1 AND Rancho = '" + _Rancho + "'";
                string RanchoValue = "";
                button1.Enabled = false;
                objRanchos = fnFunciones.cargaRanchos(objRanchos);
                objCostoManoObra = fnFunciones.GeneraTablaCosto(objCostoManoObra, query);//cambiar "" por query de busqueda en la DB
                objActividades = fnFunciones.cargaListaActividades(objActividades);
                showColumn("Lotes", 0);
                int numix = 0;
                for (numix = 0; numix < objActividades.Count; numix++)
                    showColumn(objActividades[numix].Actividad, numix);
                showColumn("TOTAL", numix + 1);
                foreach (ListRanchos rancho in objRanchos)
                    if (cbRancho.Text == rancho.Rancho)
                    { RanchoValue = rancho.Clave; break; }
                objLotes = fnFunciones.cargaListalotes(objLotes, RanchoValue);
                //ASIGNA LOS COSTOS AL ARREGLO
                int banderaInterna = 0;
                foreach (ListLotes lote in objLotes)
                {
                    foreach (ListCostoManoObra costo in objCostoManoObra)
                    {
                        if (lote.Clave == costo.Lote)
                            CostoManoObra[int.Parse(costo.Actividad), lote.Index] += Math.Round(costo.SueldoDiario + (costo.SueldoDiario * 0.1666666) +
                                costo.Otros + (costo.Otros * 0.1666666) + fnCostoHoraExtra((int)costo.HE, costo.IdEmpleado, costo.SueldoDiario / 8), 2);
                        else if (costo.Lote == "" && banderaInterna == 0)
                            CostoManoObra[int.Parse(costo.Actividad), objLotes.Count] += Math.Round(costo.SueldoDiario + (costo.SueldoDiario * 0.1666666) +
                            costo.Otros + (costo.Otros * 0.1666666) + fnCostoHoraExtra((int)costo.HE, costo.IdEmpleado, costo.SueldoDiario / 8), 1);
                    }
                    banderaInterna = 1;
                }
                ///
                ///
                /// GENERA LAS FILAS Y COLUMNAS EN EL GRID PARA POSTERIORMENTE COLOCAR LOS COSTOS SEGUN CORRESPONDA, APARTIR DEL ARREGLO
                /// CostoManoObra[];
                ///
                ///
                for (int j = 0; j < objLotes.Count; j++)
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = objLotes[j].Clave;
                    for (int i = 1; i <= objActividades.Count; i++)
                    {
                        dataGridView1.Rows[n].Cells[i].Value = 0;
                    }
                    if (j + 1 == objLotes.Count)
                    {
                        int x = dataGridView1.Rows.Add();
                        dataGridView1.Rows[x].Cells[0].Value = "General";
                        for (int i = 1; i <= objActividades.Count; i++)
                        {
                            dataGridView1.Rows[x].Cells[i].Value = 0;
                        }
                    }
                }

                /**************************************************************************   
                **   ASIGNA LOS COSTOS AL ARREGLO CORRESPONDIENDO COLUMNA Y RENGLON      **
                **************************************************************************/
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    double acumulador = 0;
                    int cont = 0;
                    for (int i = 1; i < dataGridView1.Columns.Count; i++)
                    {
                        dataGridView1.Rows[j].Cells[i].Value = Convert.ToDouble(CostoManoObra[i, j]);
                        acumulador += Convert.ToDouble(CostoManoObra[i, j]);
                        cont = i;
                    }
                    dataGridView1.Rows[j].Cells[cont].Value = acumulador;
                }
                /**************************************************************************   
                  **   CALCULA TOTALES POR COLUMNA (ACTIVIDAD                             **
                  **************************************************************************/
                int nn = dataGridView1.Rows.Add();
                dataGridView1.Rows[nn].Cells[0].Value = "TOTAL";
                for (int i = 1; i <= objActividades.Count + 1; i++)
                {
                    int x = 0;
                    double acumulador = 0;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        acumulador += Convert.ToDouble(dataGridView1.Rows[x].Cells[i].Value);
                        x++;
                    }
                    dataGridView1.Rows[nn].Cells[i].Value = acumulador;
                    textBox1.Text = acumulador.ToString("$###,###.##");
                }

                label2.Text = cbRancho.Text;
                Hilo1.Abort();
            }
            //catch
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*************************************************************************************************************************************************************
         * 
         * ********************************************************     COSTOS DE ALMACEN    *************************************************************************
         * 
         ************************************************************************************************************************************************************/
        string ExtraerCodigoProducto(string nom_producto)
        {
            string codigo = "";
            string query =
                 "SELECT Codigo FROM Articulos WHERE Producto = '" + nom_producto.Substring(0, nom_producto.Length - 3) + "'";
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
                            codigo = dr[0].ToString();                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return codigo;
        }


        void Costo_Almacen_Rancho()
        {
            try
            {
                string[,] SubCategorias =
                { 
                    {"Fungicidas","101"},
                    {"Insecticidas","102"},
                    {"Nematicidas","103"},
                    {"Herbicidas","104"},
                    {"Foliares","105"},
                    {"Adherentes","106"},
                    {"Bactericidas","107"},
                    {"Fertilizantes","108"},
                    {"Mejoradores de Suelos","109"},
                    {"Repelentes","110"}
                };

                WindowsFormsApplication1.FuncionesProductos funciones = new WindowsFormsApplication1.FuncionesProductos();
                Thread Hilo1 = new Thread(new ThreadStart(wait));
                Hilo1.Start();
                dataGridView1.Rows.Clear();
                this.WindowState = FormWindowState.Maximized;                

                List<ListRanchos> objRanchos = new List<ListRanchos>();                                               
                objRanchos = fnFunciones.cargaRanchos(objRanchos);                

                showColumn("", -1);
                for (int Columnas = 0; Columnas < objRanchos.Count; Columnas++)
                    showColumn(objRanchos[Columnas].Rancho, Columnas);
                
                //COLOCACION Y ORDENAMIENTO DE LAS FILAS

                
                for (int cat = 0; cat < 0.5 * SubCategorias.Length; cat++)
                {
                    int ban = 0; 
                    List<WindowsFormsApplication1.ListProductos> objProducto = new List<WindowsFormsApplication1.ListProductos>();
                    objProducto = funciones.llenarProducto(objProducto, SubCategorias[cat, 1]);                    

                    foreach (WindowsFormsApplication1.ListProductos producto in objProducto)
                    {                        
                        int n = dataGridView1.Rows.Add();
                        if (ban == 0)
                        {
                            dataGridView1.Rows[n].Cells[0].Value = SubCategorias[cat, 0];
                            int columna = 0;
                            foreach (ListRanchos rancho in objRanchos)
                            {
                                columna += 1;
                                dataGridView1.Rows[n].Cells[columna].Value = "-";
                            }
                            ban = 1;
                        }
                        else
                        {
                            dataGridView1.Rows[n].Cells[0].Value = producto.Producto + " " + producto.UM;
                        }
                        
                    }
                }

                //BUSQUEDA EN LAS SALIDAS Y COLOCACION DE COSTOS SEGUN EL AGROQUIMICO Y EL RANCHO
                
                for (int fila = 0; fila < dataGridView1.Rows.Count; fila++)
                {

                    string producto = dataGridView1.Rows[fila].Cells[0].Value.ToString();

                    int es_sub_cat = 0;
                    //VERIFICA QUE NO SE TRATE DEL NOMBRE DE LA CATEGORIA
                    for (int j = 0; j < 0.5 * SubCategorias.Length; j++)
                        if (producto == SubCategorias[j, 0])
                        {
                            es_sub_cat = 1;
                            break;
                        }

                    if (es_sub_cat == 0)
                    {
                        List<SalidaCosto> objSalida = new List<SalidaCosto>();
                        objSalida = fnFunciones.fillSalidas(objSalida, ExtraerCodigoProducto(producto));
                        //objSalida = fnFunciones.fillSalidas(objSalida, "100101005");
                        //double importe = 0;
                        //foreach (SalidaCosto salida in objSalida)
                        //    importe += salida.Importe;
                        //dataGridView1.Rows[fila].Cells[1].Value = importe + " " + producto;                        

                        foreach (SalidaCosto salida in objSalida)
                        {
                            int columna = 0;
                            foreach(ListRanchos rancho in objRanchos)
                            {
                                columna += 1;
                                if (salida.Rancho == rancho.Rancho)
                                {
                                    double var = 0;
                                    try
                                    {
                                        var = Convert.ToDouble(dataGridView1.Rows[fila].Cells[columna].Value);
                                    }
                                    catch (Exception exs)
                                    {
                                        //MessageBox.Show("Error en la Celda[" + columna + "," + fila + "]");
                                        var = 0;
                                    }
                                    finally
                                    {
                                        dataGridView1.Rows[fila].Cells[columna].Value = var + Math.Round(salida.Importe, 2);
                                    }
                                }                                
                            }
                        }
                    }
                }
                
                Hilo1.Abort();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message + ". Error"); }
        }

        void costo_agroquimico_lote_ha()
        {
            try
            {
                string[,] SubCategorias =
                { 
                    {"Fungicidas","101"},
                    {"Insecticidas","102"},
                    {"Nematicidas","103"},
                    {"Herbicidas","104"},
                    {"Foliares","105"},
                    {"Adherentes","106"},
                    {"Bactericidas","107"},
                    {"Fertilizantes","108"},
                    {"Mejoradores de Suelos","109"},
                    {"Repelentes","110"}
                };

                WindowsFormsApplication1.FuncionesProductos funciones = new WindowsFormsApplication1.FuncionesProductos();
                Thread Hilo1 = new Thread(new ThreadStart(wait));
                Hilo1.Start();
                dataGridView1.Rows.Clear();
                this.WindowState = FormWindowState.Maximized;               

                List<ListRanchos> objRanchos = new List<ListRanchos>();
                objRanchos = fnFunciones.cargaRanchos(objRanchos);

                showColumn("Rancho/Lote/Ha", -1);
                for (int Columnas = 0; Columnas < 0.5 * SubCategorias.Length; Columnas++)
                    showColumn(SubCategorias[Columnas, 0], Columnas);

                //COLOCACION Y ORDENAMIENTO DE LAS FILAS

                for (int ran = 0; ran < objRanchos.Count; ran++)
                {
                    int ban = 0;
                    List<ListLotes> objLote = new List<ListLotes>();
                    objLote = fnFunciones.cargaListalotes(objLote, objRanchos[ran].Clave);                    

                    foreach (ListLotes lote in objLote)
                    {
                        int n = dataGridView1.Rows.Add();
                        if (ban == 0)
                        {
                            dataGridView1.Rows[n].Cells[0].Value = objRanchos[ran].Rancho;
                            
                            for (int Columnas = 1; Columnas <= 0.5 * SubCategorias.Length; Columnas++)                            
                                dataGridView1.Rows[n].Cells[Columnas].Value = "-";                            
                            ban = 1;
                        }
                        else
                        {
                            dataGridView1.Rows[n].Cells[0].Value = lote.Clave + " | " + lote.Hectareaje;
                        }

                    }
                }

                //BUSQUEDA EN LAS SALIDAS Y COLOCACION DE COSTOS SEGUN EL AGROQUIMICO Y EL RANCHO
                string ranch = "";
                for (int fila = 0; fila < dataGridView1.Rows.Count; fila++)
                {

                    string celda = dataGridView1.Rows[fila].Cells[0].Value.ToString();

                    bool es_rancho = false;                    
                    //VERIFICA QUE NO SE TRATE DEL NOMBRE DEL RANCHO
                    for (int j = 0; j < objRanchos.Count; j++)
                        if (celda == objRanchos[j].Rancho)
                        {
                            es_rancho = true;
                            ranch = objRanchos[j].Rancho;
                            break;
                        }

                    if (!es_rancho)
                    {
                        List<SalidaCosto> objSalida = new List<SalidaCosto>();
                        objSalida = fnFunciones.fillSalidas(objSalida, ranch, celda.Substring(0, 4));
                        //objSalida = fnFunciones.fillSalidas(objSalida, "100101005");
                        //double importe = 0;
                        //foreach (SalidaCosto salida in objSalida)
                        //    importe += salida.Importe;
                        //dataGridView1.Rows[fila].Cells[1].Value = importe + " " + producto;                        

                        foreach (SalidaCosto salida in objSalida)
                        {
                            
                            for(int columna = 1 ;columna <= 0.5 * SubCategorias.Length; columna++)                            
                            {
                                string prod_cat = salida.Producto.Substring(3, 3);
                                string sub__cat = SubCategorias[columna - 1, 1];
                                if (salida.Producto.Substring(3, 3) == SubCategorias[columna - 1, 1])
                                {
                                    double var = 0;
                                    try
                                    {
                                        var = Convert.ToDouble(dataGridView1.Rows[fila].Cells[columna].Value);
                                    }
                                    catch (Exception exs)
                                    {
                                        //MessageBox.Show("Error en la Celda[" + columna + "," + fila + "]");
                                        var = 0;
                                    }
                                    finally
                                    {
                                        dataGridView1.Rows[fila].Cells[columna].Value = var + Math.Round(salida.Importe, 2);
                                    }
                                }
                            }
                        }
                    }
                }

                Hilo1.Abort();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message + ". Error"); }
        }

        public void CargaCombos(string TextoCombo, int Combo_a_Cargar)
        {
            clConexion objConexion = new clConexion();
            string query = "";
            try
            {
                switch (Combo_a_Cargar)
                {
                    case 1:
                        query = "SELECT * from Filtro1";
                        using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            comboBox1.DataSource = dt;
                            comboBox1.ValueMember = dt.Columns[0].ToString();
                            comboBox1.DisplayMember = dt.Columns[1].ToString();
                        }
                        break;
                    case 2:
                        query = "SELECT Filtro2 from Filtro2 WHERE Ref = '" + TextoCombo + "'";
                        using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            comboBox2.DataSource = dt;
                            comboBox2.ValueMember = dt.Columns[0].ToString();
                            comboBox2.DisplayMember = dt.Columns[0].ToString();
                        }
                        break;
                    case 3:
                        query = "SELECT Filtro3 from Filtro3 WHERE Ref = '" + TextoCombo + "'";
                        using (SqlDataAdapter adapter = new SqlDataAdapter(query, objConexion.conexion()))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            comboBox3.DataSource = dt;
                            comboBox3.ValueMember = dt.Columns[0].ToString();
                            comboBox3.DisplayMember = dt.Columns[0].ToString();
                        }
                        break;
                    default:                        
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("No puede cargar Datos. " + ex.Message);
            }
        }    

        private void Consultas_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Usuario: " + LogUsers.getUserName();
            radioButton1.Checked = true;
            CargaComboRancho();
            //this.Size = new System.Drawing.Size(430, 120);            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _Rancho = cbRancho.Text;
            int banExportar = 0;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            if (_Rancho == "-")
            {
                if (comboBox1.Text == "Almacen" && comboBox2.Text == "Agroquimicos" && comboBox3.Text == "-")
                { Costo_Almacen_Rancho(); dataGridView1.Visible = true; }
                if (comboBox1.Text == "Almacen" && comboBox2.Text == "Agroquimicos" && comboBox3.Text == "Variedad/Ha")
                { costo_agroquimico_lote_ha(); dataGridView1.Visible = true; }
            }
            else
            {
                if (comboBox1.Text == "Mano Obra" && comboBox2.Text == "-" && comboBox3.Text == "-")
                { CostoRanchoLoteActividad(); dataGridView1.Visible = true; }
                else if (comboBox1.Text == "Mano Obra" && comboBox2.Text == "Costo Hectarea" && comboBox3.Text == "-")
                { CostoManoObra_CostoHectarea(); dataGridView1.Visible = true; }
                else if (comboBox1.Text == "Mano Obra" && comboBox2.Text == "Costo Planta" && comboBox3.Text == "-")
                { CostoManoObra_CostoPlanta(); dataGridView1.Visible = true; }
                else
                {
                    MessageBox.Show("Debe Seleccionar un criterio valido");
                    banExportar = 1;
                }
            }
            if (banExportar == 0)
                button3.Enabled = true;
        }

        private void cbRancho_SelectedIndexChanged(object sender, EventArgs e)
        {
            //button1.Enabled = true;
            //button1_Click(null, null);            
            CargaCombos("", 1);
        }

        private int ordenarPorId(ListCostoManoObra P1, ListCostoManoObra P2)
        {
            return P1.IdEmpleado.CompareTo(P2.IdEmpleado);            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*List<ListCostoManoObra> objCostoManoObra= new List<ListCostoManoObra>();
            objCostoManoObra = fnFunciones.GeneraTablaCosto(objCostoManoObra, cbRancho.Text);
            objCostoManoObra.Sort(ordenarPorId);
            dataGridView2.DataSource = objCostoManoObra;            
            //dataGridView2.DataBind();*/
            _Rancho = cbRancho.Text;
            double total = 0;
            for (int i = 0; i < 6; i++)
            {
                if (i == 2)
                    total += fnCostoHoraExtra(1, 40227, 130.73 / 8);
                else
                    total += fnCostoHoraExtra(3, 40227, 130.73 / 8);
            }
            MessageBox.Show("40227: " + total);
            for (int i = 0; i < 6; i++)
            {
                if (i == 2)
                    total += fnCostoHoraExtra(1, 40288, 130.73 / 8);
                else
                    total += fnCostoHoraExtra(3, 40288, 130.73 / 8);
            }
            MessageBox.Show("40288: " + total);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            comboBox2.Enabled = true;
            CargaCombos(comboBox1.Text, 2);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = true;
            CargaCombos(comboBox2.Text, 3);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] Cabecera = new string[4];
            try
            {                            
                if (comboBox1.Text == "Mano Obra")
                {
                    if (comboBox2.Text == "-")
                        Cabecera = new string[] { "1", "HOJA DE COSTOS LOTE-ACTIVIDAD", "RANCHO: " +
                            cbRancho.Text, "USUARIO: " + LogUsers._username };
                    if (comboBox2.Text == "Costo Hectarea")
                        Cabecera = new string[] { "3", "HOJA DE COSTOS POR HECTAREA", "RANCHO: " +
                            cbRancho.Text, "USUARIO: " + LogUsers._username };
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally
            {
                Exportar.excel(dataGridView1, Cabecera);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] Cabecera = new string[4];
            try
            {
                if (comboBox1.Text == "Mano Obra")
                {
                    if (comboBox2.Text == "-")
                        Cabecera = new string[] { "1", "HOJA DE COSTOS LOTE-ACTIVIDAD", "RANCHO: " + 
                            cbRancho.Text, "USUARIO: " + LogUsers._username };
                    if (comboBox2.Text == "Costo Hectarea")
                        Cabecera = new string[] { "3", "HOJA DE COSTOS POR HECTAREA", "RANCHO: " +
                            cbRancho.Text, "USUARIO: " + LogUsers._username };
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally
            {
                Exportar.html(dataGridView1, Cabecera);
            }
        }
        //MENU EXPORTAR
        private void excelcsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            List<ListRanchos> objRanchos = new List<ListRanchos>();                                               
            objRanchos = fnFunciones.cargaRanchos(objRanchos);
            int row = 0;

            string[] data = { };

            string[] header = new string[objRanchos.Count + 1];
            header[0] = "";
            foreach (ListRanchos rancho in objRanchos)
            {
                row += 1;
                header[row] = rancho.Rancho;
            }
            ExportarAlmacen.ExportarExcel(dataGridView1, comboBox2.Text, data, header);
             
            /*
             * 
             * 
             * 
             * 
             * 
             * */

            string[] data = { };
            string[] header =
                { 
                    "Fungicidas",
                    "Insecticidas",
                    "Nematicidas",
                    "Herbicidas",
                    "Foliares",
                    "Adherentes",
                    "Bactericidas",
                    "Fertilizantes",
                    "Mejoradores de Suelos",
                    "Repelentes",
                };
            ExportarAlmacen.ExportarExcel(dataGridView1, comboBox3.Text, data, header);
        }

        private void webhtmlToolStripMenuItem_Click(object sender, EventArgs e)
        {/*
            List<ListRanchos> objRanchos = new List<ListRanchos>();
            objRanchos = fnFunciones.cargaRanchos(objRanchos);
            int row = 0;

            string[] data = { };
            string[] header = new string[objRanchos.Count + 1];

            foreach (ListRanchos rancho in objRanchos)
            {
                row += 1;
                header[row] = rancho.Rancho;
            }
            ExportarAlmacen.ExportarWeb(dataGridView1, comboBox2.Text, data, header);
            /*3
             * 
             * 
             * 
             * */                       
            string[] data = { };
            string[] header =
                { 
                    "Fungicidas",
                    "Insecticidas",
                    "Nematicidas",
                    "Herbicidas",
                    "Foliares",
                    "Adherentes",
                    "Bactericidas",
                    "Fertilizantes",
                    "Mejoradores de Suelos",
                    "Repelentes",
                };
            ExportarAlmacen.ExportarWeb(dataGridView1, comboBox3.Text, data, header);



        }
    }
}
