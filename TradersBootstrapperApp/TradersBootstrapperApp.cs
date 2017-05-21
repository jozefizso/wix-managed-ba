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

            this.ExecuteFilesInUse += TradersBootstrapperApp_ExecuteFilesInUse;
            this.DetectComplete += OnDetectComplete;
            this.PlanComplete += OnPlanComplete;
            this.ApplyComplete += OnApplyComplete;

            this.Engine.Detect();

            this.Engine.Quit(0);
        }

        private void OnDetectComplete(object sender, DetectCompleteEventArgs detectCompleteEventArgs)
        {

            if (LaunchAction.Uninstall == this.Command.Action)
            {
                this.Engine.Log(LogLevel.Verbose, "Invoking automatic plan for uninstall");
                this.Engine.Plan(LaunchAction.Uninstall);
            }
            else
            {
                // If we're not waiting for the user to click install, dispatch plan with the default action.
                this.Engine.Log(LogLevel.Verbose, "Invoking automatic plan for non-interactive mode.");
                this.Engine.Plan(this.Command.Action);
            }
        }

        private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
        {
            this.Engine.Log(LogLevel.Standard, $"OnPlanComplete: Status={e.Status}");
            if (e.Status == 0)
            {
                this.Engine.Log(LogLevel.Standard, "Engine.Apply()");
                this.Engine.Apply(IntPtr.Zero);
            }
        }

        private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            this.Engine.Log(LogLevel.Standard, $"OnApplyComplete: Status={e.Status}, Result={e.Result}");
        }

        private void TradersBootstrapperApp_ExecuteFilesInUse(object sender, ExecuteFilesInUseEventArgs e)
        {
            foreach (var file in e.Files)
            {
                this.Engine.Log(LogLevel.Standard, $"File in use: {file}");
            }

            e.Result = Result.Ok;
        }
    }
}
