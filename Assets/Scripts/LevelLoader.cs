using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [Space]
    [SerializeField] private Level[] levels;
    private int currentLevelIndex;

    [System.Serializable]
    private class Level
    {
        [SerializeField] private GameObject terrain;
        public Vector3 playerStartPosition;
        public bool load;

        public void SetActive(bool setting)
        {
            terrain.SetActive(setting);
        }
    }
    public void LoadLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;

        for (int i = 0; i < levels.Length; i++)
        {
            if (i == levelIndex)
            {
                levels[i].SetActive(true);
                continue;
            }
            levels[i].SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].load)
            {
                ExtensionMethods.DrawBox(levels[i].playerStartPosition, 0.4f, Color.red);
            }
        }
    }

    [ExecuteAlways]
    public void LoadSelectedLevelsEditor()
    {
        bool hasLoadedLevel = false;

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].load && !hasLoadedLevel)
            {
                levels[i].SetActive(true);
                player.transform.position = levels[i].playerStartPosition;
                currentLevelIndex = i;
                hasLoadedLevel = true;
                continue;
            }
            levels[i].SetActive(false);
        }
    }

    public void LoadNextLevel()
    {
        currentLevelIndex += 1;

        player.transform.position = levels[currentLevelIndex].playerStartPosition;

        LoadLevel(currentLevelIndex);
    }
}
