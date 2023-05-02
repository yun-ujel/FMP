using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

public class DSSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private DSGraphView graphView;
    private EditorWindow window;

    private Texture2D indentationIcon;

    public void Init(DSGraphView graphView, EditorWindow window)
    {
        this.graphView = graphView;
        this.window = window;

        indentationIcon = new Texture2D(1, 1);
        indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
        indentationIcon.Apply();
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
            new SearchTreeGroupEntry(new GUIContent("Dialogue"), 1),
            new SearchTreeEntry(new GUIContent("Dialogue Node", indentationIcon))
            {
                userData = new DSNode(),
                level = 2
            }
        };

        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        Vector2 worldMousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, context.screenMousePosition - window.position.position);
        Vector2 localMousePosition = graphView.contentViewContainer.WorldToLocal(worldMousePosition);

        switch (SearchTreeEntry.userData)
        {
            case DSNode dialogueNode:
                graphView.AddDialogueNode("Dialogue Node", localMousePosition);
                return true;
            default:
                return false;
        }
    }
}
