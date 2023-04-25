using UnityEngine;

public class MindCharacterInfo : CharacterInfo
{
    [Header("Base References")]

    [SerializeField] private GroundCheck ground;
    [SerializeField] private Move move;

    public float TimeSinceLastLanding { get; private set; }
    private bool wasMidairOnPreviousFrame;

    [Header("Capabilities To Trigger")]
    [SerializeField] private Capability[] capabilitiesToTrigger;


    public override void Awake()
    {
        base.Awake();

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

        if (move == null)
        {
            for (int i = 0; i < Capabilities.Length; i++)
            {
                if (Capabilities[i].GetType() == typeof(Move))
                {
                    move = (Move)Capabilities[i];
                    break;
                }
            }
        }
    }

    private void Update()
    {
        CheckPlayerLanding();
        Facing = move.Facing;
    }

    private void CheckPlayerLanding()
    {
        TimeSinceLastLanding += Time.deltaTime;

        if (wasMidairOnPreviousFrame && ground.OnGround)
        {
            // Landing
            TimeSinceLastLanding = 0f;
            wasMidairOnPreviousFrame = false;
        }

        wasMidairOnPreviousFrame = !ground.OnGround;
    }
}
