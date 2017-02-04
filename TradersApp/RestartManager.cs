using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TradersApp
{
    public class RestartManager
    {
        public static readonly string RestartCommandLineParameter = "--restarted";

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int RegisterApplicationRestart([MarshalAs(UnmanagedType.LPWStr)] string commandLineArgs, RestartFlags flags);

    }

    [Flags]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum RestartFlags
    {
        /// <summary>
        /// No restart restrictions.
        /// </summary>
        NONE = 0,
        /// <summary>
        /// Do not restart if process terminates due to unhandled exception.
        /// </summary>
        RESTART_NO_CRASH = 1,
        /// <summary>
        /// Do not restart if process terminates due to application not responding.
        /// </summary>
        RESTART_NO_HANG = 2,
        /// <summary>
        /// Do not restart if process terminates due to installation of update.
        /// </summary>
        RESTART_NO_PATCH = 4,
        /// <summary>
        /// Do not restart if process terminates due to computer restart as result of an update.
        /// </summary>
        RESTART_NO_REBOOT = 8
    }
}
