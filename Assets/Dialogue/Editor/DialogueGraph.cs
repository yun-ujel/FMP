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
        GenerateBlackboard();
    }

    private void GenerateBlackboard()
    {
        Blackboard blackboardInstance = new Blackboard(graphView);
        blackboardInstance.Add(new BlackboardSection { title = "Exposed Properties" });
        blackboardInstance.addItemRequested = blackboard => { graphView.AddPropertyToBlackboard(new ExposedProperty()); };
        blackboardInstance.editTextRequested = (blackboard, blackboardField, newValue) =>
        {
            string oldPropertyName = ((BlackboardField)blackboardField).text;
            if (graphView.exposedProperties.Any(exposedProperty => exposedProperty.Name == newValue))
            {
                EditorUtility.DisplayDialog("Error", "Property name already exists. Please choose a different name.", "OK");
                return;
            }

            int propertyIndex = graphView.exposedProperties.FindIndex(property => property.Name == oldPropertyName);
            graphView.exposedProperties[propertyIndex].Name = newValue;
            ((BlackboardField)blackboardField).text = newValue;
        };

        blackboardInstance.SetPosition(new Rect(10, 30, 200, 300));

        graphView.Add(blackboardInstance);
        graphView.blackboard = blackboardInstance;
    }

    private void ConstructGraphView()
    {
        graphView = new DialogueGraphView(this)
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

        rootVisualElement.Add(toolbar);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void LogNodeNames()
    {
        DialogueNode[] allNodes = graphView.nodes.ToList().Cast<DialogueNode>().ToArray();

        for (int i = 0; i < allNodes.Length; i++)
        {
            Debug.Log($"ALL NODES {i}: {allNodes[i].DialogueText}, {allNodes[i].GUID}");
        }

        DialogueNode[] selectedNodes = graphView.selection.OfType<DialogueNode>().ToArray();

        for (int count = 0; count < selectedNodes.Length; count++)
        {
            int currentNodeIndex = 1984;
            for (int i = 0; i < allNodes.Length; i++)
            {
                if (allNodes[i].GUID == selectedNodes[count].GUID)
                {
                    currentNodeIndex = i;
                    break;
                }
            }
            Debug.Log($"SELECTED NODE {count}: {selectedNodes[count].DialogueText}, {selectedNodes[count].GUID}, is node #{currentNodeIndex}, is Entry Point: {selectedNodes[count].IsEntryPoint}");

            for (int i = 0; i < selectedNodes[count].outputContainer.childCount; i++)
            {
                Debug.Log($"SELECTED NODE {count}, OUTPUT {i}: {selectedNodes[count].outputContainer[i].Q<Port>().portName}");
            }
        }

        Edge[] allEdges = graphView.edges.ToList().Cast<Edge>().ToArray();
        Edge[] selectedEdges = graphView.selection.OfType<Edge>().ToArray();

        for (int i = 0; i < allEdges.Length; i++)
        {
            DialogueNode inputNode = allEdges[i].input.node as DialogueNode;
            DialogueNode outputNode = allEdges[i].output.node as DialogueNode;

            Debug.Log($"ALL EDGES {i}: OUTPUT {outputNode.DialogueText} {outputNode.GUID}, INPUT {inputNode.DialogueText} {inputNode.GUID}");
        }

        for (int count = 0; count < selectedEdges.Length; count++)
        {
            DialogueNode inputNode = selectedEdges[count].input.node as DialogueNode;
            DialogueNode outputNode = selectedEdges[count].output.node as DialogueNode;

            int currentEdgeIndex = 1984;

            for (int i = 0; i < allEdges.Length; i++)
            {
                if (allEdges[i].Equals(selectedEdges[count]))
                {
                    currentEdgeIndex = i;
                    break;
                }
            }            
            Debug.Log($"SELECTED EDGE {count}: OUTPUT {outputNode.DialogueText} {outputNode.GUID}, INPUT {inputNode.DialogueText} {inputNode.GUID}, is Edge {currentEdgeIndex}");
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
