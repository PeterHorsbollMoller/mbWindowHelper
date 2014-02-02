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
using MapInfo.MiPro.Interop;


namespace WindowHelper
{
    public partial class WindowHelperForm : UserControl
    {
        //*****************************************************************************    
        #region private variables
        // file with path for docking persistence
        private string _sDockInfoXMLFile = "";

        private static MapInfoApplication _miApp = null;
        private static DockWindow _dockWindow = null;
        private DockWindowState _dockWindowState = null;
        //To prevent overwriting of xml storing the docking state Info, by different 
        //running instance of MapInfo Professional Mutexes will be employed
        private Mutex _controllerMutex = null;

        private Windows.Window _activeWindow = null;

        private Windows.WindowList _windowsList = null;
        private Windows.WindowList _mapperWindows = null;
        private Windows.WindowList _browserWindows = null;
        private Windows.WindowList _layoutWindows = null;
        private Windows.WindowList _legendWindows = null;

        private Windows.ClosedWindowList _closedWindowsList = null;
        private Windows.ClosedWindowList _closedMapperWindows = null;
        private Windows.ClosedWindowList _closedBrowserWindows = null;
        private Windows.ClosedWindowList _closedLayoutWindows = null;
        private Windows.ClosedWindowList _closedLegendWindows = null;

        private Windows.WindowList _specialWindows = null;

        private bool _autolockWindowEnabled = false;

        private Windows.MapWindowExtentsList _mapWindowExtents = new Windows.MapWindowExtentsList();

        #endregion

        //*****************************************************************************    
        #region Constructors
        public WindowHelperForm(string title)
        {
            InitializeComponent();

            if (title != "")
            {
                _sDockInfoXMLFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                                    + "\\MapInfo\\MapInfo\\" + title + ".xml";
                _controllerMutex = new Mutex(false, title);
            }

            _miApp = InteropServices.MapInfoApplication;
            //_miApp.Do("Set Paper Units \"cm\"");
            // Register the window with the docking system
            _dockWindow = _miApp.RegisterDockWindow(this.Handle);
            SetDockPosition();

            _dockWindow.Title = title;

            _tabPageCurrentWindows.Text = Controller.GetResItemStr("TAB_CURRENT");
            _tabPageClosedWindows.Text = Controller.GetResItemStr("TAB_CLOSED");
            _tabPageSpecialWindows.Text = Controller.GetResItemStr("TAB_SPECIAL");

            AddTooltips();
        }
        #endregion

        //*****************************************************************************    
        #region Methods
        public void Close()
        {
            //MessageBox.Show("Now we would close the docked normal windows!");
            if (_windowsList != null)
            {
                foreach (Windows.Window window in _windowsList)
                {
                    if (window.IsMadeDockable() == true)
                        window.Close(true);
                }
            }

            //MessageBox.Show("Now we would close the WindowHelper docked window!");
            DockWindowClose();

            if (_miApp != null)
            {
                _miApp.UnregisterDockWindow(_dockWindow);
                // Clean up the MapInfoApplication object
                _miApp.Dispose();
                _miApp = null;
            }
        }

        private void AddTooltips()
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Buttons
            toolTip1.SetToolTip(_checkBoxAutolockWindows, Controller.GetResItemStr("MNU_HLP_AUTOLOCK_ALL_WINDOWS"));
            toolTip1.SetToolTip(_buttonCloneWindow, Controller.GetResItemStr("MNU_HLP_CLONE_WINDOW"));
            toolTip1.SetToolTip(_buttonCloseWindow, Controller.GetResItemStr("MNU_HLP_CLOSE_WINDOW"));
            toolTip1.SetToolTip(_buttonCurrentMakeDockable, Controller.GetResItemStr("MNU_HLP_DOCK_WINDOW"));
            toolTip1.SetToolTip(_buttonMakeDockable, Controller.GetResItemStr("MNU_HLP_DOCK_WINDOW"));
            toolTip1.SetToolTip(_buttonRestoreWindow, Controller.GetResItemStr("MNU_HLP_RESTORE_WINDOW"));
            toolTip1.SetToolTip(_buttonUnlockWindow, Controller.GetResItemStr("MNU_HLP_UNLOCK_WINDOW"));
            toolTip1.SetToolTip(_buttonLockWindow, Controller.GetResItemStr("MNU_HLP_LOCK_WINDOW"));
            toolTip1.SetToolTip(_buttonCurrentOptions, Controller.GetResItemStr("MNU_HLP_MOFIFY_WINDOW"));

            toolTip1.SetToolTip(_buttonZoomToFirst, Controller.GetResItemStr("MNU_HLP_ZOOM_TO_FIRST"));
            toolTip1.SetToolTip(_buttonZoomToLast, Controller.GetResItemStr("MNU_HLP_ZOOM_TO_LAST"));
            toolTip1.SetToolTip(_buttonZoomToPrevious, Controller.GetResItemStr("MNU_HLP_ZOOM_TO_PREVIOUS"));
            toolTip1.SetToolTip(_buttonZoomToNext, Controller.GetResItemStr("MNU_HLP_ZOOM_TO_NEXT"));
        }

