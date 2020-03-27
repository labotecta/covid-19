using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Contagio
{
    public partial class Form1 : Form
    {
        private readonly string[] nombre_estados = { "Sano", "Infectado", "Curado", "Desinmunizado", "Muerto" };
        public string TITULO;
        public string U_SALIDAS;
        public string PREFIJO;
        private readonly char[] ESPECIAL = { ':', '/', '\\' };
        public string CARPETA_SALIDAS = "Contagio";
        private string CARPETA_CASO;
        private string F_CASO;
        private string F_GRUPOS;
        private bool TABLA_VECINOS;
        private long INDIVIDUOS;
        private int RADIO;
        private double PROB_CONTAGIO;
        private double PROB_RECONTAGIO;
        private int MIN_INMUNIDAD = 0;
        private int LATENCIA;
        private int N_FOCOS_IMPORTADOS;
        private int DIAS_FOCOS_IMPORTADOS;
        private int GRUPO_IMPORTADO;
        private int N_INMUNES_INICIALES;
        private int N_FOCOS_INICIALES;
        public int CONTACTO;
        private int CONTACTO2;
        public int LON_10PASOS;
        private double LON_PASO;
        private double LON_PASO2;
        private int DIAS;
        private double PROB_CONTAGIO_C;
        private double PROB_CONTAGIO_Cd2;
        private int NUMERO_CLUSTERS_DATO;
        private int NUMERO_CLUSTERS;
        private double FACTOR_INDIVIDUOS_CLUSTER;
        private int[] clusters_grupo;
        private const int MAX_CLUSTERS = 1000;
        private const int MAX_INDIVIDUOS_CLUSTER = 1000;
        private readonly int[] n_individuos_cluster = new int[MAX_CLUSTERS];
        private readonly int[] grupo_cluster = new int[MAX_CLUSTERS];
        private readonly int[,] individuos_cluster_id = new int[MAX_CLUSTERS, MAX_INDIVIDUOS_CLUSTER];
        private readonly long[] intentos_cluster = new long[MAX_CLUSTERS];
        private readonly long[] infectados_cluster = new long[MAX_CLUSTERS];
        private long infectados_total_clusters;
        private long curados_clusters_actuales;
        private long muertos_clusters_actuales;
        int num_dias_exportar;
        int prox_dia_exportar;
        private int[] dias_exportar;
        private long sanos;
        public long desinmunizados;
        public long infectados;
        public long curados;
        public long importados;
        public long muertos;
        private long acumulador_infectados;
        private int max_pasos_dia;
        private long total_pasos;
        private long total_recorridos;
        private double total_distancias_ida;
        private long total_contactos_de_riesgo;
        private long total_dias_infectados_curados;
        private long total_dias_infectados_muertos;
        private long[] n_sorteos_fin_infeccion;
        private long[] n_fin_infeccion;
        private long[,] res_fin_infeccion;
        private double max_r0;
        private Form2 lzo_ex;
        public bool cancelar;
        private Random azar;
        /*
         * y = a * x ^ p entre 0 y n
         * a = 1 / (n ^ p)
         */
        private double POTENCIA;

        private const int MAX_CONVALECENCIA = 200;
        private double[,] PROB_FIN_INFECCION;   // grupo, dias
        private bool poblacion_creada;
        private bool tabla_vecinos_creada;
        private bool ignorar_cambio;
        public class Grupo
        {
            public int ID;
            public string grupo;
            public int genero;
            public int edad_min;
            public int edad_max;
            public double prob_contagio;
            public int duracion_infeccion;
            public double prob_curacion;
            public int pasos_min;
            public int pasos_max;
            public double fraccion_poblacion;
            public double fraccion_clusters;
            public int individuos_c;
            public Grupo(int ID, string grupo, int genero, int edad_min, int edad_max, double prob_contagio, int duracion_infeccion, double prob_curacion, int pasos_min, int pasos_max, double fraccion_poblacion, double fraccion_clusters, int individuos_c)
            {
                this.ID = ID;
                this.grupo = grupo;
                this.genero = genero;
                this.edad_min = edad_min;
                this.edad_max = edad_max;
                this.prob_contagio = prob_contagio;
                this.duracion_infeccion = duracion_infeccion;
                this.prob_curacion = prob_curacion;
                this.pasos_min = pasos_min;
                this.pasos_max = pasos_max;
                this.fraccion_poblacion = fraccion_poblacion;
                this.fraccion_clusters = fraccion_clusters;
                this.individuos_c = individuos_c;
            }
        }
        public readonly List<Grupo> grupos = new List<Grupo>();
        public class Individuo
        {
            public int ID;
            public int grupo_ID;
            public string grupo;
            public double xi;
            public double yi;
            /*
             *  0 = Sano
             *  1 = Infectado
             *  2 = Curado
             *  3 = Desinmunizado
             * -1 = Muerto
             */
            public int estado;
            public int dias_infectado;
            public int dias_curado;
            public int enfermados;
            public int pasos_dia;
            /*
             *  0 = Ida
             *  1 = Vuelta
             * -1 = Quieto
             */
            public int sentido;
            public double incx;
            public double incy;
            public double incd;
            public double x;
            public double y;
            public int pasos_dados;
            public double[,] pasos;
            public double distancia;
            public int cluster;
            public bool contagio_en_cluster;
            public Individuo(int ID, int grupo_ID, string grupo, double xi, double yi, int estado)
            {
                this.ID = ID;
                this.grupo_ID = grupo_ID;
                this.grupo = grupo;
                this.xi = xi;
                this.yi = yi;
                this.estado = estado;
                dias_infectado = 0;
                dias_curado = 0;
                enfermados = 0;
                pasos_dia = 0;
                sentido = 0;
                incx = 0.0;
                incy = 0.0;
                incd = 0.0;
                x = xi;
                y = yi;
                pasos_dados = 0;
                pasos = null;
                distancia = 0.0;
                cluster = -1;
                contagio_en_cluster = false;
            }
            public Individuo(int ID, int grupo_ID, string grupo, double xi, double yi, int estado, int dias_infectado, int dias_curado, int enfermados, int cluster, bool contagio_en_cluster)
            {
                this.ID = ID;
                this.grupo_ID = grupo_ID;
                this.grupo = grupo;
                this.xi = xi;
                this.yi = yi;
                this.estado = estado;
                this.dias_infectado = dias_infectado;
                this.dias_curado = dias_curado;
                this.enfermados = enfermados;
                pasos_dia = 0;
                sentido = 0;
                incx = 0.0;
                incy = 0.0;
                incd = 0.0;
                x = xi;
                y = yi;
                pasos_dados = 0;
                pasos = null;
                distancia = 0.0;
                this.cluster = cluster;
                this.contagio_en_cluster = contagio_en_cluster;
            }
        }
        public readonly List<Individuo> individuos = new List<Individuo>();

        private int tabla_alto_fila;
        private int tabla_alto_cabecera;

        private const int MAX_INDIVIDUOS_PARA_TABLA = 50001;
        private const int MAX_VECINOS_PARA_TABLA = 1001;
        private readonly int[] nvecinas = new int[MAX_INDIVIDUOS_PARA_TABLA + 1];
        private readonly int[,] vecinas = new int[MAX_INDIVIDUOS_PARA_TABLA + 1, MAX_VECINOS_PARA_TABLA + 1];
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.Manual;
            Left = 0;
            Top = 0;
            Text = TITULO = string.Format("Simulador de contagios. {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Desactiva(true);
            b_cancelar.Enabled = false;
            Show();
            linea_estado.Text = "Espera por favor ...";
            Oculta(true);
            Application.DoEvents();
            tabla_alto_fila = tablaGrupos.RowTemplate.Height;
            tabla_alto_cabecera = tablaGrupos.ColumnHeadersHeight;
            if (tabla_alto_cabecera <= tabla_alto_fila) tabla_alto_cabecera = tabla_alto_fila + 1;
            IniciaTablaGrupos(tablaGrupos);
            U_SALIDAS = Properties.Settings.Default.u_salidas;
            if (string.IsNullOrEmpty(U_SALIDAS))
            {
                U_SALIDAS = "C";
            }
            selunidad.SelectedItem = U_SALIDAS;
            senda_caso.Text = F_CASO = string.Empty;
            CARPETA_CASO = string.Empty;
            senda_grupos.Text = F_GRUPOS = string.Empty;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.ftecaso))
            {
                senda_caso.Text = F_CASO = Properties.Settings.Default.ftecaso;
                CARPETA_CASO = Path.GetDirectoryName(F_CASO);
                Application.DoEvents();
                if (LeeCaso(F_CASO))
                {
                    LeeTodo();
                }
            }
            string s = string.Format(@"{0}:\{1}", U_SALIDAS, CARPETA_SALIDAS);
            if (!Directory.Exists(s)) Directory.CreateDirectory(s);
            b_cancelar.Enabled = false;
            Oculta(false);
            Desactiva(false);
            linea_estado.Text = string.Empty;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SalvaConfiguracion();
        }
        private void SalvaConfiguracion()
        {
            Properties.Settings.Default.u_salidas = U_SALIDAS;
            Properties.Settings.Default.ftecaso = F_CASO;
            Properties.Settings.Default.Save();
        }
        private void Desactiva(bool que)
        {
            prefijo.Enabled = !que;
            d_radio.Enabled = !que;
            tablaGrupos.Enabled = !que;
            sel_caso.Enabled = !que;
            sel_grupos.Enabled = !que;
            selunidad.Enabled = !que;
            b_salva_grupos.Enabled = !que;
            b_simula.Enabled = !que;
            d_individuos.Enabled = !que;
            b_salva_caso.Enabled = !que;
            d_dias.Enabled = !que;
            d_lon_paso.Enabled = !que;
            d_contacto.Enabled = !que;
            b_crea_poblacion.Enabled = !que;
            b_crea_clusters.Enabled = !que;
            d_potencia.Enabled = !que;
            d_contagio.Enabled = !que;
            d_recontagio.Enabled = !que;
            d_mininmunidad.Enabled = !que;
            d_latencia.Enabled = !que;
            b_importa_individuos.Enabled = !que;
            b_exporta_individuos.Enabled = !que;
            d_focos.Enabled = !que;
            d_inmunes.Enabled = !que;
            d_importados.Enabled = !que;
            d_dias_importados.Enabled = !que;
            d_grupo_importado.Enabled = !que;
            tabla_vecinos_en_uso.Enabled = !que;
            d_contagio_c.Enabled = !que;
            d_numero_c.Enabled = !que;
            d_individuos_c.Enabled = !que;
            d_dias_exportar.Enabled = !que;
        }
        private void Oculta(bool que)
        {
            prefijo.Visible = !que;
            d_radio.Visible = !que;
            tablaGrupos.Visible = !que;
            sel_caso.Visible = !que;
            sel_grupos.Visible = !que;
            selunidad.Visible = !que;
            b_salva_grupos.Visible = !que;
            b_simula.Visible = !que;
            d_individuos.Visible = !que;
            b_salva_caso.Visible = !que;
            d_dias.Visible = !que;
            d_lon_paso.Visible = !que;
            d_contacto.Visible = !que;
            b_crea_poblacion.Visible = !que;
            b_crea_clusters.Visible = !que;
            d_potencia.Visible = !que;
            d_contagio.Visible = !que;
            d_recontagio.Visible = !que;
            d_mininmunidad.Visible = !que;
            d_latencia.Visible = !que;
            b_importa_individuos.Visible = !que;
            b_exporta_individuos.Visible = !que;
            d_focos.Visible = !que;
            d_inmunes.Visible = !que;
            d_importados.Visible = !que;
            d_dias_importados.Visible = !que;
            d_grupo_importado.Visible = !que;
            tabla_vecinos_en_uso.Visible = !que;
            d_contagio_c.Visible = !que;
            d_numero_c.Visible = !que;
            d_individuos_c.Visible = !que;
            mostrar_recorridos.Visible = !que;
            puntos_gordos.Visible = !que;
            salvar_imagenes.Visible = !que;
            d_dias_exportar.Visible = !que;
        }
        private string FicheroEscritura(string fichero, string filtro)
        {
            SaveFileDialog ficheroescritura = new SaveFileDialog()
            {
                FileName = fichero,
                Filter = filtro,
                FilterIndex = 1,
            };
            ficheroescritura.RestoreDirectory = ficheroescritura.OverwritePrompt = false;
            ficheroescritura.CheckPathExists = true;
            if (ficheroescritura.ShowDialog() == DialogResult.OK)
            {
                return ficheroescritura.FileName;
            }
            return null;
        }
        #region Caso
        private void Sel_caso_Click(object sender, EventArgs e)
        {
            OpenFileDialog leefichero = new OpenFileDialog
            {
                Filter = "CCC (*.ccc)|*.ccc|All files (*.*)|*.*",
                CheckFileExists = false,
                Multiselect = false
            };
            if (leefichero.ShowDialog() == DialogResult.OK)
            {
                F_CASO = leefichero.FileName;
                CARPETA_CASO = Path.GetDirectoryName(F_CASO);
                if (!File.Exists(F_CASO))
                {
                    // Caso nuevo

                    SalvaCaso(F_CASO);
                    senda_caso.Text = F_CASO;
                    return;
                }
                else
                {
                    string tmp = senda_caso.Text;
                    senda_caso.Text = F_CASO;
                    Application.DoEvents();
                    if (LeeCaso(F_CASO))
                    {
                        if (LeeTodo())
                        {
                            senda_caso.Text = F_CASO;
                            SalvaConfiguracion();
                            return;
                        }
                        else
                        {
                            senda_caso.Text = tmp;
                        }
                    }
                    else
                    {
                        senda_caso.Text = tmp;
                    }
                }
                F_CASO = senda_caso.Text;
                CARPETA_CASO = Path.GetDirectoryName(F_CASO);
            }
        }
        private bool LeeCaso(string F_CASO)
        {
            if (!File.Exists(F_CASO))
            {
                MessageBox.Show("No se encuentra el fichero " + F_CASO, "Lectura de caso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            FileStream fr = new FileStream(F_CASO, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fr);
            ignorar_cambio = true;
            try
            {
                CARPETA_CASO = Path.GetDirectoryName(F_CASO);
                U_SALIDAS = sr.ReadLine();
                if (string.IsNullOrEmpty(U_SALIDAS))
                {
                    U_SALIDAS = "C";
                }
                selunidad.SelectedItem = U_SALIDAS;
                prefijo.Text = PREFIJO = sr.ReadLine();
                F_GRUPOS = sr.ReadLine();
                if (F_GRUPOS.IndexOfAny(ESPECIAL) == -1)
                {
                    // Senda relativa

                    F_GRUPOS = Path.Combine(CARPETA_CASO, F_GRUPOS);
                }
                senda_grupos.Text = F_GRUPOS;
                d_individuos.Text = sr.ReadLine();
                d_radio.Text = sr.ReadLine();
                d_potencia.Text = sr.ReadLine();
                d_contagio.Text = sr.ReadLine();
                d_recontagio.Text = sr.ReadLine();
                d_mininmunidad.Text = sr.ReadLine();
                d_latencia.Text = sr.ReadLine();
                d_focos.Text = sr.ReadLine();
                d_inmunes.Text = sr.ReadLine();
                d_importados.Text = sr.ReadLine();
                d_dias_importados.Text = sr.ReadLine();
                d_grupo_importado.Text = sr.ReadLine();
                d_contacto.Text = sr.ReadLine();
                d_lon_paso.Text = sr.ReadLine();
                d_dias.Text = sr.ReadLine();
                d_contagio_c.Text = sr.ReadLine();
                d_numero_c.Text = sr.ReadLine();
                d_individuos_c.Text = sr.ReadLine();
                sr.Close();
                Application.DoEvents();
                ignorar_cambio = false;
                if (!Parametros())
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Leer caso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ignorar_cambio = false;
                return false;
            }
        }
        private bool LeeTodo()
        {
            if (!LeeGrupos(F_GRUPOS))
            {
                return false;
            }
            return true;
        }
        private void B_salva_caso_Click(object sender, EventArgs e)
        {
            string fichero = FicheroEscritura(F_CASO, "CCC (*.ccc)|*.ccc|All files (*.*)|*.*");
            if (!string.IsNullOrEmpty(fichero))
            {
                if (SalvaCaso(fichero))
                {
                    senda_caso.Text = F_CASO = fichero;
                    CARPETA_CASO = Path.GetDirectoryName(F_CASO);
                    MessageBox.Show("OK", "Actualizar caso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private bool SalvaCaso(string F_CASO)
        {
            ignorar_cambio = true;
            if (Parametros())
            {
                try
                {
                    FileStream fw = new FileStream(F_CASO, FileMode.Create, FileAccess.Write, FileShare.Read);
                    StreamWriter sw = new StreamWriter(fw);
                    CARPETA_CASO = Path.GetDirectoryName(F_CASO);
                    sw.WriteLine(selunidad.SelectedItem);
                    sw.WriteLine(prefijo.Text);
                    string senda = Path.GetDirectoryName(senda_grupos.Text);
                    if (CARPETA_CASO.Equals(senda, StringComparison.OrdinalIgnoreCase))
                    {
                        // Senda relativa

                        sw.WriteLine(Path.GetFileName(F_GRUPOS));
                    }
                    else
                    {
                        sw.WriteLine(senda_grupos.Text);
                    }
                    sw.WriteLine(d_individuos.Text);
                    sw.WriteLine(d_radio.Text);
                    sw.WriteLine(d_potencia.Text);
                    sw.WriteLine(d_contagio.Text);
                    sw.WriteLine(d_recontagio.Text);
                    sw.WriteLine(d_mininmunidad.Text);
                    sw.WriteLine(d_latencia.Text);
                    sw.WriteLine(d_focos.Text);
                    sw.WriteLine(d_inmunes.Text);
                    sw.WriteLine(d_importados.Text);
                    sw.WriteLine(d_dias_importados.Text);
                    sw.WriteLine(d_grupo_importado.Text);
                    sw.WriteLine(d_contacto.Text);
                    sw.WriteLine(d_lon_paso.Text);
                    sw.WriteLine(d_dias.Text);
                    sw.WriteLine(d_contagio_c.Text);
                    sw.WriteLine(d_numero_c.Text);
                    sw.WriteLine(d_individuos_c.Text);
                    sw.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Actualizar caso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ignorar_cambio = false;
                    return false;
                }
                b_salva_caso.ForeColor = Color.Black;
                ignorar_cambio = false;
                return true;
            }
            return false;
        }
        private void PoblacionCambiado()
        {
            if (ignorar_cambio) return;
            SinPoblacion();
            CasoCambiado();
        }
        private void PoblacionCambiado(object sender, EventArgs e)
        {
            PoblacionCambiado();
        }
        private void CasoCambiado()
        {
            if (ignorar_cambio) return;
            b_salva_caso.ForeColor = Color.Red;
        }
        private void CasoCambiado(object sender, EventArgs e)
        {
            CasoCambiado();
        }
        private void ClusterCambiado()
        {
            if (ignorar_cambio) return;
            b_crea_clusters.ForeColor = Color.Red;
        }
        private void ClusterCambiado(object sender, EventArgs e)
        {
            ClusterCambiado();
            CasoCambiado();
        }
        private bool Parametros()
        {
            try
            {
                U_SALIDAS = selunidad.SelectedItem.ToString();
                if (string.IsNullOrEmpty(U_SALIDAS))
                {
                    U_SALIDAS = "C";
                }
                selunidad.SelectedItem = U_SALIDAS;
                PREFIJO = prefijo.Text;
                F_GRUPOS = senda_grupos.Text;
                INDIVIDUOS = Convert.ToInt64(d_individuos.Text);
                RADIO = Convert.ToInt32(d_radio.Text);
                POTENCIA = Convert.ToDouble(d_potencia.Text.Replace('.', ','));
                PROB_CONTAGIO = Convert.ToDouble(d_contagio.Text.Replace('.', ',')) / 100.0;
                PROB_RECONTAGIO = Convert.ToDouble(d_recontagio.Text.Replace('.', ',')) / 100.0;
                MIN_INMUNIDAD = Convert.ToInt32(d_mininmunidad.Text);
                LATENCIA = Convert.ToInt32(d_latencia.Text);
                N_FOCOS_INICIALES = Convert.ToInt32(d_focos.Text);
                N_INMUNES_INICIALES = Convert.ToInt32(d_inmunes.Text);
                N_FOCOS_IMPORTADOS = Convert.ToInt32(d_importados.Text);
                DIAS_FOCOS_IMPORTADOS = Convert.ToInt32(d_dias_importados.Text);
                GRUPO_IMPORTADO = -1;
                if (grupos.Count > 0 && N_FOCOS_IMPORTADOS > 0)
                {
                    string sg = d_grupo_importado.Text;
                    int n = 0;
                    foreach (Grupo g in grupos)
                    {
                        if (sg.Equals(g.grupo, StringComparison.OrdinalIgnoreCase))
                        {
                            GRUPO_IMPORTADO = n;
                            break;
                        }
                        n++;
                    }
                    if (GRUPO_IMPORTADO == -1)
                    {
                        MessageBox.Show("No se reconoce el grupo de los focos a importar", "Datos de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                CONTACTO = Convert.ToInt32(d_contacto.Text);
                CONTACTO2 = CONTACTO * CONTACTO;
                LON_10PASOS = Convert.ToInt32(d_lon_paso.Text);

                // Multiplicamos por 2.0 porque al generar los desplazamientos entre 0 y 1 en incremebto medio es de 0.5

                LON_PASO = LON_10PASOS / 10.0;
                LON_PASO2 = 2.0 * LON_PASO;
                DIAS = Convert.ToInt32(d_dias.Text);
                PROB_CONTAGIO_C = Convert.ToDouble(d_contagio_c.Text.Replace('.', ',')) / 100.0;

                // El cálculo de contagios en cluster se hace parqa i-j y j-i, es decir el mismo par se trata dos veces

                PROB_CONTAGIO_Cd2 = PROB_CONTAGIO_C / 2.0;
                NUMERO_CLUSTERS_DATO = Convert.ToInt32(d_numero_c.Text);
                FACTOR_INDIVIDUOS_CLUSTER = Convert.ToDouble(d_individuos_c.Text.Replace('.', ','));
                string s = string.Format(@"{0}:\{1}", U_SALIDAS, CARPETA_SALIDAS);
                if (!Directory.Exists(s)) Directory.CreateDirectory(s);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Datos de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        #endregion

        #region Grupos
        private void IniciaTablaGrupos(DataGridView tabla)
        {
            tabla.BackgroundColor = Color.LightGray;
            tabla.BorderStyle = BorderStyle.Fixed3D;
            tabla.ReadOnly = false;
            tabla.MultiSelect = false;
            tabla.AllowUserToAddRows = true;
            tabla.AllowUserToDeleteRows = true;
            tabla.AllowUserToOrderColumns = false;
            tabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            tabla.AllowUserToResizeRows = false;
            tabla.AllowUserToResizeColumns = false;
            tabla.ColumnHeadersVisible = true;
            tabla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            tabla.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            tabla.DefaultCellStyle.SelectionBackColor = Color.Red;
            tabla.RowHeadersVisible = false;
            tabla.RowTemplate.Height = tabla_alto_fila;
            tabla.ColumnHeadersHeight = tabla_alto_cabecera;
            tabla.ColumnCount = 12;
            int j = 0;
            tabla.Columns[j].FillWeight = 30;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            tabla.Columns[j].Name = "Nombre";
            tabla.Columns[j].SortMode = DataGridViewColumnSortMode.Automatic;
            j++;
            tabla.Columns[j].FillWeight = 18;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].Name = "Genero";
            j++;
            tabla.Columns[j].FillWeight = 18;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].DefaultCellStyle.BackColor = Color.Beige;
            tabla.Columns[j].Name = "Edad-Min";
            j++;
            tabla.Columns[j].FillWeight = 18;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].DefaultCellStyle.BackColor = Color.Beige;
            tabla.Columns[j].Name = "Edad-Max";
            j++;
            tabla.Columns[j].FillWeight = 20;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].Name = "Contagio";
            j++;
            tabla.Columns[j].FillWeight = 20;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].Name = "Duración";
            j++;
            tabla.Columns[j].FillWeight = 20;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].Name = "ProbCura";
            j++;
            tabla.Columns[j].FillWeight = 20;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].DefaultCellStyle.BackColor = Color.Beige;
            tabla.Columns[j].Name = "PasosMin";
            j++;
            tabla.Columns[j].FillWeight = 20;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].DefaultCellStyle.BackColor = Color.Beige;
            tabla.Columns[j].Name = "PasosMax";
            j++;
            tabla.Columns[j].FillWeight = 18;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].Name = "% Pobl";
            j++;
            tabla.Columns[j].FillWeight = 18;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].DefaultCellStyle.BackColor = Color.Beige;
            tabla.Columns[j].Name = "% Clus";
            j++;
            tabla.Columns[j].FillWeight = 20;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].DefaultCellStyle.BackColor = Color.Beige;
            tabla.Columns[j].Name = "Ind/Cluster";
        }
        private void Sel_grupos_Click(object sender, EventArgs e)
        {
            OpenFileDialog leefichero = new OpenFileDialog
            {
                Filter = "GGG (*.ggg)|*.ggg|All files (*.*)|*.*",
                CheckFileExists = false,
                Multiselect = false
            };
            if (leefichero.ShowDialog() == DialogResult.OK)
            {
                string fo = leefichero.FileName;
                if (!File.Exists(fo))
                {
                    // Nuevos fichero de grupos

                    SinPoblacion();
                    grupos.Clear();
                    MuestraGrupos();
                    F_GRUPOS = fo;
                    SalvaGrupos(F_GRUPOS);
                }
                if (LeeGrupos(fo))
                {
                    F_GRUPOS = fo;
                }
                else
                {
                    F_GRUPOS = string.Empty;
                    SinPoblacion();
                }
                senda_grupos.Text = F_GRUPOS;
            }
        }
        private bool LeeGrupos(string F_GRUPOS)
        {
            if (!File.Exists(F_GRUPOS))
            {
                MessageBox.Show("No se encuentra el fichero " + F_GRUPOS, "Lectura de grupos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            SinPoblacion();
            grupos.Clear();
            FileStream fr = new FileStream(F_GRUPOS, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fr);
            string s;
            string[] sd;
            string grupo;
            int genero;
            int edad_min;
            int edad_max;
            int pasos_min;
            int pasos_max;
            double prob_contagio;
            int duracion_infeccion;
            double prob_curacion;
            double fraccion_poblacion;
            double fraccion_clusters;
            int individuos_c;
            while (!sr.EndOfStream)
            {
                s = sr.ReadLine();
                sd = s.Split(';');
                if (sd.Length != 12)
                {
                    MessageBox.Show(string.Format("Número incorrecto de campos: {0} {1}", sd.Length, s), "Lectura de grupos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (sr != null) sr.Close();
                    return false;
                }
                try
                {
                    grupo = sd[0];
                    if (sd[1].Equals("m", StringComparison.OrdinalIgnoreCase))
                    {
                        genero = 0;
                    }
                    else if (sd[1].Equals("f", StringComparison.OrdinalIgnoreCase))
                    {
                        genero = 1;
                    }
                    else
                    {
                        genero = 2;
                    }
                    edad_min = Convert.ToInt32(sd[2]);
                    edad_max = Convert.ToInt32(sd[3]);
                    prob_contagio = Convert.ToDouble(sd[4]);
                    duracion_infeccion = Convert.ToInt32(sd[5]);
                    prob_curacion = Convert.ToDouble(sd[6]);
                    pasos_min = Convert.ToInt32(sd[7]);
                    pasos_max = Convert.ToInt32(sd[8]);
                    fraccion_poblacion = Convert.ToDouble(sd[9]);
                    fraccion_clusters = Convert.ToInt64(sd[10]);
                    individuos_c = Convert.ToInt32(sd[11]);
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("<{0}> <{1}> <{2}> <{3}> <{4}> <{5}> <{6}> <{7}> <{8}> <{9}> <{10}> <{11}> Mensaje: {12}", sd[0], sd[1], sd[2], sd[3], sd[4], sd[5], sd[6], sd[7], sd[8], sd[9], sd[10], sd[11], e.Message), "Lectura de grupos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (sr != null) sr.Close();
                    return false;
                }
                grupos.Add(new Grupo(grupos.Count, grupo, genero, edad_min, edad_max, prob_contagio, duracion_infeccion, prob_curacion, pasos_min, pasos_max, fraccion_poblacion, fraccion_clusters, individuos_c));
            }
            MuestraGrupos();
            Desactiva(true);
            if (CreaPoblacion(false))
            {
                CreaClusters(false);
            }
            Desactiva(false);
            sr.Close();
            return true;
        }
        private void MuestraGrupos()
        {
            tablaGrupos.RowCount = 1;
            object[] fila = new object[12];
            foreach (Grupo g in grupos)
            {
                fila[0] = g.grupo;
                if (g.genero == 0)
                {
                    fila[1] = "m";
                }
                if (g.genero == 1)
                {
                    fila[1] = "f";
                }
                else
                {
                    fila[1] = "-";
                }
                fila[2] = g.edad_min;
                fila[3] = g.edad_max;
                fila[4] = g.prob_contagio;
                fila[5] = g.duracion_infeccion;
                fila[6] = g.prob_curacion;
                fila[7] = g.pasos_min;
                fila[8] = g.pasos_max;
                fila[9] = g.fraccion_poblacion;
                fila[10] = g.fraccion_clusters;
                fila[11] = g.individuos_c;
                tablaGrupos.Rows.Add(fila);
            }
        }
        private bool ActualizaGrupos()
        {
            string[] sd = new string[tablaGrupos.ColumnCount];
            string grupo;
            int genero;
            int edad_min;
            int edad_max;
            int pasos_min;
            int pasos_max;
            double prob_contagio;
            int duracion_infeccion;
            double prob_curacion;
            double fraccion_poblacion;
            double fraccion_clusters;
            int individuos_c;
            grupos.Clear();
            DataGridViewRow fila;
            for (int i = 0; i < tablaGrupos.RowCount; i++)
            {
                fila = tablaGrupos.Rows[i];

                // Evitar la última fila que está vacía

                if (fila.Cells[0].Value == null || String.IsNullOrEmpty(fila.Cells[0].Value.ToString())) continue;
                for (int j = 0; j < tablaGrupos.ColumnCount; j++)
                {
                    sd[j] = fila.Cells[j].Value.ToString();
                }
                try
                {
                    grupo = sd[0];
                    if (sd[1].Equals("m", StringComparison.OrdinalIgnoreCase))
                    {
                        genero = 0;
                    }
                    else if (sd[1].Equals("f", StringComparison.OrdinalIgnoreCase))
                    {
                        genero = 1;
                    }
                    else
                    {
                        genero = 2;
                    }
                    edad_min = Convert.ToInt32(sd[2]);
                    edad_max = Convert.ToInt32(sd[3]);
                    prob_contagio = Convert.ToDouble(sd[4]);
                    duracion_infeccion = Convert.ToInt32(sd[5]);
                    prob_curacion = Convert.ToDouble(sd[6]);
                    pasos_min = Convert.ToInt32(sd[7]);
                    pasos_max = Convert.ToInt32(sd[8]);
                    fraccion_poblacion = Convert.ToDouble(sd[9]);
                    fraccion_clusters = Convert.ToDouble(sd[10]);
                    individuos_c = Convert.ToInt32(sd[11]);
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("<{0}> <{1}> <{2}> <{3}> <{4}> <{5}> <{6}> <{7}> <{8}> <{9}> <{10}> <{11}> Mensaje: {12}", sd[0], sd[1], sd[2], sd[3], sd[4], sd[5], sd[6], sd[7], sd[8], sd[9], sd[10], sd[11], e.Message), "Lectura de grupos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                grupos.Add(new Grupo(grupos.Count, grupo, genero, edad_min, edad_max, prob_contagio, duracion_infeccion, prob_curacion, pasos_min, pasos_max, fraccion_poblacion, fraccion_clusters, individuos_c));
            }
            b_salva_grupos.ForeColor = Color.Black;
            return true;
        }
        private void B_salva_grupos_Click(object sender, EventArgs e)
        {
            string fichero = FicheroEscritura(F_GRUPOS, "GGG (*.ggg)|*.ggg|All files (*.*)|*.*");
            if (!string.IsNullOrEmpty(fichero))
            {
                if (SalvaGrupos(fichero))
                {
                    if (!fichero.Equals(F_GRUPOS, StringComparison.OrdinalIgnoreCase))
                    {
                        senda_grupos.Text = F_GRUPOS = fichero;
                        CasoCambiado();
                    }
                    MessageBox.Show("OK", "Actualizar grupos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private bool SalvaGrupos(string F_GRUPOS)
        {
            ActualizaGrupos();
            try
            {
                FileStream fw = new FileStream(F_GRUPOS, FileMode.Create, FileAccess.Write, FileShare.Read);
                StreamWriter sw = new StreamWriter(fw);
                for (int i = 0; i < tablaGrupos.RowCount; i++)
                {
                    DataGridViewRow fila = tablaGrupos.Rows[i];
                    if (fila.Cells[0].Value == null || String.IsNullOrEmpty(fila.Cells[0].Value.ToString())) continue;
                    if (fila.Cells[1].Value == null) fila.Cells[1].Value = "f";
                    if (fila.Cells[2].Value == null) fila.Cells[2].Value = "0";
                    if (fila.Cells[3].Value == null) fila.Cells[3].Value = "0";
                    if (fila.Cells[4].Value == null) fila.Cells[4].Value = "0";
                    if (fila.Cells[5].Value == null) fila.Cells[5].Value = "a";
                    if (fila.Cells[6].Value == null) fila.Cells[6].Value = "0";
                    if (fila.Cells[7].Value == null) fila.Cells[7].Value = "0";
                    if (fila.Cells[8].Value == null) fila.Cells[8].Value = "0";
                    if (fila.Cells[9].Value == null) fila.Cells[9].Value = "0";
                    if (fila.Cells[10].Value == null) fila.Cells[10].Value = "0";
                    if (fila.Cells[11].Value == null) fila.Cells[11].Value = "0";
                    sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11}", fila.Cells[0].Value.ToString(), fila.Cells[1].Value.ToString(), fila.Cells[2].Value.ToString(), fila.Cells[3].Value.ToString(), fila.Cells[4].Value.ToString(), fila.Cells[5].Value.ToString(), fila.Cells[6].Value.ToString(), fila.Cells[7].Value.ToString(), fila.Cells[8].Value.ToString(), fila.Cells[9].Value.ToString(), fila.Cells[10].Value.ToString(), fila.Cells[11].Value.ToString()));
                }
                sw.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("{0}", e.Message), "Actualizar grupos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void SinPoblacion()
        {
            poblacion_creada = false;
            b_crea_poblacion.ForeColor = Color.Red;
            TABLA_VECINOS = false;
            tabla_vecinos_en_uso.Text = string.Empty;
            tabla_vecinos_en_uso.BackColor = Color.Red;
        }
        private void Tabla_vecinos_en_uso_DoubleClick(object sender, EventArgs e)
        {
            if (!poblacion_creada || !tabla_vecinos_creada) return;
            TABLA_VECINOS = !TABLA_VECINOS;
            tabla_vecinos_en_uso.Text = TABLA_VECINOS ? "Tabla vecinos" : string.Empty;
            tabla_vecinos_en_uso.BackColor = TABLA_VECINOS ? SystemColors.Control : Color.Red;
        }
        private void TablaGrupos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SinPoblacion();
            b_salva_grupos.ForeColor = Color.Red;
        }
        #endregion

        #region Simulación
        private void B_crea_poblacion_Click(object sender, EventArgs e)
        {
            if (!Parametros()) return;
            if (!ActualizaGrupos()) return;
            SinPoblacion();
            Desactiva(true);
            if (CreaPoblacion(true))
            {
                CreaClusters(true);
            }
            Desactiva(false);
        }
        private bool CreaPoblacion(bool mensaje)
        {
            if (!Parametros()) return false;
            if (grupos.Count == 0)
            {
                MessageBox.Show("No hay grupos de población", "Crear población", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            linea_estado.Text = "Creando población ...";
            Application.DoEvents();
            double[] fraccion_antes = new double[grupos.Count];
            double[] fraccion_ahora = new double[grupos.Count];
            long[] indi_ahora = new long[grupos.Count];

            // Normalizar porcentajes ('fraccion_poblacion'))

            int n = 0;
            double suma = 0.0;
            foreach (Grupo g in grupos)
            {
                fraccion_antes[n++] = g.fraccion_poblacion;
                suma += g.fraccion_poblacion;
            }
            suma = Math.Round(suma / 100.0, 4);
            n = 0;
            foreach (Grupo g in grupos)
            {
                if (suma != 1.0000) g.fraccion_poblacion = Math.Round(g.fraccion_poblacion / suma, 3);
                fraccion_ahora[n] = g.fraccion_poblacion;
                indi_ahora[n] = (long)(INDIVIDUOS * g.fraccion_poblacion / 100.0);
                n++;
            }

            // Por los posibles cambios en los porcentajes y el número de individuos

            MuestraGrupos();
            individuos.Clear();
            double xt;
            double yt;
            Random azarindi = new Random(12345);
            n = 0;
            foreach (Grupo g in grupos)
            {
                for (long i = 0; i < indi_ahora[n]; i++)
                {
                    while (true)
                    {
                        xt = 2.0 * azarindi.NextDouble() - 1.0;
                        yt = 2.0 * azarindi.NextDouble() - 1.0;
                        if ((xt * xt + yt * yt) <= 1.0) break;
                    }
                    xt = RADIO * xt;
                    yt = RADIO * yt;
                    individuos.Add(new Individuo(individuos.Count, g.ID, g.grupo, xt, yt, 0));
                }
                n++;
            }
            if (individuos.Count == 0)
            {
                return false;
            }
            sanos = 0;
            desinmunizados = 0;
            infectados = 0;
            importados = 0;
            curados = 0;
            foreach (Individuo indi_i in individuos)
            {
                switch (indi_i.estado)
                {
                    case 0:
                        sanos++;
                        break;
                    case 1:
                        infectados++;
                        break;
                    case 2:
                        curados++;
                        break;
                    case 3:
                        desinmunizados++;
                        break;
                    default:
                        muertos++;
                        break;
                }
            }
            ActualizaMonitorEstados();
            Individuo indi;

            // Focos inciciales

            azar = new Random(17292);
            if (sanos <= N_FOCOS_INICIALES)
            {
                MessageBox.Show("Hay más focos de infecciónque que individuos sanos. Cancelado", "Crear población", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            for (int nf = 0; nf < N_FOCOS_INICIALES; nf++)
            {
                // Buscar al azar uno sano para infectarlo y marcarlo como recien infectado (indi.dias_infectado = 0)

                int i;
                Individuo paciente_cero;
                while (true)
                {
                    i = (int)(azar.NextDouble() * individuos.Count);
                    if (individuos.ElementAt(i).estado == 0)
                    {
                        paciente_cero = individuos.ElementAt(i);
                        paciente_cero.estado = 1;
                        /*
                         * Se han creado con estos valores a cero
                         * 
                         * paciente_cero.dias_infectado = 0;
                         * paciente_cero.dias_curado = 0;
                         * paciente_cero.enfermados = 0;
                         */
                        infectados++;
                        sanos--;
                        break;
                    }
                }
            }

            // Inmunizados iniciales

            if (sanos <= N_INMUNES_INICIALES)
            {
                MessageBox.Show("Hay más inmunizados que individuos sanos. Cancelado", "Crear población", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            for (int nf = 0; nf < N_INMUNES_INICIALES; nf++)
            {
                // Buscar al azar uno sano para inmunizarlo (curarlo)

                int i;
                while (true)
                {
                    i = (int)(azar.NextDouble() * individuos.Count);
                    if (individuos.ElementAt(i).estado == 0)
                    {
                        indi = individuos.ElementAt(i);
                        indi.estado = 2;
                        /*
                         * Se han creado con estos valores a cero
                         * 
                         * paciente_cero.dias_infectado = 0;
                         * paciente_cero.dias_curado = 0;
                         * paciente_cero.enfermados = 0;
                         */
                        curados++;
                        sanos--;
                        break;
                    }
                }
            }
            ActualizaMonitorEstados();
            if (INDIVIDUOS != individuos.Count)
            {
                INDIVIDUOS = individuos.Count;
                d_individuos.Text = INDIVIDUOS.ToString();
                CasoCambiado();
            }
            poblacion_creada = true;
            b_crea_poblacion.ForeColor = Color.Black;
            n = 0;
            foreach (Grupo g in grupos)
            {
                if (fraccion_ahora[n] != fraccion_antes[n])
                {
                    b_salva_grupos.ForeColor = Color.Red;
                    break;
                }
                n++;
            }
            if (individuos.Count < MAX_INDIVIDUOS_PARA_TABLA)
            {
                if (!TablaVecinos(mensaje))
                {
                    tabla_vecinos_creada = false;
                    TABLA_VECINOS = false;
                    tabla_vecinos_en_uso.Text = string.Empty;
                    tabla_vecinos_en_uso.BackColor = Color.Red;
                }
            }
            else
            {
                tabla_vecinos_creada = false;
                TABLA_VECINOS = false;
                tabla_vecinos_en_uso.Text = string.Empty;
                tabla_vecinos_en_uso.BackColor = Color.Red;
            }
            if (mensaje)
            {
                if (individuos.Count == 0)
                {
                    MessageBox.Show("No se ha creado ningún individuo.", "Crear población", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(string.Format("Creados {0:N0} individuos.", individuos.Count), "Crear población", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            linea_estado.Text = string.Empty;
            return true;
        }
        private void B_crea_clusters_Click(object sender, EventArgs e)
        {
            Desactiva(true);
            CreaClusters(true);
            Desactiva(false);
        }
        private bool CreaClusters(bool mensaje)
        {
            if (!Parametros()) return false;
            if (grupos.Count == 0)
            {
                MessageBox.Show("No hay grupos de población", "Crear clusters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            linea_estado.Text = "Creando clusters ...";
            Application.DoEvents();
            double[] fraccion_antes = new double[grupos.Count];
            double[] fraccion_ahora = new double[grupos.Count];
            clusters_grupo = new int[grupos.Count];

            // Normalizar porcentajes ('fraccion_clusters'))

            int n = 0;
            double suma = 0.0;
            foreach (Grupo g in grupos)
            {
                fraccion_antes[n++] = g.fraccion_clusters;
                suma += g.fraccion_clusters;
            }
            suma = Math.Round(suma / 100.0, 4);

            // Cluster por grupo

            int suma_c = 0;
            n = 0;
            foreach (Grupo g in grupos)
            {
                if (suma != 1.0000) g.fraccion_clusters = Math.Round(g.fraccion_clusters / suma, 3);
                fraccion_ahora[n] = g.fraccion_clusters;
                clusters_grupo[n] = (int)(NUMERO_CLUSTERS_DATO * g.fraccion_clusters / 100.0);
                suma_c += clusters_grupo[n];
                n++;
            }

            // Individuos por cluster

            Individuo indi;

            for (int nindi = 0; nindi < individuos.Count; nindi++)
            {
                indi = individuos.ElementAt(nindi);
                indi.cluster = -1;
                indi.contagio_en_cluster = false;
            }
            int indi_g;
            int indi_c;
            int desde_indi;
            n = 0;
            int k = -1;
            long total_individuos = 0;
            foreach (Grupo g in grupos)
            {
                k++;
                if (clusters_grupo[k] == 0) continue;
                linea_estado.Text = string.Format("Creando cluster {0} de {1} ", k + 1, suma);
                Application.DoEvents();
                indi_g = (int)(g.fraccion_poblacion / 100.0 * INDIVIDUOS);
                indi_c = (int)(g.individuos_c * FACTOR_INDIVIDUOS_CLUSTER);
                desde_indi = 0;
                for (int i = 0; i < clusters_grupo[k]; i++)
                {
                    if (indi_g == 0)
                    {
                        // No quedan individos de este grupo

                        clusters_grupo[k] = i;
                        break;
                    }
                    if (indi_g < indi_c)
                    {
                        // No hay suficientes individuos de este grupo

                        indi_c = indi_g;
                    }

                    // Se asignan individuos consecutivos de la lista de población
                    // lo cual no debería ser ningún problema

                    grupo_cluster[n] = g.ID;
                    n_individuos_cluster[n] = 0;
                    for (int nindi = desde_indi; nindi < individuos.Count; nindi++)
                    {
                        indi = individuos.ElementAt(nindi);
                        if (indi.grupo_ID == g.ID)
                        {
                            if (n_individuos_cluster[n] >= MAX_INDIVIDUOS_CLUSTER)
                            {
                                MessageBox.Show(string.Format("Superado el número máximo de indiciduos por cluster {0}", MAX_INDIVIDUOS_CLUSTER), "Crear clusters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            individuos_cluster_id[n, n_individuos_cluster[n]] = indi.ID;
                            indi.cluster = n;
                            n_individuos_cluster[n]++;
                            total_individuos++;
                            if (n_individuos_cluster[n] == indi_c)
                            {
                                desde_indi = nindi + 1;
                                break;
                            }
                        }
                    }
                    n++;
                    indi_g -= indi_c;
                }
            }
            NUMERO_CLUSTERS = n;
            if (mensaje)
            {
                if (NUMERO_CLUSTERS == 0)
                {
                    MessageBox.Show("No se ha creado ningún cluster.", "Crear clusters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(string.Format("Creados {0:N0} clusters.{1}Con {2:N0} individuos", NUMERO_CLUSTERS, Environment.NewLine, total_individuos), "Crear clusters", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            linea_estado.Text = string.Empty;
            b_crea_clusters.ForeColor = Color.Black;
            return true;
        }
        private bool TablaVecinos(bool mensaje)
        {
            cancelar = false;
            b_cancelar.Enabled = true;
            Application.DoEvents();
            Individuo indi_i;
            Individuo indi_j;
            Grupo g;
            double x;
            double y;
            double d;
            double d_max_i;
            double d_max;
            double d_med = 0;
            double d_max_med = 0;
            int nv;
            int nv_max = 0;
            int n = 0;
            linea_estado.Text = "Creando tabla de vecinos";
            Application.DoEvents();
            for (int i = 0; i < individuos.Count; i++)
            {
                if (i % 10 == 0)
                {
                    linea_estado.Text = string.Format("Creando tabla de vecinos {0} de {1} ", i + 1, individuos.Count);
                    Application.DoEvents();
                }
                if (cancelar)
                {
                    linea_estado.Text = string.Empty;
                    cancelar = false;
                    b_cancelar.Enabled = false;
                    tabla_vecinos_creada = false;
                    TABLA_VECINOS = false;
                    tabla_vecinos_en_uso.Text = string.Empty;
                    tabla_vecinos_en_uso.BackColor = Color.Red;
                    MessageBox.Show("Sin tabla de vecinos los cálculos son muy lentos. Para crearla, vuelve a Crear la Población", "Tabla de vecinos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    linea_estado.Text = string.Empty;
                    return false;
                }
                indi_i = individuos.ElementAt(i);
                g = grupos.Find(v => v.grupo == indi_i.grupo);
                d_max_i = g.pasos_max * LON_PASO;
                nv = 0;
                for (int j = 0; j < individuos.Count; j++)
                {
                    if (i == j) continue;
                    indi_j = individuos.ElementAt(j);
                    g = grupos.Find(v => v.grupo == indi_j.grupo);
                    d_max = d_max_i + g.pasos_max * LON_PASO;
                    x = indi_i.xi - indi_j.xi;
                    y = indi_i.yi - indi_j.yi;
                    d = Math.Sqrt(x * x + y * y);
                    if (d < d_max)
                    {
                        if (nv == MAX_VECINOS_PARA_TABLA - 1)
                        {
                            MessageBox.Show("Superada la capacidad de la tabla", "Tabla de vecinos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            linea_estado.Text = string.Empty;
                            cancelar = false;
                            b_cancelar.Enabled = false;
                            tabla_vecinos_creada = false;
                            TABLA_VECINOS = false;
                            tabla_vecinos_en_uso.Text = string.Empty;
                            tabla_vecinos_en_uso.BackColor = Color.Red;
                            linea_estado.Text = string.Empty;
                            return false;
                        }
                        vecinas[i, nv] = j;
                        nv++;
                        n++;
                    }
                    d_med += d;
                    d_max_med += d_max;
                }
                nvecinas[i] = nv;
                if (nv > nv_max) nv_max = nv;
            }
            tabla_vecinos_creada = true;
            d = individuos.Count * individuos.Count;
            double factor = n / d;
            if (mensaje)
            {
                MessageBox.Show(string.Format(@"Distancia media: {0:N0}{1}Distancia máxima media: {2:N0}{1}Factor vecinas: {3:f3}{4}Máx vecinas: {5}", d_med / d, Environment.NewLine, d_max_med / d, factor, Environment.NewLine, nv_max), "Tabla de vecinos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (factor < 0.12)
            {
                TABLA_VECINOS = true;
                tabla_vecinos_en_uso.Text = "Tabla vecinos";
                tabla_vecinos_en_uso.BackColor = SystemColors.Control;
            }
            else
            {
                TABLA_VECINOS = false;
                tabla_vecinos_en_uso.Text = string.Empty;
                tabla_vecinos_en_uso.BackColor = Color.Red;
            }
            linea_estado.Text = string.Empty;
            cancelar = false;
            b_cancelar.Enabled = false;
            return true;
        }
        private bool ProbabilidadFinInfeccion()
        {
            if (grupos.Count == 0) return false;
            /*
             * y = a * x ^ p entre 0 y n
             * a = 1 / (n ^ p)
             */
            int n;
            double a;
            double suma;
            PROB_FIN_INFECCION = new double[grupos.Count, MAX_CONVALECENCIA];
            foreach (Grupo g in grupos)
            {
                n = g.duracion_infeccion;
                if (n >= MAX_CONVALECENCIA)
                {
                    MessageBox.Show(string.Format("El máximo número de días de convalecencia es {0}", MAX_CONVALECENCIA - 1), string.Format("Simular [{0}]", PREFIJO), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                a = 1 / Math.Pow(n, POTENCIA);
                suma = 0;
                for (int i = 0; i <= n; i++)
                {
                    PROB_FIN_INFECCION[g.ID, i] = a * Math.Pow(i, POTENCIA);
                    suma += PROB_FIN_INFECCION[g.ID, i];
                }
                for (int i = 0; i <= n; i++)
                {
                    PROB_FIN_INFECCION[g.ID, i] /= suma;
                }
            }
            return true;
        }
        private void B_simula_Click(object sender, EventArgs e)
        {
            if (grupos.Count == 0)
            {
                MessageBox.Show("No hay grupos de población", string.Format("Simular [{0}]", PREFIJO), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!poblacion_creada)
            {
                MessageBox.Show("Aún no se ha creado la población", string.Format("Simular [{0}]", PREFIJO), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!ActualizaGrupos()) return;
            if (!ProbabilidadFinInfeccion()) return;
            if (!Parametros()) return;
            try
            {
                string[] sdex;
                if (d_dias_exportar.Text.Trim().IndexOf(',') != -1)
                {
                    sdex = d_dias_exportar.Text.Trim().Split(',');
                }
                else
                {
                    sdex = d_dias_exportar.Text.Trim().Split(' ');
                }
                prox_dia_exportar = 0;
                num_dias_exportar = sdex.Length;
                if (num_dias_exportar > 0)
                {
                    int ne = 0;
                    dias_exportar = new int[sdex.Length];
                    for (int i = 0; i < num_dias_exportar; i++)
                    {
                        if (sdex[i].Trim().Length == 0) continue;
                        dias_exportar[ne] = Convert.ToInt32(sdex[i]) - 1;
                        if (dias_exportar[i] < 0) dias_exportar[i] = 0;
                        ne++;
                    }
                    num_dias_exportar = ne;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, string.Format("Simular [{0}]", PREFIJO), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (lzo_ex != null)
            {
                lzo_ex.Dispose();
                lzo_ex = null;
            }
            lzo_ex = new Form2();
            azar = new Random(4816);
            sanos = 0;
            desinmunizados = 0;
            infectados = 0;
            importados = 0;
            curados = 0;
            muertos = 0;
            ActualizaMonitorEstados();
            foreach (Individuo indi in individuos)
            {
                // Para el primer gráfico

                indi.pasos_dia = 0;
                indi.pasos = null;
                indi.distancia = 0.0;
                indi.sentido = 0;
                indi.pasos_dados = 0;
                switch (indi.estado)
                {
                    case 0:
                        sanos++;
                        break;
                    case 1:
                        infectados++;
                        break;
                    case 2:
                        curados++;
                        break;
                    case 3:
                        desinmunizados++;
                        break;
                    default:
                        muertos++;
                        break;
                }
            }
            ActualizaMonitorEstados();
            total_pasos = 0;
            total_recorridos = 0;
            acumulador_infectados = 0;
            total_distancias_ida = 0.0;
            total_contactos_de_riesgo = 0;
            total_dias_infectados_curados = 0;
            total_dias_infectados_muertos = 0;
            v_dia.Text = string.Empty;
            v_paso.Text = string.Empty;
            v_max_pasos.Text = string.Empty;
            v_recorridos.Text = string.Empty;
            v_contactos.Text = string.Empty;
            v_infectado.Text = string.Empty;
            v_r0.Text = string.Empty;
            n_sorteos_fin_infeccion = new long[grupos.Count];
            n_fin_infeccion = new long[grupos.Count];
            res_fin_infeccion = new long[grupos.Count, 2];
            for (int i = 0; i < grupos.Count; i++)
            {
                n_sorteos_fin_infeccion[i] = 0;
                n_fin_infeccion[i] = 0; ;
                res_fin_infeccion[i, 0] = 0;
                res_fin_infeccion[i, 1] = 0;
            }
            infectados_total_clusters = 0;
            curados_clusters_actuales = 0;
            muertos_clusters_actuales = 0;
            for (int i = 0; i < NUMERO_CLUSTERS; i++)
            {
                intentos_cluster[i] = 0;
                infectados_cluster[i] = 0;
            }
            lzo_ex.principal = this;
            lzo_ex.radio = RADIO;
            lzo_ex.ActualizaDia(0);
            lzo_ex.ActualizaTitulo();
            lzo_ex.Show(this);
            DialogResult respuesta = MessageBox.Show("¿ Preguntar a cada día ?", "Simulación", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (respuesta == DialogResult.Yes || respuesta == DialogResult.No)
            {
                Desactiva(true);
                CicloDias(respuesta == DialogResult.Yes);
                linea_estado.Text = string.Empty;
                Desactiva(false);
            }
        }
        private void CicloDias(bool paradas)
        {
            string carpeta = string.Format(@"{0}:\{1}\{2}", U_SALIDAS, CARPETA_SALIDAS, PREFIJO);
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
            string fichero = string.Format(@"{0}\s.log", carpeta);
            FileStream fw = new FileStream(fichero, FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(fw);
            sw.WriteLine("--------------------------------------------------------------------------------------------------");
            sw.WriteLine(string.Format("Simulador de contagios. {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            sw.WriteLine(string.Format("{0:D2}/{1:D2}/{2:D4} {3:D2}:{4:D2}:{5:D2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
            sw.WriteLine();
            sw.WriteLine(string.Format("Prefijo {0}", PREFIJO));
            sw.WriteLine(string.Format("Potencia para fin de infección  %  {0,12:f2}", POTENCIA));
            sw.WriteLine(string.Format("Probabilidad base de contagio   %  {0,12:f2}", 100.0 * PROB_CONTAGIO));
            sw.WriteLine(string.Format("Probabilidad contagio cluster   %  {0,12:f2}", 100.0 * PROB_CONTAGIO_C));
            sw.WriteLine(string.Format("Probabilidad base de REcontagio %  {0,12:f2}", 100.0 * PROB_RECONTAGIO));
            sw.WriteLine(string.Format("Días mínimos de inmunidad          {0,12:N0}", MIN_INMUNIDAD));
            sw.WriteLine(string.Format("Días de latencia para contagiar    {0,12:N0}", LATENCIA));
            sw.WriteLine(string.Format("Número de individuos               {0,12:N0}", INDIVIDUOS));
            sw.WriteLine(string.Format("Radio                              {0,12:N0}", RADIO));
            sw.WriteLine(string.Format("Número de focos iniciales          {0,12:N0}", N_FOCOS_INICIALES));
            sw.WriteLine(string.Format("Número de inmunes iniciales        {0,12:N0}", N_INMUNES_INICIALES));
            if (N_FOCOS_IMPORTADOS > 0 && GRUPO_IMPORTADO == -1)
            {
                sw.WriteLine(string.Format("Número de focos a importar         {0,12:N0} cada {1} días, del grupo {2} NO ENCONTRADO. Opción deshabilitada.", N_FOCOS_IMPORTADOS, DIAS_FOCOS_IMPORTADOS, d_grupo_importado.Text));
            }
            else
            {
                sw.WriteLine(string.Format("Número de focos a importar         {0,12:N0} cada {1} días, del grupo {2}", N_FOCOS_IMPORTADOS, DIAS_FOCOS_IMPORTADOS, d_grupo_importado.Text));
            }
            sw.WriteLine(string.Format("Radio de contacto                  {0,12:N0}", CONTACTO));
            sw.WriteLine(string.Format("Longitud media 10 pasos            {0,12:N0}", LON_10PASOS));
            sw.WriteLine(string.Format("Tabla de vecinos                   {0,-12}", TABLA_VECINOS ? "Si" : "No"));
            Distancias(0);
            sw.WriteLine();
            sw.WriteLine(string.Format("Número clusters objetivo           {0,12:N0}", NUMERO_CLUSTERS_DATO));
            sw.WriteLine(string.Format("Número clusters generados          {0,12:N0}", NUMERO_CLUSTERS));
            sw.WriteLine(string.Format("Factor individuos por cluster      {0,12:f2}", FACTOR_INDIVIDUOS_CLUSTER));
            sw.WriteLine();
            //ClustersInicio(sw);
            sw.WriteLine("                        - Edad- % Factor B    Días         %      Num.Pasos      %             %      Individuos");
            sw.WriteLine("Grupo            Género Min Max   Contagio  Duración    Curación   Min   Max Individuos     clusters    cluster ");
            sw.WriteLine("---------------- ------ --- --- ---------- ---------- ---------- ----- ----- ---------- ------------ ------------");
            foreach (Grupo gt in grupos)
            {
                sw.WriteLine(string.Format("{0,-16} {1,6} {2,3:N0} {3,3:N0} {4,10:f1} {5,10:f1} {6,10:f1} {7,5:N0} {8,5:N0} {9,10:f2} {10,12:f1} {11,12:N0}", gt.grupo, gt.genero, gt.edad_min, gt.edad_max, gt.prob_contagio, gt.duracion_infeccion, gt.prob_curacion, gt.pasos_min, gt.pasos_max, gt.fraccion_poblacion, gt.fraccion_clusters, gt.individuos_c));
            }
            sw.WriteLine("---------------- ------ --- --- ---------- ---------- ---------- ----- ----- ---------- ------------ ------------");
            sw.WriteLine();
            sw.WriteLine("Valores al final del día");
            sw.WriteLine("                                                                       Recorrido  Contactos Media días            -- Producidos en los Clusters --");
            sw.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos     Ida     críticos  infectados         R0  Contagios Curaciones    Muertes");
            sw.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            sw.Flush();
            BotonCancelar(true);
            if (lzo_ex != null) lzo_ex.BotonCancelar(true);
            Application.DoEvents();
            double contactos_de_riesgo;
            double media_dias_infectado;
            double recorrido_individuo_activo;
            double R0;
            max_r0 = 0.0;
            int resultado_dia;
            double xt;
            double yt;
            double pre;
            DateTime tiempo_inicio = DateTime.Now;
            TimeSpan dt = (DateTime.Now - tiempo_inicio);
            string tiempo;
            int dias_sim = 0;
            Grupo g;
            int id_grupo_importar = -1;
            string nombre_grupo_importar = string.Empty;
            if (N_FOCOS_IMPORTADOS > 0 && GRUPO_IMPORTADO != -1)
            {
                g = grupos.ElementAt(GRUPO_IMPORTADO);
                id_grupo_importar = g.ID;
                nombre_grupo_importar = g.grupo;
            }

            for (int i = 0; i < DIAS; i++)
            {
                // Empieza un nuevo día

                v_dia.Text = string.Format("{0}", i + 1);
                Application.DoEvents();
                if (sanos > 0 || desinmunizados > 0)
                {
                    // A pasear !

                    max_pasos_dia = 0;
                    foreach (Individuo indi in individuos)
                    {
                        if (indi.estado == 2 || indi.estado == -1)
                        {
                            // Los curados y los muertos no pasean

                            continue;
                        }
                        g = grupos.Find(x => x.grupo == indi.grupo);

                        // Para cada individuo, un nuevo recorrido al azar

                        // Desde el punto inicial

                        indi.x = indi.xi;
                        indi.y = indi.yi;

                        // Número de pasos del nuevo recorrido

                        indi.pasos_dia = g.pasos_min + (int)((g.pasos_max - g.pasos_min) * azar.NextDouble());
                        indi.pasos = new double[indi.pasos_dia, 2];
                        if (indi.pasos_dia > max_pasos_dia) max_pasos_dia = indi.pasos_dia;
                        indi.distancia = 0.0;

                        // Empezamos yendo

                        indi.sentido = 0;

                        // Con el contador de pasos a cero

                        indi.pasos_dados = 0;
                    }
                    v_max_pasos.Text = string.Format("{0}", 2 * max_pasos_dia);
                    Application.DoEvents();
                    resultado_dia = TABLA_VECINOS ? ContagiosDiaVecinos() : ContagiosDia();
                    if (NUMERO_CLUSTERS > 0) ContagiosCluster();
                }
                else
                {
                    // Si no hay a quien infectar, para que caminar
                    // sólo hay que seguir la evolución de los contagiados.

                    v_max_pasos.Text = string.Empty;
                    Application.DoEvents();
                    if (cancelar || lzo_ex.cancelar)
                    {
                        resultado_dia = -1;
                    }
                    else
                    {
                        resultado_dia = 0;
                    }
                }
                if (resultado_dia == -1)
                {
                    dt = (DateTime.Now - tiempo_inicio);
                    tiempo = CierraCiclo(sw, dt, "Cancelado tras simular", dias_sim);
                    MessageBox.Show(tiempo, string.Format("Simular [{0}]", PREFIJO), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 'Curacion / Desinmunización / Muerte' al final del día

                long n_R0 = 0;
                long suma_R0 = 0;
                foreach (Individuo indi in individuos)
                {
                    if (indi.estado == 2)
                    {
                        // ¿ Curado -> desinmunizado ?

                        g = grupos.Find(x => x.grupo == indi.grupo);
                        pre = 1.0;
                        if (indi.dias_curado > MIN_INMUNIDAD && azar.NextDouble() < pre * PROB_RECONTAGIO)
                        {
                            // Perdida de la inmunidad

                            indi.estado = 3;
                            desinmunizados++;
                            curados--;
                            total_dias_infectados_curados += indi.dias_infectado;
                        }
                        else
                        {
                            // Sigue curado un día más

                            indi.dias_curado++;
                        }
                    }
                    else if (indi.estado == 1)
                    {
                        // Infectado

                        // Primero se sortea  la posibilidad de terminar la infección

                        g = grupos.Find(x => x.grupo == indi.grupo);
                        double pfi = indi.dias_infectado > g.duracion_infeccion ? 1 : PROB_FIN_INFECCION[g.ID, indi.dias_infectado];
                        n_sorteos_fin_infeccion[g.ID]++;
                        if (azar.NextDouble() < pfi)
                        {
                            // Ahora sortear si es por curación o muerte

                            n_fin_infeccion[g.ID]++;
                            double rcm = g.prob_curacion / 100;
                            if (azar.NextDouble() < rcm)
                            {
                                // Curado

                                indi.estado = 2;
                                indi.dias_curado = 0;
                                curados++;
                                infectados--;
                                if (indi.contagio_en_cluster)
                                {
                                    curados_clusters_actuales++;
                                    infectados_total_clusters--;
                                }
                                total_dias_infectados_curados += indi.dias_infectado;
                                res_fin_infeccion[g.ID, 0]++;
                            }
                            else
                            {
                                // Muerte

                                indi.estado = -1;
                                muertos++;
                                infectados--;
                                if (indi.contagio_en_cluster)
                                {
                                    muertos_clusters_actuales++;
                                    infectados_total_clusters--;
                                }
                                total_dias_infectados_muertos += indi.dias_infectado;
                                res_fin_infeccion[g.ID, 1]++;
                            }
                        }
                        else
                        {
                            // Sigue infectado un día más

                            indi.dias_infectado++;
                        }
                    }
                    else if (indi.estado == 2 || indi.estado == -1)
                    {
                        n_R0++;
                        suma_R0 += indi.enfermados;
                    }
                }

                // Focos inportados

                if (N_FOCOS_IMPORTADOS > 0 && DIAS_FOCOS_IMPORTADOS > 0 && GRUPO_IMPORTADO != -1)
                {
                    if (i > 0 && i % DIAS_FOCOS_IMPORTADOS == 0)
                    {
                        for (int j = 0; j < N_FOCOS_IMPORTADOS; j++)
                        {
                            while (true)
                            {
                                xt = 2.0 * azar.NextDouble() - 1.0;
                                yt = 2.0 * azar.NextDouble() - 1.0;
                                if ((xt * xt + yt * yt) <= 1.0) break;
                            }
                            xt = RADIO * xt;
                            yt = RADIO * yt;
                            Individuo individuo_nuevo = new Individuo(individuos.Count, id_grupo_importar, nombre_grupo_importar, xt, yt, 1, 0, 0, 0, -1, false);
                            individuos.Add(individuo_nuevo);
                            infectados++;
                            importados++;
                        }
                        if (TABLA_VECINOS)
                        {
                            if (!TablaVecinos(false))
                            {
                                tabla_vecinos_creada = false;
                                TABLA_VECINOS = false;
                                tabla_vecinos_en_uso.Text = string.Empty;
                                tabla_vecinos_en_uso.BackColor = Color.Red;
                            }
                        }
                        label23.ForeColor = Color.Red;
                    }
                    else
                    {
                        label23.ForeColor = Color.Black;
                    }
                }
                if (num_dias_exportar > 0 && prox_dia_exportar != -1 && i == dias_exportar[prox_dia_exportar])
                {
                    string ficheroex = Path.Combine(carpeta, string.Format("Ind_{0:D3}.iii", i + 1));
                    ExportarPoblacion(ficheroex);
                    Distancias(i + 1);
                    v_dias_exportar.Text = string.Format("{0}", i + 1);
                    Application.DoEvents();
                    prox_dia_exportar++;
                    if (prox_dia_exportar == num_dias_exportar)
                    {
                        prox_dia_exportar = -1;
                    }
                }

                // La monitorización se hace al final del día

                ActualizaMonitorEstados();
                acumulador_infectados += infectados;
                if ((total_recorridos) > 0)
                {
                    recorrido_individuo_activo = total_distancias_ida / total_recorridos;
                }
                else
                {
                    recorrido_individuo_activo = 0.0;
                }
                v_recorridos.Text = string.Format("{0:f2}", recorrido_individuo_activo);
                if (acumulador_infectados > 0)
                {
                    contactos_de_riesgo = (double)total_contactos_de_riesgo / acumulador_infectados;
                }
                else
                {
                    contactos_de_riesgo = 0.0;
                }
                if (curados + muertos > 0)
                {
                    media_dias_infectado = (double)(total_dias_infectados_curados + total_dias_infectados_muertos) / (curados + muertos);
                }
                else
                {
                    media_dias_infectado = 0.0;
                }
                if (n_R0 > 0)
                {
                    R0 = (double)suma_R0 / n_R0;
                    if (R0 > max_r0) max_r0 = R0;
                }
                else
                {
                    R0 = 0.0;
                }
                v_contactos.Text = string.Format("{0:f2}", contactos_de_riesgo);
                v_infectado.Text = string.Format("{0:f2}", media_dias_infectado);
                v_r0.Text = string.Format("{0:f2}", R0);
                sw.WriteLine(string.Format("{0,3} {1,10} {2,10} {3,10} {4,10} {5,10} {6,10} {7,10:f2} {8,10:f2} {9,10:f1} {10,10:f2} {11,10} {12,10} {13,10}", i + 1, sanos, infectados, importados, curados, desinmunizados, muertos, recorrido_individuo_activo, contactos_de_riesgo, media_dias_infectado, R0, infectados_total_clusters, curados_clusters_actuales, muertos_clusters_actuales));
                sw.Flush();
                lzo_ex.ActualizaDia(i + 1);
                lzo_ex.Grafico(false);
                Application.DoEvents();
                dias_sim = i + 1;
                dt = (DateTime.Now - tiempo_inicio);
                if (infectados == 0 && (N_FOCOS_IMPORTADOS == 0 || (N_FOCOS_IMPORTADOS > 0 && sanos == 0)))
                {
                    MessageBox.Show("No quedan infectados", string.Format("Simular [{0}]", PREFIJO), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                if (paradas)
                {
                    DialogResult respuesta = MessageBox.Show("¿ Siguiente ?", string.Format("Simular día: {0} [{1}]", PREFIJO, i + 1), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (respuesta != DialogResult.Yes)
                    {
                        dt = (DateTime.Now - tiempo_inicio);
                        break;
                    }
                }
            }
            label23.ForeColor = Color.Black;
            tiempo = CierraCiclo(sw, dt, "Simulados", dias_sim);
            MessageBox.Show(tiempo, string.Format("Simular [{0}]", PREFIJO), MessageBoxButtons.OK, MessageBoxIcon.Information);
            b_crea_poblacion.ForeColor = Color.Black;
        }
        private string CierraCiclo(StreamWriter sw, TimeSpan dt, string texto, int dias_sim)
        {
            ActualizaMonitorEstados();
            sw.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            sw.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos     Ida     críticos  infectados         R0 Infectados    Curados    Muertos");
            sw.WriteLine();
            sw.WriteLine(string.Format("Infectados importados           {0,20:N0}", importados));
            sw.WriteLine();
            sw.WriteLine(string.Format("Total pasos                     {0,20:N0}", total_pasos));
            sw.WriteLine(string.Format("Total recorridos                {0,20:N0}", total_recorridos));
            sw.WriteLine(string.Format("Total distancia recorrida       {0,20:N0}", total_distancias_ida));
            sw.WriteLine(string.Format("Contactos de riesgo             {0,20:N0}", total_contactos_de_riesgo));
            sw.WriteLine(string.Format("Acumulador infectados           {0,20:N0}", acumulador_infectados));
            sw.WriteLine();
            sw.WriteLine(string.Format("Total días infectados (curados) {0,20:N0} {1,10:f2}", total_dias_infectados_curados, curados == 0 ? 0 : total_dias_infectados_curados / curados));
            sw.WriteLine(string.Format("Total días infectados (muertos) {0,20:N0} {1,10:f2}", total_dias_infectados_muertos, muertos == 0 ? 0 : total_dias_infectados_muertos / muertos));
            sw.WriteLine(string.Format("Total días infectados           {0,20:N0} {1,10:f2}", total_dias_infectados_curados + total_dias_infectados_muertos, muertos == 0 ? 0 : (total_dias_infectados_curados + total_dias_infectados_muertos) / (curados + muertos)));
            sw.WriteLine();
            Distancias(-1);
            sw.WriteLine();
            //ClustersFinal(sw);
            long[,] infectadores = new long[grupos.Count, 2];
            long[,] suma_curados_muertos = new long[grupos.Count, 2];
            long[] suma_total = new long[grupos.Count];
            int[] infectador_max = new int[grupos.Count];
            int[] infectador_min = new int[grupos.Count];
            for (int i = 0; i < grupos.Count; i++)
            {
                infectadores[i, 0] = infectadores[i, 1] = 0;
                suma_curados_muertos[i, 0] = suma_curados_muertos[i, 1] = 0;
                suma_total[i] = 0;
                infectador_max[i] = -1;
                infectador_min[i] = int.MaxValue;
            }
            int k;
            Grupo g_indi;
            foreach (Individuo indi in individuos)
            {
                g_indi = grupos.Find(v => v.grupo == indi.grupo);
                suma_total[g_indi.ID] += indi.enfermados;
                if (indi.estado == 2 || indi.estado == -1)
                {
                    k = indi.estado == 2 ? 0 : 1;
                    infectadores[g_indi.ID, k]++;
                    suma_curados_muertos[g_indi.ID, k] += indi.enfermados;
                    if (indi.enfermados < infectador_min[g_indi.ID]) infectador_min[g_indi.ID] = indi.enfermados;
                    if (indi.enfermados > infectador_max[g_indi.ID]) infectador_max[g_indi.ID] = indi.enfermados;
                }
            }
            foreach (Grupo g in grupos)
            {
                sw.WriteLine(string.Format("Grupo {0,3:N0} {1}", g.ID, g.grupo));
                for (int i = 0; i < g.duracion_infeccion; i++)
                {
                    sw.Write(string.Format(" {0,6:N0}", i));
                }
                sw.WriteLine();
                for (int i = 0; i < g.duracion_infeccion; i++)
                {
                    sw.Write(" ------");
                }
                sw.WriteLine();
                for (int i = 0; i < g.duracion_infeccion; i++)
                {
                    sw.Write(string.Format("{0,7:f4}", PROB_FIN_INFECCION[g.ID, i]));
                }
                sw.WriteLine();
                sw.WriteLine(string.Format("Número intentos fin infección         {0,14:N0}", n_sorteos_fin_infeccion[g.ID]));
                sw.WriteLine(string.Format("Número finales de infección           {0,14:N0} {1,10:f2}", n_fin_infeccion[g.ID], 100.0 * n_fin_infeccion[g.ID] / n_sorteos_fin_infeccion[g.ID]));
                sw.WriteLine(string.Format("Número finales curacion               {0,14:N0} {1,10:f2}", res_fin_infeccion[g.ID, 0], n_fin_infeccion[g.ID] == 0 ? 0 : 100.0 * res_fin_infeccion[g.ID, 0] / n_fin_infeccion[g.ID]));
                sw.WriteLine(string.Format("Número finales muerte                 {0,14:N0} {1,10:f2}", res_fin_infeccion[g.ID, 1], n_fin_infeccion[g.ID] == 0 ? 0 : 100.0 * res_fin_infeccion[g.ID, 1] / n_fin_infeccion[g.ID]));
                sw.WriteLine(string.Format("Total infecciones                     {0,14:N0}", suma_total[g.ID]));
                sw.WriteLine(string.Format("Total infecciones por curados         {0,14:N0}", suma_curados_muertos[g.ID, 0]));
                sw.WriteLine(string.Format("Total infecciones por muertos         {0,14:N0}", suma_curados_muertos[g.ID, 1]));
                sw.WriteLine(string.Format("Total infectadores curados            {0,14:N0}", infectadores[g.ID, 0]));
                sw.WriteLine(string.Format("Total infectadores muertos            {0,14:N0}", infectadores[g.ID, 1]));
                if (infectadores[g.ID, 0] > 0)
                {
                    sw.WriteLine(string.Format("Máximo de contagios por 1 individuo   {0,14:N0}", infectador_max[g.ID]));
                    sw.WriteLine(string.Format("Mínimo de contagios por 1 individuo   {0,14:N0}", infectador_min[g.ID]));
                    sw.WriteLine(string.Format("Falso (R0) curados                    {0,14:f4}", infectadores[g.ID, 0] == 0 ? 0 : (double)suma_curados_muertos[g.ID, 0] / infectadores[g.ID, 0]));
                    sw.WriteLine(string.Format("Falso (R0) muertos                    {0,14:f4}", infectadores[g.ID, 1] == 0 ? 0 : (double)suma_curados_muertos[g.ID, 1] / infectadores[g.ID, 1]));
                }
                else
                {
                    sw.WriteLine(string.Format("Máximo de contagios por 1 individuo   {0,14}", "-"));
                    sw.WriteLine(string.Format("Mínimo de contagios por 1 individuo   {0,14}", "-"));
                    sw.WriteLine(string.Format("Falso (R0) curados                    {0,14}", "-"));
                    sw.WriteLine(string.Format("Falso (R0) muertos                    {0,14}", "-"));
                }
                sw.WriteLine();
            }
            sw.WriteLine(string.Format("Número básico de reproducción (R0)    {0,14:f4}", max_r0));
            string tiempo = string.Format("{0} {1:N0} días. {2:f1} s", texto, dias_sim, dt.TotalMilliseconds / 1000.0d);
            sw.WriteLine(tiempo);
            sw.WriteLine();
            sw.Close();
            b_cancelar.Enabled = false;
            return tiempo;
        }
        private void ClustersInicio(StreamWriter sw)
        {
            sw.WriteLine("Cluster GID Grupo                Individuos     Sanos  Infectados    Curados Desinmuniz    Muertos");
            sw.WriteLine("------- --- -------------------- ---------- ---------- ---------- ---------- ---------- ----------");
            int estado;
            long[] sum_total_clusters = new long[5];
            long[] num_indi_estado = new long[5];
            for (int n = 0; n < NUMERO_CLUSTERS; n++)
            {
                num_indi_estado[0] = num_indi_estado[1] = num_indi_estado[2] = num_indi_estado[3] = num_indi_estado[4] = 0;
                for (int nic = 0; nic < n_individuos_cluster[n]; nic++)
                {
                    estado = individuos.ElementAt(individuos_cluster_id[n, nic]).estado;
                    switch (estado)
                    {
                        case 0:
                            num_indi_estado[0]++;
                            break;
                        case 1:
                            num_indi_estado[1]++;
                            break;
                        case 2:
                            num_indi_estado[2]++;
                            break;
                        case 3:
                            num_indi_estado[3]++;
                            break;
                        default:
                            num_indi_estado[4]++;
                            break;
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    sum_total_clusters[i] += num_indi_estado[i];
                }
                sw.WriteLine(string.Format("{0,7:N0} {1,3:N0} {2,-20} {3,10} {4,10} {5,10} {6,10} {7,10} {8,10}", n + 1, grupo_cluster[n], grupos.ElementAt(grupo_cluster[n]).grupo, n_individuos_cluster[n], num_indi_estado[0], num_indi_estado[1], num_indi_estado[2], num_indi_estado[3], num_indi_estado[4]));
            }
            long total = 0;
            for (int i = 0; i < 5; i++)
            {
                total += sum_total_clusters[i];
            }
            sw.WriteLine("------- --- -------------------- ---------- ---------- ---------- ---------- ---------- ----------");
            sw.WriteLine(string.Format("                                 {0,10} {1,10} {2,10} {3,10} {4,10} {5,10}", total, sum_total_clusters[0], sum_total_clusters[1], sum_total_clusters[2], sum_total_clusters[3], sum_total_clusters[4]));
            sw.WriteLine();
        }
        private void ClustersFinal(StreamWriter sw)
        {
            sw.WriteLine("                                                       -------------- Dentro y fuera ------------- -------- Dentro -----      %");
            sw.WriteLine("Cluster GID Grupo                Individuos      Sanos Infectados    Curados Desinmuniz    Muertos   Intentos Infectados   Aciertos");
            sw.WriteLine("------- --- -------------------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            int estado;
            long[] suma = new long[2];
            long[] sum_total_clusters = new long[5];
            long[] num_indi_estado = new long[5];
            for (int n = 0; n < NUMERO_CLUSTERS; n++)
            {
                suma[0] += intentos_cluster[n];
                suma[1] += infectados_cluster[n];
                num_indi_estado[0] = num_indi_estado[1] = num_indi_estado[2] = num_indi_estado[3] = num_indi_estado[4] = 0;
                for (int nic = 0; nic < n_individuos_cluster[n]; nic++)
                {
                    estado = individuos.ElementAt(individuos_cluster_id[n, nic]).estado;
                    switch (estado)
                    {
                        case 0:
                            num_indi_estado[0]++;
                            break;
                        case 1:
                            num_indi_estado[1]++;
                            break;
                        case 2:
                            num_indi_estado[2]++;
                            break;
                        case 3:
                            num_indi_estado[3]++;
                            break;
                        default:
                            num_indi_estado[4]++;
                            break;
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    sum_total_clusters[i] += num_indi_estado[i];
                }
                sw.WriteLine(string.Format("{0,7:N0} {1,3:N0} {2,-20} {3,10} {4,10} {5,10} {6,10} {7,10} {8,10} {9,10} {10,10} {11,10:f2}", n + 1, grupo_cluster[n], grupos.ElementAt(grupo_cluster[n]).grupo, n_individuos_cluster[n], num_indi_estado[0], num_indi_estado[1], num_indi_estado[2], num_indi_estado[3], num_indi_estado[4], intentos_cluster[n], infectados_cluster[n], intentos_cluster[n] == 0 ? 0 : 100.0 * infectados_cluster[n] / intentos_cluster[n]));
            }
            long total = 0;
            for (int i = 0; i < 5; i++)
            {
                total += sum_total_clusters[i];
            }
            sw.WriteLine("------- --- -------------------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            sw.WriteLine(string.Format("                                 {0,10} {1,10} {2,10} {3,10} {4,10} {5,10} {6,10} {7,10} {8,10:f2}", total, sum_total_clusters[0], sum_total_clusters[1], sum_total_clusters[2], sum_total_clusters[3], sum_total_clusters[4], suma[0], suma[1], suma[0] == 0 ? 0 : 100.0 * suma[1] / suma[0]));
            sw.WriteLine();
        }
        private void Distancias(int dia)
        {
            linea_estado.Text = "Calculando distancias ...";
            Application.DoEvents();
            string carpeta = string.Format(@"{0}:\{1}\{2}", U_SALIDAS, CARPETA_SALIDAS, PREFIJO);
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
            string fichero = string.Format(@"{0}\saux.log", carpeta);
            FileStream fw = new FileStream(fichero, FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(fw);
            if (dia == 0)
            {
                sw.WriteLine("Situación inicial");
                sw.WriteLine();
                ClustersInicio(sw);
            }
            else if (dia == -1)
            {
                sw.WriteLine("Situación final");
                sw.WriteLine();
                ClustersFinal(sw);
            }
            else
            {
                string ficheroex = Path.Combine(carpeta, string.Format("Ind_{0:D3}.iii", dia));
                sw.WriteLine(string.Format("Exportada población al fichero {0}", ficheroex));
                sw.WriteLine();
                ClustersFinal(sw);
            }
            long[,] par = new long[5, 5];
            double[,] dis = new double[5, 5];
            int e1;
            int e2;
            Individuo indi_i;
            Individuo indi_j;
            double x;
            double y;
            double d;
            for (int i = 0; i < individuos.Count; i++)
            {
                indi_i = individuos.ElementAt(i);
                for (int j = 0; j < individuos.Count; j++)
                {
                    indi_j = individuos.ElementAt(j);
                    x = indi_i.xi - indi_j.xi;
                    y = indi_i.yi - indi_j.yi;
                    d = Math.Sqrt(x * x + y * y);
                    e1 = indi_i.estado == -1 ? 4 : indi_i.estado;
                    e2 = indi_j.estado == -1 ? 4 : indi_j.estado;
                    dis[e1, e2] += d;
                    par[e1, e2]++;
                }
            }
            sw.Write("Individuos     ");
            for (int i = 0; i < 5; i++)
            {
                sw.Write(string.Format(" {0,15}", nombre_estados[i]));
            }
            sw.WriteLine();
            sw.Write("---------------");
            for (int i = 0; i < 5; i++)
            {
                sw.Write(" ---------------");
            }
            sw.WriteLine();
            sw.Write("               ");
            sw.Write(string.Format(" {0,15:N0}", sanos));
            sw.Write(string.Format(" {0,15:N0}", infectados));
            sw.Write(string.Format(" {0,15:N0}", curados));
            sw.Write(string.Format(" {0,15:N0}", desinmunizados));
            sw.Write(string.Format(" {0,15:N0}", muertos));
            sw.WriteLine();
            sw.Write("---------------");
            for (int i = 0; i < 5; i++)
            {
                sw.Write(" ---------------");
            }
            sw.WriteLine();
            sw.WriteLine();
            sw.WriteLine("Número de pares");
            sw.Write("Estado         ");
            for (int i = 0; i < 5; i++)
            {
                sw.Write(string.Format(" {0,15}", nombre_estados[i]));
            }
            sw.WriteLine();
            sw.Write("---------------");
            for (int i = 0; i < 5; i++)
            {
                sw.Write(" ---------------");
            }
            sw.WriteLine();
            for (int i = 0; i < 5; i++)
            {
                sw.Write(string.Format("{0,-15}", nombre_estados[i]));
                for (int j = 0; j <= i; j++)
                {
                    sw.Write(string.Format(" {0,15:N0}", par[i, j]));
                }
                sw.WriteLine();
            }
            sw.Write("---------------");
            for (int i = 0; i < 5; i++)
            {
                sw.Write(" ---------------");
            }
            sw.WriteLine();
            sw.WriteLine();
            sw.WriteLine("Distancia media");
            sw.Write("Estado         ");
            for (int i = 0; i < 5; i++)
            {
                sw.Write(string.Format(" {0,15}", nombre_estados[i]));
            }
            sw.WriteLine();
            sw.Write("---------------");
            for (int i = 0; i < 5; i++)
            {
                sw.Write(" ---------------");
            }
            sw.WriteLine();
            for (int i = 0; i < 5; i++)
            {
                sw.Write(string.Format("{0,-15}", nombre_estados[i]));
                for (int j = 0; j <= i; j++)
                {
                    sw.Write(string.Format(" {0,15:f1}", par[i, j] == 0 ? 0 : dis[i, j] / par[i, j]));
                }
                sw.WriteLine();
            }
            sw.Write("---------------");
            for (int i = 0; i < 5; i++)
            {
                sw.Write(" ---------------");
            }
            sw.WriteLine();
            sw.WriteLine();
            for (int i = 0; i < 9; i++)
            {
                sw.Write("----------------");
            }
            sw.WriteLine();
            for (int i = 0; i < 9; i++)
            {
                sw.Write("----------------");
            }
            sw.WriteLine();
            sw.WriteLine();
            sw.Close();
            linea_estado.Text = string.Empty;
            Application.DoEvents();
        }
        private int ContagiosDia()
        {
            /*
             * DEvuelve:
             *  0 Normal
             * -1 Cancelado
             */

            if (cancelar || lzo_ex.cancelar)
            {
                return -1;
            }
            Grupo g_indi;

            // Excluiremos a los curados y muertos de caminar

            // Para acelerar el cálculo, se construye una lista con los individuos activos (sanos + desinmunizados + infectados)

            List<Individuo> indi_activos = new List<Individuo>();
            List<Grupo> g_activos = new List<Grupo>();
            foreach (Individuo indi in individuos)
            {
                if (cancelar || lzo_ex.cancelar)
                {
                    return -1;
                }
                if (indi.estado == 2 || indi.estado == -1)
                {
                    // 'indi' Curado o Muerto

                    continue;
                }
                indi_activos.Add(indi);
                g_indi = grupos.Find(v => v.grupo == indi.grupo);
                g_activos.Add(g_indi);
            }
            double x;
            double y;
            double d;
            Individuo indi_i;
            Individuo indi_j;
            double pictg_i;
            double pictg_j;
            int pasos = 0;
            long pendientes_de_volver = sanos + desinmunizados + infectados;
            while (true)
            {
                // Caminos de ida y vuelta
                // Los individuos dan un paso tras otro, hasta que no quede ninguno 'pendiente' de volver al punto de partida

                pasos++;
                v_paso.Text = string.Format("{0}", pasos);
                Application.DoEvents();
                foreach (Individuo indi in indi_activos)
                {
                    if (cancelar || lzo_ex.cancelar)
                    {
                        return -1;
                    }
                    if (indi.sentido == 0)
                    {
                        // Ida

                        if (indi.pasos_dados == 0)
                        {
                            // Primer paso

                            if (azar.NextDouble() >= 0.5)
                            {
                                indi.incx = LON_PASO2 * azar.NextDouble();
                            }
                            else
                            {
                                indi.incx = -LON_PASO2 * azar.NextDouble();
                            }
                            if (azar.NextDouble() >= 0.5)
                            {
                                indi.incy = LON_PASO2 * azar.NextDouble();
                            }
                            else
                            {
                                indi.incy = -LON_PASO2 * azar.NextDouble();
                            }
                            indi.incd = Math.Sqrt(indi.incx * indi.incx + indi.incy * indi.incy);
                        }

                        // Resto de pasos. Se repite el primer paso

                        indi.x += indi.incx;
                        indi.y += indi.incy;
                        indi.pasos[indi.pasos_dados, 0] = indi.x;
                        indi.pasos[indi.pasos_dados, 1] = indi.y;
                        total_pasos++;
                        indi.distancia += indi.incd;
                        indi.pasos_dados++;
                        if (indi.pasos_dados == indi.pasos_dia)
                        {
                            // El próximo paso será de vuelta

                            indi.sentido = 1;
                        }
                    }
                    else if (indi.sentido == 1)
                    {
                        // Vuelta. Empieza repitiendo el último punto de la ida

                        indi.pasos_dados--;
                        if (indi.pasos_dados == -1)
                        {
                            // Ha vuelto a casa

                            total_recorridos++;
                            total_distancias_ida += indi.distancia;
                            indi.x = indi.xi;
                            indi.y = indi.yi;
                            pendientes_de_volver--;

                            // En los próximos pasos no se moverá

                            indi.sentido = -1;
                        }
                        else
                        {
                            indi.x = x = indi.pasos[indi.pasos_dados, 0];
                            indi.y = y = indi.pasos[indi.pasos_dados, 1];
                        }
                    }
                }

                // Todos los individuos sanos y contagiados que no habían vuelto a casa (pendientes) han dado un paso

                // Determinación de los contactos de riesgo en función de la distancia entre individuos

                for (int i = 0; i < indi_activos.Count; i++)
                {
                    indi_i = indi_activos.ElementAt(i);
                    g_indi = g_activos.ElementAt(i);
                    pictg_i = g_indi.prob_contagio / 100.0;
                    for (int j = i + 1; j < indi_activos.Count; j++)
                    {
                        indi_j = indi_activos.ElementAt(j);
                        x = indi_i.x - indi_j.x;
                        y = indi_i.y - indi_j.y;
                        d = x * x + y * y;
                        if (d < CONTACTO2)
                        {
                            // Dentro de la distancia de contacto

                            if ((indi_i.estado == 0 || indi_i.estado == 3) && indi_j.estado == 1 && indi_j.dias_infectado >= LATENCIA)
                            {
                                // Posible contagio de 'i'

                                total_contactos_de_riesgo++;
                                if (azar.NextDouble() < pictg_i * PROB_CONTAGIO)
                                {
                                    // 'i' infectado 

                                    if (indi_i.estado == 0)
                                    {
                                        sanos--;
                                    }
                                    else
                                    {
                                        desinmunizados--;
                                    }
                                    indi_i.estado = 1;
                                    infectados++;

                                    // Una muesca más en la culata de 'j'

                                    indi_j.enfermados++;
                                }
                            }
                            else if (indi_i.estado == 1 && indi_i.dias_infectado >= LATENCIA && (indi_j.estado == 0 || indi_j.estado == 3))
                            {
                                // Posible contagio de 'j'

                                total_contactos_de_riesgo++;
                                g_indi = g_activos.ElementAt(j);
                                pictg_j = g_indi.prob_contagio / 100.0;
                                if (azar.NextDouble() < pictg_j * PROB_CONTAGIO)
                                {
                                    // 'j' infectado 

                                    if (indi_j.estado == 0)
                                    {
                                        sanos--;
                                    }
                                    else
                                    {
                                        desinmunizados--;
                                    }
                                    indi_j.estado = 1;
                                    infectados++;

                                    // Una muesca más en la culata de 'i'

                                    indi_i.enfermados++;
                                }
                            }
                        }
                    }
                }

                // Listos para un nuevo paso

                if (pendientes_de_volver == 0)
                {
                    // Todos están en casa. El día ha terminado

                    return 0;
                }
            }
        }
        private int ContagiosDiaVecinos()
        {
            /*
             * DEvuelve:
             *  0 Normal
             * -1 Cancelado
             */

            if (cancelar || lzo_ex.cancelar)
            {
                return -1;
            }
            Grupo g_indi;

            // Excluiremos a los curados y muertos de caminar

            // Para acelerar el cálculo, se construye una lista con los individuos activos (sanos + desinmunizados + infectados)

            List<Individuo> indi_activos = new List<Individuo>();
            SortedList<int, int> puntero_activo = new SortedList<int, int>();
            List<Grupo> grupo_indi_activo = new List<Grupo>();
            foreach (Individuo indi in individuos)
            {
                if (cancelar || lzo_ex.cancelar)
                {
                    return -1;
                }
                if (indi.estado == 2 || indi.estado == -1)
                {
                    // 'indi' Curado o Muerto

                    continue;
                }
                puntero_activo.Add(indi.ID, indi_activos.Count);
                g_indi = grupos.Find(v => v.grupo == indi.grupo);
                grupo_indi_activo.Add(g_indi);
                indi_activos.Add(indi);
            }

            int i;
            int j;
            double x;
            double y;
            double d;
            Individuo indi_i;
            Individuo indi_j;
            double pictg_i;
            double pictg_j;
            int pasos = 0;
            long pendientes_de_volver = sanos + desinmunizados + infectados;
            while (true)
            {
                // Caminos de ida y vuelta
                // Los individuos dan un paso tras otro, hasta que no quede ninguno 'pendiente' de volver al punto de partida

                pasos++;
                v_paso.Text = string.Format("{0}", pasos);
                Application.DoEvents();
                foreach (Individuo indi in indi_activos)
                {
                    if (cancelar || lzo_ex.cancelar)
                    {
                        return -1;
                    }
                    if (indi.sentido == 0)
                    {
                        // Ida

                        if (indi.pasos_dados == 0)
                        {
                            // Primer paso

                            if (azar.NextDouble() >= 0.5)
                            {
                                indi.incx = LON_PASO2 * azar.NextDouble();
                            }
                            else
                            {
                                indi.incx = -LON_PASO2 * azar.NextDouble();
                            }
                            if (azar.NextDouble() >= 0.5)
                            {
                                indi.incy = LON_PASO2 * azar.NextDouble();
                            }
                            else
                            {
                                indi.incy = -LON_PASO2 * azar.NextDouble();
                            }
                            indi.incd = Math.Sqrt(indi.incx * indi.incx + indi.incy * indi.incy);
                        }

                        // Resto de pasos. Se repite el primer paso

                        indi.x += indi.incx;
                        indi.y += indi.incy;
                        indi.pasos[indi.pasos_dados, 0] = indi.x;
                        indi.pasos[indi.pasos_dados, 1] = indi.y;
                        total_pasos++;
                        indi.distancia += indi.incd;
                        indi.pasos_dados++;
                        if (indi.pasos_dados == indi.pasos_dia)
                        {
                            // El próximo paso será de vuelta

                            indi.sentido = 1;
                        }
                    }
                    else if (indi.sentido == 1)
                    {
                        // Vuelta. Empieza repitiendo el último punto de la ida

                        indi.pasos_dados--;
                        if (indi.pasos_dados == -1)
                        {
                            // Ha vuelto a casa

                            total_recorridos++;
                            total_distancias_ida += indi.distancia;
                            indi.x = indi.xi;
                            indi.y = indi.yi;
                            pendientes_de_volver--;

                            // En los próximos pasos no se moverá

                            indi.sentido = -1;
                        }
                        else
                        {
                            indi.x = x = indi.pasos[indi.pasos_dados, 0];
                            indi.y = y = indi.pasos[indi.pasos_dados, 1];
                        }
                    }
                }

                // Todos los individuos sanos y contagiados que no habían vuelto a casa (pendientes) han dado un paso

                // Determinar los contactos de riesgo en función de la distancia entre individuos

                for (int ci = 0; ci < indi_activos.Count; ci++)
                {
                    indi_i = indi_activos.ElementAt(ci);
                    g_indi = grupo_indi_activo.ElementAt(ci);
                    i = indi_i.ID;
                    pictg_i = g_indi.prob_contagio / 100.0;
                    for (int cj = 0; cj < nvecinas[indi_i.ID]; cj++)
                    {
                        if (!puntero_activo.TryGetValue(vecinas[i, cj], out j)) continue;
                        indi_j = indi_activos.ElementAt(j);
                        x = indi_i.x - indi_j.x;
                        y = indi_i.y - indi_j.y;
                        d = x * x + y * y;
                        if (d < CONTACTO2)
                        {
                            // Dentro de la distancia de contacto

                            if ((indi_i.estado == 0 || indi_i.estado == 3) && indi_j.estado == 1 && indi_j.dias_infectado >= LATENCIA)
                            {
                                // Posible contagio de 'indi_i'

                                total_contactos_de_riesgo++;
                                if (azar.NextDouble() < pictg_i * PROB_CONTAGIO)
                                {
                                    // 'indi_i' infectado 

                                    if (indi_i.estado == 0)
                                    {
                                        sanos--;
                                    }
                                    else
                                    {
                                        desinmunizados--;
                                    }
                                    indi_i.estado = 1;
                                    infectados++;

                                    // Una muesca más en la culata de 'indi_j'

                                    indi_j.enfermados++;
                                }
                            }
                            else if (indi_i.estado == 1 && indi_i.dias_infectado >= LATENCIA && (indi_j.estado == 0 || indi_j.estado == 3))
                            {
                                // Posible contagio de 'indi_j'

                                total_contactos_de_riesgo++;
                                g_indi = grupo_indi_activo.ElementAt(j);
                                pictg_j = g_indi.prob_contagio / 100.0;
                                if (azar.NextDouble() < pictg_j * PROB_CONTAGIO)
                                {
                                    // 'indi_j' infectado 

                                    if (indi_j.estado == 0)
                                    {
                                        sanos--;
                                    }
                                    else
                                    {
                                        desinmunizados--;
                                    }
                                    indi_j.estado = 1;
                                    infectados++;

                                    // Una muesca más en la culata de 'indi_i'

                                    indi_i.enfermados++;
                                }
                            }
                        }
                    }
                }

                // Listos para un nuevo paso

                if (pendientes_de_volver == 0)
                {
                    // Todos están en casa. El día ha terminado

                    return 0;
                }
            }
        }
        private int ContagiosCluster()
        {
            Individuo indi_i;
            Individuo indi_j;
            for (int n = 0; n < NUMERO_CLUSTERS; n++)
            {
                for (int i = 0; i < n_individuos_cluster[n]; i++)
                {
                    indi_i = individuos.ElementAt(individuos_cluster_id[n, i]);
                    for (int j = 0; j < n_individuos_cluster[n]; j++)
                    {
                        if (i == j) continue;
                        indi_j = individuos.ElementAt(individuos_cluster_id[n, j]);
                        if ((indi_i.estado == 0 || indi_i.estado == 3) && indi_j.estado == 1 && indi_j.dias_infectado >= LATENCIA)
                        {
                            // Posible contagio de 'i'

                            intentos_cluster[n]++;
                            if (azar.NextDouble() < PROB_CONTAGIO_Cd2)
                            {
                                // 'i' infectado 

                                if (indi_i.estado == 0)
                                {
                                    sanos--;
                                }
                                else
                                {
                                    desinmunizados--;
                                }
                                indi_i.estado = 1;
                                infectados++;

                                // 'i' se ha contagiado en el cluster 'n'

                                indi_i.contagio_en_cluster = true;
                                infectados_cluster[n]++;
                                infectados_total_clusters++;

                                // Una muesca más en la culata de 'j'

                                indi_j.enfermados++;
                            }
                        }
                        else if (indi_i.estado == 1 && indi_i.dias_infectado >= LATENCIA && (indi_j.estado == 0 || indi_j.estado == 3))
                        {
                            // Posible contagio de 'j'

                            intentos_cluster[n]++;
                            if (azar.NextDouble() < PROB_CONTAGIO_Cd2)
                            {
                                // 'j' infectado 

                                if (indi_j.estado == 0)
                                {
                                    sanos--;
                                }
                                else
                                {
                                    desinmunizados--;
                                }
                                indi_j.estado = 1;
                                infectados++;

                                // 'j' se ha contagiado en el cluster 'n'

                                indi_j.contagio_en_cluster = true;
                                infectados_cluster[n]++;
                                infectados_total_clusters++;

                                // Una muesca más en la culata de 'i'

                                indi_i.enfermados++;
                            }
                        }
                    }
                }
            }
            return 0;
        }
        private void B_cancelar_Click(object sender, EventArgs e)
        {
            linea_estado.Text = "Cancelando ...";
            BotonCancelar(false);
            if (lzo_ex != null) lzo_ex.BotonCancelar(false);
            Application.DoEvents();
        }
        public void BotonCancelar(bool que)
        {
            cancelar = !que;
            b_cancelar.Enabled = que;
        }
        #endregion

        #region Individuos
        private void ActualizaMonitorEstados()
        {
            v_sanos.Text = string.Format("{0:N0}", sanos);
            v_desinmunizados.Text = string.Format("{0:N0}", desinmunizados);
            v_infectados.Text = string.Format("{0:N0}", infectados);
            v_curados.Text = string.Format("{0:N0}", curados);
            v_muertos.Text = string.Format("{0:N0}", muertos);
            Application.DoEvents();
        }
        private void B_importa_individuos_Click(object sender, EventArgs e)
        {
            OpenFileDialog leefichero = new OpenFileDialog
            {
                Filter = "III (*.iii)|*.iii|All files (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false
            };
            if (leefichero.ShowDialog() == DialogResult.OK)
            {
                Desactiva(true);
                string s;
                string[] sd;
                FileStream fr = new FileStream(leefichero.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fr);
                int id;
                double xi;
                double yi;
                int estado;
                int dias_infectado;
                int dias_curado;
                int enfermados;
                int cluster;
                int contagiado_en_cluster;
                individuos.Clear();
                SinPoblacion();
                sanos = 0;
                desinmunizados = 0;
                infectados = 0;
                curados = 0;
                muertos = 0;
                ActualizaMonitorEstados();

                // Primero los clusters

                NUMERO_CLUSTERS = Convert.ToInt32(sr.ReadLine());
                d_numero_c.Text = string.Format("{0}", NUMERO_CLUSTERS);
                for (int i = 0; i < NUMERO_CLUSTERS; i++)
                {
                    s = sr.ReadLine();
                    sd = s.Split(';');
                    if (sd.Length != 2)
                    {
                        MessageBox.Show(string.Format("Número incorrecto de campos: {0} {1}", sd.Length, s), "Importar individuos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (sr != null) sr.Close();
                        return;
                    }
                    grupo_cluster[i] = Convert.ToInt32(sd[0]);
                    n_individuos_cluster[i] = Convert.ToInt32(sd[1]);
                    for (int j = 0; j < n_individuos_cluster[i]; j++)
                    {
                        s = sr.ReadLine();
                        individuos_cluster_id[i, j] = Convert.ToInt32(s);
                    }
                }

                // Ahora los individuos

                while (!sr.EndOfStream)
                {
                    s = sr.ReadLine();
                    sd = s.Split(';');
                    if (sd.Length != 10)
                    {
                        MessageBox.Show(string.Format("Número incorrecto de campos: {0} {1}", sd.Length, s), "Importar individuos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (sr != null) sr.Close();
                        return;
                    }
                    id = Convert.ToInt32(sd[0]);
                    xi = Convert.ToDouble(sd[2]);
                    yi = Convert.ToDouble(sd[3]);
                    estado = Convert.ToInt32(sd[4]);
                    dias_infectado = Convert.ToInt32(sd[5]);
                    dias_curado = Convert.ToInt32(sd[6]);
                    enfermados = Convert.ToInt32(sd[7]);
                    cluster = Convert.ToInt32(sd[8]);
                    contagiado_en_cluster = Convert.ToInt32(sd[9]);
                    individuos.Add(new Individuo(individuos.Count, id, sd[1], xi, yi, estado, dias_infectado, dias_curado, enfermados, cluster, contagiado_en_cluster == 1 ? true : false));
                    switch (estado)
                    {
                        case 0:
                            sanos++;
                            break;
                        case 1:
                            infectados++;
                            break;
                        case 2:
                            curados++;
                            break;
                        case 3:
                            desinmunizados++;
                            break;
                        default:
                            muertos++;
                            break;
                    }
                }
                sr.Close();
                ActualizaMonitorEstados();
                if (individuos.Count == 0)
                {
                    MessageBox.Show("No se ha importado ningún individuo", "Importar individuos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string sal = string.Format("Importados {0:N0} individuos", individuos.Count);
                    sal += Environment.NewLine + string.Format("Sanos      {0:N0}", sanos);
                    sal += Environment.NewLine + string.Format("Infectados {0:N0}", infectados);
                    sal += Environment.NewLine + string.Format("Curados    {0:N0}", curados);
                    sal += Environment.NewLine + string.Format("Muertos    {0:N0}", muertos);
                    MessageBox.Show(sal, "Importar individuos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    poblacion_creada = true;
                    b_crea_poblacion.ForeColor = Color.Black;
                    if (individuos.Count < MAX_INDIVIDUOS_PARA_TABLA)
                    {
                        if (!TablaVecinos(true))
                        {
                            tabla_vecinos_creada = false;
                            TABLA_VECINOS = false;
                            tabla_vecinos_en_uso.Text = string.Empty;
                            tabla_vecinos_en_uso.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        tabla_vecinos_creada = false;
                        TABLA_VECINOS = false;
                        tabla_vecinos_en_uso.Text = string.Empty;
                        tabla_vecinos_en_uso.BackColor = Color.Red;
                    }
                }
                Desactiva(false);
            }
        }
        private void B_exporta_individuos_Click(object sender, EventArgs e)
        {
            if (individuos.Count == 0)
            {
                MessageBox.Show("No hay individuos que exportar", "Exportar individuos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Desactiva(true);
            ExportarPoblacion(string.Empty);
            MessageBox.Show(string.Format("Exportados {0:N0} individuos", individuos.Count), "Exportar individuos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Desactiva(false);
        }
        private void ExportarPoblacion(string fichero)
        {
            if (string.IsNullOrEmpty(fichero))
            {
                SaveFileDialog ficheroescritura = new SaveFileDialog()
                {
                    Filter = "III (*.iii)|*.iii|All files (*.*)|*.*",
                    FilterIndex = 1
                };
                ficheroescritura.RestoreDirectory = ficheroescritura.OverwritePrompt = false;
                ficheroescritura.CheckPathExists = true;
                if (ficheroescritura.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                fichero = ficheroescritura.FileName;
            }
            FileStream fw = new FileStream(fichero, FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(fw);

            // Primero los clusters

            sw.WriteLine(string.Format("{0}", NUMERO_CLUSTERS));
            for (int i = 0; i < NUMERO_CLUSTERS; i++)
            {
                sw.WriteLine(string.Format("{0};{1}", grupo_cluster[i], n_individuos_cluster[i]));
                for (int j = 0; j < n_individuos_cluster[i]; j++)
                {
                    sw.WriteLine(string.Format("{0}", individuos_cluster_id[i, j]));
                }
            }

            // Luego los individuos

            foreach (Individuo p in individuos)
            {
                sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", p.grupo_ID, p.grupo, p.xi, p.yi, p.estado, p.dias_infectado, p.dias_curado, p.enfermados, p.cluster, p.contagio_en_cluster ? 1 : 0));
            }
            sw.Close();
        }
        #endregion
        private void Puntos_gordos_CheckedChanged(object sender, EventArgs e)
        {
            if (lzo_ex != null) lzo_ex.Grafico(true);
        }
        private void Mostrar_recorridos_CheckedChanged(object sender, EventArgs e)
        {
            if (lzo_ex != null) lzo_ex.Grafico(true);
        }
    }
}
