using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowHelper.Windows
{
    class ClosedWindow
    {
        private int _windowID = 0;
        private string _windowName = "";
        private int _windowType = 0;
        private DateTime _closedAt = DateTime.Now;
        private string _restoreStatement = "";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="windowID">ID of the Window</param>
        /// <returns></returns>
        public ClosedWindow(int windowID, string windowName, int windowType, string restoreStatement)
        {
            _windowID = windowID;
            _windowName = windowName;
            _windowType = windowType;
            _restoreStatement = restoreStatement;
            _closedAt = DateTime.Now;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="windowID">ID of the Window</param>
        /// <returns></returns>
        public ClosedWindow(int windowID, string windowName, int windowType)
        {
            _windowID = windowID;
            _windowName = windowName;
            _windowType = windowType;
            _restoreStatement = InteropHelper.GetWindowCloneStatement(windowID);
            _closedAt = DateTime.Now;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="windowID">ID of the Window</param>
        /// <returns></returns>
        public ClosedWindow(int windowID, string windowName)
        {
            _windowID = windowID;
            _windowName = windowName;
            _windowType = InteropHelper.GetWindowType(windowID);
            _restoreStatement = InteropHelper.GetWindowCloneStatement(windowID);
            _closedAt = DateTime.Now;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="windowID">ID of the Window</param>
        /// <returns></returns>
        public ClosedWindow(int windowID)
        {
            _windowID = windowID;
            _windowName = InteropHelper.GetWindowName(windowID);
            _windowType = InteropHelper.GetWindowType(windowID);
            _restoreStatement = InteropHelper.GetWindowCloneStatement(windowID);
            _closedAt = DateTime.Now;
        }

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
        /// <returns>Get the DateTime when the window was closed</returns>
        public DateTime ClosedAt
        {
            get { return _closedAt; }
        }

        /// <summary>
        /// Property.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Get the TimeOfDay (as string) when the window was closed</returns>
        public string ClosedAtTimeOfDay
        {
            get 
            {
                string timeofday = string.Format("{0}:{1}:{2}", ClosedAt.TimeOfDay.Hours.ToString("00"), ClosedAt.TimeOfDay.Minutes.ToString("00"), ClosedAt.TimeOfDay.Seconds.ToString("00"));
                return timeofday; 
            }
        }

        /// <summary>
        /// Property.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Get the Type of the Window</returns>
        public Window.WindowType Type
        {
            get
            {
                if (Enum.IsDefined(typeof(Window.WindowType), _windowType))
                    return (Window.WindowType)_windowType;
                else
                    return Window.WindowType.Undefined;
            }
        }

        /// <summary>
        /// Property.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Set or Get the Title of the Window</returns>
        public string Name
        {
            get { return _windowName; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method.
        /// </summary>
        /// <param name=""></param>
        /// <returns>Restore the Window</returns>
        public void Restore()
        {
            InteropHelper.Do(_restoreStatement);
        }

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
                case Window.WindowType.Mapper:
                    type = "Mapper";
                    break;
                case Window.WindowType.Browser:
                    type = "Browser";
                    break;
                case Window.WindowType.Layout:
                    type = "Layout";
                    break;
                case Window.WindowType.Graph:
                    type = "Graph";
                    break;
                case Window.WindowType.Mapper3D:
                    type = "3DMap";
                    break;
                case Window.WindowType.MapBasic:
                    type = "MapBasic";
                    break;
                case Window.WindowType.CartographicLegend:
                    type = "Cartographic Legend";
                    break;
                case Window.WindowType.ButtonPad:
                    type = "Buttonpad";
                    break;
                case Window.WindowType.Toolbar:
                    type = "Toolbar";
                    break;
                //case Window.WindowType.Adornment:
                //    type = "Adornment";
                //    break;
                case Window.WindowType.Help:
                    type = "Help";
                    break;
                case Window.WindowType.Message:
                    type = "Message";
                    break;
                case Window.WindowType.Ruler:
                    type = "Ruler";
                    break;
                case Window.WindowType.Info:
                    type = "Info";
                    break;
                case Window.WindowType.Legend:
                    type = "Legend";
                    break;
                case Window.WindowType.Statistics:
                    type = "Statistics";
                    break;
                case Window.WindowType.MapInfo:
                    type = "MapInfo";
                    break;
            }
            return type;
        }
        # endregion
    }
}
