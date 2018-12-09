using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace ProductCalculation
{
    static class Program
    {
        //static MainForm _MainForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] arguments)
        {
            MainForm oForm = null;
            bool bInstanceFlag;

            Mutex MyApplicationMutex = new Mutex(true, "ProductCalculation_Mutex", out bInstanceFlag);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!bInstanceFlag)
            {
                oForm = new MainForm();
                SingleInstanceApplication.Run(oForm, NewInstanceHandler);
            }
            else
            {
                oForm = new MainForm(arguments);
                SingleInstanceApplication.Run(oForm, NewInstanceHandler);
                oForm.Close();
            }
        }

        public static void NewInstanceHandler(object sender, StartupNextInstanceEventArgs e)
        {
            if (sender != null && (e.CommandLine != null && e.CommandLine.Count > 0))
            {
                SingleInstanceApplication app = (SingleInstanceApplication)sender;
                MainForm oForm = (MainForm)app.OpenForms[0];

                e.BringToForeground = true;
                oForm.CallByProffix(e.CommandLine.ToArray());
            }
        }

        public class SingleInstanceApplication : WindowsFormsApplicationBase
        {
            private SingleInstanceApplication()
            {
                base.IsSingleInstance = true;
            }

            public static void Run(Form f, StartupNextInstanceEventHandler startupHandler)
            {
                SingleInstanceApplication app = new SingleInstanceApplication();
                app.MainForm = f;
                app.StartupNextInstance += startupHandler;
                app.Run(Environment.GetCommandLineArgs());
            }
        }
    }
}
