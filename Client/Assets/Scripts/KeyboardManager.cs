using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using System.Linq;

public class KeyboardManager : MonoSingleton<KeyboardManager>
{
    private Dictionary<KeyCode, Action> keyDownAction = new Dictionary<KeyCode, Action>();
    private Dictionary<KeyCode, Action> keyUpAction = new Dictionary<KeyCode, Action>();
    private Dictionary<KeyCode, Action> keyHoldAction = new Dictionary<KeyCode, Action>();

    private void Update()
    {
        keyDownAction?.Keys.ToList().ForEach(keyCode =>
        {
            if (Input.GetKeyDown(keyCode))
            {
                keyDownAction[keyCode]?.Invoke();
            }
        });

        keyUpAction?.Keys.ToList().ForEach(keyCode =>
        {
            if (Input.GetKeyUp(keyCode))
            {
                keyUpAction[keyCode]?.Invoke();
            }
        });

        keyHoldAction?.Keys.ToList().ForEach(keyCode =>
        {
            if (Input.GetKey(keyCode))
            {
                keyHoldAction[keyCode]?.Invoke();
            }
        });
    }

    public void AddKeyDownAction(KeyCode keyCode, Action action) => AddKeyAction(ref keyDownAction, keyCode, action);

    public void AddKeyUpAction(KeyCode keyCode, Action action) => AddKeyAction(ref keyUpAction, keyCode, action);

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
