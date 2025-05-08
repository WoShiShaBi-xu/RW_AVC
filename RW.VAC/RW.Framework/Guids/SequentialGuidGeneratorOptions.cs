namespace RW.Framework.Guids;

public class SequentialGuidGeneratorOptions
{
	public SequentialGuidType? DefaultSequentialGuidType { get; set; }

	public SequentialGuidType GetDefaultSequentialGuidType()
	{
		return DefaultSequentialGuidType ??
		       SequentialGuidType.SequentialAsString;
	}
}