        //-----------------------------------------------------------------
        private void UpdateWindowsButtons(TreeNode node)
        {
            //TreeNode node = null;
            //node = _treeViewWindows.SelectedNode;
            if (node == null)
            {
                //No node currently selected
                _buttonCloneWindow.Enabled = false;
                _buttonCloseWindow.Enabled = false;
                _buttonLockWindow.Enabled = false;
                _buttonUnlockWindow.Enabled = false;
                _buttonCurrentMakeDockable.Enabled = false;
                _buttonCurrentOptions.Enabled = false;

                _buttonZoomToFirst.Enabled = false;
                _buttonZoomToLast.Enabled = false;
                _buttonZoomToPrevious.Enabled = false;
                _buttonZoomToNext.Enabled = false;
            }
            else
            {
                if (IsNodeAWindow(node))
                {
                    _buttonCloneWindow.Enabled = true;
                    _buttonCloseWindow.Enabled = true;
                    _buttonCurrentOptions.Enabled = true;

                    Windows.Window window = (Windows.Window)node.Tag;
                    if (window.SystemMenuClose == true)
                    {
                        _buttonLockWindow.Enabled = true;
                        _buttonUnlockWindow.Enabled = false;
                    }
                    else
                    {
                        _buttonLockWindow.Enabled = false;
                        _buttonUnlockWindow.Enabled = true;
                    }
                    if (window.IsMadeDockable() == true)
                    {
                        _buttonCurrentMakeDockable.Enabled = false;
                    }
                    else
                    {
                        _buttonCurrentMakeDockable.Enabled = true;
                    }
                    if (window.Type == Windows.Window.WindowType.Mapper)
                    {
                        _buttonZoomToFirst.Enabled = true;
                        _buttonZoomToLast.Enabled = true;
                        _buttonZoomToPrevious.Enabled = true;
                        _buttonZoomToNext.Enabled = true;
                    }
                    else
                    {
                        _buttonZoomToFirst.Enabled = false;
                        _buttonZoomToLast.Enabled = false;
                        _buttonZoomToPrevious.Enabled = false;
                        _buttonZoomToNext.Enabled = false;
                    }
                }
                else
                {
                    _buttonLockWindow.Enabled = false;
                    _buttonUnlockWindow.Enabled = false;
                    _buttonCloneWindow.Enabled = false;
                    _buttonCloseWindow.Enabled = false;
                    _buttonCurrentOptions.Enabled = false;
                    _buttonCurrentMakeDockable.Enabled = false;

                    _buttonZoomToFirst.Enabled = false;
                    _buttonZoomToLast.Enabled = false;
                    _buttonZoomToPrevious.Enabled = false;
                    _buttonZoomToNext.Enabled = false;
                }
            }
        }

