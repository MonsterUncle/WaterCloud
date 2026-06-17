using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace WaterCloud.Code
{
    public class ComputerHelper
    {
        public static ComputerInfo GetComputerInfo()
        {
            var computerInfo = new ComputerInfo();
            try
            {
                var client = new MemoryMetricsClient();
                var memoryMetrics = client.GetMetrics();
                computerInfo.TotalRAM = Math.Ceiling(memoryMetrics.Total / 1024).ToString() + " GB";
                computerInfo.RAMRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total).ToString();
                computerInfo.CPURate = Math.Ceiling(GetCPURate().ToDouble()).ToString();
                computerInfo.RunTime = GetRunTime();
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return computerInfo;
        }

        public static bool IsUnix()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public static string GetCPURate()
        {
            if (IsUnix())
            {
                string output = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
                return output.Trim();
            }
            else
            {
                return GetWindowsCPURate();
            }
        }

        [SupportedOSPlatform("windows")]
        private static string GetWindowsCPURate()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT LoadPercentage FROM Win32_Processor"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return obj["LoadPercentage"]?.ToString() ?? "0";
                    }
                }
            }
            catch { }
            return "0";
        }

        public static string GetRunTime()
        {
            try
            {
                if (IsUnix())
                {
                    string output = ShellHelper.Bash("uptime -s");
                    output = output.Trim();
                    return Extensions.FormatTime((DateTime.Now - output.ToDate()).TotalMilliseconds.ToString().Split('.')[0].ToLong());
                }
                else
                {
                    return GetWindowsRunTime();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return "";
        }

        [SupportedOSPlatform("windows")]
        private static string GetWindowsRunTime()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT LastBootUpTime FROM Win32_OperatingSystem"))
            {
                foreach (var obj in searcher.Get())
                {
                    var bootTime = ManagementDateTimeConverter.ToDateTime(obj["LastBootUpTime"]?.ToString() ?? "");
                    return Extensions.FormatTime((DateTime.Now - bootTime).TotalMilliseconds.ToString().Split('.')[0].ToLong());
                }
            }
            return "";
        }
    }

    public class MemoryMetrics
    {
        public double Total { get; set; }
        public double Used { get; set; }
        public double Free { get; set; }
    }

    public class MemoryMetricsClient
    {
        public MemoryMetrics GetMetrics()
        {
            if (ComputerHelper.IsUnix())
            {
                return GetUnixMetrics();
            }
            return GetWindowsMetrics();
        }

        [SupportedOSPlatform("windows")]
        private MemoryMetrics GetWindowsMetrics()
        {
            var metrics = new MemoryMetrics();
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        metrics.Total = Math.Round(Convert.ToDouble(obj["TotalVisibleMemorySize"] ?? 0) / 1024, 0);
                        metrics.Free = Math.Round(Convert.ToDouble(obj["FreePhysicalMemory"] ?? 0) / 1024, 0);
                        metrics.Used = metrics.Total - metrics.Free;
                    }
                }
            }
            catch { }
            return metrics;
        }

        private MemoryMetrics GetUnixMetrics()
        {
            string output = ShellHelper.Bash("free -m");

            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = double.Parse(memory[1]);
            metrics.Used = double.Parse(memory[2]);
            metrics.Free = double.Parse(memory[3]);

            return metrics;
        }
    }

    public class ComputerInfo
    {
        public string CPURate { get; set; }
        public string TotalRAM { get; set; }
        public string RAMRate { get; set; }
        public string RunTime { get; set; }
    }
}
