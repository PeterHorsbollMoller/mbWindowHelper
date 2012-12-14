namespace WindowHelper
{
    partial class WindowHelperForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowHelperForm));
            this._buttonCloseWindow = new System.Windows.Forms.Button();
            this._buttonCloneWindow = new System.Windows.Forms.Button();
            this._buttonLockWindow = new System.Windows.Forms.Button();
            this._buttonUnlockWindow = new System.Windows.Forms.Button();
            this._panelWindowsTop = new System.Windows.Forms.Panel();
            this._labelExtentIndex = new System.Windows.Forms.Label();
            this._labelActiveWindowTitle = new System.Windows.Forms.Label();
            this._buttonZoomToLast = new System.Windows.Forms.Button();
            this._buttonZoomToNext = new System.Windows.Forms.Button();
            this._buttonZoomToPrevious = new System.Windows.Forms.Button();
            this._buttonZoomToFirst = new System.Windows.Forms.Button();
            this._buttonCurrentOptions = new System.Windows.Forms.Button();
            this._checkBoxAutolockWindows = new System.Windows.Forms.CheckBox();
            this._buttonCurrentMakeDockable = new System.Windows.Forms.Button();
            this._panelMainBottom = new System.Windows.Forms.Panel();
            this._tabControlWindows = new System.Windows.Forms.TabControl();
            this._tabPageCurrentWindows = new System.Windows.Forms.TabPage();
            this._panelWindowsBottom = new System.Windows.Forms.Panel();
            this._treeViewWindows = new System.Windows.Forms.TreeView();
            this._tabPageClosedWindows = new System.Windows.Forms.TabPage();
            this._panelClosedWindowsBottom = new System.Windows.Forms.Panel();
            this._treeViewClosedWindows = new System.Windows.Forms.TreeView();
            this._panelClosedWindowsTop = new System.Windows.Forms.Panel();
            this._buttonRestoreWindow = new System.Windows.Forms.Button();
            this._tabPageSpecialWindows = new System.Windows.Forms.TabPage();
            this._panelSpecialWindowsBottom = new System.Windows.Forms.Panel();
            this._treeViewSpecialWindows = new System.Windows.Forms.TreeView();
            this._panelSpecialWindowsTop = new System.Windows.Forms.Panel();
            this._buttonMakeDockable = new System.Windows.Forms.Button();
            this._panelWindowsTop.SuspendLayout();
            this._panelMainBottom.SuspendLayout();
            this._tabControlWindows.SuspendLayout();
            this._tabPageCurrentWindows.SuspendLayout();
            this._panelWindowsBottom.SuspendLayout();
            this._tabPageClosedWindows.SuspendLayout();
            this._panelClosedWindowsBottom.SuspendLayout();
            this._panelClosedWindowsTop.SuspendLayout();
            this._tabPageSpecialWindows.SuspendLayout();
            this._panelSpecialWindowsBottom.SuspendLayout();
            this._panelSpecialWindowsTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // _buttonCloseWindow
            // 
            this._buttonCloseWindow.Image = ((System.Drawing.Image)(resources.GetObject("_buttonCloseWindow.Image")));
            this._buttonCloseWindow.Location = new System.Drawing.Point(104, 3);
            this._buttonCloseWindow.Name = "_buttonCloseWindow";
            this._buttonCloseWindow.Size = new System.Drawing.Size(24, 24);
            this._buttonCloseWindow.TabIndex = 0;
            this._buttonCloseWindow.UseVisualStyleBackColor = true;
            this._buttonCloseWindow.Click += new System.EventHandler(this._buttonCloseWindow_Click);
            // 
            // _buttonCloneWindow
            // 
            this._buttonCloneWindow.Image = ((System.Drawing.Image)(resources.GetObject("_buttonCloneWindow.Image")));
            this._buttonCloneWindow.Location = new System.Drawing.Point(0, 3);
            this._buttonCloneWindow.Name = "_buttonCloneWindow";
            this._buttonCloneWindow.Size = new System.Drawing.Size(24, 24);
            this._buttonCloneWindow.TabIndex = 1;
            this._buttonCloneWindow.UseVisualStyleBackColor = true;
            this._buttonCloneWindow.Click += new System.EventHandler(this._buttonCloneWindow_Click);
            // 
            // _buttonLockWindow
            // 
            this._buttonLockWindow.Image = ((System.Drawing.Image)(resources.GetObject("_buttonLockWindow.Image")));
            this._buttonLockWindow.Location = new System.Drawing.Point(28, 3);
            this._buttonLockWindow.Name = "_buttonLockWindow";
            this._buttonLockWindow.Size = new System.Drawing.Size(24, 24);
            this._buttonLockWindow.TabIndex = 4;
            this._buttonLockWindow.UseVisualStyleBackColor = true;
            this._buttonLockWindow.Click += new System.EventHandler(this._buttonLockWindow_Click);
            // 
            // _buttonUnlockWindow
            // 
            this._buttonUnlockWindow.Image = ((System.Drawing.Image)(resources.GetObject("_buttonUnlockWindow.Image")));
            this._buttonUnlockWindow.Location = new System.Drawing.Point(52, 3);
            this._buttonUnlockWindow.Name = "_buttonUnlockWindow";
            this._buttonUnlockWindow.Size = new System.Drawing.Size(24, 24);
            this._buttonUnlockWindow.TabIndex = 5;
            this._buttonUnlockWindow.UseVisualStyleBackColor = true;
            this._buttonUnlockWindow.Click += new System.EventHandler(this._buttonUnlockWindow_Click);
            // 
            // _panelWindowsTop
            // 
            this._panelWindowsTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelWindowsTop.Controls.Add(this._labelExtentIndex);
            this._panelWindowsTop.Controls.Add(this._labelActiveWindowTitle);
            this._panelWindowsTop.Controls.Add(this._buttonZoomToLast);
            this._panelWindowsTop.Controls.Add(this._buttonZoomToNext);
            this._panelWindowsTop.Controls.Add(this._buttonZoomToPrevious);
            this._panelWindowsTop.Controls.Add(this._buttonZoomToFirst);
            this._panelWindowsTop.Controls.Add(this._buttonCurrentOptions);
            this._panelWindowsTop.Controls.Add(this._checkBoxAutolockWindows);
            this._panelWindowsTop.Controls.Add(this._buttonCurrentMakeDockable);
            this._panelWindowsTop.Controls.Add(this._buttonUnlockWindow);
            this._panelWindowsTop.Controls.Add(this._buttonCloseWindow);
            this._panelWindowsTop.Controls.Add(this._buttonLockWindow);
            this._panelWindowsTop.Controls.Add(this._buttonCloneWindow);
            this._panelWindowsTop.Location = new System.Drawing.Point(3, 3);
            this._panelWindowsTop.Name = "_panelWindowsTop";
            this._panelWindowsTop.Size = new System.Drawing.Size(219, 68);
            this._panelWindowsTop.TabIndex = 6;
            // 
            // _labelExtentIndex
            // 
            this._labelExtentIndex.AutoSize = true;
            this._labelExtentIndex.Location = new System.Drawing.Point(100, 36);
            this._labelExtentIndex.Name = "_labelExtentIndex";
            this._labelExtentIndex.Size = new System.Drawing.Size(99, 13);
            this._labelExtentIndex.TabIndex = 15;
            this._labelExtentIndex.Text = "window of windows";
            // 
            // _labelActiveWindowTitle
            // 
            this._labelActiveWindowTitle.AutoSize = true;
            this._labelActiveWindowTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelActiveWindowTitle.Location = new System.Drawing.Point(5, 57);
            this._labelActiveWindowTitle.Name = "_labelActiveWindowTitle";
            this._labelActiveWindowTitle.Size = new System.Drawing.Size(0, 13);
            this._labelActiveWindowTitle.TabIndex = 14;
            // 
            // _buttonZoomToLast
            // 
            this._buttonZoomToLast.Image = ((System.Drawing.Image)(resources.GetObject("_buttonZoomToLast.Image")));
            this._buttonZoomToLast.Location = new System.Drawing.Point(70, 30);
            this._buttonZoomToLast.Name = "_buttonZoomToLast";
            this._buttonZoomToLast.Size = new System.Drawing.Size(24, 24);
            this._buttonZoomToLast.TabIndex = 13;
            this._buttonZoomToLast.UseVisualStyleBackColor = true;
            this._buttonZoomToLast.Click += new System.EventHandler(this._buttonZoomToLast_Click);
            // 
            // _buttonZoomToNext
            // 
            this._buttonZoomToNext.Image = ((System.Drawing.Image)(resources.GetObject("_buttonZoomToNext.Image")));
            this._buttonZoomToNext.Location = new System.Drawing.Point(46, 30);
            this._buttonZoomToNext.Name = "_buttonZoomToNext";
            this._buttonZoomToNext.Size = new System.Drawing.Size(24, 24);
            this._buttonZoomToNext.TabIndex = 12;
            this._buttonZoomToNext.UseVisualStyleBackColor = true;
            this._buttonZoomToNext.Click += new System.EventHandler(this._buttonZoomToNext_Click);
            // 
            // _buttonZoomToPrevious
            // 
            this._buttonZoomToPrevious.Image = ((System.Drawing.Image)(resources.GetObject("_buttonZoomToPrevious.Image")));
            this._buttonZoomToPrevious.Location = new System.Drawing.Point(22, 30);
            this._buttonZoomToPrevious.Name = "_buttonZoomToPrevious";
            this._buttonZoomToPrevious.Size = new System.Drawing.Size(24, 24);
            this._buttonZoomToPrevious.TabIndex = 11;
            this._buttonZoomToPrevious.UseVisualStyleBackColor = true;
            this._buttonZoomToPrevious.Click += new System.EventHandler(this._buttonZoomToPrevious_Click);
            // 
            // _buttonZoomToFirst
            // 
            this._buttonZoomToFirst.Image = ((System.Drawing.Image)(resources.GetObject("_buttonZoomToFirst.Image")));
            this._buttonZoomToFirst.Location = new System.Drawing.Point(0, 30);
            this._buttonZoomToFirst.Name = "_buttonZoomToFirst";
            this._buttonZoomToFirst.Size = new System.Drawing.Size(24, 24);
            this._buttonZoomToFirst.TabIndex = 10;
            this._buttonZoomToFirst.UseVisualStyleBackColor = true;
            this._buttonZoomToFirst.Click += new System.EventHandler(this._buttonZoomToFirst_Click);
            // 
            // _buttonCurrentOptions
            // 
            this._buttonCurrentOptions.Image = ((System.Drawing.Image)(resources.GetObject("_buttonCurrentOptions.Image")));
            this._buttonCurrentOptions.Location = new System.Drawing.Point(134, 3);
            this._buttonCurrentOptions.Name = "_buttonCurrentOptions";
            this._buttonCurrentOptions.Size = new System.Drawing.Size(24, 24);
            this._buttonCurrentOptions.TabIndex = 9;
            this._buttonCurrentOptions.UseVisualStyleBackColor = true;
            this._buttonCurrentOptions.Click += new System.EventHandler(this._buttonCurrentOptions_Click);
            // 
            // _checkBoxAutolockWindows
            // 
            this._checkBoxAutolockWindows.Appearance = System.Windows.Forms.Appearance.Button;
            this._checkBoxAutolockWindows.Image = ((System.Drawing.Image)(resources.GetObject("_checkBoxAutolockWindows.Image")));
            this._checkBoxAutolockWindows.Location = new System.Drawing.Point(76, 3);
            this._checkBoxAutolockWindows.Name = "_checkBoxAutolockWindows";
            this._checkBoxAutolockWindows.Size = new System.Drawing.Size(24, 24);
            this._checkBoxAutolockWindows.TabIndex = 8;
            this._checkBoxAutolockWindows.UseVisualStyleBackColor = true;
            this._checkBoxAutolockWindows.CheckedChanged += new System.EventHandler(this._checkBoxAutolockWindows_CheckedChanged);
            // 
            // _buttonCurrentMakeDockable
            // 
            this._buttonCurrentMakeDockable.Image = ((System.Drawing.Image)(resources.GetObject("_buttonCurrentMakeDockable.Image")));
            this._buttonCurrentMakeDockable.Location = new System.Drawing.Point(164, 3);
            this._buttonCurrentMakeDockable.Name = "_buttonCurrentMakeDockable";
            this._buttonCurrentMakeDockable.Size = new System.Drawing.Size(24, 24);
            this._buttonCurrentMakeDockable.TabIndex = 7;
            this._buttonCurrentMakeDockable.UseVisualStyleBackColor = true;
            this._buttonCurrentMakeDockable.Click += new System.EventHandler(this._buttonCurrentMakeDockable_Click);
            // 
            // _panelMainBottom
            // 
            this._panelMainBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelMainBottom.Controls.Add(this._tabControlWindows);
            this._panelMainBottom.Location = new System.Drawing.Point(0, 0);
            this._panelMainBottom.Name = "_panelMainBottom";
            this._panelMainBottom.Size = new System.Drawing.Size(233, 273);
            this._panelMainBottom.TabIndex = 7;
            // 
            // _tabControlWindows
            // 
            this._tabControlWindows.Controls.Add(this._tabPageCurrentWindows);
            this._tabControlWindows.Controls.Add(this._tabPageClosedWindows);
            this._tabControlWindows.Controls.Add(this._tabPageSpecialWindows);
            this._tabControlWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControlWindows.Location = new System.Drawing.Point(0, 0);
            this._tabControlWindows.Name = "_tabControlWindows";
            this._tabControlWindows.SelectedIndex = 0;
            this._tabControlWindows.Size = new System.Drawing.Size(233, 273);
            this._tabControlWindows.TabIndex = 0;
            // 
            // _tabPageCurrentWindows
            // 
            this._tabPageCurrentWindows.Controls.Add(this._panelWindowsBottom);
            this._tabPageCurrentWindows.Controls.Add(this._panelWindowsTop);
            this._tabPageCurrentWindows.Location = new System.Drawing.Point(4, 22);
            this._tabPageCurrentWindows.Name = "_tabPageCurrentWindows";
            this._tabPageCurrentWindows.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageCurrentWindows.Size = new System.Drawing.Size(225, 247);
            this._tabPageCurrentWindows.TabIndex = 0;
            this._tabPageCurrentWindows.Text = "Current";
            this._tabPageCurrentWindows.UseVisualStyleBackColor = true;
            // 
            // _panelWindowsBottom
            // 
            this._panelWindowsBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelWindowsBottom.Controls.Add(this._treeViewWindows);
            this._panelWindowsBottom.Location = new System.Drawing.Point(3, 77);
            this._panelWindowsBottom.Name = "_panelWindowsBottom";
            this._panelWindowsBottom.Size = new System.Drawing.Size(219, 167);
            this._panelWindowsBottom.TabIndex = 2;
            // 
            // _treeViewWindows
            // 
            this._treeViewWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeViewWindows.Location = new System.Drawing.Point(0, 0);
            this._treeViewWindows.Name = "_treeViewWindows";
            this._treeViewWindows.Size = new System.Drawing.Size(219, 167);
            this._treeViewWindows.TabIndex = 0;
            this._treeViewWindows.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._treeViewWindows_NodeMouseClick);
            this._treeViewWindows.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._treeViewWindows_NodeMouseDoubleClick);
            // 
            // _tabPageClosedWindows
            // 
            this._tabPageClosedWindows.Controls.Add(this._panelClosedWindowsBottom);
            this._tabPageClosedWindows.Controls.Add(this._panelClosedWindowsTop);
            this._tabPageClosedWindows.Location = new System.Drawing.Point(4, 22);
            this._tabPageClosedWindows.Name = "_tabPageClosedWindows";
            this._tabPageClosedWindows.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageClosedWindows.Size = new System.Drawing.Size(225, 247);
            this._tabPageClosedWindows.TabIndex = 1;
            this._tabPageClosedWindows.Text = "Closed";
            this._tabPageClosedWindows.UseVisualStyleBackColor = true;
            // 
            // _panelClosedWindowsBottom
            // 
            this._panelClosedWindowsBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelClosedWindowsBottom.Controls.Add(this._treeViewClosedWindows);
            this._panelClosedWindowsBottom.Location = new System.Drawing.Point(3, 36);
            this._panelClosedWindowsBottom.Name = "_panelClosedWindowsBottom";
            this._panelClosedWindowsBottom.Size = new System.Drawing.Size(219, 208);
            this._panelClosedWindowsBottom.TabIndex = 8;
            // 
            // _treeViewClosedWindows
            // 
            this._treeViewClosedWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeViewClosedWindows.Location = new System.Drawing.Point(0, 0);
            this._treeViewClosedWindows.Name = "_treeViewClosedWindows";
            this._treeViewClosedWindows.Size = new System.Drawing.Size(219, 208);
            this._treeViewClosedWindows.TabIndex = 0;
            this._treeViewClosedWindows.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._treeviewClosedWindows_NodeMouseDoubleClick);
            // 
            // _panelClosedWindowsTop
            // 
            this._panelClosedWindowsTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelClosedWindowsTop.Controls.Add(this._buttonRestoreWindow);
            this._panelClosedWindowsTop.Location = new System.Drawing.Point(3, 3);
            this._panelClosedWindowsTop.Name = "_panelClosedWindowsTop";
            this._panelClosedWindowsTop.Size = new System.Drawing.Size(219, 32);
            this._panelClosedWindowsTop.TabIndex = 7;
            // 
            // _buttonRestoreWindow
            // 
            this._buttonRestoreWindow.Image = ((System.Drawing.Image)(resources.GetObject("_buttonRestoreWindow.Image")));
            this._buttonRestoreWindow.Location = new System.Drawing.Point(0, 3);
            this._buttonRestoreWindow.Name = "_buttonRestoreWindow";
            this._buttonRestoreWindow.Size = new System.Drawing.Size(24, 24);
            this._buttonRestoreWindow.TabIndex = 2;
            this._buttonRestoreWindow.UseVisualStyleBackColor = true;
            this._buttonRestoreWindow.Click += new System.EventHandler(this._buttonRestoreWindow_Click);
            // 
            // _tabPageSpecialWindows
            // 
            this._tabPageSpecialWindows.Controls.Add(this._panelSpecialWindowsBottom);
            this._tabPageSpecialWindows.Controls.Add(this._panelSpecialWindowsTop);
            this._tabPageSpecialWindows.Location = new System.Drawing.Point(4, 22);
            this._tabPageSpecialWindows.Name = "_tabPageSpecialWindows";
            this._tabPageSpecialWindows.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageSpecialWindows.Size = new System.Drawing.Size(225, 247);
            this._tabPageSpecialWindows.TabIndex = 2;
            this._tabPageSpecialWindows.Text = "Special";
            this._tabPageSpecialWindows.UseVisualStyleBackColor = true;
            // 
            // _panelSpecialWindowsBottom
            // 
            this._panelSpecialWindowsBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelSpecialWindowsBottom.Controls.Add(this._treeViewSpecialWindows);
            this._panelSpecialWindowsBottom.Location = new System.Drawing.Point(3, 33);
            this._panelSpecialWindowsBottom.Name = "_panelSpecialWindowsBottom";
            this._panelSpecialWindowsBottom.Size = new System.Drawing.Size(219, 211);
            this._panelSpecialWindowsBottom.TabIndex = 9;
            // 
            // _treeViewSpecialWindows
            // 
            this._treeViewSpecialWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeViewSpecialWindows.Location = new System.Drawing.Point(0, 0);
            this._treeViewSpecialWindows.Name = "_treeViewSpecialWindows";
            this._treeViewSpecialWindows.Size = new System.Drawing.Size(219, 211);
            this._treeViewSpecialWindows.TabIndex = 0;
            this._treeViewSpecialWindows.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._treeViewSpecialWindows_NodeMouseClick);
            this._treeViewSpecialWindows.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._treeViewSpecialWindows_NodeMouseDoubleClick);
            // 
            // _panelSpecialWindowsTop
            // 
            this._panelSpecialWindowsTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._panelSpecialWindowsTop.Controls.Add(this._buttonMakeDockable);
            this._panelSpecialWindowsTop.Location = new System.Drawing.Point(3, 0);
            this._panelSpecialWindowsTop.Name = "_panelSpecialWindowsTop";
            this._panelSpecialWindowsTop.Size = new System.Drawing.Size(219, 32);
            this._panelSpecialWindowsTop.TabIndex = 8;
            // 
            // _buttonMakeDockable
            // 
            this._buttonMakeDockable.Image = ((System.Drawing.Image)(resources.GetObject("_buttonMakeDockable.Image")));
            this._buttonMakeDockable.Location = new System.Drawing.Point(0, 3);
            this._buttonMakeDockable.Name = "_buttonMakeDockable";
            this._buttonMakeDockable.Size = new System.Drawing.Size(24, 24);
            this._buttonMakeDockable.TabIndex = 2;
            this._buttonMakeDockable.UseVisualStyleBackColor = true;
            this._buttonMakeDockable.Click += new System.EventHandler(this._buttonMakeDockable_Click);
            // 
            // WindowHelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._panelMainBottom);
            this.Name = "WindowHelperForm";
            this.Size = new System.Drawing.Size(233, 273);
            this.DockChanged += new System.EventHandler(this.WindowHelperForm_DockChanged);
            this.LocationChanged += new System.EventHandler(this.WindowHelperForm_LocationChanged);
            this._panelWindowsTop.ResumeLayout(false);
            this._panelWindowsTop.PerformLayout();
            this._panelMainBottom.ResumeLayout(false);
            this._tabControlWindows.ResumeLayout(false);
            this._tabPageCurrentWindows.ResumeLayout(false);
            this._panelWindowsBottom.ResumeLayout(false);
            this._tabPageClosedWindows.ResumeLayout(false);
            this._panelClosedWindowsBottom.ResumeLayout(false);
            this._panelClosedWindowsTop.ResumeLayout(false);
            this._tabPageSpecialWindows.ResumeLayout(false);
            this._panelSpecialWindowsBottom.ResumeLayout(false);
            this._panelSpecialWindowsTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _buttonCloseWindow;
        private System.Windows.Forms.Button _buttonCloneWindow;
        private System.Windows.Forms.Button _buttonLockWindow;
        private System.Windows.Forms.Button _buttonUnlockWindow;
        private System.Windows.Forms.Panel _panelWindowsTop;
        private System.Windows.Forms.Panel _panelMainBottom;
        private System.Windows.Forms.TabControl _tabControlWindows;
        private System.Windows.Forms.TabPage _tabPageCurrentWindows;
        private System.Windows.Forms.TabPage _tabPageClosedWindows;
        private System.Windows.Forms.TabPage _tabPageSpecialWindows;
        private System.Windows.Forms.TreeView _treeViewWindows;
        private System.Windows.Forms.TreeView _treeViewSpecialWindows;
        private System.Windows.Forms.Panel _panelWindowsBottom;
        private System.Windows.Forms.Panel _panelClosedWindowsBottom;
        private System.Windows.Forms.TreeView _treeViewClosedWindows;
        private System.Windows.Forms.Panel _panelClosedWindowsTop;
        private System.Windows.Forms.Button _buttonRestoreWindow;
        private System.Windows.Forms.Panel _panelSpecialWindowsBottom;
        private System.Windows.Forms.Panel _panelSpecialWindowsTop;
        private System.Windows.Forms.Button _buttonMakeDockable;
        private System.Windows.Forms.Button _buttonCurrentMakeDockable;
        private System.Windows.Forms.CheckBox _checkBoxAutolockWindows;
        private System.Windows.Forms.Button _buttonCurrentOptions;
        private System.Windows.Forms.Button _buttonZoomToLast;
        private System.Windows.Forms.Button _buttonZoomToNext;
        private System.Windows.Forms.Button _buttonZoomToPrevious;
        private System.Windows.Forms.Button _buttonZoomToFirst;
        private System.Windows.Forms.Label _labelActiveWindowTitle;
        private System.Windows.Forms.Label _labelExtentIndex;
    }
}