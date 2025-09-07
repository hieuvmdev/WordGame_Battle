using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordNode
{
    public bool isWord;
    public WordNode[] childrenNodes;

    public WordNode(bool isWord)
    {
        this.isWord = isWord;
        childrenNodes = new WordNode[GameConstants.AMOUNT_CHAR];
    }

    public WordNode(bool isWord, WordNode[] childrenNodes)
    {
        this.isWord = isWord;
        this.childrenNodes = childrenNodes;
    }

    public int GetAmountOfChildren()
    {
        int amount = 0;

        for (int i = 0; i < childrenNodes.Length; i++)
        {
            if (childrenNodes[i] != null)
            {
                amount++;
            }
            
        }

        return amount;
    }

    public bool ContainKeyChildren(int key)
    {
        return childrenNodes[key] != null;
    }
}
