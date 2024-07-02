using SysPro.Event;
using SysPro.Interface;
using SysPro.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysPro
{
    public class Presenter
    {
        private readonly IModel model;
        private readonly IView view;

        public Presenter(IModel model, IView view)
        {
            this.model = model;
            this.view = view;
            InitPresenter();
        }

        private void InitPresenter()
        {
            view.LowLevelStarted += View_LowLevelStarted;

            view.OnFileInitialized += View_OnFileInitialized;
            view.OnFileSaved += View_OnFileSaved;
            view.OnNodeAdded += View_OnNodeAdded;
            view.OnNodeEdited += View_OnNodeEdited;
            view.OnNodeRemoved += View_OnNodeRemoved;
            view.OnAnalysisStarted += view_OnAnalysisStarted;
            model.OnErrorThrowed += model_OnErrorThrowed;
            view.OnLogsOpened += View_OnLogsOpened;
        }





        /// <summary>
        /// При открытии файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_OnFileInitialized(object sender, FileInitEventArgs e)
        {
            model.FilePath = e.FilePath;
            model.InitFile();

            view.UpdateNodesList(model.Nodes);
            model.AddLog("Новый файл успешно проинициализирован");

        }

        /// <summary>
        /// При сохранение файла
        /// </summary>
        private void View_OnFileSaved()
        {
            model.SaveFile();
            model.AddLog("Файл успешно сохранён");

        }
        /// <summary>
        /// При добавлении записи
        /// </summary>
        private void View_OnNodeAdded(object sender, NodeAddedEventArgs e)
        {
            string name = FileHelper.GetNodeName(e.NodePath);
            string lastUpdateTime = FileHelper.GetNodeLastUpdateTime(e.NodePath);
            string version = FileHelper.GetNodeVersion(e.NodePath);
            model.AddNode(name, lastUpdateTime, version);
            view.UpdateNodesList(model.Nodes);

            model.AddLog("В файл добавлена новая запись");

        }

        /// <summary>
        /// При удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_OnNodeRemoved(object sender, NodeRemovedEventArgs e)
        {
            model.RemoveNode(e.SelectedNode as Node);
            view.UpdateNodesList(model.Nodes);
            model.AddLog("Из файла успешно удалена запись");
        }
        /// <summary>
        /// При изменении записи
        /// </summary>
        private void View_OnNodeEdited(object sender, NodeEditedEventArgs e)
        {
            model.EditNode(e.Node as Node);
            model.AddLog("Изменение клетки произошло успешно!");
        }




        /// <summary>
        /// При открытие вкладки логов
        /// </summary>
        private void View_OnLogsOpened()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Log log in model.GetLogs())
            {
                sb.Append(string.Format("<{0}> | <{1}>" + Environment.NewLine, log.Time, log.Info));
            }
            view.ShowLogs(sb.ToString());
        }


        /// <summary>
        /// При нажатии кнопки старта низкоуровненой функции
        /// </summary>
        private void View_LowLevelStarted(object sender, LowLevelEventArgs e)
        {
            try
            {
                model.AddLog("Вызов низкоуровневой функции");
                model.InputValue1 = Convert.ToDouble(e.InputValue1);
                model.InputValue2 = Convert.ToDouble(e.InputValue2);
                model.LowLevel();
                view.ShowLowLvlResult(model.OutputValue.ToString());
                model.AddLog("Вызов низкоуровневой функции прошёл успешно");
            }
            catch (Exception ex)
            {
                view.ShowErrorMessage("Ошибка в низкоуровневой функции");
                model.AddLog("Ошибка в низкоуровневой функции");
            }
        }


        /// <summary>
        /// При нажатии кнопки старта анализа цикла
        /// </summary>
        private void view_OnAnalysisStarted(object sender, AnalysisEventArgs e)
        {
            model.AddLog("Вызов анализатора цикла");
            model.ToAnalyze(e.LoopCode);
            view.ShowResult(model.IterationsCount.ToString());
        }
        /// <summary>
        /// При ошибки цикла
        /// </summary>
        private void model_OnErrorThrowed(object sender, ErrorThrowedEventArgs e)
        {
            view.ShowErrorMessage("Ошибка в работе анализатора цикла: " + e.ErrorMessage);
            model.AddLog("Ошибка в работе анализатора цикла: " + e.ErrorMessage);
        }

    }
}
