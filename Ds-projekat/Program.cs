using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ds_projekat
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var isConnected = DataBase.Instance.TestConnection(out var message);
            MessageBox.Show(
                message,
                isConnected ? "Povezivanje sa bazom" : "Greska pri povezivanju sa bazom",
                MessageBoxButtons.OK,
                isConnected ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            Application.Run(new LoginForm());
        }
    }
}
