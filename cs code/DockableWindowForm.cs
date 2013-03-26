using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;

using MapInfo.MiPro.Interop;


namespace WindowHelper
{
    public partial class DockableWindowForm : UserControl
    {
        private Win32Window _window;
        private System.IntPtr _originalParent;

        // file with path for docking persistence
        private string _sDockInfoXMLFile = "";

        private static MapInfoApplication _miApp = null;
        private static DockWindow _dockWindow = null;
        private DockWindowState _dockWindowState = null;
        //To prevent overwriting of xml storing the docking state Info, by different 
        //running instance of MapInfo Professional Mutexes will be employed
        private Mutex _controllerMutex = null;

        //----------------------------------------------------------------------------------------
        #region Constructors

        public DockableWindowForm(Win32Window mapInfoWindow)
        {
            InitializeComponent();
            ConfigureDockableWindow(mapInfoWindow, "", "");

            //_window = mapInfoWindow;

            //_miApp = InteropServices.MapInfoApplication;

            //// Register the window with the docking system
            ////_dockWindow = _miApp.RegisterDockWindow(_window.Handle);
            //_dockWindow = _miApp.RegisterDockWindow(this.Handle);

            //_dockWindow.Title = _window.Title;
            //DockableWindowForm_Load();

            //SetDockPosition();
        }

        public DockableWindowForm(Win32Window mapInfoWindow, string name)
        {
            InitializeComponent();
            ConfigureDockableWindow(mapInfoWindow, name, "");

            //_window = mapInfoWindow;

            //if (name != "")
            //{
            //    _sDockInfoXMLFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            //                        + "\\MapInfo\\MapInfo\\" + name + ".xml";

            //    _controllerMutex = new Mutex(false, name);
            //}

            //_miApp = InteropServices.MapInfoApplication;
            //// Register the window with the docking system
            ////_dockWindow = _miApp.RegisterDockWindow(_window.Handle);
            //_dockWindow = _miApp.RegisterDockWindow(this.Handle);

            //_dockWindow.Title = _window.Title;
            //DockableWindowForm_Load();

            //SetDockPosition();
        }

        public DockableWindowForm(Win32Window mapInfoWindow, string name, string windowTitle)
        {
            InitializeComponent();
            ConfigureDockableWindow(mapInfoWindow, name, windowTitle);

            //_window = mapInfoWindow;

            //if (name != "")
            //{
            //    _sDockInfoXMLFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            //                        + "\\MapInfo\\MapInfo\\" + name + ".xml";
            //    _controllerMutex = new Mutex(false, name);
            //}

            //_miApp = InteropServices.MapInfoApplication;
            //// Register the window with the docking system
            ////_dockWindow = _miApp.RegisterDockWindow(_window.Handle);
            //_dockWindow = _miApp.RegisterDockWindow(this.Handle);

            //_dockWindow.Title = windowTitle;
            //DockableWindowForm_Load();

            //SetDockPosition();
        }

        private void ConfigureDockableWindow(Win32Window mapInfoWindow, string name, string windowTitle)
        {
            _window = mapInfoWindow;

            if (name != "")
            {
                _sDockInfoXMLFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                                    + "\\MapInfo\\MapInfo\\" + name + ".xml";
                _controllerMutex = new Mutex(false, name);
            }

            _miApp = InteropServices.MapInfoApplication;
            // Register the window with the docking system
            _dockWindow = _miApp.RegisterDockWindow(this.Handle);

            //Setting the title of the dockable window
            if (windowTitle == "" || windowTitle == null)
                _dockWindow.Title = _window.Title;
            //_dockWindow.Title = _window.Name;
            else
                _dockWindow.Title = windowTitle;

            DockableWindowForm_Load();

            SetDockPosition();
        }
        #endregion

        public string Title
        {
            get { return _dockWindow.Title; }
            set { _dockWindow.Title = value; }
        }

