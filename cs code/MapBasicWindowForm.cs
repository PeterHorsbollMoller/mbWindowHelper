using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;


namespace WindowHelper
{
    public partial class MapBasicWindowForm : UserControl
    {
        private MapBasicWindowController _controller;
        private Win32Window _mapBasicWindow;

        public MapBasicWindowForm()
        {
            InitializeComponent();
            //Resize += new EventHandler(MapBasicWindowForm_Resize);
            //MapBasicWindowForm_Load();
        }

        public MapBasicWindowForm(Win32Window mapbasicWindow)
        {
            InitializeComponent();
            _mapBasicWindow = mapbasicWindow;
            //Resize += new EventHandler(MapBasicWindowForm_Resize);
            //MapBasicWindowForm_Load();
        }

        public MapBasicWindowForm(MapBasicWindowController controller, Win32Window mapbasicWindow)
		{
            InitializeComponent();
            _controller = controller;
            _mapBasicWindow = mapbasicWindow;
            //Resize += new EventHandler(MapBasicWindowForm_Resize);
            //MapBasicWindowForm_Load();
        }

        /// <summary>
        /// Set the dialog position and docking state 
        /// </summary>
        public void SetDockPosition()
        {
            _controller.SetDockWindowPositionFromFile();
        }

        public void CloseDockWindow()
        {
            //Write out the XML file that stores the Named Views info
            _controller.DockWindowClose();
            _mapBasicWindow.Close();
        }

        void MapBasicWindowForm_Closed(object sender, FormClosedEventArgs e)
        {
            _mapBasicWindow.Close();
        }

        void MapBasicWindowForm_Load(object sender, EventArgs e)
        //void MapBasicWindowForm_Load()
        {
            //Set the parent of Mpabsic window to a picture box on the form.
            _mapBasicWindow.Parent = this._pictureBoxWindow.Handle;
            _mapBasicWindow.RemoveCaptionBar();
            _mapBasicWindow.RemoveBorder();
            _mapBasicWindow.Maximize();

            //Add one to the width to update the control.
            Width += 1;

            //InteropHelper.Do("Alter Button ID 2005 Disable");
        }

        //public void EnableDockChangedEvent()
        //{
        //    Resize += new EventHandler(MapBasicWindowForm_Resize);
        //    DockChanged += new EventHandler(MapBasicWindowForm_DockChanged);
 
        //}

        private void MapBasicWindowForm_Resize(object sender, EventArgs e)
        {
            if (_mapBasicWindow != null)
            {
                _mapBasicWindow.UpdateSize(_pictureBoxWindow.Width, _pictureBoxWindow.Height);
            }
        }

        public enum WindowStyles : uint
        {
            WS_POPUP = 0x80000000
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;

                // As a top-level window it has no parent
                //createParams.Parent = IntPtr.Zero;

                // Appear as a pop-up window
                int style = createParams.Style;
                createParams.Style = style + unchecked((int)WindowStyles.WS_POPUP);

                return createParams;
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetFocus(IntPtr hWnd);

        private void MapBasicWindowForm_DockChanged(object sender, EventArgs e)
        {
            _controller.DockWindowChange();
        }

        private void MapBasicWindowForm_LocationChanged(object sender, EventArgs e)
        {
            _controller.DockWindowChange();
        }
    }
}
