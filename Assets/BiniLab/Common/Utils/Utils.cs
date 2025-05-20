using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.UI;
using Random = System.Random;

namespace BiniLab
{
    public enum CountryCode
    {
        NONE = 0,

        AR,
        AU,
        BR,
        CA,
        CN,
        ES,
        DE,
        DK,
        FR,
        GB,
        HK,
        HU,
        ID,
        IN,
        IT,
        JP,
        KR,
        MX,
        MY,
        NL,
        NZ,
        PH,
        PL,
        PT,
        RO,
        RU,
        SA,
        SE,
        SG,
        TH,
        TR,
        TW,
        UA,
        US,
        VN,
        ETC = int.MaxValue
    }

    public static class Utils
    {
        public static Vector3 toXZ(this Vector3 vector3)
        {
            vector3.y = 0;
            return vector3;
        }

        public static Vector3 CalcPosition(Vector3 CurPos, float angle, float distance)
        {
            Vector3 calcPos = Vector3.zero;
            calcPos.x = 1 * Mathf.Cos(angle * Mathf.PI / 180) * distance;
            calcPos.y = 1 * Mathf.Sin(angle * Mathf.PI / 180) * distance;
            return calcPos;
        }

        public static bool CheckProbability(float rate)
        {
            return UnityEngine.Random.Range(0f, 1f) < rate;
        }

        public static void Shuffle(IList list)
        {
            int count = list.Count;
            int last = count - 1;
            for (int i = 0; i < last; ++i)
            {
                int r = UnityEngine.Random.Range(i, count);
                var tmp = list[i];
                list[i] = list[r];
                list[r] = tmp;
            }
        }

        public static T RandomPick<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return default(T);

            int pick = UnityEngine.Random.Range(0, list.Count);
            return list[pick];
        }

        public static T RandomRatePick<T>(
            this IEnumerable<T> list,
            Func<T, double> selector)
        {
            double sum = list.Sum(selector);

            Random random = new System.Random();
            double randomValue = random.NextDouble() * sum;

            foreach (T item in list)
            {
                var rate = selector(item);
                if (rate > randomValue)
                {
                    return item;
                }

                randomValue -= rate;
            }

            return default(T);
        }

        public static float RoundFloat(float v, int c = 2)
        {
            return (float)Mathf.RoundToInt(v * (Mathf.Pow(10, c))) / Mathf.Pow(10, c);
        }

        public static double RoundDouble(double v, int c = 2)
        {
            return Math.Round(v * (Mathf.Pow(10, c))) / Mathf.Pow(10, c);
        }

        public static Vector3 GetVelocity(float angle, float power)
        {
            Vector3 velocity = new Vector3();
            velocity.x = Mathf.Cos(angle * Mathf.PI / 180.0f) * power;
            velocity.y = Mathf.Sin(angle * Mathf.PI / 180.0f) * power;
            return velocity;
        }

        public static string GetEnumDescription(Enum value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static Vector3 ChangeGameObjectUIPos(Vector3 pos, Camera worldCamera, Camera UICamera)
        {
            Vector3 resultPos = worldCamera.WorldToScreenPoint(pos);
            resultPos = UICamera.ScreenToWorldPoint(resultPos);
            return resultPos;
        }

        public static Color ColorFromHTMLString(string htmlColor)
        {
            Color color = Color.white;
            ColorUtility.TryParseHtmlString(htmlColor, out color);
            return color;
        }


        private static readonly int charA = Convert.ToInt32('a');

        public static Vector3 BezierCurve(float t, Vector3 p0, Vector3 p1)
        {
            return ((1 - t) * p0) + ((t) * p1);
        }

        public static Vector3 BezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            Vector3 pa = BezierCurve(t, p0, p1);
            Vector3 pb = BezierCurve(t, p1, p2);
            return BezierCurve(t, pa, pb);
        }

        private static readonly Dictionary<int, string> units = new Dictionary<int, string>
        {
            { 0, "" },
            { 1, "K" },
            { 2, "M" },
            { 3, "B" },
            { 4, "T" }
        };

