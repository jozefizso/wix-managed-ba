using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradersApp
{
    public class Program
    {

        [STAThread]
        public static void Main(string[] args)
        {
            int hr = RestartManager.RegisterApplicationRestart(RestartManager.RestartCommandLineParameter, RestartFlags.RESTART_NO_CRASH | RestartFlags.RESTART_NO_HANG);

            bool isRestarted = args.Length > 0 && args[0] == "--restarted";

            App app = new App();
            var window = new MainWindow();
            window.IsRestarted = isRestarted;
            app.Run(window);
        }
    }
}
