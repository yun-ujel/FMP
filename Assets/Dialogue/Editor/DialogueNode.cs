using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

public class DialogueNode : Node
{
    public string GUID;

    public string DialogueText;

    public bool IsEntryPoint = false;
}