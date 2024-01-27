using System;
using System.Collections.Generic;

namespace GameData
{
    [Serializable]
    public class DataArray<T>
    {
        public List<T> data;
    }
}