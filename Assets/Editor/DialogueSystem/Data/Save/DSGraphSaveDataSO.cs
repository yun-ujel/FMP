using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    public class DSGraphSaveDataSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<DSGroupSaveData> Groups { get; set; }
        [field: SerializeField] public List<DSGroupSaveData> Nodes { get; set; }
        [field: SerializeField] public List<string> PreviousGroupNames { get; set; }
        [field: SerializeField] public List<string> PreviousUngroupedNodeNames { get; set; }
        [field: SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            Groups = new List<DSGroupSaveData>();
            Nodes = new List<DSGroupSaveData>();
        }
    }
}