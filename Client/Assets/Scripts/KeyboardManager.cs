using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class KeyboardManager : MonoSingleton<KeyboardManager>
{
    private Dictionary<KeyCode, Action> keyDownAction = new Dictionary<KeyCode, Action>();

    void Update()
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
    }
}
