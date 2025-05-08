namespace RW.VAC.Client.Models;

public class FormOption<TKey, TDto>
{
	public TKey Id { get; set; }

	public TDto Data { get; set; }
}

public class FormOption<TDto> : FormOption<Guid?, TDto>
{
}