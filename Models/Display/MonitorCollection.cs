using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonitorProfiler.Win32;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MonitorProfiler.Models.Display
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

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

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

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Debug.Write("Checking duration: " + ts.ToString());
            return true;
        }
    }
}