        void DockableWindowForm_Load()
        {
            _window.Open();
            //Set the parent of MapBasic window to a picture box on the form.
            _originalParent = _window.Parent;
            _window.Parent = _pictureBoxWindow.Handle;
            _window.RemoveCaptionBar();
            _window.RemoveBorder();
            //_window.Maximize();

            //Add one to the width to update the control.
            Width += 1;
            _window.Update();

        }

        /// <summary>
        /// Set the dialog position and docking state 
        /// </summary>
        public void SetDockPosition()
        {
            SetDockWindowPositionFromFile();
        }

        public void Activate()
        {
            _dockWindow.Activate();
        }

        public void Close()
        {
            //_window.Parent = _originalParent;
            _window.AddCaptionBar();
            _window.AddBorder();

            //Write out the XML file that stores the Named Views info
            DockWindowClose();

            _window.Close();

            if (_miApp != null)
            {
                _miApp.UnregisterDockWindow(_dockWindow);
                // Clean up the MapInfoApplication object
                _miApp.Dispose();
                _miApp = null;
            }

            this.Dispose();
        }

        void DockableWindowForm_Closed(object sender, FormClosedEventArgs e)
        {
            //MessageBox.Show("DockableWindowForm_Closed");
            Close();
        }

        private void DockableWindow_VisibleChanged(object sender, EventArgs e)
        {
            //Close();
        }

        //public void EnableDockChangedEvent()
        //{
        //    Resize += new EventHandler(MapBasicWindowForm_Resize);
        //    DockChanged += new EventHandler(MapBasicWindowForm_DockChanged);
 
        //}

        private void DockableWindowForm_Resize(object sender, EventArgs e)
        {
            if (_window != null)
            {
                _window.UpdateSize(_panelWindow.Width, _panelWindow.Height);
            }
        }

        //public void ReleaseWindow()
        //{
        //    MessageBox.Show("_window.AddCaptionBar()");
        //    _window.AddCaptionBar();
        //    MessageBox.Show("_window.AddBorder()");
        //    _window.AddBorder();
        //    //_mapInfoWindow.Restore();
        //    MessageBox.Show("_window.Close()");
        //    _window.Close();
        //    MessageBox.Show("_window.Open()");
        //    _window.Open();
        //}

        private void DockableWindowForm_DockChanged(object sender, EventArgs e)
        {
            DockWindowChange();
        }

        private void DockableWindowForm_LocationChanged(object sender, EventArgs e)
        {
            DockWindowChange();
        }

        ///****************************************************************************
        ///Lets you dock the form using the Docking system within MapInfo Professional
        ///****************************************************************************
        // strings for dock persistence xml file
        //-----------------------------------------------------------------------------
        #region Docking Window Constants
        private const string STR_DOCK_WINDOW_STATE = "DockWindowState";
        private const string STR_TYPE = "Type";
        private const string STR_FLOATING = "Floating";
        private const string STR_FLOAT_STATE_RECT = "FloatStateRect";
        private const string STR_RIGHT = "Right";
        private const string STR_BOTTOM = "Bottom";
        private const string STR_DOCKED = "Docked";
        private const string STR_DOCK_STATE = "DockState";
        private const string STR_POSITION = "Position";
        private const string STR_CX = "CX";
        private const string STR_CY = "CY";
        private const string STR_PINNED = "Pinned";
        private const string STR_LEFT = "Left";
        private const string STR_TOP = "Top";
        private const string STR_APP_DOCK_INFO_NODE = "NodesDockPosInfo";
        #endregion
        
        //-----------------------------------------------------------------------------
        #region Docking of Window within MapInfo Pro
        /// <summary>
        /// Close the dockable usercontrol(Named view Dialog)
        /// </summary>
        public void DockWindowClose()
        {
            //store the docking related info in xml file
            WriteDockWindowStateToFile();

            _dockWindow.Close();
        }

        public void DockWindowChange()
        {
            //store the docking related info in xml file
            //WriteDockWindowStateToFile();
        }

        /// <summary>
        /// Structure for storing left, right, top, bottom information of floating dialog
        /// </summary>
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }

        public struct FloatState
        {
            public RECT Rect;
        }

