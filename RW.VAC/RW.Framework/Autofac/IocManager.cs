using Autofac;

namespace RW.Framework.Autofac;

public class IocManager
{
	private static readonly Lazy<IocManager> LazyIoc = new(() => new IocManager());

	public static IocManager Instance => LazyIoc.Value;

	private IocManager()
	{
	}

	private ILifetimeScope? _lifetimeScope;

	public void Initialize(ILifetimeScope lifetimeScope)
	{
		_lifetimeScope ??= lifetimeScope;
	}

	public T Resolve<T>()
	{
		return _lifetimeScope!.Resolve<T>();
	}
}