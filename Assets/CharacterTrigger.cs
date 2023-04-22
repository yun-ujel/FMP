using UnityEngine;

public class CharacterTrigger : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [Space]
    private AdditionalCharacterInfo characterInfo;

    [SerializeField] private ParticleSystem particles;
    private bool usingParticleSystem;

    private void Start()
    {
        characterInfo = character.GetComponent<AdditionalCharacterInfo>();
        usingParticleSystem = particles != null;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (usingParticleSystem) { particles.Play(); }
        characterInfo.UseCharacterTrigger(this);
    }
}
