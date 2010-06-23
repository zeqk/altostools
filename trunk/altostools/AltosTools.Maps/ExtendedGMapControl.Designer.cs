namespace AltosTools.WindowsForms.Maps
{
    partial class ExtendedGMapControl
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
            // ExtendedGMapControl
            // 
            this.Name = "ExtendedGMapControl";
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ExtendedGMapControl_MouseDoubleClick);
            this.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.ExtendedGMapControl_OnMarkerClick);
            this.OnMarkerEnter += new GMap.NET.WindowsForms.MarkerEnter(this.ExtendedGMapControl_OnMarkerEnter);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ExtendedGMapControl_MouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ExtendedGMapControl_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ExtendedGMapControl_MouseDown);
            this.OnMarkerLeave += new GMap.NET.WindowsForms.MarkerLeave(this.ExtendedGMapControl_OnMarkerLeave);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ExtendedGMapControl_MouseDoubleClick);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
