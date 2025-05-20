using System;
using UnityEngine;

public static class DoubleExtension
{
    private const string Zero = "0";

    static readonly string[] UnitsKR = new string[]
        { "", "만", "억", "조", "경", "해", "자", "양", "구", "간", "정", "재", "극", "항", "아", "나", "불", "무", "홍", "몽" };

    static readonly string[] Units = new string[]
    {
        "", "K", "M", "G", "T", "P", "F", "E", "Z", "Y", "KK", "MM", "GG", "TT", "PP", "FF", "EE", "ZZ", "YY", "KKK"
    };

    public static string ToN0String(this double value)
    {
        return $"{Math.Round(value):n0}";
    }

    private static string ToCurrentStringExceptKr(this double value)
    {
        if (Math.Abs(value) < 1) return Zero;
        if (value < 0)
        {
            return $"-{ToCurrentStringExceptKr(-value)}";
        }

        int digit = 1 + (int)Math.Log10(Math.Abs(value));
        if (digit <= 3)
        {
            return value.ToString("0");
        }

        int quot = (digit - 1) / 3;
        string firstUnit = Units[quot];
        string secondUnit = Units[quot - 1];

        double firstValue = Math.Truncate(value * Math.Pow(0.1d, (quot * 3)));
        double secondValue = Math.Truncate(value * Math.Pow(0.1d, (quot - 1) * 3)) - firstValue * 1000;

        //  출력
        if (secondValue >= 1)
        {
            return $"{firstValue}{firstUnit}{secondValue}{secondUnit}";
        }
        return $"{firstValue}{firstUnit}";
    }

    public static string ToCurrencyString(this double value, int blankSize = 0)
    {
        if (Math.Abs(value) < 1) return Zero;
        if (value < 0)
        {
            return $"-{ToCurrencyString(-value, blankSize)}";
        }

        //  자리수 구하고
        int digit = 1 + (int)Math.Log10(Math.Abs(value));
        if (digit <= 4)
        {
            //  4자리 이하면 그냥 ToString()
            return value.ToString("0");
        }

        //  자리수 4로 나누고
        int quot = (digit - 1) / 4;
        string firstUnit = UnitsKR[quot];
        string secondUnit = UnitsKR[quot - 1];

        //  앞에 4자리 뽑아내고 소수점 날림
        double firstValue = Math.Truncate(value * Math.Pow(0.1d, (quot * 4)));
        //  그뒤 4자리
        double secondValue = Math.Truncate(value * Math.Pow(0.1d, (quot - 1) * 4)) - firstValue * 10000;

        //  출력
        if (secondValue >= 1)
        {
            if (blankSize > 0)
            {
                return $"{firstValue}{firstUnit}<size={blankSize}> </size>{secondValue}{secondUnit}";
            }
            else
            {
                return $"{firstValue}{firstUnit} {secondValue}{secondUnit}";
            }
        }
        return $"{firstValue}{firstUnit}";
    }
    
    public static double Lerp(double a, double b, double t) => a + (b - a) * Math.Clamp(t, 0, 1);
}