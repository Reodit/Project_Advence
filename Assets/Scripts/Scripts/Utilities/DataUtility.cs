using System.Collections.Generic;
using GameData;
using UnityEngine;

namespace Utilities
{
    public static class DataUtility
    {
        public static void LoadData<T>(string dataPath, out Dictionary<int, T> dataDictionary) where T : IIdentifiable
        {
            TextAsset rawData = Resources.Load<TextAsset>(dataPath);
            DataArray<T> dataArray = JsonUtility.FromJson<DataArray<T>>(rawData.text);

            dataDictionary = null;
            dataDictionary ??= new Dictionary<int, T>();
            
            // Key 중복 검사 
            foreach (var e in dataArray.data)
            {
                if (!dataDictionary.TryAdd(e.GetID(), e))
                {
#if UNITY_EDITOR
                    Debug.LogError($"Could not add item with key \n ID : {e.GetID()}   DataName : {e.GetDataName()}");
#endif
                }
            }
        }

        public static T LoadData<T>(string dataPath) where T : IIdentifiable
        {
            TextAsset rawData = Resources.Load<TextAsset>(dataPath);

            if (rawData == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"Failed to load data from path: {dataPath}");
#endif
                return default(T);
            }

            DataArray<T> dataArray = JsonUtility.FromJson<DataArray<T>>(rawData.text);

            if (dataArray != null && dataArray.data.Count > 0)
            {
                return dataArray.data[0];
            }
    
#if UNITY_EDITOR
            Debug.LogError($"DataArray is empty or null for path: {dataPath}");
#endif

            return default(T);
        }
    }
}


