using System;
using System.Collections.Generic;

namespace Event;

public static class EventManager
{
    private static readonly Dictionary<string, Action<object[]>> _eventDictionary = new();

    public static void StartListening(string eventName, Action<object[]> listener)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] += listener;
        }
        else
        {
            _eventDictionary.Add(eventName, listener);
        }
    }

    public static void StartListening(string eventName, Action listener)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] += args => listener();
        }
        else
        {
            _eventDictionary.Add(eventName, args => listener());
        }
    }

    public static void StopListening(string eventName, Action<object[]> listener)
    {
        if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent -= listener;
        }
    }

    public static void StopListening(string eventName, Action listener)
    {
        if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent -= args => listener();
        }
    }

    public static void DeleteEvent(string eventName)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] = null;
            _eventDictionary.Remove(eventName);
        }
    }

    public static void TriggerEvent(string eventName, params object[] args)
    {
        if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent?.Invoke(args);
        }
    }

    public static void ClearEvent()
    {
        _eventDictionary.Clear();
    }
}