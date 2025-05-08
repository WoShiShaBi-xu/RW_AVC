using System.Windows.Input;

namespace RW.VAC.Client.Models;

public class DropDownMenuItem
{
	public DropDownMenuItem(string title)
	{
		Title = title;
	}

	public DropDownMenuItem(string title, string icon)
	{
		Title = title;
		Icon = icon;
	}

	public DropDownMenuItem(string title, string icon, ICommand command)
	{
		Title = title;
		Icon = icon;
		Command = command;
	}

	public string Title { get; set; }

	public string? Icon { get; set; }

	public ICommand? Command { get; set; }
}