        /// <summary>
        /// Structure for storing docking information
        /// </summary>
        public struct DockState
        {
            public DockPosition Position;
            public int CX;
            public int CY;
            public bool Pinned;
        }

        /// <summary>
        /// Class for docking state related information of dockable window
        /// </summary>
        public class DockWindowState
        {
            public bool Floating;
            public bool Visible;
            public FloatState FloatState;
            public DockState DockState;
        }

        /// <summary>
        /// get the open/close status of dockable user control(WindowHelper dialog)
        /// </summary>
        public bool GetDockWindowClosed()
        {
            return _dockWindow.Closed;
        }

        /// <summary>
        /// Method for getting the Docking state related information 
        /// </summary>
        private DockWindowState getDockWindowState()
        {
            if (_dockWindowState == null)
            {
               _dockWindowState = new DockWindowState();
            }

                // store float state
                _dockWindowState.Floating = _dockWindow.Floating;
                // store float size/location
                _dockWindow.FloatSize(
                    ref _dockWindowState.FloatState.Rect.Left,
                    ref _dockWindowState.FloatState.Rect.Top,
                    ref _dockWindowState.FloatState.Rect.Right,
                    ref _dockWindowState.FloatState.Rect.Bottom);
                // store dock position
                _dockWindowState.DockState.Position = _dockWindow.DockPosition;
                // store docked pinned state
                _dockWindowState.DockState.Pinned = _dockWindow.Pinned;
                // store docked size
                _dockWindowState.DockState.CX = _dockWindow.DockSizeCX;
                _dockWindowState.DockState.CY = _dockWindow.DockSizeCY;

            return _dockWindowState;
        }

        /// <summary>
        /// Method for Setting the Docking state from xml file containg
        /// docking persistance information
        /// </summary>
        public void SetDockWindowPositionFromFile()
        {
            LoadDockWindowStateFromFile();
            ApplyDockWindowState();
        }

        /// <summary>
        /// Method for setting some resaonable position/ docked state
        /// </summary>
        private void SetDefaultDockingInfo()
        {
            if (_dockWindowState == null)
            {
                _dockWindowState = new DockWindowState();
            }
            _dockWindowState.Floating = true;   //false
            _dockWindowState.DockState.Position = DockPosition.PositionLeft;
            _dockWindowState.DockState.CX = 300;
            _dockWindowState.DockState.CY = 225;
            _dockWindowState.DockState.Pinned = false;

            _dockWindowState.FloatState.Rect.Top = 200;
            _dockWindowState.FloatState.Rect.Bottom = 400;
            _dockWindowState.FloatState.Rect.Left = 200;
            _dockWindowState.FloatState.Rect.Right = 400;

            _dockWindowState.Visible = true;
        }

        /// <summary>
        /// Method for setting the docking and position of dockable window
        /// </summary>
        private void ApplyDockWindowState()
        {
            if (_dockWindowState == null)
            {
                // This can be a case in which the persistence xml file is deleted/ xml
                // corruption etc. Even if we docking state info does not exist, we will
                // still like the dialog to open with some resaonable position/ docked state
                _dockWindowState = new DockWindowState();
                SetDefaultDockingInfo();
            }
            if (_dockWindowState.Floating)
            {
                _dockWindow.Float(
                    _dockWindowState.FloatState.Rect.Left,
                    _dockWindowState.FloatState.Rect.Top,
                    _dockWindowState.FloatState.Rect.Right,
                    _dockWindowState.FloatState.Rect.Bottom);
            }
            else
            {
                _dockWindow.Dock(
                    _dockWindowState.DockState.Position,
                    _dockWindowState.DockState.CX,
                    _dockWindowState.DockState.CY);
                if (_dockWindowState.DockState.Pinned)
                {
                    _dockWindow.Pin();
                }
            }
        }

