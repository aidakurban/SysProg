using SysPro.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SysPro
{
    public partial class View : Form, IView
    {
        private string filePath = "";

        public event EventHandler<FileInitEventArgs> OnFileInitialized;
        public event EventHandler<NodeAddedEventArgs> OnNodeAdded;
        public event Action OnFileSaved;
        public event EventHandler<NodeRemovedEventArgs> OnNodeRemoved;
        public event EventHandler<NodeEditedEventArgs> OnNodeEdited;

        public event Action OnLogsOpened;


        public event EventHandler<LowLevelEventArgs> LowLevelStarted;


        public event EventHandler<AnalysisEventArgs> OnAnalysisStarted;


        public View()
        {
            InitializeComponent();
        }

        public void UpdateNodesList(List<Node> nodes)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = nodes;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        /// <summary>
        /// Открытие файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileBtn_Click(object sender, EventArgs e)
        {
            filePath = GetOpenedFilePath("txt файлы (*.txt)|*.txt");
            if (filePath != null)
            {
                OnFileInitialized?.Invoke(this, new FileInitEventArgs(filePath));
                fileNameTextBox.Text = filePath.Split('\\').Last();
                AddNoteButton.Enabled = true;
                DeleteNodeButton.Enabled = true;    
            }
        }

        /// <summary>
        /// Получение пути открытого файла
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string GetOpenedFilePath(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = filter
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        /// <summary>
        /// Сохранение файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            OnFileSaved?.Invoke();
        }

        /// <summary>
        /// Добавления в файл 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNoteButton_Click(object sender, EventArgs e)
        {
            string nodeName = GetOpenedFilePath("Все файлы (*.*)|*.*");
            if (nodeName != null)
            {
                OnNodeAdded?.Invoke(this, new NodeAddedEventArgs(nodeName));
                MessageBox.Show(filePath+ "FILEPATH AFTER ONNODEADDED");
                OnFileSaved?.Invoke();
                MessageBox.Show(filePath + "FILEPATH AFTER ONFILESAVED");
                OnFileInitialized?.Invoke(this, new FileInitEventArgs(filePath));
                MessageBox.Show(filePath + "FILEPATH AFTER ONFILEINIT");
            }
        }
        /// <summary>
        /// Удаление из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteNodeButton_Click(object sender, EventArgs e)
        {
            Node node = dataGridView1.SelectedCells[0].OwningRow.DataBoundItem as Node;
            OnNodeRemoved?.Invoke(this, new NodeRemovedEventArgs(node));
            OnFileSaved?.Invoke();
        }

        /// <summary>
        /// При изменение записи в файле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Node node = dataGridView1.SelectedCells[0].OwningRow.DataBoundItem as Node;
            OnNodeEdited?.Invoke(this, new NodeEditedEventArgs(node));
            OnFileSaved?.Invoke();

        }

        /// <summary>
        /// При создании файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFileButton_Click(object sender, EventArgs e)
        {
            filePath = GetCreatedFilePath();

            if (filePath != null)
            {
                OnFileInitialized?.Invoke(this, new FileInitEventArgs(filePath));
                fileNameTextBox.Text = filePath.Split('\\').Last();
                AddNoteButton.Enabled = true;
                DeleteNodeButton.Enabled = true;
            }
        }
        /// <summary>
        /// Создание файла и получение его пути
        /// </summary>
        /// <returns></returns>
        private string GetCreatedFilePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "txt файлы (*.txt)|*.txt",
                DefaultExt = ".txt"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.Create(saveFileDialog.FileName).Close();
                return saveFileDialog.FileName;
            }
            return null;
        }


        public void ShowLogs(string logs)
        {
            logBox.Text = logs;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage4)
                OnLogsOpened?.Invoke();
        }

        /// <summary>
        /// Работа с низкоуровневой функцией
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lowLvlButton_Click(object sender, EventArgs e)
        {
            string InputValue1 = lowLvlFirst.Text;
            string InputValue2 = lowLvlSecond.Text;
            InputValue1 = InputValue1.Replace('.', ',');
            InputValue2 = InputValue2.Replace('.', ',');
            LowLevelStarted?.Invoke(this, new LowLevelEventArgs(InputValue1, InputValue2));
        }

        /// <summary>
        /// Вывод ошибки
        /// </summary>
        /// <param name="message"></param>
        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Результат работы низкоуровневой функции
        /// </summary>
        /// <param name="result"></param>
        public void ShowLowLvlResult(string result)
        {
            if(result.Equals("1"))
                lowLvlResultLabel.Text = "Равны";
            else
                lowLvlResultLabel.Text = "Не равны";
        }



        private void AnalysisButton_Click(object sender, EventArgs e)
        {
            OnAnalysisStarted?.Invoke(this, new AnalysisEventArgs(CodeBox.Text));
        }
        public void ShowResult(string result)
        {
            if (result.Equals("1"))
                ResultLabel.Text = "if";
            else
            {
                if (result.Equals("2"))
                    ResultLabel.Text = "else";
                else
                    ResultLabel.Text = "Ничего";
            }
        }

        private void AdviceButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В коде должен быть хотя бы if\nПосле условия if и else обязательны {*}");
        }
    }
}
