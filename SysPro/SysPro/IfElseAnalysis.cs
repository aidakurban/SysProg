using Microsoft.CSharp;
using SysPro.Event;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysPro
{
    public delegate void IterationCounter();
    public delegate void ErrorHandler(object msg);

    public class IfElseAnalysis
    {
        private string trueProgram;
        public static IterationCounter OnIteration;
        public bool HasError { get; set; }
        public event EventHandler<ErrorThrowedEventArgs> OnErrorThrowed;
        private List<string> refferences = new List<string>();

        public List<string> Refferences
        {
            get { return refferences; }
            set { refferences = value; }
        }


        private readonly string header = @"
            using System;
            using System.IO;
            namespace IfElseCheck
            {
                public class Program
                {
                    public static void Main()
                    { 
                        ";

        private readonly string footer = @"
                    }
                    static void Count()
                    {
                        if(SysPro.IfElseAnalysis.OnIteration != null)
                            SysPro.IfElseAnalysis.OnIteration();
                    }
                }
            }";


        public IfElseAnalysis(IterationCounter onIteration)
        {
            OnIteration += onIteration;
            refferences.AddRange(new string[]
                 {
                    "System.dll",
                    "System.Core.dll",
                    Assembly.GetAssembly(typeof(IfElseAnalysis)).Location,
                 });

        }

        public void Execute()
        {
            Execute(trueProgram);
        }

        public void Execute(string program)
        {
            HasError = false;
            var CSHarpProvider = CSharpCodeProvider.CreateProvider("CSharp");
            CompilerParameters compilerParams = new CompilerParameters()
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
            };
            compilerParams.ReferencedAssemblies.AddRange(refferences.ToArray());
            var compilerResult = CSHarpProvider.CompileAssemblyFromSource(compilerParams, program);
            if (!compilerResult.Errors.HasErrors)
            {
                try
                {
                    Task task = Task.Run(() =>
                    compilerResult.CompiledAssembly.GetType("IfElseCheck.Program").GetMethod("Main").Invoke(null, null));
                    if (!task.Wait(5000)) // Задержка в миллисекундах
                    {
                        OnErrorThrowed?.Invoke(this, new ErrorThrowedEventArgs("stack overflow exception"));
                    }
                }
                catch (Exception e)
                {
                    OnErrorThrowed?.Invoke(this, new ErrorThrowedEventArgs(e.InnerException.Message));
                }
            }
            else
            {
                OnErrorThrowed?.Invoke(this, new ErrorThrowedEventArgs(compilerResult.Errors[0].ErrorText));
            }
        }

        public string FormatSources(string text)
        {
            try
            {
                string elseText = text;
                string s = text.Substring(text.IndexOf("if")); // То что начиная с if
                text = text.Substring(0, text.IndexOf("if"));// То что до if
                s = s.Insert(s.IndexOf("}"), "Count();");
                text += s;
                if (text.IndexOf("else") != -1)
                {
                    string str = text.Substring(text.IndexOf("else")); // То что начиная с else
                    text = text.Substring(0, text.IndexOf("else"));// То что до else
                    str = str.Insert(str.IndexOf("}"), "Count();\nCount();");
                    text += str;
                }
                
            }
            catch (Exception e)
            {
                OnErrorThrowed?.Invoke(this, new ErrorThrowedEventArgs("Ожидался if"));
                text = "";
            }
            trueProgram = string.Concat(header, text, footer);

            return trueProgram;
        }

        public string FormatSources(List<string> code)
        {

            StringBuilder sb = new StringBuilder(header);

            foreach (var sc in code)
            {
                sb.AppendLine(sc);
            }
            sb.AppendLine(footer);

            trueProgram = sb.ToString();
            return trueProgram;
        }
    }
}
