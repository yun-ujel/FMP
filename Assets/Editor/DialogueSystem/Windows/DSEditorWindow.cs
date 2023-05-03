using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using Utilities;

    public class DSEditorWindow : EditorWindow
    {
        private readonly string defaultFileName = "NewDialogue";

        private Button saveButton;

        [MenuItem("Window/Dialogue System/Dialogue Graph Window")]
        public static void Open()
        {
            DSEditorWindow window = GetWindow<DSEditorWindow>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            TextField fileNameTextField = DSElementUtility.CreateTextField(defaultFileName, null, "File Name:");

            saveButton = DSElementUtility.CreateButton("Save");

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);

            rootVisualElement.Add(toolbar);
        }

        #region Elements Addition
        private void AddGraphView()
        {
            DSGraphView graphView = new DSGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
        #endregion

        #region Utility Methods
        public void SetSaving(bool setting)
        {
            saveButton.SetEnabled(setting);
        }
        #endregion
    }
}