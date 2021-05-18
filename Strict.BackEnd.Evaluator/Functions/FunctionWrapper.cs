using System;
using System.Threading;
using Strict.Context;
using Strict.Language;

namespace Strict.BackEnd.Evaluator.Functions
{
	public class FunctionWrapper
	{
		protected IFunction Function { get; }
		protected IContext Context { get; }

		public FunctionWrapper(IFunction function, IContext context)
		{
			Function = function;
			Context = context;
		}

		public virtual ThreadStart CreateThreadStart() => DoAction;

		public virtual Delegate CreateActionDelegate() =>
			Delegate.CreateDelegate(typeof(Action), this, "DoAction");

		public virtual Delegate CreateFunctionDelegate() =>
			Delegate.CreateDelegate(typeof(Func<object>), this, "DoFunction");

		private object DoFunction() => Function.Visit(Context, null, null);

		private void DoAction() => Function.Visit(Context, null, null);
	}

	public class FunctionWrapper<TR, TD> : FunctionWrapper
	{
		public FunctionWrapper(IFunction function, IContext context) : base(function, context) { }

		public override Delegate CreateFunctionDelegate() =>
			Delegate.CreateDelegate(typeof(TD), this, "DoFunction");

		public override Delegate CreateActionDelegate() =>
			Delegate.CreateDelegate(typeof(TD), this, "DoAction");

		public TR DoFunction() => (TR)Function.Visit(Context, null, null);

		public void DoAction() => Function.Visit(Context, null, null);
	}

	public class FunctionWrapper<T1, TR, TD> : FunctionWrapper
	{
		public FunctionWrapper(IFunction function, IContext context) : base(function, context) { }

		public override Delegate CreateFunctionDelegate() =>
			Delegate.CreateDelegate(typeof(TD), this, "DoFunction");

		public override Delegate CreateActionDelegate() =>
			Delegate.CreateDelegate(typeof(TD), this, "DoAction");

		public TR DoFunction(T1 t1) => (TR)Function.Visit(Context, new object[] { t1 }, null);

		public void DoAction(T1 t1) => Function.Visit(Context, new object[] { t1 }, null);
	}

	public class FunctionWrapper<T1, T2, TR, TD> : FunctionWrapper
	{
		public FunctionWrapper(IFunction function, IContext context) : base(function, context) { }

		public override Delegate CreateFunctionDelegate() =>
			Delegate.CreateDelegate(typeof(TD), this, "DoFunction");

		public override Delegate CreateActionDelegate() =>
			Delegate.CreateDelegate(typeof(TD), this, "DoAction");

		public TR DoFunction(T1 t1, T2 t2) =>
			(TR)Function.Visit(Context, new object[] { t1, t2 }, null);

		public void DoAction(T1 t1, T2 t2) => Function.Visit(Context, new object[] { t1, t2 }, null);
	}

	public class FunctionWrapper<T1, T2, T3, TR, TD> : FunctionWrapper
	{
		public FunctionWrapper(IFunction function, IContext context) : base(function, context) { }

		public override Delegate CreateFunctionDelegate() =>
			Delegate.CreateDelegate(typeof(TD), this, "DoFunction");

		public override Delegate CreateActionDelegate() =>
			Delegate.CreateDelegate(typeof(TD), this, "DoAction");

		public TR DoFunction(T1 t1, T2 t2, T3 t3) =>
			(TR)Function.Visit(Context, new object[] { t1, t2, t3 }, null);

		public void DoAction(T1 t1, T2 t2, T3 t3) =>
			Function.Visit(Context, new object[] { t1, t2, t3 }, null);
	}
}