using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysPro
{
    internal class FileHelper
    {
        public static string GetNodeName(string NodePath)
        {
            return (new FileInfo(NodePath).Name).ToString();
        }

        public static string GetNodeVersion(string NodePath)
        {
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(NodePath);
            if (myFileVersionInfo.FileVersion != null)
            {
                return myFileVersionInfo.FileVersion.ToString();
            }
            else
                return "1.0.0.0";
        }

        public static string GetNodeLastUpdateTime(string NodePath)
        {
            return new FileInfo(NodePath).LastWriteTime.ToString();
        }

        public static void Serialize(string filePath, List<Node> nodes)
        {
            if(nodes.Count != 0)
            {
                StreamWriter sw = new StreamWriter(filePath);
                for (int i = 0; i < nodes.Count; i++)
                {
                    sw.Write("Имя файла:");
                    sw.WriteLine(nodes[i].Name);
                    sw.Write("Версия файла:");
                    sw.WriteLine(nodes[i].Version);
                    sw.Write("Время последнего обновления файла:");
                    sw.WriteLine(nodes[i].LastUpdateDate);
                    sw.Flush();
                }
                sw.Close();
            }
            else
            {
                File.WriteAllText(filePath, "");
            }
        }

        public static object Deserialize(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            List < Node > value = new List<Node>();
            string name;
            string version;
            string lastupdate;
            int v;
            while(sr.Peek() != -1) 
            {
                name = sr.ReadLine();
                v = name.LastIndexOf(':') + 1;
                name = name.Remove(0, v);
                version = sr.ReadLine();
                v = version.LastIndexOf(':') + 1;
                version = version.Remove(0, v);
                lastupdate = sr.ReadLine();
                v = lastupdate.IndexOf(':') + 1;
                lastupdate = lastupdate.Remove(0, v);
                Node n = new Node(name, version, lastupdate);
                value.Add(n);
            }
            sr.Close();
            return value;
        }

    }
}
