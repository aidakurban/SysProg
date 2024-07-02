using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyLib1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Generate();
        }
        public static void Generate()
        {
            string FunctionName = "LowLevelFunc";
            string assemblyName = FunctionName;
            string modName = FunctionName + ".dll";
            string typeName = "LowFuncDll";
            AssemblyName name = new AssemblyName(assemblyName);
            AppDomain domain = System.Threading.Thread.GetDomain();
            AssemblyBuilder builder = domain.DefineDynamicAssembly(
            name, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder module = builder.DefineDynamicModule
            (modName, true);
            TypeBuilder typeBuilder = module.DefineType(typeName,
            TypeAttributes.Public);
            MethodBuilder methodBuilderCompare = typeBuilder.DefineMethod(
            "Compare",
            MethodAttributes.Public,
            typeof(int),
            new Type[] { typeof(double), typeof(double) }
            );
            var iLGenerator = methodBuilderCompare.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_1);
            iLGenerator.Emit(OpCodes.Ldarg_2);
            iLGenerator.Emit(OpCodes.Ceq);
            iLGenerator.Emit(OpCodes.Ret);
            var ptType = typeBuilder.CreateType();
            builder.Save(assemblyName + ".dll");
        }
    }
}
