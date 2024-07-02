using SysPro.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysPro
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            View view = new View();
            IView iview = view;
            IModel model = new Model();
            Presenter presenter = new Presenter(model, iview);
            try
            {
                Application.Run(view);
            }
            catch (Exception e)
            {
               Console.WriteLine(e.StackTrace);
            }

        }
    }
}
