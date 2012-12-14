using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowHelper.Windows
{
    class MapWindowExtents
    {
        Window _mapWindow;
        MapExtentList _extentList;

        #region Constructors
        public MapWindowExtents(Window mapWindow)
        {
            _mapWindow = mapWindow;

            _extentList = new MapExtentList(_mapWindow);
        }

        public MapWindowExtents(Window mapWindow, Boolean allowWrapAround)
        {
            _mapWindow = mapWindow;

            _extentList = new MapExtentList(_mapWindow, allowWrapAround);
        }
        #endregion Constructors

        #region Properties
        public Window MapWindow
        {
            get { return _mapWindow; }
            //set { _mapWindow = value; }
        }

        public MapExtentList MapExtentList
        {
            get { return _extentList; }
            //set { _extentList = value; }
        }

        public List<string> MapExtentNames
        {
            get 
            {
                List<string> names = new List<string>();
                foreach (MapExtent extent in _extentList)
                {
                    names.Add(extent.ToString());
                }
                return names;
            }
            //set { }
        }

        #endregion Properties
    }
}
