/// <summary>
/// 作者: Teddy
/// 时间: 2018/06/07
/// 功能: 
/// </summary>
public static class StringExt
{
    public static string RichTextBold(this string str)
    {
        return $"<b>{str}</b>";
    }

    public static string RichTextItalic(this string str)
    {
        return $"<i>{str}</i>";
    }

    public static string RichTextColor(this string str, string colorString)
    {
        return $"<color=\"{colorString}\">{str}</color>";
    }

    public static string RichTextSize(this string str, int size)
    {
        return $"<size={size}>{str}</size>";
    }
}