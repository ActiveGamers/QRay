using SQLitePCL;

namespace QRay
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Batteries.Init();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}