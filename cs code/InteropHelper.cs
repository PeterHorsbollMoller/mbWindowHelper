using System;
using System.Windows.Forms;
using MapInfo.MiPro.Interop;
using WindowHelper.Geometry;

namespace WindowHelper
{
	static class InteropHelper
	{
        //private static System.Globalization.NumberFormatInfo _usNumberFormat;

        #region [Do and EVAL]

        /// <summary>
        /// Sends a statement to MapInfo to be executed
        /// </summary>
        /// <param name="statement">statement to send to MapInfo</param>
        public static void Do(string statement)
        {
            //InteropServices.MapInfoApplication.Do(string.Format("Print \"{0}\"", statement));
            InteropServices.MapInfoApplication.Do(statement);
        }

        /// <summary>
        /// Sends a statement to MapInfo to be evaluated
        /// </summary>
        /// <param name="statement">statement to send to MapInfo</param>
        /// <returns>result of evaluation as string</returns>
        public static string Eval(string statement)
        {
            //InteropServices.MapInfoApplication.Do(string.Format("Print \"{0}\"", statement));
            return InteropServices.MapInfoApplication.Eval(statement);
        }

        #endregion

		#region [GET APP VERSION]

		private const int SYS_INFO_APPVERSION = 2; // Used with SystemInfo to get appversion

		/// <summary>
		/// Gets the MapInfo Professional version number
		/// </summary>
		/// <returns>Version number (multiplied by 100) as string</returns>
		public static string GetAppVersion()
		{
			string expr = string.Format("SystemInfo({0})", SYS_INFO_APPVERSION);
			return InteropServices.MapInfoApplication.Eval(expr);
		}

		#endregion

		#region [GET FRONT WINDOW]

		/// <summary>
		/// Get front window (child window) from the running instance of MapInfo Professional
		/// </summary>
		/// <returns>Id of the front window</returns>
		public static int GetFrontWindow()
		{
			string evalResult = InteropServices.MapInfoApplication.Eval("FrontWindow()");
			return Int32.Parse(evalResult);
		}

		#endregion

		#region [GET WINDOW INFORMATION]

        private const int WIN_INFO_NAME = 1;            // Used with WindowInfo to get win name
        private const int WIN_INFO_TYPE = 3;            // Used with WindowInfo to get win type
        private const int WIN_INFO_WIDTH = 4;           // Used with WindowInfo to get win width
        private const int WIN_INFO_HEIGHT = 5;          // Used with WindowInfo to get win height
        private const int WIN_INFO_X = 6;               // Used with WindowInfo to get win position X
        private const int WIN_INFO_Y = 7;               // Used with WindowInfo to get win positon Y
        private const int WIN_INFO_TABLE = 10;          // Used with WindowInfo to get the table of the window
        private const int WIN_INFO_WORKSPACE = 14;      // the string of MapBasic statements that a Save Workspace operation would write to a workspace to record the settings for this map. Differs from WIN_INFO_CLONEWINDOW (15) in that the results include Open Table statement, etc
        private const int WIN_INFO_CLONEWINDOW = 15;    // a string of MapBasic statements that can be used in a Run Command statement to duplicate a window. 
        private const int WIN_INFO_SYSMENUCLOSE = 16;   // Used with WindowInfo indicates if a Set Window statement has disabled the Close command on the window's system menu. 

        /// <summary>
        /// Returns window name for given window id.
        /// </summary>
        /// <param name="windowId"></param>
        /// <returns>Window name</returns>
        public static string GetWindowName(int windowId)
        {
            string expr = string.Format("WindowInfo({0}, {1})", windowId, WIN_INFO_NAME);
            string evalResult = InteropServices.MapInfoApplication.Eval(expr);
            return evalResult;
        }
        
        /// <summary>
		/// Returns window type for given window id.
		/// </summary>
		/// <param name="windowId"></param>
		/// <returns>Window type</returns>
        public static int GetWindowType(int windowId)
		{
			string expr = string.Format("WindowInfo({0}, {1})", windowId, WIN_INFO_TYPE);
			string evalResult = InteropServices.MapInfoApplication.Eval(expr);
			return Int32.Parse(evalResult);
		}