        //-----------------------------------------------------------------
        private void UpdateWindowsButtons()
        {
            //AddTooltips();

            if (_activeWindow == null)
            {
                //No node currently selected
                _buttonCloneWindow.Enabled = false;
                _buttonCloseWindow.Enabled = false;
                _buttonLockWindow.Enabled = false;
                _buttonUnlockWindow.Enabled = false;
                _buttonCurrentMakeDockable.Enabled = false;
                _buttonCurrentOptions.Enabled = false;

                _buttonZoomToFirst.Enabled = false;
                _buttonZoomToLast.Enabled = false;
                _buttonZoomToPrevious.Enabled = false;
                _buttonZoomToNext.Enabled = false;

                _labelActiveWindowTitle.Text = "";
                _labelExtentIndex.Text = "";

            }
            else
            {
                    _buttonCloneWindow.Enabled = true;
                    _buttonCloseWindow.Enabled = true;
                    _buttonCurrentOptions.Enabled = true;

                    _labelActiveWindowTitle.Text = String.Format("{0}", _activeWindow.Title);

                    if (_activeWindow.SystemMenuClose == true)
                    {
                        _buttonLockWindow.Enabled = true;
                        _buttonUnlockWindow.Enabled = false;
                    }
                    else
                    {
                        _buttonLockWindow.Enabled = false;
                        _buttonUnlockWindow.Enabled = true;
                    }
                    if (_activeWindow.IsMadeDockable() == true)
                    {
                        _buttonCurrentMakeDockable.Enabled = false;
                    }
                    else
                    {
                        _buttonCurrentMakeDockable.Enabled = true;
                    }
                    if (_activeWindow.Type == Windows.Window.WindowType.Mapper)
                    {
                        int mapWindowIndex = _mapWindowExtents.FindWindow(_activeWindow);
                        if (mapWindowIndex >= 0)
                        {
                            int extentNumber = (_mapWindowExtents[mapWindowIndex].MapExtentList.CurrentIndex + 1);

                            _labelExtentIndex.Text = Controller.GetResItemStr("VIEW_OF_VIEWS"
                                                        , string.Format("{0};{1}", extentNumber, _mapWindowExtents[mapWindowIndex].MapExtentList.NumberOfExtents));

                            if (extentNumber == 1)
                            {
                                _buttonZoomToFirst.Enabled = false;

                                if (_mapWindowExtents[mapWindowIndex].MapExtentList.NumberOfExtents == 1)
                                {
                                    _buttonZoomToLast.Enabled = false;
                                    _buttonZoomToPrevious.Enabled = false;
                                    _buttonZoomToNext.Enabled = false;
                                }
                                else
                                {
                                    _buttonZoomToLast.Enabled = true;
                                    _buttonZoomToNext.Enabled = true;

                                    if (_mapWindowExtents[mapWindowIndex].MapExtentList.AllowWrapAround == true)
                                        _buttonZoomToPrevious.Enabled = true;
                                    else
                                        _buttonZoomToPrevious.Enabled = false;
                                }
                            }
                            else
                            {
                                if (extentNumber == _mapWindowExtents[mapWindowIndex].MapExtentList.NumberOfExtents)
                                {
                                    _buttonZoomToLast.Enabled = false;

                                    _buttonZoomToFirst.Enabled = true;
                                    _buttonZoomToPrevious.Enabled = true;

                                    if (_mapWindowExtents[mapWindowIndex].MapExtentList.AllowWrapAround == true)
                                        _buttonZoomToNext.Enabled = true;
                                    else
                                        _buttonZoomToNext.Enabled = false;
                                }
                                else
                                {
                                    _buttonZoomToFirst.Enabled = true;
                                    _buttonZoomToLast.Enabled = true;
                                    _buttonZoomToPrevious.Enabled = true;
                                    _buttonZoomToNext.Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            _buttonZoomToFirst.Enabled = false;
                            _buttonZoomToLast.Enabled = false;
                            _buttonZoomToPrevious.Enabled = false;
                            _buttonZoomToNext.Enabled = false;
                        }

                    }
                    else
                    {
                        _buttonZoomToFirst.Enabled = false;
                        _buttonZoomToLast.Enabled = false;
                        _buttonZoomToPrevious.Enabled = false;
                        _buttonZoomToNext.Enabled = false;

                        _labelExtentIndex.Text = "";
                    }
            }
        }

        //-----------------------------------------------------------------
        public void ResetActiveWindow()
        {
            if (_activeWindow != null)
                _activeWindow = null;

            UpdateWindowsButtons();
        }

        //-----------------------------------------------------------------
        public void SetActiveWindow(int windowID)
        {
            Windows.Window window = new Windows.Window(windowID);
            _activeWindow = window;

            UpdateWindowsButtons();
        }

        #endregion Methods

        ///****************************************************************************
        ///Lets you dock the form using the Docking system within MapInfo Professional
        ///****************************************************************************
        // strings for dock persistence xml file
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

        //*****************************************************************************    
        #region Docking Handling
        /// <summary>
        /// Set the dialog position and docking state 
        /// </summary>
        public void SetDockPosition()
        {
            SetDockWindowPositionFromFile();
        }

        //[DllImport("user32.dll")]
        //private static extern IntPtr SetFocus(IntPtr hWnd);

        private void WindowHelperForm_DockChanged(object sender, EventArgs e)
        {
            DockWindowChange();
        }

        private void WindowHelperForm_LocationChanged(object sender, EventArgs e)
        {
            DockWindowChange();
        }

        /// <summary>
        /// Close the dockable usercontrol(Named view Dialog)
        /// </summary>
        public void DockWindowClose()
        {
            StoreDockedSpecialWindows();

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
            _dockWindowState.Floating = false;
            _dockWindowState.DockState.Position = DockPosition.PositionRight;
            _dockWindowState.DockState.CX = 300;
            _dockWindowState.DockState.CY = 225;
            _dockWindowState.Visible = true;

            _dockWindowState.DockState.Pinned = false;
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

                    if (_dockWindowState.DockState.CX == 0)
                        _dockWindowState.DockState.CX = (_dockWindowState.FloatState.Rect.Right - _dockWindowState.FloatState.Rect.Left) / 2;
                    if (_dockWindowState.DockState.CX == 0)
                        _dockWindowState.DockState.CX = 200;
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

        //*****************************************************************************    
        #region Windows TreeView

        //-----------------------------------------------------------------
        private int GetWindowIndex(Windows.WindowList windowsList, Windows.Window windowToLocate)
        {
            int index = 0;
            foreach (Windows.Window window in windowsList)
            {
                if (window.ID == windowToLocate.ID)
                {
                    return index;
                }
                index++;
            }

            return -1;
        }

        //-----------------------------------------------------------------
        private bool UpdateInWindowList(Windows.WindowList windowList, Windows.Window window)
        {
            int index = 0;

            index = GetWindowIndex(windowList, window);
            Windows.Window windowExisting = windowList[index];
            if (window.Title != windowExisting.Title)
            {
                windowList[index] = window;
                return true;
            }
            else
                return false;
        }

        //-----------------------------------------------------------------
        private bool UpdateInWindowLists(Windows.Window window)
        {
            bool listUpdated = false;

            if (window.IsDocumentWindow() == true)
            {
                if (UpdateInWindowList(_windowsList, window) == true)
                {
                    listUpdated = true;
                }

                switch (window.Type)
                {
                    case Windows.Window.WindowType.Mapper:
                        if (UpdateInWindowList(_mapperWindows, window) == true)
                        {
                            listUpdated = true;
                        }
                        break;
                    case Windows.Window.WindowType.Browser:
                        if (UpdateInWindowList(_browserWindows, window) == true)
                        {
                            listUpdated = true;
                        }
                        break;
                    case Windows.Window.WindowType.Layout:
                        if (UpdateInWindowList(_layoutWindows, window) == true)
                        {
                            listUpdated = true;
                        }
                        break;
                    case Windows.Window.WindowType.LegendDesigner:
                        if (UpdateInWindowList(_legendWindows, window) == true)
                        {
                            listUpdated = true;
                        }
                        break;
                }
            }
            return listUpdated;
        }

        //-----------------------------------------------------------------
        private void AddToWindowLists(Windows.Window window)
        {
            if (window.IsDocumentWindow() == true)
            {
                _windowsList.Add(window);

                switch (window.Type)
                {
                    case Windows.Window.WindowType.Mapper:
                        _mapperWindows.Add(window);
                        break;
                    case Windows.Window.WindowType.Browser:
                        _browserWindows.Add(window);
                        break;
                    case Windows.Window.WindowType.Layout:
                        _layoutWindows.Add(window);
                        break;
                    case Windows.Window.WindowType.LegendDesigner:
                        _legendWindows.Add(window);
                        break;
                }
            }
        }

        //-----------------------------------------------------------------
        private bool AddToOrUpdateInWindowLists(Windows.Window window)
        {
            bool refreshDialog = false;

            if (_windowsList == null)
                _windowsList = new WindowHelper.Windows.WindowList();
            if (_mapperWindows == null)
                _mapperWindows = new WindowHelper.Windows.WindowList();
            if (_browserWindows == null)
                _browserWindows = new WindowHelper.Windows.WindowList();
            if (_layoutWindows == null)
                _layoutWindows = new WindowHelper.Windows.WindowList();
            if (_legendWindows == null)
                _legendWindows = new WindowHelper.Windows.WindowList();

            //Let's see if this window alread exists in the list of all windows
            int index = GetWindowIndex(_windowsList, window);
            if (index == -1)
            {
                AddToWindowLists(window);
                refreshDialog = true;
            }
            else
            {
                if (UpdateInWindowLists(window) == true)
                    refreshDialog = true;
            }

            return refreshDialog;
        }

        //-----------------------------------------------------------------
        public void AddToOrUpdateInWindowLists(int windowID, string windowName, int windowType)
        {
            Windows.Window window = new Windows.Window(windowID, windowName, windowType);
            if (AddToOrUpdateInWindowLists(window) == true)
                RefreshWindowsList();
        }

        //-----------------------------------------------------------------
        public void AddToOrUpdateInWindowLists(int[] windowIDs, string[] windowNames, int[] windowTypes)
        {
            Windows.Window window = null;
            bool refreshList = false;

            for (int i = 0; i < windowIDs.Length; i++)
            {
                window = new WindowHelper.Windows.Window(windowIDs[i], windowNames[i], windowTypes[i]);
                if (AddToOrUpdateInWindowLists(window) == true)
                    refreshList = true;
            }

            if (refreshList == true)
                RefreshWindowsList();
        }

        //-----------------------------------------------------------------
        public void CreateWindowList(int[] windowIDs, string[] windowNames, int[] windowTypes)
        {
            if (windowIDs.Length == 0)
                return;

            if (_windowsList != null)
            {
                _mapperWindows.Clear();
                _browserWindows.Clear();
                _layoutWindows.Clear();
                _legendWindows.Clear();
                _windowsList.Clear();
            }
            else
            {
                _windowsList = new WindowHelper.Windows.WindowList();
                _mapperWindows = new WindowHelper.Windows.WindowList();
                _browserWindows = new WindowHelper.Windows.WindowList();
                _layoutWindows = new WindowHelper.Windows.WindowList();
                _legendWindows = new WindowHelper.Windows.WindowList();
            }
            Windows.Window window = null;

            for (int i = 0; i < windowIDs.Length; i++)
            {
                window = new WindowHelper.Windows.Window(windowIDs[i], windowNames[i], windowTypes[i]);
                AddToWindowLists(window);
            }
            RefreshWindowsList();
         }

        //-----------------------------------------------------------------
        public void RemoveFromAllWindowsLists(int windowID)
        {
            Windows.Window window = new WindowHelper.Windows.Window(windowID);

            RemoveFromWindowList(_windowsList, window);
 
            switch (window.Type)
            {
                case Windows.Window.WindowType.Mapper:
                    RemoveFromWindowList(_mapperWindows, window);
                    break;
                case Windows.Window.WindowType.Browser:
                    RemoveFromWindowList(_browserWindows, window);
                    break;
                case Windows.Window.WindowType.Layout:
                    RemoveFromWindowList(_layoutWindows, window);
                    break;
                case Windows.Window.WindowType.LegendDesigner:
                    RemoveFromWindowList(_legendWindows, window);
                    break;
            }

            RefreshWindowsList();
        }

        //-----------------------------------------------------------------
        private void RemoveFromWindowList(Windows.WindowList windowsList, Windows.Window windowToRemove)
        {
            int index = 0;
            foreach (Windows.Window window in windowsList)
            {
                if (window.ID == windowToRemove.ID)
                {
                    windowsList.RemoveAt(index);
                    break;
                }
                index++;
            }
        }

        //-----------------------------------------------------------------
        public void RefreshWindowsList()
        {
            TreeNode newNode = null;
            TreeNode subNode = null;
            int nodeNum = 0;
            string nodeName = "";
            int nodeindex = 0;

            //Adding the main nodes to the tree
            if (_treeViewWindows.Nodes.Count == 0)
            {
                for (int i = 0; i <= 4; i++)
                {
                    newNode = new TreeNode();

                    switch (i)
                    {
                        case 0:
                            nodeName = Controller.GetResItemStr("LST_WINDOWS_MAPS");
                            newNode.Tag = _mapperWindows;
                            break;
                        case 1:
                            nodeName = Controller.GetResItemStr("LST_WINDOWS_BROWSERS");
                            newNode.Tag = _browserWindows;
                            break;
                        case 2:
                            nodeName = Controller.GetResItemStr("LST_WINDOWS_LAYOUTS");
                            newNode.Tag = _layoutWindows;
                            break;
                        case 3:
                            nodeName = Controller.GetResItemStr("LST_WINDOWS_LEGENDS");
                            newNode.Tag = _legendWindows;
                            break;
                        case 4:
                            nodeName = Controller.GetResItemStr("LST_WINDOWS_ALL");
                            newNode.Tag = _windowsList;
                            break;
                    }

                    newNode.ImageIndex = 1;
                    newNode.SelectedImageIndex = 1;
                    newNode.Text = nodeName;
                    newNode.Name = nodeName;
                    newNode.ToolTipText = nodeName;
                    
                    _treeViewWindows.Nodes.Add(newNode);
                    newNode.Expand();
                    newNode = null;
                }
            }

            //Mapper Windows
            newNode = _treeViewWindows.Nodes[nodeindex];
            newNode.Nodes.Clear();

            foreach (Windows.Window window in _mapperWindows)
            {
                nodeNum++;
                subNode = new TreeNode();
                subNode.ImageIndex = 1;
                subNode.SelectedImageIndex = 1;
                subNode.Tag = window;
                nodeName = string.Format("{0}", window.Title);
                subNode.Text = nodeName;
                subNode.Name = nodeName;
                subNode.ToolTipText = nodeName;
                newNode.Nodes.Add(subNode);
                subNode = null;
            }
            newNode = null;

            //Browser Windows
            nodeindex++;
            newNode = _treeViewWindows.Nodes[nodeindex];
            newNode.Nodes.Clear();

            foreach (Windows.Window window in _browserWindows)
            {
                nodeNum++;
                subNode = new TreeNode();
                subNode.ImageIndex = 1;
                subNode.SelectedImageIndex = 1;
                subNode.Tag = window;
                nodeName = string.Format("{0}", window.Title);
                subNode.Text = nodeName;
                subNode.Name = nodeName;
                subNode.ToolTipText = nodeName;
                newNode.Nodes.Add(subNode);
                subNode = null;
            }

            newNode = null;

            //Layout Windows
            nodeindex++;
            newNode = _treeViewWindows.Nodes[nodeindex];
            newNode.Nodes.Clear();

            foreach (Windows.Window window in _layoutWindows)
            {
                nodeNum++;
                subNode = new TreeNode();
                subNode.ImageIndex = 1;
                subNode.SelectedImageIndex = 1;
                subNode.Tag = window;
                nodeName = string.Format("{0}", window.Title);
                subNode.Text = nodeName;
                subNode.Name = nodeName;
                subNode.ToolTipText = nodeName;
                newNode.Nodes.Add(subNode);
                subNode = null;
            }

            newNode = null;

            //Legend Designer Windows
            nodeindex++;
            newNode = _treeViewWindows.Nodes[nodeindex];
            newNode.Nodes.Clear();

            foreach (Windows.Window window in _legendWindows)
            {
                nodeNum++;
                subNode = new TreeNode();
                subNode.ImageIndex = 1;
                subNode.SelectedImageIndex = 1;
                subNode.Tag = window;
                nodeName = string.Format("{0}", window.Title);
                subNode.Text = nodeName;
                subNode.Name = nodeName;
                subNode.ToolTipText = nodeName;
                newNode.Nodes.Add(subNode);
                subNode = null;
            }

            newNode = null;

            //Windows list in general
            nodeindex++;
            newNode = _treeViewWindows.Nodes[nodeindex];
            newNode.Nodes.Clear();

            foreach (Windows.Window window in _windowsList)
            {
                nodeNum++;
                subNode = new TreeNode();
                subNode.ImageIndex = 1;
                subNode.SelectedImageIndex = 1;
                subNode.Tag = window;
                nodeName = string.Format("{0}", window.Title);
                subNode.Text = nodeName;
                subNode.Name = nodeName;
                subNode.ToolTipText = nodeName;
                newNode.Nodes.Add(subNode);
                subNode = null;
            }

            //UpdateWindowsButtons(_treeViewWindows.SelectedNode);
            UpdateWindowsButtons();
        }

        //-----------------------------------------------------------------
        public void AutolockWindow(int windowID)
        {
            if (_autolockWindowEnabled == true)
            {
                foreach (Windows.Window window in _windowsList)
                {
                    if (window.ID == windowID)
                    {
                        window.SystemMenuClose = false;
                        //UpdateWindowsButtons(_treeViewWindows.SelectedNode);
                        UpdateWindowsButtons();
                        return;
                    }
                }
            }
        }


        //-----------------------------------------------------------------
        private void _treeViewWindows_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (node == null)
                return;

            if (IsNodeAWindow(node))
            {
                Windows.Window window = (Windows.Window)node.Tag;
                window.SetFront();
            }

            UpdateWindowsButtons();

            _treeViewWindows.Select();
        }

        //-----------------------------------------------------------------
        private void _treeViewWindows_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (node == null)
                return;

            _ModifyWindowUsingTreeNode(node);
        }

        //-----------------------------------------------------------------
        private void _ModifyWindowUsingTreeNode(TreeNode node)
        {
            if (node == null)
                return;

            if (IsNodeAWindow(node))
            {
                Windows.Window window = (Windows.Window)node.Tag;
                window.SetFront();

                _ModifyWindow(window);
            }
            _treeViewWindows.Select();
        }

        //-----------------------------------------------------------------
        private void _ModifyWindow(Windows.Window window)
        {
            string name = window.Title;
            
            ModifyWindowForm dlgModifyWindow = new ModifyWindowForm();
            dlgModifyWindow.Window = window;
            dlgModifyWindow.ShowDialog();

            if (name != window.Title)
                 RefreshWindowsList();
        }

        #endregion

        //*****************************************************************************    
        #region Windows Buttons

        //-----------------------------------------------------------------
        private void _checkBoxAutolockWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (_autolockWindowEnabled == false)
            {
                foreach (Windows.Window window in _windowsList)
                {
                    window.SystemMenuClose = false;
                }

                _checkBoxAutolockWindows.Checked = true;
                _autolockWindowEnabled = true;
            }
            else
            {
                _checkBoxAutolockWindows.Checked = false;
                _autolockWindowEnabled = false;
            }

            UpdateWindowsButtons();
        }

