using System.Collections;
using System.Collections.Generic;
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
}
