using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowHelper.Windows
{
    class ClosedWindowList : List<ClosedWindow>
    {
        public ClosedWindowList(int[] windowIDs, string[] windowNames, int[] windowTypes, string[] restoreStatements)
        {
            ClosedWindow window;

            for (int i = 0; i <= windowIDs.GetUpperBound(0); i++)
            {
                window = new ClosedWindow(windowIDs[i], windowNames[i], windowTypes[i], restoreStatements[i]);
                this.Add(window);
            }
        }

        public ClosedWindowList(int[] windowIDs, string[] windowNames, int[] windowTypes)
        {
            ClosedWindow window;

            for (int i = 0; i <= windowIDs.GetUpperBound(0); i++)
            {
                window = new ClosedWindow(windowIDs[i], windowNames[i], windowTypes[i]);
                this.Add(window);
            }
        }

        public ClosedWindowList(int[] windowIDs, string[] windowNames)
        {
            ClosedWindow window;

            for (int i = 0; i <= windowIDs.GetUpperBound(0); i++)
            {
                window = new ClosedWindow(windowIDs[i], windowNames[i]);
                this.Add(window);
            }
        }

         public ClosedWindowList(int[] windowIDs)
         {
             ClosedWindow window;

             for (int i = 0; i <= windowIDs.GetUpperBound(0); i++)
             {
                 window = new ClosedWindow(windowIDs[i]);
                 this.Add(window);
             }
         }

         public ClosedWindowList()
         {
             //Nothing to do - just an empty list of Closed Windows
         }
}
}
