using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class KeyboardManager : MonoSingleton<KeyboardManager>
{
    private Dictionary<KeyCode, Action> keyDownAction = new Dictionary<KeyCode, Action>();
    private Dictionary<KeyCode, Action> keyUpAction = new Dictionary<KeyCode, Action>();
    private Dictionary<KeyCode, Action> keyHoldAction = new Dictionary<KeyCode, Action>();

    private void Update()
    {
        foreach (var item in keyDownAction)
        {
            if (Input.GetKeyDown(item.Key))
            {
                item.Value?.Invoke();
            }
        }
    }

    public void AddKeyDownAction(KeyCode keyCode, Action action)
    {
        if (keyDownAction.ContainsKey(keyCode))
        {
            keyDownAction[keyCode] += action;
            return;
        }

        keyDownAction.Add(keyCode, action);
    }

    public void AddKeyUpAction(KeyCode keyCode, Action action)
    {
        if(keyUpAction.ContainsKey(keyCode))
        {
            keyUpAction[keyCode] += action;
            return;
        }

        keyUpAction.Add(keyCode, action);
    }

    public void AddKeyHoldAction(KeyCode keyCode, Action action) => AddKeyAction(ref keyHoldAction, keyCode, action);

    public void AddKeyDownActionList(Action[] actions)
    {
        for(int i = 0; i < actions.Length; i++)
        {
            AddKeyDownAction((KeyCode)((int)KeyCode.Alpha1 + (i)), actions[i]);
        }
    }

    public void ClearKeyActions()
    {
        keyDownAction?.Clear();
        keyUpAction?.Clear();
        keyHoldAction?.Clear();
    }

    private void AddKeyAction(ref Dictionary<KeyCode, Action> keyAction, KeyCode keyCode, Action action)
    {
        if (keyAction.ContainsKey(keyCode))
        {
            keyAction[keyCode] += action;
            return;
        }
        keyAction.Add(keyCode, action);
    }
}
