using UnityEngine;
using System.Collections.Generic;

public class MindLevelLoader : MonoBehaviour
{
    private enum LevelLoadState { none, loading, unloading }
    private LevelLoadState levelLoadState;
    
    private bool levelLoadQueued;

    [System.Serializable]
    private class Level
    {
        [SerializeField] private GameObject[] levelTilemaps;

        [Space]

        [SerializeField] private GameObject[] levelObjects;
        private List<Vector3> levelObjectPositions;

        public void Start()
        {
            for (int i = 0; i < levelTilemaps.Length; i++)
            {
                levelTilemaps[i].SetActive(false);
            }

            levelObjectPositions = new List<Vector3>();
            for (int i = 0; i < levelObjects.Length; i++)
            {
                levelObjectPositions.Add(levelObjects[i].transform.position);
                levelObjects[i].SetActive(false);
            }
        }
        public void Load(float time)
        {
            for (int i = 0; i < levelTilemaps.Length; i++)
            {
                levelTilemaps[i].SetActive(true);
                levelTilemaps[i].transform.position = Vector3.Lerp
                (
                    Vector3.down * 15f,
                    Vector3.zero,
                    time
                );
            }

            for (int i = 0; i < levelObjects.Length; i++)
            {
                if (levelObjects[i] != null)
                {
                    levelObjects[i].SetActive(true);
                    levelObjects[i].transform.position = Vector3.Lerp
                    (
                        levelObjectPositions[i] + (Vector3.down * 15f),
                        levelObjectPositions[i],
                        time
                    );
                }
            }
        }
        public void Unload(float time)
        {
            for (int i = 0; i < levelTilemaps.Length; i++)
            {
                levelTilemaps[i].transform.position = Vector3.Lerp
                (
                    Vector3.zero,
                    Vector3.down * 15f,
                    time
                );
            }

            for (int i = 0; i < levelObjects.Length; i++)
            {
                if (levelObjects[i] != null)
                {
                    levelObjects[i].transform.position = Vector3.Lerp
                    (
                        levelObjectPositions[i],
                        levelObjectPositions[i] + (Vector3.down * 15f),
                        time
                    );
                }
            }
        }
        public void Disable()
        {
            for (int i = 0; i < levelTilemaps.Length; i++)
            {
                levelTilemaps[i].SetActive(false);
            }

            for (int i = 0; i < levelObjects.Length; i++)
            {
                if (levelObjects[i] != null)
                {
                    levelObjects[i].SetActive(false);
                }
            }
        }
    }

    [SerializeField] private Level[] levels;

    [Header("Animation")]
    [SerializeField] private AnimationCurve curve;
    private int currentLevelIndex = 5;
    private float counter;

    [SerializeField] private float loadAnimationSpeed = 1f;
    [SerializeField] private float unloadAnimationSpeed = 1f;

    private void Start()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].Start();
        }
    }

    private void Update()
    {
        if (levelLoadState == LevelLoadState.loading)
        {
            if (counter <= 1f)
            {
                levels[currentLevelIndex].Load(curve.Evaluate(counter));
                counter += Time.unscaledDeltaTime * loadAnimationSpeed;
            }
            else if (counter > 1f)
            {
                levelLoadState = LevelLoadState.none;
            }
        }
        else if (levelLoadState == LevelLoadState.unloading)
        {

            if (counter <= 1f)
            {
                levels[currentLevelIndex].Unload(curve.Evaluate(counter));
                counter += Time.unscaledDeltaTime * unloadAnimationSpeed;
            }
            else if (counter > 1f)
            {
                levelLoadState = LevelLoadState.none;
                levels[currentLevelIndex].Disable();

                if (levelLoadQueued)
                {
                    LoadNextLevel();
                }
            }
        }
    }

    public void ProceedToNextLevel()
    {
        UnloadCurrentLevel();
        levelLoadQueued = true;
    }


    private void LoadNextLevel()
    {
        levelLoadQueued = false;

        if (levelLoadState == LevelLoadState.unloading)
        {
            levelLoadQueued = true;
        }
        else if (levelLoadState == LevelLoadState.none)
        {
            levelLoadState = LevelLoadState.loading;
            counter = 0f;

            if (currentLevelIndex == levels.Length - 1)
            {
                currentLevelIndex = 0;
            }
            else
            {
                currentLevelIndex++;
            }
        }
    }

    public void UnloadCurrentLevel()
    {
        if (levelLoadState != LevelLoadState.unloading)
        {
            counter = 0f;
            levelLoadState = LevelLoadState.unloading;
        }
    }
}
