using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

namespace DS.Windows
{
    using Elements;
    using Enumerations;
    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddManipulators();
            AddGridBackground();

            AddStyles();
        }

        private DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
        {
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");
            
            DSNode node = (DSNode)Activator.CreateInstance(nodeType);

            node.Initialize(position);
            node.Draw();

            return node;
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu(DSDialogueType.SingleChoice, "Add Node (Single Choice)"));
            this.AddManipulator(CreateNodeContextualMenu(DSDialogueType.MultipleChoice, "Add Node (Multiple Choice)"));
        }

        private IManipulator CreateNodeContextualMenu(DSDialogueType dialogueType, string actionTitle)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator
            (
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSGraphViewStyles.uss");

            styleSheets.Add(styleSheet);
        }
    }

}