using UnityEngine;

public class LevelLoadTrigger : MonoBehaviour
{
    [SerializeField] private GameObject character;

    [Space]

    [SerializeField] private bool reloadCurrentLevel;

    [Header("Particles")]

    [SerializeField] private ParticleSystem particles;
    private bool usingParticleSystem;
    [SerializeField] private bool usePlayerPositionForParticles;

    [Space]

    [SerializeField] private LevelLoader levelLoader;
    private bool hasLoaded;

    private void Start()
    {
        hasLoaded = false;

        usingParticleSystem = particles != null;
        if (levelLoader == null) { levelLoader = LevelLoader.Instance; }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == character)
        {
            if (usingParticleSystem)
            {
                if (usePlayerPositionForParticles) { particles.transform.position = character.transform.position; }
                particles.Play();
            }

            if (!hasLoaded)
            {
                if (reloadCurrentLevel)
                {
                    levelLoader.ReloadCurrentLevel();
                }
                else
                {
                    levelLoader.LoadNextLevel();
                }
                hasLoaded = true;
            }
        }
    }
}
