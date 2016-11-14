using System;
using System.Web;
using System.Configuration;
using log4net;
using log4net.Config;
namespace System
{
    public static class LoggingExtension
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void LogException(this Exception ex)
        {
            log.Error(ex);
        }
        public static void LogDebug(this Exception ex)
        {
            log.Debug(ex);
        }
        private static HttpApplication httpApplication = null;
        private static void InitNoContext()
        {
        }
    }
}
