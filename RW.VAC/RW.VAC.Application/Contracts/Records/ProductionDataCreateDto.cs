namespace RW.VAC.Application.Contracts.Records;

public class ProductionDataCreateDto(string name, string value, string? unit)
{
	public string Name { get; set; } = name;

	public string Value { get; set; } = value;

	public string? Unit { get; set; } = unit;
}