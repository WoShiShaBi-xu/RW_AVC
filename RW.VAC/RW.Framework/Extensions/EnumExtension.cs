using System.ComponentModel;

namespace RW.Framework.Extensions;

public static class EnumExtension
{
	public static string GetDescription(this Enum enumValue)
	{
		var description = enumValue.ToString();
		var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

		if (fieldInfo != null)
		{
			var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
			if (attrs is {Length: > 0})
			{
				description = ((DescriptionAttribute)attrs[0]).Description;
			}
		}

		return description;
	}
}