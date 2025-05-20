using System;

public static class LongExtension
{
    public static string ToCurrencyString(this long value, int blankSize = 3)
    {
        return Convert.ToDouble(value).ToCurrencyString(blankSize);
    }
}