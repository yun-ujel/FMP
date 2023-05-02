using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;

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

        #region Overrided Methods
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                {
                    return;
                }

                if (startPort.node == port.node)
                {
                    // This check makes the previous one obsolete, but I kept the previous one just to be sure
                    return;
                }

                if (startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
        #endregion

        #region Manipulators
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu(DSDialogueType.SingleChoice, "Add Node (Single Choice)"));
            this.AddManipulator(CreateNodeContextualMenu(DSDialogueType.MultipleChoice, "Add Node (Multiple Choice)"));

            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator
            (
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("DialogueGroup", actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }

        private IManipulator CreateNodeContextualMenu(DSDialogueType dialogueType, string actionTitle)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator
            (
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }
        #endregion

        #region Elements Creation
        private GraphElement CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = title
            };

            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }

        private DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
        {
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");

            DSNode node = (DSNode)Activator.CreateInstance(nodeType);

            node.Initialize(position);
            node.Draw();

            return node;
        }
        #endregion

        #region Elements Addition
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            StyleSheet graphViewStyleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSGraphViewStyles.uss");
            StyleSheet nodeStyleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSNodeStyles.uss");

            styleSheets.Add(graphViewStyleSheet);
            styleSheets.Add(nodeStyleSheet);
        }
        #endregion
    }
}