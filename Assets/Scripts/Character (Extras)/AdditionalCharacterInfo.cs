using System.Collections.Generic;
using UnityEngine;

public class AdditionalCharacterInfo : MonoBehaviour
{
    public List<Capability> Capabilities { get; private set; }

    [Header("Base References")]

    [SerializeField] private GroundCheck ground;
    public float TimeSinceLastLanding { get; private set; }
    private bool wasMidairOnPreviousFrame;

    
    [Header("On Can Collected")]    

    [SerializeField] private Capability[] capabilitiesToTrigger;

    [SerializeField] private List<CharacterTrigger> disabledTriggers = new List<CharacterTrigger>();

    private void Awake()
    {
        Capabilities = new List<Capability>();
        Capabilities.AddRange(GetComponents<Capability>());

        if (ground == null)
        {
            CollisionCheck[] checks = GetComponent<CollisionRelay>().CollisionChecks;
            for (int i = 0; i < checks.Length; i++)
            {
                if (checks[i].GetType() == typeof(GroundCheck))
                {
                    ground = (GroundCheck)checks[i];
                    break;
                }
            }
        }

    }

    private void Update()
    {
        CheckPlayerLanding();

        if (Input.GetKeyDown(KeyCode.P))
        {
            TriggerCapabilityEffects();
        }
    }

    private void CheckPlayerLanding()
    {
        TimeSinceLastLanding += Time.deltaTime;

        if (wasMidairOnPreviousFrame && ground.OnGround)
        {
            // Landing
            TimeSinceLastLanding = 0f;
            wasMidairOnPreviousFrame = false;

            for (int i = 0; i < disabledTriggers.Count; i++)
            {
                disabledTriggers[i].gameObject.SetActive(true);
            }
            disabledTriggers.Clear();
        }

        wasMidairOnPreviousFrame = !ground.OnGround;
    }

    public void TriggerCapabilityEffects()
    {
        for (int i = 0; i < capabilitiesToTrigger.Length; i++)
        {
            capabilitiesToTrigger[i].TriggerMainEffect();
        }
    }

    public void UseCharacterTrigger(CharacterTrigger trigger)
    {
        TriggerCapabilityEffects();

        disabledTriggers.Add(trigger);

        trigger.gameObject.SetActive(false);
    }
}
