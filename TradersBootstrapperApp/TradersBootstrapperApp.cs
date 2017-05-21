using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace TradersBootstrapper.Installer
{
    public class TradersBootstrapperApp : BootstrapperApplication
    {
        public static Dispatcher BootstrapperDispatcher { get; private set; }

        protected override void Run()
        {
            //this.Engine.Log(LogLevel.Standard, $"Running the managed bootstrapper from class {nameof(TradersBootstrapperApp)}.");
            BootstrapperDispatcher = Dispatcher.CurrentDispatcher;

            MainViewModel viewModel = new MainViewModel(this);
            viewModel.Engine.Detect();

            //this.ExecuteFilesInUse += TradersBootstrapperApp_ExecuteFilesInUse;
            //this.DetectComplete += OnDetectComplete;
            //this.PlanComplete += OnPlanComplete;
            //this.ApplyComplete += OnApplyComplete;

            //this.Engine.Detect();

            MainView view = new MainView();
            view.DataContext = viewModel;
            view.Closed += (sender, e) => BootstrapperDispatcher.InvokeShutdown();
            view.Show();
            Dispatcher.Run();

            this.Engine.Quit(0);
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
