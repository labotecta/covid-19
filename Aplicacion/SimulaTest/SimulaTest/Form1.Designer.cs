namespace SimulaTest
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
            this.label51 = new System.Windows.Forms.Label();
            this.prevalencia = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sensibilidad = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.especificidad = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.simulaciones = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ancho = new System.Windows.Forms.TextBox();
            this.b_calcula = new System.Windows.Forms.Button();
            this.pv = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pf = new System.Windows.Forms.Label();
            this.nv = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nf = new System.Windows.Forms.Label();
            this.positivos = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.contador = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.muestra = new System.Windows.Forms.TextBox();
            this.grafico = new System.Windows.Forms.PictureBox();
            this.b_cancela = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grafico)).BeginInit();
            this.SuspendLayout();
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label51.Location = new System.Drawing.Point(24, 14);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(82, 17);
            this.label51.TabIndex = 292;
            this.label51.Text = "Prevalencia";
            // 
            // prevalencia
            // 
            this.prevalencia.ForeColor = System.Drawing.SystemColors.ControlText;
            this.prevalencia.Location = new System.Drawing.Point(172, 14);
            this.prevalencia.Name = "prevalencia";
            this.prevalencia.Size = new System.Drawing.Size(56, 22);
            this.prevalencia.TabIndex = 291;
            this.prevalencia.Text = "5";
            this.prevalencia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(24, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 17);
            this.label1.TabIndex = 294;
            this.label1.Text = "Sensibilidad";
            // 
            // sensibilidad
            // 
            this.sensibilidad.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sensibilidad.Location = new System.Drawing.Point(172, 40);
            this.sensibilidad.Name = "sensibilidad";
            this.sensibilidad.Size = new System.Drawing.Size(56, 22);
            this.sensibilidad.TabIndex = 293;
            this.sensibilidad.Text = "97";
            this.sensibilidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(24, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 17);
            this.label2.TabIndex = 296;
            this.label2.Text = "Especifidad";
            // 
            // especificidad
            // 
            this.especificidad.ForeColor = System.Drawing.SystemColors.ControlText;
            this.especificidad.Location = new System.Drawing.Point(172, 67);
            this.especificidad.Name = "especificidad";
            this.especificidad.Size = new System.Drawing.Size(56, 22);
            this.especificidad.TabIndex = 295;
            this.especificidad.Text = "100";
            this.especificidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(253, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 17);
            this.label3.TabIndex = 298;
            this.label3.Text = "Simulaciones";
            // 
            // simulaciones
            // 
            this.simulaciones.ForeColor = System.Drawing.SystemColors.ControlText;
            this.simulaciones.Location = new System.Drawing.Point(401, 14);
            this.simulaciones.Name = "simulaciones";
            this.simulaciones.Size = new System.Drawing.Size(56, 22);
            this.simulaciones.TabIndex = 297;
            this.simulaciones.Text = "100000";
            this.simulaciones.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(253, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 300;
            this.label4.Text = "Muestra";
            // 
            // ancho
            // 
            this.ancho.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ancho.Location = new System.Drawing.Point(401, 67);
            this.ancho.Name = "ancho";
            this.ancho.Size = new System.Drawing.Size(56, 22);
            this.ancho.TabIndex = 299;
            this.ancho.Text = "0,1";
            this.ancho.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // b_calcula
            // 
            this.b_calcula.Location = new System.Drawing.Point(289, 98);
            this.b_calcula.Margin = new System.Windows.Forms.Padding(2);
            this.b_calcula.Name = "b_calcula";
            this.b_calcula.Size = new System.Drawing.Size(168, 29);
            this.b_calcula.TabIndex = 301;
            this.b_calcula.Text = "Calcula";
            this.b_calcula.UseVisualStyleBackColor = true;
            this.b_calcula.Click += new System.EventHandler(this.B_calcula_Click);
            // 
            // pv
            // 
            this.pv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pv.Location = new System.Drawing.Point(200, 197);
            this.pv.Name = "pv";
            this.pv.Size = new System.Drawing.Size(42, 22);
            this.pv.TabIndex = 330;
            this.pv.Text = "0";
            this.pv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(102, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 17);
            this.label5.TabIndex = 331;
            this.label5.Text = "TEST +";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(200, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 17);
            this.label6.TabIndex = 332;
            this.label6.Text = "% SI";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(283, 169);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 17);
            this.label7.TabIndex = 333;
            this.label7.Text = "% NO";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pf
            // 
            this.pf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pf.Location = new System.Drawing.Point(283, 197);
            this.pf.Name = "pf";
            this.pf.Size = new System.Drawing.Size(42, 22);
            this.pf.TabIndex = 334;
            this.pf.Text = "0";
            this.pf.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nv
            // 
            this.nv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nv.Location = new System.Drawing.Point(283, 223);
            this.nv.Name = "nv";
            this.nv.Size = new System.Drawing.Size(42, 22);
            this.nv.TabIndex = 337;
            this.nv.Text = "0";
            this.nv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(102, 223);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 17);
            this.label9.TabIndex = 336;
            this.label9.Text = "TEST -";
            // 
            // nf
            // 
            this.nf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nf.Location = new System.Drawing.Point(200, 223);
            this.nf.Name = "nf";
            this.nf.Size = new System.Drawing.Size(42, 22);
            this.nf.TabIndex = 335;
            this.nf.Text = "0";
            this.nf.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // positivos
            // 
            this.positivos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.positivos.Location = new System.Drawing.Point(415, 197);
            this.positivos.Name = "positivos";
            this.positivos.Size = new System.Drawing.Size(42, 22);
            this.positivos.TabIndex = 338;
            this.positivos.Text = "0";
            this.positivos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(345, 169);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 17);
            this.label10.TabIndex = 339;
            this.label10.Text = "% Positivos";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contador
            // 
            this.contador.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contador.ForeColor = System.Drawing.SystemColors.ControlText;
            this.contador.Location = new System.Drawing.Point(51, 169);
            this.contador.Name = "contador";
            this.contador.Size = new System.Drawing.Size(125, 22);
            this.contador.TabIndex = 340;
            this.contador.Text = "0";
            this.contador.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(253, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 17);
            this.label8.TabIndex = 342;
            this.label8.Text = "Ancho histograma";
            // 
            // muestra
            // 
            this.muestra.ForeColor = System.Drawing.SystemColors.ControlText;
            this.muestra.Location = new System.Drawing.Point(401, 40);
            this.muestra.Name = "muestra";
            this.muestra.Size = new System.Drawing.Size(56, 22);
            this.muestra.TabIndex = 341;
            this.muestra.Text = "50000";
            this.muestra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // grafico
            // 
            this.grafico.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grafico.Location = new System.Drawing.Point(18, 260);
            this.grafico.Name = "grafico";
            this.grafico.Size = new System.Drawing.Size(447, 349);
            this.grafico.TabIndex = 343;
            this.grafico.TabStop = false;
            // 
            // b_cancela
            // 
            this.b_cancela.Enabled = false;
            this.b_cancela.ForeColor = System.Drawing.Color.Red;
            this.b_cancela.Location = new System.Drawing.Point(289, 131);
            this.b_cancela.Margin = new System.Windows.Forms.Padding(2);
            this.b_cancela.Name = "b_cancela";
            this.b_cancela.Size = new System.Drawing.Size(168, 29);
            this.b_cancela.TabIndex = 344;
            this.b_cancela.Text = "Cancela";
            this.b_cancela.UseVisualStyleBackColor = true;
            this.b_cancela.Click += new System.EventHandler(this.B_cancela_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 617);
            this.Controls.Add(this.b_cancela);
            this.Controls.Add(this.grafico);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.muestra);
            this.Controls.Add(this.contador);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.positivos);
            this.Controls.Add(this.nv);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.nf);
            this.Controls.Add(this.pf);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pv);
            this.Controls.Add(this.b_calcula);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ancho);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.simulaciones);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.especificidad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sensibilidad);
            this.Controls.Add(this.label51);
            this.Controls.Add(this.prevalencia);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Simula test";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grafico)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox prevalencia;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sensibilidad;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox especificidad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox simulaciones;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ancho;
        private System.Windows.Forms.Button b_calcula;
        private System.Windows.Forms.Label pv;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label pf;
        private System.Windows.Forms.Label nv;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label nf;
        private System.Windows.Forms.Label positivos;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label contador;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox muestra;
        private System.Windows.Forms.PictureBox grafico;
        private System.Windows.Forms.Button b_cancela;
    }
}

