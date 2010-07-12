namespace AltosTools.WindowsForms.Controls
{
    partial class ExtendedCheckedListBox
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ExtendedCheckedListBox
            // 
            this.Size = new System.Drawing.Size(120, 94);
            this.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ExtendedCheckedListBox_ItemCheck);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
