namespace FromFinal
{
    partial class CreateDevolucionForm
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
            this.cmbPrestamos = new System.Windows.Forms.ComboBox();
            this.dtpFechaDevolucion = new System.Windows.Forms.DateTimePicker();
            this.btnGuardarDevolucion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbPrestamos
            // 
            this.cmbPrestamos.FormattingEnabled = true;
            this.cmbPrestamos.Location = new System.Drawing.Point(42, 34);
            this.cmbPrestamos.Name = "cmbPrestamos";
            this.cmbPrestamos.Size = new System.Drawing.Size(201, 21);
            this.cmbPrestamos.TabIndex = 0;
            // 
            // dtpFechaDevolucion
            // 
            this.dtpFechaDevolucion.Location = new System.Drawing.Point(42, 81);
            this.dtpFechaDevolucion.Name = "dtpFechaDevolucion";
            this.dtpFechaDevolucion.Size = new System.Drawing.Size(200, 20);
            this.dtpFechaDevolucion.TabIndex = 1;
            // 
            // btnGuardarDevolucion
            // 
            this.btnGuardarDevolucion.Location = new System.Drawing.Point(88, 148);
            this.btnGuardarDevolucion.Name = "btnGuardarDevolucion";
            this.btnGuardarDevolucion.Size = new System.Drawing.Size(75, 23);
            this.btnGuardarDevolucion.TabIndex = 2;
            this.btnGuardarDevolucion.Text = "Guardar";
            this.btnGuardarDevolucion.UseVisualStyleBackColor = true;
            this.btnGuardarDevolucion.Click += new System.EventHandler(this.btnGuardarDevolucion_Click);
            // 
            // CreateDevolucionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 295);
            this.Controls.Add(this.btnGuardarDevolucion);
            this.Controls.Add(this.dtpFechaDevolucion);
            this.Controls.Add(this.cmbPrestamos);
            this.Name = "CreateDevolucionForm";
            this.Text = "CreateDevolucionForm";
            this.Load += new System.EventHandler(this.CreateDevolucionForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPrestamos;
        private System.Windows.Forms.DateTimePicker dtpFechaDevolucion;
        private System.Windows.Forms.Button btnGuardarDevolucion;
    }
}