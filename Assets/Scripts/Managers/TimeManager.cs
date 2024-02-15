using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    private TimeManager() { }

    private Dictionary<string, float> coolTimes;
    private Dictionary<string, float> lastUsedTimes;

    [Serializable]
    public class CoolTimeFinishedEvent : UnityEvent<string> { }

    public CoolTimeFinishedEvent OnCoolTimeFinished;

    private void Awake()
    {
        Instance = this;
        coolTimes = new Dictionary<string, float>();
        lastUsedTimes = new Dictionary<string, float>();
    }

    public void RegisterCoolTime(string id, float coolTime)
    {
        if (coolTime < 0)
        {
            throw new ArgumentException("Cool time cannot be negative.", nameof(coolTime));
        }

        if (coolTimes.ContainsKey(id))
        {
            UpdateCoolTime(id, coolTime);
            Debug.Log($"Cool time item with id '{id}' is already registered.\nCooltime will be update");
        }

        coolTimes.Add(id, coolTime);
        lastUsedTimes.Add(id, 0f);
    }
    
    public void UpdateCoolTime(string id, float newCoolTime)
    {
        if (newCoolTime < 0)
        {
            throw new ArgumentException("Cool time cannot be negative.", nameof(newCoolTime));
        }

        if (!coolTimes.ContainsKey(id))
        {
            throw new ArgumentException($"No cool time item with id '{id}' is registered.", id);
        }

        // 기존의 쿨타임 값을 새로운 값으로 업데이트
        coolTimes[id] = newCoolTime;
    }

    public void Use(string id)
    {
        if (!coolTimes.ContainsKey(id))
        {
            throw new ArgumentException($"No cool time item with id '{id}' is registered.", id);
        }

        lastUsedTimes[id] = Time.time;
    }

    public bool IsCoolTimeFinished(string id)
    {
        if (!coolTimes.ContainsKey(id))
        {
            Debug.Log($"No cool time item with id '{id}' is registered.");
            return false;
        }

        if (Time.time >= lastUsedTimes[id] + coolTimes[id])
        {
            OnCoolTimeFinished?.Invoke(id);
            return true;
        }

        return false;
    }
}
