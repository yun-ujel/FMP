using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Linq;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;
    private DialogueContainer cachedDialogueContainer;

    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {
        if (!Edges.Any()) { return; } // If there are no connections, return

        DialogueContainer dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
        dialogueContainer.EntryNodeGUID = Nodes.Find(node => node.IsEntryPoint).GUID;

        Edge[] connectedPorts = Edges.Where(edge => edge.input.node != null).ToArray();
        for (int i = 0; i < connectedPorts.Length; i++)
        {
            DialogueNode outputNode = connectedPorts[i].output.node as DialogueNode;
            DialogueNode inputNode = connectedPorts[i].input.node as DialogueNode;

            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGUID = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGUID = inputNode.GUID
            });
        }

        foreach (DialogueNode dialogueNode in Nodes.Where(node => !node.IsEntryPoint))
        {
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
            {
                GUID = dialogueNode.GUID,
                DialogueText = dialogueNode.DialogueText,
                Position = dialogueNode.GetPosition().position
            });
        }

        if (!AssetDatabase.IsValidFolder("Assets/Dialogue/Resources"))
        {
            AssetDatabase.CreateFolder("Assets/Dialogue", "Resources");
        }

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Dialogue/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {
        cachedDialogueContainer = Resources.Load<DialogueContainer>(fileName);
        if (cachedDialogueContainer == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exist", "OK");
            return;
        }

        ClearGraph();
        CreateDialogueNodes();
        ConnectNodes();
    }

    private void ClearGraph()
    {
        // Set entry points GUID back from the save.
        DialogueNode entryPoint = Nodes.Find(dialogueNode => dialogueNode.IsEntryPoint);

        if (entryPoint != null)
        {
            entryPoint.GUID = cachedDialogueContainer.EntryNodeGUID;
            entryPoint.SetPosition(new Rect
            (
                cachedDialogueContainer.EntryNodePosition,
                new Vector2(100, 150)
            ));
        }
        else
        {
            _targetGraphView.AddElement(_targetGraphView.GenerateEntryPointNode
            (
                cachedDialogueContainer.EntryNodeGUID,
                cachedDialogueContainer.EntryNodePosition.x,
                cachedDialogueContainer.EntryNodePosition.y
            ));
        }

        foreach (DialogueNode node in Nodes)
        {
            if (node.IsEntryPoint) { continue; }

            // Remove edges connected to this node
            Edges.Where(edge => edge.input.node == node).ToList()
                .ForEach(edge => _targetGraphView.RemoveElement(edge));

            // Remove the node
            _targetGraphView.RemoveElement(node);
        }
    }

    private void CreateDialogueNodes()
    {
        foreach (DialogueNodeData nodeData in cachedDialogueContainer.DialogueNodeData)
        {
            DialogueNode tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText, Vector2.zero);
            tempNode.GUID = nodeData.GUID;
            _targetGraphView.AddElement(tempNode);

            List<NodeLinkData> nodeLinks = cachedDialogueContainer.NodeLinks.Where(nodeLink => nodeLink.BaseNodeGUID == nodeData.GUID).ToList();
            nodeLinks.ForEach(nodeLink => _targetGraphView.AddChoicePort(tempNode, nodeLink.PortName));
        }
    }

    private void ConnectNodes()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            List<NodeLinkData> nodeLinks = cachedDialogueContainer.NodeLinks.Where(nodeLink => nodeLink.BaseNodeGUID == Nodes[i].GUID).ToList();
            for (int j = 0; j < nodeLinks.Count; j++)
            {
                DialogueNode targetNode = Nodes.First(node => node.GUID == nodeLinks[j].TargetNodeGUID);
                LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                targetNode.SetPosition(new Rect
                (
                    cachedDialogueContainer.DialogueNodeData.First(node => node.GUID == nodeLinks[j].TargetNodeGUID).Position,
                    _targetGraphView.defaultNodeSize
                ));
            }

        }
    }

    private void LinkNodes(Port output, Port input)
    {
        Edge tempEdge = new Edge
        {
            output = output,
            input = input
        };

        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);

        _targetGraphView.Add(tempEdge);
    }
}
