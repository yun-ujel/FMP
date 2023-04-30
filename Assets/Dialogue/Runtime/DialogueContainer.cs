using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueContainer : ScriptableObject
{
    public string EntryNodeGUID;
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();
}
