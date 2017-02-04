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
        public static void Main()
        {
            App app = new App();
            var window = new MainWindow();
            app.Run(window);
        }
    }
}