        /// <summary>
        /// Load the dock window state from the xml file
        /// It uses mutexes to synchronize the threads accessing
        /// the xml file
        /// </summary>
        private void LoadDockWindowStateFromFile()
        {
            //If the name of the file is not set, loading the settings is not possible
            if (_sDockInfoXMLFile == "")
                return;

            string sErr = string.Empty;

            // Wait until safe to read from file
            _controllerMutex.WaitOne();

            // Try to read the xml file
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                // Load the xml file
                xmlDoc.Load(_sDockInfoXMLFile);

                //Get the docking related information 
                _dockWindowState = new DockWindowState();

                //All Docking related information is in "DockWindowState" node and its child nodes
                XmlNodeList xmlNodeList = xmlDoc.GetElementsByTagName(STR_DOCK_WINDOW_STATE);
                XmlNode dockWinStateNode = xmlNodeList[0];
                if (dockWinStateNode == null || dockWinStateNode.Attributes[STR_TYPE] == null)
                    throw new XmlException(Properties.Resources.ERR_INVALID_XML);
                if (dockWinStateNode.Attributes[STR_TYPE].Value.CompareTo(STR_FLOATING) == 0)
                {
                    // The dialog was floating the last time the application was closed
                    _dockWindowState.Floating = true;
                    XmlNode FloatStateRectNode = dockWinStateNode.ChildNodes[0];
                    if (FloatStateRectNode == null)
                        throw new XmlException(Properties.Resources.ERR_INVALID_XML);
                    _dockWindowState.FloatState.Rect.Top = Convert.ToInt32(FloatStateRectNode.Attributes[STR_TOP].Value);
                    _dockWindowState.FloatState.Rect.Left = Convert.ToInt32(FloatStateRectNode.Attributes[STR_LEFT].Value);
                    _dockWindowState.FloatState.Rect.Right = Convert.ToInt32(FloatStateRectNode.Attributes[STR_RIGHT].Value);
                    _dockWindowState.FloatState.Rect.Bottom = Convert.ToInt32(FloatStateRectNode.Attributes[STR_BOTTOM].Value);
                    _dockWindowState.DockState.Position = DockPosition.PositionFloat;
                }
                else if (dockWinStateNode.Attributes[STR_TYPE].Value.CompareTo(STR_DOCKED) == 0)
                {
                    // The dialog was docked the last time the application was closed
                    _dockWindowState.Floating = false;
                    XmlNode dockStateNode = dockWinStateNode.ChildNodes[0];
                    if (dockStateNode == null)
                        throw new XmlException(Properties.Resources.ERR_INVALID_XML);
                    _dockWindowState.DockState.Position = (DockPosition)Enum.Parse(typeof(DockPosition), dockStateNode.Attributes["Position"].Value);
                    _dockWindowState.DockState.Pinned = Convert.ToBoolean(dockStateNode.Attributes[STR_PINNED].Value);
                    _dockWindowState.DockState.CX = Convert.ToInt32(dockStateNode.Attributes[STR_CX].Value);
                    _dockWindowState.DockState.CY = Convert.ToInt32(dockStateNode.Attributes[STR_CY].Value);

                    _dockWindowState.FloatState.Rect.Top = Convert.ToInt32(dockStateNode.Attributes[STR_TOP].Value);
                    _dockWindowState.FloatState.Rect.Left = Convert.ToInt32(dockStateNode.Attributes[STR_LEFT].Value);
                    _dockWindowState.FloatState.Rect.Right = Convert.ToInt32(dockStateNode.Attributes[STR_RIGHT].Value);
                    _dockWindowState.FloatState.Rect.Bottom = Convert.ToInt32(dockStateNode.Attributes[STR_BOTTOM].Value);
                }
            }
            catch (System.Xml.XPath.XPathException ex)
            {
                sErr = ex.Message;
            }
            catch (XmlException ex)
            {
                sErr = ex.Message;
            }
            catch (ArgumentException ex)
            {
                sErr = ex.Message;
            }
            catch (FileNotFoundException ex)
            {
                sErr = ex.Message;
                sErr = string.Empty;
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }
            finally
            {
                if (sErr != string.Empty)
                {
                    MessageBox.Show(sErr);
                }
                _controllerMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Write the dock window state to file
        /// It uses mutexes to synchronize the threads accessing
        /// the xml file
        /// </summary>
        private void WriteDockWindowStateToFile()
        {
            //If the name of the file is not set, storing the settings is not possible
            if (_sDockInfoXMLFile == "")
                return;

            string sErr = string.Empty;

            //wait until safe to read from file
            _controllerMutex.WaitOne();
            try
            {
                XmlTextWriter xw = new XmlTextWriter(_sDockInfoXMLFile, System.Text.Encoding.Unicode);

                // Use indenting for readability.
                xw.Formatting = Formatting.Indented;

                // write the XML declaration
                xw.WriteStartDocument();

                // write the Root node 
                xw.WriteStartElement(STR_APP_DOCK_INFO_NODE);
                getDockWindowState();

                //Write the docking related information
                if (_dockWindowState.Floating)
                {
                    //DockWindowState node contains docking and position related info
                    xw.WriteStartElement(STR_DOCK_WINDOW_STATE);
                    xw.WriteAttributeString(STR_TYPE, STR_FLOATING);
                    xw.WriteStartElement(STR_FLOAT_STATE_RECT);
                    //start the FloatStateRect node
                    xw.WriteAttributeString(STR_TOP, Convert.ToString(_dockWindowState.FloatState.Rect.Top));
                    xw.WriteAttributeString(STR_LEFT, Convert.ToString(_dockWindowState.FloatState.Rect.Left));
                    xw.WriteAttributeString(STR_RIGHT, Convert.ToString(_dockWindowState.FloatState.Rect.Right));
                    xw.WriteAttributeString(STR_BOTTOM, Convert.ToString(_dockWindowState.FloatState.Rect.Bottom));
                    // end the FloatStateRect node 
                    xw.WriteEndElement();
                    // end the DockWindowState node 
                    xw.WriteEndElement();
                }
                else
                {
                    //DockWindowState node contains docking and position related info
                    xw.WriteStartElement(STR_DOCK_WINDOW_STATE);
                    xw.WriteAttributeString(STR_TYPE, STR_DOCKED);
                    //start the DockState node
                    xw.WriteStartElement(STR_DOCK_STATE);
                    xw.WriteAttributeString(STR_POSITION, Convert.ToString(_dockWindowState.DockState.Position));
                    xw.WriteAttributeString(STR_CX, Convert.ToString(_dockWindowState.DockState.CX));
                    xw.WriteAttributeString(STR_CY, Convert.ToString(_dockWindowState.DockState.CY));
                    xw.WriteAttributeString(STR_PINNED, Convert.ToString(_dockWindowState.DockState.Pinned));

                    xw.WriteAttributeString(STR_TOP, Convert.ToString(_dockWindowState.FloatState.Rect.Top));
                    xw.WriteAttributeString(STR_LEFT, Convert.ToString(_dockWindowState.FloatState.Rect.Left));
                    xw.WriteAttributeString(STR_RIGHT, Convert.ToString(_dockWindowState.FloatState.Rect.Right));
                    xw.WriteAttributeString(STR_BOTTOM, Convert.ToString(_dockWindowState.FloatState.Rect.Bottom));

                    // end the DockState node
                    xw.WriteEndElement();
                    // end the DockWindowState node
                    xw.WriteEndElement();
                }

                // end the root element (represent the tool)
                xw.WriteEndElement();

                // finish the write operation
                xw.Flush(); xw.Close();

            }
            catch (DirectoryNotFoundException ex)
            {
                sErr = ex.Message;
            }
            catch (IOException ex)
            {
                sErr = ex.Message;
            }
            catch (UnauthorizedAccessException ex)
            {
                sErr = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                sErr = ex.Message;
            }
            catch (ArgumentException ex)
            {
                sErr = ex.Message;
            }

            if (sErr != string.Empty)
                MessageBox.Show(sErr);

            //release the mutex
            _controllerMutex.ReleaseMutex();
        }
        #endregion 
    }
}
