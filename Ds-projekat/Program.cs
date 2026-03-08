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

            try
            {
                AppConfig.Instance.Load("config.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju config.txt fajla:\n" + ex.Message, "Config error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Application.Run(new LoginForm());

        }
    }
}
