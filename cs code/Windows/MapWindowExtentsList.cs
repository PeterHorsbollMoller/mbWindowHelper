using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowHelper.Windows
{
    class MapWindowExtentsList : List<MapWindowExtents>
    {

        //********************************************************************************************
        #region Contructors
        //----------------------------------------------------------------
        public MapWindowExtentsList()
        {
        }
        //----------------------------------------------------------------
        public MapWindowExtentsList(Window mapWindow)
        {
            MapWindowExtents extent = new MapWindowExtents(mapWindow);
            this.Add(extent);
        }

        //----------------------------------------------------------------
        public MapWindowExtentsList(Window mapWindow, Boolean allowWrapAround)
        {
            MapWindowExtents extent = new MapWindowExtents(mapWindow, allowWrapAround);
            this.Add(extent);
        }
        #endregion Constructors

        //********************************************************************************************
        #region Methods

        //----------------------------------------------------------------
        public int FindWindow(Window mapWindow)
        {
            int i = 0;

            foreach (MapWindowExtents mapWindowExtent in this)
            {
                if (mapWindowExtent.MapWindow.ID == mapWindow.ID)
                    return i;

                i++;
            }

            return -1;
        }

        //----------------------------------------------------------------
        public Boolean WindowExists(Window mapWindow)
        {
            if (FindWindow(mapWindow) == -1)
                return false;
            else
                return true;
        }

        #endregion Methods
    }
}
