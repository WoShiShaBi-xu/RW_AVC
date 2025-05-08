namespace RW.Framework.Extensions;

public static class StringExtension
{
	/// <summary>
	/// 判断字符串是否为null或空
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static bool IsNullOrEmpty(this string? str)
	{
		return string.IsNullOrEmpty(str);
	}

	/// <summary>
	/// 判断字符串不为null或空
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static bool NotNullOrEmpty(this string? str)
	{
		return !string.IsNullOrEmpty(str);
	}

	/// <summary>
	/// 判断字符串是否为null或空白字符
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static bool IsNullOrWhiteSpace(this string? str)
	{
		return string.IsNullOrWhiteSpace(str);
	}

	/// <summary>
	/// 判断字符串不为null或空白字符串
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static bool NotNullOrWhiteSpace(this string? str)
	{
		return !string.IsNullOrWhiteSpace(str);
	}
}