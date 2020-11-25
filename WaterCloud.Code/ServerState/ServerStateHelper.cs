using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WaterCloud.Code
{
    public class ServerStateHelper
    {
        public static string GetUseARM()
        {

            double zyl = (GetARM() - GetARMable()) / GetARM() * 100;
            return Math.Round(zyl, 2).ToString();
        }

        public static double GetARMable()
        {
            double totalCapacity = 0;
            ObjectQuery objectQuery = new ObjectQuery("select * from Win32_PerfRawData_PerfOS_Memory");
            ManagementObjectSearcher searcher = new
           ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection vals = searcher.Get();
            foreach (ManagementObject val in vals)
            {
                totalCapacity += System.Convert.ToDouble(val.GetPropertyValue("Availablebytes"));
            }
            double ramCapacity = totalCapacity / 1048576;
            return ramCapacity;
        }

        public static double GetARM()
        {
            double totalCapacity = 0;
            ObjectQuery objectQuery = new ObjectQuery("select * from Win32_PhysicalMemory");
            ManagementObjectSearcher searcher = new
            ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection vals = searcher.Get();
            foreach (ManagementObject val in vals)
            {
                totalCapacity += System.Convert.ToDouble(val.GetPropertyValue("Capacity"));
            }
            double ramCapacity = totalCapacity / 1048576;
            return ramCapacity;
        }
        public static string GetCPU()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");
            var cpuTimes = searcher.Get()
                .Cast<ManagementObject>()
                .Select(mo => new
                {
                    Name = mo["Name"],
                    Usage = mo["PercentProcessorTime"]
                }
                )
                .ToList();
            var query = cpuTimes.Where(x => x.Name.ToString() == "_Total").Select(x => x.Usage);
            return query.SingleOrDefault().ToString();
        }
        public static string GetIISConnection()
        {
            try
            {
                var name = GetSiteName();
                System.Management.ManagementObject o = new ManagementObject("Win32_PerfFormattedData_W3SVC_WebService.Name='"+name+"'");
                return o.Properties["CurrentConnections"].Value.ToString();
            }
            catch (Exception)
            {

                return "0";
            }

        }
        public static string GetSiteName()
        {
            try
            {
                var name = System.Web.Hosting.HostingEnvironment.ApplicationHost.GetSiteName();
                return name;
            }
            catch (Exception)
            {

                return "0";
            }

        }
    }
    
}
