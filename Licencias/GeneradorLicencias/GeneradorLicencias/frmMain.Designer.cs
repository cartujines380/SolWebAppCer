namespace GeneradorLicencias
{
    partial class frmMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tabContenedor = new System.Windows.Forms.TabControl();
            this.tpProveedor = new System.Windows.Forms.TabPage();
            this.txtPagina = new System.Windows.Forms.TextBox();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.dgClientes = new System.Windows.Forms.DataGridView();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.tpGenerar = new System.Windows.Forms.TabPage();
            this.txtLicencia = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.txtCantMax = new System.Windows.Forms.NumericUpDown();
            this.cmbPlan = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.Codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClienteId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCantProv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblLicId = new System.Windows.Forms.Label();
            this.tabContenedor.SuspendLayout();
            this.tpProveedor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgClientes)).BeginInit();
            this.tpGenerar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCantMax)).BeginInit();
            this.SuspendLayout();
            // 
            // tabContenedor
            // 
            this.tabContenedor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabContenedor.Controls.Add(this.tpProveedor);
            this.tabContenedor.Controls.Add(this.tpGenerar);
            this.tabContenedor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabContenedor.Location = new System.Drawing.Point(13, 13);
            this.tabContenedor.Name = "tabContenedor";
            this.tabContenedor.SelectedIndex = 0;
            this.tabContenedor.Size = new System.Drawing.Size(612, 436);
            this.tabContenedor.TabIndex = 0;
            // 
            // tpProveedor
            // 
            this.tpProveedor.Controls.Add(this.txtPagina);
            this.tpProveedor.Controls.Add(this.btnFirst);
            this.tpProveedor.Controls.Add(this.btnLast);
            this.tpProveedor.Controls.Add(this.btnNext);
            this.tpProveedor.Controls.Add(this.btnPrev);
            this.tpProveedor.Controls.Add(this.dgClientes);
            this.tpProveedor.Controls.Add(this.btnClear);
            this.tpProveedor.Controls.Add(this.btnBuscar);
            this.tpProveedor.Controls.Add(this.txtBuscar);
            this.tpProveedor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tpProveedor.Location = new System.Drawing.Point(4, 22);
            this.tpProveedor.Name = "tpProveedor";
            this.tpProveedor.Padding = new System.Windows.Forms.Padding(3);
            this.tpProveedor.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tpProveedor.Size = new System.Drawing.Size(604, 410);
            this.tpProveedor.TabIndex = 0;
            this.tpProveedor.Text = "Clientes";
            this.tpProveedor.UseVisualStyleBackColor = true;
            // 
            // txtPagina
            // 
            this.txtPagina.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtPagina.Location = new System.Drawing.Point(186, 377);
            this.txtPagina.Name = "txtPagina";
            this.txtPagina.ReadOnly = true;
            this.txtPagina.Size = new System.Drawing.Size(234, 20);
            this.txtPagina.TabIndex = 4;
            this.txtPagina.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(23, 375);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(75, 23);
            this.btnFirst.TabIndex = 3;
            this.btnFirst.Text = "|<";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(507, 375);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(75, 23);
            this.btnLast.TabIndex = 3;
            this.btnLast.Text = ">|";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(426, 375);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(104, 375);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(75, 23);
            this.btnPrev.TabIndex = 3;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // dgClientes
            // 
            this.dgClientes.AllowUserToDeleteRows = false;
            this.dgClientes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgClientes.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgClientes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgClientes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgClientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Codigo,
            this.Lic,
            this.ClienteId,
            this.PlanId,
            this.colCliente,
            this.colPlan,
            this.colCantProv,
            this.Id});
            this.dgClientes.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgClientes.Location = new System.Drawing.Point(23, 72);
            this.dgClientes.MultiSelect = false;
            this.dgClientes.Name = "dgClientes";
            this.dgClientes.ReadOnly = true;
            this.dgClientes.Size = new System.Drawing.Size(557, 286);
            this.dgClientes.TabIndex = 2;
            this.dgClientes.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgClientes_RowEnter);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(273, 35);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 1;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(23, 35);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(243, 20);
            this.txtBuscar.TabIndex = 0;
            this.txtBuscar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBuscar_KeyDown);
            // 
            // tpGenerar
            // 
            this.tpGenerar.Controls.Add(this.lblLicId);
            this.tpGenerar.Controls.Add(this.txtLicencia);
            this.tpGenerar.Controls.Add(this.label4);
            this.tpGenerar.Controls.Add(this.btnSave);
            this.tpGenerar.Controls.Add(this.btnGenerar);
            this.tpGenerar.Controls.Add(this.txtCantMax);
            this.tpGenerar.Controls.Add(this.cmbPlan);
            this.tpGenerar.Controls.Add(this.label3);
            this.tpGenerar.Controls.Add(this.label2);
            this.tpGenerar.Controls.Add(this.cmbCliente);
            this.tpGenerar.Controls.Add(this.label1);
            this.tpGenerar.Location = new System.Drawing.Point(4, 22);
            this.tpGenerar.Name = "tpGenerar";
            this.tpGenerar.Padding = new System.Windows.Forms.Padding(3);
            this.tpGenerar.Size = new System.Drawing.Size(604, 410);
            this.tpGenerar.TabIndex = 1;
            this.tpGenerar.Text = "Generador";
            this.tpGenerar.UseVisualStyleBackColor = true;
            // 
            // txtLicencia
            // 
            this.txtLicencia.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtLicencia.Location = new System.Drawing.Point(194, 217);
            this.txtLicencia.Multiline = true;
            this.txtLicencia.Name = "txtLicencia";
            this.txtLicencia.ReadOnly = true;
            this.txtLicencia.Size = new System.Drawing.Size(219, 81);
            this.txtLicencia.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(74, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Licencia Generada";
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(194, 316);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(219, 54);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Guardar";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGenerar
            // 
            this.btnGenerar.Location = new System.Drawing.Point(194, 147);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(219, 49);
            this.btnGenerar.TabIndex = 4;
            this.btnGenerar.Text = "Generar Licencia";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // txtCantMax
            // 
            this.txtCantMax.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtCantMax.Location = new System.Drawing.Point(194, 108);
            this.txtCantMax.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.txtCantMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtCantMax.Name = "txtCantMax";
            this.txtCantMax.ReadOnly = true;
            this.txtCantMax.Size = new System.Drawing.Size(219, 20);
            this.txtCantMax.TabIndex = 3;
            this.txtCantMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCantMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmbPlan
            // 
            this.cmbPlan.FormattingEnabled = true;
            this.cmbPlan.Location = new System.Drawing.Point(194, 71);
            this.cmbPlan.Name = "cmbPlan";
            this.cmbPlan.Size = new System.Drawing.Size(219, 21);
            this.cmbPlan.TabIndex = 1;
            this.cmbPlan.SelectedIndexChanged += new System.EventHandler(this.cmbPlan_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(71, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Cantidad maxima";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(71, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Plan";
            // 
            // cmbCliente
            // 
            this.cmbCliente.FormattingEnabled = true;
            this.cmbCliente.Location = new System.Drawing.Point(194, 35);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(330, 21);
            this.cmbCliente.TabIndex = 1;
            this.cmbCliente.SelectedIndexChanged += new System.EventHandler(this.cmbCliente_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(71, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cliente";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(354, 35);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Limpiar";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Codigo
            // 
            this.Codigo.DataPropertyName = "Codigo";
            this.Codigo.HeaderText = "Codigo";
            this.Codigo.Name = "Codigo";
            this.Codigo.ReadOnly = true;
            this.Codigo.Visible = false;
            // 
            // Lic
            // 
            this.Lic.DataPropertyName = "Licencia";
            this.Lic.HeaderText = "Licencia";
            this.Lic.Name = "Lic";
            this.Lic.ReadOnly = true;
            this.Lic.Visible = false;
            // 
            // ClienteId
            // 
            this.ClienteId.DataPropertyName = "ClienteId";
            this.ClienteId.HeaderText = "ClienteId";
            this.ClienteId.Name = "ClienteId";
            this.ClienteId.ReadOnly = true;
            this.ClienteId.Visible = false;
            // 
            // PlanId
            // 
            this.PlanId.DataPropertyName = "PlanId";
            this.PlanId.HeaderText = "PlanId";
            this.PlanId.Name = "PlanId";
            this.PlanId.ReadOnly = true;
            this.PlanId.Visible = false;
            // 
            // colCliente
            // 
            this.colCliente.DataPropertyName = "Cliente";
            this.colCliente.HeaderText = "Nombre Cliente";
            this.colCliente.Name = "colCliente";
            this.colCliente.ReadOnly = true;
            this.colCliente.Width = 300;
            // 
            // colPlan
            // 
            this.colPlan.DataPropertyName = "Plan";
            this.colPlan.HeaderText = "Plan";
            this.colPlan.Name = "colPlan";
            this.colPlan.ReadOnly = true;
            // 
            // colCantProv
            // 
            this.colCantProv.DataPropertyName = "Cantidad";
            this.colCantProv.HeaderText = "Cantidad Proveedores";
            this.colCantProv.Name = "colCantProv";
            this.colCantProv.ReadOnly = true;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // lblLicId
            // 
            this.lblLicId.AutoSize = true;
            this.lblLicId.Location = new System.Drawing.Point(74, 4);
            this.lblLicId.Name = "lblLicId";
            this.lblLicId.Size = new System.Drawing.Size(0, 13);
            this.lblLicId.TabIndex = 7;
            this.lblLicId.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 450);
            this.Controls.Add(this.tabContenedor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Generador de Licencias";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tabContenedor.ResumeLayout(false);
            this.tpProveedor.ResumeLayout(false);
            this.tpProveedor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgClientes)).EndInit();
            this.tpGenerar.ResumeLayout(false);
            this.tpGenerar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCantMax)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabContenedor;
        private System.Windows.Forms.TabPage tpProveedor;
        private System.Windows.Forms.TabPage tpGenerar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.DataGridView dgClientes;
        private System.Windows.Forms.TextBox txtPagina;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.ComboBox cmbPlan;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbCliente;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown txtCantMax;
        private System.Windows.Forms.TextBox txtLicencia;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridViewTextBoxColumn Codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lic;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClienteId;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCantProv;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.Label lblLicId;
    }
}

