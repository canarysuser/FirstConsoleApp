using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit; 

namespace FirstConsoleApp
{
    public class TestClass
    {
        public int IdField = 10; 
        public string NameProperty { get; set; } 
        public TestClass(int id)
        {
            Console.WriteLine($"Testclass.ctor(int) called.");
            IdField = id;
        }
        public void Show(string name)
        {
            NameProperty = name;
            Console.WriteLine($"IdField: {IdField}, NameProperty: {NameProperty}");
        }
    }

    internal class WorkingWithReflection
    {
        internal static void Test()
        {
            string line = "".PadLeft(50, '-');
            //Get the current executing assembly
            Assembly assembly = Assembly.GetExecutingAssembly();
            //Every assembly is a collection of modules -> Single Module or Multi-file assembly 
            Module mod = assembly.GetModule("FirstConsoleApp.dll")!;
            //Find the TestClass within the module 
            Type t = assembly.GetType(typeof(TestClass).FullName)!;
            Console.WriteLine($"Type: {t.FullName}\nParent Type: {t.BaseType.FullName}");
            Console.WriteLine($"Visibility: {(t.IsPublic? "Public": "Not Public")}");
            Console.WriteLine(line);

            var fields = t.GetFields().ToList();
            Console.WriteLine("\nFields: ");
            fields.ForEach(f =>
            {
                Console.WriteLine($"Name: {f.Name}, Type: {f.FieldType.FullName}, Attributes: {f.Attributes}");
            });
            Console.WriteLine(line);

            var properties=t.GetProperties().ToList();
            Console.WriteLine("\nProperties: ");
            properties.ForEach(p =>
            {
                Console.WriteLine($"Name: {p.Name}, Type: {p.PropertyType.FullName}");
            });
            Console.WriteLine(line);

            var methods = t.GetMethods().ToList();
            Console.WriteLine("\nMethods: ");
            methods.ForEach(m =>
            {
                Console.WriteLine($"Name: {m.Name}\n\tReturn Type: {m.ReturnType.FullName}");
                Console.WriteLine($"\tDeclaring Type: : {m.DeclaringType.FullName}");
            });
            Console.WriteLine(line);

            //Dynamically instantiate the types: 
            object obj = Activator.CreateInstance(type: t, args: 999); 

            //Invoke methods on the newly created type 
            var invocationAttrs = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public;

            t.InvokeMember(
                name: "Show",
                invokeAttr: invocationAttrs,
                binder: Type.DefaultBinder,
                target: obj,
                args: new[] { ".NET Core 9.0" });

            List<string> types = new List<string>()
            {
                typeof(WorkingWithDelegates1).FullName,
                typeof(WorkingWithDelegates2).FullName
            };
            Console.WriteLine("Choose the type to instantiate: ");
            int counter = 1; 
            for(int i=0; i<types.Count; i++)
            {
                Console.WriteLine($"{counter++}. {types[i]}");
            }
            Console.Write("Enter Choice: ");
            if(int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine($"Your Choice: {types[choice - 1]}"); 
                Type t1 = assembly.GetType(types[choice-1]);
                object obj1 = Activator.CreateInstance(t1);
                invocationAttrs = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
                t1.InvokeMember("Test", invocationAttrs, null, obj1, null);
            }
        }
        internal static void TestDynamicAssembly()
        {
            AssemblyName asmName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder asmBldr = AssemblyBuilder.DefineDynamicAssembly(
                name: asmName,
                access: AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder modBldr = asmBldr.DefineDynamicModule(name: "DynamicAssembly.dll"); 
            TypeBuilder tBldr = modBldr.DefineType(name: "DynamicClass", attr: TypeAttributes.Public);
            FieldBuilder fBldr = tBldr.DefineField(
                fieldName: "Id", 
                type: typeof(int), 
                attributes: FieldAttributes.Public);
            MethodBuilder mBldr = tBldr.DefineMethod(name: "Show", attributes: MethodAttributes.Public);
            //Include some IL instructions 
            ILGenerator il = mBldr.GetILGenerator();
            il.EmitWriteLine("Hello World!");
            il.Emit(OpCodes.Nop);
            var t = tBldr.CreateType();
            var obj = asmBldr.CreateInstance(t.FullName);
            t.InvokeMember("Show", BindingFlags.InvokeMethod| BindingFlags.Instance | BindingFlags.Public,
                null, obj, null);
           // asmBldr.Save("DynamicAssembly.dll");

            Console.WriteLine("Assembly created.");

        }
    }
}
