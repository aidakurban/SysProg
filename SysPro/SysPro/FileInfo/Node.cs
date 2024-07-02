using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysPro
{
    [Serializable]
    public class Node
    {
        [DisplayName("Имя файла")]
        public string Name { get; set; }
        [DisplayName("Версия файла")]
        public string Version { get; set; }
        [DisplayName("Дата последнего редактирования")]
        public string LastUpdateDate { get; set; }

        public Node() { }

        public Node(string name, string version, string lastUpdateDate)
        {
            Name = name;
            Version = version;
            LastUpdateDate = lastUpdateDate;
        }
    }
}
