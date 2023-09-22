using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

[Serializable]
public class InputBinding
{
    public Dictionary<UserAction, KeyCode> Bindings => _bindingDict;
    private Dictionary<UserAction, KeyCode> _bindingDict;

    public InputBinding(bool initalize = true)
    {
        _bindingDict = new Dictionary<UserAction, KeyCode>();

        if (initalize)
        {
            ResetAll();
        }
    }

    public InputBinding(SerializableInputBinding sib)
    {
        _bindingDict = new Dictionary<UserAction, KeyCode>();

        foreach (var pair in sib.bindPairs)
        {
            _bindingDict[pair.key] = pair.value;
        }
    }

    public void ApplyNewBindings(InputBinding newBinding)
    {
        _bindingDict = new Dictionary<UserAction, KeyCode>(newBinding._bindingDict);
    }

    public void ApplyNewBindings(SerializableInputBinding sib)
    {
        _bindingDict.Clear();

        foreach (var pair in sib.bindPairs)
        {
            _bindingDict[pair.key] = pair.value;
        }
    }

    public void Bind(in UserAction action, in KeyCode code, bool allowOverlap = false)
    {
        if (!allowOverlap && _bindingDict.ContainsValue(code))
        {
            var copy = new Dictionary<UserAction, KeyCode>(_bindingDict);

            foreach (var pair in copy)
            {
                if (pair.Value.Equals(code))
                {
                    _bindingDict[pair.Key] = KeyCode.None;
                }
            }
        }
        _bindingDict[action] = code;
    }

    public void ResetAll()
    {
        Bind(UserAction.Back, KeyCode.Escape);

        Bind(UserAction.Start, KeyCode.Space);
        Bind(UserAction.OpenTechTree, KeyCode.T);
        Bind(UserAction.OpenFilter, KeyCode.F);
        Bind(UserAction.HideHanger, KeyCode.H);
        Bind(UserAction.LeftMoveHanger, KeyCode.LeftArrow);
        Bind(UserAction.RightMoveHanger, KeyCode.RightArrow);
    }

    public void SaveToFile()
    {
        SerializableInputBinding sib = new SerializableInputBinding(this);

        SaveManager.Save("InputBinding", sib);
    }

    public void LoadFromFile()
    {
        var sib = SaveManager.Load<SerializableInputBinding>("InputBinding");
        ApplyNewBindings(sib);
    }
}
