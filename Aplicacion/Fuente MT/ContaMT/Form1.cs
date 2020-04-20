using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Contagio
{
    public partial class Form1 : Form
    {
        private readonly char[] ESPECIAL = { ':', '/', '\\' };
        private readonly string CARPETA_SALIDAS = "Contagio";
        private string CARPETA_CASO;
        private string U_SALIDAS;
        private string CARPETA;
        private int NUM_HILOS;
        private int SEMILLA_POBLACION;
        private int SEMILLA_SIMULACION;
        private string F_CASO;
        private string F_CONDICIONES;
        private string F_GRUPOS;
        private string F_INDIVIDUOS;
        private long INDIVIDUOS;
        private int RADIO;
        private double PROB_CONTAGIO;
        private double PROB_CONTAGIO_C;
        private double PROB_CONTAGIO_Cd2;
        private double PROB_RECONTAGIO;
        private int MIN_INMUNIDAD = 0;
        private int CARENCIA_CONTAGIAR;
        private int DIAS_INCUBACION;
        private double PROB_ENFERMAR;
        private double F_PROB_ENFERMO;
        private double PROB_HOSPITALIZACION;
        private double F_PROB_HOSPITALIZADO;
        private double DENSIDAD_VECINOS;
        private double PROB_INDIRECTA;
        private int N_FOCOS_INICIALES;
        private int N_FOCOS_IMPORTADOS;
        private int DIAS_FOCOS_IMPORTADOS;
        private int[] GRUPO_IMPORTADO;
        private int N_INMUNES_INICIALES;
        private int CONTACTO;
        private int CONTACTO2;
        private int LON_10PASOS;
        private double LON_PASO;
        private double LON_PASO2;
        private int NUM_DIAS_EXPORTAR;
        private int[] DIAS_EXPORTAR;
        private int[] PROX_DIA_EXPORTAR;
        private int[] NUMERO_CLUSTERS_DATO;
        private int[] NUMERO_CLUSTERS;
        private int DIAS;
        private double POTENCIA;
        private const int MAX_CONVALECENCIA = 200;
        private double[,] PROB_FIN_INFECCION;   // grupo, dias

        private const int MAX_GRUPOS = 512;
        private long[,] total_pasos_g;
        private long[,] total_recorridos_g;
        private double[,] total_distancias_ida_g;
        private long[,] total_contactos_de_riesgo_g;
        private long[,] total_dias_infectados_curados_g;
        private long[,] total_dias_infectados_muertos_g;

        private const int MAX_INDIVIDUOS_CLUSTER = 1000;
        private double FACTOR_INDIVIDUOS_CLUSTER;
        private List<Individuo>[,] individuos_cluster;
        private long[,] intentos_cluster;
        private long[,] infectados_cluster;
        private long[] total_individuos_clusters;
        private long[] infectados_iniciales_clusters;
        private long[] contagios_clusters;
        private long[] infectados_total_clusters;
        private long[] curados_total_clusters;
        private long[] muertos_total_clusters;

        private const int MAX_TIPOS = 32;
        private int NUM_TIPOS;
        private long[,] sanos;
        private long[,] desinmunizados;
        private long[,] infectados;
        private long[,] enfermos;
        private long[,] hospitalizados;
        private long[,] importados;
        private long[,] curados;
        private long[,] muertos;
        private long[,] infectados_indirectos;
        private long[] acumulador_infectados;
        private long[] contagios_cercania;

        private long[,] infectados_graves;
        private long[,] curados_graves;
        private long[,] muertos_graves;

        private int[] max_pasos_dia;
        private long[] total_pasos;
        private long[] total_recorridos;
        private double[] total_distancias_ida;
        private long[] total_contactos_de_riesgo;
        private long[] total_dias_infectados_curados;
        private long[] total_dias_infectados_muertos;

        private long[,] n_sorteos_fin_infeccion;
        private long[,] n_fin_infeccion;
        private long[,,] res_fin_infeccion;
        private long[,] infectados_grupo;
        private bool cancelar;
        /*
         * y = a * x ^ p entre 0 y n
         * a = 1 / (n ^ p)
         */
        private bool ignorar_cambio;
        private class Condicion
        {
            public int ID;
            public int dias;
            public string f_grupos;
            public int clusters;
            public Condicion(int ID, int dias, string f_grupos, int clusters)
            {
                this.ID = ID;
                this.dias = dias;
                this.f_grupos = f_grupos;
                this.clusters = clusters;
            }
        }
        private readonly List<Condicion> condiciones = new List<Condicion>();
        private int[] prox_condiciones;
        private int[] cont_condiciones;

        private class Grupo
        {
            public int ID;
            public string grupo;
            public int tipo;
            public int edad_min;
            public int edad_max;
            public double gravedad;
            public double prob_contagio;
            public int duracion_infeccion;
            public double prob_curacion;
            public int pasos_min;
            public int pasos_max;
            public double fraccion_poblacion;
            public double fraccion_clusters;
            public int individuos_c;
            public Grupo(int ID, string grupo, int tipo, int edad_min, int edad_max, double gravedad, double prob_contagio, int duracion_infeccion, double prob_curacion, int pasos_min, int pasos_max, double fraccion_poblacion, double fraccion_clusters, int individuos_c)
            {
                this.ID = ID;
                this.grupo = grupo;
                this.tipo = tipo;
                this.edad_min = edad_min;
                this.edad_max = edad_max;
                this.gravedad = gravedad;
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
        private List<Grupo> grupos;
        private List<Grupo>[] grupos_hilo;
        private long[,] indi_grupos;
        private class Individuo
        {
            public int ID;
            public int tipo;
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
            public int indi_enfermados;
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
            public bool enfermo;
            public bool hospitalizado;
            public bool grave;
            public Individuo(int ID, int tipo, int grupo_ID, string grupo, double xi, double yi, int estado)
            {
                this.ID = ID;
                this.tipo = tipo;
                this.grupo_ID = grupo_ID;
                this.grupo = grupo;
                this.xi = xi;
                this.yi = yi;
                this.estado = estado;
                dias_infectado = 0;
                dias_curado = 0;
                indi_enfermados = 0;
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
                enfermo = false;
                hospitalizado = false;
            }
            public Individuo(int ID, int tipo, int grupo_ID, string grupo, double xi, double yi, int estado, int dias_infectado, int dias_curado, int indi_enfermados, int cluster, bool contagio_en_cluster, bool enfermo, bool hospitalizado, bool grave)
            {
                this.ID = ID;
                this.tipo = tipo;
                this.grupo_ID = grupo_ID;
                this.grupo = grupo;
                this.xi = xi;
                this.yi = yi;
                this.estado = estado;
                this.dias_infectado = dias_infectado;
                this.dias_curado = dias_curado;
                this.indi_enfermados = indi_enfermados;
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
                this.enfermo = enfermo;
                this.hospitalizado = hospitalizado;
                this.grave = grave;
            }
        }
        private List<Individuo>[] individuos;
        private List<Individuo> individuos_base;
        private class Resultado
        {
            public int r_dia;
            public long r_sanos;
            public long r_infectados;
            public long r_importados;
            public long r_curados;
            public long r_desinmunizados;
            public long r_muertos;
            public double r_recorrido_individuo_activo;
            public double r_contactos_de_riesgo;
            public double r_media_dias_infectado;
            public double r_R0;
            public long r_contagios_cercania;
            public long r_contagios_clusters;
            public long r_infectados_total_clusters;
            public long r_curados_total_clusters;
            public long r_muertos_total_clusters;
            public long r_infectados_indirectos;
            public long r_enfermos;
            public long r_hospitalizados;
            public long r_infectados_graves;
            public long r_curados_graves;
            public long r_muertos_graves;
            public double r_R0_graves;
            public Resultado(
                int dia,
                long sanos,
                long infectados,
                long importados,
                long curados,
                long desinmunizados,
                long muertos,
                double recorrido_individuo_activo,
                double contactos_de_riesgo,
                double media_dias_infectado,
                double R0,
                long contagios_cercania,
                long contagios_clusters,
                long infectados_total_clusters,
                long curados_total_clusters,
                long muertos_total_clusters,
                long infectados_indirectos,
                long enfermos,
                long hospitalizados,
                long infectados_graves,
                long curados_graves,
                long muertos_graves,
                double R0_graves)
            {
                r_dia = dia;
                r_sanos = sanos;
                r_infectados = infectados;
                r_importados = importados;
                r_curados = curados;
                r_desinmunizados = desinmunizados;
                r_muertos = muertos;
                r_recorrido_individuo_activo = recorrido_individuo_activo;
                r_contactos_de_riesgo = contactos_de_riesgo;
                r_media_dias_infectado = media_dias_infectado;
                r_R0 = R0;
                r_contagios_cercania = contagios_cercania;
                r_contagios_clusters = contagios_clusters;
                r_infectados_total_clusters = infectados_total_clusters;
                r_curados_total_clusters = curados_total_clusters;
                r_muertos_total_clusters = muertos_total_clusters;
                r_infectados_indirectos = infectados_indirectos;
                r_enfermos = enfermos;
                r_hospitalizados = hospitalizados;
                r_infectados_graves = infectados_graves;
                r_curados_graves = curados_graves;
                r_muertos_graves = muertos_graves;
                r_R0_graves = R0_graves;
            }
        }
        private List<Resultado>[] resultados;
        private class ResTipo
        {
            public long r_sanos;
            public long r_infectados;
            public long r_importados;
            public long r_curados;
            public long r_desinmunizados;
            public long r_muertos;
            public long r_infectados_indirectos;
            public long r_enfermos;
            public long r_hospitalizados;
            public ResTipo(
                long sanos,
                long infectados,
                long importados,
                long curados,
                long desinmunizados,
                long muertos,
                long infectados_indirectos,
                long enfermos,
                long hospitalizados)
            {
                r_sanos = sanos;
                r_infectados = infectados;
                r_importados = importados;
                r_curados = curados;
                r_desinmunizados = desinmunizados;
                r_muertos = muertos;
                r_infectados_indirectos = infectados_indirectos;
                r_enfermos = enfermos;
                r_hospitalizados = hospitalizados;
            }
        }
        private List<ResTipo>[,] restipos;
        private const int MAX_INDIVIDUOS_PARA_TABLA = 100001;
        private const int MAX_VECINOS_PARA_TABLA = 401;
        private int[,] nvecinas;
        private int[,,] vecinas;

        private readonly object sync = new object();
        private DateTime tiempo_inicio;
        private Thread[] hiloSimulacion;
        private int[] hiloDia;
        private Random[] hiloAzar;
        /*
         *  0 Activo
         *  1 Terminado
         */
        private int[] hiloEstado;
        private delegate void DelegadoActualizaEstadisticas(int hilo, int dia);
        private DelegadoActualizaEstadisticas delegadoActualizaEstadisticas = null;
        private delegate void DelegadoFinHilo();
        private DelegadoFinHilo delegadoFinHilo = null;
        private delegate void DelegadoMensaje(int i, string s);
        private DelegadoMensaje delegadoMensaje = null;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.Manual;
            Left = 0;
            Top = 0;
            Text = string.Format("Simulador de contagios [Paralelo] {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Desactiva(true);
            b_cancelar.Enabled = false;
            Show();
            linea_estado.Text = "Espera por favor ...";
            Oculta(true);
            Application.DoEvents();
            IniciaTablaCondiciones(tablaCondiciones);
            IniciaTablaGrupos(tablaGrupos);
            U_SALIDAS = Properties.Settings.Default.u_salidas;
            if (string.IsNullOrEmpty(U_SALIDAS))
            {
                U_SALIDAS = "C";
            }
            selunidad.SelectedItem = U_SALIDAS;
            senda_caso.Text = F_CASO = string.Empty;
            CARPETA_CASO = string.Empty;
            senda_condiciones.Text = F_CONDICIONES = string.Empty;
            senda_individuos.Text = F_INDIVIDUOS = string.Empty;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.ftecaso))
            {
                try
                {
                    senda_caso.Text = F_CASO = Properties.Settings.Default.ftecaso;
                    CARPETA_CASO = Path.GetDirectoryName(F_CASO);
                    Application.DoEvents();
                    if (LeeCaso(F_CASO))
                    {
                        LeeTodo();
                    }
                    string s = string.Format(@"{0}:\{1}", U_SALIDAS, CARPETA_SALIDAS);
                    if (!Directory.Exists(s)) Directory.CreateDirectory(s);
                }
                catch { }
            }
            b_cancelar.Enabled = false;
            Oculta(false);
            Desactiva(false);
            linea_estado.Text = string.Empty;
            delegadoActualizaEstadisticas = new DelegadoActualizaEstadisticas(ActualizaEstadisticas);
            delegadoFinHilo = new DelegadoFinHilo(FinHilo);
            delegadoMensaje = new DelegadoMensaje(MensajeHilo);
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
            sel_condiciones.Enabled = !que;
            selunidad.Enabled = !que;
            b_salva_grupos.Enabled = !que;
            b_simula.Enabled = !que;
            d_individuos.Enabled = !que;
            b_salva_caso.Enabled = !que;
            d_dias.Enabled = !que;
            d_lon_paso.Enabled = !que;
            d_contacto.Enabled = !que;
            d_potencia.Enabled = !que;
            d_contagio.Enabled = !que;
            d_recontagio.Enabled = !que;
            d_mininmunidad.Enabled = !que;
            d_carencia.Enabled = !que;
            d_incubacion.Enabled = !que;
            d_enfermar.Enabled = !que;
            d_fpc_enfermo.Enabled = !que;
            d_hospitalizacion.Enabled = !que;
            d_densidad.Enabled = !que;
            d_indirecta.Enabled = !que;
            d_fpc_hospitalizado.Enabled = !que;
            sel_individuos.Enabled = !que;
            d_focos.Enabled = !que;
            d_inmunes.Enabled = !que;
            d_importados.Enabled = !que;
            d_dias_importados.Enabled = !que;
            d_grupo_importado.Enabled = !que;
            d_contagio_c.Enabled = !que;
            d_individuos_c.Enabled = !que;
            d_dias_exportar.Enabled = !que;
            semilla_poblacion.Enabled = !que;
            semilla_simulacion.Enabled = !que;
            d_hilos.Enabled = !que;
            tablaCondiciones.Enabled = !que;
            b_condicion_mas.Enabled = !que;
            b_condicion_menos.Enabled = !que;
            b_salva_condiciones.Enabled = !que;
            b_sube.Enabled = !que;
            b_baja.Enabled = !que;
        }
        private void Oculta(bool que)
        {
            prefijo.Visible = !que;
            d_radio.Visible = !que;
            tablaGrupos.Visible = !que;
            sel_caso.Visible = !que;
            sel_condiciones.Visible = !que;
            selunidad.Visible = !que;
            b_salva_grupos.Visible = !que;
            b_simula.Visible = !que;
            d_individuos.Visible = !que;
            b_salva_caso.Visible = !que;
            d_dias.Visible = !que;
            d_lon_paso.Visible = !que;
            d_contacto.Visible = !que;
            d_potencia.Visible = !que;
            d_contagio.Visible = !que;
            d_recontagio.Visible = !que;
            d_mininmunidad.Visible = !que;
            d_incubacion.Visible = !que;
            d_enfermar.Visible = !que;
            d_fpc_enfermo.Visible = !que;
            d_hospitalizacion.Visible = !que;
            d_fpc_hospitalizado.Visible = !que;
            d_densidad.Enabled = !que;
            d_indirecta.Enabled = !que;
            d_carencia.Visible = !que;
            sel_individuos.Visible = !que;
            d_focos.Visible = !que;
            d_inmunes.Visible = !que;
            d_importados.Visible = !que;
            d_dias_importados.Visible = !que;
            d_grupo_importado.Visible = !que;
            d_contagio_c.Visible = !que;
            d_individuos_c.Visible = !que;
            d_dias_exportar.Visible = !que;
            semilla_poblacion.Visible = !que;
            semilla_simulacion.Visible = !que;
            d_hilos.Visible = !que;
            tablaCondiciones.Visible = !que;
            b_condicion_mas.Visible = !que;
            b_condicion_menos.Visible = !que;
            b_salva_condiciones.Visible = !que;
            b_sube.Visible = !que;
            b_baja.Visible = !que;
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
                F_CASO = senda_caso.Text;
                if (string.IsNullOrEmpty(F_CASO))
                {
                    CARPETA_CASO = string.Empty;
                }
                else
                {
                    CARPETA_CASO = Path.GetDirectoryName(F_CASO);
                }
            }
        }
        private bool LeeCaso(string fichero)
        {
            if (!File.Exists(fichero))
            {
                MessageBox.Show("No se encuentra el fichero " + fichero, "Lectura de caso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            FileStream fr = new FileStream(fichero, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fr);
            ignorar_cambio = true;
            try
            {
                CARPETA_CASO = Path.GetDirectoryName(fichero);
                U_SALIDAS = sr.ReadLine();
                if (string.IsNullOrEmpty(U_SALIDAS))
                {
                    U_SALIDAS = "C";
                }
                selunidad.SelectedItem = U_SALIDAS;
                prefijo.Text = CARPETA = sr.ReadLine();
                d_hilos.Text = sr.ReadLine();
                semilla_poblacion.Text = sr.ReadLine();
                semilla_simulacion.Text = sr.ReadLine();
                F_CONDICIONES = sr.ReadLine();
                if (F_CONDICIONES.Length > 0 && F_CONDICIONES.IndexOfAny(ESPECIAL) == -1)
                {
                    // Senda relativa

                    F_CONDICIONES = Path.Combine(CARPETA_CASO, F_CONDICIONES);
                }
                senda_condiciones.Text = F_CONDICIONES;
                F_INDIVIDUOS = sr.ReadLine();
                if (F_INDIVIDUOS.Length > 0)
                {
                    if (F_INDIVIDUOS.IndexOfAny(ESPECIAL) == -1)
                    {
                        // Senda relativa

                        F_INDIVIDUOS = Path.Combine(CARPETA_CASO, F_INDIVIDUOS);
                    }
                }
                senda_individuos.Text = F_INDIVIDUOS;
                d_dias_exportar.Text = sr.ReadLine();
                d_individuos.Text = sr.ReadLine();
                d_radio.Text = sr.ReadLine();
                d_potencia.Text = sr.ReadLine();
                d_contagio.Text = sr.ReadLine();
                d_recontagio.Text = sr.ReadLine();
                d_mininmunidad.Text = sr.ReadLine();
                d_carencia.Text = sr.ReadLine();
                d_incubacion.Text = sr.ReadLine();
                d_enfermar.Text = sr.ReadLine();
                d_fpc_enfermo.Text = sr.ReadLine();
                d_hospitalizacion.Text = sr.ReadLine();
                d_fpc_hospitalizado.Text = sr.ReadLine();
                d_densidad.Text = sr.ReadLine();
                d_indirecta.Text = sr.ReadLine();
                d_focos.Text = sr.ReadLine();
                d_inmunes.Text = sr.ReadLine();
                d_importados.Text = sr.ReadLine();
                d_dias_importados.Text = sr.ReadLine();
                d_grupo_importado.Text = sr.ReadLine();
                d_contacto.Text = sr.ReadLine();
                d_lon_paso.Text = sr.ReadLine();
                d_dias.Text = sr.ReadLine();
                d_contagio_c.Text = sr.ReadLine();
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
            try
            {
                if (string.IsNullOrEmpty(F_CONDICIONES) || !LeeCondiciones(F_CONDICIONES))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Leer caso", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    sw.WriteLine(d_hilos.Text);
                    sw.WriteLine(semilla_poblacion.Text);
                    sw.WriteLine(semilla_simulacion.Text);
                    string senda;
                    if (senda_condiciones.Text.Length == 0)
                    {
                        sw.WriteLine();
                    }
                    else
                    {
                        senda = Path.GetDirectoryName(senda_condiciones.Text);
                        if (CARPETA_CASO.Equals(senda, StringComparison.OrdinalIgnoreCase))
                        {
                            // Senda relativa

                            sw.WriteLine(Path.GetFileName(F_CONDICIONES));
                        }
                        else
                        {
                            sw.WriteLine(senda_condiciones.Text);
                        }
                    }
                    if (senda_individuos.Text.Length == 0)
                    {
                        sw.WriteLine();
                    }
                    else
                    {
                        senda = Path.GetDirectoryName(senda_individuos.Text);
                        if (CARPETA_CASO.Equals(senda, StringComparison.OrdinalIgnoreCase))
                        {
                            // Senda relativa

                            sw.WriteLine(Path.GetFileName(F_INDIVIDUOS));
                        }
                        else
                        {
                            sw.WriteLine(senda_individuos.Text);
                        }
                    }
                    sw.WriteLine(d_dias_exportar.Text);
                    sw.WriteLine(d_individuos.Text);
                    sw.WriteLine(d_radio.Text);
                    sw.WriteLine(d_potencia.Text);
                    sw.WriteLine(d_contagio.Text);
                    sw.WriteLine(d_recontagio.Text);
                    sw.WriteLine(d_mininmunidad.Text);
                    sw.WriteLine(d_carencia.Text);
                    sw.WriteLine(d_incubacion.Text);
                    sw.WriteLine(d_enfermar.Text);
                    sw.WriteLine(d_fpc_enfermo.Text);
                    sw.WriteLine(d_hospitalizacion.Text);
                    sw.WriteLine(d_fpc_hospitalizado.Text);
                    sw.WriteLine(d_densidad.Text);
                    sw.WriteLine(d_indirecta.Text);
                    sw.WriteLine(d_focos.Text);
                    sw.WriteLine(d_inmunes.Text);
                    sw.WriteLine(d_importados.Text);
                    sw.WriteLine(d_dias_importados.Text);
                    sw.WriteLine(d_grupo_importado.Text);
                    sw.WriteLine(d_contacto.Text);
                    sw.WriteLine(d_lon_paso.Text);
                    sw.WriteLine(d_dias.Text);
                    sw.WriteLine(d_contagio_c.Text);
                    sw.WriteLine(d_individuos_c.Text);
                    sw.Close();
                    b_salva_caso.ForeColor = Color.Black;
                    ignorar_cambio = false;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Actualizar caso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ignorar_cambio = false;
                    return false;
                }
                return true;
            }
            return false;
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
        private bool Parametros()
        {
            try
            {
                U_SALIDAS = selunidad.SelectedItem.ToString();
                if (string.IsNullOrEmpty(U_SALIDAS))
                {
                    U_SALIDAS = "C";
                }
                else
                {
                    try
                    {
                        string s = string.Format(@"{0}:\{1}", U_SALIDAS, CARPETA_SALIDAS);
                        if (!Directory.Exists(s)) Directory.CreateDirectory(s);
                    }
                    catch
                    {
                        U_SALIDAS = "C";
                    }
                }
                selunidad.SelectedItem = U_SALIDAS;
                CARPETA = prefijo.Text;
                NUM_HILOS = Convert.ToInt32(d_hilos.Text);
                SEMILLA_POBLACION = string.IsNullOrEmpty(semilla_poblacion.Text.Trim()) ? 0 : Convert.ToInt32(semilla_poblacion.Text.Trim());
                SEMILLA_SIMULACION = string.IsNullOrEmpty(semilla_simulacion.Text.Trim()) ? 0 : Convert.ToInt32(semilla_simulacion.Text.Trim());
                F_GRUPOS = senda_condiciones.Text;
                F_INDIVIDUOS = senda_individuos.Text;
                INDIVIDUOS = Convert.ToInt64(d_individuos.Text);
                RADIO = Convert.ToInt32(d_radio.Text);
                POTENCIA = Convert.ToDouble(d_potencia.Text.Replace('.', ','));
                PROB_CONTAGIO = Convert.ToDouble(d_contagio.Text.Replace('.', ',')) / 100.0;
                PROB_RECONTAGIO = Convert.ToDouble(d_recontagio.Text.Replace('.', ',')) / 100.0;
                MIN_INMUNIDAD = Convert.ToInt32(d_mininmunidad.Text);
                CARENCIA_CONTAGIAR = Convert.ToInt32(d_carencia.Text);
                DIAS_INCUBACION = Convert.ToInt32(d_incubacion.Text);
                PROB_ENFERMAR = Convert.ToInt32(d_enfermar.Text) / 100.0;
                F_PROB_ENFERMO = Convert.ToDouble(d_fpc_enfermo.Text) / 100.0;
                PROB_HOSPITALIZACION = Convert.ToDouble(d_hospitalizacion.Text) / 100.0;
                F_PROB_HOSPITALIZADO = Convert.ToDouble(d_fpc_hospitalizado.Text) / 100.0;
                DENSIDAD_VECINOS = Convert.ToDouble(d_densidad.Text) / 100.0;
                PROB_INDIRECTA = Convert.ToDouble(d_indirecta.Text) / 100.0;
                N_FOCOS_INICIALES = Convert.ToInt32(d_focos.Text);
                N_INMUNES_INICIALES = Convert.ToInt32(d_inmunes.Text);
                N_FOCOS_IMPORTADOS = Convert.ToInt32(d_importados.Text);
                DIAS_FOCOS_IMPORTADOS = Convert.ToInt32(d_dias_importados.Text);
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
                FACTOR_INDIVIDUOS_CLUSTER = Convert.ToDouble(d_individuos_c.Text.Replace('.', ','));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Datos de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        #endregion

        #region Condiciones
        private void IniciaTablaCondiciones(DataGridView tabla)
        {
            tabla.BackgroundColor = Color.LightGray;
            tabla.BorderStyle = BorderStyle.Fixed3D;
            tabla.ReadOnly = false;
            tabla.MultiSelect = false;
            tabla.AllowUserToAddRows = false;
            tabla.AllowUserToDeleteRows = false;
            tabla.AllowUserToOrderColumns = false;
            tabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            tabla.AllowUserToResizeRows = false;
            tabla.AllowUserToResizeColumns = false;
            tabla.ColumnHeadersVisible = true;
            tabla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            tabla.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            tabla.DefaultCellStyle.SelectionBackColor = Color.LightSalmon;
            tabla.RowHeadersVisible = false;
            int tabla_alto_fila = tabla.RowTemplate.Height;
            int tabla_alto_cabecera = tabla.ColumnHeadersHeight;
            if (tabla_alto_cabecera <= tabla_alto_fila) tabla_alto_cabecera = tabla_alto_fila + 1;
            tabla.RowTemplate.Height = tabla_alto_fila;
            tabla.ColumnHeadersHeight = tabla_alto_cabecera;
            tabla.ColumnCount = 3;
            int j = 0;
            tabla.Columns[j].FillWeight = 10;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].Name = "Días";
            tabla.Columns[j].SortMode = DataGridViewColumnSortMode.Automatic;
            j++;
            tabla.Columns[j].FillWeight = 80;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            tabla.Columns[j].Name = "Fichero grupos";
            j++;
            tabla.Columns[j].FillWeight = 10;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].DefaultCellStyle.BackColor = Color.Beige;
            tabla.Columns[j].Name = "Clusters";
        }
        private void Sel_condiciones_Click(object sender, EventArgs e)
        {
            Sel_Fichero_Condiciones();
        }
        private void Sel_Fichero_Condiciones()
        {
            OpenFileDialog leefichero = new OpenFileDialog
            {
                Filter = "RRR (*.rrr)|*.rrr|All files (*.*)|*.*",
                CheckFileExists = false,
                Multiselect = false
            };
            if (leefichero.ShowDialog() == DialogResult.OK)
            {
                string fo = leefichero.FileName;
                if (!File.Exists(fo))
                {
                    // Nuevo fichero de restricciones

                    condiciones.Clear();
                    MuestraCondiciones();
                    F_CONDICIONES = fo;
                    SalvaCondiciones(F_CONDICIONES);
                }
                if (LeeCondiciones(fo))
                {
                    F_CONDICIONES = fo;
                }
                else
                {
                    F_CONDICIONES = string.Empty;
                }
                senda_condiciones.Text = F_CONDICIONES;
                b_salva_caso.ForeColor = Color.Red;
            }
        }
        private bool LeeCondiciones(string fichero)
        {
            if (!File.Exists(fichero))
            {
                MessageBox.Show("No se encuentra el fichero " + fichero, "Lectura de condiciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            condiciones.Clear();
            FileStream fr = new FileStream(fichero, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fr);
            string s;
            string[] sd;
            int dias;
            string f_grupos;
            int clusters;
            while (!sr.EndOfStream)
            {
                s = sr.ReadLine();
                sd = s.Split(';');
                if (sd.Length != 3)
                {
                    MessageBox.Show(string.Format("Número incorrecto de campos: {0} {1}", sd.Length, s), "Lectura de condiciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (sr != null) sr.Close();
                    return false;
                }
                try
                {
                    dias = Convert.ToInt32(sd[0]);
                    f_grupos = sd[1];
                    clusters = Convert.ToInt32(sd[2]);
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("<{0}> <{1}> <{2}> Mensaje: {3}", sd[0], sd[1], sd[2], e.Message), "Lectura de grupos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (sr != null) sr.Close();
                    return false;
                }
                condiciones.Add(new Condicion(condiciones.Count, dias, f_grupos, clusters));
            }
            sr.Close();
            MuestraCondiciones();
            return true;
        }
        private bool SalvaCondiciones(string fichero)
        {
            try
            {
                FileStream fw = new FileStream(fichero, FileMode.Create, FileAccess.Write, FileShare.Read);
                StreamWriter sw = new StreamWriter(fw);
                for (int i = 0; i < tablaCondiciones.RowCount; i++)
                {
                    DataGridViewRow fila = tablaCondiciones.Rows[i];
                    if (fila.Cells[0].Value == null || String.IsNullOrEmpty(fila.Cells[0].Value.ToString())) continue;
                    if (fila.Cells[1].Value == null) fila.Cells[1].Value = "0";
                    if (fila.Cells[2].Value == null) fila.Cells[2].Value = "0";
                    sw.WriteLine(string.Format("{0};{1};{2}", fila.Cells[0].Value.ToString(), fila.Cells[1].Value.ToString(), fila.Cells[2].Value.ToString()));
                }
                sw.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("{0}", e.Message), "Actualizar condiciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void B_salva_condiciones_Click(object sender, EventArgs e)
        {
            string fichero = FicheroEscritura(F_CONDICIONES, "RRR (*.rrr)|*.rrr|All files (*.*)|*.*");
            if (!string.IsNullOrEmpty(fichero))
            {
                if (SalvaCondiciones(fichero))
                {
                    if (!fichero.Equals(F_CONDICIONES, StringComparison.OrdinalIgnoreCase))
                    {
                        senda_condiciones.Text = F_CONDICIONES = fichero;
                    }
                    b_salva_condiciones.ForeColor = Color.Black;
                    b_salva_caso.ForeColor = Color.Red;
                    MessageBox.Show("OK", "Actualizar condiciones", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void MuestraCondiciones()
        {
            tablaGrupos.RowCount = 1;
            tablaCondiciones.RowCount = 0;
            object[] fila = new object[3];
            foreach (Condicion c in condiciones)
            {
                fila[0] = c.dias;
                fila[1] = c.f_grupos;
                fila[2] = c.clusters;
                tablaCondiciones.Rows.Add(fila);
            }
        }
        private bool ActualizaCondiciones()
        {
            string[] sd = new string[tablaCondiciones.ColumnCount];
            int dias;
            string f_grupos;
            int clusters;
            condiciones.Clear();
            DataGridViewRow fila;
            for (int i = 0; i < tablaCondiciones.RowCount; i++)
            {
                fila = tablaCondiciones.Rows[i];

                // Evitar la última fila que está vacía

                if (fila.Cells[0].Value == null || String.IsNullOrEmpty(fila.Cells[0].Value.ToString())) continue;
                for (int j = 0; j < tablaCondiciones.ColumnCount; j++)
                {
                    sd[j] = fila.Cells[j].Value.ToString();
                }
                try
                {
                    dias = Convert.ToInt32(sd[0]);
                    f_grupos = sd[1];
                    clusters = Convert.ToInt32(sd[2]);
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("<{0}> <{1}> <{2}> Mensaje: {3}", sd[0], sd[1], sd[2], e.Message), "Actualizar condiciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                condiciones.Add(new Condicion(condiciones.Count, dias, f_grupos, clusters));
            }
            return true;
        }
        private void B_condicion_mas_Click(object sender, EventArgs e)
        {
            if (Sel_Fichero_Grupos())
            {
                ActualizaCondiciones();
                condiciones.Add(new Condicion(condiciones.Count, 0, F_GRUPOS, -1));
                MuestraCondiciones();
                b_salva_condiciones.ForeColor = Color.Red;
            }
        }
        private void B_condicion_menos_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirmar", "Eliminar condición", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ActualizaCondiciones();
                condiciones.RemoveAt(tablaCondiciones.SelectedRows[0].Index);
                MuestraCondiciones();
                b_salva_condiciones.ForeColor = Color.Red;
            }
        }
        private void TablaCondiciones_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow fila = tablaCondiciones.Rows[e.RowIndex];
            if (fila.Cells[1].Value == null) return;
            F_GRUPOS = fila.Cells[1].Value.ToString();
            grupos = LeeGrupos(F_GRUPOS);
            MuestraGrupos();
        }
        private void TablaCondiciones_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            b_salva_condiciones.ForeColor = Color.Red;
        }
        private void B_sube_Click(object sender, EventArgs e)
        {
            if (tablaCondiciones.SelectedRows != null && tablaCondiciones.SelectedRows[0].Index > 0)
            {
                int i = tablaCondiciones.SelectedRows[0].Index;
                DataGridViewRow fila1 = tablaCondiciones.Rows[i];
                tablaCondiciones.Rows.RemoveAt(i);
                tablaCondiciones.Rows.Insert(i - 1, fila1);
                tablaCondiciones.Rows[i - 1].Selected = true;
                Condicion c = condiciones.ElementAt(i);
                condiciones.RemoveAt(i);
                condiciones.Insert(i - 1, c);
            }
        }
        private void B_baja_Click(object sender, EventArgs e)
        {
            if (tablaCondiciones.SelectedRows != null && tablaCondiciones.SelectedRows[0].Index < tablaCondiciones.Rows.Count - 1)
            {
                int i = tablaCondiciones.SelectedRows[0].Index;
                DataGridViewRow fila1 = tablaCondiciones.Rows[i];
                tablaCondiciones.Rows.RemoveAt(i);
                tablaCondiciones.Rows.Insert(i + 1, fila1);
                tablaCondiciones.Rows[i + 1].Selected = true;
                Condicion c = condiciones.ElementAt(i);
                condiciones.RemoveAt(i);
                condiciones.Insert(i + 1, c);
            }
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
            tabla.DefaultCellStyle.SelectionBackColor = Color.LightSalmon;
            tabla.RowHeadersVisible = false;
            int tabla_alto_fila = tabla.RowTemplate.Height;
            int tabla_alto_cabecera = tabla.ColumnHeadersHeight;
            tabla.RowTemplate.Height = tabla_alto_fila;
            tabla.ColumnHeadersHeight = tabla_alto_cabecera;
            tabla.ColumnCount = 13;
            int j = 0;
            tabla.Columns[j].FillWeight = 30;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            tabla.Columns[j].Name = "Nombre";
            tabla.Columns[j].SortMode = DataGridViewColumnSortMode.Automatic;
            j++;
            tabla.Columns[j].FillWeight = 14;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].Name = "Tipo";
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
            tabla.Columns[j].FillWeight = 18;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].Name = "Grave";
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
            tabla.Columns[j].FillWeight = 18;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].DefaultCellStyle.BackColor = Color.Beige;
            tabla.Columns[j].Name = "PasosMin";
            j++;
            tabla.Columns[j].FillWeight = 18;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].DefaultCellStyle.BackColor = Color.Beige;
            tabla.Columns[j].Name = "PasosMax";
            j++;
            tabla.Columns[j].FillWeight = 16;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].Name = "% Pobl";
            j++;
            tabla.Columns[j].FillWeight = 16;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].DefaultCellStyle.BackColor = Color.Beige;
            tabla.Columns[j].Name = "% Clus";
            j++;
            tabla.Columns[j].FillWeight = 20;
            tabla.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            tabla.Columns[j].DefaultCellStyle.BackColor = Color.Beige;
            tabla.Columns[j].Name = "Ind/Cluster";
        }
        private bool Sel_Fichero_Grupos()
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
                    // Nuevo fichero de grupos

                    grupos = new List<Grupo>();
                    F_GRUPOS = fo;
                    SalvaGrupos(F_GRUPOS);
                }
                if ((grupos = LeeGrupos(fo)) != null)
                {
                    F_GRUPOS = fo;
                }
                else
                {
                    grupos = new List<Grupo>();
                    F_GRUPOS = string.Empty;
                }
                MuestraGrupos();
                return true;
            }
            return false;
        }
        private List<Grupo> LeeGrupos(string F_GRUPOS)
        {
            if (!File.Exists(F_GRUPOS))
            {
                MessageBox.Show("No se encuentra el fichero " + F_GRUPOS, "Lectura de gr_tmp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            List<Grupo> g_temp = new List<Grupo>();
            FileStream fr = new FileStream(F_GRUPOS, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fr);
            string s;
            string[] sd;
            string grupo;
            int tipo;
            int edad_min;
            int edad_max;
            int pasos_min;
            int pasos_max;
            double gravedad;
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
                if (sd.Length != 13)
                {
                    MessageBox.Show(string.Format("Número incorrecto de campos: {0} {1}", sd.Length, s), "Lectura de gr_tmp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (sr != null) sr.Close();
                    return null;
                }
                try
                {
                    grupo = sd[0];
                    tipo = Convert.ToInt32(sd[1]);
                    edad_min = Convert.ToInt32(sd[2]);
                    edad_max = Convert.ToInt32(sd[3]);
                    gravedad = Convert.ToDouble(sd[4]);
                    prob_contagio = Convert.ToDouble(sd[5]);
                    duracion_infeccion = Convert.ToInt32(sd[6]);
                    prob_curacion = Convert.ToDouble(sd[7]);
                    pasos_min = Convert.ToInt32(sd[8]);
                    pasos_max = Convert.ToInt32(sd[9]);
                    fraccion_poblacion = Convert.ToDouble(sd[10]);
                    fraccion_clusters = Convert.ToInt64(sd[11]);
                    individuos_c = Convert.ToInt32(sd[12]);
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("<{0}> <{1}> <{2}> <{3}> <{4}> <{5}> <{6}> <{7}> <{8}> <{9}> <{10}> <{11}> <{12}> Mensaje: {13}", sd[0], sd[1], sd[2], sd[3], sd[4], sd[5], sd[6], sd[7], sd[8], sd[9], sd[10], sd[11], sd[12], e.Message), "Lectura de gr_tmp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (sr != null) sr.Close();
                    return null;
                }
                g_temp.Add(new Grupo(g_temp.Count, grupo, tipo, edad_min, edad_max, gravedad / 100.0, prob_contagio / 100.0, duracion_infeccion, prob_curacion / 100.0, pasos_min, pasos_max, fraccion_poblacion / 100.0, fraccion_clusters / 100.0, individuos_c));
            }
            sr.Close();
            return g_temp;
        }
        private void MuestraGrupos()
        {
            tablaGrupos.RowCount = 1;
            object[] fila = new object[13];
            foreach (Grupo g in grupos)
            {
                fila[0] = g.grupo;
                fila[1] = g.tipo;
                fila[2] = g.edad_min;
                fila[3] = g.edad_max;
                fila[4] = g.gravedad * 100.0;
                fila[5] = g.prob_contagio * 100.0;
                fila[6] = g.duracion_infeccion;
                fila[7] = g.prob_curacion * 100.0;
                fila[8] = g.pasos_min;
                fila[9] = g.pasos_max;
                fila[10] = g.fraccion_poblacion * 100.0;
                fila[11] = g.fraccion_clusters * 100.0;
                fila[12] = g.individuos_c;
                tablaGrupos.Rows.Add(fila);
            }
        }
        private bool ActualizaGrupos()
        {
            string[] sd = new string[tablaGrupos.ColumnCount];
            string grupo;
            int tipo;
            int edad_min;
            int edad_max;
            int pasos_min;
            int pasos_max;
            double gravedad;
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

                // Evitar la última fila que está vacía (para añdir datos)

                if (fila.Cells[0].Value == null || String.IsNullOrEmpty(fila.Cells[0].Value.ToString())) continue;
                for (int j = 0; j < tablaGrupos.ColumnCount; j++)
                {
                    sd[j] = fila.Cells[j].Value.ToString();
                }
                try
                {
                    grupo = sd[0];
                    tipo = Convert.ToInt32(sd[1]);
                    edad_min = Convert.ToInt32(sd[2]);
                    edad_max = Convert.ToInt32(sd[3]);
                    gravedad = Convert.ToDouble(sd[4]);
                    prob_contagio = Convert.ToDouble(sd[5]);
                    duracion_infeccion = Convert.ToInt32(sd[6]);
                    prob_curacion = Convert.ToDouble(sd[7]);
                    pasos_min = Convert.ToInt32(sd[8]);
                    pasos_max = Convert.ToInt32(sd[9]);
                    fraccion_poblacion = Convert.ToDouble(sd[10]);
                    fraccion_clusters = Convert.ToDouble(sd[11]);
                    individuos_c = Convert.ToInt32(sd[12]);
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("<{0}> <{1}> <{2}> <{3}> <{4}> <{5}> <{6}> <{7}> <{8}> <{9}> <{10}> <{11}> <{12}> Mensaje: {13}", sd[0], sd[1], sd[2], sd[3], sd[4], sd[5], sd[6], sd[7], sd[8], sd[9], sd[10], sd[11], sd[12], e.Message), "Actualiza grupos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                grupos.Add(new Grupo(grupos.Count, grupo, tipo, edad_min, edad_max, gravedad / 100.0, prob_contagio / 100.0, duracion_infeccion, prob_curacion / 100.0, pasos_min, pasos_max, fraccion_poblacion / 100.0, fraccion_clusters / 100.0, individuos_c));
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
                        senda_condiciones.Text = F_GRUPOS = fichero;
                        if (tablaCondiciones.SelectedRows != null)
                        {
                            int i = tablaCondiciones.SelectedRows[0].Index;
                            tablaCondiciones.Rows[i].Cells[1].Value = F_GRUPOS;
                            condiciones.ElementAt(i).f_grupos = F_GRUPOS;
                        }
                    }
                    b_salva_grupos.ForeColor = Color.Black;
                    b_salva_condiciones.ForeColor = Color.Red;
                    b_salva_caso.ForeColor = Color.Red;
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
                    if (fila.Cells[12].Value == null) fila.Cells[12].Value = "0";
                    sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}", fila.Cells[0].Value.ToString(), fila.Cells[1].Value.ToString(), fila.Cells[2].Value.ToString(), fila.Cells[3].Value.ToString(), fila.Cells[4].Value.ToString(), fila.Cells[5].Value.ToString(), fila.Cells[6].Value.ToString(), fila.Cells[7].Value.ToString(), fila.Cells[8].Value.ToString(), fila.Cells[9].Value.ToString(), fila.Cells[10].Value.ToString(), fila.Cells[11].Value.ToString(), fila.Cells[12].Value.ToString()));
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
        private void TablaGrupos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            b_salva_grupos.ForeColor = Color.Red;
        }
        #endregion

        #region Poblacion
        private bool IndividuosBase()
        {
            // Con la tabla de grupos de la primera entrada en 'condiciones'

            Condicion c = condiciones.ElementAt(0);
            grupos = LeeGrupos(c.f_grupos);
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
            suma = Math.Round(suma, 4);
            n = 0;
            foreach (Grupo g in grupos)
            {
                if (suma != 1.0000) g.fraccion_poblacion = Math.Round(g.fraccion_poblacion / suma, 3);
                fraccion_ahora[n] = g.fraccion_poblacion;
                indi_ahora[n] = (long)(INDIVIDUOS * g.fraccion_poblacion);
                n++;
            }
            individuos_base = new List<Individuo>();
            double xt;
            double yt;
            Random azar_poblacion;
            if (SEMILLA_POBLACION == 0)
            {
                azar_poblacion = new Random(12345);
            }
            else
            {
                azar_poblacion = new Random(SEMILLA_POBLACION);
            }
            n = 0;
            individuos_base = new List<Individuo>();
            foreach (Grupo g in grupos)
            {
                for (long i = 0; i < indi_ahora[n]; i++)
                {
                    while (true)
                    {
                        xt = 2.0 * azar_poblacion.NextDouble() - 1.0;
                        yt = 2.0 * azar_poblacion.NextDouble() - 1.0;
                        if ((xt * xt + yt * yt) <= 1.0) break;
                    }
                    xt = RADIO * xt;
                    yt = RADIO * yt;
                    individuos_base.Add(new Individuo(individuos_base.Count, g.tipo, g.ID, g.grupo, xt, yt, 0));
                }
                n++;
            }
            if (individuos_base.Count == 0)
            {
                return false;
            }
            return true;
        }
        private bool CreaIndividuos(int hilo)
        {
            // Copiar la población base

            individuos[hilo] = new List<Individuo>();
            foreach (Individuo individuo in individuos_base)
            {
                individuos[hilo].Add(new Individuo(individuo.ID, grupos_hilo[hilo].ElementAt(individuo.grupo_ID).tipo, individuo.grupo_ID, individuo.grupo, individuo.xi, individuo.yi, 0));
            }
            long sanos_ini = individuos[hilo].Count;
            Random azar_poblacion;
            if (SEMILLA_POBLACION == 0)
            {
                azar_poblacion = new Random(12345);
            }
            else
            {
                azar_poblacion = new Random(SEMILLA_POBLACION + hilo);
            }
            Individuo indi;

            // Focos inciciales

            if (sanos_ini <= N_FOCOS_INICIALES)
            {
                try
                {
                    object[] objetomensaje = new object[] { hilo, string.Format("{0}. Hay más focos de infección que que individuos sanos. Cancelado [{1}]", hilo, CARPETA) };
                    Invoke(delegadoMensaje, objetomensaje);
                }
                catch (Exception ee)
                {
                    object[] objetomensaje = new object[] { hilo, ee.Message };
                    Invoke(delegadoMensaje, objetomensaje);
                }
                return false;
            }
            for (int nf = 0; nf < N_FOCOS_INICIALES; nf++)
            {
                // Buscar al azar uno sano para infectarlo y marcarlo como recien infectado (indi.dias_infectado = 0)

                int i;
                while (true)
                {
                    i = (int)(azar_poblacion.NextDouble() * individuos[hilo].Count);
                    if (individuos[hilo].ElementAt(i).estado == 0)
                    {
                        indi = individuos[hilo].ElementAt(i);
                        indi.estado = 1;
                        sanos_ini--;
                        break;
                    }
                }
            }

            // Inmunizados iniciales

            if (sanos_ini <= N_INMUNES_INICIALES)
            {
                try
                {
                    object[] objetomensaje = new object[] { hilo, string.Format("{0}. Hay más inmunizados que individuos sanos. Cancelado [{1}]", hilo, CARPETA) };
                    Invoke(delegadoMensaje, objetomensaje);
                }
                catch (Exception ee)
                {
                    object[] objetomensaje = new object[] { hilo, ee.Message };
                    Invoke(delegadoMensaje, objetomensaje);
                }
                return false;
            }
            for (int nf = 0; nf < N_INMUNES_INICIALES; nf++)
            {
                // Buscar al azar uno sano para inmunizarlo (curarlo)

                int i;
                while (true)
                {
                    i = (int)(azar_poblacion.NextDouble() * individuos[hilo].Count);
                    if (individuos[hilo].ElementAt(i).estado == 0)
                    {
                        indi = individuos[hilo].ElementAt(i);
                        indi.estado = 2;
                        break;
                    }
                }
            }
            return true;
        }
        private bool CreaClusters(int hilo)
        {
            int[] clusters_grupo = new int[grupos_hilo[hilo].Count];

            // Normalizar porcentajes ('fraccion_clusters'))

            double suma = 0.0;
            foreach (Grupo g in grupos_hilo[hilo])
            {
                suma += g.fraccion_clusters;
            }
            suma = Math.Round(suma, 4);

            // Cluster por grupo

            int suma_c = 0;
            int n = 0;
            foreach (Grupo g in grupos_hilo[hilo])
            {
                if (suma != 1.0000) g.fraccion_clusters = Math.Round(g.fraccion_clusters / suma, 3);
                clusters_grupo[n] = (int)(NUMERO_CLUSTERS_DATO[hilo] * g.fraccion_clusters);
                suma_c += clusters_grupo[n];
                n++;
            }

            // Individuos por cluster

            long infectados_dentro = 0;
            Individuo indi;

            for (int nindi = 0; nindi < individuos[hilo].Count; nindi++)
            {
                indi = individuos[hilo].ElementAt(nindi);
                indi.cluster = -1;
                indi.contagio_en_cluster = false;
            }
            int indi_g;
            int indi_c;
            int desde_indi;
            n = 0;
            int k = -1;
            total_individuos_clusters[hilo] = 0;
            infectados_iniciales_clusters[hilo] = 0;
            foreach (Grupo g in grupos_hilo[hilo])
            {
                k++;
                if (clusters_grupo[k] == 0) continue;
                indi_g = (int)(g.fraccion_poblacion * INDIVIDUOS);
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

                    individuos_cluster[hilo, n] = new List<Individuo>();
                    for (int nindi = desde_indi; nindi < individuos[hilo].Count; nindi++)
                    {
                        indi = individuos[hilo].ElementAt(nindi);
                        if (indi.grupo_ID == g.ID)
                        {
                            if (individuos_cluster[hilo, n].Count >= MAX_INDIVIDUOS_CLUSTER)
                            {
                                try
                                {
                                    object[] objetomensaje = new object[] { hilo, string.Format("Superado el número máximo de individuos por cluster {0}", MAX_INDIVIDUOS_CLUSTER) };
                                    Invoke(delegadoMensaje, objetomensaje);
                                }
                                catch (Exception ee)
                                {
                                    object[] objetomensaje = new object[] { hilo, ee.Message };
                                    Invoke(delegadoMensaje, objetomensaje);
                                }
                                return false;
                            }
                            individuos_cluster[hilo, n].Add(indi);
                            if (indi.estado == 1)
                            {
                                infectados_dentro++;
                                infectados_iniciales_clusters[hilo]++;
                            }
                            indi.cluster = n;
                            total_individuos_clusters[hilo]++;
                            if (individuos_cluster[hilo, n].Count == indi_c)
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
            NUMERO_CLUSTERS[hilo] = n;
            return true;
        }
        private bool TablaVecinos()
        {
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
            long n = 0;
            if (individuos_base.Count == 0)
            {
                return false;
            }
            for (int i = 0; i < individuos_base.Count; i++)
            {
                if (i % 1000 == 0)
                {
                    linea_estado.Text = string.Format("Creando tabla de vecinos {0} de {1}", i, individuos_base.Count);
                    Application.DoEvents();
                }
                if (cancelar)
                {
                    linea_estado.Text = string.Empty;
                    return false;
                }
                indi_i = individuos_base.ElementAt(i);
                g = grupos.ElementAt(indi_i.grupo_ID);
                d_max_i = g.pasos_max * LON_PASO;
                nv = 0;
                for (int j = 0; j < individuos_base.Count; j++)
                {
                    if (i == j) continue;
                    indi_j = individuos_base.ElementAt(j);
                    g = grupos.ElementAt(indi_j.grupo_ID);
                    d_max = d_max_i + g.pasos_max * LON_PASO;
                    x = indi_i.xi - indi_j.xi;
                    y = indi_i.yi - indi_j.yi;
                    d = Math.Sqrt(x * x + y * y);
                    if (d < d_max)
                    {
                        if (nv == MAX_VECINOS_PARA_TABLA - 1)
                        {
                            MessageBox.Show("Superada la capacidad de la tabla", "Tabla de vecinos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                        vecinas[0, i, nv] = j;
                        nv++;
                        n++;
                    }
                    d_med += d;
                    d_max_med += d_max;
                }
                nvecinas[0, i] = nv;
                if (nv > nv_max) nv_max = nv;
            }
            linea_estado.Text = string.Empty;
            return true;
        }
        private bool ActualizaTablaVecinos()
        {
            // PENDIENTE

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
                    MessageBox.Show(string.Format("El máximo número de días de convalecencia es {0}", MAX_CONVALECENCIA - 1), string.Format("Simulación [{0}]", CARPETA), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void PoblacionCambiado()
        {
            if (ignorar_cambio) return;
            CasoCambiado();
        }
        private void PoblacionCambiado(object sender, EventArgs e)
        {
            PoblacionCambiado();
        }
        #endregion

        #region Simulación
        private void B_simula_Click(object sender, EventArgs e)
        {
            Simulacion();
        }
        private bool SituaCondiciones(int hilo, int dia)
        {
            Condicion c = condiciones.ElementAt(cont_condiciones[hilo]);
            grupos_hilo[hilo] = LeeGrupos(c.f_grupos);
            if (c.clusters != -1)
            {
                NUMERO_CLUSTERS_DATO[hilo] = c.clusters;
                GRUPO_IMPORTADO[hilo] = -1;
                if (grupos_hilo[hilo].Count > 0 && N_FOCOS_IMPORTADOS > 0)
                {
                    string sg = d_grupo_importado.Text;
                    int n = 0;
                    foreach (Grupo g in grupos_hilo[hilo])
                    {
                        if (sg.Equals(g.grupo, StringComparison.OrdinalIgnoreCase))
                        {
                            GRUPO_IMPORTADO[hilo] = n;
                            break;
                        }
                        n++;
                    }
                }
                if (individuos[hilo] == null)
                {
                    // Si no está creada la población es que es la primera vez y los clusters se crearán con la población
                    // es decir, aún no se han creados

                    EscribeCambioCondiciones(hilo, dia, c, false);
                }
                else
                {
                    if (!CreaClusters(hilo))
                    {
                        return false;
                    }
                    EscribeCambioCondiciones(hilo, dia, c, true);
                }
            }
            cont_condiciones[hilo]++;
            if (cont_condiciones[hilo] == condiciones.Count)
            {
                // No hay más cambios de condiciones

                prox_condiciones[hilo] = 99999;
            }
            else
            {
                // Cambio de condiciones dentro de 'c.dias'

                if (c.dias < 1)
                {
                    // No hay más cambios de condiciones

                    prox_condiciones[hilo] = 99999;
                }
                else
                {
                    prox_condiciones[hilo] += c.dias;
                }
            }
            return true;
        }
        private void EscribeCambioCondiciones(int hilo, int dia, Condicion c, bool clus)
        {
            string carpeta = string.Format(@"{0}:\{1}\{2}", U_SALIDAS, CARPETA_SALIDAS, CARPETA);
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
            string fichero = string.Format(@"{0}\saux{1:D3}.log", carpeta, hilo + 1);
            FileStream fw = new FileStream(fichero, FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(fw);
            sw.WriteLine(string.Format("{0:D2}/{1:D2}/{2:D4} {3:D2}:{4:D2}:{5:D2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
            sw.WriteLine();
            sw.WriteLine(string.Format("Cambio de condiciones el día {0}", dia));
            sw.WriteLine(string.Format("Fichero de grupos            {0}", c.f_grupos));
            sw.WriteLine(string.Format("Número de clusters           {0}", c.clusters));
            if (c.dias == 0)
            {
                sw.WriteLine("No hay próximos cambios");
            }
            else
            {
                sw.WriteLine(string.Format("Próximo cambio en            {0} días", c.dias));
            }
            sw.WriteLine();
            sw.WriteLine("                        - Edad-      %     % Factor B    Días         %      Num.Pasos      %                          %      Individuos ");
            sw.WriteLine("Grupo            Género Min Max   Gravedad   Contagio  Duración    Curación   Min   Max Individuos   Individuos     clusters    cluster  ");
            sw.WriteLine("---------------- ------ --- --- ---------- ---------- ---------- ---------- ----- ----- ---------- ------------ ------------ ------------");
            int n = 0;
            long i_g;
            foreach (Grupo gt in grupos_hilo[hilo])
            {
                i_g = indi_grupos[hilo, n];
                sw.WriteLine(string.Format("{0,-16} {1,6} {2,3:N0} {3,3:N0} {4,10:f2} {5,10:f2} {6,10:f2} {7,10:f2} {8,5:N0} {9,5:N0} {10,10:f2} {11,12:N0} {12,12:f1} {13,12:N0}", gt.grupo, gt.tipo, gt.edad_min, gt.edad_max, gt.gravedad * 100.0, gt.prob_contagio * 100.0, gt.duracion_infeccion, gt.prob_curacion * 100.0, gt.pasos_min, gt.pasos_max, gt.fraccion_poblacion * 100.0, i_g, gt.fraccion_clusters * 100.0, gt.individuos_c));
                n++;
            }
            sw.WriteLine("---------------- ------ --- --- ---------- ---------- ---------- ---------- ----- ----- ---------- ------------ ------------ ------------");
            sw.WriteLine();
            if (clus)
            {
                ClustersFinal(sw, hilo);
            }
            sw.Close();
        }
        private bool PreparaPoblacion(int hilo)
        {
            if (!string.IsNullOrEmpty(F_INDIVIDUOS))
            {
                // Importar población

                string carpeta = Path.GetDirectoryName(F_INDIVIDUOS);
                string[] sd = F_INDIVIDUOS.Split('_');
                if (sd.Length != 3 || !sd[0].Equals("Ind") || !sd[3].EndsWith(".iii"))
                {
                    return false;
                }
                string ficheroimp = Path.Combine(carpeta, string.Format("Pobla_{0:D3}_{1}", hilo, sd[2]));
                if (!ImportaPoblacion(ficheroimp, hilo))
                {
                    return false;
                }

                // Actualizar los clusters al número de clusters del caso

                if (NUMERO_CLUSTERS_DATO[hilo] != NUMERO_CLUSTERS[hilo])
                {
                    if (!CreaClusters(hilo))
                    {
                        return false;
                    }
                }
            }
            else
            {
                // Crear la población

                F_INDIVIDUOS = string.Empty;
                if (!CreaIndividuos(hilo))
                {
                    return false;
                }

                // Crear los clusters

                if (!CreaClusters(hilo))
                {
                    return false;
                }
            }
            return true;
        }
        private void ActualizaEstadisticas(int hilo, int dia)
        {
            hiloDia[hilo] = dia;
            v_dia.Text = string.Format("{0:N0}", dia + 1);
            v_hilo.Text = string.Format("{0:N0}", hilo + 1);
            int faltan = 0;
            int dia_min_hilo = 9999;
            int dia_max_hilo = 0;
            int hilo_min_hilo = 0;
            int hilo_max_hilo = 0;
            for (int i = 0; i < NUM_HILOS; i++)
            {
                if (hiloEstado[i] == 0)
                {
                    faltan++;
                    if (hiloDia[i] > dia_max_hilo)
                    {
                        hilo_max_hilo = i + 1;
                        dia_max_hilo = hiloDia[i];
                    }
                    if (hiloDia[i] < dia_min_hilo)
                    {
                        hilo_min_hilo = i + 1;
                        dia_min_hilo = hiloDia[i];
                    }
                }
            }
            v_pendientes.Text = string.Format("{0:N0}", faltan);
            dia_min.Text = string.Format("{0:N0}", dia_min_hilo + 1);
            dia_max.Text = string.Format("{0:N0}", dia_max_hilo + 1);
            hilo_min.Text = string.Format("{0:N0}", hilo_min_hilo);
            hilo_max.Text = string.Format("{0:N0}", hilo_max_hilo);
            long[] suma = new long[5];
            for (int j = 0; j < suma.Length; j++)
            {
                suma[j] = 0;
            }
            for (int j = 0; j < NUM_TIPOS; j++)
            {
                suma[0] += sanos[hilo, j];
                suma[1] += desinmunizados[hilo, j];
                suma[2] += infectados[hilo, j];
                suma[3] += curados[hilo, j];
                suma[4] += muertos[hilo, j];
            }
            v_sanos.Text = string.Format("{0:N0}", suma[0]);
            v_desinmunizados.Text = string.Format("{0:N0}", suma[1]);
            v_infectados.Text = string.Format("{0:N0}", suma[2]);
            v_curados.Text = string.Format("{0:N0}", suma[3]);
            v_muertos.Text = string.Format("{0:N0}", suma[4]);
            long[] suma_graves = new long[5];
            for (int j = 0; j < suma.Length; j++)
            {
                suma_graves[j] = 0;
            }
            for (int j = 0; j < NUM_TIPOS; j++)
            {
                suma_graves[0] += infectados_graves[hilo, j];
                suma_graves[1] += curados_graves[hilo, j];
                suma_graves[2] += muertos_graves[hilo, j];
                suma_graves[3] += enfermos[hilo, j];
                suma_graves[4] += hospitalizados[hilo, j];
            }
            v_infectados_graves.Text = string.Format("{0:N0}", suma_graves[0]);
            v_curados_graves.Text = string.Format("{0:N0}", suma_graves[1]);
            v_muertos_graves.Text = string.Format("{0:N0}", suma_graves[2]);
            v_enfermos.Text = string.Format("{0:N0}", suma_graves[3]);
            v_hospital.Text = string.Format("{0:N0}", suma_graves[4]);
            Application.DoEvents();
        }
        private void FinHilo()
        {
            lock (sync)
            {
                for (int i = 0; i < NUM_HILOS; i++)
                {
                    if (hiloEstado[i] == 0) return;
                }
            }

            // Todos los hilos han terminado

            v_pendientes.Text = "0";
            try
            {
                TimeSpan dt = (DateTime.Now - tiempo_inicio);
                SalidaValoresMedios(NUM_HILOS, dt);
            }
            catch { }
            Desactiva(false);
            b_cancelar.Enabled = false;
        }
        private void MensajeHilo(int nh, string s)
        {
            MessageBox.Show(s, "Hilo:" + nh.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void Simulacion()
        {
            if (condiciones.Count == 0)
            {
                MessageBox.Show("No hay condiciones", string.Format("Simulación [{0}]", CARPETA), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ActualizaCondiciones();
            if (!Parametros()) return;
            if (!ProbabilidadFinInfeccion()) return;

            // Días para exportar la población

            PROX_DIA_EXPORTAR = new int[NUM_HILOS];
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
                NUM_DIAS_EXPORTAR = sdex.Length;
                if (NUM_DIAS_EXPORTAR > 0)
                {
                    int ne = 0;
                    DIAS_EXPORTAR = new int[sdex.Length];
                    for (int i = 0; i < NUM_DIAS_EXPORTAR; i++)
                    {
                        if (sdex[i].Trim().Length == 0) continue;
                        DIAS_EXPORTAR[ne] = Convert.ToInt32(sdex[i]) - 1;
                        if (DIAS_EXPORTAR[i] < 0) DIAS_EXPORTAR[i] = 0;
                        ne++;
                    }
                    NUM_DIAS_EXPORTAR = ne;
                }
                for (int nh = 0; nh < NUM_HILOS; nh++)
                {
                    PROX_DIA_EXPORTAR[nh] = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, string.Format("Simulación [{0}]", CARPETA), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dia_min.Text = string.Empty;
            dia_max.Text = string.Empty;
            hilo_min.Text = string.Empty;
            hilo_max.Text = string.Empty;

            string carpeta = string.Format(@"{0}:\{1}\{2}", U_SALIDAS, CARPETA_SALIDAS, CARPETA);
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

            // Crear matrices de hilos

            NUMERO_CLUSTERS = new int[NUM_HILOS];
            NUMERO_CLUSTERS_DATO = new int[NUM_HILOS];
            GRUPO_IMPORTADO = new int[NUM_HILOS];
            prox_condiciones = new int[NUM_HILOS];
            cont_condiciones = new int[NUM_HILOS];
            max_pasos_dia = new int[NUM_HILOS];
            total_pasos = new long[NUM_HILOS];
            total_recorridos = new long[NUM_HILOS];
            acumulador_infectados = new long[NUM_HILOS];
            total_distancias_ida = new double[NUM_HILOS];
            total_contactos_de_riesgo = new long[NUM_HILOS];
            grupos_hilo = new List<Grupo>[NUM_HILOS];

            // Necesitamos un valor para 'grupos.Count'
            // Lo tomamos de la primera condición
            // Tambien necesitamos el número de tipos

            Condicion c = condiciones.ElementAt(0);
            grupos = LeeGrupos(c.f_grupos);
            int ng = grupos.Count;
            NUM_TIPOS = 0;
            foreach (Grupo g in grupos)
            {
                if (NUM_TIPOS < g.tipo)
                {
                    if (g.tipo >= MAX_TIPOS)
                    {
                        MessageBox.Show(string.Format("Número de tipo {0} superior al máximo {1}", NUM_TIPOS, MAX_TIPOS), string.Format("Simulación [{0}]", CARPETA), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    NUM_TIPOS = g.tipo;
                }
            }
            NUM_TIPOS++;

            infectados_grupo = new long[NUM_HILOS, ng];
            indi_grupos = new long[NUM_HILOS, ng];
            total_pasos_g = new long[NUM_HILOS, ng];
            total_recorridos_g = new long[NUM_HILOS, ng];
            total_distancias_ida_g = new double[NUM_HILOS, ng];
            total_contactos_de_riesgo_g = new long[NUM_HILOS, ng];
            total_dias_infectados_curados_g = new long[NUM_HILOS, ng];
            total_dias_infectados_muertos_g = new long[NUM_HILOS, ng];
            infectados_grupo = new long[NUM_HILOS, ng];
            n_sorteos_fin_infeccion = new long[NUM_HILOS, ng];
            n_fin_infeccion = new long[NUM_HILOS, ng];
            res_fin_infeccion = new long[NUM_HILOS, ng, 2];

            contagios_cercania = new long[NUM_HILOS];
            contagios_clusters = new long[NUM_HILOS];

            // Necesitamos un valor para la dimensión de número de clusters
            // La tomamos de la primera condición

            int nc = c.clusters;
            individuos_cluster = new List<Individuo>[NUM_HILOS, nc];
            intentos_cluster = new long[NUM_HILOS, nc];
            infectados_cluster = new long[NUM_HILOS, nc];

            total_individuos_clusters = new long[NUM_HILOS];
            infectados_iniciales_clusters = new long[NUM_HILOS];
            total_dias_infectados_curados = new long[NUM_HILOS];
            total_dias_infectados_muertos = new long[NUM_HILOS];
            infectados_total_clusters = new long[NUM_HILOS];
            curados_total_clusters = new long[NUM_HILOS];
            muertos_total_clusters = new long[NUM_HILOS];

            sanos = new long[NUM_HILOS, NUM_TIPOS];
            desinmunizados = new long[NUM_HILOS, NUM_TIPOS];
            infectados = new long[NUM_HILOS, NUM_TIPOS];
            importados = new long[NUM_HILOS, NUM_TIPOS];
            enfermos = new long[NUM_HILOS, NUM_TIPOS];
            hospitalizados = new long[NUM_HILOS, NUM_TIPOS];
            curados = new long[NUM_HILOS, NUM_TIPOS];
            muertos = new long[NUM_HILOS, NUM_TIPOS];
            infectados_indirectos = new long[NUM_HILOS, NUM_TIPOS];

            infectados_graves = new long[NUM_HILOS, NUM_TIPOS];
            curados_graves = new long[NUM_HILOS, NUM_TIPOS];
            muertos_graves = new long[NUM_HILOS, NUM_TIPOS];

            restipos = new List<ResTipo>[NUM_HILOS, NUM_TIPOS];
            for (int i = 0; i < NUM_HILOS; i++)
            {
                for (int j = 0; j < NUM_TIPOS; j++)
                {
                    restipos[i, j] = new List<ResTipo>();
                }
            }

            individuos = new List<Individuo>[NUM_HILOS];
            resultados = new List<Resultado>[NUM_HILOS];

            Desactiva(true);
            cancelar = false;
            b_cancelar.Enabled = true;
            tiempo_inicio = DateTime.Now;
            Application.DoEvents();

            // La población tendrá las mismas coordenadas en todos los hilos, para que la tabla de vecinos sea única

            F_INDIVIDUOS = string.Empty;
            if (!IndividuosBase())
            {
                MessageBox.Show("No se pudo crear la población base", string.Format("Simulación [{0}]", CARPETA), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tabla de vecinos únicas para todos los hilos

            if (individuos_base.Count >= MAX_INDIVIDUOS_PARA_TABLA)
            {
                MessageBox.Show("No hay memoria para crear la tabla de vecinos", string.Format("Simulación [{0}]", CARPETA), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            nvecinas = new int[1, individuos_base.Count];
            vecinas = new int[1, individuos_base.Count, MAX_VECINOS_PARA_TABLA + 1];
            linea_estado.Text = "Creando tabla de vecinos ...";
            Application.DoEvents();
            if (!TablaVecinos())
            {
                MessageBox.Show("No se pudo crear la tabla de vecinos", string.Format("Simulación [{0}]", CARPETA), MessageBoxButtons.OK, MessageBoxIcon.Error);
                linea_estado.Text = string.Empty;
                return;
            }
            linea_estado.Text = string.Empty;
            Application.DoEvents();

            // Lanzar los hilos

            hiloDia = new int[NUM_HILOS];
            hiloAzar = new Random[NUM_HILOS];
            hiloEstado = new int[NUM_HILOS];
            hiloSimulacion = new Thread[NUM_HILOS];

            for (int nh = 0; nh < NUM_HILOS; nh++)
            {
                hiloDia[nh] = 0;
                hiloAzar[nh] = new Random(SEMILLA_SIMULACION + nh);
                hiloEstado[nh] = 0;
                hiloSimulacion[nh] = new Thread(new ParameterizedThreadStart(Hilo));
                hiloSimulacion[nh].Start(nh);
            }
        }
        private void Hilo(object data)
        {
            int hilo = (int)data;
            int res;

            // Condiciones de partida

            cont_condiciones[hilo] = 0;
            prox_condiciones[hilo] = 0;
            if (!SituaCondiciones(hilo, 0))
            {
                lock (sync)
                {
                    hiloEstado[hilo] = 1;
                }
                try { Invoke(delegadoFinHilo); }
                catch { }
                return;
            }
            PreparaPoblacion(hilo);
            if (cancelar)
            {
                lock (sync)
                {
                    hiloEstado[hilo] = 1;
                }
                try { Invoke(delegadoFinHilo); }
                catch { }
                return;
            }

            // Poner a cero los contadores

            for (int i = 0; i < NUMERO_CLUSTERS[hilo]; i++)
            {
                intentos_cluster[hilo, i] = 0;
                infectados_cluster[hilo, i] = 0;
            }
            for (int i = 0; i < grupos_hilo[hilo].Count; i++)
            {
                indi_grupos[hilo, i] = 0;
                total_pasos_g[hilo, i] = 0;
                total_recorridos_g[hilo, i] = 0;
                total_distancias_ida_g[hilo, i] = 0;
                total_contactos_de_riesgo_g[hilo, i] = 0;
                total_dias_infectados_curados_g[hilo, i] = 0;
                total_dias_infectados_muertos_g[hilo, i] = 0;
                infectados_grupo[hilo, i] = 0;
                n_sorteos_fin_infeccion[hilo, i] = 0;
                n_fin_infeccion[hilo, i] = 0; ;
                res_fin_infeccion[hilo, i, 0] = 0;
                res_fin_infeccion[hilo, i, 1] = 0;
            }
            total_pasos[hilo] = 0;
            total_recorridos[hilo] = 0;
            acumulador_infectados[hilo] = 0;
            total_distancias_ida[hilo] = 0.0;
            total_contactos_de_riesgo[hilo] = 0;

            contagios_cercania[hilo] = 0;
            contagios_clusters[hilo] = 0;
            total_dias_infectados_curados[hilo] = 0;
            total_dias_infectados_muertos[hilo] = 0;
            infectados_total_clusters[hilo] = 0;
            curados_total_clusters[hilo] = 0;
            muertos_total_clusters[hilo] = 0;

            // Prepara los individuos

            for (int i = 0; i < NUM_TIPOS; i++)
            {
                sanos[hilo, i] = 0;
                desinmunizados[hilo, i] = 0;
                infectados[hilo, i] = 0;
                importados[hilo, i] = 0;
                enfermos[hilo, i] = 0;
                hospitalizados[hilo, i] = 0;
                curados[hilo, i] = 0;
                muertos[hilo, i] = 0;
                infectados_indirectos[hilo, i] = 0;

                infectados_graves[hilo, i] = 0;
                curados_graves[hilo, i] = 0;
                muertos_graves[hilo, i] = 0;
            }

            int tipo;
            double gravedad;
            foreach (Individuo indi in individuos[hilo])
            {
                indi_grupos[hilo, indi.grupo_ID]++;
                indi.pasos_dia = 0;
                indi.pasos = null;
                indi.distancia = 0.0;
                indi.sentido = 0;
                indi.pasos_dados = 0;
                tipo = grupos_hilo[hilo].ElementAt(indi.grupo_ID).tipo;
                gravedad = grupos_hilo[hilo].ElementAt(indi.grupo_ID).gravedad;
                if (indi.contagio_en_cluster)
                {
                    infectados_total_clusters[hilo]++;
                }
                switch (indi.estado)
                {
                    case 0:
                        sanos[hilo, tipo]++;
                        break;
                    case 1:
                        infectados[hilo, tipo]++;
                        infectados_grupo[hilo, indi.grupo_ID]++;
                        break;
                    case 2:
                        curados[hilo, tipo]++;
                        total_dias_infectados_curados[hilo] += indi.dias_infectado;
                        total_dias_infectados_curados_g[hilo, indi.grupo_ID] += indi.dias_infectado;
                        if (indi.contagio_en_cluster)
                        {
                            curados_total_clusters[hilo]++;
                        }
                        break;
                    case 3:
                        desinmunizados[hilo, tipo]++;
                        break;
                    default:
                        muertos[hilo, tipo]++;
                        total_dias_infectados_muertos[hilo] += indi.dias_infectado;
                        total_dias_infectados_muertos_g[hilo, indi.grupo_ID] += indi.dias_infectado;
                        if (indi.contagio_en_cluster)
                        {
                            muertos_total_clusters[hilo]++;
                        }
                        break;
                }
                if (indi.grave || gravedad > 0.9)
                {
                    switch (indi.estado)
                    {
                        case 1:
                            infectados_graves[hilo, tipo]++;
                            break;
                        case 2:
                            curados_graves[hilo, tipo]++;
                            break;
                        case -1:
                            muertos_graves[hilo, tipo]++;
                            break;
                    }
                }
                if (indi.enfermo) enfermos[hilo, tipo]++;
                if (indi.hospitalizado) hospitalizados[hilo, tipo]++;
            }
            try
            {
                object[] objetonumhilo = new object[] { hilo, 0 };
                Invoke(delegadoActualizaEstadisticas, objetonumhilo);
            }
            catch
            {
                lock (sync)
                {
                    hiloEstado[hilo] = 1;
                }
                try { Invoke(delegadoFinHilo); }
                catch { }
                return;
            }

            // Simula los días

            res = CicloDias(hilo);

            // Hilo terminado

            lock (sync)
            {
                hiloEstado[hilo] = 1;
            }
            if (res == -1)
            {
                cancelar = true;
            }
            try { Invoke(delegadoFinHilo); }
            catch { }
        }
        private int CicloDias(int hilo)
        {
            resultados[hilo] = new List<Resultado>();
            string carpeta = string.Format(@"{0}:\{1}\{2}", U_SALIDAS, CARPETA_SALIDAS, CARPETA);
            string fichero = string.Format(@"{0}\saux{1:D3}.log", carpeta, hilo + 1);
            FileStream fw = new FileStream(fichero, FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(fw);
            sw.WriteLine("Situación inicial");
            sw.WriteLine();
            ClustersInicio(sw, hilo);
            sw.Close();
            fichero = string.Format(@"{0}\s{1:D3}.log", carpeta, hilo + 1);
            fw = new FileStream(fichero, FileMode.Append, FileAccess.Write, FileShare.Read);
            sw = new StreamWriter(fw);
            CabeceraSalida(sw, hilo);
            long n_R0;
            long suma_R0;
            double R0;
            long n_R0_graves;
            long suma_R0_graves;
            double R0_graves;
            double xt;
            double yt;
            double pre;
            double contactos_de_riesgo;
            double media_dias_infectado;
            double recorrido_individuo_activo;
            long[] suma;
            long[] suma_graves = new long[3];

            Grupo g;
            int id_grupo_importar = -1;
            string nombre_grupo_importar = string.Empty;
            if (N_FOCOS_IMPORTADOS > 0 && GRUPO_IMPORTADO[hilo] != -1)
            {
                g = grupos_hilo[hilo].ElementAt(GRUPO_IMPORTADO[hilo]);
                id_grupo_importar = g.ID;
                nombre_grupo_importar = g.grupo;
            }

            // Ciclo diario

            int dias_sim = 0;
            int resultado_dia;
            TimeSpan dt;
            DateTime tiempo_inicio_hilo = DateTime.Now;
            for (int i = 0; i < DIAS; i++)
            {
                if (i == prox_condiciones[hilo])
                {
                    if (!SituaCondiciones(hilo, i))
                    {
                        lock (sync)
                        {
                            hiloEstado[hilo] = 1;
                        }
                        try { Invoke(delegadoFinHilo); }
                        catch { }
                        return -1;
                    }
                }

                // Empieza un nuevo día

                suma = new long[2];
                suma[0] = suma[1] = 0;
                for (int k = 0; k < NUM_TIPOS; k++)
                {
                    suma[0] += sanos[hilo, k];
                    suma[1] += desinmunizados[hilo, k];
                }
                if (suma[0] > 0 || suma[1] > 0)
                {
                    // Preparar los individuos para pasear

                    max_pasos_dia[hilo] = 0;
                    foreach (Individuo indi in individuos[hilo])
                    {
                        if (indi.estado == 2 || indi.estado == -1)
                        {
                            // Los curados y los muertos no pasean

                            continue;
                        }
                        g = grupos_hilo[hilo].ElementAt(indi.grupo_ID);

                        // Para cada individuo, un nuevo recorrido al azar

                        // Desde el punto inicial

                        indi.x = indi.xi;
                        indi.y = indi.yi;

                        // Número de pasos del nuevo recorrido

                        indi.pasos_dia = g.pasos_min + (int)((g.pasos_max - g.pasos_min) * hiloAzar[hilo].NextDouble());
                        indi.pasos = new double[indi.pasos_dia, 2];
                        if (indi.pasos_dia > max_pasos_dia[hilo]) max_pasos_dia[hilo] = indi.pasos_dia;
                        indi.distancia = 0.0;

                        // Empezamos yendo

                        indi.sentido = 0;

                        // Con el contador de pasos a cero

                        indi.pasos_dados = 0;
                    }
                    resultado_dia = ContagiosVecinos(hilo);
                    if (NUMERO_CLUSTERS[hilo] > 0) ContagiosCluster(hilo);
                    if (PROB_INDIRECTA > 0 && DENSIDAD_VECINOS < 1) ContagiosIndirectos(hilo);
                }
                else
                {
                    // Si no hay a quien infectar, no hay que caminar, sólo hay que seguir la evolución de los contagiados.

                    resultado_dia = cancelar ? -1 : 0;
                }
                if (cancelar || resultado_dia == -1)
                {
                    dt = (DateTime.Now - tiempo_inicio_hilo);
                    CierraCiclo(sw, dt, "Cancelado. Simulados", dias_sim, hilo);
                    return -1;
                }

                // 'Curacion / Desinmunización / Muerte / Enfermo / Hospitalizado' al final del día

                foreach (Individuo indi in individuos[hilo])
                {
                    if (indi.estado == 2 && PROB_RECONTAGIO > 0)
                    {
                        // ¿ Curado -> desinmunizado ?

                        g = grupos_hilo[hilo].ElementAt(indi.grupo_ID);
                        pre = 1.0;
                        if (indi.dias_curado > MIN_INMUNIDAD && hiloAzar[hilo].NextDouble() < pre * PROB_RECONTAGIO)
                        {
                            // Perdida de la inmunidad

                            indi.estado = 3;
                            desinmunizados[hilo, indi.tipo]++;
                            curados[hilo, g.tipo]--;
                            if (indi.grave)
                            {
                                curados_graves[hilo, g.tipo]--;
                            }
                            indi.indi_enfermados = 0;
                            indi.dias_infectado = 0;
                            indi.dias_curado = 0;
                            indi.contagio_en_cluster = false;
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

                        g = grupos_hilo[hilo].ElementAt(indi.grupo_ID);
                        double pfi = (indi.dias_infectado > g.duracion_infeccion) ? 1 : PROB_FIN_INFECCION[g.ID, indi.dias_infectado];
                        n_sorteos_fin_infeccion[hilo, g.ID]++;
                        if (hiloAzar[hilo].NextDouble() < pfi)
                        {
                            // Sólo los graves pueden morir, si lo es, sortear si es por curación o muerte

                            n_fin_infeccion[hilo, g.ID]++;
                            double rcm = g.prob_curacion;
                            if (!indi.grave || hiloAzar[hilo].NextDouble() < rcm)
                            {
                                // Curado

                                indi.estado = 2;
                                curados[hilo, g.tipo]++;
                                indi.dias_curado = 0;
                                infectados[hilo, g.tipo]--;
                                if (indi.grave)
                                {
                                    curados_graves[hilo, g.tipo]++;
                                    infectados_graves[hilo, g.tipo]--;
                                }
                                if (indi.enfermo)
                                {
                                    enfermos[hilo, g.tipo]--;
                                    indi.enfermo = false;
                                    if (indi.hospitalizado)
                                    {
                                        hospitalizados[hilo, g.tipo]--;
                                        indi.hospitalizado = false;
                                    }
                                }
                                if (indi.contagio_en_cluster)
                                {
                                    curados_total_clusters[hilo]++;
                                    infectados_total_clusters[hilo]--;
                                }
                                total_dias_infectados_curados[hilo] += indi.dias_infectado;
                                total_dias_infectados_curados_g[hilo, indi.grupo_ID] += indi.dias_infectado;
                                res_fin_infeccion[hilo, g.ID, 0]++;
                            }
                            else
                            {
                                // Muerte

                                indi.estado = -1;
                                muertos[hilo, g.tipo]++;
                                infectados[hilo, g.tipo]--;
                                if (indi.grave)
                                {
                                    muertos_graves[hilo, g.tipo]++;
                                    infectados_graves[hilo, g.tipo]--;
                                }
                                if (indi.enfermo)
                                {
                                    enfermos[hilo, g.tipo]--;
                                    indi.enfermo = false;
                                    if (indi.hospitalizado)
                                    {
                                        hospitalizados[hilo, g.tipo]--;
                                        indi.hospitalizado = false;
                                    }
                                }
                                if (indi.contagio_en_cluster)
                                {
                                    muertos_total_clusters[hilo]++;
                                    infectados_total_clusters[hilo]--;
                                }
                                total_dias_infectados_muertos[hilo] += indi.dias_infectado;
                                total_dias_infectados_muertos_g[hilo, indi.grupo_ID] += indi.dias_infectado;
                                res_fin_infeccion[hilo, g.ID, 1]++;
                            }
                        }
                        else
                        {
                            // Sigue infectado un día más

                            indi.dias_infectado++;
                            if (indi.dias_infectado == DIAS_INCUBACION)
                            {
                                if (indi.grave)
                                {
                                    if (hiloAzar[hilo].NextDouble() < PROB_ENFERMAR)
                                    {
                                        // La enfermedad da la cara

                                        indi.enfermo = true;
                                        enfermos[hilo, g.tipo]++;

                                        // Hospitalización

                                        if (hiloAzar[hilo].NextDouble() < PROB_HOSPITALIZACION)
                                        {
                                            indi.hospitalizado = true;
                                            hospitalizados[hilo, g.tipo]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                suma = new long[9];
                for (int j = 0; j < suma.Length; j++)
                {
                    suma[j] = 0;
                }
                for (int j = 0; j < NUM_TIPOS; j++)
                {
                    suma[0] += sanos[hilo, j];
                    suma[1] += infectados[hilo, j];
                    suma[2] += importados[hilo, j];
                    suma[3] += curados[hilo, j];
                    suma[4] += desinmunizados[hilo, j];
                    suma[5] += muertos[hilo, j];
                    suma[6] += infectados_indirectos[hilo, j];
                    suma[7] += enfermos[hilo, j];
                    suma[8] += hospitalizados[hilo, j];
                    restipos[hilo, j].Add(new ResTipo(sanos[hilo, j], infectados[hilo, j], importados[hilo, j], curados[hilo, j], desinmunizados[hilo, j], muertos[hilo, j], infectados_indirectos[hilo, j], enfermos[hilo, j], hospitalizados[hilo, j]));
                }
                for (int j = 0; j < suma_graves.Length; j++)
                {
                    suma_graves[j] = 0;
                }
                for (int j = 0; j < NUM_TIPOS; j++)
                {
                    suma_graves[0] += infectados_graves[hilo, j];
                    suma_graves[1] += curados_graves[hilo, j];
                    suma_graves[2] += muertos_graves[hilo, j];
                }
                n_R0 = 0;
                suma_R0 = 0;
                n_R0_graves = 0;
                suma_R0_graves = 0;
                foreach (Individuo indi in individuos[hilo])
                {
                    if (indi.estado == 2 || indi.estado == -1)
                    {
                        // Para los que han finalizado la enfermedad (curados y muertos)

                        n_R0++;
                        suma_R0 += indi.indi_enfermados;
                        if (indi.grave)
                        {
                            n_R0_graves++;
                            suma_R0_graves += indi.indi_enfermados;
                        }
                    }
                }
                R0 = n_R0 == 0 ? 0 : (double)suma_R0 / n_R0;
                R0_graves = n_R0_graves == 0 ? 0 : (double)suma_R0_graves / n_R0_graves;

                // Focos inportados

                if (N_FOCOS_IMPORTADOS > 0 && DIAS_FOCOS_IMPORTADOS > 0 && GRUPO_IMPORTADO[hilo] != -1)
                {
                    if (i > 0 && i % DIAS_FOCOS_IMPORTADOS == 0)
                    {
                        for (int j = 0; j < N_FOCOS_IMPORTADOS; j++)
                        {
                            while (true)
                            {
                                xt = 2.0 * hiloAzar[hilo].NextDouble() - 1.0;
                                yt = 2.0 * hiloAzar[hilo].NextDouble() - 1.0;
                                if ((xt * xt + yt * yt) <= 1.0) break;
                            }
                            xt = RADIO * xt;
                            yt = RADIO * yt;

                            // Se importa como tipo 0

                            int tipo = 0;
                            Individuo individuo_nuevo = new Individuo(individuos[hilo].Count, tipo, id_grupo_importar, nombre_grupo_importar, xt, yt, 1, 0, 0, 0, -1, false, false, false, false);
                            individuos[hilo].Add(individuo_nuevo);
                            infectados[hilo, tipo]++;
                            importados[hilo, tipo]++;
                            infectados_grupo[hilo, id_grupo_importar]++;
                        }

                        // Se necesita actualizar las tablas de vecinos. PENDIENTE

                        ActualizaTablaVecinos();
                    }
                }
                if (NUM_DIAS_EXPORTAR > 0 && PROX_DIA_EXPORTAR[hilo] != -1 && i == DIAS_EXPORTAR[PROX_DIA_EXPORTAR[hilo]])
                {
                    string ficheroex = Path.Combine(carpeta, string.Format("Pobla_{0:D3}_{1:D3}.iii", hilo, i + 1));
                    ExportarPoblacion(ficheroex, hilo);
                    //Distancias(i + 1);
                    PROX_DIA_EXPORTAR[hilo]++;
                    if (PROX_DIA_EXPORTAR[hilo] == NUM_DIAS_EXPORTAR)
                    {
                        PROX_DIA_EXPORTAR[hilo] = -1;
                    }
                }

                // La monitorización se hace al final del día

                acumulador_infectados[hilo] += suma[1];
                recorrido_individuo_activo = total_recorridos[hilo] == 0 ? 0 : total_distancias_ida[hilo] / total_recorridos[hilo];
                contactos_de_riesgo = acumulador_infectados[hilo] == 0 ? 0 : (double)total_contactos_de_riesgo[hilo] / acumulador_infectados[hilo];
                media_dias_infectado = suma[3] + suma[5] == 0 ? 0 : (double)(total_dias_infectados_curados[hilo] + total_dias_infectados_muertos[hilo]) / (suma[3] + suma[5]);
                sw.WriteLine(string.Format("{0,3} {1,10} {2,10} {3,10} {4,10} {5,10} {6,10} {7,10:f2} {8,10:f2} {9,10:f1} {10,10:f2} {11,10} {12,10} {13,10} {14,10} {15,10} {16,10} {17,10} {18,10} {19,10} {20,10} {21,10} {22,10:f2}", i + 1, suma[0], suma[1], suma[2], suma[3], suma[4], suma[5], recorrido_individuo_activo, contactos_de_riesgo, media_dias_infectado, R0, contagios_cercania[hilo], contagios_clusters[hilo], infectados_total_clusters[hilo], curados_total_clusters[hilo], muertos_total_clusters[hilo], suma[6], suma[7], suma[8], suma_graves[0], suma_graves[1], suma_graves[2], R0_graves));
                sw.Flush();
                resultados[hilo].Add(new Resultado(i + 1, suma[0], suma[1], suma[2], suma[3], suma[4], suma[5], recorrido_individuo_activo, contactos_de_riesgo, media_dias_infectado, R0, contagios_cercania[hilo], contagios_clusters[hilo], infectados_total_clusters[hilo], curados_total_clusters[hilo], muertos_total_clusters[hilo], suma[6], suma[7], suma[8], suma_graves[0], suma_graves[1], suma_graves[2], R0_graves));
                dias_sim = i + 1;
                try
                {
                    object[] objetonumhilo = new object[] { hilo, i };
                    Invoke(delegadoActualizaEstadisticas, objetonumhilo);
                }
                catch
                {
                    lock (sync)
                    {
                        hiloEstado[hilo] = 1;
                    }
                    try { Invoke(delegadoFinHilo); }
                    catch { }
                    return -1;
                }
                if (suma[1] == 0 && (N_FOCOS_IMPORTADOS == 0 || (N_FOCOS_IMPORTADOS > 0 && suma[0] == 0)))
                {
                    break;
                }
            }
            dt = (DateTime.Now - tiempo_inicio_hilo);
            CierraCiclo(sw, dt, "Simulados", dias_sim, hilo);
            return 0;
        }
        private string CierraCiclo(StreamWriter sw, TimeSpan dt, string texto, int dias_sim, int hilo)
        {
            try
            {
                object[] objetonumhilo = new object[] { hilo, dias_sim - 1 };
                Invoke(delegadoActualizaEstadisticas, objetonumhilo);
            }
            catch
            {
                lock (sync)
                {
                    hiloEstado[hilo] = 1;
                }
                try { Invoke(delegadoFinHilo); }
                catch { }
                return "0";
            }
            sw.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            sw.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos     Ida     críticos  infectados         R0   Cercania   Clusters Infectados    Curados    Muertos  Indirecto   Enfermos Hospitaliz Infectados    Curados    Muertos         R0");
            sw.WriteLine();
            long[] sumas = new long[9];
            for (int j = 0; j < sumas.Length; j++)
            {
                sumas[j] = 0;
            }
            for (int j = 0; j < NUM_TIPOS; j++)
            {
                sumas[0] += sanos[hilo, j];
                sumas[1] += infectados[hilo, j];
                sumas[2] += importados[hilo, j];
                sumas[3] += curados[hilo, j];
                sumas[4] += desinmunizados[hilo, j];
                sumas[5] += muertos[hilo, j];
                sumas[6] += infectados_indirectos[hilo, j];
                sumas[7] += enfermos[hilo, j];
                sumas[8] += hospitalizados[hilo, j];
            }
            sw.WriteLine(string.Format("Infectados importados           {0,20:N0}", sumas[1]));
            sw.WriteLine();
            sw.WriteLine(string.Format("Total pasos                     {0,20:N0}", total_pasos[hilo]));
            sw.WriteLine(string.Format("Total recorridos                {0,20:N0}", total_recorridos[hilo]));
            sw.WriteLine(string.Format("Total distancia recorrida       {0,20:N0}", total_distancias_ida[hilo]));
            sw.WriteLine(string.Format("Contactos de riesgo             {0,20:N0}", total_contactos_de_riesgo[hilo]));
            sw.WriteLine(string.Format("Acumulador infectados           {0,20:N0}", acumulador_infectados[hilo]));
            sw.WriteLine();
            sw.WriteLine(string.Format("Total días infectados (curados) {0,20:N0} {1,10:f2}", total_dias_infectados_curados[hilo], sumas[3] == 0 ? 0 : total_dias_infectados_curados[hilo] / sumas[3]));
            sw.WriteLine(string.Format("Total días infectados (muertos) {0,20:N0} {1,10:f2}", total_dias_infectados_muertos[hilo], sumas[5] == 0 ? 0 : total_dias_infectados_muertos[hilo] / sumas[5]));
            sw.WriteLine(string.Format("Total días infectados           {0,20:N0} {1,10:f2}", total_dias_infectados_curados[hilo] + total_dias_infectados_muertos[hilo], (sumas[3] + sumas[5]) == 0 ? 0 : (total_dias_infectados_curados[hilo] + total_dias_infectados_muertos[hilo]) / (sumas[3] + sumas[5])));
            sw.WriteLine();
            string tiempo = string.Format("{0} {1:N0} días. {2:f1} s", texto, dias_sim, dt.TotalMilliseconds / 1000.0d);
            sw.WriteLine(tiempo);
            sw.WriteLine();
            sw.Close();
            //Distancias(-1);
            string carpeta = string.Format(@"{0}:\{1}\{2}", U_SALIDAS, CARPETA_SALIDAS, CARPETA);
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
            string fichero = string.Format(@"{0}\saux{1:D3}.log", carpeta, hilo + 1);
            FileStream fw = new FileStream(fichero, FileMode.Append, FileAccess.Write, FileShare.Read);
            sw = new StreamWriter(fw);
            sw.WriteLine("Situación final");
            sw.WriteLine();
            ClustersFinal(sw, hilo);
            long[,] infectadores = new long[grupos_hilo[hilo].Count, 2];
            long[,] suma_curados_muertos = new long[grupos_hilo[hilo].Count, 2];
            long[] suma_total = new long[grupos_hilo[hilo].Count];
            int[] infectador_max = new int[grupos_hilo[hilo].Count];
            int[] infectador_min = new int[grupos_hilo[hilo].Count];
            for (int i = 0; i < grupos_hilo[hilo].Count; i++)
            {
                infectadores[i, 0] = infectadores[i, 1] = 0;
                suma_curados_muertos[i, 0] = suma_curados_muertos[i, 1] = 0;
                suma_total[i] = 0;
                infectador_max[i] = -1;
                infectador_min[i] = int.MaxValue;
            }
            int k;
            Grupo g_indi;
            foreach (Individuo indi in individuos[hilo])
            {
                g_indi = grupos_hilo[hilo].ElementAt(indi.grupo_ID);
                suma_total[g_indi.ID] += indi.indi_enfermados;
                if (indi.estado == 2 || indi.estado == -1)
                {
                    k = indi.estado == 2 ? 0 : 1;
                    infectadores[g_indi.ID, k]++;
                    suma_curados_muertos[g_indi.ID, k] += indi.indi_enfermados;
                    if (indi.indi_enfermados < infectador_min[g_indi.ID]) infectador_min[g_indi.ID] = indi.indi_enfermados;
                    if (indi.indi_enfermados > infectador_max[g_indi.ID]) infectador_max[g_indi.ID] = indi.indi_enfermados;
                }
            }
            foreach (Grupo g in grupos_hilo[hilo])
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
                sw.WriteLine(string.Format("Número infectados                     {0,14:N0}", infectados_grupo[hilo, g.ID]));
                sw.WriteLine(string.Format("Número intentos fin infección         {0,14:N0}", n_sorteos_fin_infeccion[hilo, g.ID]));
                sw.WriteLine(string.Format("Número finales de infección           {0,14:N0} {1,10:f2}", n_fin_infeccion[hilo, g.ID], 100.0 * n_fin_infeccion[hilo, g.ID] / n_sorteos_fin_infeccion[hilo, g.ID]));
                sw.WriteLine(string.Format("Número finales curacion               {0,14:N0} {1,10:f2}", res_fin_infeccion[hilo, g.ID, 0], n_fin_infeccion[hilo, g.ID] == 0 ? 0 : 100.0 * res_fin_infeccion[hilo, g.ID, 0] / n_fin_infeccion[hilo, g.ID]));
                sw.WriteLine(string.Format("Número finales muerte                 {0,14:N0} {1,10:f2}", res_fin_infeccion[hilo, g.ID, 1], n_fin_infeccion[hilo, g.ID] == 0 ? 0 : 100.0 * res_fin_infeccion[hilo, g.ID, 1] / n_fin_infeccion[hilo, g.ID]));
                sw.WriteLine(string.Format("Total infecciones                     {0,14:N0}", suma_total[g.ID]));
                sw.WriteLine(string.Format("Total infecciones por curados         {0,14:N0}", suma_curados_muertos[g.ID, 0]));
                sw.WriteLine(string.Format("Total infecciones por muertos         {0,14:N0}", suma_curados_muertos[g.ID, 1]));
                sw.WriteLine(string.Format("Total infectadores curados            {0,14:N0}", infectadores[g.ID, 0]));
                sw.WriteLine(string.Format("Total infectadores muertos            {0,14:N0}", infectadores[g.ID, 1]));
                sw.WriteLine(string.Format("Total pasos                           {0,14:N0}", total_pasos_g[hilo, g.ID]));
                sw.WriteLine(string.Format("Total recorridos                      {0,14:N0}", total_recorridos_g[hilo, g.ID]));
                sw.WriteLine(string.Format("Total distancia recorrida             {0,14:N0}", total_distancias_ida_g[hilo, g.ID]));
                sw.WriteLine(string.Format("Contactos de riesgo                   {0,14:N0}", total_contactos_de_riesgo_g[hilo, g.ID]));
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
            sw.Close();
            string fichero_t = string.Format(@"{0}\st{1:D3}.log", carpeta, hilo + 1);
            FileStream fw_t = new FileStream(fichero_t, FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw_t = new StreamWriter(fw_t);
            ResTipo r;
            for (int j = 0; j < NUM_TIPOS; j++)
            {
                sw_t.WriteLine(string.Format("Tipo {0}", j));
                sw_t.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos Inf.Indire   Enfermos Hospitaliz");
                sw_t.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
                for (int i = 0; i < dias_sim; i++)
                {
                    r = restipos[hilo, j].ElementAt(i);
                    if (r == null) break;
                    sw_t.WriteLine(string.Format("{0,3} {1,10} {2,10} {3,10} {4,10} {5,10} {6,10} {7,10} {8,10} {9,10}", i + 1, r.r_sanos, r.r_infectados, r.r_importados, r.r_curados, r.r_desinmunizados, r.r_muertos, r.r_infectados_indirectos, r.r_enfermos, r.r_hospitalizados));
                }
                sw_t.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
                sw_t.WriteLine();
            }
            sw_t.Close();
            return tiempo;
        }
        private void CabeceraSalida(StreamWriter sw, int hilo)
        {
            sw.WriteLine("--------------------------------------------------------------------------------------------------");
            sw.WriteLine(string.Format("Simulador de contagios. {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            sw.WriteLine(string.Format("{0:D2}/{1:D2}/{2:D4} {3:D2}:{4:D2}:{5:D2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
            sw.WriteLine("--------------------------------------------------------------------------------------------------");
            sw.WriteLine(string.Format("Carpeta {0}", CARPETA));
            sw.WriteLine(string.Format("Número de hilos                    {0,12:N0}", NUM_HILOS));
            sw.WriteLine(string.Format("Semilla población                  {0,12:N0}", SEMILLA_POBLACION));
            sw.WriteLine(string.Format("Semilla simulación                 {0,12:N0}", SEMILLA_SIMULACION + hilo));
            sw.WriteLine(string.Format("Potencia para fin de infección     {0,12:f2}", POTENCIA));
            sw.WriteLine();
            sw.WriteLine(string.Format("Número de individuos               {0,12:N0}", INDIVIDUOS));
            sw.WriteLine(string.Format("Radio                              {0,12:N0}", RADIO));
            sw.WriteLine(string.Format("Probabilidad base de contagio      {0,12:f2} %", 100.0 * PROB_CONTAGIO));
            sw.WriteLine(string.Format("Probabilidad contagio clúster      {0,12:f2} %", 100.0 * PROB_CONTAGIO_C));
            sw.WriteLine(string.Format("Probabilidad base de recontagio    {0,12:f2} %", 100.0 * PROB_RECONTAGIO));
            sw.WriteLine(string.Format("Días mínimos de inmunidad          {0,12:N0}", MIN_INMUNIDAD));
            sw.WriteLine(string.Format("Días de carencia para contagiar    {0,12:N0}", CARENCIA_CONTAGIAR));
            sw.WriteLine(string.Format("Días de incubación                 {0,12:N0}", DIAS_INCUBACION));
            sw.WriteLine(string.Format("Probabilidad de enfermar           {0,12:f2} %", 100.0 * PROB_ENFERMAR));
            sw.WriteLine(string.Format("Factor probab. contagio enfermo    {0,12:f2} %", 100.0 * F_PROB_ENFERMO));
            sw.WriteLine(string.Format("Probabilidad de hospitalización    {0,12:f2} %", 100.0 * PROB_HOSPITALIZACION));
            sw.WriteLine(string.Format("Factor prob. contagio hospitalizad {0,12:f2} %", 100.0 * F_PROB_HOSPITALIZADO));
            sw.WriteLine(string.Format("Densidad crítica de vecinos infect {0,12:f2} %", 100.0 * DENSIDAD_VECINOS));
            sw.WriteLine(string.Format("Probabilidad contagio indirecta    {0,12:f2} %", 100.0 * PROB_INDIRECTA));
            sw.WriteLine();
            sw.WriteLine(string.Format("Número de focos iniciales          {0,12:N0}", N_FOCOS_INICIALES));
            sw.WriteLine(string.Format("Número de inmunes iniciales        {0,12:N0}", N_INMUNES_INICIALES));
            if (N_FOCOS_IMPORTADOS > 0 && GRUPO_IMPORTADO[0] == -1)
            {
                sw.WriteLine(string.Format("Número de focos a importar         {0,12:N0} cada {1} días, del grupo {2} NO ENCONTRADO. Opción deshabilitada.", N_FOCOS_IMPORTADOS, DIAS_FOCOS_IMPORTADOS, d_grupo_importado.Text));
            }
            else
            {
                sw.WriteLine(string.Format("Número de focos a importar         {0,12:N0} cada {1} días, del grupo {2}", N_FOCOS_IMPORTADOS, DIAS_FOCOS_IMPORTADOS, d_grupo_importado.Text));
            }
            sw.WriteLine(string.Format("Radio de contacto                  {0,12:N0}", CONTACTO));
            sw.WriteLine(string.Format("Longitud media 10 pasos            {0,12:N0}", LON_10PASOS));
            //Distancias(0);
            sw.WriteLine();
            sw.WriteLine(string.Format("Número clusters objetivo           {0,12:N0}", NUMERO_CLUSTERS_DATO[hilo == -1 ? 0 : hilo]));
            long shnumeroclusters = 0;
            long shindividuos = 0;
            long shinfectados = 0;
            if (hilo == -1)
            {
                for (int nh = 0; nh < NUM_HILOS; nh++)
                {
                    shnumeroclusters += NUMERO_CLUSTERS[nh];
                    shindividuos += total_individuos_clusters[nh];
                    shinfectados += infectados_iniciales_clusters[nh];
                }
                shnumeroclusters /= NUM_HILOS;
                shindividuos /= NUM_HILOS;
                shinfectados /= NUM_HILOS;
            }
            else
            {
                shnumeroclusters = NUMERO_CLUSTERS[hilo];
                shindividuos = total_individuos_clusters[hilo];
                shinfectados = infectados_iniciales_clusters[hilo];
            }
            sw.WriteLine(string.Format("Número clusters generados          {0,12:N0} con {1,12:N0} individuos, {2,12:N0} infectados de inicio", shnumeroclusters, shindividuos, shinfectados));
            sw.WriteLine(string.Format("Factor individuos por cluster      {0,12:f2}", FACTOR_INDIVIDUOS_CLUSTER));
            sw.WriteLine();
            sw.WriteLine("                        - Edad-      %     % Factor B    Días         %      Num.Pasos      %                          %      Individuos ");
            sw.WriteLine("Grupo            Género Min Max   Gravedad   Contagio  Duración    Curación   Min   Max Individuos   Individuos     clusters    cluster  ");
            sw.WriteLine("---------------- ------ --- --- ---------- ---------- ---------- ---------- ----- ----- ---------- ------------ ------------ ------------");
            sw.Flush();
            int n = 0;
            long i_g;
            foreach (Grupo gt in grupos_hilo[hilo == -1 ? 0 : hilo])
            {
                if (hilo == -1)
                {
                    i_g = 0;
                    for (int nh = 0; nh < NUM_HILOS; nh++)
                    {
                        i_g += indi_grupos[nh, n];
                    }
                    i_g /= NUM_HILOS;
                }
                else
                {
                    i_g = indi_grupos[hilo, n];
                }
                sw.WriteLine(string.Format("{0,-16} {1,6} {2,3:N0} {3,3:N0} {4,10:f2} {5,10:f2} {6,10:f2} {7,10:f2} {8,5:N0} {9,5:N0} {10,10:f2} {11,12:N0} {11,12:f1} {12,12:N0}", gt.grupo, gt.tipo, gt.edad_min, gt.edad_max, gt.gravedad * 100.0, gt.prob_contagio * 100.0, gt.duracion_infeccion, gt.prob_curacion * 100.0, gt.pasos_min, gt.pasos_max, gt.fraccion_poblacion * 100.0, i_g, gt.fraccion_clusters * 100.0, gt.individuos_c));
                n++;
            }
            sw.WriteLine("---------------- ------ --- --- ---------- ---------- ---------- ---------- ----- ----- ---------- ------------ ------------ ------------");
            sw.WriteLine();
            sw.WriteLine("Valores al final del día");
            sw.WriteLine("                                                                       Recorrido  Contactos Media días            ------ Afectados ---- ----------  Clusters -----------                                  -----------------  GRAVES  ----------------");
            sw.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos     Ida     críticos  infectados         R0   Cercania   Clusters Infectados    Curados    Muertos  Indirecto   Enfermos Hospitaliz Infectados    Curados    Muertos         R0");
            sw.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            sw.Flush();
        }
        private void ClustersInicio(StreamWriter sw, int hilo)
        {
            sw.WriteLine("Cluster GID Grupo                Individuos     Sanos  Infectados    Curados Desinmuniz    Muertos");
            sw.WriteLine("------- --- -------------------- ---------- ---------- ---------- ---------- ---------- ----------");
            int estado;
            long[] num_indi_estado = new long[5];
            long[] sum_total_clusters = new long[5];
            for (int i = 0; i < 5; i++)
            {
                num_indi_estado[i] = 0;
                sum_total_clusters[i] = 0;
            }
            for (int n = 0; n < NUMERO_CLUSTERS[hilo]; n++)
            {
                num_indi_estado[0] = num_indi_estado[1] = num_indi_estado[2] = num_indi_estado[3] = num_indi_estado[4] = 0;
                for (int nic = 0; nic < individuos_cluster[hilo, n].Count; nic++)
                {
                    estado = individuos_cluster[hilo, n].ElementAt(nic).estado;
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
                int ng = individuos_cluster[hilo, n].ElementAt(0).grupo_ID;
                sw.WriteLine(string.Format("{0,7:N0} {1,3:N0} {2,-20} {3,10} {4,10} {5,10} {6,10} {7,10} {8,10}", n + 1, ng, grupos_hilo[hilo].ElementAt(ng).grupo, individuos_cluster[hilo, n].Count, num_indi_estado[0], num_indi_estado[1], num_indi_estado[2], num_indi_estado[3], num_indi_estado[4]));
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
        private void ClustersFinal(StreamWriter sw, int hilo)
        {
            sw.WriteLine("                                                       -------------- Dentro y fuera ------------- -------- Dentro -----      %");
            sw.WriteLine("Cluster GID Grupo                Individuos      Sanos Infectados    Curados Desinmuniz    Muertos   Intentos Infectados   Aciertos");
            sw.WriteLine("------- --- -------------------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            int estado;
            long[] suma = new long[2];
            long[] sum_total_clusters = new long[5];
            long[] num_indi_estado = new long[5];
            for (int n = 0; n < NUMERO_CLUSTERS[hilo]; n++)
            {
                suma[0] += intentos_cluster[hilo, n];
                suma[1] += infectados_cluster[hilo, n];
                num_indi_estado[0] = num_indi_estado[1] = num_indi_estado[2] = num_indi_estado[3] = num_indi_estado[4] = 0;
                for (int nic = 0; nic < individuos_cluster[hilo, n].Count; nic++)
                {
                    estado = individuos_cluster[hilo, n].ElementAt(nic).estado;
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
                int ng = individuos_cluster[hilo, n].ElementAt(0).grupo_ID;
                sw.WriteLine(string.Format("{0,7:N0} {1,3:N0} {2,-20} {3,10} {4,10} {5,10} {6,10} {7,10} {8,10} {9,10} {10,10} {11,10:f2}", n + 1, ng, grupos_hilo[hilo].ElementAt(ng).grupo, individuos_cluster[hilo, n].Count, num_indi_estado[0], num_indi_estado[1], num_indi_estado[2], num_indi_estado[3], num_indi_estado[4], intentos_cluster[hilo, n], infectados_cluster[hilo, n], intentos_cluster[hilo, n] == 0 ? 0 : 100.0 * infectados_cluster[hilo, n] / intentos_cluster[hilo, n]));
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
        private void SalidaValoresMedios(int nh, TimeSpan dt)
        {
            if (resultados[0] == null) return;
            int datos;
            long m_sanos;
            long m_infectados;
            long m_importados;
            long m_curados;
            long m_desinmunizados;
            long m_muertos;
            double m_recorrido_individuo_activo;
            double m_contactos_de_riesgo;
            double m_media_dias_infectado;
            double m_R0;
            long m_contagios_cercania;
            long m_contagios_clusters;
            long m_infectados_total_clusters;
            long m_curados_total_clusters;
            long m_muertos_total_clusters;
            long m_infectados_indirectos;
            long m_enfermos;
            long m_hospitalizados;
            long m_infectados_graves;
            long m_curados_graves;
            long m_muertos_graves;
            double m_R0_graves;
            Resultado r;
            List<Resultado> acumulado = new List<Resultado>();
            int max_dias = resultados[0].Count;
            for (int i = 0; i < nh; i++)
            {
                if (resultados[i] != null && max_dias < resultados[i].Count)
                {
                    max_dias = resultados[i].Count;
                }
            }
            for (int i = 0; i < max_dias; i++)
            {
                datos = 0;
                m_sanos = 0;
                m_infectados = 0;
                m_importados = 0;
                m_curados = 0;
                m_desinmunizados = 0;
                m_muertos = 0;
                m_recorrido_individuo_activo = 0;
                m_contactos_de_riesgo = 0;
                m_media_dias_infectado = 0;
                m_R0 = 0;
                m_contagios_cercania = 0;
                m_contagios_clusters = 0;
                m_infectados_total_clusters = 0;
                m_curados_total_clusters = 0;
                m_muertos_total_clusters = 0;
                m_infectados_indirectos = 0;
                m_enfermos = 0;
                m_hospitalizados = 0;
                m_infectados_graves = 0;
                m_curados_graves = 0;
                m_muertos_graves = 0;
                m_R0_graves = 0;
                for (int n = 0; n < nh; n++)
                {
                    if (resultados[n] == null || resultados[n].Count <= i) continue;
                    r = resultados[n].ElementAt(i);
                    datos++;
                    m_sanos += r.r_sanos;
                    m_infectados += r.r_infectados;
                    m_importados += r.r_importados;
                    m_curados += r.r_curados;
                    m_desinmunizados += r.r_desinmunizados;
                    m_muertos += r.r_muertos;
                    m_recorrido_individuo_activo += r.r_recorrido_individuo_activo;
                    m_contactos_de_riesgo += r.r_contactos_de_riesgo;
                    m_media_dias_infectado += r.r_media_dias_infectado;
                    m_R0 += r.r_R0;
                    m_contagios_cercania += r.r_contagios_cercania;
                    m_contagios_clusters += r.r_contagios_clusters;
                    m_infectados_total_clusters += r.r_infectados_total_clusters;
                    m_curados_total_clusters += r.r_curados_total_clusters;
                    m_muertos_total_clusters += r.r_muertos_total_clusters;
                    m_infectados_indirectos += r.r_infectados_indirectos;
                    m_enfermos += r.r_enfermos;
                    m_hospitalizados += r.r_hospitalizados;
                    m_infectados_graves += r.r_infectados_graves;
                    m_curados_graves += r.r_curados_graves;
                    m_muertos_graves += r.r_muertos_graves;
                    m_R0_graves += r.r_R0_graves;
                }
                if (datos == 0)
                {
                    datos = 1;
                }
                acumulado.Add(new Resultado(datos, m_sanos, m_infectados, m_importados, m_curados, m_desinmunizados, m_muertos, m_recorrido_individuo_activo, m_contactos_de_riesgo, m_media_dias_infectado, m_R0, m_contagios_cercania, m_contagios_clusters, m_infectados_total_clusters, m_curados_total_clusters, m_muertos_total_clusters, m_infectados_indirectos, m_enfermos, m_hospitalizados, m_infectados_graves, m_curados_graves, m_muertos_graves, m_R0_graves));
            }
            string carpeta = string.Format(@"{0}:\{1}\{2}", U_SALIDAS, CARPETA_SALIDAS, CARPETA);
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
            string fichero = string.Format(@"{0}\s.log", carpeta);
            FileStream fw = new FileStream(fichero, FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(fw);
            CabeceraSalida(sw, -1);
            double divisor;
            for (int n = 0; n < acumulado.Count; n++)
            {
                r = acumulado.ElementAt(n);
                divisor = (double)r.r_dia;
                sw.WriteLine(string.Format(
                    "{0,3} {1,10} {2,10} {3,10} {4,10} {5,10} {6,10} {7,10:f2} {8,10:f2} {9,10:f1} {10,10:f2} {11,10} {12,10} {13,10} {14,10} {15,10} {16,10} {17,10} {18,10} {19,10} {20,10} {21,10} {22,10:f2}",
                    n + 1,
                    (int)(r.r_sanos / divisor),
                    (int)(r.r_infectados / divisor),
                    (int)(r.r_importados / divisor),
                    (int)(r.r_curados / divisor),
                    (int)(r.r_desinmunizados / divisor),
                    (int)(r.r_muertos / divisor),
                    r.r_recorrido_individuo_activo / divisor,
                    r.r_contactos_de_riesgo / divisor,
                    r.r_media_dias_infectado / divisor,
                    r.r_R0 / divisor,
                    (int)(r.r_contagios_cercania / divisor),
                    (int)(r.r_contagios_clusters / divisor),
                    (int)(r.r_infectados_total_clusters / divisor),
                    (int)(r.r_curados_total_clusters / divisor),
                    (int)(r.r_muertos_total_clusters / divisor),
                    (int)(r.r_infectados_indirectos / divisor),
                    (int)(r.r_enfermos / divisor),
                    (int)(r.r_hospitalizados / divisor),
                    (int)(r.r_infectados_graves / divisor),
                    (int)(r.r_curados_graves / divisor),
                    (int)(r.r_muertos_graves / divisor),
                    r.r_R0_graves / divisor));
            }
            sw.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            sw.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos     Ida     críticos  infectados         R0   Cercania   Clusters Infectados    Curados    Muertos  Indirecto   Enfermos Hospitaliz Infectados    Curados    Muertos         R0");
            sw.WriteLine();
            string tiempo;
            if (cancelar)
            {
                tiempo = string.Format("Simulación [{0}]. Cancelada. {1:f1} s", CARPETA, dt.TotalMilliseconds / 1000.0d);
            }
            else
            {
                tiempo = string.Format("Simulación [{0}]. Terminada. {1:f1} s", CARPETA, dt.TotalMilliseconds / 1000.0d);

            }
            sw.WriteLine(tiempo);
            sw.WriteLine();
            sw.Close();
            string fichero_t = string.Format(@"{0}\st.log", carpeta);
            FileStream fw_t = new FileStream(fichero_t, FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw_t = new StreamWriter(fw_t);
            ResTipo rt;
            long r_sanos;
            long r_infectados;
            long r_importados;
            long r_curados;
            long r_desinmunizados;
            long r_muertos;
            long r_infectados_indirectos;
            long r_enfermos;
            long r_hospitalizados;
            for (int j = 0; j < NUM_TIPOS; j++)
            {
                sw_t.WriteLine(string.Format("Tipo {0}", j));
                sw_t.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos Inf.Indire   Enfermos Hospitaliz");
                sw_t.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
                for (int i = 0; i < max_dias; i++)
                {
                    r_sanos = 0;
                    r_infectados = 0;
                    r_importados = 0;
                    r_curados = 0;
                    r_desinmunizados = 0;
                    r_muertos = 0;
                    r_infectados_indirectos = 0;
                    r_enfermos = 0;
                    r_hospitalizados = 0;
                    for (int k = 0; k < NUM_HILOS; k++)
                    {
                        if (restipos[k, j] == null || restipos[k, j].Count <= i) break;
                        rt = restipos[k, j].ElementAt(i);
                        r_sanos += rt.r_sanos;
                        r_infectados += rt.r_infectados;
                        r_importados += rt.r_importados;
                        r_curados += rt.r_curados;
                        r_desinmunizados += rt.r_desinmunizados;
                        r_muertos += rt.r_muertos;
                        r_infectados_indirectos += rt.r_infectados_indirectos;
                        r_enfermos += rt.r_enfermos;
                        r_hospitalizados += rt.r_hospitalizados;
                    }
                    sw_t.WriteLine(string.Format("{0,3} {1,10} {2,10} {3,10} {4,10} {5,10} {6,10} {7,10} {8,10} {9,10}",
                        i + 1,
                        (long)(r_sanos / NUM_HILOS),
                        (long)(r_infectados / NUM_HILOS),
                        (long)(r_importados / NUM_HILOS),
                        (long)(r_curados / NUM_HILOS),
                        (long)(r_desinmunizados / NUM_HILOS),
                        (long)(r_muertos / NUM_HILOS),
                        (long)(r_infectados_indirectos / NUM_HILOS),
                        (long)(r_enfermos / NUM_HILOS),
                        (long)(r_hospitalizados / NUM_HILOS)
                        ));
                }
                sw_t.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
                sw_t.WriteLine();
            }
            sw_t.Close();
            MessageBox.Show(tiempo, CARPETA, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private int ContagiosVecinos(int hilo)
        {
            /*
             * Devuelve:
             *  0 Normal
             * -1 Cancelado
             */

            if (cancelar)
            {
                return -1;
            }

            // Excluiremos a los curados y muertos de caminar

            // Para acelerar el cálculo, se construye una lista con los individuos activos (sanos + desinmunizados + infectados)

            List<Individuo> indi_activos = new List<Individuo>();
            SortedList<int, int> puntero_activos = new SortedList<int, int>();
            foreach (Individuo indi_aux in individuos[hilo])
            {
                if (cancelar)
                {
                    return -1;
                }
                if (indi_aux.estado == 2 || indi_aux.estado == -1)
                {
                    // 'indi' Curado o Muerto

                    continue;
                }
                puntero_activos.Add(indi_aux.ID, indi_activos.Count);
                indi_activos.Add(indi_aux);
            }

            // Caminan todos menos los curados y los muertos

            long pendientes_de_volver = 0;
            for (int i = 0; i < NUM_TIPOS; i++)
            {
                pendientes_de_volver += sanos[hilo, i] + desinmunizados[hilo, i] + infectados[hilo, i];
            }
            while (true)
            {
                // Caminos de ida y vuelta
                // Los individuos dan un paso tras otro, hasta que no quede ninguno 'pendiente' de volver al punto de partida

                foreach (Individuo indi in indi_activos)
                {
                    if (cancelar)
                    {
                        return -1;
                    }
                    if (indi.sentido == 0)
                    {
                        // Ida

                        if (indi.pasos_dados == 0)
                        {
                            // Primer paso

                            if (hiloAzar[hilo].NextDouble() >= 0.5)
                            {
                                indi.incx = LON_PASO2 * hiloAzar[hilo].NextDouble();
                            }
                            else
                            {
                                indi.incx = -LON_PASO2 * hiloAzar[hilo].NextDouble();
                            }
                            if (hiloAzar[hilo].NextDouble() >= 0.5)
                            {
                                indi.incy = LON_PASO2 * hiloAzar[hilo].NextDouble();
                            }
                            else
                            {
                                indi.incy = -LON_PASO2 * hiloAzar[hilo].NextDouble();
                            }
                            indi.incd = Math.Sqrt(indi.incx * indi.incx + indi.incy * indi.incy);
                        }

                        // Resto de pasos. Se repite el primer paso

                        indi.x += indi.incx;
                        indi.y += indi.incy;
                        indi.pasos[indi.pasos_dados, 0] = indi.x;
                        indi.pasos[indi.pasos_dados, 1] = indi.y;
                        total_pasos[hilo]++;
                        total_pasos_g[hilo, indi.grupo_ID]++;
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

                            total_recorridos[hilo]++;
                            total_recorridos_g[hilo, indi.grupo_ID]++;
                            total_distancias_ida[hilo] += indi.distancia;
                            total_distancias_ida_g[hilo, indi.grupo_ID] += indi.distancia;
                            indi.x = indi.xi;
                            indi.y = indi.yi;
                            pendientes_de_volver--;

                            // En los próximos pasos no se moverá

                            indi.sentido = -1;
                        }
                        else
                        {
                            indi.x = indi.pasos[indi.pasos_dados, 0];
                            indi.y = indi.pasos[indi.pasos_dados, 1];
                        }
                    }
                }

                // Todos los individuos (pendientes) que no habían vuelto a casa han dado un paso

                // Determinar los contactos de riesgo en función de la distancia entre individuos

                double x;
                double y;
                double d;
                int i;
                int j;
                Grupo g_indi_i;
                Grupo g_indi_j;
                Individuo indi_j;
                double p_contagio_de_i;
                double p_contagio_de_j;
                double p_contagio_de_i_a_j;
                double p_contagio_de_j_a_i;
                double p_gravedad_i;
                double p_gravedad_j;
                foreach (Individuo indi_i in indi_activos)
                {
                    if (cancelar)
                    {
                        return -1;
                    }
                    i = indi_i.ID;
                    g_indi_i = grupos_hilo[hilo].ElementAt(indi_i.grupo_ID);
                    p_contagio_de_i = g_indi_i.prob_contagio;
                    p_gravedad_i = g_indi_i.gravedad;
                    if (indi_i.hospitalizado)
                    {
                        p_contagio_de_i_a_j = F_PROB_HOSPITALIZADO;
                    }
                    else if (indi_i.enfermo)
                    {
                        p_contagio_de_i_a_j = F_PROB_ENFERMO;
                    }
                    else
                    {
                        p_contagio_de_i_a_j = 1;
                    }
                    for (int cj = 0; cj < nvecinas[0, indi_i.ID]; cj++)
                    {
                        if (!puntero_activos.TryGetValue(vecinas[0, i, cj], out j)) continue;
                        indi_j = indi_activos.ElementAt(j);
                        if (indi_j.ID == indi_i.ID) continue;
                        x = indi_i.x - indi_j.x;
                        y = indi_i.y - indi_j.y;
                        d = x * x + y * y;
                        if (d < CONTACTO2)
                        {
                            g_indi_j = grupos_hilo[hilo].ElementAt(indi_j.grupo_ID);
                            p_contagio_de_j = g_indi_j.prob_contagio;
                            p_gravedad_j = g_indi_j.gravedad;
                            if (indi_j.hospitalizado)
                            {
                                p_contagio_de_j_a_i = F_PROB_HOSPITALIZADO;
                            }
                            else if (indi_j.enfermo)
                            {
                                p_contagio_de_j_a_i = F_PROB_ENFERMO;
                            }
                            else
                            {
                                p_contagio_de_j_a_i = 1;
                            }

                            // Dentro de la distancia de contacto

                            if ((indi_i.estado == 0 || indi_i.estado == 3) && indi_j.estado == 1 && indi_j.dias_infectado >= CARENCIA_CONTAGIAR)
                            {
                                // Posible contagio de 'indi_i' por 'indi_j'. Probabilidad de que 'indi_j' le contagie por su propensión alcontagio

                                total_contactos_de_riesgo[hilo]++;
                                total_contactos_de_riesgo_g[hilo, indi_i.grupo_ID]++;
                                if (hiloAzar[hilo].NextDouble() < p_contagio_de_j_a_i * p_contagio_de_i * PROB_CONTAGIO)
                                {
                                    // 'indi_i' infectado 

                                    if (indi_i.estado == 0)
                                    {
                                        sanos[hilo, g_indi_i.tipo]--;
                                    }
                                    else
                                    {
                                        desinmunizados[hilo, g_indi_i.tipo]--;
                                    }
                                    indi_i.estado = 1;
                                    infectados[hilo, g_indi_i.tipo]++;
                                    infectados_grupo[hilo, indi_i.grupo_ID]++;

                                    // Una muesca más en la culata de 'indi_j'

                                    indi_j.indi_enfermados++;
                                    contagios_cercania[hilo]++;

                                    // Gravedad

                                    if (hiloAzar[hilo].NextDouble() < p_gravedad_i)
                                    {
                                        indi_i.grave = true;
                                        infectados_graves[hilo, g_indi_i.tipo]++;
                                    }
                                }
                            }
                            else if (indi_i.estado == 1 && indi_i.dias_infectado >= CARENCIA_CONTAGIAR && (indi_j.estado == 0 || indi_j.estado == 3))
                            {
                                // Posible contagio de 'indi_j' por 'indi_i'. Probabilidad de que 'indi_i' le contagie por su propensión alcontagio

                                total_contactos_de_riesgo[hilo]++;
                                total_contactos_de_riesgo_g[hilo, indi_j.grupo_ID]++;
                                if (hiloAzar[hilo].NextDouble() < p_contagio_de_i_a_j * p_contagio_de_j * PROB_CONTAGIO)
                                {
                                    // 'indi_j' infectado 

                                    if (indi_j.estado == 0)
                                    {
                                        sanos[hilo, g_indi_j.tipo]--;
                                    }
                                    else
                                    {
                                        desinmunizados[hilo, g_indi_j.tipo]--;
                                    }
                                    indi_j.estado = 1;
                                    infectados[hilo, g_indi_j.tipo]++;
                                    infectados_grupo[hilo, indi_j.grupo_ID]++;

                                    // Una muesca más en la culata de 'indi_i'

                                    indi_i.indi_enfermados++;
                                    contagios_cercania[hilo]++;

                                    // Gravedad

                                    if (hiloAzar[hilo].NextDouble() < p_gravedad_j)
                                    {
                                        indi_j.grave = true;
                                        infectados_graves[hilo, g_indi_j.tipo]++;
                                    }
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
        private int ContagiosCluster(int hilo)
        {
            Grupo g_indi_i;
            Grupo g_indi_j;
            Individuo indi_i;
            Individuo indi_j;
            double p_contagio_de_i;
            double p_contagio_de_j;
            double p_contagio_de_i_a_j;
            double p_contagio_de_j_a_i;
            double p_gravedad_i;
            double p_gravedad_j;
            for (int n = 0; n < NUMERO_CLUSTERS[hilo]; n++)
            {
                for (int i = 0; i < individuos_cluster[hilo, n].Count; i++)
                {
                    indi_i = individuos_cluster[hilo, n].ElementAt(i);
                    g_indi_i = grupos_hilo[hilo].ElementAt(indi_i.grupo_ID);
                    p_contagio_de_i = g_indi_i.prob_contagio;
                    p_gravedad_i = g_indi_i.gravedad;
                    if (indi_i.hospitalizado)
                    {
                        p_contagio_de_i_a_j = F_PROB_HOSPITALIZADO;
                    }
                    else if (indi_i.enfermo)
                    {
                        p_contagio_de_i_a_j = F_PROB_ENFERMO;
                    }
                    else
                    {
                        p_contagio_de_i_a_j = 1;
                    }
                    for (int j = 0; j < individuos_cluster[hilo, n].Count; j++)
                    {
                        if (i == j) continue;
                        indi_j = individuos_cluster[hilo, n].ElementAt(j);
                        g_indi_j = grupos_hilo[hilo].ElementAt(indi_j.grupo_ID);
                        p_contagio_de_j = g_indi_j.prob_contagio;
                        p_gravedad_j = g_indi_j.gravedad;
                        if (indi_j.hospitalizado)
                        {
                            p_contagio_de_j_a_i = F_PROB_HOSPITALIZADO;
                        }
                        else if (indi_j.enfermo)
                        {
                            p_contagio_de_j_a_i = F_PROB_ENFERMO;
                        }
                        else
                        {
                            p_contagio_de_j_a_i = 1;
                        }
                        if ((indi_i.estado == 0 || indi_i.estado == 3) && indi_j.estado == 1 && indi_j.dias_infectado >= CARENCIA_CONTAGIAR)
                        {
                            // Posible contagio de 'i' por 'j'. Probabilidad de que 'j' le contagie por su propensión al contagio

                            intentos_cluster[hilo, n]++;
                            if (hiloAzar[hilo].NextDouble() < p_contagio_de_j_a_i * p_contagio_de_i * PROB_CONTAGIO_Cd2)
                            {
                                // 'i' infectado 

                                if (indi_i.estado == 0)
                                {
                                    sanos[hilo, g_indi_i.tipo]--;
                                }
                                else
                                {
                                    desinmunizados[hilo, g_indi_i.tipo]--;
                                }
                                indi_i.estado = 1;
                                infectados[hilo, g_indi_i.tipo]++;
                                infectados_grupo[hilo, indi_i.grupo_ID]++;

                                // 'i' se ha contagiado en el cluster 'n'

                                indi_i.contagio_en_cluster = true;
                                infectados_cluster[hilo, n]++;
                                infectados_total_clusters[hilo]++;

                                // Una muesca más en la culata de 'j'

                                indi_j.indi_enfermados++;
                                contagios_clusters[hilo]++;

                                // Gravedad

                                if (hiloAzar[hilo].NextDouble() < p_gravedad_i)
                                {
                                    indi_i.grave = true;
                                    infectados_graves[hilo, g_indi_i.tipo]++;
                                }
                            }
                        }
                        else if (indi_i.estado == 1 && indi_i.dias_infectado >= CARENCIA_CONTAGIAR && (indi_j.estado == 0 || indi_j.estado == 3))
                        {
                            // Posible contagio de 'j' por 'i'. Probabilidad de que 'i' le contagie por su propensión alcontagio

                            intentos_cluster[hilo, n]++;
                            if (hiloAzar[hilo].NextDouble() < p_contagio_de_i_a_j * p_contagio_de_j * PROB_CONTAGIO_Cd2)
                            {
                                // 'j' infectado 

                                if (indi_j.estado == 0)
                                {
                                    sanos[hilo, g_indi_j.tipo]--;
                                }
                                else
                                {
                                    desinmunizados[hilo, g_indi_j.tipo]--;
                                }
                                indi_j.estado = 1;
                                infectados[hilo, g_indi_j.tipo]++;
                                infectados_grupo[hilo, indi_j.grupo_ID]++;

                                // 'j' se ha contagiado en el cluster 'n'

                                indi_j.contagio_en_cluster = true;
                                infectados_cluster[hilo, n]++;
                                infectados_total_clusters[hilo]++;

                                // Una muesca más en la culata de 'i'

                                indi_i.indi_enfermados++;
                                contagios_clusters[hilo]++;

                                // Gravedad

                                if (hiloAzar[hilo].NextDouble() < p_gravedad_j)
                                {
                                    indi_j.grave = true;
                                    infectados_graves[hilo, g_indi_j.tipo]++;
                                }
                            }
                        }
                    }
                }
            }
            return 0;
        }
        private int ContagiosIndirectos(int hilo)
        {
            double den;
            Individuo indi;
            Grupo g;
            double p_gravedad;
            for (int n = 0; n < individuos[hilo].Count; n++)
            {
                den = DensidadVecinos(n, 0);
                if (den > DENSIDAD_VECINOS)
                {
                    indi = individuos[hilo].ElementAt(n);
                    g = grupos_hilo[hilo].ElementAt(indi.grupo_ID);
                    p_gravedad = g.gravedad;
                    if (indi.estado == 0 || indi.estado == 3)
                    {
                        if (hiloAzar[hilo].NextDouble() < PROB_INDIRECTA)
                        {
                            // Infectado 

                            infectados_indirectos[hilo, g.tipo]++;
                            if (indi.estado == 0)
                            {
                                sanos[hilo, g.tipo]--;
                            }
                            else
                            {
                                desinmunizados[hilo, g.tipo]--;
                            }
                            indi.estado = 1;
                            infectados[hilo, g.tipo]++;
                            infectados_grupo[hilo, indi.grupo_ID]++;

                            // Gravedad

                            if (hiloAzar[hilo].NextDouble() < p_gravedad)
                            {
                                indi.grave = true;
                                infectados_graves[hilo, g.tipo]++;
                            }
                        }
                    }
                }
            }
            return 0;
        }
        private void B_cancelar_Click(object sender, EventArgs e)
        {
            cancelar = true;
            linea_estado.Text = "Cancelando ...";
            b_cancelar.Enabled = false;
            Application.DoEvents();
        }
        private double DensidadVecinos(int n, int hilo)
        {
            if (nvecinas[0, n] == 0) return 0;
            Individuo indi;
            int vinf = 0;
            for (int i = 0; i < nvecinas[0, n]; i++)
            {
                indi = individuos[hilo].ElementAt(vecinas[0, n, i]);
                if (indi.estado == 1 && indi.dias_infectado >= CARENCIA_CONTAGIAR)
                {
                    vinf++;
                }
            }
            return (double)vinf / nvecinas[0, n];
        }
        #endregion

        #region Individuos
        private void B_sel_individuos_Click(object sender, EventArgs e)
        {
            OpenFileDialog leefichero = new OpenFileDialog
            {
                Filter = "III (*.iii)|*.iii|All files (*.*)|*.*",
                CheckFileExists = false,
                Multiselect = false
            };
            if (leefichero.ShowDialog() == DialogResult.OK)
            {
                senda_individuos.Text = F_INDIVIDUOS = leefichero.FileName;
            }
            else
            {
                senda_individuos.Text = F_INDIVIDUOS = string.Empty;
            }
        }
        private bool ImportaPoblacion(string fichero, int hilo)
        {
            string s;
            string[] sd;
            FileStream fr;
            StreamReader sr;
            if (string.IsNullOrEmpty(fichero) || !File.Exists(fichero))
            {
                MessageBox.Show("No se encuentra el fichero " + fichero, "Importar población", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            try
            {
                fr = new FileStream(fichero, FileMode.Open, FileAccess.Read, FileShare.Read);
                sr = new StreamReader(fr);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al leer el fichero " + fichero + Environment.NewLine + e.Message, "Importar población", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            int tipo;
            int grupo_id;
            double xi;
            double yi;
            int estado;
            int dias_infectado;
            int dias_curado;
            int indi_enfermados;
            int cluster;
            int contagiado_en_cluster;
            int enfermo;
            int hospitalizado;
            int grave;
            individuos[hilo] = new List<Individuo>();
            infectados_grupo = new long[NUM_HILOS, MAX_GRUPOS];
            for (int i = 0; i < MAX_GRUPOS; i++)
            {
                infectados_grupo[hilo, i] = 0;
            }
            for (int j = 0; j < MAX_TIPOS; j++)
            {
                for (int i = 0; i < NUM_HILOS; i++)
                {
                    sanos[i, j] = 0;
                    infectados[i, j] = 0;
                    curados[i, j] = 0;
                    desinmunizados[i, j] = 0;
                    muertos[i, j] = 0;
                    enfermos[i, j] = 0;
                    hospitalizados[i, j] = 0;

                    infectados_graves[i, j] = 0;
                    curados_graves[i, j] = 0;
                    muertos_graves[i, j] = 0;
                }
            }

            // Primero los individuos

            INDIVIDUOS = Convert.ToInt64(sr.ReadLine());
            d_individuos.Text = INDIVIDUOS.ToString();
            for (int i = 0; i < INDIVIDUOS; i++)
            {
                s = sr.ReadLine();
                sd = s.Split(';');
                if (sd.Length != 13)
                {
                    MessageBox.Show(string.Format("Número incorrecto de campos: {0} {1}", sd.Length, s), "Importar individuos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (sr != null) sr.Close();
                    return false;
                }
                tipo = Convert.ToInt32(sd[0]);
                grupo_id = Convert.ToInt32(sd[1]);
                xi = Convert.ToDouble(sd[3]);
                yi = Convert.ToDouble(sd[4]);
                estado = Convert.ToInt32(sd[5]);
                dias_infectado = Convert.ToInt32(sd[6]);
                dias_curado = Convert.ToInt32(sd[7]);
                indi_enfermados = Convert.ToInt32(sd[8]);
                cluster = Convert.ToInt32(sd[9]);
                contagiado_en_cluster = Convert.ToInt32(sd[10]);
                enfermo = Convert.ToInt32(sd[11]);
                hospitalizado = Convert.ToInt32(sd[12]);
                grave = Convert.ToInt32(sd[13]);

                individuos[hilo].Add(new Individuo(individuos[hilo].Count, tipo, grupo_id, sd[2], xi, yi, estado, dias_infectado, dias_curado, indi_enfermados, cluster, contagiado_en_cluster == 1 ? true : false, enfermo == 1 ? true : false, hospitalizado == 1 ? true : false, grave == 1 ? true : false));
                switch (estado)
                {
                    case 0:
                        sanos[hilo, tipo]++;
                        break;
                    case 1:
                        infectados[hilo, tipo]++;
                        infectados_grupo[hilo, grupo_id]++;
                        break;
                    case 2:
                        curados[hilo, tipo]++;
                        break;
                    case 3:
                        desinmunizados[hilo, tipo]++;
                        break;
                    default:
                        muertos[hilo, tipo]++;
                        break;
                }
                if (grave == 1)
                {
                    switch (estado)
                    {
                        case 1:
                            infectados_graves[hilo, tipo]++;
                            break;
                        case 2:
                            curados_graves[hilo, tipo]++;
                            break;
                        case -1:
                            muertos_graves[hilo, tipo]++;
                            break;
                    }
                }
                if (enfermo == 1) enfermos[hilo, tipo]++;
                if (hospitalizado == 1) hospitalizados[hilo, tipo]++;
            }

            // Ahora los clusters

            NUMERO_CLUSTERS[hilo] = Convert.ToInt32(sr.ReadLine());
            individuos_cluster = new List<Individuo>[NUM_HILOS, NUMERO_CLUSTERS[hilo]];
            int n;
            for (int i = 0; i < NUMERO_CLUSTERS[hilo]; i++)
            {
                s = sr.ReadLine();
                sd = s.Split(';');
                if (sd.Length != 2)
                {
                    MessageBox.Show(string.Format("Número incorrecto de campos: {0} {1}", sd.Length, s), "Importar individuos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (sr != null) sr.Close();
                    return false;
                }
                n = Convert.ToInt32(sd[1]);
                individuos_cluster[hilo, i] = new List<Individuo>();
                for (int j = 0; j < n; j++)
                {
                    s = sr.ReadLine();
                    individuos_cluster[hilo, i].Add(individuos[hilo].ElementAt(Convert.ToInt32(s)));
                }
            }
            sr.Close();
            CasoCambiado();
            if (individuos[hilo].Count == 0)
            {
                MessageBox.Show("No se ha importado ningún individuo", "Importar individuos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string sal = string.Format("Importados {0:N0} individuos", individuos[hilo].Count);
                long[] sumas = new long[4];
                for (int i = 0; i < sumas.Length; i++)
                {
                    sumas[i] = 0;
                }
                for (int i = 0; i < NUM_TIPOS; i++)
                {
                    sumas[0] += sanos[hilo, i];
                    sumas[1] += infectados[hilo, i];
                    sumas[2] += curados[hilo, i];
                    sumas[3] += muertos[hilo, i];
                }
                sal += Environment.NewLine + string.Format("Sanos      {0:N0}", sumas[0]);
                sal += Environment.NewLine + string.Format("Infectados {0:N0}", sumas[1]);
                sal += Environment.NewLine + string.Format("Curados    {0:N0}", sumas[2]);
                sal += Environment.NewLine + string.Format("Muertos    {0:N0}", sumas[3]);
                MessageBox.Show(sal, "Importar individuos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (individuos[hilo].Count < MAX_INDIVIDUOS_PARA_TABLA)
                {
                    cancelar = false;
                    b_cancelar.Enabled = true;
                    if (cancelar || !TablaVecinos())
                    {
                    }
                    b_cancelar.Enabled = false;
                }
            }
            return true;
        }
        private void ExportarPoblacion(string fichero, int hilo)
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

            // Primero los individuos

            sw.WriteLine(string.Format("{0}", individuos[hilo].Count));
            foreach (Individuo p in individuos[hilo])
            {
                sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{11};{12}", p.tipo, p.grupo_ID, p.grupo, p.xi, p.yi, p.estado, p.dias_infectado, p.dias_curado, p.indi_enfermados, p.cluster, p.contagio_en_cluster ? 1 : 0, p.enfermo ? 1 : 0, p.hospitalizado ? 1 : 0, p.grave ? 1 : 0));
            }

            // Luego los clusters

            sw.WriteLine(string.Format("{0}", NUMERO_CLUSTERS[hilo]));
            for (int i = 0; i < NUMERO_CLUSTERS[hilo]; i++)
            {
                int ng = individuos_cluster[hilo, i].ElementAt(0).grupo_ID;
                sw.WriteLine(string.Format("{0};{1}", ng, individuos_cluster[hilo, i].Count));
                for (int j = 0; j < individuos_cluster[hilo, i].Count; j++)
                {
                    sw.WriteLine(string.Format("{0}", individuos_cluster[hilo, i].ElementAt(j).ID));
                }
            }
            sw.Close();
        }
        #endregion
    }
}
