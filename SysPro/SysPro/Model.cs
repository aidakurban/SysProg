using SysPro.Event;
using SysPro.Interface;
using SysPro.Logs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysPro
{
    public class Model : IModel
    {
        public string FilePath { get; set; }
        public List<Node> Nodes { get; set; }


        private Logi log;

        public double InputValue1 { get; set; }
        public double InputValue2 { get; set; }
        public int OutputValue { get; set; }
        public Assembly Asm { get; set; }
        public Type Type { get; set; }
        public MethodInfo Method { get; set; }
        public object Instance { get; set; }


        public int IterationsCount { get; set; }
        public event EventHandler<ErrorThrowedEventArgs> OnErrorThrowed;

        public Model()
        {
            log = new Logi();
            Asm = Assembly.Load(File.ReadAllBytes("LowLevelFunc.dll"));
            Type = Asm.GetType("LowFuncDll", true);
            Method = Type.GetMethod("Compare", BindingFlags.Instance | BindingFlags.Public);
            Instance = Activator.CreateInstance(Type);
            loopAnalyzer = new IfElseAnalysis(OnIteration);
            loopAnalyzer.OnErrorThrowed += OnError;

        }

        /// <summary>
        /// Добавление лога в список
        /// </summary>
        /// <param name="info"></param>
        public void AddLog(string info)
        {
            log.AddLog(info);
        }

        /// <summary>
        /// Получение списка логов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Log> GetLogs()
        {
            return log.Log;
        }

        /// <summary>
        /// Инициализация файла
        /// </summary>
        public void InitFile()
        {
            object deserialized = FileHelper.Deserialize(FilePath);

            if (deserialized != null)
                Nodes = (List<Node>)deserialized;
            else
                Nodes = new List<Node>();
        }

        /// <summary>
        /// Сохранения файла
        /// </summary>
        public void SaveFile()
        {
            if (Nodes != null)
            {
                FileHelper.Serialize(FilePath, Nodes);
            }
        }

        /// <summary>
        /// Добавления записи
        /// </summary>
        /// <param name="name">Имя файла</param>
        /// <param name="lastUpdateTime">Время последнего обновления</param>
        /// <param name="version">Версия файла</param>
        public void AddNode(string name, string lastUpdateTime, string version)
        {
            Node node = new Node()
            {
                Name = name,
                LastUpdateDate = lastUpdateTime,
                Version = version
            };
            Nodes.Add(node);
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="node">Запись</param>
        public void RemoveNode(Node node)
        {
            Nodes.Remove(node);
        }

        /// Изменение записи
        /// </summary>
        /// <param name="node">Запись</param>
        public void EditNode(Node node)
        {
            int index = Nodes.IndexOf(node);
            Nodes[index] = node;
        }


        /// <summary>
        /// Вызов низкоуровневой функции
        /// </summary>
        public void LowLevel()
        {
            OutputValue = (int)Method.Invoke(Instance, new object[] { InputValue1, InputValue2 });
        }


        private IfElseAnalysis loopAnalyzer;

        /// <summary>
        /// Запуск анализатора
        /// </summary>
        /// <param name="loopText">Текст для анализа</param>
        public void ToAnalyze(string loopText)
        {
            IterationsCount = 0;
            loopAnalyzer.FormatSources(loopText);
            loopAnalyzer.Execute();
        }
        private void OnIteration()
        {
            IterationsCount++;
        }
        private void OnError(object sender, ErrorThrowedEventArgs e)
        {
            this.OnErrorThrowed?.Invoke(this, new ErrorThrowedEventArgs(e.ErrorMessage));
        }

    }
}
