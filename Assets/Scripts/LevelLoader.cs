using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;
    private int currentLevelIndex;

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

    public void LoadNextLevel()
    {
        LoadLevel(currentLevelIndex + 1);
    }
}
