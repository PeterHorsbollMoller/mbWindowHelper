using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowHelper.Windows
{
    class MapExtentList : List<MapExtent>
    {
        private int _currentIndex;
        private int _previousIndex = -1;
        private bool _allowWrapAround;
        private bool _zooming = false; 

        //********************************************************************************************
        #region Contructors
        public MapExtentList(Window mapWindow)
        {
            MapExtent extent = new MapExtent(mapWindow);
            this.Add(extent);
            _previousIndex = -1;
            _currentIndex = 0;
        }
        public MapExtentList(Window mapWindow, Boolean allowWrapAround)
        {
            _allowWrapAround = allowWrapAround;

            MapExtent extent = new MapExtent(mapWindow);
            this.Add(extent);
            _previousIndex = -1;
            _currentIndex = (this.Count - 1);
        }
        #endregion Constructors

        //********************************************************************************************
        #region Properties
        public Boolean AllowWrapAround
        {
            get { return _allowWrapAround; }
            set { _allowWrapAround = value; }
        }

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set 
            {
                _previousIndex = _currentIndex;
                _currentIndex = value; 
            }
        }

        public int PreviousIndex
        {
            get { return _currentIndex; }
            set { _currentIndex = value; }
        }

        public int NumberOfExtents
        {
            get { return this.Count; }
            //set { }
        }

        #endregion Properties

        //********************************************************************************************
        #region Methods
        //-------------------------------------------------
        public int AddExtent(Window mapWindow)
        {
            if (_zooming == true)
                return this.CurrentIndex;

            MapExtent extent = new MapExtent(mapWindow);
            if (extent.EqualsMapExtent(this[this.CurrentIndex]))
                return this.CurrentIndex;

            if (extent.EqualsMapExtent(this[(this.Count - 1)]))
            {
                this.CurrentIndex = (this.Count - 1);
                return this.CurrentIndex;
            }

            if (this.Count > 1)
            {
                if (extent.EqualsMapExtent(this[(this.Count - 2)]))
                {
                    this.CurrentIndex = (this.Count - 2);
                    return this.CurrentIndex;
                }
            }

            if (this.PreviousIndex > -1)
            {
                if (extent.EqualsMapExtent(this[this.PreviousIndex]))
                {
                    this.CurrentIndex = this.PreviousIndex;
                    return this.CurrentIndex;
                }
            }

            this.Add(extent);
            this.CurrentIndex = (this.Count - 1);
            return this.CurrentIndex;
        }

        //-------------------------------------------------
        public void ZoomToPrevious(Window mapWindow)
        {
            if (_currentIndex == 0)
            {
                if (_allowWrapAround == false)
                    return;
                else
                    _currentIndex = this.Count;
            }
            else
                _currentIndex--;

            SetExtent(_currentIndex, mapWindow);
        }
        //-------------------------------------------------
        public void ZoomToNext(Window mapWindow)
        {
            if (_currentIndex == (this.Count - 1))
            {
                if (_allowWrapAround == false)
                    return;
                else
                    _currentIndex = 0;
            }
            else
                _currentIndex++;

            SetExtent(_currentIndex, mapWindow);
        }
        //-------------------------------------------------
        public void ZoomToFirst(Window mapWindow)
        {
            _currentIndex = 0;
            SetExtent(_currentIndex, mapWindow);
        }
        //-------------------------------------------------
        public void ZoomToLast(Window mapWindow)
        {
            _currentIndex = (this.Count - 1);
            SetExtent(_currentIndex, mapWindow);
        }

        ///// <summary>
        ///// Sets the current view of the mapper window to the extent represented by index 
        ///// </summary>
        ///// <param name="index">The entent to apply on the MapWindow</param>
        //public void SetExtent(int index)
        //{
        //    this[index].SetExtent(_mapWindow);
        //}
        ///// <summary>
        ///// Sets the current center of the mapper window to the extent represented by index 
        ///// </summary>
        ///// <param name="index">The entent/center to apply on the MapWindow</param>
        //public void SetCenter(int index)
        //{
        //    this[index].SetCenter(_mapWindow);
        //}
        ///// <summary>
        ///// Sets the current zoom of the mapper window to the extent represented by index 
        ///// </summary>
        ///// <param name="index">The entent/zoom to apply on the MapWindow</param>
        //public void SetZoom(int index)
        //{
        //    this[index].SetZoom(_mapWindow);
        //}

        /// <summary>
        /// Sets the current view of the mapper window to the extent represented by index 
        /// </summary>
        /// <param name="index">The entent to apply on the MapWindow</param>
        /// <param name="mapWindow">The Map Window to apply the extent on</param>
        //---------------------------------------------------------
        public void SetExtent(int index, Window mapWindow)
        {
            _zooming = true;
            this[index].SetExtent(mapWindow);
            _zooming = false;
        }
        /// <summary>
        /// Sets the current center of the mapper window to the extent represented by index 
        /// </summary>
        /// <param name="index">The entent/center to apply on the MapWindow</param>
        /// <param name="mapWindow">The Map Window to apply the extent on</param>
        //---------------------------------------------------------
        public void SetCenter(int index, Window mapWindow)
        {
            this[index].SetCenter(mapWindow);
        }
        /// <summary>
        /// Sets the current zoom of the mapper window to the extent represented by index 
        /// </summary>
        /// <param name="index">The entent/zoom to apply on the MapWindow</param>
        /// <param name="mapWindow">The Map Window to apply the extent on</param>
        //---------------------------------------------------------
        public void SetZoom(int index, Window mapWindow)
        {
            this[index].SetZoom(mapWindow);
        }

        //---------------------------------------------------------
        public string GetCurrentExtentAsString()
        {
            return this[_currentIndex].ToString();
        }
        //---------------------------------------------------------
        public string GetExtentAsString(int index)
        {
            return this[index].ToString();
        }

        #endregion Methods
    }
}
