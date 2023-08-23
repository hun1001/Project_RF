using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableInputBinding
{
    public BindPair[] bindPairs;

    public SerializableInputBinding(InputBinding binding)
    {
        int len = binding.Bindings.Count;
        int idx = 0;

        bindPairs = new BindPair[len];

        foreach (var pair in binding.Bindings)
        {
            bindPairs[idx++] = new BindPair(pair.Key, pair.Value);
        }
    }
}

[Serializable]
public class BindPair
{
    public UserAction key;
    public KeyCode value;

    public BindPair(UserAction key, KeyCode value)
    {
        this.key = key;
        this.value = value;
    }
}
