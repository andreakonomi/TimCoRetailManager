using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.ViewModels;

namespace TRMDesktopUI
{
    class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            // sets the startup "form" as the ShellViewModel. This is the setup for Caliburn Micro
            // very similar actually to the startup code of the windows forms
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
