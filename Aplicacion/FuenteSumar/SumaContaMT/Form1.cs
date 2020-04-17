using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SumaContaMT
{
    public partial class Form1 : Form
    {
        int minimo;
        int maximo;
        double veces;
        double media_des;
        double[] suma_des;
        double des_max;
        public class Resultado
        {
            public int dia;
            public long sanos;
            public long infectados;
            public long importados;
            public long curados;
            public long desinmunizados;
            public long muertos;
            public double recorrido_individuo_activo;
            public double contactos_de_riesgo;
            public double media_dias_infectado;
            public double R0;
            public long contagios_cercania;
            public long contagios_clusters;
            public long infectados_total_clusters;
            public long curados_total_clusters;
            public long muertos_total_clusters;
            public long infectados_indirectos;
            public long enfermos;
            public long hospitalizados;
            public long infectadosb;
            public long curadosb;
            public long muertosb;
            public double R0_graves;
            public int series;
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
                long infectadosb,
                long curadosb,
                long muertosb,
                double R0_graves,
                int series)
            {
                this.dia = dia;
                this.sanos = sanos;
                this.infectados = infectados;
                this.importados = importados;
                this.curados = curados;
                this.desinmunizados = desinmunizados;
                this.muertos = muertos;
                this.recorrido_individuo_activo = recorrido_individuo_activo;
                this.contactos_de_riesgo = contactos_de_riesgo;
                this.media_dias_infectado = media_dias_infectado;
                this.R0 = R0;
                this.contagios_cercania = contagios_cercania;
                this.contagios_clusters = contagios_clusters;
                this.infectados_total_clusters = infectados_total_clusters;
                this.curados_total_clusters = curados_total_clusters;
                this.muertos_total_clusters = muertos_total_clusters;
                this.infectados_indirectos = infectados_indirectos;
                this.enfermos = enfermos;
                this.hospitalizados = hospitalizados;
                this.infectadosb = infectadosb;
                this.curadosb = curadosb;
                this.muertosb = muertosb;
                this.R0_graves = R0_graves;
                this.series = series;
            }
        }
        public class ResTipo
        {
            public int dia;
            public long sanos;
            public long infectados;
            public long importados;
            public long curados;
            public long desinmunizados;
            public long muertos;
            public long infectados_indirectos;
            public long enfermos;
            public long hospitalizados;
            public int series;
            public ResTipo(
                int dia,
                long sanos,
                long infectados,
                long importados,
                long curados,
                long desinmunizados,
                long muertos,
                long infectados_indirectos,
                long enfermos,
                long hospitalizados,
                int series)
            {
                this.dia = dia;
                this.sanos = sanos;
                this.infectados = infectados;
                this.importados = importados;
                this.curados = curados;
                this.desinmunizados = desinmunizados;
                this.muertos = muertos;
                this.infectados_indirectos = infectados_indirectos;
                this.enfermos = enfermos;
                this.hospitalizados = hospitalizados;
                this.series = series;
            }
        }
        bool cancelar;
        bool suma_tipos;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Text = string.Format("Suma salidas Contagios y ContaMTx v:{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
        private void Borra_fuentes_Click(object sender, EventArgs e)
        {
            lista.Items.Clear();
            cuenta.Text = lista.Items.Count.ToString();
        }
        private void Sel_fuente_Click(object sender, EventArgs e)
        {
            OpenFileDialog leefichero = new OpenFileDialog
            {
                Filter = "LOG (*.log)|*.log|All files (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = true
            };
            if (leefichero.ShowDialog() == DialogResult.OK)
            {
                lista.Items.Clear();
                foreach (string fichero in leefichero.FileNames)
                {
                    lista.Items.Add(fichero);
                }
                cuenta.Text = lista.Items.Count.ToString();
            }
        }
        private void Add_fuentes_Click(object sender, EventArgs e)
        {
            OpenFileDialog leefichero = new OpenFileDialog
            {
                Filter = "LOG (*.log)|*.log|All files (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = true
            };
            if (leefichero.ShowDialog() == DialogResult.OK)
            {
                foreach (string fichero in leefichero.FileNames)
                {
                    lista.Items.Add(fichero);
                }
                cuenta.Text = lista.Items.Count.ToString();
            }
        }
        private void Sel_salida_Click(object sender, EventArgs e)
        {
            SaveFileDialog ficheroescritura = new SaveFileDialog()
            {
                Filter = "TXT (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1
            };
            ficheroescritura.RestoreDirectory = ficheroescritura.OverwritePrompt = false;
            ficheroescritura.CheckPathExists = true;
            if (ficheroescritura.ShowDialog() == DialogResult.OK)
            {
                senda_salida.Text = ficheroescritura.FileName;
            }
        }
        private void B_ejecuta_Click(object sender, EventArgs e)
        {
            int fuentes = lista.Items.Count;
            if (fuentes == 0)
            {
                MessageBox.Show("No hay fuentes", "Sumar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (senda_salida.Text.Trim().Length == 0)
            {
                MessageBox.Show("No hay fichero de salida", "Sumar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            minimo = Convert.ToInt32(dias_min.Text);
            maximo = Convert.ToInt32(dias_max.Text);
            if (maximo == 0) maximo = 9999;
            if (maximo < minimo)
            {
                MessageBox.Show("El máximo debe ser igual o mayor al mínimo", "Sumar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            veces = Convert.ToDouble(des_maxima.Text);
            int hasta_dia = Convert.ToInt32(hasta.Text);
            cancelar = false;
            string fichero;
            List<Resultado>[] resultados = new List<Resultado>[fuentes];
            List<ResTipo>[][] restipos = new List<ResTipo>[fuentes][];
            int n = 0;
            for (int hilo = 0; hilo < fuentes; hilo++)
            {
                fichero = lista.Items[hilo].ToString();
                resultados[n] = ImportaResultado(fichero, hasta_dia);
                restipos[n] = ImportaResTipo(fichero, hasta_dia);
                if (cancelar) break;
                if (resultados[n] != null && resultados[n].Count >= minimo && resultados[n].Count <= maximo)
                {
                    n++;
                }
            }
            if (cancelar)
            {
                MessageBox.Show("Proceso cancelado", "Sumar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                List<Resultado> media = Media(resultados);
                List<ResTipo>[] mediatipos;
                if (suma_tipos)
                {
                    mediatipos = MediaTipos(restipos);
                }
                else
                {
                    mediatipos = null;
                }
                int ns;
                if (veces > 0)
                {
                    List<ResTipo>[][] tiposdepurados;
                    if (suma_tipos)
                    {
                        tiposdepurados = new List<ResTipo>[restipos.Length][];
                        for (int k = 0; k < restipos.Length; k++)
                        {
                            tiposdepurados[k] = new List<ResTipo>[restipos[0].Length];
                        }
                    }
                    else
                    {
                        tiposdepurados = null;
                    }
                    List<Resultado>[] resdepurados = Elimina(veces, media, resultados, restipos, ref tiposdepurados);
                    media = Media(resdepurados);
                    if (suma_tipos)
                    {
                        mediatipos = MediaTipos(tiposdepurados);
                    }
                    ns = 0;
                    for (int i = 0; i < resdepurados.Length; i++)
                    {
                        if (resdepurados[i] != null && resdepurados[i].Count > 0)
                        {
                            ns++;
                        }
                    }
                }
                else
                {
                    ns = 0;
                    for (int i = 0; i < resultados.Length; i++)
                    {
                        if (resultados[i] != null && resultados[i].Count > 0)
                        {
                            ns++;
                        }
                    }
                }
                SalidaValoresMedios(senda_salida.Text, resultados, media, mediatipos);
                MessageBox.Show(string.Format("Sumados {0} ficheros", ns), "Sumar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private List<Resultado>[] Elimina(double veces, List<Resultado> media, List<Resultado>[] depuradosres, List<ResTipo>[][] restipos, ref List<ResTipo>[][] depuradostipos)
        {
            List<Resultado>[] nuevos = new List<Resultado>[depuradosres.Length];
            double des;
            media_des = 0;
            suma_des = new double[depuradosres.Length];
            Resultado rm;
            Resultado rr;
            for (int fichero = 0; fichero < depuradosres.Length; fichero++)
            {
                suma_des[fichero] = 0;
                for (int dia = 0; dia < media.Count; dia++)
                {
                    rm = media.ElementAt(dia);
                    if (depuradosres[fichero].Count <= dia)
                    {
                        des = rm.infectados;
                    }
                    else
                    {
                        rr = depuradosres[fichero].ElementAt(dia);
                        des = rm.infectados - rr.infectados;
                    }
                    suma_des[fichero] += des * des;
                }
                suma_des[fichero] = Math.Sqrt(suma_des[fichero]) / media.Count;
                media_des += suma_des[fichero];
            }
            media_des /= depuradosres.Length;
            des_max = media_des * veces;
            int n = 0;
            for (int fichero = 0; fichero < depuradosres.Length; fichero++)
            {
                if (suma_des[fichero] < des_max)
                {
                    nuevos[n] = depuradosres[fichero];
                    if (suma_tipos) depuradostipos[n] = restipos[fichero];
                    n++;
                }
            }

            // Borra el resto

            for (int i = n; i < depuradosres.Length; i++)
            {
                nuevos[i] = null;
                if (suma_tipos) depuradostipos[i] = null;
            }
            return nuevos;
        }
        private List<Resultado> ImportaResultado(string fichero, int hasta)
        {
            string s;
            string[] sd;
            FileStream fr;
            StreamReader sr;
            if (string.IsNullOrEmpty(fichero) || !File.Exists(fichero))
            {
                MessageBox.Show("No se encuentra el fichero " + fichero, "Importar resultados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cancelar = true;
                return null;
            }
            try
            {
                fr = new FileStream(fichero, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                sr = new StreamReader(fr);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al leer el fichero " + fichero + Environment.NewLine + e.Message, "Importar resultados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cancelar = true;
                return null;
            }
            List<Resultado> resultado = new List<Resultado>();
            int cuenta_lineas = 0;
            int primera_linea = -1;
            int series = 0;
            while (true && !sr.EndOfStream)
            {
                s = sr.ReadLine();
                cuenta_lineas++;
                if (s.Equals("Valores al final del día"))
                {
                    series++;
                    primera_linea = cuenta_lineas;
                }
            }
            if (primera_linea == -1)
            {
                return null;
            }
            sr.Close();
            fr = new FileStream(fichero, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            sr = new StreamReader(fr);

            // Saltar líneas

            for (int i = 0; i < primera_linea; i++)
            {
                sr.ReadLine();
            }
            sr.ReadLine();
            if (sr.EndOfStream)
            {
                return null;
            }
            sr.ReadLine();
            if (sr.EndOfStream)
            {
                return null;
            }
            sr.ReadLine();
            int dia;
            long sanos;
            long infectados;
            long importados;
            long curados;
            long desinmunizados;
            long muertos;
            double recorrido_individuo_activo;
            double contactos_de_riesgo;
            double media_dias_infectado;
            double R0;
            long contagios_cercania;
            long contagios_clusters;
            long infectados_total_clusters;
            long curados_total_clusters;
            long muertos_total_clusters;
            long infectados_ambientales;
            long enfermos;
            long hospitalizados;
            long infectados_graves;
            long curados_graves;
            long muertos_graves;
            double R0_graves;
            int cuantos;
            while (true && !sr.EndOfStream)
            {
                s = sr.ReadLine().Trim();
                if (s.Length == 0) break;
                if (s.StartsWith("--- ---------- ")) break;
                sd = s.Split(' ');
                cuantos = 0;
                for (int i = 0; i < sd.Length; i++)
                {
                    if (sd[i].Trim().Length > 0)
                    {
                        if (cuantos != i) sd[cuantos] = sd[i];
                        cuantos++;
                    }
                }
                if (cuantos != 23)
                {
                    MessageBox.Show(string.Format("Número incorrecto de campos: {0} {1}", cuantos, s), "Importar resultados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                dia = Convert.ToInt32(sd[0]);
                sanos = Convert.ToInt64(sd[1]);
                infectados = Convert.ToInt64(sd[2]);
                importados = Convert.ToInt64(sd[3]);
                curados = Convert.ToInt64(sd[4]);
                desinmunizados = Convert.ToInt64(sd[5]);
                muertos = Convert.ToInt64(sd[6]);
                recorrido_individuo_activo = Convert.ToDouble(sd[7]);
                contactos_de_riesgo = Convert.ToDouble(sd[8]);
                media_dias_infectado = Convert.ToDouble(sd[9]);
                R0 = Convert.ToDouble(sd[10]);
                contagios_cercania = Convert.ToInt64(sd[11]);
                contagios_clusters = Convert.ToInt64(sd[12]);
                infectados_total_clusters = Convert.ToInt64(sd[13]);
                curados_total_clusters = Convert.ToInt64(sd[14]);
                muertos_total_clusters = Convert.ToInt64(sd[15]);
                infectados_ambientales = Convert.ToInt64(sd[16]);
                enfermos = Convert.ToInt64(sd[17]);
                hospitalizados = Convert.ToInt64(sd[18]);
                infectados_graves = Convert.ToInt64(sd[19]);
                curados_graves = Convert.ToInt64(sd[20]);
                muertos_graves = Convert.ToInt64(sd[21]);
                R0_graves = Convert.ToDouble(sd[22]);
                resultado.Add(new Resultado(dia, sanos, infectados, importados, curados, desinmunizados, muertos, recorrido_individuo_activo, contactos_de_riesgo, media_dias_infectado, R0, contagios_cercania, contagios_clusters, infectados_total_clusters, curados_total_clusters, muertos_total_clusters, infectados_ambientales, enfermos, hospitalizados, infectados_graves, curados_graves, muertos_graves, R0_graves, series));
                if (resultado.Count == hasta) break;
            }
            return resultado;
        }
        private List<ResTipo>[] ImportaResTipo(string fichero, int hasta)
        {
            suma_tipos = true;
            string senda = Path.GetDirectoryName(fichero);
            string fi = "st" + Path.GetFileName(fichero).Substring(1);
            fichero = Path.Combine(senda, fi);
            string s;
            string[] sd;
            FileStream fr;
            StreamReader sr;
            if (string.IsNullOrEmpty(fichero) || !File.Exists(fichero))
            {
                suma_tipos = false;
                return null;
            }
            try
            {
                fr = new FileStream(fichero, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                sr = new StreamReader(fr);
            }
            catch
            {
                suma_tipos = false;
                return null;
            }
            int cuenta_lineas = 0;
            int primera_linea = -1;
            int tipos = 0;
            while (true && !sr.EndOfStream)
            {
                s = sr.ReadLine();
                cuenta_lineas++;
                if (s.StartsWith("Tipo"))
                {
                    if (tipos == 0) primera_linea = cuenta_lineas;
                    tipos++;
                }
            }
            if (primera_linea == -1)
            {
                return null;
            }
            sr.Close();
            List<ResTipo>[] resultado = new List<ResTipo>[tipos];
            for (int i = 0; i < tipos; i++)
            {
                resultado[i] = new List<ResTipo>();
            }
            fr = new FileStream(fichero, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            sr = new StreamReader(fr);

            // Saltar líneas

            for (int i = 0; i < primera_linea; i++)
            {
                sr.ReadLine();
            }
            sr.ReadLine();
            if (sr.EndOfStream)
            {
                return null;
            }
            sr.ReadLine();
            if (sr.EndOfStream)
            {
                return null;
            }
            int dia;
            long sanos;
            long infectados;
            long importados;
            long curados;
            long desinmunizados;
            long muertos;
            long infectados_indirectos;
            long enfermos;
            long hospitalizados;
            int cuantos;
            int tipo = 0;
            while (true && !sr.EndOfStream)
            {
                s = sr.ReadLine().Trim();
                if (s.Length == 0) break;
                if (s.StartsWith("--- ---------- "))
                {
                    tipo++;
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    continue;
                }
                sd = s.Split(' ');
                cuantos = 0;
                for (int i = 0; i < sd.Length; i++)
                {
                    if (sd[i].Trim().Length > 0)
                    {
                        if (cuantos != i) sd[cuantos] = sd[i];
                        cuantos++;
                    }
                }
                if (cuantos != 10)
                {
                    suma_tipos = false;
                    return null;
                }
                dia = Convert.ToInt32(sd[0]);
                sanos = Convert.ToInt64(sd[1]);
                infectados = Convert.ToInt64(sd[2]);
                importados = Convert.ToInt64(sd[3]);
                curados = Convert.ToInt64(sd[4]);
                desinmunizados = Convert.ToInt64(sd[5]);
                muertos = Convert.ToInt64(sd[6]);
                infectados_indirectos = Convert.ToInt64(sd[7]);
                enfermos = Convert.ToInt64(sd[8]);
                hospitalizados = Convert.ToInt64(sd[9]);
                resultado[tipo].Add(new ResTipo(dia, sanos, infectados, importados, curados, desinmunizados, muertos, infectados_indirectos, enfermos, hospitalizados, tipos));
                if (resultado[tipo].Count == hasta) break;
            }
            return resultado;
        }
        private List<Resultado> Media(List<Resultado>[] resultados)
        {
            if (resultados == null || resultados[0] == null) return null;
            int datos;
            long sanos;
            long infectados;
            long importados;
            long curados;
            long desinmunizados;
            long muertos;
            double recorrido_individuo_activo;
            double contactos_de_riesgo;
            double media_dias_infectado;
            double R0;
            long contagios_cercania;
            long contagios_clusters;
            long infectados_total_clusters;
            long curados_total_clusters;
            long muertos_total_clusters;
            long infectados_ambientales;
            long enfermos;
            long hospitalizados;
            long infectados_graves;
            long curados_graves;
            long muertos_graves;
            double R0_graves;
            Resultado r;
            List<Resultado> medio = new List<Resultado>();
            int max_dias = resultados[0].Count;
            for (int i = 0; i < resultados.Length; i++)
            {
                if (resultados[i] != null && max_dias < resultados[i].Count)
                {
                    max_dias = resultados[i].Count;
                }
            }
            for (int i = 0; i < max_dias; i++)
            {
                datos = 0;
                sanos = 0;
                infectados = 0;
                importados = 0;
                curados = 0;
                desinmunizados = 0;
                muertos = 0;
                recorrido_individuo_activo = 0;
                contactos_de_riesgo = 0;
                media_dias_infectado = 0;
                R0 = 0;
                contagios_cercania = 0;
                contagios_clusters = 0;
                infectados_total_clusters = 0;
                curados_total_clusters = 0;
                muertos_total_clusters = 0;
                infectados_ambientales = 0;
                enfermos = 0;
                hospitalizados = 0;
                infectados_graves = 0;
                curados_graves = 0;
                muertos_graves = 0;
                R0_graves = 0;
                for (int n = 0; n < resultados.Length; n++)
                {
                    if (resultados[n] == null || resultados[n].Count <= i) continue;
                    r = resultados[n].ElementAt(i);
                    datos++;
                    sanos += r.sanos;
                    infectados += r.infectados;
                    importados += r.importados;
                    curados += r.curados;
                    desinmunizados += r.desinmunizados;
                    muertos += r.muertos;
                    recorrido_individuo_activo += r.recorrido_individuo_activo;
                    contactos_de_riesgo += r.contactos_de_riesgo;
                    media_dias_infectado += r.media_dias_infectado;
                    R0 += r.R0;
                    contagios_cercania += r.contagios_cercania;
                    contagios_clusters += r.contagios_clusters;
                    infectados_total_clusters += r.infectados_total_clusters;
                    curados_total_clusters += r.curados_total_clusters;
                    muertos_total_clusters += r.muertos_total_clusters;
                    infectados_ambientales += r.infectados_indirectos;
                    enfermos += r.enfermos;
                    hospitalizados += r.hospitalizados;
                    infectados_graves += r.infectadosb;
                    curados_graves += r.curadosb;
                    muertos_graves += r.muertosb;
                    R0_graves += r.R0_graves;
                }
                if (datos == 0)
                {
                    datos = 1;
                }
                medio.Add(new Resultado(
                    datos,
                    (long)(sanos / datos),
                    (long)(infectados / datos),
                    (long)(importados / datos),
                    (long)(curados / datos),
                    (long)(desinmunizados / datos),
                    (long)(muertos / datos),
                    (double)(recorrido_individuo_activo / datos),
                    (double)(contactos_de_riesgo / datos),
                    (double)(media_dias_infectado / datos),
                    (double)(R0 / datos),
                    (long)(contagios_cercania / datos),
                    (long)(contagios_clusters / datos),
                    (long)(infectados_total_clusters / datos),
                    (long)(curados_total_clusters / datos),
                    (long)(muertos_total_clusters / datos),
                    (long)(infectados_ambientales / datos),
                    (long)(enfermos / datos),
                    (long)(hospitalizados / datos),
                    (long)(infectados_graves / datos),
                    (long)(curados_graves / datos),
                    (long)(muertos_graves / datos),
                    (double)(R0_graves / datos),
                    0)
                );
            }
            return medio;
        }
        private List<ResTipo>[] MediaTipos(List<ResTipo>[][] restipos)
        {
            if (restipos == null || restipos[0] == null) return null;
            int datos;
            long sanos;
            long infectados;
            long importados;
            long curados;
            long desinmunizados;
            long muertos;
            long infectados_ambientales;
            long enfermos;
            long hospitalizados;
            ResTipo r;

            // [] Para cada tipo

            List<ResTipo>[] medio = new List<ResTipo>[restipos[0].Length];
            for (int tipo = 0; tipo < restipos[0].Length; tipo++)
            {
                // Una lista para cada tipo

                medio[tipo] = new List<ResTipo>();
            }
            int max_dias = restipos[0][0].Count;
            for (int ficheros = 0; ficheros < restipos.Length; ficheros++)
            {
                // Recorrer todo los ficheros

                if (restipos[ficheros] == null) break;
                for (int tipo = 0; tipo < restipos[ficheros].Length; tipo++)
                {
                    // Recorrer todos los tipos

                    if (restipos[ficheros][tipo] == null) continue;
                    if (max_dias < restipos[ficheros][tipo].Count)
                    {
                        max_dias = restipos[ficheros][tipo].Count;
                    }
                }
            }

            // Se usa el número de tipos del primer fichero

            for (int tipo = 0; tipo < restipos[0].Length; tipo++)
            {
                // Para cada tipo

                for (int dia = 0; dia < max_dias; dia++)
                {
                    // Para cada día, sumar todos los ficheros

                    datos = 0;
                    sanos = 0;
                    infectados = 0;
                    importados = 0;
                    curados = 0;
                    desinmunizados = 0;
                    muertos = 0;
                    infectados_ambientales = 0;
                    enfermos = 0;
                    hospitalizados = 0;
                    for (int ficheros = 0; ficheros < restipos.Length; ficheros++)
                    {
                        // Recorrer los ficheros

                        if (restipos[ficheros] == null)
                        {
                            break;
                        }
                        if (restipos[ficheros][tipo] == null || restipos[ficheros][tipo].Count <= dia)
                        {
                            continue;
                        }
                        r = restipos[ficheros][tipo].ElementAt(dia);
                        datos++;
                        sanos += r.sanos;
                        infectados += r.infectados;
                        importados += r.importados;
                        curados += r.curados;
                        desinmunizados += r.desinmunizados;
                        muertos += r.muertos;
                        infectados_ambientales += r.infectados_indirectos;
                        enfermos += r.enfermos;
                        hospitalizados += r.hospitalizados;
                    }
                    if (datos == 0)
                    {
                        datos = 1;
                    }

                    // Añadir la media del día a este tipo

                    medio[tipo].Add(new ResTipo(
                        datos,
                        (long)(sanos / datos),
                        (long)(infectados / datos),
                        (long)(importados / datos),
                        (long)(curados / datos),
                        (long)(desinmunizados / datos),
                        (long)(muertos / datos),
                        (long)(infectados_ambientales / datos),
                        (long)(enfermos / datos),
                        (long)(hospitalizados / datos),
                        0)
                    );
                }
            }
            return medio;
        }
        private void SalidaValoresMedios(string fichero, List<Resultado>[] resultados, List<Resultado> media, List<ResTipo>[] mediatipos)
        {
            int res = resultados.Length;
            FileStream fw = new FileStream(fichero, FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(fw);
            sw.WriteLine("Fichero                                                      Desv.Media Ser");
            sw.WriteLine("------------------------------------------------------------ ---------- --- --");
            for (int n = 0; n < res; n++)
            {
                sw.WriteLine(string.Format("{0,-60} {1,10:f2} {2,3:N0} {3}", lista.Items[n].ToString(), suma_des[n], resultados[n].ElementAt(0).series, suma_des[n] < des_max ? "ok" : ""));
            }
            sw.WriteLine("------------------------------------------------------------ ---------- --- --");
            sw.WriteLine(string.Format("                                          {0,2:N0} x {1,10:f2}  = {2,10:f2}", (int)veces, media_des, des_max));
            sw.WriteLine("------------------------------------------------------------ ---------- --- --");
            sw.WriteLine();
            sw.WriteLine("                                                                       Recorrido  Contactos Media días            ------ Afectados ---- ----------  Clusters -----------                                  -----------------  GRAVES -----------------");
            sw.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos     Ida     críticos  infectados         R0   Cercania   Clusters Infectados    Curados    Muertos Indirectos   Enfermos Hospitaliz Infectados    Curados    Muertos         R0");
            sw.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            Resultado r;
            for (int n = 0; n < media.Count; n++)
            {
                r = media.ElementAt(n);
                sw.WriteLine(string.Format(
                    "{0,3} {1,10} {2,10} {3,10} {4,10} {5,10} {6,10} {7,10:f2} {8,10:f2} {9,10:f1} {10,10:f2} {11,10} {12,10} {13,10} {14,10} {15,10} {16,10} {17,10} {18,10} {19,10} {20,10} {21,10} {22,10:f2}",
                    n + 1,
                    r.sanos,
                    r.infectados,
                    r.importados,
                    r.curados,
                    r.desinmunizados,
                    r.muertos,
                    r.recorrido_individuo_activo,
                    r.contactos_de_riesgo,
                    r.media_dias_infectado,
                    r.R0,
                    r.contagios_cercania,
                    r.contagios_clusters,
                    r.infectados_total_clusters,
                    r.curados_total_clusters,
                    r.muertos_total_clusters,
                    r.infectados_indirectos,
                    r.enfermos,
                    r.hospitalizados,
                    r.infectadosb,
                    r.curadosb,
                    r.muertosb,
                    r.R0_graves));
            }
            sw.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            sw.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos     Ida     críticos  infectados         R0   Cercania   Clusters Infectados    Curados    Muertos Indirectos   Enfermos Hospitaliz Infectados    Curados    Muertos         R0");
            sw.WriteLine();
            if (mediatipos == null)
            {
                sw.Close();
                return;
            }
            ResTipo rt;
            for (int tipo = 0; tipo < mediatipos.Length; tipo++)
            {
                // Tipos

                sw.WriteLine(string.Format("Tipo {0}", tipo));
                sw.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos Indirectos   Enfermos Hospitaliz");
                sw.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
                for (int dia = 0; dia < mediatipos[tipo].Count; dia++)
                {
                    // Días

                    rt = mediatipos[tipo].ElementAt(dia);
                    sw.WriteLine(string.Format(
                        "{0,3} {1,10} {2,10} {3,10} {4,10} {5,10} {6,10} {7,10} {8,10} {9,10}",
                        dia + 1,
                        rt.sanos,
                        rt.infectados,
                        rt.importados,
                        rt.curados,
                        rt.desinmunizados,
                        rt.muertos,
                        rt.infectados_indirectos,
                        rt.enfermos,
                        rt.hospitalizados));
                }
                sw.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
                sw.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos Indirectos   Enfermos Hospitaliz");
                sw.WriteLine();
            }
            sw.Close();
        }
    }
}
