using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace TradersBootstrapper.Installer
{
    public class TradersBootstrapperApp : BootstrapperApplication
    {
        protected override void Run()
        {
            this.Engine.Log(LogLevel.Standard, $"Running the managed bootstrapper from class {nameof(TradersBootstrapperApp)}.");

            this.Engine.Detect();

            this.Engine.Quit(0);
        }
    }
}
