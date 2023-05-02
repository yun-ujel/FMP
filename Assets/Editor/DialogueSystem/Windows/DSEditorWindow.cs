using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        [MenuItem("Window/Dialogue System/Dialogue Graph Window")]
        public static void Open()
        {
            DSEditorWindow window = GetWindow<DSEditorWindow>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
        }

        #region Elements Addition
        private void AddGraphView()
        {
            DSGraphView graphView = new DSGraphView();

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
        #endregion
    }
}