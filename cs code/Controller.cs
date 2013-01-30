using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Threading;
using WindowHelper.Geometry;
using MapInfo.MiPro.Interop;

namespace WindowHelper
{
    public class Controller
    {
        private static WindowWrapper _winWrap = null;
        private static WindowHelperForm _winhelperDlg = null;

        #region MapInfo Handlers for the Resource strings
        /// <summary>
        /// This is called from MapBasic code
        /// to get a named resource string
        /// </summary>
        /// <param name="itemName">Title of the string resource</param>
        /// <returns>Value of the string resource</returns>
        public static string GetResItemStr(string itemName)
        {
            string str = "";
            str = Properties.Resources.ResourceManager.GetString(itemName, Properties.Resources.Culture);
            if (str == "" || str == null)
            {
                str = itemName;
                str = str.Replace("_", " ");
                str = str.ToLowerInvariant();
            }
            return str.Replace(@"{\n}", Environment.NewLine);
        }

        /// <summary>
        /// This is called from MapBasic code
        /// to get a named resource string
        /// and to replace {0}, {1}, etc. with values in the replaceString (; separated)
        /// </summary>
        /// <param name="itemName">Title of the string resource</param>
        /// <param name="replaceStrings">List of values to insert into the string, ; separated</param>
        /// <returns>Value of the string resource</returns>
        public static string GetResItemStr(string itemName, string replaceStrings)
        {
            string str = "";
            str = Properties.Resources.ResourceManager.GetString(itemName, Properties.Resources.Culture);

            //System.Windows.Forms.MessageBox.Show(string.Format("{0} replace with {1}", str, replaceStrings));
            if (str == "")
                str = itemName;

            //Searching for {i} to be replaced with values in replaceStrings
            if (replaceStrings != "")
            {
                string[] elements;
                char[] splitAt = new char[1];
                splitAt[0] = ';';
                elements = replaceStrings.Split(splitAt, System.StringSplitOptions.None);
                int i = 0;
                foreach (string element in elements)
                {
                    //System.Windows.Forms.MessageBox.Show(string.Format("{0} => {1}", i, element));
                    str = str.Replace("{" + i + "}", element);
                    i++;
                }
            }

            return str.Replace(@"{\n}", Environment.NewLine);
        }
        #endregion

        //<summary>
        /// </summary>
        /// <param name="filePath">Get screen size pixels</param>
        /// <returns>string: width x height in pixels, like '1920x1200'</returns>
        public static string GetScreenSize()
        {
            string size = "";
            size = string.Format("{0}x{1}", SystemInformation.PrimaryMonitorSize.Width, SystemInformation.PrimaryMonitorSize.Height);
            return size;
        }

