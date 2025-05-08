namespace RW.VAC.Infrastructure.Opc;

public class TagChangedEventWrapper
{
	public event Action<TagChangedEventArgs>? Changed;

	public void OnChanged(TagChangedEventArgs e)
	{
		Changed?.Invoke(e);
	}
}