        //-----------------------------------------------------------------
        private void _buttonLockWindow_Click(object sender, EventArgs e)
        {
              _activeWindow.SystemMenuClose = false;
              UpdateWindowsButtons();
        }

        //-----------------------------------------------------------------
        private void _buttonUnlockWindow_Click(object sender, EventArgs e)
        {
             _activeWindow.SystemMenuClose = true;
             UpdateWindowsButtons();
        }

        //-----------------------------------------------------------------
        private void _buttonCloseWindow_Click(object sender, EventArgs e)
        {
            if (_activeWindow.SystemMenuClose == false)
            {
                if (MessageBox.Show(Controller.GetResItemStr("QUEST_CLOSE_WINDOW", _activeWindow.Title), Controller.GetResItemStr("STR_CLOSE_WINDOW"), MessageBoxButtons.YesNo) == DialogResult.No)
                   return;
            }
            _activeWindow.Close(true);
        }

        //-----------------------------------------------------------------
        private void _buttonCurrentMakeDockable_Click(object sender, EventArgs e)
        {
            if (_activeWindow.IsMadeDockable() == false)
                _activeWindow.MakeDockable();
        }

        //-----------------------------------------------------------------
        private void _buttonCloneWindow_Click(object sender, EventArgs e)
        {
            _activeWindow.Clone();
        }

