/*
 * Creado por SharpDevelop.
 * Usuario: Supernato
 * Fecha: 06/05/2010
 * Hora: 07:28 p.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
namespace EditLoc
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLanguageName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpEdit = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtOriginal = new System.Windows.Forms.TextBox();
            this.txtTranslated = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnReadFile = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.grdLanguages = new System.Windows.Forms.DataGridView();
            this.Original = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Translated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboAssociatedCulture = new System.Windows.Forms.ComboBox();
            this.lblAssociatedCulture = new System.Windows.Forms.Label();
            this.btnLoadOriginal = new System.Windows.Forms.Button();
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.grpEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLanguages)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // FileName
            // 
            this.FileName.HeaderText = "";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Language Name";
            // 
            // txtLanguageName
            // 
            this.txtLanguageName.Location = new System.Drawing.Point(6, 32);
            this.txtLanguageName.Name = "txtLanguageName";
            this.txtLanguageName.Size = new System.Drawing.Size(386, 20);
            this.txtLanguageName.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grpEdit);
            this.groupBox1.Controls.Add(this.btnReadFile);
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.btnNew);
            this.groupBox1.Controls.Add(this.grdLanguages);
            this.groupBox1.Location = new System.Drawing.Point(13, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 440);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Strings (F2 to edit - F3 to add)";
            // 
            // grpEdit
            // 
            this.grpEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.grpEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grpEdit.Controls.Add(this.btnCancel);
            this.grpEdit.Controls.Add(this.btnOK);
            this.grpEdit.Controls.Add(this.txtOriginal);
            this.grpEdit.Controls.Add(this.txtTranslated);
            this.grpEdit.Controls.Add(this.label4);
            this.grpEdit.Controls.Add(this.label3);
            this.grpEdit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grpEdit.Location = new System.Drawing.Point(22, 169);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.Size = new System.Drawing.Size(639, 111);
            this.grpEdit.TabIndex = 4;
            this.grpEdit.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(588, 57);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(33, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "X";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(549, 57);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(33, 23);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOKClick);
            // 
            // txtOriginal
            // 
            this.txtOriginal.Location = new System.Drawing.Point(79, 31);
            this.txtOriginal.Name = "txtOriginal";
            this.txtOriginal.Size = new System.Drawing.Size(542, 20);
            this.txtOriginal.TabIndex = 14;
            // 
            // txtTranslated
            // 
            this.txtTranslated.Location = new System.Drawing.Point(79, 57);
            this.txtTranslated.Name = "txtTranslated";
            this.txtTranslated.Size = new System.Drawing.Size(464, 20);
            this.txtTranslated.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Translated:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Original:";
            // 
            // btnReadFile
            // 
            this.btnReadFile.Location = new System.Drawing.Point(687, 120);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(66, 23);
            this.btnReadFile.TabIndex = 4;
            this.btnReadFile.Text = "Read File";
            this.btnReadFile.UseVisualStyleBackColor = true;
            this.btnReadFile.Click += new System.EventHandler(this.BtnReadFileClick);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(687, 77);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(66, 23);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemoveClick);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(687, 48);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(66, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEditClick);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(687, 19);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(66, 23);
            this.btnNew.TabIndex = 3;
            this.btnNew.Text = "Add";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.BtnNewClick);
            // 
            // grdLanguages
            // 
            this.grdLanguages.AllowUserToAddRows = false;
            this.grdLanguages.AllowUserToDeleteRows = false;
            this.grdLanguages.AllowUserToResizeRows = false;
            this.grdLanguages.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.grdLanguages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdLanguages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Original,
            this.Translated});
            this.grdLanguages.Location = new System.Drawing.Point(6, 19);
            this.grdLanguages.Name = "grdLanguages";
            this.grdLanguages.ReadOnly = true;
            this.grdLanguages.RowHeadersVisible = false;
            this.grdLanguages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdLanguages.Size = new System.Drawing.Size(675, 415);
            this.grdLanguages.TabIndex = 3;
            this.grdLanguages.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GrdLanguagesCellDoubleClick);
            // 
            // Original
            // 
            this.Original.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Original.HeaderText = "Original";
            this.Original.Name = "Original";
            this.Original.ReadOnly = true;
            // 
            // Translated
            // 
            this.Translated.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Translated.HeaderText = "Translated";
            this.Translated.Name = "Translated";
            this.Translated.ReadOnly = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(690, 527);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(19, 527);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 23);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExitClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboAssociatedCulture);
            this.groupBox2.Controls.Add(this.lblAssociatedCulture);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtLanguageName);
            this.groupBox2.Controls.Add(this.btnLoadOriginal);
            this.groupBox2.Location = new System.Drawing.Point(13, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(759, 63);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Information";
            // 
            // cboAssociatedCulture
            // 
            this.cboAssociatedCulture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAssociatedCulture.FormattingEnabled = true;
            this.cboAssociatedCulture.Location = new System.Drawing.Point(410, 32);
            this.cboAssociatedCulture.Name = "cboAssociatedCulture";
            this.cboAssociatedCulture.Size = new System.Drawing.Size(234, 21);
            this.cboAssociatedCulture.TabIndex = 5;
            // 
            // lblAssociatedCulture
            // 
            this.lblAssociatedCulture.AutoSize = true;
            this.lblAssociatedCulture.Location = new System.Drawing.Point(407, 16);
            this.lblAssociatedCulture.Name = "lblAssociatedCulture";
            this.lblAssociatedCulture.Size = new System.Drawing.Size(92, 13);
            this.lblAssociatedCulture.TabIndex = 4;
            this.lblAssociatedCulture.Text = "AssociatedCulture";
            // 
            // btnLoadOriginal
            // 
            this.btnLoadOriginal.Location = new System.Drawing.Point(656, 29);
            this.btnLoadOriginal.Name = "btnLoadOriginal";
            this.btnLoadOriginal.Size = new System.Drawing.Size(97, 23);
            this.btnLoadOriginal.TabIndex = 2;
            this.btnLoadOriginal.Text = "Open";
            this.btnLoadOriginal.UseVisualStyleBackColor = true;
            this.btnLoadOriginal.Click += new System.EventHandler(this.BtnLoadOriginalClick);
            // 
            // OpenDialog
            // 
            this.OpenDialog.Title = "Open Original File";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "EditLoc";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyUp);
            this.groupBox1.ResumeLayout(false);
            this.grpEdit.ResumeLayout(false);
            this.grpEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLanguages)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button btnReadFile;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.SaveFileDialog SaveDialog;
		private System.Windows.Forms.Panel grpEdit;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.TextBox txtTranslated;
		private System.Windows.Forms.TextBox txtOriginal;
		private System.Windows.Forms.TextBox txtLanguageName;
		private System.Windows.Forms.Button btnLoadOriginal;
		private System.Windows.Forms.OpenFileDialog OpenDialog;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.DataGridViewTextBoxColumn Translated;
		private System.Windows.Forms.DataGridViewTextBoxColumn Original;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.DataGridView grdLanguages;
		private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.Label lblAssociatedCulture;
        private System.Windows.Forms.ComboBox cboAssociatedCulture;
		
	}
}