        public static string FormatNumber(double value)
        {
            if (value < 1d)
            {
                return "0";
            }

            var n = (int)Math.Log(value, 1000);
            var m = value / Math.Pow(1000, n);
            var unit = "";

            if (n < units.Count)
            {
                unit = units[n];
            }
            else
            {
                var unitInt = n - units.Count;
                var secondUnit = unitInt % 26;
                var firstUnit = unitInt / 26;
                unit = Convert.ToChar(firstUnit + charA).ToString() + Convert.ToChar(secondUnit + charA).ToString();
            }

            // Math.Floor(m * 100) / 100) fixes rounding errors
            if (n == 0)
                return (Math.Floor(m * 100) / 100).ToString("0");
            else
                return (Math.Floor(m * 100) / 100).ToString("0.##") + unit;
        }

        private static readonly Dictionary<SystemLanguage, string> COUTRY_CODES = new Dictionary<SystemLanguage, string>()
        {
            { SystemLanguage.Afrikaans, "ZA" },
            { SystemLanguage.Arabic, "SA" },
            { SystemLanguage.Basque, "US" },
            { SystemLanguage.Belarusian, "BY" },
            { SystemLanguage.Bulgarian, "BJ" },
            { SystemLanguage.Catalan, "ES" },
            { SystemLanguage.Chinese, "CN" },
            { SystemLanguage.Czech, "HK" },
            { SystemLanguage.Danish, "DK" },
            { SystemLanguage.Dutch, "BE" },
            { SystemLanguage.English, "US" },
            { SystemLanguage.Estonian, "EE" },
            { SystemLanguage.Faroese, "FU" },
            { SystemLanguage.Finnish, "FI" },
            { SystemLanguage.French, "FR" },
            { SystemLanguage.German, "DE" },
            { SystemLanguage.Greek, "JR" },
            { SystemLanguage.Hebrew, "IL" },
            { SystemLanguage.Icelandic, "IS" },
            { SystemLanguage.Indonesian, "ID" },
            { SystemLanguage.Italian, "IT" },
            { SystemLanguage.Japanese, "JP" },
            { SystemLanguage.Korean, "KR" },
            { SystemLanguage.Latvian, "LV" },
            { SystemLanguage.Lithuanian, "LT" },
            { SystemLanguage.Norwegian, "NO" },
            { SystemLanguage.Polish, "PL" },
            { SystemLanguage.Portuguese, "PT" },
            { SystemLanguage.Romanian, "RO" },
            { SystemLanguage.Russian, "RU" },
            { SystemLanguage.SerboCroatian, "SP" },
            { SystemLanguage.Slovak, "SK" },
            { SystemLanguage.Slovenian, "SI" },
            { SystemLanguage.Spanish, "ES" },
            { SystemLanguage.Swedish, "SE" },
            { SystemLanguage.Thai, "TH" },
            { SystemLanguage.Turkish, "TR" },
            { SystemLanguage.Ukrainian, "UA" },
            { SystemLanguage.Vietnamese, "VN" },
            { SystemLanguage.ChineseSimplified, "CN" },
            { SystemLanguage.ChineseTraditional, "TW" },
            { SystemLanguage.Unknown, "US" },
            { SystemLanguage.Hungarian, "HU" },
        };

        /// <summary>
        /// Returns approximate country code of the language.
        /// </summary>
        /// <returns>Approximated country code.</returns>
        /// <param name="language">Language which should be converted to country code.</param>
        public static string GetCountryCode()
        {
            string result;
            if (COUTRY_CODES.TryGetValue(Application.systemLanguage, out result))
            {
                return result;
            }
            else
            {
                return COUTRY_CODES[SystemLanguage.Unknown];
            }
        }