        //-----------------------------------------------------------------
        private void _buttonCurrentOptions_Click(object sender, EventArgs e)
        {
            _ModifyWindow(_activeWindow);
        }

        //-----------------------------------------------------------------
        private void _buttonZoomToPrevious_Click(object sender, EventArgs e)
        {
            ZoomNextAndPreviousZoomToPrevious();
        }
        //-----------------------------------------------------------------
        private void _buttonZoomToNext_Click(object sender, EventArgs e)
        {
            ZoomNextAndPreviousZoomToNext();
        }
        //-----------------------------------------------------------------
        private void _buttonZoomToFirst_Click(object sender, EventArgs e)
        {
            ZoomNextAndPreviousZoomToFirst();
        }
        //-----------------------------------------------------------------
        private void _buttonZoomToLast_Click(object sender, EventArgs e)
        {
            ZoomNextAndPreviousZoomToLast();
        }


        #endregion

        //*****************************************************************************    
        #region Closed Windows TreeView
        public void AddToClosedWindowList(int windowID, string windowName, int windowType)
        {
            Windows.ClosedWindow window = new WindowHelper.Windows.ClosedWindow(windowID, windowName, windowType);

            if (_closedWindowsList == null)
                _closedWindowsList = new WindowHelper.Windows.ClosedWindowList();
            if (_closedMapperWindows == null)
                _closedMapperWindows = new WindowHelper.Windows.ClosedWindowList();
            if (_closedBrowserWindows == null)
                _closedBrowserWindows = new WindowHelper.Windows.ClosedWindowList();
            if (_closedLayoutWindows == null)
                _closedLayoutWindows = new WindowHelper.Windows.ClosedWindowList();
            if (_closedLegendWindows == null)
                _closedLegendWindows = new WindowHelper.Windows.ClosedWindowList();

            _closedWindowsList.Add(window);

            switch (window.Type)
            {
                case Windows.Window.WindowType.Mapper:
                    _closedMapperWindows.Add(window);
                    break;
                case Windows.Window.WindowType.Browser:
                    _closedBrowserWindows.Add(window);
                    break;
                case Windows.Window.WindowType.Layout:
                    _closedLayoutWindows.Add(window);
                    break;
                case Windows.Window.WindowType.LegendDesigner:
                    _closedLegendWindows.Add(window);
                    break;
            }

            RefreshClosedWindowsList();
        }

