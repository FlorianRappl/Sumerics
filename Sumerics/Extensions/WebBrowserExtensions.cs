namespace Sumerics
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Controls;

    static class WebBrowserExtensions
    {
        public static void SetSilent(this WebBrowser browser, Boolean silent = true)
        {
            var sp = browser.Document as IOleServiceProvider;

            if (sp != null)
            {
                var IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                var IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");
                var webBrowser = default(Object);

                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);

                if (webBrowser != null)
                {
                    var type = webBrowser.GetType();
                    var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty;
                    type.InvokeMember("Silent", flags, null, webBrowser, new Object[] { silent });
                }
            }
        }

        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IOleServiceProvider
        {
            [PreserveSig]
            Int32 QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out Object ppvObject);
        }
    }
}
