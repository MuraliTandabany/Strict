using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Strict.Context;
using Strict.Evaluator.Functions;
using Strict.Language;

namespace Strict.Evaluator.Utilities
{
	public class ObjectUtilities
	{
		public static void SetValue(object obj, string name, object value)
		{
			if (obj is IValues)
			{
				((IObject)obj).SetValue(name, value);
				return;
			}
			var type = obj.GetType();
			type.InvokeMember(name,
				BindingFlags.SetProperty | BindingFlags.SetField | BindingFlags.IgnoreCase |
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, obj,
				new[] { value });
		}

		public static object GetValue(object obj, string name)
		{
			if (obj is IValues)
				return ((IValues)obj).GetValue(name);
			var type = obj.GetType();
			try
			{
				return type.InvokeMember(name, BindingFlags.GetProperty | BindingFlags.GetField |
					BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Public |
					BindingFlags.NonPublic | /* System.Reflection.BindingFlags.InvokeMethod | */
					BindingFlags.Instance, null, obj, null);
			}
			catch
			{
				return type.GetMethod(name);
			}
		}

		public static object GetValue(object obj, string name, IList<object> arguments) =>
			obj switch
			{
				IObject iObject when arguments == null => iObject.GetValue(name),
				IObject iObject => iObject.Invoke(name, null, arguments, null),
				_ => obj is IValues && (arguments == null || arguments.Count == 0)
					? ((IValues)obj).GetValue(name)
					: GetNativeValue(obj, name, arguments)
			};

		public static IList<string> GetNames(object obj) => TypeUtilities.GetNames(obj.GetType());

		public static object GetNativeValue(object obj, string name, IList<object> arguments)
		{
			var type = obj.GetType();
			return type.InvokeMember(name,
				BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.IgnoreCase |
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod |
				BindingFlags.Instance, null, obj, arguments == null
					? null
					: arguments.ToArray());
		}

		public static bool IsNumber(object obj) =>
			obj is int || obj is short || obj is long || obj is decimal || obj is double ||
			obj is float || obj is byte;

		public static object GetIndexedValue(object obj, object[] indexes) =>
			obj switch
			{
				Array array => GetIndexedValue(array, indexes),
				IList list => GetIndexedValue(list, indexes),
				IDictionary dictionary => GetIndexedValue(dictionary, indexes),
				DynamicObject dynamicObject when indexes != null && indexes.Length == 1 => dynamicObject.
					GetValue((string)indexes[0]),
				_ => GetValue(obj, string.Empty, indexes)
			};

		public static void SetIndexedValue(object obj, object[] indexes, object value)
		{
			if (obj is Array)
			{
				SetIndexedValue((Array)obj, indexes, value);
				return;
			}
			if (obj is IList)
			{
				if (indexes.Length != 1)
					throw new InvalidOperationException("Invalid number of sub-indices");
				var index = (int)indexes[0];
				var list = (IList)obj;
				if (list.Count == index)
					list.Add(value);
				else
					list[index] = value;
				return;
			}
			if (obj is IDictionary)
			{
				if (indexes.Length != 1)
					throw new InvalidOperationException("Invalid number of sub-indices");
				((IDictionary)obj)[indexes[0]] = value;
				return;
			}
			throw new InvalidOperationException($"Not indexed value of type {obj.GetType()}");
		}

		public static void SetIndexedValue(Array array, object[] indexes, object value)
		{
			switch (indexes.Length)
			{
			case 1:
				array.SetValue(value, (int)indexes[0]);
				return;
			case 2:
				array.SetValue(value, (int)indexes[0], (int)indexes[1]);
				return;
			case 3:
				array.SetValue(value, (int)indexes[0], (int)indexes[1], (int)indexes[2]);
				return;
			default:
				throw new InvalidOperationException("Invalid number of sub-indices");
			}
		}

		public static void AddHandler(object obj, string eventName, IFunction function,
			IContext context)
		{
			var type = obj.GetType();
			var @event = type.GetEvent(eventName);
			var invoke = @event.EventHandlerType.GetMethod("Invoke");
			var parameters = invoke.GetParameters();
			var numberOfParameters = parameters.Count();
			var types = new Type[numberOfParameters + 1];
			Type wrapperType = null;
			var partypes = new Type[numberOfParameters + 2];
			var returnType = invoke.ReturnParameter.ParameterType;
			var isAction = returnType.FullName == "System.Void";
			if (isAction)
				returnType = typeof(int);
			switch (numberOfParameters)
			{
			case 0:
				partypes[0] = returnType;
				partypes[1] = @event.EventHandlerType;
				wrapperType = typeof(FunctionWrapper<,>).MakeGenericType(partypes);
				break;
			case 1:
				partypes[0] = parameters.ElementAt(0).ParameterType;
				partypes[1] = returnType;
				partypes[2] = @event.EventHandlerType;
				wrapperType = typeof(FunctionWrapper<,,>).MakeGenericType(partypes);
				break;
			case 2:
				partypes[0] = parameters.ElementAt(0).ParameterType;
				partypes[1] = parameters.ElementAt(1).ParameterType;
				partypes[2] = returnType;
				partypes[3] = @event.EventHandlerType;
				wrapperType = typeof(FunctionWrapper<,,,>).MakeGenericType(partypes);
				break;
			case 3:
				partypes[0] = parameters.ElementAt(0).ParameterType;
				partypes[1] = parameters.ElementAt(1).ParameterType;
				partypes[2] = parameters.ElementAt(2).ParameterType;
				partypes[3] = returnType;
				partypes[4] = @event.EventHandlerType;
				wrapperType = typeof(FunctionWrapper<,,,,>).MakeGenericType(partypes);
				break;
			}
			var wrapper = Activator.CreateInstance(wrapperType, function, context);
			@event.AddEventHandler(obj, (Delegate)GetValue(wrapper, isAction
				? "CreateActionDelegate"
				: "CreateFunctionDelegate", null));
		}

		private static object GetIndexedValue(Array array, object[] indexes) =>
			indexes.Length switch
			{
				1 => array.GetValue((int)indexes[0]),
				2 => array.GetValue((int)indexes[0], (int)indexes[1]),
				3 => array.GetValue((int)indexes[0], (int)indexes[1], (int)indexes[2]),
				_ => throw new InvalidOperationException("Invalid number of subindices"),
			};

		private static object GetIndexedValue(IList list, object[] indexes) =>
			indexes.Length != 1
				? throw new InvalidOperationException("Invalid number of subindices")
				: list[(int)indexes[0]];

		private static object GetIndexedValue(IDictionary dictionary, object[] indexes) =>
			indexes.Length != 1
				? throw new InvalidOperationException("Invalid number of subindices")
				: dictionary[indexes[0]];
	}
}