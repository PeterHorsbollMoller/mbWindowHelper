using System;
using System.Collections.Generic;
using System.Text;

namespace WindowHelper.Geometry
{
    /// <summary>
    /// Represents a single element of a coordinate of type Double
    /// </summary>

    public class Coordinate
    {
        #region Private fields

        private double _x;
        private double _y;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new (0,0) Coordinate.
        /// </summary>
        public Coordinate()
        {
            _x = 0D;
            _y = 0D;
        }

        /// <summary>
        /// Create a new Coordinate
        /// </summary>
        /// <param name="x">value the X Coordinate should have</param>
        /// <param name="y">value the Y Coordinate should have</param>
        public Coordinate(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public Coordinate(string x, string y)
        {
            System.Globalization.NumberFormatInfo usNumberFormat;
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
            usNumberFormat = cultureInfo.NumberFormat;

            double ordinateX = 0.00;
            double ordinateY = 0.00;

            if (double.TryParse(x, System.Globalization.NumberStyles.Number, usNumberFormat, out ordinateX))
            {
                if (double.TryParse(y, System.Globalization.NumberStyles.Number, usNumberFormat, out ordinateY))
                {
                    _x = ordinateX;
                    _y = ordinateY;
                }
                else
                {
                    _x = 0;
                    _y = 0;
                }
            }
            else
            {
                _x = 0;
                _y = 0;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Get or set the X of the coordinate.
        /// </summary>
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Get or set the Y of the coordinate.
        /// </summary>
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a Clone Coordinate
        /// </summary>
        public Coordinate Clone()
        {
            return new Coordinate(_x, _y);
        }

       public override string ToString()
        {
           string strValue = String.Format("{0} , {1}", this.XNumericString(), this.YNumericString());
           return strValue;
        }

       //public string ToString(int numDecimals)
       //{
       //    string strValue = String.Format("{0} , {1}", this.XNumericString(numDecimals), this.YNumericString(numDecimals));
       //    return strValue;
       //}

       public string XNumericString()
       {
           string strValue = String.Format("{0:n6}", _x);
           return strValue;
       }

       public string YNumericString()
       {
           string strValue = String.Format("{0:n6}", _y);
           return strValue;
       }

       //public string XNumericString(int numDecimals)
       //{
       //    string format = string.Format("{0:n{0}}", numDecimals);
       //    string strValue = string.Format(format, _x);
       //    return strValue;
       //}

       //public string YNumericString(int numDecimals)
       //{
       //    string format = string.Format("{0:n{0}}", numDecimals);
       //    string strValue = string.Format(format, _y);
       //    return strValue;
       //}

       public string XUSNumericString()
       {
           string strValue = _x.ToString("#.######", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
           return strValue;
       }

       public string YUSNumericString()
       {
           string strValue = _y.ToString("#.######", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
           return strValue;
       }

       public Coordinate Offset(double distance, double degrees)
       {
           degrees = degrees * (Math.PI / 180.0);
           double x = _x + (Math.Cos(degrees) * distance);
           double y = _y + (Math.Sin(degrees) * distance);

           return new Coordinate(x, y);
       }

       public Coordinate Move(double dX, double dY)
       {
           double x = _x + dX;
           double y = _y + dY;

           return new Coordinate(x, y);
       }

       public double DistanceTo(Coordinate coordTo)
       {
           Double dist;
           dist = Math.Sqrt((Math.Pow((this.X - coordTo.X), 2.0) + Math.Pow((this.Y - coordTo.Y), 2.0)));
           return dist;
       }

       public double DirectionTo(Coordinate coordTo)
       {
           double dir;

           //Checking for vertical dirction
           if (coordTo.X == this.X)
           {
               if (coordTo.Y == this.Y)
               {
                   //_start == _end
                   dir = 0;
                   return dir;
               }

               //not allowed to divide by zero...
               if (coordTo.Y > this.Y)
                   //tEnd is north of tStart...
                   dir = 90;	//degrees
               else
                   //tEnd is south of tStart...
                   dir = 270;	//degrees

               return dir;
           }
           //Checking for horisontal direction
           else if (coordTo.Y == this.Y)
           {
               //not allowed to divide by zero...
               if (coordTo.X < this.X)
                   //tEnd is west of tStart...
                   dir = 180;	//degrees
               else
                   //tEnd is east of tStart...
                   dir = 0;    //degrees

               return dir;
           }

           dir = Math.Atan((coordTo.Y - this.Y) / (coordTo.X - this.X)); //radians
           dir = dir * (180.0 / Math.PI);  //Converting radians to 360 degress

           if (coordTo.X < this.X)
           {
               if (coordTo.Y > this.Y)
                   //tEnd is northwest of tStart
                   dir = dir + 180;	//degrees
               else if (coordTo.Y < this.Y)
                   //tEnd is southwest of tStart
                   dir = dir + 180;	//degrees
           }

           //Making sure dir is between 0 and 360
           while (dir > 360)
           {
               dir = dir - 360;
           }
           while (dir < 0)
           {
               dir = dir + 360;
           }

           return dir;
       }

       public double DirectionFrom(Coordinate coordTo)
       {
           return coordTo.DirectionTo(this);
       }
        
       #endregion    
    
    }
}
