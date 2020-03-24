namespace Contagio
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.lienzo = new System.Windows.Forms.PictureBox();
            this.dia_actual = new System.Windows.Forms.Label();
            this.b_cancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lienzo)).BeginInit();
            this.SuspendLayout();
            // 
            // lienzo
            // 
            this.lienzo.Location = new System.Drawing.Point(12, 107);
            this.lienzo.Name = "lienzo";
            this.lienzo.Size = new System.Drawing.Size(300, 300);
            this.lienzo.TabIndex = 153;
            this.lienzo.TabStop = false;
            this.lienzo.Paint += new System.Windows.Forms.PaintEventHandler(this.Lienzo_Paint);
            // 
            // dia_actual
            // 
            this.dia_actual.AutoSize = true;
            this.dia_actual.Location = new System.Drawing.Point(12, 9);
            this.dia_actual.Name = "dia_actual";
            this.dia_actual.Size = new System.Drawing.Size(16, 17);
            this.dia_actual.TabIndex = 154;
            this.dia_actual.Text = "0";
            // 
            // b_cancelar
            // 
            this.b_cancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_cancelar.ForeColor = System.Drawing.Color.Red;
            this.b_cancelar.Location = new System.Drawing.Point(107, 9);
            this.b_cancelar.Margin = new System.Windows.Forms.Padding(2);
            this.b_cancelar.Name = "b_cancelar";
            this.b_cancelar.Size = new System.Drawing.Size(176, 29);
            this.b_cancelar.TabIndex = 180;
            this.b_cancelar.Text = "Cancelar";
            this.b_cancelar.UseVisualStyleBackColor = true;
            this.b_cancelar.Click += new System.EventHandler(this.B_cancelar_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 419);
            this.Controls.Add(this.b_cancelar);
            this.Controls.Add(this.dia_actual);
            this.Controls.Add(this.lienzo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lienzo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox lienzo;
        private System.Windows.Forms.Label dia_actual;
        private System.Windows.Forms.Button b_cancelar;
    }
}