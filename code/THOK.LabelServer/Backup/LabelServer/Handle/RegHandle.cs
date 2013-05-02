using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace THOK.Application.LabelServer
{
    class RegHandle <T>
    {
        public void WriteReg(string key, string item,T value)
        {
            // Create a subkey named Test9999 under HKEY_CURRENT_USER.
            RegistryKey tempkey =
                Registry.CurrentUser.CreateSubKey("LabelServer");
            // Create two subkeys under HKEY_CURRENT_USER\Test9999. The
            // keys are disposed when execution exits the using statement.
            using (RegistryKey
                tempSettings = tempkey.CreateSubKey(key))
            {
                // Create data for the TestSettings subkey.
                tempSettings.SetValue(item, value );
            }
        }
        public T ReadReg(string key, string item)
        {
            try
            {
                // Create a subkey named Test9999 under HKEY_CURRENT_USER.
                RegistryKey tempkey =
                    Registry.CurrentUser.OpenSubKey("LabelServer");
                // Create two subkeys under HKEY_CURRENT_USER\Test9999. The
                // keys are disposed when execution exits the using statement.
                using (RegistryKey
                    tempSettings = tempkey.OpenSubKey(key))
                {
                    // Create data for the TestSettings subkey.
                    return (T)(tempSettings.GetValue(item));
                }
            }
            catch 
            {
                throw new Exception("读取注册表失败，请检查系统注册表是否异常！");
            }
        }
    }
}
