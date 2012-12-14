using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowHelper
{
    public partial class ModifyWindowForm : Form
    {
        private Windows.Window _window;
        //private string _windowTitle = "";
        //private double _windowWidth = 0.0;
        //private double _windowHeight = 0.0;

        public ModifyWindowForm()
        {
            InitializeComponent();

            this._buttonApply.Text = Controller.GetResItemStr("STR_APPLY");
            this._buttonCancel.Text = Controller.GetResItemStr("STR_CLOSE");
            this._buttonOK.Text = Controller.GetResItemStr("STR_OK");

            this._labelHeight.Text = Controller.GetResItemStr("STR_HEIGHT");
            this._labelWidth.Text = Controller.GetResItemStr("STR_WIDTH");
            this._labelWindowName.Text = Controller.GetResItemStr("STR_WINDOW_NAME");

            this.Text = Controller.GetResItemStr("DLG_MODIFY_WINDOW");

            this._checkBoxUseDefaultName.Text = Controller.GetResItemStr("STR_USE_DEFAULT_WINDOW_NAME");
        }

        public Windows.Window Window
        {
            set 
            { 
                _window = value;

                _textBoxWindowName.Text = _window.Title;
                _textBoxWidth.Text = _window.Width.ToString();
                _textBoxHeight.Text = _window.Height.ToString();

                _labelUnits1.Text = InteropHelper.GetSessionPaperUnit();
                _labelUnits2.Text = _labelUnits1.Text;
            }
            get { return _window; }
        }

        private void _ApplyToWindow()
        {
            if (_checkBoxUseDefaultName.Checked == true)
            {
                _window.ResetName();
                _textBoxWindowName.Text = _window.Title;
            }
            else
            {
                if (_textBoxWindowName.Text != _window.Title)
                    _window.Title = _textBoxWindowName.Text;
            }

            if (_textBoxWidth.Text != _window.Width.ToString())
                _window.Width = double.Parse(_textBoxWidth.Text);//, System.Globalization.CultureInfo.CurrentCulture);

            if (_textBoxHeight.Text != _window.Height.ToString())
                _window.Height = double.Parse(_textBoxHeight.Text);//, System.Globalization.CultureInfo.CurrentCulture);
 
        }

        private void _buttonApply_Click(object sender, EventArgs e)
        {
            _ApplyToWindow();
        }

        private void _checkBoxUseDefaultName_CheckedChanged(object sender, EventArgs e)
        {
            if (_checkBoxUseDefaultName.Checked == true)
                _textBoxWindowName.Enabled = false;
            else
                _textBoxWindowName.Enabled = true;

        }

        private void _buttonOK_Click(object sender, EventArgs e)
        {
            _ApplyToWindow();
        }
    }
}
