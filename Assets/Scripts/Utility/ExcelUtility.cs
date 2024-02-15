using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class ExcelUtility
{
    public static int GetPercentValue(string description)
    {
        string[] strArr = description.Split();

        for (int i = 0; i < strArr.Length; i++)
        {
            int index = strArr[i].IndexOf('%');
            if (index != -1)
            {
                return int.Parse(strArr[i].Remove(index));
            }
        }

        return 0;
    }

    public static int GetNumericValue(string description)
    {
        string[] strArr = description.Split();

        for (int i = 0; i < strArr.Length; i++)
        {
            if (int.TryParse(strArr[i], out int result))
            {
                return result;
            }
        }

        return 0;
    }

    public static string FormatStringWithVariables(string input, params object[] values)
    {
        if (values == null || values.Length == 0)
        {
            return input;
        }

        var regex = new Regex(@"{(?<index>\d+)}");
        var matches = regex.Matches(input);

        foreach (Match match in matches)
        {
            var index = int.Parse(match.Groups["index"].Value);
            if (index < values.Length)
            {
                input = input.Replace(match.Value, values[index].ToString());
            }
        }

        return input;
    }
}
