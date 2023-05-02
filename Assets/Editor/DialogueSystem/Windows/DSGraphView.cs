using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using DS.Elements;

namespace DS.Windows
{
    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddManipulators();
            AddGridBackground();

            CreateNode();

            AddStyles();
        }

        private void CreateNode()
        {
            DSNode node = new DSNode();

            node.Initialize();
            node.Draw();

            AddElement(node);
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
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