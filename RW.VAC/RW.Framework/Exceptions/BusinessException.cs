namespace RW.Framework.Exceptions;

public class BusinessException(string? message = null, Exception? innerException = null)
	: Exception(message, innerException)
{
	public BusinessException WithData(string name, object value)
	{
		Data[name] = value;
		return this;
	}

}