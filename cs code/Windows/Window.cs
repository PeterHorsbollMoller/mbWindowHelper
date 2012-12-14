using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WindowHelper.Windows
{
    public class Window
    {
        private int _windowID = 0;
        private string _windowTitle = "";
        private int _windowType = 0;
        private DockableWindowForm _dockableWindow = null;

        /// <summary>
        /// Type of Window.
        /// Indicates what type of window the window is
        /// </summary>
        public enum WindowType
        {
            Mapper = 1,
            Browser = 2,
            Layout = 3,
            Graph = 4,
            ButtonPad = 19,
            Toolbar = 25,
            CartographicLegend = 27,
            Mapper3D = 28,
            LegendDesigner = 35,
            Help = 1001,
            MapBasic = 1002,
            Message = 1003,
            Ruler = 1007,
            Info = 1008,
            Legend = 1009,
            Statistics = 1010,
            MapInfo = 1011,
            Undefined = 0
        }

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="windowID">ID of the Window</param>
        /// <returns></returns>
        public Window(int windowID, string windowName, int windowType)
        {
            _windowID = windowID;
            if (windowName == "" || windowName == null)
                _windowTitle = InteropHelper.GetWindowName(_windowID);
            else
                _windowTitle = windowName;
            if (windowType == 0)
                _windowType = InteropHelper.GetWindowType(_windowID);
            else
                _windowType = windowType;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="windowID">ID of the Window</param>
        /// <returns></returns>
        public Window(int windowID, string windowName)
        {
            _windowID = windowID;
            if (windowName == "" || windowName == null)
                _windowTitle = InteropHelper.GetWindowName(_windowID);
            else
                _windowTitle = windowName;

            _windowType = InteropHelper.GetWindowType(_windowID);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="windowID">ID of the Window</param>
        /// <returns></returns>
        public Window(int windowID)
        {
            _windowID = windowID;
            _windowTitle = InteropHelper.GetWindowName(_windowID);
            _windowType = InteropHelper.GetWindowType(_windowID);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Property.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Get the Title of the Window</returns>
        public int ID
        {
            get { return _windowID; }
            //set { _windowID = value;}
        }

        /// <summary>
        /// Property.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Get the Window Handle of the Window</returns>
        public IntPtr Handle
        {
            get
            {
                string handle = InteropHelper.Eval(String.Format("WindowInfo({0},12)", this.ID));
                return new IntPtr(Convert.ToInt32(handle));
            }
        }

        /// <summary>
        /// Property.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Get the Type of the Window</returns>
        public WindowType Type
        {
            get 
            {
                if (Enum.IsDefined(typeof(WindowType), _windowType))
                    return (WindowType)_windowType;
                else
                    return WindowType.Undefined;
            }
        }

        /// <summary>
        /// Property.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Set or Get the Title of the Window</returns>
        public string Title
        {
            get { return _windowTitle; }
            set 
            {   
                _windowTitle = value;
                InteropHelper.Do(string.Format("Set Window {0} Title \"{1}\"", _windowID, _windowTitle));

                if (IsMadeDockable() == true)
                    _dockableWindow.Title = _windowTitle;
            }
        }

        /// <summary>
        /// Property.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Set or Get the Width of the Window</returns>
        public double Width
        {
            get {
                //if (IsMadeDockable() == true)
                //    return _dockableWindow.Width();
                //else
                    return InteropHelper.GetWindowWidth(_windowID); 
                }
            set 
            { 
                InteropHelper.Do(string.Format("Set Window {0} Width {1}", _windowID, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture))); 
            }
        }

        /// <summary>
        /// Property.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Set or Get the Height of the Window</returns>
        public double Height
        {
            get {
                //if (IsMadeDockable() == true)
                //    return _dockableWindow.Height();
                //else
                    return InteropHelper.GetWindowHeight(_windowID);
                }
            set 
            { 
                InteropHelper.Do(string.Format("Set Window {0} Height {1}", _windowID, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture)));
            }
        }

        /// <summary>
        /// Property.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Locks/unlocks the window and ask what it is currently</returns>
        public bool SystemMenuClose
        {
            get { return InteropHelper.GetWindowSystemMenuClose(_windowID); }
            set 
            { 
                if  (value == true)
                    InteropHelper.Do(string.Format("Set Window {0} SysMenuClose On", _windowID)); 
                else
                    InteropHelper.Do(string.Format("Set Window {0} SysMenuClose Off", _windowID));
            }
        }

        /// <summary>
        /// Property.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Locks/unlocks the window and ask what it is currently</returns>
        public DockableWindowForm DockedWindow
        {
            get { return _dockableWindow; }
        }

        #endregion

        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Resets the Title of the Window to the default (dynamic) value</returns>
        public void ResetName()
        {
            InteropHelper.Do(string.Format("Set Window {0} Title Default", _windowID));
            _windowTitle = InteropHelper.GetWindowName(_windowID);
        }

        //------------------------------------------------------------
        #region Methods for Opening/Closing
        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Closes the Window</returns>
        public void Close()
        {
            Close(false);
        }

        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Closes the Window</returns>
        public void Close(bool interactive)
        {
            if (IsMadeDockable() == true)
            {
                _dockableWindow.Close();
                _dockableWindow = null;
            }

            if (interactive == true)
                InteropHelper.Do(string.Format("Close Window {0} Interactive", _windowID));
            else
                InteropHelper.Do(string.Format("Close Window {0}", _windowID));
        }

        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Opens the Window - if it is of a certain type</returns>
        public void Open()
        {
            switch (this.Type)
            {
                case Window.WindowType.Info:
                    InteropHelper.Do(String.Format("Open Window {0}", this.ID));
                    break;
                case Window.WindowType.Legend:
                    InteropHelper.Do(String.Format("Open Window {0}", this.ID));
                    break;
                case Window.WindowType.MapBasic:
                    InteropHelper.Do(String.Format("Open Window {0}", this.ID));
                    break;
                case Window.WindowType.Message:
                    InteropHelper.Do(String.Format("Open Window {0}", this.ID));
                    break;
                case Window.WindowType.Ruler:
                    InteropHelper.Do(String.Format("Open Window {0}", this.ID));
                    break;
                case Window.WindowType.Statistics:
                    InteropHelper.Do(String.Format("Open Window {0}", this.ID));
                    break;
                //case else:
                //Do nothing only the windows above can be opened
            }
        }

        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Is this Window Open - if it is of a certain type</returns>
        public bool IsOpen()
        {
            string isOpen = InteropHelper.Eval(String.Format("WindowInfo({0}, 11)", this.ID));
            if (isOpen == "T")
                return true;
            else
                return false;
        }
        #endregion

        //------------------------------------------------------------
        #region Methods for Min/Max/Restore/Set Front
        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Makes the Window the front most window</returns>
        public void SetFront()
        {
            InteropHelper.Do(string.Format("Set Window {0} Front", _windowID));

            if (IsMadeDockable() == true)
            {
                _dockableWindow.Activate();
                _dockableWindow.BringToFront();
            }
        }

        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Maximizes the Window</returns>
        public void Maximize()
        {
            InteropHelper.Eval(String.Format("Set Window {0} Max", _windowID));
        }

        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Minimizes the Window</returns>
        public void Minimize()
        {
            InteropHelper.Eval(String.Format("Set Window {0} Min", _windowID));
        }

        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Restores the Window</returns>
        public void Restore()
        {
            InteropHelper.Eval(String.Format("Set Window {0} Restore", _windowID));
        }
        #endregion

        //------------------------------------------------------------
        #region Methods for Cloning/Updating
        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Closes the Window</returns>
        public void Clone()
        {
            InteropHelper.Do(string.Format("Run Command WindowInfo({0},15)", _windowID));
        }

        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Updates/Refreshes the Window</returns>
        public void Update()
        {
            InteropHelper.Do(string.Format("Update Window {0}", _windowID));
        }
        #endregion

        //------------------------------------------------------------
        #region Methods for Docking
        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Makes the Window into a dockable window</returns>
        public void MakeDockable()
        {
            Win32Window window = new Win32Window(_windowID);

            switch (Type)
            {
                case WindowType.Browser:
                    _dockableWindow = new DockableWindowForm(window);
                    break;
                case WindowType.CartographicLegend:
                    _dockableWindow = new DockableWindowForm(window);
                    break;
                case WindowType.LegendDesigner:
                    _dockableWindow = new DockableWindowForm(window);
                    break;
                case WindowType.Graph:
                    _dockableWindow = new DockableWindowForm(window);
                    break;
                case WindowType.Layout:
                    _dockableWindow = new DockableWindowForm(window);
                    break;
                case WindowType.Mapper:
                    _dockableWindow = new DockableWindowForm(window);
                    break;
                case WindowType.Mapper3D:
                    _dockableWindow = new DockableWindowForm(window);
                    break;

                default:
                    _dockableWindow = new DockableWindowForm(window, window.Title);
                    break;
            }
        }

        ///// <summary>
        ///// Method.
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns>Makes the Window into a not dockable window</returns>
        //public void MakeUndockable()
        //{
        //    _dockableWindow.ReleaseWindow();
        //}

        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>True if the window has been made dockable</returns>
        public bool IsMadeDockable()
        {
            if (_dockableWindow == null)
                return false;
            else
                return true;
        }
        #endregion

        //------------------------------------------------------------
        #region Window Type
        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>A string representing the window, here the name</returns>
        public string TypeAsString()
        {
            string type = "Undefined";
            switch (this.Type)
            {
                case WindowType.Mapper:
                    type = "Mapper";
                    break;
                case WindowType.Browser:
                    type = "Browser";
                    break;
                case WindowType.Layout:
                    type = "Layout";
                    break;
                case WindowType.Graph:
                    type = "Graph";
                    break;
                case WindowType.Mapper3D:
                    type = "3DMap";
                    break;
                case WindowType.MapBasic:
                    type = "MapBasic";
                    break;
                case WindowType.CartographicLegend:
                    type = "Cartographic Legend";
                    break;
                case WindowType.LegendDesigner:
                    type = "Legend Designer";
                    break;
                case WindowType.ButtonPad:
                    type = "Buttonpad";
                    break;
                case WindowType.Toolbar:
                    type = "Toolbar";
                    break;
                //case WindowType.Adornment:
                //    type = "Adornment";
                //    break;
                case WindowType.Help:
                    type = "Help";
                    break;
                case WindowType.Message:
                    type = "Message";
                    break;
                case WindowType.Ruler:
                    type = "Ruler";
                    break;
                case WindowType.Info:
                    type = "Info";
                    break;
                case WindowType.Legend:
                    type = "Legend";
                    break;
                case WindowType.Statistics:
                    type = "Statistics";
                    break;
                case WindowType.MapInfo:
                    type = "MapInfo";
                    break;
            }
            return type;
        }

        public bool IsSpecialWindow()
        {
            bool isSpecial = false;
            switch (this.Type)
            {
                case WindowType.MapBasic:
                    isSpecial = true;
                    break;
                case WindowType.Message:
                    isSpecial = true;
                    break;
                case WindowType.Ruler:
                    isSpecial = true;
                    break;
                case WindowType.Info:
                    isSpecial = true;
                    break;
                case WindowType.Legend:
                    isSpecial = true;
                    break;
                case WindowType.Statistics:
                    isSpecial = true;
                    break;
            }
            return isSpecial;
        }

        public bool IsDocumentWindow()
        {
            bool isDocument = false;
            switch (this.Type)
            {
                case WindowType.Mapper:
                    isDocument = true;
                    break;
                case WindowType.Browser:
                    isDocument = true;
                    break;
                case WindowType.Layout:
                    isDocument = true;
                    break;
                case WindowType.Graph:
                    isDocument = true;
                    break;
                case WindowType.Mapper3D:
                    isDocument = true;
                    break;
                case WindowType.CartographicLegend:
                    isDocument = true;
                    break;
                case WindowType.LegendDesigner:
                    isDocument = true;
                    break;
            }
            return isDocument;
        }

        #endregion

        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>A string representing the window, here the name</returns>
        public override string ToString()
        {
            return _windowTitle;
        }
    
    }
}
