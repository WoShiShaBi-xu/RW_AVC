namespace RW.Framework.Guids;

public enum SequentialGuidType
{
	/// <summary>
	///	适用于MySql、PostgreSql
	/// </summary>
	SequentialAsString,

	/// <summary>
	/// 适用于Oracle
	/// </summary>
	SequentialAsBinary,

	/// <summary>
	/// 适用于SqlServer
	/// </summary>
	SequentialAtEnd

}