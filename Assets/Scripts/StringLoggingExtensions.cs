public static class StringLoggingExtensions
{
	public static string Colored(this string message, Colors color)
	{
		return $"<color={color.ToString()}>{message}</color>";
	}

	public static string Colored(this string message, string colorCode)
	{
		return $"<color={colorCode}>{message}</color>";
	}

	public static string Sized(this string message, int size)
	{
		return $"<size={size}>{message}</size>";
	}

	public static string Bold(this string message)
	{
		return $"<b>{message}</b>";
	}

	public static string Italics(this string message)
	{
		return $"<i>{message}</i>";
	}
}
