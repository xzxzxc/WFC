using Autofac;
using WFC.Internals;

namespace WFC.DI;

public class WfcModule : Module
{
	private readonly int? _randomSeed;

	public WfcModule(int? randomSeed = default)
	{
		_randomSeed = randomSeed;
	}
	
	protected override void Load(ContainerBuilder builder)
	{
		base.Load(builder);

		builder.RegisterInstance(new RandomImpl(_randomSeed))
			.AsImplementedInterfaces();
		builder.RegisterType<SuitableSelector>()
			.AsImplementedInterfaces()
			.SingleInstance();
		builder.RegisterType<SuitableValuesCalculator>()
			.AsImplementedInterfaces()
			.SingleInstance();
		builder.RegisterType<WfcAlgorithm>()
			.AsImplementedInterfaces()
			.InstancePerLifetimeScope();
		builder.RegisterType<SimpleCellSetter>()
			.AsImplementedInterfaces()
			.InstancePerLifetimeScope();
	}
}
