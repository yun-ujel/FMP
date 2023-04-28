using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using System.Collections.Generic;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView graphView;
    private string fileName = "New Narrative";

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        DialogueGraph window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void ConstructGraphView()
    {
        graphView = new DialogueGraphView
        {
            name = "Dialogue Graph"

        };
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        Toolbar toolbar = new Toolbar();

        TextField fileNameTextField = new TextField("File Name:");
        fileNameTextField.SetValueWithoutNotify(fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load" });
        toolbar.Add(new Button(() => LogNodeNames()) { text = "Debug" });

        Button nodeCreateButton = new Button(() => { graphView.CreateNode("Dialogue Node"); })
        {
            text = "Create Node"
        };

        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void LogNodeNames()
    {
        int count = 0;

        List<DialogueNode> nodesList = graphView.nodes.ToList().Cast<DialogueNode>().ToList();
        for (int i = 0; i < nodesList.Count; i++)
        {
            Debug.Log($"ALL NODES {i}: {nodesList[i].DialogueText}, {nodesList[i].GUID}");
        }

        foreach (DialogueNode node in graphView.selection)
        {
            int currentNodeIndex = 1984;
            for (int i = 0; i < nodesList.Count; i++)
            {
                if (nodesList[i].GUID == node.GUID)
                {
                    currentNodeIndex = i;
                    break;
                }
            }
            Debug.Log($"SELECTED NODE {count}: {node.DialogueText}, {node.GUID}, is node #{currentNodeIndex}");
            Debug.Log($"SELECTED NODE {count} has {node.outputContainer.childCount} Outputs.");
            for (int i = 0; i < node.outputContainer.childCount; i++)
            {
                Debug.Log(node.outputContainer[i].Q<Port>().portName);
            }

            count++;
        }
    }

    private void RequestDataOperation(bool isSaving)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name", "Please enter a valid file name", "OK");
        }

        GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(graphView);
        if (isSaving)
        {
            saveUtility.SaveGraph(fileName);
        }
        else
        {
            saveUtility.LoadGraph(fileName);
        }
    }
}
