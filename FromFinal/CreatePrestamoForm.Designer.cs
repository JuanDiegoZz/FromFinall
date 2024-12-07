namespace FromFinal
{
    partial class CreatePrestamoForm
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
            this.TimeDevolucion = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.UsPrestamo = new System.Windows.Forms.ComboBox();
            this.MaterialPrestamo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnGuardar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TimeDevolucion
            // 
            this.TimeDevolucion.Location = new System.Drawing.Point(25, 51);
            this.TimeDevolucion.Name = "TimeDevolucion";
            this.TimeDevolucion.Size = new System.Drawing.Size(200, 20);
            this.TimeDevolucion.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fecha De Devolucion ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // UsPrestamo
            // 
            this.UsPrestamo.FormattingEnabled = true;
            this.UsPrestamo.Location = new System.Drawing.Point(25, 127);
            this.UsPrestamo.Name = "UsPrestamo";
            this.UsPrestamo.Size = new System.Drawing.Size(168, 21);
            this.UsPrestamo.TabIndex = 2;
            // 
            // MaterialPrestamo
            // 
            this.MaterialPrestamo.FormattingEnabled = true;
            this.MaterialPrestamo.Location = new System.Drawing.Point(25, 200);
            this.MaterialPrestamo.Name = "MaterialPrestamo";
            this.MaterialPrestamo.Size = new System.Drawing.Size(168, 21);
            this.MaterialPrestamo.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Usuario  A Realizar Prestamo ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Material Bibliografico De Prestamo ";
            // 
            // BtnGuardar
            // 
            this.BtnGuardar.Location = new System.Drawing.Point(135, 246);
            this.BtnGuardar.Name = "BtnGuardar";
            this.BtnGuardar.Size = new System.Drawing.Size(75, 23);
            this.BtnGuardar.TabIndex = 6;
            this.BtnGuardar.Text = "Guardar";
            this.BtnGuardar.UseVisualStyleBackColor = true;
            this.BtnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            // 
            // CreatePrestamoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 286);
            this.Controls.Add(this.BtnGuardar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MaterialPrestamo);
            this.Controls.Add(this.UsPrestamo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TimeDevolucion);
            this.Name = "CreatePrestamoForm";
            this.Text = "CreatePrestamoForm";
            this.Load += new System.EventHandler(this.CreatePrestamoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker TimeDevolucion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox UsPrestamo;
        private System.Windows.Forms.ComboBox MaterialPrestamo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnGuardar;
    }
}