        private static readonly Dictionary<CountryCode, string> COUTRY_NAMES = new Dictionary<CountryCode, string>()
        {
            { CountryCode.AR, "Argentina" },
            { CountryCode.AU, "Australia" },
            { CountryCode.BR, "Brasil" },
            { CountryCode.CA, "Canada" },
            { CountryCode.CN, "中国" },
            { CountryCode.ES, "España" },
            { CountryCode.DE, "Deutschland" },
            { CountryCode.DK, "Danmark" },
            { CountryCode.FR, "France" },
            { CountryCode.GB, "U.K." },
            { CountryCode.HK, "Hong Kong" },
            { CountryCode.HU, "Magyarország" },
            { CountryCode.ID, "Indonesia" },
            { CountryCode.IN, "India" },
            { CountryCode.IT, "Italia" },
            { CountryCode.JP, "日本" },
            { CountryCode.KR, "대한민국" },
            { CountryCode.MX, "México" },
            { CountryCode.MY, "Malaysia" },
            { CountryCode.NL, "Nederland" },
            { CountryCode.NZ, "New Zealand" },
            { CountryCode.PH, "Philippines" },
            { CountryCode.PL, "Polska" },
            { CountryCode.PT, "República Portuguesa" },
            { CountryCode.RO, "România" },
            { CountryCode.RU, "Росси́я" },
            { CountryCode.SA, "العربية السعودية" },
            { CountryCode.SE, "Sverige" },
            { CountryCode.SG, "Singapore" },
            { CountryCode.TH, "ประเทศไทย" },
            { CountryCode.TR, "Türkiye" },
            { CountryCode.TW, "中華民國" },
            { CountryCode.UA, "Україна" },
            { CountryCode.US, "U.S.A." },
            { CountryCode.VN, "Việt Nam" },
            { CountryCode.ETC, "World" },
        };

        public static string GetCountryName(CountryCode country)
        {
            string result;
            if (COUTRY_NAMES.TryGetValue(country, out result))
            {
                return result;
            }
            else
            {
                return COUTRY_NAMES[CountryCode.ETC];
            }
        }

        public static void ChangeLayers(this Transform trans, string name, bool changeChild = true)
        {
            trans.gameObject.layer = LayerMask.NameToLayer(name);

            if (changeChild == true)
            {
                foreach (Transform child in trans)
                {
                    ChangeLayers(child, name);
                }
            }
        }

        public static void ChangeLayers(this Transform trans, int num, bool changeChild = true)
        {
            trans.gameObject.layer = num;

            if (changeChild == true)
            {
                foreach (Transform child in trans)
                {
                    ChangeLayers(child, num);
                }
            }
        }

        public static void CenterItem(ScrollRect scrollRect, RectTransform rectTransform, int index, float CelllHeight, float CellWidth, Rect scrollRectValue)
        {
            if (scrollRect.vertical)
            {
                float offSet = (float)index * CelllHeight;
                float scrollCenter = scrollRectValue.height / 2f;
                float itemCenter = CelllHeight / 2f;
                float max = rectTransform.rect.height - scrollRectValue.height;
                rectTransform.anchoredPosition = new Vector2(0f, (Mathf.Clamp((offSet - scrollCenter + itemCenter), 0, max)));
            }
            else
            {
                float offSet = (float)index * CellWidth;
                float scrollCenter = scrollRectValue.width / 2f;
                float itemCenter = CellWidth / 2f;
                float max = rectTransform.rect.width - scrollRectValue.width;
                rectTransform.anchoredPosition = new Vector2(-(Mathf.Clamp((offSet - scrollCenter + itemCenter), 0, max)), 0f);
            }
        }
    }

    public static class JsonHelper
    {
        public static T FromJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public static string ToJson<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public static string ToJson<T>(T obj, bool prettyPrint)
        {
            return JsonUtility.ToJson(obj, prettyPrint);
        }

        public static T[] ListFromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            if (wrapper == null) return null;
            return wrapper.items;
        }

        public static string ListToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ListToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        public static string FixJson(string value)
        {
            value = "{\"items\":" + value + "}";
            return value;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
}