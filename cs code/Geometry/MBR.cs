using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace WindowHelper.Geometry
{
    class MBR
    {
        Double _minimumX;
        Double _minimumY;
        Double _maximumX;
        Double _maximumY;

        public MBR(Double minimumX, Double minimumY, Double maximumX, Double maximumY)
        {
            _minimumX = minimumX;
            _minimumY = minimumY;
            _maximumX = maximumX;
            _maximumY = maximumY;
        }
        public MBR()
        {
            _minimumX = 0;
            _minimumY = 0;
            _maximumX = 0;
            _maximumY = 0;
        }
        public MBR(MBR miMBR)
        {
            _minimumX = miMBR.MinimumX;
            _minimumY = miMBR.MinimumY;
            _maximumX = miMBR.MaximumX;
            _maximumY = miMBR.MaximumY;
        }

        //properties
        public Double MinimumX
        {
            get { return _minimumX; }
            set { _minimumX = value; }
        }
        public Double MinimumY
        {
            get { return _minimumY; }
            set { _minimumY = value; }
        }
        public Double MaximumX
        {
            get { return _maximumX; }
            set { _maximumX = value; }
        }
        public Double MaximumY
        {
            get { return _maximumY; }
            set { _maximumY = value; }
        }

        public override String ToString()
        { 
            return String.Format("({0} {1}) ({2} {3})", Convert.ToString(_minimumX), Convert.ToString(_minimumY), Convert.ToString(_maximumX), Convert.ToString(_maximumY));
        }

        public String GetCenter()
        {
            return String.Format("({0} {1})", Convert.ToString(GetCenterX()), Convert.ToString(GetCenterY()));
        }

        public Double GetCenterX()
        { 
            return _minimumX + ((_maximumX - _minimumX) / 2.0);
        }

        public Double GetCenterY()
        { 
            return _minimumY + ((_maximumY - _minimumY) / 2.0);
        }

    }
}
