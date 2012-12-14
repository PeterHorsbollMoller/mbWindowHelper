namespace WindowHelper
{
    partial class ModifyWindowForm
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
            this._labelWindowName = new System.Windows.Forms.Label();
            this._textBoxWindowName = new System.Windows.Forms.TextBox();
            this._labelWidth = new System.Windows.Forms.Label();
            this._textBoxWidth = new System.Windows.Forms.TextBox();
            this._textBoxHeight = new System.Windows.Forms.TextBox();
            this._labelHeight = new System.Windows.Forms.Label();
            this._buttonApply = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this._labelUnits1 = new System.Windows.Forms.Label();
            this._labelUnits2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._checkBoxUseDefaultName = new System.Windows.Forms.CheckBox();
            this._buttonOK = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _labelWindowName
            // 
            this._labelWindowName.AutoSize = true;
            this._labelWindowName.Location = new System.Drawing.Point(13, 13);
            this._labelWindowName.Name = "_labelWindowName";
            this._labelWindowName.Size = new System.Drawing.Size(35, 13);
            this._labelWindowName.TabIndex = 0;
            this._labelWindowName.Text = "&Name";
            // 
            // _textBoxWindowName
            // 
            this._textBoxWindowName.Location = new System.Drawing.Point(16, 30);
            this._textBoxWindowName.Name = "_textBoxWindowName";
            this._textBoxWindowName.Size = new System.Drawing.Size(289, 20);
            this._textBoxWindowName.TabIndex = 1;
            // 
            // _labelWidth
            // 
            this._labelWidth.AutoSize = true;
            this._labelWidth.Location = new System.Drawing.Point(9, 11);
            this._labelWidth.Name = "_labelWidth";
            this._labelWidth.Size = new System.Drawing.Size(35, 13);
            this._labelWidth.TabIndex = 2;
            this._labelWidth.Text = "&Width";
            // 
            // _textBoxWidth
            // 
            this._textBoxWidth.Location = new System.Drawing.Point(12, 32);
            this._textBoxWidth.Name = "_textBoxWidth";
            this._textBoxWidth.Size = new System.Drawing.Size(85, 20);
            this._textBoxWidth.TabIndex = 3;
            // 
            // _textBoxHeight
            // 
            this._textBoxHeight.Location = new System.Drawing.Point(153, 32);
            this._textBoxHeight.Name = "_textBoxHeight";
            this._textBoxHeight.Size = new System.Drawing.Size(85, 20);
            this._textBoxHeight.TabIndex = 4;
            this._textBoxHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _labelHeight
            // 
            this._labelHeight.AutoSize = true;
            this._labelHeight.Location = new System.Drawing.Point(150, 11);
            this._labelHeight.Name = "_labelHeight";
            this._labelHeight.Size = new System.Drawing.Size(38, 13);
            this._labelHeight.TabIndex = 5;
            this._labelHeight.Text = "&Height";
            // 
            // _buttonApply
            // 
            this._buttonApply.Location = new System.Drawing.Point(149, 164);
            this._buttonApply.Name = "_buttonApply";
            this._buttonApply.Size = new System.Drawing.Size(75, 23);
            this._buttonApply.TabIndex = 8;
            this._buttonApply.Text = "&Apply";
            this._buttonApply.UseVisualStyleBackColor = true;
            this._buttonApply.Click += new System.EventHandler(this._buttonApply_Click);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancel.Location = new System.Drawing.Point(230, 164);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(75, 23);
            this._buttonCancel.TabIndex = 9;
            this._buttonCancel.Text = "&Close";
            this._buttonCancel.UseVisualStyleBackColor = true;
            // 
            // _labelUnits1
            // 
            this._labelUnits1.AutoSize = true;
            this._labelUnits1.Location = new System.Drawing.Point(103, 35);
            this._labelUnits1.Name = "_labelUnits1";
            this._labelUnits1.Size = new System.Drawing.Size(21, 13);
            this._labelUnits1.TabIndex = 9;
            this._labelUnits1.Text = "cm";
            // 
            // _labelUnits2
            // 
            this._labelUnits2.AutoSize = true;
            this._labelUnits2.Location = new System.Drawing.Point(244, 35);
            this._labelUnits2.Name = "_labelUnits2";
            this._labelUnits2.Size = new System.Drawing.Size(21, 13);
            this._labelUnits2.TabIndex = 10;
            this._labelUnits2.Text = "cm";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this._labelHeight);
            this.panel1.Controls.Add(this._labelWidth);
            this.panel1.Controls.Add(this._labelUnits2);
            this.panel1.Controls.Add(this._textBoxWidth);
            this.panel1.Controls.Add(this._labelUnits1);
            this.panel1.Controls.Add(this._textBoxHeight);
            this.panel1.Location = new System.Drawing.Point(16, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(289, 69);
            this.panel1.TabIndex = 12;
            // 
            // _checkBoxUseDefaultName
            // 
            this._checkBoxUseDefaultName.AutoSize = true;
            this._checkBoxUseDefaultName.Location = new System.Drawing.Point(16, 57);
            this._checkBoxUseDefaultName.Name = "_checkBoxUseDefaultName";
            this._checkBoxUseDefaultName.Size = new System.Drawing.Size(153, 17);
            this._checkBoxUseDefaultName.TabIndex = 13;
            this._checkBoxUseDefaultName.Text = "Use Default Window name";
            this._checkBoxUseDefaultName.UseVisualStyleBackColor = true;
            this._checkBoxUseDefaultName.CheckedChanged += new System.EventHandler(this._checkBoxUseDefaultName_CheckedChanged);
            // 
            // _buttonOK
            // 
            this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._buttonOK.Location = new System.Drawing.Point(68, 164);
            this._buttonOK.Name = "_buttonOK";
            this._buttonOK.Size = new System.Drawing.Size(75, 23);
            this._buttonOK.TabIndex = 7;
            this._buttonOK.Text = "&OK";
            this._buttonOK.UseVisualStyleBackColor = true;
            this._buttonOK.Click += new System.EventHandler(this._buttonOK_Click);
            // 
            // ModifyWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._buttonCancel;
            this.ClientSize = new System.Drawing.Size(317, 199);
            this.Controls.Add(this._buttonOK);
            this.Controls.Add(this._checkBoxUseDefaultName);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._buttonCancel);
            this.Controls.Add(this._buttonApply);
            this.Controls.Add(this._textBoxWindowName);
            this.Controls.Add(this._labelWindowName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ModifyWindowForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Modify Window";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _labelWindowName;
        private System.Windows.Forms.TextBox _textBoxWindowName;
        private System.Windows.Forms.Label _labelWidth;
        private System.Windows.Forms.TextBox _textBoxWidth;
        private System.Windows.Forms.TextBox _textBoxHeight;
        private System.Windows.Forms.Label _labelHeight;
        private System.Windows.Forms.Button _buttonApply;
        private System.Windows.Forms.Button _buttonCancel;
        private System.Windows.Forms.Label _labelUnits1;
        private System.Windows.Forms.Label _labelUnits2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox _checkBoxUseDefaultName;
        private System.Windows.Forms.Button _buttonOK;
    }
}