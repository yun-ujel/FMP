using UnityEngine;

public class LevelLoadTrigger : MonoBehaviour
{
    [SerializeField] private GameObject character;

    [Space]

    [SerializeField] private ParticleSystem particles;
    private bool usingParticleSystem;

    [Space]

    [SerializeField] private LevelLoader levelLoader;

    private void Start()
    {
        usingParticleSystem = particles != null;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (usingParticleSystem) { particles.Play(); }
        levelLoader.LoadNextLevel();
    }
}