        public void RefreshClosedWindowsList()
        {
            TreeNode newNode = null;
            TreeNode subNode = null;
            int nodeNum = 0;
            string nodeName = "";
            int nodeindex = 0;

            //Adding the main nodes to the tree
            if (_treeViewClosedWindows.Nodes.Count == 0)
            {
                for (int i = 0; i <= 4; i++)
                {
                    newNode = new TreeNode();

                    switch (i)
                    {
                        case 0:
                            nodeName = Controller.GetResItemStr("LST_CLOSED_WINDOWS_MAPS");
                            newNode.Tag = _closedMapperWindows;
                            break;
                        case 1:
                            nodeName = Controller.GetResItemStr("LST_CLOSED_WINDOWS_BROWSERS");
                            newNode.Tag = _closedBrowserWindows;
                            break;
                        case 2:
                            nodeName = Controller.GetResItemStr("LST_CLOSED_WINDOWS_LAYOUTS");;
                            newNode.Tag = _closedLayoutWindows;
                            break;
                        case 3:
                            nodeName = Controller.GetResItemStr("LST_CLOSED_WINDOWS_LEGENDS"); ;
                            newNode.Tag = _closedLegendWindows;
                            break;
                        case 4:
                            nodeName = Controller.GetResItemStr("LST_CLOSED_WINDOWS_ALL");
                            newNode.Tag = _closedWindowsList;
                            break;
                    }

                    newNode.ImageIndex = 1;
                    newNode.SelectedImageIndex = 1;
                    newNode.Text = nodeName;
                    newNode.Name = nodeName;
                    newNode.ToolTipText = nodeName;
                    _treeViewClosedWindows.Nodes.Add(newNode);
                    newNode.Expand();
                    newNode = null;
                }
            }

            //Mapper Windows
            newNode = _treeViewClosedWindows.Nodes[nodeindex];
            newNode.Nodes.Clear();

            foreach (Windows.ClosedWindow window in _closedMapperWindows)
            {
                nodeNum++;
                subNode = new TreeNode();
                subNode.ImageIndex = 1;
                subNode.SelectedImageIndex = 1;
                subNode.Tag = window;
                nodeName = string.Format("{0} {1}", window.ClosedAtTimeOfDay, window.Name);
                subNode.Text = nodeName;
                subNode.Name = nodeName;
                subNode.ToolTipText = nodeName;
                newNode.Nodes.Add(subNode);
                subNode = null;
            }
            newNode = null;

            //Browser Windows
            nodeindex++;
            newNode = _treeViewClosedWindows.Nodes[nodeindex];
            newNode.Nodes.Clear();

            foreach (Windows.ClosedWindow window in _closedBrowserWindows)
            {
                nodeNum++;
                subNode = new TreeNode();
                subNode.ImageIndex = 1;
                subNode.SelectedImageIndex = 1;
                subNode.Tag = window;
                nodeName = string.Format("{0} {1}", window.ClosedAtTimeOfDay, window.Name);
                subNode.Text = nodeName;
                subNode.Name = nodeName;
                subNode.ToolTipText = nodeName;
                newNode.Nodes.Add(subNode);
                subNode = null;
            }

            newNode = null;

            //Layout Windows
            nodeindex++;
            newNode = _treeViewClosedWindows.Nodes[nodeindex];
            newNode.Nodes.Clear();

            foreach (Windows.ClosedWindow window in _closedLayoutWindows)
            {
                nodeNum++;
                subNode = new TreeNode();
                subNode.ImageIndex = 1;
                subNode.SelectedImageIndex = 1;
                subNode.Tag = window;
                nodeName = string.Format("{0} {1}", window.ClosedAtTimeOfDay, window.Name);
                subNode.Text = nodeName;
                subNode.Name = nodeName;
                subNode.ToolTipText = nodeName;
                newNode.Nodes.Add(subNode);
                subNode = null;
            }

            newNode = null;

            //Layout Windows
            nodeindex++;
            newNode = _treeViewClosedWindows.Nodes[nodeindex];
            newNode.Nodes.Clear();

            foreach (Windows.ClosedWindow window in _closedLegendWindows)
            {
                nodeNum++;
                subNode = new TreeNode();
                subNode.ImageIndex = 1;
                subNode.SelectedImageIndex = 1;
                subNode.Tag = window;
                nodeName = string.Format("{0} {1}", window.ClosedAtTimeOfDay, window.Name);
                subNode.Text = nodeName;
                subNode.Name = nodeName;
                subNode.ToolTipText = nodeName;
                newNode.Nodes.Add(subNode);
                subNode = null;
            }

            newNode = null;

            //Windows list in general
            nodeindex++;
            newNode = _treeViewClosedWindows.Nodes[nodeindex];
            newNode.Nodes.Clear();

            foreach (Windows.ClosedWindow window in _closedWindowsList)
            {
                nodeNum++;
                subNode = new TreeNode();
                subNode.ImageIndex = 1;
                subNode.SelectedImageIndex = 1;
                subNode.Tag = window;
                nodeName = string.Format("{0} {1} ({2})", window.ClosedAtTimeOfDay, window.Name, window.TypeAsString());
                subNode.Text = nodeName;
                subNode.Name = nodeName;
                subNode.ToolTipText = nodeName;
                newNode.Nodes.Add(subNode);
                subNode = null;
            }
        }

