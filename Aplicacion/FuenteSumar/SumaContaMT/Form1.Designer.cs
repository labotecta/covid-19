namespace SumaContaMT
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
            this.senda_salida = new System.Windows.Forms.Label();
            this.b_ejecuta = new System.Windows.Forms.Button();
            this.sel_salida = new System.Windows.Forms.Button();
            this.sel_fuente = new System.Windows.Forms.Button();
            this.lista = new System.Windows.Forms.ListBox();
            this.cuenta = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.dias_min = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dias_max = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.des_maxima = new System.Windows.Forms.TextBox();
            this.add_fuentes = new System.Windows.Forms.Button();
            this.borra_fuentes = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.hasta = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // senda_salida
            // 
            this.senda_salida.BackColor = System.Drawing.Color.White;
            this.senda_salida.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.senda_salida.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.senda_salida.Location = new System.Drawing.Point(12, 296);
            this.senda_salida.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.senda_salida.Name = "senda_salida";
            this.senda_salida.Size = new System.Drawing.Size(581, 24);
            this.senda_salida.TabIndex = 196;
            // 
            // b_ejecuta
            // 
            this.b_ejecuta.Location = new System.Drawing.Point(542, 335);
            this.b_ejecuta.Margin = new System.Windows.Forms.Padding(2);
            this.b_ejecuta.Name = "b_ejecuta";
            this.b_ejecuta.Size = new System.Drawing.Size(92, 28);
            this.b_ejecuta.TabIndex = 195;
            this.b_ejecuta.Text = "Ejecuta";
            this.b_ejecuta.UseVisualStyleBackColor = true;
            this.b_ejecuta.Click += new System.EventHandler(this.B_ejecuta_Click);
            // 
            // sel_salida
            // 
            this.sel_salida.Location = new System.Drawing.Point(600, 294);
            this.sel_salida.Margin = new System.Windows.Forms.Padding(2);
            this.sel_salida.Name = "sel_salida";
            this.sel_salida.Size = new System.Drawing.Size(34, 28);
            this.sel_salida.TabIndex = 194;
            this.sel_salida.Text = "S";
            this.sel_salida.UseVisualStyleBackColor = true;
            this.sel_salida.Click += new System.EventHandler(this.Sel_salida_Click);
            // 
            // sel_fuente
            // 
            this.sel_fuente.Location = new System.Drawing.Point(600, 63);
            this.sel_fuente.Margin = new System.Windows.Forms.Padding(2);
            this.sel_fuente.Name = "sel_fuente";
            this.sel_fuente.Size = new System.Drawing.Size(34, 28);
            this.sel_fuente.TabIndex = 193;
            this.sel_fuente.Text = "S";
            this.sel_fuente.UseVisualStyleBackColor = true;
            this.sel_fuente.Click += new System.EventHandler(this.Sel_fuente_Click);
            // 
            // lista
            // 
            this.lista.FormattingEnabled = true;
            this.lista.ItemHeight = 16;
            this.lista.Location = new System.Drawing.Point(12, 17);
            this.lista.Name = "lista";
            this.lista.Size = new System.Drawing.Size(580, 276);
            this.lista.TabIndex = 197;
            // 
            // cuenta
            // 
            this.cuenta.BackColor = System.Drawing.Color.White;
            this.cuenta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cuenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cuenta.Location = new System.Drawing.Point(600, 136);
            this.cuenta.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cuenta.Name = "cuenta";
            this.cuenta.Size = new System.Drawing.Size(34, 24);
            this.cuenta.TabIndex = 198;
            this.cuenta.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(12, 341);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(82, 17);
            this.label51.TabIndex = 292;
            this.label51.Text = "Mínimo días";
            // 
            // dias_min
            // 
            this.dias_min.Location = new System.Drawing.Point(103, 338);
            this.dias_min.Name = "dias_min";
            this.dias_min.Size = new System.Drawing.Size(42, 22);
            this.dias_min.TabIndex = 291;
            this.dias_min.Text = "0";
            this.dias_min.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 341);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 17);
            this.label1.TabIndex = 294;
            this.label1.Text = "Máximo días";
            // 
            // dias_max
            // 
            this.dias_max.Location = new System.Drawing.Point(248, 338);
            this.dias_max.Name = "dias_max";
            this.dias_max.Size = new System.Drawing.Size(42, 22);
            this.dias_max.TabIndex = 293;
            this.dias_max.Text = "9999";
            this.dias_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(302, 341);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 296;
            this.label2.Text = "Max Desv";
            // 
            // des_maxima
            // 
            this.des_maxima.Location = new System.Drawing.Point(380, 338);
            this.des_maxima.Name = "des_maxima";
            this.des_maxima.Size = new System.Drawing.Size(42, 22);
            this.des_maxima.TabIndex = 295;
            this.des_maxima.Text = "2";
            this.des_maxima.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // add_fuentes
            // 
            this.add_fuentes.Location = new System.Drawing.Point(600, 95);
            this.add_fuentes.Margin = new System.Windows.Forms.Padding(2);
            this.add_fuentes.Name = "add_fuentes";
            this.add_fuentes.Size = new System.Drawing.Size(34, 28);
            this.add_fuentes.TabIndex = 297;
            this.add_fuentes.Text = "+";
            this.add_fuentes.UseVisualStyleBackColor = true;
            this.add_fuentes.Click += new System.EventHandler(this.Add_fuentes_Click);
            // 
            // borra_fuentes
            // 
            this.borra_fuentes.Location = new System.Drawing.Point(600, 17);
            this.borra_fuentes.Margin = new System.Windows.Forms.Padding(2);
            this.borra_fuentes.Name = "borra_fuentes";
            this.borra_fuentes.Size = new System.Drawing.Size(34, 28);
            this.borra_fuentes.TabIndex = 298;
            this.borra_fuentes.Text = "B";
            this.borra_fuentes.UseVisualStyleBackColor = true;
            this.borra_fuentes.Click += new System.EventHandler(this.Borra_fuentes_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(434, 341);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 17);
            this.label3.TabIndex = 300;
            this.label3.Text = "Hasta";
            // 
            // hasta
            // 
            this.hasta.Location = new System.Drawing.Point(492, 338);
            this.hasta.Name = "hasta";
            this.hasta.Size = new System.Drawing.Size(42, 22);
            this.hasta.TabIndex = 299;
            this.hasta.Text = "9999";
            this.hasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 369);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.hasta);
            this.Controls.Add(this.borra_fuentes);
            this.Controls.Add(this.add_fuentes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.des_maxima);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dias_max);
            this.Controls.Add(this.label51);
            this.Controls.Add(this.dias_min);
            this.Controls.Add(this.cuenta);
            this.Controls.Add(this.lista);
            this.Controls.Add(this.senda_salida);
            this.Controls.Add(this.b_ejecuta);
            this.Controls.Add(this.sel_salida);
            this.Controls.Add(this.sel_fuente);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Suma salidas Contagios y ContaMT";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label senda_salida;
        private System.Windows.Forms.Button b_ejecuta;
        private System.Windows.Forms.Button sel_salida;
        private System.Windows.Forms.Button sel_fuente;
        private System.Windows.Forms.ListBox lista;
        private System.Windows.Forms.Label cuenta;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox dias_min;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox dias_max;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox des_maxima;
        private System.Windows.Forms.Button add_fuentes;
        private System.Windows.Forms.Button borra_fuentes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox hasta;
    }
}

