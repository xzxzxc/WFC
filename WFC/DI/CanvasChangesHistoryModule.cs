using Autofac;
using WFC.Internals.CellSetters;

namespace WFC.DI;

public class CanvasChangesHistoryModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		base.Load(builder);

		builder.RegisterType<SaveHistoryDecorator>()
			.AsSelf()
			.As<IHistoryHolder>()
			.InstancePerLifetimeScope();
		builder.RegisterDecorator<SaveHistoryDecorator, ICellSetter>();
		builder.RegisterDecorator<ICellSetter>((context, _, setter) => 
			context.Resolve<SaveHistoryDecorator>(TypedParameter.From(setter)));
	}
}
