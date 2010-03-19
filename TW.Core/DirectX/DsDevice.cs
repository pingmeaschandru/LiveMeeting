using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace TW.Core.DirectX
{
    public class DsDevice : IDisposable
    {
        private string name;

        public DsDevice(IMoniker moniker)
        {
            Moniker = moniker;
            name = null;
        }

        public IMoniker Moniker { get; private set; }

        public string Name
        {
            get
            {
                if (name == null)
                    name = GetFriendlyName();
                
                return name;
            }
        }

        public string DevicePath
        {
            get
            {
                string s = null;

                try
                {
                    Moniker.GetDisplayName(null, null, out s);
                }
                catch
                {
                }

                return s;
            }
        }

        public static DsDevice[] GetDevicesOfCat(Guid FilterCategory)
        {
            DsDevice[] devret;
            var devs = new ArrayList();
            IEnumMoniker enumMon;
            var enumDev = (ICreateDevEnum)new CreateDevEnum();
            var hr = enumDev.CreateClassEnumerator(FilterCategory, out enumMon, 0);
            DsError.ThrowExceptionForHR(hr);

            if (hr != 1)
            {
                try
                {
                    try
                    {
                        var mon = new IMoniker[1];
                        while ((enumMon.Next(1, mon, IntPtr.Zero) == 0))
                        {
                            try
                            {
                                devs.Add(new DsDevice(mon[0]));
                            }
                            catch
                            {
                                Marshal.ReleaseComObject(mon[0]);
                                throw;
                            }
                        }
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(enumMon);
                    }

                    devret = new DsDevice[devs.Count];
                    devs.CopyTo(devret);
                }
                catch
                {
                    foreach (DsDevice d in devs)
                    {
                        d.Dispose();
                    }
                    throw;
                }
            }
            else
            {
                devret = new DsDevice[0];
            }

            return devret;
        }

        private string GetFriendlyName()
        {
            IPropertyBag bag;
            string ret;
            object bagObj = null;

            try
            {
                var bagId = typeof(IPropertyBag).GUID;
                Moniker.BindToStorage(null, null, ref bagId, out bagObj);

                bag = (IPropertyBag)bagObj;

                object val;
                var hr = bag.Read("FriendlyName", out val, null);
                DsError.ThrowExceptionForHR(hr);

                ret = val as string;
            }
            catch
            {
                ret = null;
            }
            finally
            {
                if (bagObj != null)
                    Marshal.ReleaseComObject(bagObj);
            }

            return ret;
        }

        public void Dispose()
        {
            if (Moniker != null)
            {
                Marshal.ReleaseComObject(Moniker);
                Moniker = null;
                GC.SuppressFinalize(this);
            }
            name = null;
        }
    }
}