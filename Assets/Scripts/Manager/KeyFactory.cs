using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct KeyNode
{
    public char key;
    public Sprite sprite;
}

public class KeyFactory : Singleton<KeyFactory>
{
    [SerializeField] private KeyNode[] _keyNodes;

    public KeyNode GetKey(char c)
    {
        for (int i = 0; i < _keyNodes.Length; i++)
        {
            if (_keyNodes[i].key == c)
            {
                return _keyNodes[i];
            }
        }

        DebugUtils.LogError("Cannot find key " + c);

        return new KeyNode();
    }

    public KeyNode[] GetAllKeys()
    {
        return _keyNodes;
    }
}