        #region MapInfo Handlers for the WindowHelper dialog
        /// <summary>
        /// This function is called from MapBasic code
        /// when another window gets focus in MapInfo Pro
        /// </summary>
        /// <param name="hMainWnd"></param>
        /// <returns></returns>
        public static void WinHelpDlgWinFocusChanged(int windowID)
        {
            try
            {
                if ((_winhelperDlg == null) || (_winhelperDlg.IsDisposed))
                    return;
                else
                {
                    if (windowID == 0)
                    {
                        _winhelperDlg.ResetActiveWindow();
                    }
                    else
                    {
                        //For some reason the ID sent was 145 which didn't refer to any window in side Mi Pro
                        //for now we just ignore ID below 1000
                        if (windowID < 1000)
                            //Do nothing
                            return;
                        else
                        {
                            _winhelperDlg.SetActiveWindow(windowID);
                            _winhelperDlg.AutolockWindow(windowID);
                            _winhelperDlg.AddToOrUpdateInWindowLists(windowID, "", 0);
                            _winhelperDlg.ZoomNextAndPreviousAddWindow(windowID);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("{0} Exception caught.", e));
            }
        }

        /// <summary>
        /// This function is called from MapBasic code
        /// when a window is closed in MapInfo Pro
        /// </summary>
        /// <param name="hMainWnd"></param>
        /// <returns></returns>
        public static void WinHelpDlgWinClosed(int windowID)
        {
            try
            {
                if ((_winhelperDlg == null) || (_winhelperDlg.IsDisposed))
                    return;
                else
                {
                    //For some reason the ID sent was 145 which didn't refer to any window in side Mi Pro
                    //for now we just ignore ID below 1000
                    if (windowID < 1000)
                        //Do nothing
                        return;
                    else
                    {
                        _winhelperDlg.ResetActiveWindow();
                        _winhelperDlg.RemoveFromAllWindowsLists(windowID);
                        _winhelperDlg.ZoomNextAndPreviousRemoveWindow(windowID);
                    }
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("{0} Exception caught.", e));
            }

        }

                /// <summary>
        /// This function is called from MapBasic code
        /// when a window is changed in some way in MapInfo Pro
        /// </summary>
        /// <param name="hMainWnd"></param>
        /// <returns></returns>
        public static void WinHelpDlgWinChanged(int windowID)
        {
            try
            {
                if ((_winhelperDlg == null) || (_winhelperDlg.IsDisposed))
                    return;
                else
                {
                    //For some reason the ID sent was 145 which didn't refer to any window in side Mi Pro
                    //for now we just ignore ID below 1000
                    if (windowID < 1000)
                        //Do nothing
                        return;
                    else 
                        {
                            _winhelperDlg.ZoomNextAndPreviousAddExtent(windowID);
                            _winhelperDlg.AddToOrUpdateInWindowLists(windowID, "", 0);
                            _winhelperDlg.SetActiveWindow(windowID);
                        }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("{0} Exception caught.", e));
            }

        }

        /// <summary>
        /// This is called from MapBasic code
        /// to get a named resource string
        /// </summary>
        /// <param name="itemName">Title of the string resource</param>
        /// <returns>Value of the string resource</returns>
        //       public static string GetResItemStr(string itemName)
        //       {
        //           return Properties.Resources.ResourceManager.GetString(itemName, Properties.Resources.Culture);
        //       }

        /// <summary>
        /// This function is called from MapBasic code
        /// to display the WindowHelper dialog.
        /// </summary>
        /// <param name="hMainWnd"></param>
        /// <returns></returns>
        public static void WinHelpDlgShow(int hMainWnd)
        {
            if ((_winhelperDlg == null) || (_winhelperDlg.IsDisposed))
                _winhelperDlg = new WindowHelperForm("Window Helper");
        }
         
        /// <summary>
        /// This function is called to close the WindowHelper dialog
        ///and release the resources when the end handeler is called
        /// </summary>
        public static void WinHelpDlgClose()
        {
            if (_winhelperDlg != null)
            {
                _winhelperDlg.Close();
            }
        }

        /// <summary>
        /// This function is called from MapBasic code
        /// to add a window to the WindowHelper dialog.
        /// </summary>
        /// <param name="hMainWnd"></param>
        /// <param name="windowWID"></param>
        /// <param name="windowName"></param>
        /// <param name="windowType"></param>
        /// <returns></returns>
        public static void WinHelpDlgAddWindow(int hMainWnd, int windowID, string windowName, int windowType)
        {
            WinHelpDlgShow(hMainWnd);
            _winhelperDlg.AddToOrUpdateInWindowLists(windowID, windowName, windowType);

            _winhelperDlg.ZoomNextAndPreviousAddWindow(windowID);
        }

        /// <summary>
        /// This function is called from MapBasic code
        /// to add a window to the WindowHelper dialog.
        /// </summary>
        /// <param name="hMainWnd"></param>
        /// <param name="windowWID"></param>
        /// <param name="windowName"></param>
        /// <param name="windowType"></param>
        /// <returns></returns>
        public static void WinHelpDlgAddWindows(int hMainWnd, int[] windowIDs, string[] windowName, int[] windowType)
        {
            WinHelpDlgShow(hMainWnd);
            _winhelperDlg.AddToOrUpdateInWindowLists(windowIDs, windowName, windowType);

            foreach (int windowID in windowIDs)
                _winhelperDlg.ZoomNextAndPreviousAddWindow(windowID);
        }

        /// <summary>
        /// This function is called from MapBasic code
        /// to add all windows to the WindowHelper dialog.
        /// </summary>
        /// <param name="hMainWnd"></param>
        /// <param name="windowWID"></param>
        /// <param name="windowName"></param>
        /// <param name="windowType"></param>
        /// <returns></returns>
        public static void WinHelpDlgAddAllWindows(int hMainWnd, int[] windowIDs, string[] windowNames, int[] windowTypes)
        {
            WinHelpDlgShow(hMainWnd);
            _winhelperDlg.CreateWindowList(windowIDs, windowNames, windowTypes);

            foreach (int windowID in windowIDs)
                _winhelperDlg.ZoomNextAndPreviousAddWindow(windowID);
        }

        /// <summary>
        /// This function is called from MapBasic code
        /// to add a closed window to the WindowHelper dialog.
        /// </summary>
        /// <param name="hMainWnd"></param>
        /// <param name="windowWID"></param>
        /// <param name="windowName"></param>
        /// <param name="windowType"></param>
        /// <returns></returns>
        public static void WinHelpDlgAddClosedWindow(int hMainWnd, int windowID, string windowName, int windowType)
        {
            WinHelpDlgShow(hMainWnd);
            _winhelperDlg.AddToClosedWindowList(windowID, windowName, windowType);
        }

        /// <summary>
        /// This function is called from MapBasic code
        /// to add the special windows to the WindowHelper dialog.
        /// </summary>
        /// <param name="hMainWnd"></param>
        /// <param name="windowWID"></param>
        /// <param name="windowName"></param>
        /// <param name="windowType"></param>
        /// <returns></returns>
        public static void WinHelpDlgLoadSpecialWindows(int hMainWnd)
        {
            WinHelpDlgShow(hMainWnd);
            _winhelperDlg.LoadSpecialWindows();
        }

        #endregion MapInfo Handlers for the WindowHelper dialog

        ///****************************************************************************
        ///lets you bind the new form to the MapInfo Pro window
        ///****************************************************************************
        #region WindowWrapper
        /// <summary>
        /// Returns the window wrapper.
        /// If window wrapper is null it correctly initializes the static member
        /// </summary>
        /// <param name="hMainWnd">Handle to a window</param>
        /// <returns>Window wrapper for the given handle</returns>
        private static WindowWrapper GetWindowWrapper(int hMainWnd)
        {
            if (_winWrap == null)
            {
                IntPtr hwnd = new IntPtr(hMainWnd);
                _winWrap = new WindowWrapper(hwnd);
            }

            return _winWrap;
        }

        /// <summary>
        /// This class implements IWin32Window wrapping a handle to window.
        /// It is used to wrap the handle to a running instance of 
        /// MapInfo Professional application.
        /// </summary>
        public class WindowWrapper : System.Windows.Forms.IWin32Window
        {
            private IntPtr _hwnd;

            public WindowWrapper(IntPtr handle)
            {
                _hwnd = handle;
            }

            public IntPtr Handle
            {
                get { return _hwnd; }
            }
        }
        #endregion
        
    }


}
