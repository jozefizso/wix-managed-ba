using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace TradersBootstrapper.Installer
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel(BootstrapperApplication bootstrapperApp)
        {
            this.BootstrapperApp = bootstrapperApp;
            this.BootstrapperApp.ApplyComplete += this.OnApplyComplete;
            this.BootstrapperApp.DetectPackageComplete += this.OnDetectPackageComplete;
            this.BootstrapperApp.PlanComplete += this.OnPlanComplete;

            this.Initialize();
        }

        public BootstrapperApplication BootstrapperApp { get; }

        public Engine Engine
        {
            get { return this.BootstrapperApp.Engine; }
        }


        private string displayName;
        public string DisplayName
        {
            get { return displayName; }
            set
            {
                displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }


        private bool installEnabled;
        public bool InstallEnabled
        {
            get { return installEnabled; }
            set
            {
                installEnabled = value;
                OnPropertyChanged("InstallEnabled");
            }
        }

        private bool uninstallEnabled;
        public bool UninstallEnabled
        {
            get { return uninstallEnabled; }
            set
            {
                uninstallEnabled = value;
                OnPropertyChanged("UninstallEnabled");
            }
        }

        private bool isThinking;
        public bool IsThinking
        {
            get { return isThinking; }
            set
            {
                isThinking = value;
                OnPropertyChanged("IsThinking");
            }
        }

        private string cmdParameters;

        public string CommandLineParameters
        {
            get { return cmdParameters; }
            set
            {
                cmdParameters = value;
                OnPropertyChanged("CommandLineParameters");
            }
        }

        private void InstallExecute()
        {
            IsThinking = true;
            this.Engine.Plan(LaunchAction.Install);
        }

        private void UninstallExecute()
        {
            IsThinking = true;
            this.Engine.Plan(LaunchAction.Uninstall);
        }

        private void ExitExecute()
        {
            TradersBootstrapperApp.BootstrapperDispatcher.InvokeShutdown();
        }

        private void OnDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
        {
            if (e.State == PackageState.Absent)
            {
                this.InstallEnabled = true;
            }
            else if (e.State == PackageState.Present)
            {
                this.UninstallEnabled = true;
            }
        }

        private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            this.IsThinking = false;
            this.InstallEnabled = false;
            this.UninstallEnabled = false;
        }

        private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
        {
            if (e.Status >= 0)
            {
                this.Engine.Apply(System.IntPtr.Zero);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Initialize()
        {
            var bundleManifestData = this.ApplicationData;
            this.DisplayName = bundleManifestData.Element(ManifestNamespace + "WixBundleProperties").Attribute("DisplayName").Value + " Setup";
            this.CommandLineParameters = "/" + this.BootstrapperApp.Command.Action.ToString().ToLowerInvariant();
        }

        public static readonly XNamespace ManifestNamespace = (XNamespace)"http://schemas.microsoft.com/wix/2010/BootstrapperApplicationData";

        public XElement ApplicationData
        {
            get
            {
                var workingFolder = Path.GetDirectoryName(this.GetType().Assembly.Location);
                var bootstrapperDataFilePath = Path.Combine(workingFolder, "BootstrapperApplicationData.xml");

                using (var reader = new StreamReader(bootstrapperDataFilePath))
                {
                    var xml = reader.ReadToEnd();
                    var xDoc = XDocument.Parse(xml);
                    return xDoc.Element(ManifestNamespace + "BootstrapperApplicationData");
                }
            }
        }

        private RelayCommand installCommand;
        public RelayCommand InstallCommand
        {
            get
            {
                if (installCommand == null)
                    installCommand = new RelayCommand(() => InstallExecute(), () => InstallEnabled == true);
                return installCommand;
            }
        }

        private RelayCommand uninstallCommand;
        public RelayCommand UninstallCommand
        {
            get
            {
                if (uninstallCommand == null)
                    uninstallCommand = new RelayCommand(() => UninstallExecute(), () => UninstallEnabled == true);
                return uninstallCommand;
            }
        }

        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                    exitCommand = new RelayCommand(() => ExitExecute());
                return exitCommand;
            }
        }
    }
}