        public static Double GetWindowHeight(int windowId)
        {
            string expr = string.Format("WindowInfo({0}, {1})", windowId, WIN_INFO_HEIGHT);
            string evalResult = InteropServices.MapInfoApplication.Eval(expr);
            return Double.Parse(evalResult, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static Double GetWindowWidth(int windowId)
        {
            string expr = string.Format("WindowInfo({0}, {1})", windowId, WIN_INFO_WIDTH);
            string evalResult = InteropServices.MapInfoApplication.Eval(expr);
            return Double.Parse(evalResult, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static bool GetWindowSystemMenuClose(int windowId)
        {
            string expr = string.Format("WindowInfo({0}, {1})", windowId, WIN_INFO_SYSMENUCLOSE);
            string evalResult = InteropServices.MapInfoApplication.Eval(expr);
            if (evalResult == "T")
                return true;
            else
                return false;
        }

        public static string GetWindowCloneStatement(int windowId)
        {
            string expr = string.Format("WindowInfo({0}, {1})", windowId, WIN_INFO_CLONEWINDOW);
            string evalResult = InteropServices.MapInfoApplication.Eval(expr);
            return evalResult;
        }

        public static string GetWindowWorkspaceStatement(int windowId)
        {
            string expr = string.Format("WindowInfo({0}, {1})", windowId, WIN_INFO_WORKSPACE);
            string evalResult = InteropServices.MapInfoApplication.Eval(expr);
            return evalResult;
        }

		#endregion

		#region [GET MAPPER INFORMATION]

		private const int MAPPER_INFO_ZOOM = 1;    // Used with MapperInfo to get zoom
		private const int MAPPER_INFO_CENTERX = 3; // Used with MapperInfo to get center X
		private const int MAPPER_INFO_CENTERY = 4; // Used with MapperInfo to get center Y
		private const int MAPPER_INFO_DISTUNITS = 12; // Used with MapperInfo to get distance units e.g. "mi" 
		private const int MAPPER_INFO_COORDSYS_CLAUSE_WITH_BOUNDS = 22; // Used with MapperInfo to get a CoordSys string

		/// <summary>
		/// Gets the view information from a mapper window in
		/// MapInfo Professional application
		/// </summary>
		/// <remarks>
        /// MapBasic's MapperInfo function can return numeric information
        /// such as Zoom width.  However, MapInfoApplication.Eval returns 
        /// results as strings, so if you request numeric information such
        /// as MAPPER_INFO_ZOOM, Eval will return a string such as "1234.5"
        /// (with a period as the decimal separator, regardless of 
        /// the user's regional settings).  
        /// 
        /// Instead of parsing such String results into Double values, we 
        /// will return the String results.  The string representation
        /// of numeric values is ideal for this application, because the 
        /// string formatting returned by the Eval method (i.e. always using 
        /// the period as the decimal separator) is appropriate for use  
		/// in the Set Map statement we will be constructing later on. 
		/// </remarks>
		/// <param name="windowId">identification number of mapper window</param>
		/// <param name="infoType">The type of information</param>
		/// <returns>The requested information</returns>
		private static string GetMapperInfo(int windowId, int infoType)
		{
			string expr, evalResult;

			expr = string.Format("MapperInfo({0}, {1})", windowId, infoType);
			evalResult = InteropServices.MapInfoApplication.Eval(expr);
			return evalResult;
		}

		/// <summary>
		/// Get a string representing the coordinate system of the map window
		/// </summary>
		/// <param name="windowId">identification number of mapper window</param>
		/// <returns>a CoordSys clause string</returns>
		public static string GetMapperCoordSys(int windowId)
		{
			string expr;

			expr = string.Format("MapperInfo({0}, {1})", windowId, MAPPER_INFO_COORDSYS_CLAUSE_WITH_BOUNDS);
			return InteropServices.MapInfoApplication.Eval(expr);
		}

		/// <summary>
		/// Get a string representing the distance unit in use in a specific map window
		/// </summary>
		/// <param name="windowId">identification number of mapper window</param>
		/// <returns></returns>
		public static string GetMapperDistanceUnit(int windowId)
		{
			string expr;

			expr = string.Format("MapperInfo({0}, {1})", windowId, MAPPER_INFO_DISTUNITS);
			return InteropServices.MapInfoApplication.Eval(expr);
		}

		/// <summary>
		/// Gets mapper window zoom value
		/// </summary>
		/// <param name="windowId">identification number of mapper window</param>
		/// <returns>Zoom value of mapper window's current view</returns>
		public static string GetMapperZoom(int windowId)
		{
			return GetMapperInfo(windowId, MAPPER_INFO_ZOOM);
		}
        //public static double GetMapperZoom(int windowId)
        //{
        //    string evalResult = GetMapperInfo(windowId, MAPPER_INFO_ZOOM); 
        //    return Double.Parse(evalResult, System.Globalization.CultureInfo.InvariantCulture);
        //}

		/// <summary>
		/// Gets mapper window center X value
		/// </summary>
		/// <param name="windowId">window identification number of mapper window</param>
		/// <returns>Center Y of mapper window's current view</returns>
		public static string GetMapperCenterX(int windowId)
		{
			return GetMapperInfo(windowId, MAPPER_INFO_CENTERX);
		}
        //public static double GetMapperCenterX(int windowId)
        //{
        //    string evalResult = GetMapperInfo(windowId, MAPPER_INFO_CENTERX);
        //    return Double.Parse(evalResult, System.Globalization.CultureInfo.InvariantCulture);
        //}

		/// <summary>
		/// Gets mapper window center Y value
		/// </summary>
		/// <param name="windowId">window identification number of mapper window</param>
		/// <returns>Center X of mapper window's current view</returns>
		public static string GetMapperCenterY(int windowId)
		{
			return GetMapperInfo(windowId, MAPPER_INFO_CENTERY);
        }
        //public static double GetMapperCenterY(int windowId)
        //{
        //    string evalResult = GetMapperInfo(windowId, MAPPER_INFO_CENTERY);
        //    return Double.Parse(evalResult, System.Globalization.CultureInfo.InvariantCulture);
        //}

        #endregion

        #region [GET GEOGRAPHY INFORMATION]

        private const int GEOGRAPHY_INFO_MINX = 1;    // Used with GeographyInfo to get minimum X
        private const int GEOGRAPHY_INFO_MINY = 2;    // Used with GeographyInfo to get minimum Y
        private const int GEOGRAPHY_INFO_MAXX = 3;    // Used with GeographyInfo to get maximum X
        private const int GEOGRAPHY_INFO_MAXY = 4;    // Used with GeographyInfo to get maximum Y

        /// <summary>
        /// Gets the geographical information from a geographical object
        /// MapInfo Professional application
        /// </summary>
        /// <remarks>
        /// MapBasic's MapperInfo function can return numeric information
        /// such as Zoom width.  However, MapInfoApplication.Eval returns 
        /// results as strings, so if you request numeric information such
        /// as MAPPER_INFO_ZOOM, Eval will return a string such as "1234.5"
        /// (with a period as the decimal separator, regardless of 
        /// the user's regional settings).  
        /// 
        /// </remarks>
        /// <param name="windowId">identification number of mapper window</param>
        /// <param name="infoType">The type of information</param>
        /// <returns>The requested information</returns>

        /// <summary>

        #endregion

        #region [HANDLERS]
        public static void EnableHandler(string handlerName)
        {
            InteropServices.MapInfoApplication.Do(string.Format("Set Handler {0} On", handlerName));
        }

        public static void DisableHandler(string handlerName)
        {
            InteropServices.MapInfoApplication.Do(string.Format("Set Handler {0} Off", handlerName));
        }

        #endregion [HANDLERS]

        #region [SESSION HANDLING]

        /// <summary>
		/// Gets a string representing MapInfo's current distance units, such as mi or km. 
		/// Defaults to "mi" but can be reset through the Set Distance Units statement. 
		/// </summary>
		/// <returns>A unit string such as mi or km</returns>
		public static string GetSessionDistanceUnit()
		{
			// Use SessionInfo(SESSION_INFO_DISTANCE_UNITS) to get the unit string
			return InteropServices.MapInfoApplication.Eval("SessionInfo(2)");
		}

		/// <summary>
		/// Sets MapInfo's current distance unit, such as mi or km.   Has the same effect
		/// as typing a Set Distance Units statement into the MapBasic window. 
		/// </summary>
		/// <param name="unit">a distance unit string, such as mi or km</param>
		public static void SetSessionDistanceUnit(string unit)
		{
			string expr;

			expr = string.Format("Set Distance Units \"{0}\"", unit);
			InteropServices.MapInfoApplication.Do(expr);
		}

        /// <summary>
        /// Gets a string representing MapInfo's current paper units, such as mi or km. 
        /// Defaults to "in" but can be reset through the Set Distance Units statement. 
        /// </summary>
        /// <returns>A unit string such as mi or km</returns>
        public static string GetSessionPaperUnit()
        {
            // Use SessionInfo(SESSION_INFO_DISTANCE_UNITS) to get the unit string
            return InteropServices.MapInfoApplication.Eval("SessionInfo(4)");
        }

        /// <summary>
        /// Sets MapInfo's current paper unit, such as in or cm.   Has the same effect
        /// as typing a Set Paper Units statement into the MapBasic window. 
        /// </summary>
        /// <param name="unit">a distance unit string, such as in or cm</param>
        public static void SetSessionPaperUnit(string unit)
        {
            string expr;

            expr = string.Format("Set Paper Units \"{0}\"", unit);
            InteropServices.MapInfoApplication.Do(expr);
        }

		/// <summary>
		/// Get a string representing the CoordSys clause of the coordinate system 
		/// that is currently in effect. 
		/// </summary>
		/// <returns>string such as "CoordSys Earth" </returns>
		public static string GetSessionCoordSys()
		{
			// Make note of the current MapBasic Coordinate System SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
			return InteropServices.MapInfoApplication.Eval("SessionInfo(1)"); 
		}

		/// <summary>
		/// Set the current coordinate system.  Has the same effect as typing 
		/// a Set CoordSys statement into the MapBasic window. 
		/// </summary>
		/// <param name="csys">string such as "CoordSys Earth"</param>
		public static void SetSessionCoordSys(string csys)
		{
			InteropServices.MapInfoApplication.Do(string.Format("Set {0}", csys));
        }

        #endregion

        #region [GET FORMATTED STRING]

        /// <summary>
        /// Given a string representation of a number, in invariant formatting 
        /// (always using the period as the decimal separator), return a  
        /// string formatted according to the user's current system settings. 
		/// </summary>
		/// <remarks>
        /// The resulting number string is appropriate for displaying numbers 
        /// in the user interface, but not appropriate for constructing MapBasic 
        /// statements.  When you construct a MapBasic statement string (to 
        /// be executed through a call to the Do method), any numeric literals
        /// in the string must use period as the decimal separator, even if 
        /// the user's system's regional settings use some other character 
        /// as the decimal separator. 
		/// </remarks>
		/// <param name="numericString">A number string with period (.) as the decimal separator, if any</param>
		/// <returns>A number string with a decimal separator based on the user's system settings</returns>
		public static string GetFormattedString(string numericString)
		{
			return InteropServices.MapInfoApplication.Eval(string.Format("FormatNumber$({0})", numericString)); 
		}

        public static string GetDeformattedString(string numericString)
        {
            string expr;
            expr = string.Format("DeformatNumber$(\"{0}\")", numericString);
            //MessageBox.Show(expr);
            return InteropServices.MapInfoApplication.Eval(expr);
        }

		#endregion

		#region [SET CURRENT VIEW OF MAPPER WINDOW]

		/// <summary>
		/// Sets the current view of mapper window represented by windowId
		/// </summary>
		/// <param name="windowId">Window identification number of mapper window</param>
		/// <param name="centerX">New center X of the mapper window</param>
		/// <param name="centerY">New center Y of the mapper window</param>
		/// <param name="mapperZoom">New zoom of the mapper window</param>
		/// <param name="unit">Distance unit string that applies to mapperZoom, such as mi or km</param>
		/// <param name="csys">CoordSys string that specifies the coordinate system used by the X/Y arguments</param>
		public static void SetView(int windowId, string centerX, string centerY, string mapperZoom, string unit, string csys)
		{
            // Before we do any work involving the map's X/Y coordinates, we
            // will set the current coordinate system; that way, we will guarantee 
            // that the coordinates will be processed correctly, regardless of which
            // coordinate system is in use in the current map. 
            // But, before we set the coordinate system, make note of the current 
            // coordinate system, so that we can restore it later.  This way, in
            // the unlikely event that the user typed a Set CoordSys statement into 
            // the MapBasic window, we will preserve the coordsys typed in by the user. 

			// Make note of the current MapBasic Coordinate System, equivalent
			// to calling:  SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
			string oldCoordSys = GetSessionCoordSys();  

			// Set the coordsys clause to the csys that was saved with the named view 
			SetSessionCoordSys(csys);  

			// Set the map view 
			string setMapStatement = string.Format(
				"Set Map Window {0} Center ( {1}, {2} ) Zoom {3} Units \"{4}\"", 
					windowId, centerX, centerY, mapperZoom, unit);

            //MessageBox.Show(setMapStatement);

			InteropServices.MapInfoApplication.Do(setMapStatement); 

			// Restore the MapBasic Coordinate System to its previous state 
			SetSessionCoordSys(oldCoordSys); 
		}

        /// <summary>
        /// Sets the current view of mapper window represented by windowId
        /// </summary>
        /// <param name="windowId">Window identification number of mapper window</param>
        /// <param name="centerX">New center X of the mapper window</param>
        /// <param name="centerY">New center Y of the mapper window</param>
        /// <param name="csys">CoordSys string that specifies the coordinate system used by the X/Y arguments</param>
        public static void SetView(int windowId, string centerX, string centerY, string csys)
        {
            // Before we do any work involving the map's X/Y coordinates, we
            // will set the current coordinate system; that way, we will guarantee 
            // that the coordinates will be processed correctly, regardless of which
            // coordinate system is in use in the current map. 
            // But, before we set the coordinate system, make note of the current 
            // coordinate system, so that we can restore it later.  This way, in
            // the unlikely event that the user typed a Set CoordSys statement into 
            // the MapBasic window, we will preserve the coordsys typed in by the user. 

            // Make note of the current MapBasic Coordinate System, equivalent
            // to calling:  SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
            string oldCoordSys = GetSessionCoordSys();

            // Set the coordsys clause to the csys that was saved with the named view 
            SetSessionCoordSys(csys);

            // Set the map view 
            string setMapStatement = string.Format(
                "Set Map Window {0} Center ( {1}, {2} )",
                    windowId, centerX, centerY);

            //MessageBox.Show(setMapStatement);

            InteropServices.MapInfoApplication.Do(setMapStatement);

            // Restore the MapBasic Coordinate System to its previous state 
            SetSessionCoordSys(oldCoordSys);
        }

        /// <summary>
        /// Sets the current view of mapper window represented by windowId
        /// </summary>
        /// <param name="windowId">Window identification number of mapper window</param>
        /// <param name="mapperZoom">New zoom of the mapper window</param>
        /// <param name="unit">Distance unit string that applies to mapperZoom, such as mi or km</param>
        public static void SetView(int windowId, string mapperZoom, string unit)
        {
            // Set the map view 
            string setMapStatement = string.Format(
                "Set Map Window {0} Zoom {3} Units \"{4}\"",
                    windowId, mapperZoom, unit);

            InteropServices.MapInfoApplication.Do(setMapStatement);
        }


        public static void SetView(int windowId, double centerX, double centerY, double mapperZoom, string unit, string csys)
        {
            SetView(windowId
                    , GetDeformattedString(Convert.ToString(centerX))
                    , GetDeformattedString(Convert.ToString(centerY))
                    , GetDeformattedString(Convert.ToString(mapperZoom))
                    , unit
                    , csys
                    );
        }
        /// <summary>
        /// Sets the current view of mapper window represented by windowId using a MapInfo MBR
        /// </summary>
        /// <param name="windowId">Window identification number of mapper window</param>
        /// <param name="mipMBR">MBR to fit Map to</param>
        /// <param name="csys">CoordSys string that specifies the coordinate system used by the mipMBR arguments</param>
        public static void SetViewMBR(int windowId, MBR mipMBR, string unit, string csys)
        {
            double xMin, yMin, xMax, yMax;
            double winHeight, winWidth;
            double mapZoom;

            xMin = mipMBR.MinimumX;
            yMin = mipMBR.MinimumY;
            xMax = mipMBR.MaximumX;
            yMax = mipMBR.MaximumY;

            //MessageBox.Show (String.Format("MBR: {0}", mipMBR.ToString()));
            //MessageBox.Show (String.Format("Center: {0}", mipMBR.GetCenter()));

            winHeight = GetWindowHeight(windowId);
            winWidth = GetWindowWidth(windowId);

            if (((xMax - xMin)/winWidth) > ((yMax-yMin)/winHeight))
                {
                    mapZoom = xMax - xMin;
                }
            else
                {
                    mapZoom = ((winWidth + 10)/winHeight) * (yMax - yMin);
                }

            //MessageBox.Show(String.Format("Zoom: {0} {1}", Convert.ToString(mapZoom), unit));

            SetView(windowId, mipMBR.GetCenterX(), mipMBR.GetCenterY(), mapZoom, unit, csys);
        }

        /// <summary>
        /// Sets the current view of mapper window represented by windowId to match a table
        /// </summary>
        /// <param name="windowId">Window identification number of mapper window</param>
        /// <param name="table">Table to zoom to</param>
        public static void SetViewTable(int windowId, string table)
        {
            // Add the table to the map
            string cmd = string.Format(
                "Set Map Window {0} Zoom Entire Layer {1}",
                    windowId, table);

            //MessageBox.Show(cmd);

            InteropServices.MapInfoApplication.Do(cmd);
        }

        #endregion

        #region [ADDING TABLES/LAYERS TO A MAP]

        /// <summary>
        /// Adds a table to a existing map
        /// </summary>
        /// <param name="windowId">Window identification number of mapper window</param>
        /// <param name="table">Title of table to add</param>
        public static void AddTableToMap(int windowId, string table)
        {
            // Add the table to the map
            string cmd = string.Format(
                "Add Map Window {0} Layer {1}",
                    windowId, table);

            //MessageBox.Show(cmd);

            InteropServices.MapInfoApplication.Do(cmd);
        }
		#endregion

		#region [GET MAPPER WINDOW IDENTIFICATION NUMBER]

		/// <summary>
		/// Get the ID of the front window.  Displays a message
		/// and returns 0 if there is no window open, or the 
		/// front window is not a mapper
		/// </summary>
		/// <returns></returns>
		public static int GetMapWindowId()
		{
			int windowId = GetFrontWindow();
			// Make sure we have a window
			if (windowId == 0)
			{
				MessageBox.Show(WindowHelper.Properties.Resources.ERR_NO_WIN_OPEN);
                return 0;
			}

			int windowType = GetWindowType(windowId); ;
			// Make sure if front window is a mapper window
			if (windowType != 1)
			{
                MessageBox.Show(WindowHelper.Properties.Resources.ERR_FRONT_WIN_NOT_MAPPER);
				return 0;
			}

			return windowId;
		}

		#endregion

        #region [WRITE TO MAPINFO MESSAGE WINDOW]
        /// <summary>
        /// Prints a message to the MapInfo Message window
        /// </summary>
        /// <returns></returns>
        public static void PrintMessage(string sText)
        {
            string sCmd = string.Format("Print \"{0}\"", sText);
            InteropServices.MapInfoApplication.Do(sCmd);
        }

        #endregion

        #region [EXECUTE SQL STATEMENT]

        public static Int32 RunSQLStatement(string sCmd, string sIntoTable)
        {
            InteropServices.MapInfoApplication.Do(sCmd);
            return GetTableNumRows(sIntoTable);
        }

        #endregion

        #region [GET TABLE INFORMATION]

        private const int TAB_INFO_NAME = 1;    
        private const int TAB_INFO_NUM = 2;
        private const int TAB_INFO_TYPE = 3;
        private const int TAB_INFO_NCOLS = 4;
        private const int TAB_INFO_MAPPABLE = 5;
        private const int TAB_INFO_READONLY = 6;
        private const int TAB_INFO_TEMP = 7;
        private const int TAB_INFO_NROWS = 8;
        private const int TAB_INFO_EDITED = 9;
        private const int TAB_INFO_FASTEDIT = 10;
        private const int TAB_INFO_UNDO = 11;
        private const int TAB_INFO_MAPPABLE_TABLE = 12;
        private const int TAB_INFO_USERMAP = 13;
        private const int TAB_INFO_USERBROWSE = 14;
        private const int TAB_INFO_USERCLOSE = 15;
        private const int TAB_INFO_USEREDITABLE = 16;
        private const int TAB_INFO_USERREMOVEMAP = 17;
        private const int TAB_INFO_USERDISPLAYMAP = 18;
        private const int TAB_INFO_TABFILE = 19;
        private const int TAB_INFO_MINX = 20;
        private const int TAB_INFO_MINY = 21;
        private const int TAB_INFO_MAXX = 22;
        private const int TAB_INFO_MAXY = 23;
        private const int TAB_INFO_SEAMLESS = 24;
        private const int TAB_INFO_COORDSYS_MINX = 25;
        private const int TAB_INFO_COORDSYS_MINY = 26;
        private const int TAB_INFO_COORDSYS_MAXX = 27;
        private const int TAB_INFO_COORDSYS_MAXY = 28;
        private const int TAB_INFO_COORDSYS_CLAUSE = 29;
        private const int TAB_INFO_COORDSYS_NAME = 30;
        private const int TAB_INFO_NREFS = 31;
        private const int TAB_INFO_SUPPORT_MZ = 32;
        private const int TAB_INFO_Z_UNIT_SET = 33;
        private const int TAB_INFO_Z_UNIT = 34;
        private const int TAB_INFO_BROWSER_LIST = 35;
        private const int TAB_INFO_THEME_METADATA = 36;

        /// <summary>
        /// Gets the view information from a mapper window in
        /// MapInfo Professional application
        /// </summary>
        /// <remarks>
        /// MapBasic's TableInfo function can return numeric information
        /// such as Minimun X coordinate.  However, MapInfoApplication.Eval returns 
        /// results as strings, so if you request numeric information such
        /// as TAB_INFO_MINX, Eval will return a string such as "123456.789"
        /// (with a period as the decimal separator, regardless of 
        /// the user's regional settings).  
        /// 
        /// Instead of parsing such String results into Double values, we 
        /// will return the String results.  
        /// </remarks>

        /// <param name="tableId">identification number of table</param>
        /// <param name="infoType">The type of information</param>
        /// <returns>The requested information</returns>
        private static string GetTableInfo(int tableId, int infoType)
        {
            string expr, evalResult;

            expr = string.Format("TableInfo({0}, {1})", tableId, infoType);
            evalResult = InteropServices.MapInfoApplication.Eval(expr);
            return evalResult;
        }

        /// <param name="tableName">identification name of table</param>
        /// <param name="infoType">The type of information</param>
        /// <returns>The requested information</returns>
        private static string GetTableInfo(string tableName, int infoType)
        {
            string expr, evalResult;

            expr = string.Format("TableInfo({0}, {1})", tableName, infoType);
            evalResult = InteropServices.MapInfoApplication.Eval(expr);
            return evalResult;
        }

        /// <summary>
        /// Get a number representing the number of records in a given table
        /// </summary>
        /// <param name="tableId">identification number of table</param>
        /// <returns>the numbe rof rows in the table</returns>
        public static Int32 GetTableNumRows(int tableId)
        {
            string evalResult;

            evalResult = GetTableInfo(tableId, TAB_INFO_NROWS);
            return System.Convert.ToInt32(evalResult);
        }

        /// <summary>
        /// Get a string representing the coordsys of a given table
        /// </summary>
        /// <param name="tableId">identification number of table</param>
        /// <returns>coordsys of the table</returns>
        public static string GetTableCoordsys(int tableId)
        {
            string evalResult;

            evalResult = GetTableInfo(tableId, TAB_INFO_COORDSYS_CLAUSE);
            return evalResult;
        }

        /// <summary>
        /// Get a string representing the coordsys of a given table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        /// <returns>coordsys of the table</returns>
        public static string GetTableCoordsys(string tableName)
        {
            string evalResult;

            evalResult = GetTableInfo(tableName, TAB_INFO_COORDSYS_CLAUSE);
            return evalResult;
        }

        /// <summary>
        /// Get a number representing the number of records in a given table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        /// <returns>the numbe rof rows in the table</returns>
        public static Int32 GetTableNumRows(string tableName)
        {
            string evalResult;

            evalResult = GetTableInfo(tableName, TAB_INFO_NROWS);
            return System.Convert.ToInt32(evalResult);
        }

            /// <summary>
        /// Get a number representing the number of records in a given table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        /// <returns>the numbe rof rows in the table</returns>
        public static int GetNumberOfOpenTables()
        {
            string evalResult, expr;

            expr = string.Format("NumTables()");
            evalResult = InteropServices.MapInfoApplication.Eval(expr);
            return System.Convert.ToInt32(evalResult);
        }

        /// <summary>
        /// Get a list of open tables
        /// </summary>
        /// <returns>an array with the names of the open tables</returns>
        public static string[] GetOpenTablesList()
        {
            string[] tables;
            int numTables;

            numTables = GetNumberOfOpenTables();
            tables = new string[numTables];

            for (int i = 1; i <= numTables; i++)
            {
                tables[i - 1] = GetTableInfo(i, TAB_INFO_NAME);
            }
            return tables;
        }

        #endregion

        #region [LOOPING MAPINFO TABLES]

         /// <summary>
        /// Fetching first record from a given table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void FetchFirstFromMITable(string tableName)
        {
            string expr;

            expr = string.Format("Fetch First From {0}", tableName);
            InteropServices.MapInfoApplication.Do(expr);
        }

        /// <summary>
        /// Fetching last record from a given table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void FetchLastFromMITable(string tableName)
        {
            string expr;

            expr = string.Format("Fetch Last From {0}", tableName);
            InteropServices.MapInfoApplication.Do(expr);
        }

        /// <summary>
        /// Fetching next record from a given table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void FetchNextFromMITable(string tableName)
        {
            string expr;

            expr = string.Format("Fetch Next From {0}", tableName);
            InteropServices.MapInfoApplication.Do(expr);
        }

        /// <summary>
        /// Fetching previous record from a given table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void FetchPrevFromMITable(string tableName)
        {
            string expr;

            expr = string.Format("Fetch Prev From {0}", tableName);
            InteropServices.MapInfoApplication.Do(expr);
        }

        /// <summary>
        /// Checks whether the cursor is at end of table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        /// <returns>returns true if cursor is at End Of Table</returns>
        public static bool EndOfMITable(string tableName)
        {
            string expr, evalResult;

            expr = string.Format("EOT({0})", tableName);
            evalResult = InteropServices.MapInfoApplication.Eval(expr);
            return (evalResult == "T");
        }

        #endregion

        #region [READING VALUES FROM MAPINFO TABLES]
        /// <summary>
        /// Reading a value from a column for the current record in a table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        /// <param name="columnName">identification name of column</param>
        /// <returns>returns value read from column in table as string</returns>
        public static string ReadValueFromMITableCurrentRecord(string tableName, string columnName)
        {
            string expr, evalResult;

            expr = string.Format("{0}.{1}", tableName, columnName);
            evalResult = InteropServices.MapInfoApplication.Eval(expr);
            return evalResult;
        }

        /// <summary>
        /// Reading a MBR from the object for the current record in a table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        /// <returns>returns MBR read from current record in table as MapInfoMBR</returns>
        public static MBR ReadMBRFromMITableCurrentRecord(string tableName)
        {
            string expr, evalResult;
            MBR miMBR = new MBR(-10.0, -10.0, -10.0, -10.0);

            //System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
            //_usNumberFormat = cultureInfo.NumberFormat;

            // Make note of the current MapBasic Coordinate System, equivalent
            // to calling:  SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
            string oldCoordSys = GetSessionCoordSys();
            //double coord;

            expr = string.Format("Set CoordSys Table {0}", tableName);
            InteropServices.MapInfoApplication.Do(expr);

            expr = string.Format("ObjectGeography({0}.OBJ, 1)", tableName);
            evalResult = InteropServices.MapInfoApplication.Eval(expr);
            //MessageBox.Show(String.Format("{0} => {1}", string.Format("ObjectGeography({0}.OBJ, 1)", tableName), evalResult));

            //if (double.TryParse(evalResult, System.Globalization.NumberStyles.Number, _usNumberFormat, out coord))
            //    miMBR.MinimumX = coord;
            //else
            //    miMBR.MinimumX = 0;
            miMBR.MinimumX = Double.Parse(evalResult, System.Globalization.CultureInfo.InvariantCulture);

            expr = string.Format("ObjectGeography({0}.OBJ, 2)", tableName);
            evalResult = InteropServices.MapInfoApplication.Eval(expr);
            //if (double.TryParse(evalResult, System.Globalization.NumberStyles.Number, _usNumberFormat, out coord))
            //    miMBR.MinimumY = coord;
            //else
            //    miMBR.MinimumY = 0;
            miMBR.MinimumY = Double.Parse(evalResult, System.Globalization.CultureInfo.InvariantCulture);

            expr = string.Format("ObjectGeography({0}.OBJ, 3)", tableName);
            evalResult = InteropServices.MapInfoApplication.Eval(expr);
            //if (double.TryParse(evalResult, System.Globalization.NumberStyles.Number, _usNumberFormat, out coord))
            //    miMBR.MaximumX = coord;
            //else
            //    miMBR.MaximumX = 0;
            miMBR.MaximumX = Double.Parse(evalResult, System.Globalization.CultureInfo.InvariantCulture);

            expr = string.Format("ObjectGeography({0}.OBJ, 4)", tableName);
            evalResult = InteropServices.MapInfoApplication.Eval(expr);
            //if (double.TryParse(evalResult, System.Globalization.NumberStyles.Number, _usNumberFormat, out coord))
            //    miMBR.MaximumY = coord;
            //else
            //    miMBR.MaximumY = 0;
            miMBR.MaximumY = Double.Parse(evalResult, System.Globalization.CultureInfo.InvariantCulture);

            // Restore the MapBasic Coordinate System to its previous state 
            SetSessionCoordSys(oldCoordSys); 

            return miMBR;
        }

        #endregion

        #region [HANDLING TABLES - CLOSING]

        /// <summary>
        /// Closes a table open in MapInfo
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void CloseMITable(string tableName)
        {
            string expr;

            expr = string.Format("Close Table {0}", tableName);
            InteropServices.MapInfoApplication.Do(expr);
        }

        #endregion

        #region [CREATING TABLES]

        /// <summary>
        /// Create a dummy table with only an ID column
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void CreateDummyTableWithID(string tableName)
        {
            string expr;

            expr = string.Format("Create Table {0} (ID Integer) File TempFileName$(\"\")", tableName);
            InteropServices.MapInfoApplication.Do(expr);
        }

        /// <summary>
        /// Create a dummy table with only an ID column
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void MakeTableMappable(string tableName, string csys)
        {
            string expr;

            expr = string.Format("Create Map For {0} {1}", tableName, csys);
            PrintMessage(expr);
            InteropServices.MapInfoApplication.Do(expr);
        }

        #endregion

        #region [HANDLING COORDINATE SYSTEMS - SETTING AND GETTING]

        /// <summary>
        /// Sets the current coordinatsystem using a table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void SetCoordSysUsingTable(string tableName)
        {
            string expr;

            expr = string.Format("Set CoordSys Table {0}", tableName);
            InteropServices.MapInfoApplication.Do(expr);
        }

        /// <summary>
        /// Sets the current coordinatsystem using a mapper
        /// </summary>
        /// <param name="windowID">identification ID of a Window</param>
        public static void SetCoordSysUsingWindow(Int32 windowID)
        {
            string expr;

            expr = string.Format("Set CoordSys Window {0}", Convert.ToString(windowID));
            InteropServices.MapInfoApplication.Do(expr);
        }

        /// <summary>
        /// Gets the coordinatsystem from an ESPG code
        /// </summary>
        /// <param name="ESPG">Known EPSG code</param>
        public static string GetCoordSysClauseFromEPSG(String EPSG)
        {
            string expr;

            expr = string.Format("EPSGToCoordSysString$(\"{0}\")", EPSG);
            //MessageBox.Show(string.Format("EPSG query: {0}", expr));
            return InteropServices.MapInfoApplication.Eval(expr);
        }

        #endregion

        #region [INSERTING OBJECTS INTO THE COSMETIC LAYER]

        /// <summary>
        /// Inserting points into cosmetic layer
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void CreatePointInCosmeticLayer(Int32 windowID, Double pointX, Double pointY, String csys)
        {
            // Before we do any work involving the map's X/Y coordinates, we
            // will set the current coordinate system; that way, we will guarantee 
            // that the coordinates will be processed correctly, regardless of which
            // coordinate system is in use in the current map. 
            // But, before we set the coordinate system, make note of the current 
            // coordinate system, so that we can restore it later.  This way, in
            // the unlikely event that the user typed a Set CoordSys statement into 
            // the MapBasic window, we will preserve the coordsys typed in by the user. 

            // Make note of the current MapBasic Coordinate System, equivalent
            // to calling:  SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
            string oldCoordSys = GetSessionCoordSys();

            // Set the coordsys clause to the csys that was saved with the named view 
            SetSessionCoordSys(csys);

            // Set the map view 
            string insertStmt = string.Format(
                "Insert Into WindowInfo({0}, {1}) (OBJ) Values (CreatePoint({2}, {3}))",
                    windowID, WIN_INFO_TABLE, GetDeformattedString(Convert.ToString(pointX)), GetDeformattedString(Convert.ToString(pointY)));

            //MessageBox.Show(insertStmt);
            //PrintMessage(insertStmt);

            InteropServices.MapInfoApplication.Do(insertStmt);

            // Restore the MapBasic Coordinate System to its previous state 
            SetSessionCoordSys(oldCoordSys);
        }

        /// <summary>
        /// Inserting points into cosmetic layer
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void CreateLineInCosmeticLayer(Int32 windowID, Double startX, Double startY, Double endX, Double endY, String csys)
        {
            // Before we do any work involving the map's X/Y coordinates, we
            // will set the current coordinate system; that way, we will guarantee 
            // that the coordinates will be processed correctly, regardless of which
            // coordinate system is in use in the current map. 
            // But, before we set the coordinate system, make note of the current 
            // coordinate system, so that we can restore it later.  This way, in
            // the unlikely event that the user typed a Set CoordSys statement into 
            // the MapBasic window, we will preserve the coordsys typed in by the user. 

            // Make note of the current MapBasic Coordinate System, equivalent
            // to calling:  SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
            string oldCoordSys = GetSessionCoordSys();

            // Set the coordsys clause to the csys that was saved with the named view 
            SetSessionCoordSys(csys);

            // Set the map view 
            string insertStmt = string.Format(
                "Insert Into WindowInfo({0}, {1}) (OBJ) Values (CreateLine({2}, {3}, {4}, {5}))"
                    , windowID
                    , WIN_INFO_TABLE
                    , GetDeformattedString(Convert.ToString(startX))
                    , GetDeformattedString(Convert.ToString(startY))
                    , GetDeformattedString(Convert.ToString(endX))
                    , GetDeformattedString(Convert.ToString(endY)));

            //MessageBox.Show(insertStmt);
            //PrintMessage(insertStmt);

            InteropServices.MapInfoApplication.Do(insertStmt);

            // Restore the MapBasic Coordinate System to its previous state 
            SetSessionCoordSys(oldCoordSys);
        }

        #endregion

        #region [INSERTING OBJECTS INTO A TABLE]

        /// <summary>
        /// Inserting points into a table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void InsertPointIntoTable(string table, Double pointX, Double pointY, String csys)
        {
            // Before we do any work involving the map's X/Y coordinates, we
            // will set the current coordinate system; that way, we will guarantee 
            // that the coordinates will be processed correctly, regardless of which
            // coordinate system is in use in the current map. 
            // But, before we set the coordinate system, make note of the current 
            // coordinate system, so that we can restore it later.  This way, in
            // the unlikely event that the user typed a Set CoordSys statement into 
            // the MapBasic window, we will preserve the coordsys typed in by the user. 

            // Make note of the current MapBasic Coordinate System, equivalent
            // to calling:  SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
            string oldCoordSys = GetSessionCoordSys();

            // Set the coordsys clause to the csys that was saved with the named view 
            SetSessionCoordSys(csys);

            // Set the map view 
            string insertStmt = string.Format(
                "Insert Into {0} (OBJ) Values (CreatePoint({1}, {2}))"
                    , table
                    , GetDeformattedString(Convert.ToString(pointX))
                    , GetDeformattedString(Convert.ToString(pointY)));

            //MessageBox.Show(insertStmt);
            //PrintMessage(insertStmt);

            InteropServices.MapInfoApplication.Do(insertStmt);

            // Restore the MapBasic Coordinate System to its previous state 
            SetSessionCoordSys(oldCoordSys);
        }

        /// <summary>
        /// Inserting lines into a table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void InsertLineIntoTable(string table, Double startX, Double startY, Double endX, Double endY, String csys)
        {
            // Before we do any work involving the map's X/Y coordinates, we
            // will set the current coordinate system; that way, we will guarantee 
            // that the coordinates will be processed correctly, regardless of which
            // coordinate system is in use in the current map. 
            // But, before we set the coordinate system, make note of the current 
            // coordinate system, so that we can restore it later.  This way, in
            // the unlikely event that the user typed a Set CoordSys statement into 
            // the MapBasic window, we will preserve the coordsys typed in by the user. 

            // Make note of the current MapBasic Coordinate System, equivalent
            // to calling:  SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
            string oldCoordSys = GetSessionCoordSys();

            // Set the coordsys clause to the csys that was saved with the named view 
            SetSessionCoordSys(csys);

            // Set the map view 
            string insertStmt = string.Format(
                "Insert Into {0} (OBJ) Values (CreateLine({1}, {2}, {3}, {4}))"
                    , table
                    , GetDeformattedString(Convert.ToString(startX))
                    , GetDeformattedString(Convert.ToString(startY))
                    , GetDeformattedString(Convert.ToString(endX))
                    , GetDeformattedString(Convert.ToString(endY)));

            //MessageBox.Show(insertStmt);
            //PrintMessage(insertStmt);

            InteropServices.MapInfoApplication.Do(insertStmt);

            // Restore the MapBasic Coordinate System to its previous state 
            SetSessionCoordSys(oldCoordSys);
        }

        /// <summary>
        /// Inserting lines into a table
        /// </summary>
        /// <param name="tableName">identification name of table</param>
        public static void InsertLineIntoTableWithID(string table, int id, Double startX, Double startY, Double endX, Double endY, String csys)
        {
            // Before we do any work involving the map's X/Y coordinates, we
            // will set the current coordinate system; that way, we will guarantee 
            // that the coordinates will be processed correctly, regardless of which
            // coordinate system is in use in the current map. 
            // But, before we set the coordinate system, make note of the current 
            // coordinate system, so that we can restore it later.  This way, in
            // the unlikely event that the user typed a Set CoordSys statement into 
            // the MapBasic window, we will preserve the coordsys typed in by the user. 

            // Make note of the current MapBasic Coordinate System, equivalent
            // to calling:  SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
            string oldCoordSys = GetSessionCoordSys();

            // Set the coordsys clause to the csys that was saved with the named view 
            SetSessionCoordSys(csys);

            // Set the map view 
            string insertStmt = string.Format(
                "Insert Into {0} (ID, OBJ) Values ({1}, CreateLine({2}, {3}, {4}, {5}))"
                    , table
                    , id
                    , GetDeformattedString(Convert.ToString(startX))
                    , GetDeformattedString(Convert.ToString(startY))
                    , GetDeformattedString(Convert.ToString(endX))
                    , GetDeformattedString(Convert.ToString(endY)));

            //MessageBox.Show(insertStmt);
            //PrintMessage(insertStmt);

            InteropServices.MapInfoApplication.Do(insertStmt);

            // Restore the MapBasic Coordinate System to its previous state 
            SetSessionCoordSys(oldCoordSys);
        }

        #endregion
    
    }
}
