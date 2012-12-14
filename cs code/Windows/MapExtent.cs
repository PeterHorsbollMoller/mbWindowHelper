using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowHelper.Windows
{
    class MapExtent
    {
        private string _centerX;
        private string _centerY;
        private string _coordSys;
        private string _zoomWidth;
        private string _distanceUnit;

        //********************************************************************************************
        #region Contructors
        public MapExtent(string centerX, string centerY, string coordSys, string zoomWidth, string distanceUnit)
        {
            _centerX        = centerX;
            _centerY        = centerY;
            _coordSys       = coordSys;
            _zoomWidth      = zoomWidth;
            _distanceUnit   = distanceUnit;
        }

        public MapExtent(Window mapWindow)
        {
            _coordSys = InteropHelper.GetMapperCoordSys(mapWindow.ID);

            // Make note of the current MapBasic Coordinate System, equivalent
            // to calling:  SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
            string coordSysSystem = InteropHelper.GetSessionCoordSys();
            string distanceUnitSystem = InteropHelper.GetSessionDistanceUnit();

            InteropHelper.SetSessionCoordSys(_coordSys);

            _centerX = InteropHelper.GetMapperCenterX(mapWindow.ID);
            _centerY = InteropHelper.GetMapperCenterY(mapWindow.ID);

            _distanceUnit = InteropHelper.GetMapperDistanceUnit(mapWindow.ID);
            InteropHelper.SetSessionDistanceUnit(_distanceUnit);
            
            _zoomWidth = InteropHelper.GetMapperZoom(mapWindow.ID);

            // Restore the MapBasic Coordinate System to its previous state 
            InteropHelper.SetSessionCoordSys(coordSysSystem);
            InteropHelper.SetSessionDistanceUnit(distanceUnitSystem);
        }
        #endregion Contructors

        //********************************************************************************************
        #region Operators
        public bool EqualsMapExtent(MapExtent mapExtent)
        {
            if (_centerX != mapExtent.CenterX)
                return false;
            if (_centerY != mapExtent.CenterY)
                return false;
            if (_coordSys != mapExtent.CoordSys)
                return false;
            if (_zoomWidth != mapExtent.ZoomWidth)
                return false;
            if (_distanceUnit != mapExtent.DistanceUnit)
                return false;

            return true;
        }

        #endregion Operators

        //********************************************************************************************
        #region Properties
        public string CenterX
        {
            get { return _centerX; }
        }
        public string CenterY
        {
            get { return _centerY; }
        }
        public string CoordSys
        {
            get { return _coordSys; }
        }
        public string ZoomWidth
        {
            get { return _zoomWidth; }
        }
        public string DistanceUnit
        {
            get { return _distanceUnit; }
        }

        #endregion Properties

        //********************************************************************************************
        #region Methods
        /// <summary>
        /// Sets the current view of mapper window represented by mapWindow
        /// </summary>
        /// <param name="mapWindow">The window to apply the extent on</param>
        public void SetExtent(Window mapWindow)
        {
            InteropHelper.SetView(mapWindow.ID, _centerX, _centerY, _zoomWidth, _distanceUnit, _coordSys);
        }

        /// <summary>
        /// Sets the current center of mapper window represented by mapWindow
        /// </summary>
        /// <param name="mapWindow">The window to apply the extent on</param>
        public void SetCenter(Window mapWindow)
        {
            InteropHelper.SetView(mapWindow.ID, _centerX, _centerY, _coordSys);
        }

        /// <summary>
        /// Sets the current zoom of mapper window represented by mapWindow
        /// </summary>
        /// <param name="mapWindow">The window to apply the extent on</param>
        public void SetZoom(Window mapWindow)
        {
            InteropHelper.SetView(mapWindow.ID, _zoomWidth, _distanceUnit);
        }

        public override string ToString()
        {
            return string.Format("Center ( {0} , {1} ) Zoom {2} {3}", _centerX, _centerY, _zoomWidth, _distanceUnit);
        }

        #endregion Methods
    }
}
