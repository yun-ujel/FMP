using UnityEngine;
public class MindCharacterAnimation : CharacterAnimation
{
    [Header("Character References")]
    private CollisionRelay relay;

    public override void Start()
    {
        relay = character.GetComponent<CollisionRelay>();
        base.Start();
    }


    public override void Update()
    {
        base.Update();
        FlipTowardsMovement();
    }

    
    // Methods for use of AnimationHandlers    
    public override bool TryGetCheck(out CollisionCheck check, System.Type type)
    {
        for (int i = 0; i < relay.CollisionChecks.Length; i++)
        {
            if (relay.CollisionChecks[i].GetType() == type)
            {
                check = relay.CollisionChecks[i];
                return true;
            }
        }

        check = null;
        return false;
    }
}
