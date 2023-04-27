using UnityEngine;

public class LevelLoader : MonoBehaviour
{
	public static LevelLoader Instance { get; private set; }

	[SerializeField] private GameObject player;
	private Rigidbody2D body;
	[Space]
	[SerializeField] private Level[] levels;
	private int currentLevelIndex;

	[Header("Graphics")]
	[SerializeField] private Parallax parallax;

	private void Start()
	{
		if (Instance != null)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}

		body = player.GetComponent<Rigidbody2D>();

		for (int i = 0; i < levels.Length; i++)
		{
			if (levels[i].IsActive)
			{
				currentLevelIndex = i;
				break;
			}
		}
	}

	[System.Serializable]
	private class Level
	{
		[SerializeField] private GameObject terrain;
		public Vector3 playerStartPosition;
		public bool load;

		public Rigidbody2D body { get; set; }

		public void SetActive(bool setting)
		{
			terrain.SetActive(setting);
		}

		public bool IsActive => terrain.activeInHierarchy && load;
	}

	public void LoadLevel(int levelIndex)
	{
		Debug.Log($"Loading Level {levelIndex}");

		player.SendMessage("ResetPlayer");
		body.velocity = new Vector2(body.velocity.x * 0.2f, body.velocity.y * 0.6f);

		currentLevelIndex = levelIndex;

		for (int i = 0; i < levels.Length; i++)
		{
			if (i == levelIndex)
			{
				levels[i].SetActive(true);
				player.transform.position = levels[i].playerStartPosition;
				levels[i].load = true;
				continue;
			}
			levels[i].SetActive(false);
			levels[i].load = false;
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
			levels[i].load = false;
		}
	}

	public void LoadNextLevel()
	{
		currentLevelIndex += 1;

		LoadLevel(currentLevelIndex);
	}

	public void ReloadCurrentLevel()
	{
		LoadLevel(currentLevelIndex);
	}
}
