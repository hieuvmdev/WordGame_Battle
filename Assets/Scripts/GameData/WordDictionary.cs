using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;

public class WordDictionary
{
    public static WordDictionary Instance
    {
        get
        {
            if (_instance == null)
            { 
                _instance = new WordDictionary();
            }

            return _instance;
        }
    }

    public bool IsInitialized
    {
        get { return _isInitialized; }
    }

    protected static WordDictionary _instance;

    private WordNode _rootNode;
    private Dictionary<string, WordNode> _nodesDictionary;

    private bool _isInitialized = false;

    public WordDictionary()
    {
        
    }

    public void GenDictionary(string filePath)
    {
        _nodesDictionary = new Dictionary<string, WordNode>();

        string[] listWord = FileHelper.LoadResourceTextFile(filePath, true).Split('\n');
        Thread thread = new Thread(() =>
        {
            for (int i = 0; i < listWord.Length; i++)
            {
                AddWord(listWord[i].Trim().ToLower(), null, 0);
            }

            _rootNode = _nodesDictionary[""];

            _isInitialized = true;
            Debug.Log("Dictionary: " + _nodesDictionary.Count);
        });

        thread.Start();

    }

    public WordNode GetWord(string word)
    {
        if (!_nodesDictionary.ContainsKey(word))
        {
            return null;
        }

        Debug.Log("IsWord: " + _nodesDictionary[word].isWord);

        return _nodesDictionary[word];
    }

    public bool IsWord(string word)
    {
        if (!_isInitialized)
        {
            DebugUtils.Log("Dictionary wasn't initialized");
            return false;
        }

        return IsWord(word, 0, _rootNode);
    }

    private bool IsWord(string word, int wordIndex, WordNode parent)
    {
        WordNode  node = parent.childrenNodes[CharToKeyNumber(word[wordIndex])];
        if (node != null)
        {
            if (word.Length == wordIndex + 1)
            {
                return node.isWord;
            }
            else
            {
                return IsWord(word, wordIndex + 1, node);
            }
        }
        else
        {
            return false;
        }
    }

    public void AddWord(string word, WordNode childNode, int childIndex)
    {
        if (_nodesDictionary.ContainsKey(word))
        {
            if (childNode == null)
            {
                _nodesDictionary[word].isWord = true;
            }
            else
            {
                _nodesDictionary[word].childrenNodes[childIndex] = childNode;
            }
        
        } else if (word.Length > 0)
        {
            WordNode newNode = new WordNode(childNode == null ? true : false);
            newNode.childrenNodes[childIndex] = childNode;
            _nodesDictionary[word] = newNode;
            AddWord(word.Substring(0, word.Length - 1), newNode, CharToKeyNumber(word[word.Length - 1]));
        }
        else
        {
            WordNode rootNode = new WordNode(false);
            rootNode.childrenNodes[childIndex] = childNode;
            _nodesDictionary[word] = rootNode;
        }
    }

    public int CharToKeyNumber(char c)
    {
        int i = -1;
        switch (c)
        {
            case 'a': i = 0; break;
            case 'b': i = 1; break;
            case 'c': i = 2; break;
            case 'd': i = 3; break;
            case 'e': i = 4; break;
            case 'f': i = 5; break;
            case 'g': i = 6; break;
            case 'h': i = 7; break;
            case 'i': i = 8; break;
            case 'j': i = 9; break;
            case 'k': i = 10; break;
            case 'l': i = 11; break;
            case 'm': i = 12; break;
            case 'n': i = 13; break;
            case 'o': i = 14; break;
            case 'p': i = 15; break;
            case 'q': i = 16; break;
            case 'r': i = 17; break;
            case 's': i = 18; break;
            case 't': i = 19; break;
            case 'u': i = 20; break;
            case 'v': i = 21; break;
            case 'w': i = 22; break;
            case 'x': i = 23; break;
            case 'y': i = 24; break;
            case 'z': i = 25; break;

            default:
                DebugUtils.LogError("This char dont exist: "  + c);
                break;
        }
        return i;
    }

    public char KeyNumberToChar(int id)
    {
        char c = ' ';
        switch (id)
        {
            case 0: c = 'a'; break;
            case 1: c = 'b'; break;
            case 2: c = 'c'; break;
            case 3: c = 'd'; break;
            case 4: c = 'e'; break;
            case 5: c = 'f'; break;
            case 6: c = 'g'; break;
            case 7: c = 'h'; break;
            case 8: c = 'i'; break;
            case 9: c = 'j'; break;
            case 10: c = 'k'; break;
            case 11: c = 'l'; break;
            case 12: c = 'm'; break;
            case 13: c = 'n'; break;
            case 14: c = 'o'; break;
            case 15: c = 'p'; break;
            case 16: c = 'q'; break;
            case 17: c = 'r'; break;
            case 18: c = 's'; break;
            case 19: c = 't'; break;
            case 20: c = 'u'; break;
            case 21: c = 'v'; break;
            case 22: c = 'w'; break;
            case 23: c = 'x'; break;
            case 24: c = 'y'; break;
            case 25: c = 'z'; break;
            default:
                DebugUtils.LogError("This id dont exist: " + id);
                break;
        }
        return c;
    }

}
