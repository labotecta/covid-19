namespace Contagio
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.d_radio = new System.Windows.Forms.TextBox();
            this.tablaGrupos = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.sel_caso = new System.Windows.Forms.Button();
            this.sel_grupos = new System.Windows.Forms.Button();
            this.senda_grupos = new System.Windows.Forms.Label();
            this.selunidad = new System.Windows.Forms.ListBox();
            this.label19 = new System.Windows.Forms.Label();
            this.b_salva_grupos = new System.Windows.Forms.Button();
            this.b_simula = new System.Windows.Forms.Button();
            this.d_individuos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.b_salva_caso = new System.Windows.Forms.Button();
            this.d_dias = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.d_lon_paso = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.d_contacto = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.b_crea_poblacion = new System.Windows.Forms.Button();
            this.d_contagio = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.b_cancelar = new System.Windows.Forms.Button();
            this.v_contactos = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.v_dia = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.v_paso = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.v_recorridos = new System.Windows.Forms.Label();
            this.senda_caso = new System.Windows.Forms.Label();
            this.b_importa_individuos = new System.Windows.Forms.Button();
            this.b_exporta_individuos = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.v_max_pasos = new System.Windows.Forms.Label();
            this.d_focos = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.v_infectado = new System.Windows.Forms.Label();
            this.salvar_imagenes = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.v_r0 = new System.Windows.Forms.Label();
            this.d_latencia = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.d_recontagio = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.d_mininmunidad = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.tabla_vecinos_en_uso = new System.Windows.Forms.Label();
            this.d_importados = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.d_dias_importados = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.d_grupo_importado = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.prefijo = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.mostrar_recorridos = new System.Windows.Forms.CheckBox();
            this.d_potencia = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.d_numero_c = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.d_contagio_c = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.d_individuos_c = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.linea_estado = new System.Windows.Forms.Label();
            this.puntos_gordos = new System.Windows.Forms.CheckBox();
            this.b_crea_clusters = new System.Windows.Forms.Button();
            this.d_dias_exportar = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.v_dias_exportar = new System.Windows.Forms.Label();
            this.v_sanos = new System.Windows.Forms.Label();
            this.v_infectados = new System.Windows.Forms.Label();
            this.v_curados = new System.Windows.Forms.Label();
            this.v_desinmunizados = new System.Windows.Forms.Label();
            this.v_muertos = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tablaGrupos)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(206, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Radio";
            // 
            // d_radio
            // 
            this.d_radio.Location = new System.Drawing.Point(263, 155);
            this.d_radio.Name = "d_radio";
            this.d_radio.Size = new System.Drawing.Size(79, 22);
            this.d_radio.TabIndex = 1;
            this.d_radio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_radio.TextChanged += new System.EventHandler(this.PoblacionCambiado);
            // 
            // tablaGrupos
            // 
            this.tablaGrupos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaGrupos.Location = new System.Drawing.Point(12, 186);
            this.tablaGrupos.Name = "tablaGrupos";
            this.tablaGrupos.RowHeadersWidth = 51;
            this.tablaGrupos.RowTemplate.Height = 24;
            this.tablaGrupos.Size = new System.Drawing.Size(984, 281);
            this.tablaGrupos.TabIndex = 86;
            this.tablaGrupos.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.TablaGrupos_CellEndEdit);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 17);
            this.label5.TabIndex = 158;
            this.label5.Text = "Grupos";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 17);
            this.label3.TabIndex = 157;
            this.label3.Text = "Caso";
            // 
            // sel_caso
            // 
            this.sel_caso.Location = new System.Drawing.Point(623, 46);
            this.sel_caso.Margin = new System.Windows.Forms.Padding(2);
            this.sel_caso.Name = "sel_caso";
            this.sel_caso.Size = new System.Drawing.Size(34, 28);
            this.sel_caso.TabIndex = 156;
            this.sel_caso.Text = "S";
            this.sel_caso.UseVisualStyleBackColor = true;
            this.sel_caso.Click += new System.EventHandler(this.Sel_caso_Click);
            // 
            // sel_grupos
            // 
            this.sel_grupos.Location = new System.Drawing.Point(623, 74);
            this.sel_grupos.Margin = new System.Windows.Forms.Padding(2);
            this.sel_grupos.Name = "sel_grupos";
            this.sel_grupos.Size = new System.Drawing.Size(34, 28);
            this.sel_grupos.TabIndex = 154;
            this.sel_grupos.Text = "S";
            this.sel_grupos.UseVisualStyleBackColor = true;
            this.sel_grupos.Click += new System.EventHandler(this.Sel_grupos_Click);
            // 
            // senda_grupos
            // 
            this.senda_grupos.BackColor = System.Drawing.Color.White;
            this.senda_grupos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.senda_grupos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.senda_grupos.Location = new System.Drawing.Point(139, 78);
            this.senda_grupos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.senda_grupos.Name = "senda_grupos";
            this.senda_grupos.Size = new System.Drawing.Size(480, 24);
            this.senda_grupos.TabIndex = 153;
            this.senda_grupos.Text = "F:\\Contagio\\grupos.csv";
            // 
            // selunidad
            // 
            this.selunidad.ForeColor = System.Drawing.Color.Blue;
            this.selunidad.FormattingEnabled = true;
            this.selunidad.ItemHeight = 16;
            this.selunidad.Items.AddRange(new object[] {
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P"});
            this.selunidad.Location = new System.Drawing.Point(268, 6);
            this.selunidad.Name = "selunidad";
            this.selunidad.Size = new System.Drawing.Size(61, 36);
            this.selunidad.TabIndex = 150;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Blue;
            this.label19.Location = new System.Drawing.Point(12, 17);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(224, 18);
            this.label19.TabIndex = 149;
            this.label19.Text = "UNIDAD FICHEROS DE SALIDA";
            // 
            // b_salva_grupos
            // 
            this.b_salva_grupos.Location = new System.Drawing.Point(820, 474);
            this.b_salva_grupos.Margin = new System.Windows.Forms.Padding(2);
            this.b_salva_grupos.Name = "b_salva_grupos";
            this.b_salva_grupos.Size = new System.Drawing.Size(176, 29);
            this.b_salva_grupos.TabIndex = 162;
            this.b_salva_grupos.Text = "Salva grupos";
            this.b_salva_grupos.UseVisualStyleBackColor = true;
            this.b_salva_grupos.Click += new System.EventHandler(this.B_salva_grupos_Click);
            // 
            // b_simula
            // 
            this.b_simula.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_simula.Location = new System.Drawing.Point(877, 643);
            this.b_simula.Margin = new System.Windows.Forms.Padding(2);
            this.b_simula.Name = "b_simula";
            this.b_simula.Size = new System.Drawing.Size(119, 29);
            this.b_simula.TabIndex = 164;
            this.b_simula.Text = "Simula";
            this.b_simula.UseVisualStyleBackColor = true;
            this.b_simula.Click += new System.EventHandler(this.B_simula_Click);
            // 
            // d_individuos
            // 
            this.d_individuos.Location = new System.Drawing.Point(97, 155);
            this.d_individuos.Name = "d_individuos";
            this.d_individuos.Size = new System.Drawing.Size(100, 22);
            this.d_individuos.TabIndex = 166;
            this.d_individuos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_individuos.TextChanged += new System.EventHandler(this.PoblacionCambiado);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 17);
            this.label2.TabIndex = 165;
            this.label2.Text = "Individuos";
            // 
            // b_salva_caso
            // 
            this.b_salva_caso.Location = new System.Drawing.Point(678, 9);
            this.b_salva_caso.Margin = new System.Windows.Forms.Padding(2);
            this.b_salva_caso.Name = "b_salva_caso";
            this.b_salva_caso.Size = new System.Drawing.Size(145, 28);
            this.b_salva_caso.TabIndex = 167;
            this.b_salva_caso.Text = "Salva Caso";
            this.b_salva_caso.UseVisualStyleBackColor = true;
            this.b_salva_caso.Click += new System.EventHandler(this.B_salva_caso_Click);
            // 
            // d_dias
            // 
            this.d_dias.Location = new System.Drawing.Point(830, 645);
            this.d_dias.Name = "d_dias";
            this.d_dias.Size = new System.Drawing.Size(38, 22);
            this.d_dias.TabIndex = 172;
            this.d_dias.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_dias.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(781, 647);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 17);
            this.label6.TabIndex = 171;
            this.label6.Text = "Dias";
            // 
            // d_lon_paso
            // 
            this.d_lon_paso.Location = new System.Drawing.Point(735, 645);
            this.d_lon_paso.Name = "d_lon_paso";
            this.d_lon_paso.Size = new System.Drawing.Size(38, 22);
            this.d_lon_paso.TabIndex = 176;
            this.d_lon_paso.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_lon_paso.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(625, 647);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 17);
            this.label7.TabIndex = 175;
            this.label7.Text = "Long 10 pasos";
            // 
            // d_contacto
            // 
            this.d_contacto.Location = new System.Drawing.Point(576, 645);
            this.d_contacto.Name = "d_contacto";
            this.d_contacto.Size = new System.Drawing.Size(38, 22);
            this.d_contacto.TabIndex = 178;
            this.d_contacto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_contacto.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Fuchsia;
            this.label4.Location = new System.Drawing.Point(390, 647);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 17);
            this.label4.TabIndex = 177;
            this.label4.Text = "Distancia de contacto";
            // 
            // b_crea_poblacion
            // 
            this.b_crea_poblacion.Location = new System.Drawing.Point(375, 151);
            this.b_crea_poblacion.Margin = new System.Windows.Forms.Padding(2);
            this.b_crea_poblacion.Name = "b_crea_poblacion";
            this.b_crea_poblacion.Size = new System.Drawing.Size(134, 29);
            this.b_crea_poblacion.TabIndex = 179;
            this.b_crea_poblacion.Text = "Crear población";
            this.b_crea_poblacion.UseVisualStyleBackColor = true;
            this.b_crea_poblacion.Click += new System.EventHandler(this.B_crea_poblacion_Click);
            // 
            // d_contagio
            // 
            this.d_contagio.ForeColor = System.Drawing.Color.Maroon;
            this.d_contagio.Location = new System.Drawing.Point(340, 581);
            this.d_contagio.Name = "d_contagio";
            this.d_contagio.Size = new System.Drawing.Size(38, 22);
            this.d_contagio.TabIndex = 181;
            this.d_contagio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_contagio.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Maroon;
            this.label8.Location = new System.Drawing.Point(167, 583);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(161, 17);
            this.label8.TabIndex = 180;
            this.label8.Text = "% Probabilidad contagio";
            // 
            // b_cancelar
            // 
            this.b_cancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_cancelar.ForeColor = System.Drawing.Color.Red;
            this.b_cancelar.Location = new System.Drawing.Point(827, 10);
            this.b_cancelar.Margin = new System.Windows.Forms.Padding(2);
            this.b_cancelar.Name = "b_cancelar";
            this.b_cancelar.Size = new System.Drawing.Size(171, 29);
            this.b_cancelar.TabIndex = 182;
            this.b_cancelar.Text = "Cancelar";
            this.b_cancelar.UseVisualStyleBackColor = true;
            this.b_cancelar.Click += new System.EventHandler(this.B_cancelar_Click);
            // 
            // v_contactos
            // 
            this.v_contactos.Location = new System.Drawing.Point(795, 114);
            this.v_contactos.Name = "v_contactos";
            this.v_contactos.Size = new System.Drawing.Size(70, 17);
            this.v_contactos.TabIndex = 183;
            this.v_contactos.Text = "0";
            this.v_contactos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(678, 114);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(128, 17);
            this.label10.TabIndex = 184;
            this.label10.Text = "Encuentros críticos";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(678, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 17);
            this.label9.TabIndex = 186;
            this.label9.Text = "Dia";
            // 
            // v_dia
            // 
            this.v_dia.Location = new System.Drawing.Point(795, 47);
            this.v_dia.Name = "v_dia";
            this.v_dia.Size = new System.Drawing.Size(70, 17);
            this.v_dia.TabIndex = 185;
            this.v_dia.Text = "0";
            this.v_dia.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(678, 68);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(40, 17);
            this.label13.TabIndex = 188;
            this.label13.Text = "Paso";
            // 
            // v_paso
            // 
            this.v_paso.Location = new System.Drawing.Point(795, 68);
            this.v_paso.Name = "v_paso";
            this.v_paso.Size = new System.Drawing.Size(70, 17);
            this.v_paso.TabIndex = 187;
            this.v_paso.Text = "0";
            this.v_paso.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(678, 91);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(103, 17);
            this.label11.TabIndex = 190;
            this.label11.Text = "Recorrido (ida)";
            // 
            // v_recorridos
            // 
            this.v_recorridos.Location = new System.Drawing.Point(795, 91);
            this.v_recorridos.Name = "v_recorridos";
            this.v_recorridos.Size = new System.Drawing.Size(70, 17);
            this.v_recorridos.TabIndex = 189;
            this.v_recorridos.Text = "0";
            this.v_recorridos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // senda_caso
            // 
            this.senda_caso.BackColor = System.Drawing.Color.White;
            this.senda_caso.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.senda_caso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.senda_caso.Location = new System.Drawing.Point(139, 50);
            this.senda_caso.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.senda_caso.Name = "senda_caso";
            this.senda_caso.Size = new System.Drawing.Size(480, 24);
            this.senda_caso.TabIndex = 191;
            this.senda_caso.Text = "F:\\Contagio\\grupos.csv";
            // 
            // b_importa_individuos
            // 
            this.b_importa_individuos.Location = new System.Drawing.Point(11, 474);
            this.b_importa_individuos.Margin = new System.Windows.Forms.Padding(2);
            this.b_importa_individuos.Name = "b_importa_individuos";
            this.b_importa_individuos.Size = new System.Drawing.Size(142, 29);
            this.b_importa_individuos.TabIndex = 192;
            this.b_importa_individuos.Text = "Importa individuos";
            this.b_importa_individuos.UseVisualStyleBackColor = true;
            this.b_importa_individuos.Click += new System.EventHandler(this.B_importa_individuos_Click);
            // 
            // b_exporta_individuos
            // 
            this.b_exporta_individuos.Location = new System.Drawing.Point(513, 151);
            this.b_exporta_individuos.Margin = new System.Windows.Forms.Padding(2);
            this.b_exporta_individuos.Name = "b_exporta_individuos";
            this.b_exporta_individuos.Size = new System.Drawing.Size(145, 29);
            this.b_exporta_individuos.TabIndex = 193;
            this.b_exporta_individuos.Text = "Exporta individuos";
            this.b_exporta_individuos.UseVisualStyleBackColor = true;
            this.b_exporta_individuos.Click += new System.EventHandler(this.B_exporta_individuos_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(875, 68);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 17);
            this.label12.TabIndex = 195;
            this.label12.Text = "de";
            // 
            // v_max_pasos
            // 
            this.v_max_pasos.Location = new System.Drawing.Point(915, 68);
            this.v_max_pasos.Name = "v_max_pasos";
            this.v_max_pasos.Size = new System.Drawing.Size(70, 17);
            this.v_max_pasos.TabIndex = 196;
            this.v_max_pasos.Text = "0";
            this.v_max_pasos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // d_focos
            // 
            this.d_focos.ForeColor = System.Drawing.Color.Green;
            this.d_focos.Location = new System.Drawing.Point(340, 554);
            this.d_focos.Name = "d_focos";
            this.d_focos.Size = new System.Drawing.Size(38, 22);
            this.d_focos.TabIndex = 198;
            this.d_focos.Text = "1";
            this.d_focos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_focos.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Green;
            this.label14.Location = new System.Drawing.Point(167, 556);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(100, 17);
            this.label14.TabIndex = 197;
            this.label14.Text = "Focos iniciales";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(678, 137);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(98, 17);
            this.label15.TabIndex = 200;
            this.label15.Text = "Días infectado";
            // 
            // v_infectado
            // 
            this.v_infectado.Location = new System.Drawing.Point(795, 137);
            this.v_infectado.Name = "v_infectado";
            this.v_infectado.Size = new System.Drawing.Size(70, 17);
            this.v_infectado.TabIndex = 199;
            this.v_infectado.Text = "0";
            this.v_infectado.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // salvar_imagenes
            // 
            this.salvar_imagenes.AutoSize = true;
            this.salvar_imagenes.Location = new System.Drawing.Point(576, 502);
            this.salvar_imagenes.Name = "salvar_imagenes";
            this.salvar_imagenes.Size = new System.Drawing.Size(135, 21);
            this.salvar_imagenes.TabIndex = 201;
            this.salvar_imagenes.Text = "Salvar imágenes";
            this.salvar_imagenes.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(678, 162);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(26, 17);
            this.label16.TabIndex = 203;
            this.label16.Text = "R0";
            // 
            // v_r0
            // 
            this.v_r0.Location = new System.Drawing.Point(795, 162);
            this.v_r0.Name = "v_r0";
            this.v_r0.Size = new System.Drawing.Size(70, 17);
            this.v_r0.TabIndex = 202;
            this.v_r0.Text = "0";
            this.v_r0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // d_latencia
            // 
            this.d_latencia.Location = new System.Drawing.Point(179, 645);
            this.d_latencia.Name = "d_latencia";
            this.d_latencia.Size = new System.Drawing.Size(38, 22);
            this.d_latencia.TabIndex = 205;
            this.d_latencia.Text = "0";
            this.d_latencia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_latencia.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(11, 647);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(152, 17);
            this.label17.TabIndex = 204;
            this.label17.Text = "Días Latencia contagio";
            // 
            // d_recontagio
            // 
            this.d_recontagio.ForeColor = System.Drawing.Color.Maroon;
            this.d_recontagio.Location = new System.Drawing.Point(576, 581);
            this.d_recontagio.Name = "d_recontagio";
            this.d_recontagio.Size = new System.Drawing.Size(38, 22);
            this.d_recontagio.TabIndex = 213;
            this.d_recontagio.Text = "0";
            this.d_recontagio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_recontagio.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.Maroon;
            this.label20.Location = new System.Drawing.Point(390, 583);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(174, 17);
            this.label20.TabIndex = 212;
            this.label20.Text = "% Probabilidad recontagio";
            // 
            // d_mininmunidad
            // 
            this.d_mininmunidad.ForeColor = System.Drawing.Color.Maroon;
            this.d_mininmunidad.Location = new System.Drawing.Point(831, 581);
            this.d_mininmunidad.Name = "d_mininmunidad";
            this.d_mininmunidad.Size = new System.Drawing.Size(38, 22);
            this.d_mininmunidad.TabIndex = 215;
            this.d_mininmunidad.Text = "0";
            this.d_mininmunidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.Maroon;
            this.label22.Location = new System.Drawing.Point(625, 583);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(153, 17);
            this.label22.TabIndex = 214;
            this.label22.Text = "Días mínimo inmunidad";
            // 
            // tabla_vecinos_en_uso
            // 
            this.tabla_vecinos_en_uso.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabla_vecinos_en_uso.Location = new System.Drawing.Point(877, 580);
            this.tabla_vecinos_en_uso.Name = "tabla_vecinos_en_uso";
            this.tabla_vecinos_en_uso.Size = new System.Drawing.Size(119, 22);
            this.tabla_vecinos_en_uso.TabIndex = 216;
            this.tabla_vecinos_en_uso.Text = "Tabla vecinos";
            this.tabla_vecinos_en_uso.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tabla_vecinos_en_uso.DoubleClick += new System.EventHandler(this.Tabla_vecinos_en_uso_DoubleClick);
            // 
            // d_importados
            // 
            this.d_importados.ForeColor = System.Drawing.Color.Green;
            this.d_importados.Location = new System.Drawing.Point(576, 554);
            this.d_importados.Name = "d_importados";
            this.d_importados.Size = new System.Drawing.Size(38, 22);
            this.d_importados.TabIndex = 220;
            this.d_importados.Text = "0";
            this.d_importados.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_importados.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.Color.Green;
            this.label23.Location = new System.Drawing.Point(390, 556);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(120, 17);
            this.label23.TabIndex = 219;
            this.label23.Text = "Focos importados";
            // 
            // d_dias_importados
            // 
            this.d_dias_importados.ForeColor = System.Drawing.Color.Green;
            this.d_dias_importados.Location = new System.Drawing.Point(673, 554);
            this.d_dias_importados.Name = "d_dias_importados";
            this.d_dias_importados.Size = new System.Drawing.Size(38, 22);
            this.d_dias_importados.TabIndex = 218;
            this.d_dias_importados.Text = "10";
            this.d_dias_importados.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_dias_importados.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.Color.Green;
            this.label25.Location = new System.Drawing.Point(719, 556);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(102, 17);
            this.label25.TabIndex = 221;
            this.label25.Text = "días, del grupo";
            // 
            // d_grupo_importado
            // 
            this.d_grupo_importado.ForeColor = System.Drawing.Color.Green;
            this.d_grupo_importado.Location = new System.Drawing.Point(831, 554);
            this.d_grupo_importado.Name = "d_grupo_importado";
            this.d_grupo_importado.Size = new System.Drawing.Size(165, 22);
            this.d_grupo_importado.TabIndex = 222;
            this.d_grupo_importado.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.Color.Green;
            this.label24.Location = new System.Drawing.Point(625, 556);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(39, 17);
            this.label24.TabIndex = 223;
            this.label24.Text = "cada";
            // 
            // prefijo
            // 
            this.prefijo.Location = new System.Drawing.Point(395, 13);
            this.prefijo.Name = "prefijo";
            this.prefijo.Size = new System.Drawing.Size(262, 22);
            this.prefijo.TabIndex = 225;
            this.prefijo.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(337, 13);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(47, 17);
            this.label26.TabIndex = 224;
            this.label26.Text = "prefijo";
            // 
            // mostrar_recorridos
            // 
            this.mostrar_recorridos.AutoSize = true;
            this.mostrar_recorridos.Checked = true;
            this.mostrar_recorridos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mostrar_recorridos.Location = new System.Drawing.Point(167, 502);
            this.mostrar_recorridos.Name = "mostrar_recorridos";
            this.mostrar_recorridos.Size = new System.Drawing.Size(146, 21);
            this.mostrar_recorridos.TabIndex = 226;
            this.mostrar_recorridos.Text = "mostrar recorridos";
            this.mostrar_recorridos.UseVisualStyleBackColor = true;
            this.mostrar_recorridos.CheckedChanged += new System.EventHandler(this.Mostrar_recorridos_CheckedChanged);
            // 
            // d_potencia
            // 
            this.d_potencia.ForeColor = System.Drawing.SystemColors.ControlText;
            this.d_potencia.Location = new System.Drawing.Point(576, 528);
            this.d_potencia.Name = "d_potencia";
            this.d_potencia.Size = new System.Drawing.Size(38, 22);
            this.d_potencia.TabIndex = 228;
            this.d_potencia.Text = "3";
            this.d_potencia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label27.Location = new System.Drawing.Point(390, 530);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(173, 17);
            this.label27.TabIndex = 227;
            this.label27.Text = "Potencia p para y = a* x^p";
            // 
            // d_numero_c
            // 
            this.d_numero_c.ForeColor = System.Drawing.SystemColors.ControlText;
            this.d_numero_c.Location = new System.Drawing.Point(576, 607);
            this.d_numero_c.Name = "d_numero_c";
            this.d_numero_c.Size = new System.Drawing.Size(38, 22);
            this.d_numero_c.TabIndex = 232;
            this.d_numero_c.Text = "0";
            this.d_numero_c.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_numero_c.TextChanged += new System.EventHandler(this.ClusterCambiado);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label18.Location = new System.Drawing.Point(390, 609);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(131, 17);
            this.label18.TabIndex = 231;
            this.label18.Text = "Número de clusters";
            // 
            // d_contagio_c
            // 
            this.d_contagio_c.ForeColor = System.Drawing.SystemColors.ControlText;
            this.d_contagio_c.Location = new System.Drawing.Point(340, 607);
            this.d_contagio_c.Name = "d_contagio_c";
            this.d_contagio_c.Size = new System.Drawing.Size(38, 22);
            this.d_contagio_c.TabIndex = 230;
            this.d_contagio_c.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_contagio_c.TextChanged += new System.EventHandler(this.CasoCambiado);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label21.Location = new System.Drawing.Point(167, 609);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(161, 17);
            this.label21.TabIndex = 229;
            this.label21.Text = "% Probabilidad contagio";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.ForeColor = System.Drawing.Color.Maroon;
            this.label28.Location = new System.Drawing.Point(11, 583);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(93, 17);
            this.label28.TabIndex = 233;
            this.label28.Text = "PROXIMIDAD";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label29.Location = new System.Drawing.Point(11, 609);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(72, 17);
            this.label29.TabIndex = 234;
            this.label29.Text = "CLUSTER";
            // 
            // d_individuos_c
            // 
            this.d_individuos_c.ForeColor = System.Drawing.SystemColors.ControlText;
            this.d_individuos_c.Location = new System.Drawing.Point(831, 607);
            this.d_individuos_c.Name = "d_individuos_c";
            this.d_individuos_c.Size = new System.Drawing.Size(38, 22);
            this.d_individuos_c.TabIndex = 236;
            this.d_individuos_c.Text = "1";
            this.d_individuos_c.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.d_individuos_c.TextChanged += new System.EventHandler(this.ClusterCambiado);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label30.Location = new System.Drawing.Point(625, 609);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(186, 17);
            this.label30.TabIndex = 235;
            this.label30.Text = "Factor Individuos por cluster";
            // 
            // linea_estado
            // 
            this.linea_estado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.linea_estado.Location = new System.Drawing.Point(12, 708);
            this.linea_estado.Name = "linea_estado";
            this.linea_estado.Size = new System.Drawing.Size(984, 22);
            this.linea_estado.TabIndex = 237;
            this.linea_estado.Text = "Ok";
            this.linea_estado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // puntos_gordos
            // 
            this.puntos_gordos.AutoSize = true;
            this.puntos_gordos.Checked = true;
            this.puntos_gordos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.puntos_gordos.Location = new System.Drawing.Point(340, 502);
            this.puntos_gordos.Name = "puntos_gordos";
            this.puntos_gordos.Size = new System.Drawing.Size(122, 21);
            this.puntos_gordos.TabIndex = 238;
            this.puntos_gordos.Text = "Puntos gordos";
            this.puntos_gordos.UseVisualStyleBackColor = true;
            this.puntos_gordos.CheckedChanged += new System.EventHandler(this.Puntos_gordos_CheckedChanged);
            // 
            // b_crea_clusters
            // 
            this.b_crea_clusters.Location = new System.Drawing.Point(878, 604);
            this.b_crea_clusters.Margin = new System.Windows.Forms.Padding(2);
            this.b_crea_clusters.Name = "b_crea_clusters";
            this.b_crea_clusters.Size = new System.Drawing.Size(119, 29);
            this.b_crea_clusters.TabIndex = 239;
            this.b_crea_clusters.Text = "Crea Clusters";
            this.b_crea_clusters.UseVisualStyleBackColor = true;
            this.b_crea_clusters.Click += new System.EventHandler(this.B_crea_clusters_Click);
            // 
            // d_dias_exportar
            // 
            this.d_dias_exportar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.d_dias_exportar.Location = new System.Drawing.Point(225, 678);
            this.d_dias_exportar.Name = "d_dias_exportar";
            this.d_dias_exportar.Size = new System.Drawing.Size(708, 22);
            this.d_dias_exportar.TabIndex = 241;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label31.Location = new System.Drawing.Point(12, 680);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(198, 17);
            this.label31.TabIndex = 240;
            this.label31.Text = "Exportar población en los días";
            // 
            // v_dias_exportar
            // 
            this.v_dias_exportar.Location = new System.Drawing.Point(952, 680);
            this.v_dias_exportar.Name = "v_dias_exportar";
            this.v_dias_exportar.Size = new System.Drawing.Size(33, 17);
            this.v_dias_exportar.TabIndex = 242;
            this.v_dias_exportar.Text = "0";
            this.v_dias_exportar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // v_sanos
            // 
            this.v_sanos.Location = new System.Drawing.Point(190, 477);
            this.v_sanos.Name = "v_sanos";
            this.v_sanos.Size = new System.Drawing.Size(70, 17);
            this.v_sanos.TabIndex = 243;
            this.v_sanos.Text = "0";
            this.v_sanos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // v_infectados
            // 
            this.v_infectados.Location = new System.Drawing.Point(316, 477);
            this.v_infectados.Name = "v_infectados";
            this.v_infectados.Size = new System.Drawing.Size(70, 17);
            this.v_infectados.TabIndex = 244;
            this.v_infectados.Text = "0";
            this.v_infectados.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // v_curados
            // 
            this.v_curados.Location = new System.Drawing.Point(442, 477);
            this.v_curados.Name = "v_curados";
            this.v_curados.Size = new System.Drawing.Size(70, 17);
            this.v_curados.TabIndex = 245;
            this.v_curados.Text = "0";
            this.v_curados.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // v_desinmunizados
            // 
            this.v_desinmunizados.Location = new System.Drawing.Point(568, 477);
            this.v_desinmunizados.Name = "v_desinmunizados";
            this.v_desinmunizados.Size = new System.Drawing.Size(70, 17);
            this.v_desinmunizados.TabIndex = 246;
            this.v_desinmunizados.Text = "0";
            this.v_desinmunizados.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // v_muertos
            // 
            this.v_muertos.Location = new System.Drawing.Point(694, 477);
            this.v_muertos.Name = "v_muertos";
            this.v_muertos.Size = new System.Drawing.Size(70, 17);
            this.v_muertos.TabIndex = 247;
            this.v_muertos.Text = "0";
            this.v_muertos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 734);
            this.Controls.Add(this.v_muertos);
            this.Controls.Add(this.v_desinmunizados);
            this.Controls.Add(this.v_curados);
            this.Controls.Add(this.v_infectados);
            this.Controls.Add(this.v_sanos);
            this.Controls.Add(this.v_dias_exportar);
            this.Controls.Add(this.d_dias_exportar);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.b_crea_clusters);
            this.Controls.Add(this.puntos_gordos);
            this.Controls.Add(this.linea_estado);
            this.Controls.Add(this.d_individuos_c);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.d_numero_c);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.d_contagio_c);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.d_potencia);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.mostrar_recorridos);
            this.Controls.Add(this.prefijo);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.d_grupo_importado);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.d_importados);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.d_dias_importados);
            this.Controls.Add(this.tabla_vecinos_en_uso);
            this.Controls.Add(this.d_mininmunidad);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.d_recontagio);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.d_latencia);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.v_r0);
            this.Controls.Add(this.salvar_imagenes);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.v_infectado);
            this.Controls.Add(this.d_focos);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.v_max_pasos);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.b_exporta_individuos);
            this.Controls.Add(this.b_importa_individuos);
            this.Controls.Add(this.senda_caso);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.v_recorridos);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.v_paso);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.v_dia);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.v_contactos);
            this.Controls.Add(this.b_cancelar);
            this.Controls.Add(this.d_contagio);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.b_crea_poblacion);
            this.Controls.Add(this.d_contacto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.d_lon_paso);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.d_dias);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.b_salva_caso);
            this.Controls.Add(this.d_individuos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.b_simula);
            this.Controls.Add(this.b_salva_grupos);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sel_caso);
            this.Controls.Add(this.sel_grupos);
            this.Controls.Add(this.senda_grupos);
            this.Controls.Add(this.selunidad);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.tablaGrupos);
            this.Controls.Add(this.d_radio);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tablaGrupos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox d_radio;
        private System.Windows.Forms.DataGridView tablaGrupos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button sel_caso;
        private System.Windows.Forms.Button sel_grupos;
        private System.Windows.Forms.Label senda_grupos;
        private System.Windows.Forms.ListBox selunidad;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button b_salva_grupos;
        private System.Windows.Forms.Button b_simula;
        private System.Windows.Forms.TextBox d_individuos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button b_salva_caso;
        private System.Windows.Forms.TextBox d_dias;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox d_lon_paso;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox d_contacto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button b_crea_poblacion;
        private System.Windows.Forms.TextBox d_contagio;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button b_cancelar;
        private System.Windows.Forms.Label v_contactos;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label v_dia;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label v_paso;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label v_recorridos;
        private System.Windows.Forms.Label senda_caso;
        private System.Windows.Forms.Button b_importa_individuos;
        private System.Windows.Forms.Button b_exporta_individuos;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label v_max_pasos;
        private System.Windows.Forms.TextBox d_focos;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label v_infectado;
        public System.Windows.Forms.CheckBox salvar_imagenes;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label v_r0;
        private System.Windows.Forms.TextBox d_latencia;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox d_recontagio;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox d_mininmunidad;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label tabla_vecinos_en_uso;
        private System.Windows.Forms.TextBox d_importados;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox d_dias_importados;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox d_grupo_importado;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox prefijo;
        private System.Windows.Forms.Label label26;
        public System.Windows.Forms.CheckBox mostrar_recorridos;
        private System.Windows.Forms.TextBox d_potencia;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox d_numero_c;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox d_contagio_c;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox d_individuos_c;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label linea_estado;
        public System.Windows.Forms.CheckBox puntos_gordos;
        private System.Windows.Forms.Button b_crea_clusters;
        private System.Windows.Forms.TextBox d_dias_exportar;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label v_dias_exportar;
        private System.Windows.Forms.Label v_sanos;
        private System.Windows.Forms.Label v_infectados;
        private System.Windows.Forms.Label v_curados;
        private System.Windows.Forms.Label v_desinmunizados;
        private System.Windows.Forms.Label v_muertos;
    }
}

