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

        private DSGraphView graphView;

        private TextField fileNameTextField;
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

            fileNameTextField = DSElementUtility.CreateTextField(defaultFileName, callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            }, "File Name:");

            saveButton = DSElementUtility.CreateButton("Save", () => Save());

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);

            rootVisualElement.Add(toolbar);
        }

        #region Elements Addition
        private void AddGraphView()
        {
            graphView = new DSGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }
        #endregion

        #region Toolbar Actions
        private void Save()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid File Name", "Please ensure the file name you've entered is valid", "OK");
                return;
            }
            DSSaveUtility.Initialize(graphView, fileNameTextField.value);
            DSSaveUtility.Save();
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