        private void _treeviewClosedWindows_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (node == null)
                return;

            if (IsNodeAClosedWindow(node))
            {
                Windows.ClosedWindow window = (Windows.ClosedWindow)node.Tag;
                window.Restore();
            }
        }
        
        #endregion

        //*****************************************************************************    
        #region Closed Window Buttons
        private void _buttonRestoreWindow_Click(object sender, EventArgs e)
        {
            TreeNode node = null;
            node = _treeViewClosedWindows.SelectedNode;
            if (node == null)
                return;

            if (IsNodeAClosedWindow(node))
            {
                Windows.ClosedWindow window = (Windows.ClosedWindow)node.Tag;
                window.Restore();
            }
        }
        #endregion

        //*****************************************************************************    
        #region Special Windows TreeView

        public void LoadSpecialWindows()
        {
            TreeNode newNode = null;
            TreeNode subNode = null;
            int nodeNum = 0;
            string nodeName = "";
            Windows.Window specialWindow = null;

            if (_specialWindows == null)
                _specialWindows = new WindowHelper.Windows.WindowList();
            else
                _specialWindows.Clear();


            for (int i = 0; i <= 5; i++)
            {
                switch (i)
                {
                    //Define WIN_MAPPER         1
                    //Define WIN_BROWSER        2
                    //Define WIN_LAYOUT         3
                    //Define WIN_GRAPH          4
                    //Define WIN_BUTTONPAD      19
                    //Define WIN_TOOLBAR        25
                    //Define WIN_CART_LEGEND    27
                    //Define WIN_3DMAP          28
                    //Define WIN_ADORNMENT      32
                    //Define WIN_HELP           1001
                    //Define WIN_MAPBASIC       1002
                    //Define WIN_MESSAGE        1003
                    //Define WIN_RULER          1007
                    //Define WIN_INFO           1008
                    //Define WIN_LEGEND         1009
                    //Define WIN_STATISTICS     1010
                    //Define WIN_MAPINFO        1011

                    case 0:
                        specialWindow = new WindowHelper.Windows.Window(1008);
                        break;
                    case 1:
                        specialWindow = new WindowHelper.Windows.Window(1009);
                        break;
                    case 2:
                        specialWindow = new WindowHelper.Windows.Window(1002);
                        break;
                    case 3:
                        specialWindow = new WindowHelper.Windows.Window(1003);
                        break;
                    case 4:
                        specialWindow = new WindowHelper.Windows.Window(1007);
                        break;
                    case 5:
                        specialWindow = new WindowHelper.Windows.Window(1010);
                        break;
                }

                //MessageBox.Show(specialWindow.Title);
                _specialWindows.Add(specialWindow);
                specialWindow = null;
            }

            newNode = new TreeNode();
            nodeName = Controller.GetResItemStr("LST_WINDOWS_SPECIAL");
            newNode.Tag = _specialWindows;
            newNode.ImageIndex = 1;
            newNode.SelectedImageIndex = 1;
            newNode.Text = nodeName;
            newNode.Name = nodeName;
            newNode.ToolTipText = nodeName;
            _treeViewSpecialWindows.Nodes.Add(newNode);
            newNode.Expand();

            //Adding the special windows to the tree
            foreach (Windows.Window window in _specialWindows)
            {
                nodeNum++;
                subNode = new TreeNode();
                subNode.ImageIndex = 1;
                subNode.SelectedImageIndex = 1;
                subNode.Tag = window;
                nodeName = string.Format("{0}", window.Title);
                subNode.Text = nodeName;
                subNode.Name = nodeName;
                subNode.ToolTipText = nodeName;
                newNode.Nodes.Add(subNode);
                subNode = null;
            }
        }

        public void StoreDockedSpecialWindows()
        {
            //MessageBox.Show("Now we are storing the docked special windows!");
            foreach (Windows.Window window in _specialWindows)
            {
                //MessageBox.Show("Window " + window.Title);
                if (window.IsMadeDockable() == true)
                {
                    window.DockedWindow.Close();
                }
            }
        }

        private void _treeViewSpecialWindows_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (node == null)
                return;

            if (IsNodeAWindow(node))
            {
                Windows.Window window = (Windows.Window)node.Tag;
            }
        }

        private void _treeViewSpecialWindows_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (node == null)
                return;

            if (IsNodeAWindow(node))
            {
                Windows.Window window = (Windows.Window)node.Tag;
                if (window.IsMadeDockable())
                {
                    window.DockedWindow.Visible = true;
                    window.DockedWindow.Activate();
                }
                else
                {
                    if (window.IsOpen() == false)
                        window.Open();
                }
            }
            _treeViewWindows.Select();
        }

        #endregion

        //*****************************************************************************    
        #region Special Windows Buttons
        private void _buttonMakeDockable_Click(object sender, EventArgs e)
        {
            TreeNode node = null;
            node = _treeViewSpecialWindows.SelectedNode;
            if (node == null)
                return;

            if (IsNodeAWindow(node))
            {
                Windows.Window window = (Windows.Window)node.Tag;
                if (window.IsMadeDockable() != true)
                    window.MakeDockable();
            }
            //_treeViewWindows.Select();
        }

        #endregion

        //*****************************************************************************    
        #region TreeView Node to Window helper functions
        private bool IsNodeAWindow(TreeNode node)
        {
            if (node == null)
                return false;
            if (node.Tag == null)
                return false;

            if (node.Tag.GetType().ToString() == "WindowHelper.Windows.Window")
                return true;
            else
                return false;
        }

        private bool IsNodeAMapperWindow(TreeNode node)
        {
            if (node == null)
                return false;
            if (node.Tag == null)
                return false;

            if (IsNodeAWindow(node))
            {    
                Windows.Window window = (Windows.Window)node.Tag;
                if (window.Type == Windows.Window.WindowType.Mapper)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private bool IsNodeABrowserWindow(TreeNode node)
        {
            if (node == null)
                return false;
            if (node.Tag == null)
                return false;

            if (IsNodeAWindow(node))
            {
                Windows.Window window = (Windows.Window)node.Tag;
                if (window.Type == Windows.Window.WindowType.Browser)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private bool IsNodeALayoutWindow(TreeNode node)
        {
            if (node == null)
                return false;
            if (node.Tag == null)
                return false;

            if (IsNodeAWindow(node))
            {
                Windows.Window window = (Windows.Window)node.Tag;
                if (window.Type == Windows.Window.WindowType.Layout)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        private bool IsNodeAGraphWindow(TreeNode node)
        {
            if (node == null)
                return false;
            if (node.Tag == null)
                return false;

            if (IsNodeAWindow(node))
            {
                Windows.Window window = (Windows.Window)node.Tag;
                if (window.Type == Windows.Window.WindowType.Graph)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        private bool IsNodeAClosedWindow(TreeNode node)
        {
            if (node == null)
                return false;
            if (node.Tag == null)
                return false;

            if (node.Tag.GetType().ToString() == "WindowHelper.Windows.ClosedWindow")
                return true;
            else
                return false;
        }

        #endregion

        //*****************************************************************************    
        #region Zoom Next and Previous

        //-----------------------------------------------------------------
        public void ZoomNextAndPreviousAddWindow(int windowID)
        {
            try
            {
                Windows.Window mapWindow = new Windows.Window(windowID);
                if (mapWindow.Type != Windows.Window.WindowType.Mapper)
                    return;

                if (_mapWindowExtents.WindowExists(mapWindow) == false)
                {
                    Windows.MapWindowExtents mapExtent = new Windows.MapWindowExtents(mapWindow);
                    _mapWindowExtents.Add(mapExtent);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("{0} Exception caught.", e));
            }
        }

        //-----------------------------------------------------------------
        public void ZoomNextAndPreviousRemoveWindow(int windowID)
        {
            Windows.Window mapWindow = new Windows.Window(windowID);
            if (mapWindow.Type != Windows.Window.WindowType.Mapper)
                return;

            int index = _mapWindowExtents.FindWindow(mapWindow);
            if (index >= 0)
                _mapWindowExtents.RemoveAt(index);
        }

        //-----------------------------------------------------------------
        public void ZoomNextAndPreviousAddExtent(int windowID)
        {
            try
            {

                Windows.Window mapWindow = new Windows.Window(windowID);
                if (mapWindow.Type != Windows.Window.WindowType.Mapper)
                    return;

                int index = _mapWindowExtents.FindWindow(mapWindow);
                if (index >= 0)
                {
                    int extentIndex = _mapWindowExtents[index].MapExtentList.AddExtent(mapWindow);
                }
            }
            catch (Exception e)
            {
                WindowHelper.InteropHelper.PrintMessage(string.Format("{0} Exception caught.", e));
            }
        }

        //-----------------------------------------------------------------
        public void ZoomNextAndPreviousZoomToPrevious()
        {
            if (_activeWindow.Type != Windows.Window.WindowType.Mapper)
                return;

            int index = _mapWindowExtents.FindWindow(_activeWindow);
            if (index >= 0)
            {
                _mapWindowExtents[index].MapExtentList.ZoomToPrevious(_activeWindow);
            }
        }
        //-----------------------------------------------------------------
        public void ZoomNextAndPreviousZoomToNext()
        {
            if (_activeWindow.Type != Windows.Window.WindowType.Mapper)
                return;

            int index = _mapWindowExtents.FindWindow(_activeWindow);
            if (index >= 0)
            {
                _mapWindowExtents[index].MapExtentList.ZoomToNext(_activeWindow);
            }
        }
        //-----------------------------------------------------------------
        public void ZoomNextAndPreviousZoomToFirst()
        {
            if (_activeWindow.Type != Windows.Window.WindowType.Mapper)
                return;

            int index = _mapWindowExtents.FindWindow(_activeWindow);
            if (index >= 0)
            {
                _mapWindowExtents[index].MapExtentList.ZoomToFirst(_activeWindow);
            }
        }
        //-----------------------------------------------------------------
        public void ZoomNextAndPreviousZoomToLast()
        {
            if (_activeWindow.Type != Windows.Window.WindowType.Mapper)
                return;

            int index = _mapWindowExtents.FindWindow(_activeWindow);
            if (index >= 0)
            {
                _mapWindowExtents[index].MapExtentList.ZoomToLast(_activeWindow);
            }
        }

        #endregion
    }
}
