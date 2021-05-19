using Strict.Compiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Strict.Context;
using Strict.Language;
using Strict.Language.Commands;

namespace Strict.BackEnd.Evaluator.Utilities
{
  public static class ModuleUtilities
    {
        private static IDictionary<string, Module> Modules { get; } = new Dictionary<string, Module>();

        public static string ModuleFileName(string name)
        {
            string dirname = name.Replace('.', '/');
            string filename = dirname + ".strict";

            string fullfilename = Path.Combine(".", filename);

            if (File.Exists(fullfilename))
                return (new FileInfo(fullfilename)).FullName;
						
            string location = (new FileInfo(System.Reflection.Assembly.GetAssembly(typeof(ModuleUtilities)).Location)).DirectoryName;
            string lib = Path.Combine(location, "Lib");

            fullfilename = Path.Combine(lib, filename);

            if (File.Exists(fullfilename))
                return (new FileInfo(fullfilename)).FullName;
						
            location = (new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)).DirectoryName;
            lib = Path.Combine(location, "Lib");

            fullfilename = Path.Combine(lib, filename);

            return File.Exists(fullfilename)
							? (new FileInfo(fullfilename)).FullName
							: null;
				}

        public static Module LoadModule(string name, IContext context, IVisitor visitor)
        {
            Module module = null;

            if (TypeUtilities.IsNamespace(name))
            {
                var types = TypeUtilities.GetTypesByNamespace(name);

                module = new Module(context.GlobalContext);

                foreach (var type in types)
                    module.SetValue(type.Name, type);
            }
            else
            {
                string filename = ModuleUtilities.ModuleFileName(name);

                if (filename == null)
                    throw new Exception($"No module named {name}");

                if (Modules.ContainsKey(filename) && Modules[filename].GlobalContext == context.GlobalContext)
                    return Modules[filename];

                var parser = new Parser(new StreamReader(filename));
                var command = parser.CompileCommandList();

                module = new Module(context.GlobalContext);
                var doc = CommandUtilities.GetDocString(command);

                command.Accept(visitor, module);
                module.SetValue("__documentation_", doc);

                Modules[filename] = module;
            }

            return module;
        }
    }
}
