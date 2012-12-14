namespace WindowHelper
{
    partial class MapBasicWindowForm
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
            this._pictureBoxWindow = new System.Windows.Forms.PictureBox();
            this._panelWindow = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBoxWindow)).BeginInit();
            this._panelWindow.SuspendLayout();
            this.SuspendLayout();
            // 
            // _pictureBoxWindow
            // 
            this._pictureBoxWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pictureBoxWindow.Location = new System.Drawing.Point(0, 0);
            this._pictureBoxWindow.Name = "_pictureBoxWindow";
            this._pictureBoxWindow.Size = new System.Drawing.Size(285, 267);
            this._pictureBoxWindow.TabIndex = 1;
            this._pictureBoxWindow.TabStop = false;
            this._pictureBoxWindow.LocationChanged += new System.EventHandler(this.MapBasicWindowForm_LocationChanged);
            this._pictureBoxWindow.DockChanged += new System.EventHandler(this.MapBasicWindowForm_DockChanged);
            // 
            // _panelWindow
            // 
            this._panelWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._panelWindow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._panelWindow.Controls.Add(this._pictureBoxWindow);
            this._panelWindow.Location = new System.Drawing.Point(4, 3);
            this._panelWindow.Name = "_panelWindow";
            this._panelWindow.Size = new System.Drawing.Size(285, 267);
            this._panelWindow.TabIndex = 3;
            // 
            // MapBasicWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._panelWindow);
            this.Name = "MapBasicWindowForm";
            this.Size = new System.Drawing.Size(292, 273);
            this.Load += new System.EventHandler(this.MapBasicWindowForm_Load);
            this.Resize += new System.EventHandler(this.MapBasicWindowForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this._pictureBoxWindow)).EndInit();
            this._panelWindow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _pictureBoxWindow;
        private System.Windows.Forms.Panel _panelWindow;

    }
}