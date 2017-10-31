using System;
using System.Collections.Generic;
using MonitorManager.Win32;

namespace MonitorManager.Models.Display
{
    public class MonitorCollection : List<Monitor>
    {
        public bool Add(IntPtr hMonitor)
        {
            uint monitorCount = 0;
            NativeMethods.GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, ref monitorCount);
            if (monitorCount <= 0)
                return false;

            var monitorArray = new NativeStructures.PHYSICAL_MONITOR[monitorCount];
            NativeMethods.GetPhysicalMonitorsFromHMONITOR(hMonitor, monitorCount, monitorArray);
            
            /*
            Parallel.ForEach(monitorArray, (physicalMonitor) =>
            {
               Monitor newMonitor = new Monitor(physicalMonitor);

               this.Add(newMonitor);
            });
            */
            
            foreach(var physicalMonitor in monitorArray)
            {
                Monitor newMonitor = new Monitor(physicalMonitor);
                this.Add(newMonitor);
            };

            /*
            Debug.Write("\n\nStart Add");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            Debug.Write("Checking duration: " + ts.ToString() + "\n");
            */

            return true;
        }
    }
}
