using AntDesign;
using Microsoft.Extensions.Logging;
using Rougamo;
using Rougamo.Context;
using RW.Framework.Autofac;
using RW.Framework.Exceptions;
using RW.Framework.Extensions;

namespace RW.VAC.Client.Interceptors;

public class ComponentExceptionAttribute : MoAttribute
{
	public override string Pattern => "method(* Microsoft.AspNetCore.Components.ComponentBase+.*(..))";

	public override Feature Features => Feature.ExceptionHandle;
	
	public override void OnException(MethodContext context)
	{
		var notificationService = context.TargetType.GetProperty("NotificationService",
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
			?.GetValue(context.Target)!.As<NotificationService>();
		if (context.Exception is BusinessException)
		{
			notificationService?.Error(new()
			{
				Message = "系统错误",
				Description = context.Exception?.Message
			});
		}
		else
		{
			var loggerFactory = IocManager.Instance.Resolve<ILoggerFactory>();
			loggerFactory.CreateLogger(context.TargetType).LogError(context.Exception, "未处理异常");
			notificationService?.Error(new()
			{
				Message = "未处理异常",
				Description = "请稍后重试或联系管理人员"
			});
		}

		foreach (var arg in context.Arguments)
		{
			if (arg is ModalClosingEventArgs e)
			{
				e.Cancel = true;
				break;
			}
		}
		context.HandledException(this, null!);
	}
}