using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Strict.Context;
using Strict.Exceptions;

namespace Strict.Evaluator.Utilities
{
	public class TypeUtilities
	{
		private static bool referencedAssembliesLoaded;

		public static Type GetType(IContext context, string name)
		{
			var obj = context.GetValue(name);
			if (obj != null || context.GlobalContext == null || context.GlobalContext == context)
				return obj != null && obj is Type
					? (Type)obj
					: GetType(name);
			obj = context.GlobalContext.GetValue(name);
			return obj != null && obj is Type
				? (Type)obj
				: GetType(name);
		}

		public static Type AsType(string name)
		{
			var type = Type.GetType(name);
			if (type != null)
				return type;
			type = GetTypeFromLoadedAssemblies(name);
			if (type != null)
				return type;
			type = GetTypeFromPartialNamedAssembly(name);
			if (type == null)
			{
				LoadReferencedAssemblies();
				type = GetTypeFromLoadedAssemblies(name);
				return type ?? null;
			}
			return type;
		}

		public static Type GetType(string name) =>
			AsType(name) ?? throw new InvalidOperationException($"Unknown type '{name}'");

		public static ICollection<Type> GetTypesByNamespace(string @namespace)
		{
			LoadReferencedAssemblies();
			return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly =>
				assembly.GetTypes().Where(tp => tp.Namespace == @namespace)).ToList();
		}

		public static bool IsNamespace(string name) => GetNamespaces().Contains(name);

		public static IList<string> GetNames(Type type) =>
			type.GetMembers(BindingFlags.Public | BindingFlags.Instance).Select(m => m.Name).ToList();

		public static object GetValue(Type type, string name)
		{
			try
			{
				return type.InvokeMember(name,
					BindingFlags.FlattenHierarchy | BindingFlags.GetProperty | BindingFlags.GetField |
					BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic |
					BindingFlags.Static, null, null, null);
			}
			catch
			{
				return type.GetMethod(name);
			}
		}

		public static object InvokeTypeMember(Type type, string name, IList<object> parameters) =>
			type.InvokeMember(name,
				BindingFlags.FlattenHierarchy | BindingFlags.GetProperty | BindingFlags.GetField |
				BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic |
				BindingFlags.InvokeMethod | BindingFlags.Static, null, null, parameters == null
					? null
					: parameters.ToArray());

		public static object ParseEnumValue(Type type, string name)
		{
			var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0, count = fields.Length; i < count; i++)
			{
				var fi = fields[i];
				if (fi.Name == name)
					return fi.GetValue(null);
			}
			throw new ValueErrorException($"'{name}' is not a valid value of '{type.Name}'");
		}

		private static ICollection<string> GetNamespaces()
		{
			var namespaces = new List<string>();
			LoadReferencedAssemblies();
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			foreach (var type in assembly.GetTypes())
				if (!namespaces.Contains(type.Namespace))
					namespaces.Add(type.Namespace);
			return namespaces;
		}

		private static Type GetTypeFromPartialNamedAssembly(string name)
		{
			var p = name.LastIndexOf(".");
			if (p < 0)
				return null;
			var assemblyName = name.Substring(0, p);
			try
			{
				var assembly = Assembly.Load(assemblyName);
				return assembly.GetType(name);
			}
			catch
			{
				return null;
			}
		}

		private static Type GetTypeFromLoadedAssemblies(string name) =>
			AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetType(name)).
				FirstOrDefault(type => type != null);

		private static void LoadReferencedAssemblies()
		{
			if (referencedAssembliesLoaded)
				return;
			var loaded = AppDomain.CurrentDomain.GetAssemblies().
				Select(assembly => assembly.GetName().Name).ToList();
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
				LoadReferencedAssemblies(assembly, loaded);
			referencedAssembliesLoaded = true;
		}

		private static void LoadReferencedAssemblies(Assembly assembly, List<string> loaded)
		{
			foreach (var referenced in assembly.GetReferencedAssemblies())
				if (!loaded.Contains(referenced.Name))
				{
					loaded.Add(referenced.Name);
					var newassembly = Assembly.Load(referenced);
					LoadReferencedAssemblies(newassembly, loaded);
				}
		}
	}
}