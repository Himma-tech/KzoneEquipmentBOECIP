using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using KZONE.MessageManager;
using KZONE.Work;

namespace KZONE.Launcher
{
    static class StartServer
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Mutex mutex = null;
            Workbench workbench = null;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {

                SplashScreenForm.ShowSplashScreen();
                Workbench.WorkbenchInitialStatus += new WorkbenchInitialStatusEventHandler(OnWorkbenchInitialStatus);
                if (args.Length == 0)
                    Workbench.Instance.Init(@"..\Config\startup.xml");
                else
                    Workbench.Instance.Init(args[0]);

                workbench = Workbench.Instance;
                bool isAppRunning = false;
                try
                {
                    mutex = new Mutex(true, "Global\\" + Workbench.ServerName, out isAppRunning);
                }
                catch
                {
                }

                if (!isAppRunning)
                {
                    MessageBox.Show(string.Format("{0} is already running, can't open again!", Workbench.ServerName), @"Execute Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                    Environment.Exit(1);
                }

                workbench.Run();

                SplashScreenForm.SplashScreen.BeginInvoke(new MethodInvoker(SplashScreenForm.SplashScreen.Dispose));
                SplashScreenForm.SplashScreen = null;

                Application.Run(workbench.MainForm);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Workbench Init Fail Please check Configration.\n" + ex.ToString(), "Initial", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Environment.Exit(0);
            }
            finally
            {
                if (mutex != null)
                    mutex.ReleaseMutex();
                if (SplashScreenForm.SplashScreen != null)
                {
                    SplashScreenForm.SplashScreen.Dispose();
                }
                if (workbench != null)
                    workbench.Shutdown();
                Environment.Exit(0);
            }



        }

        static void OnWorkbenchInitialStatus(string msg, bool isStart)
        {
            //SplashScreenForm.SplashScreen.BeginInvoke(new UpdateLableTextEventHandler(SplashScreenForm.SplashScreen.UpdateInitInformation),new object[]{msg}];
            if (isStart)
                SplashScreenForm.SplashScreen.UpdateInitInformation(msg);
            else
                SplashScreenForm.SplashScreen.UpdateServerName(msg);
        }
    }
}
