using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelLoader))]
public class LevelLoaderEditor : Editor
{
    private LevelLoader loader;
    private bool loadLevels;

    private void OnEnable()
    {
        loader = (LevelLoader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        loadLevels = GUILayout.Button("Load Selected Levels");

        if (loadLevels)
        {
            loader.LoadSelectedLevelsEditor();
            loadLevels = false;
        }
    }
}
