using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WindowHelper
{
    public class Win32Window : IWin32Window
    {
        private Windows.Window _window = null;
        private IntPtr _parent;

        //-------------------------------------------------
        #region Constructor
        public Win32Window(int windowID)
        {
            this.ID = windowID;
            _window = new Windows.Window(windowID);
        }
        #endregion

        //-------------------------------------------------
        #region ID, Parent, Handle
        public int ID { get; set; }

        public IntPtr Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                WinAPI.SetParent(this.Handle, value);
                this._parent = value;
            }
        }

        public IntPtr Handle
        {
            get
            {
                return _window.Handle;
            }
        }
        #endregion

        //public Windows.Window Window
        //{
        //    get { return _window; }
        //}

        //-------------------------------------------------
        #region Title/Type
        public string Title
        {
            get
            {
                return _window.Title;
            }
        }

        public Windows.Window.WindowType Type
        {
            get
            {
                return _window.Type;
            }
        }
        #endregion

        //-------------------------------------------------
        #region Size, Width and Height
        public int Width
        {
            get
            {
                RECT rec = new RECT();
                WinAPI.GetWindowRect(this.Handle, out rec);
                return (rec.Right - rec.Left);
            }
        }

        public int Height
        {
            get
            {
                RECT rec = new RECT();
                WinAPI.GetWindowRect(this.Handle, out rec);
                return (rec.Bottom - rec.Top);
            }
        }

        public void UpdateSize(int width, int height)
        {
            WinAPI.MoveWindow(this.Handle, 0, 0, width, height, false);
        }

        #endregion

        //-------------------------------------------------
        #region IsOpen/Open/Close/Update
        public bool IsOpen
        {
            get
            {
                return (this.Handle != IntPtr.Zero);
            }
        }

        public void Open()
        {
            _window.Open();
        }

        public void Close()
        {
            _window.Close();
        }

        public void Update()
        {
            _window.Update();
        }
        #endregion

        //-------------------------------------------------
        #region Max/Restore
        public void Maximize()
        {
            _window.Maximize();
        }

        public void Restore()
        {
            _window.Restore();
        }
        #endregion

        //-------------------------------------------------
        #region Caption/Border
        public void RemoveCaptionBar()
        {
            int style = WinAPI.GetWindowLong(this.Handle, WinAPI.GWL_STYLE);
            WinAPI.SetWindowLong(this.Handle, WinAPI.GWL_STYLE, (style & ~WinAPI.WS_CAPTION));
        }

        public void AddCaptionBar()
        {
            int style = WinAPI.GetWindowLong(this.Handle, WinAPI.GWL_STYLE);
            WinAPI.SetWindowLong(this.Handle, WinAPI.GWL_STYLE, (style & WinAPI.WS_CAPTION));
        }

        public void RemoveBorder()
        {
            int style = WinAPI.GetWindowLong(this.Handle, WinAPI.GWL_STYLE);
            WinAPI.SetWindowLong(this.Handle, WinAPI.GWL_STYLE, (style & ~WinAPI.WS_THICKFRAME));
        }

        public void AddBorder()
        {
            int style = WinAPI.GetWindowLong(this.Handle, WinAPI.GWL_STYLE);
            WinAPI.SetWindowLong(this.Handle, WinAPI.GWL_STYLE, (style & WinAPI.WS_THICKFRAME));
        }
        #endregion
    }

    //-------------------------------------------------
    #region WinAPI imports
    public static class WinAPI
    {
        // Sets the parent of a window.
        [DllImport("User32", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndParent);

        //Sets window attributes
        [DllImport("USER32.DLL")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //Gets window attributes
        [DllImport("USER32.DLL")]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        internal static extern
            IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        internal static extern
            IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, int fAttach);

        //assorted constants needed
        public static int GWL_STYLE = -16;
        public static int WS_CHILD = 0x40000000; //child window
        public static int WS_BORDER = 0x00800000; //window with border
        public static int WS_DLGFRAME = 0x00400000; //window with double border but no title
        public static int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar
        public static int WS_MAXIMIZE = 0x1000000;
        public static int WS_THICKFRAME = 0x40000;
    }
    #endregion

    //-------------------------------------------------
    #region Struct Rect
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    #endregion
}
