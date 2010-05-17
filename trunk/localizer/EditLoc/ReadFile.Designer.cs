/*
 * Created by SharpDevelop.
 * User: NGRUIZ
 * Date: 5/17/2010
 * Time: 9:14 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace EditLoc
{
	partial class ReadFile
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.grdCriteria = new System.Windows.Forms.DataGridView();
			this.begins = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ends = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnAdd = new System.Windows.Forms.Button();
			this.txtEnds = new System.Windows.Forms.TextBox();
			this.txtBegins = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdReadFile = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.OpenFile = new System.Windows.Forms.OpenFileDialog();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdCriteria)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnBrowse);
			this.groupBox1.Controls.Add(this.txtFileName);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(414, 51);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "File Name";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(333, 20);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.BtnBrowseClick);
			// 
			// txtFileName
			// 
			this.txtFileName.Location = new System.Drawing.Point(7, 20);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(320, 20);
			this.txtFileName.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.grdCriteria);
			this.groupBox2.Controls.Add(this.btnAdd);
			this.groupBox2.Controls.Add(this.txtEnds);
			this.groupBox2.Controls.Add(this.txtBegins);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Location = new System.Drawing.Point(12, 70);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(414, 244);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Criteria";
			// 
			// grdCriteria
			// 
			this.grdCriteria.AllowUserToAddRows = false;
			this.grdCriteria.AllowUserToDeleteRows = false;
			this.grdCriteria.AllowUserToResizeColumns = false;
			this.grdCriteria.AllowUserToResizeRows = false;
			this.grdCriteria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdCriteria.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.begins,
									this.ends});
			this.grdCriteria.Location = new System.Drawing.Point(7, 64);
			this.grdCriteria.MultiSelect = false;
			this.grdCriteria.Name = "grdCriteria";
			this.grdCriteria.ReadOnly = true;
			this.grdCriteria.RowHeadersVisible = false;
			this.grdCriteria.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grdCriteria.Size = new System.Drawing.Size(401, 160);
			this.grdCriteria.TabIndex = 5;
			this.grdCriteria.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GrdCriteriaCellMouseDoubleClick);
			// 
			// begins
			// 
			this.begins.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.begins.HeaderText = "Begins with";
			this.begins.Name = "begins";
			this.begins.ReadOnly = true;
			this.begins.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// ends
			// 
			this.ends.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ends.HeaderText = "Ends with";
			this.ends.Name = "ends";
			this.ends.ReadOnly = true;
			this.ends.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(325, 37);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(83, 22);
			this.btnAdd.TabIndex = 4;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.BtnAddClick);
			// 
			// txtEnds
			// 
			this.txtEnds.Location = new System.Drawing.Point(168, 37);
			this.txtEnds.Name = "txtEnds";
			this.txtEnds.Size = new System.Drawing.Size(151, 20);
			this.txtEnds.TabIndex = 3;
			// 
			// txtBegins
			// 
			this.txtBegins.Location = new System.Drawing.Point(7, 37);
			this.txtBegins.Name = "txtBegins";
			this.txtBegins.Size = new System.Drawing.Size(151, 20);
			this.txtBegins.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(168, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Ends with";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 227);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(401, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Double click to remove an item";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Begins with";
			// 
			// cmdReadFile
			// 
			this.cmdReadFile.Location = new System.Drawing.Point(350, 321);
			this.cmdReadFile.Name = "cmdReadFile";
			this.cmdReadFile.Size = new System.Drawing.Size(75, 23);
			this.cmdReadFile.TabIndex = 7;
			this.cmdReadFile.Text = "Read File";
			this.cmdReadFile.UseVisualStyleBackColor = true;
			this.cmdReadFile.Click += new System.EventHandler(this.CmdReadFileClick);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(12, 320);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 2;
			this.button4.Text = "Close";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.Button4Click);
			// 
			// OpenFile
			// 
			this.OpenFile.FileName = "openFileDialog1";
			// 
			// ReadFile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(438, 362);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.cmdReadFile);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "ReadFile";
			this.Text = "ReadFile";
			this.Load += new System.EventHandler(this.ReadFileLoad);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdCriteria)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.DataGridViewTextBoxColumn ends;
		private System.Windows.Forms.DataGridViewTextBoxColumn begins;
		private System.Windows.Forms.DataGridView grdCriteria;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.OpenFileDialog OpenFile;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button cmdReadFile;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtBegins;
		private System.Windows.Forms.TextBox txtEnds;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}
