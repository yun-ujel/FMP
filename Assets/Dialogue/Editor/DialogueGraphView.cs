using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 defaultNodeSize = new Vector2(150f, 200f);

    public Blackboard blackboard;
    public List<ExposedProperty> exposedProperties = new List<ExposedProperty>();

    private NodeSearchWindow searchWindow;

    public DialogueGraphView(EditorWindow editorWindow)
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        AddElement(GenerateEntryPointNode());
        AddSearchWindow(editorWindow);
    }

    private void AddSearchWindow(EditorWindow editorWindow)
    {
        searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        searchWindow.Init(this, editorWindow);

        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
    }

    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort
        (
            Orientation.Horizontal,
            portDirection,
            capacity,
            typeof(float) // Arbitrary type, no calculations are going to be made / no data is being pushed through nodes
        );
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    public DialogueNode GenerateEntryPointNode(string GUIDOverride = "", float xPos = 300, float yPos = 200)
    {
        if (GUIDOverride == string.Empty || GUIDOverride == null) { GUIDOverride = System.Guid.NewGuid().ToString(); }

        DialogueNode startNode = new DialogueNode
        {
            title = "Start",
            GUID = GUIDOverride,
            DialogueText = "Entry Point",
            IsEntryPoint = true
        };

        Port outputPort = GeneratePort(startNode, Direction.Output);
        outputPort.portName = "Next";
        startNode.outputContainer.Add(outputPort);

        startNode.capabilities &= ~Capabilities.Deletable;

        startNode.RefreshExpandedState();
        startNode.RefreshPorts();

        startNode.SetPosition(new Rect(xPos, yPos, 100, 150));
        return startNode;
    }

    public void AddDialogueNode(string nodeName, Vector2 position)
    {
        AddElement(CreateDialogueNode(nodeName, position));
    }

    public DialogueNode CreateDialogueNode(string nodeName, Vector2 position)
    {
        DialogueNode dialogueNode = new DialogueNode
        {
            title = nodeName,
            GUID = System.Guid.NewGuid().ToString(),
            DialogueText = nodeName,
        };

        Port inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);

        Button addPortButton = new Button(() => { AddChoicePort(dialogueNode); });
        addPortButton.text = "Add Choice";
        dialogueNode.titleContainer.Add(addPortButton);

        TextField textField = new TextField(string.Empty);
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.DialogueText = evt.newValue;
            dialogueNode.title = evt.newValue;
        });
        textField.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(textField);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();

        dialogueNode.SetPosition(new Rect
        (
            position, 
            defaultNodeSize
        ));

        return dialogueNode;
    }

    public void AddChoicePort(DialogueNode dialogueNode, string overriddenPortName = "")
    {
        Port choicePort = GeneratePort(dialogueNode, Direction.Output);

        Label excessLabel = choicePort.contentContainer.Q<Label>("type");
        choicePort.contentContainer.Remove(excessLabel);

        int portCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
        string choicePortName = string.IsNullOrEmpty(overriddenPortName)
            ? $"Choice {portCount + 1}"
            : overriddenPortName;

        TextField textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };
        textField.RegisterValueChangedCallback(evt => choicePort.portName = evt.newValue);

        choicePort.contentContainer.Add(new Label("  "));
        choicePort.contentContainer.Add(textField);

        Button deleteButton = new Button(() => RemovePort(dialogueNode, choicePort))
        {
            text = "-"
        };
        choicePort.contentContainer.Add(deleteButton);

        choicePort.portName = choicePortName;
        dialogueNode.outputContainer.Add(choicePort);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
    }

    private void RemovePort(Node node, Port socket)
    {
        IEnumerable<Edge> targetEdge = edges.ToList()
            .Where(x => x.output.portName == socket.portName && x.output.node == socket.node);
        if (targetEdge.Any())
        {
            Edge edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        node.outputContainer.Remove(socket);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }

    public void AddPropertyToBlackboard(ExposedProperty property)
    {
        string localPropertyName = property.Name;
        while (exposedProperties.Any(exposedProperty => exposedProperty.Name == localPropertyName))
        {
            localPropertyName = $"{localPropertyName}(1)";
        }


        ExposedProperty propertyInstance = new ExposedProperty
        {
            Name = localPropertyName,
            Value = property.Value
        };

        exposedProperties.Add(propertyInstance);

        VisualElement container = new VisualElement();
        BlackboardField blackboardField = new BlackboardField { text = propertyInstance.Name, typeText = "string" };
        container.Add(blackboardField);

        TextField propertyValueTextField = new TextField("Value:")
        {
            value = propertyInstance.Value
        };
        propertyValueTextField.RegisterValueChangedCallback(evt =>
        {
            int changingPropertyIndex = exposedProperties.FindIndex(exposedProperty => exposedProperty.Name == propertyInstance.Name);
            exposedProperties[changingPropertyIndex].Value = evt.newValue;
        });

        BlackboardRow blackboardValueRow = new BlackboardRow(blackboardField, propertyValueTextField);
        container.Add(blackboardValueRow);


        blackboard.Add(container);
    }

    public void ClearBlackboardAndExposedProperties()
    {
        exposedProperties.Clear();
        blackboard.Clear();
    }
}
