namespace TestTask.Infrastructure.Extenders;

public static class EnumExtensions
{
    public static List<TEnum> ToList<TEnum>(this TEnum value) where TEnum : Enum
    {
        var result = new List<TEnum>();
        foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
        {
            if (value.HasFlag(enumValue) && Convert.ToInt32(enumValue) != 0)
            {
                result.Add(enumValue);
            }
        }
        return result;
    }
}