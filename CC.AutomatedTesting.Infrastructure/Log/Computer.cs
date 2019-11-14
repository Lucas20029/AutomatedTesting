using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Management;

/// <summary>
///Computer 的摘要说明
/// </summary>
public class Computer
{
    public static string ipAddress;
    public static string IpAddress
    {
        get
        {
            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = GetIPAddress();
            return ipAddress;
        }
    }


    public static string computerName;
    public static string ComputerName
    {
        get
        {
            if (string.IsNullOrEmpty(computerName))
                computerName = GetComputerName();
            return computerName;
        }
    }

    public static string loginUserName;
    public static string LoginUserName
    {
        get
        {
            if (string.IsNullOrEmpty(loginUserName))
                loginUserName = GetUserName();
            return loginUserName;
        }
    }
    
    public static string systemType;
    public static string SystemType
    {
        get
        {
            if (string.IsNullOrEmpty(systemType))
                systemType = GetSystemType();
            return systemType;
        }
    }

    
    public static string GetCpuID()
    {
        try
        {
            //获取CPU序列号代码 
            string cpuInfo = "";//cpu序列号 
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
            }
            moc = null;
            mc = null;
            return cpuInfo;
        }
        catch
        {
            return "unknow";
        }
        finally
        {
        }

    }
    public static string GetMacAddress()
    {
        try
        {
            //获取网卡硬件地址 
            string mac = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    mac = mo["MacAddress"].ToString();
                    break;
                }
            }
            moc = null;
            mc = null;
            return mac;
        }
        catch
        {
            return "unknow";
        }
        finally
        {
        }

    }
    public static string GetIPAddress()
    {
        try
        {
            //获取IP地址 
            string st = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    //st=mo["IpAddress"].ToString(); 
                    System.Array ar;
                    ar = (System.Array)(mo.Properties["IpAddress"].Value);
                    st = ar.GetValue(0).ToString();
                    break;
                }
            }
            moc = null;
            mc = null;
            return st;
        }
        catch
        {
            return "unknow";
        }
        finally
        {
        }

    }
    public static string GetDiskID()
    {
        try
        {
            //获取硬盘ID 
            String HDid = "";
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["Model"].Value;
            }
            moc = null;
            mc = null;
            return HDid;
        }
        catch
        {
            return "unknow";
        }
        finally
        {
        }

    }
    /// <summary> 
    /// 操作系统的登录用户名 
    /// </summary> 
    /// <returns></returns> 
    public static string GetUserName()
    {
        try
        {
            string st = "";
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {

                st = mo["UserName"].ToString();

            }
            moc = null;
            mc = null;
            return st;
        }
        catch
        {
            return "unknow";
        }
        finally
        {
        }

    }
    /// <summary> 
    /// PC类型 
    /// </summary> 
    /// <returns></returns> 
    public static string GetSystemType()
    {
        try
        {
            string st = "";
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {

                st = mo["SystemType"].ToString();

            }
            moc = null;
            mc = null;
            return st;
        }
        catch
        {
            return "unknow";
        }
        finally
        {
        }

    }
    /// <summary> 
    /// 物理内存 
    /// </summary> 
    /// <returns></returns> 
    public static string GetTotalPhysicalMemory()
    {
        try
        {

            string st = "";
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {

                st = mo["TotalPhysicalMemory"].ToString();

            }
            moc = null;
            mc = null;
            return st;
        }
        catch
        {
            return "unknow";
        }
        finally
        {
        }
    }
    /// <summary> 
    ///  
    /// </summary> 
    /// <returns></returns> 
    public static string GetComputerName()
    {
        try
        {
            return System.Environment.GetEnvironmentVariable("ComputerName");
        }
        catch
        {
            return "unknow";
        }
        finally
        {
        }
    }



}
