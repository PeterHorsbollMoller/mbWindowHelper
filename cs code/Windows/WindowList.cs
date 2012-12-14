using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowHelper.Windows
{
    class WindowList : List<Window> 
    {
        public WindowList(int[] windowIDs, string[] windowNames, int[] windowTypes)
        {
            Window window;

            for (int i = 0; i <= windowIDs.GetUpperBound(0); i++)
            {
                window = new Window(windowIDs[i], windowNames[i], windowTypes[i]);
                this.Add(window);
            }
        }
        
        public WindowList(int[] windowIDs, string[] windowNames)
        {
            Window window;

            for (int i = 0; i <= windowIDs.GetUpperBound(0); i++)
            {
                window = new Window(windowIDs[i], windowNames[i]);
                this.Add(window);
            }
        }

         public WindowList(int[] windowIDs)
         {
             Window window;

             for (int i = 0; i <= windowIDs.GetUpperBound(0); i++)
             {
                 window = new Window(windowIDs[i]);
                 this.Add(window);
             }
         }

         public WindowList()
         {
             //Nothing to do - just an empty list of Windows
         }
    }
}
