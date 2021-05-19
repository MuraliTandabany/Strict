using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Strict.Compiler;
using Strict.Context;
using Strict.Language;

namespace Strict.BackEnd.Evaluator.Utilities
{
	public static class ModuleUtilities
	{
		private static IDictionary<string, Module> Modules { get; } = new Dictionary<string, Module>();

		public static string ModuleFileName(string name)
		{
			var dirname = name.Replace('.', '/');
			var filename = dirname + ".strict";
			var fullfilename = Path.Combine(".", filename);
			if (File.Exists(fullfilename))
				return new FileInfo(fullfilename).FullName;
			var location = new FileInfo(Assembly.GetAssembly(typeof(ModuleUtilities)).Location).
				DirectoryName;
			var lib = Path.Combine(location, "Lib");
			fullfilename = Path.Combine(lib, filename);
			if (File.Exists(fullfilename))
				return new FileInfo(fullfilename).FullName;
			location = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
			lib = Path.Combine(location, "Lib");
			fullfilename = Path.Combine(lib, filename);
			return File.Exists(fullfilename)
				? new FileInfo(fullfilename).FullName
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
				var filename = ModuleFileName(name);
				if (filename == null)
					throw new Exception($"No module named {name}");
				if (Modules.ContainsKey(filename) &&
					Modules[filename].GlobalContext == context.GlobalContext)
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