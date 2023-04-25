using UnityEngine;

public class WorldCharacterAnimation : CharacterAnimation
{
    public override bool TryGetCheck(out CollisionCheck check, System.Type type)
    {
        check = null;
        return false;
    }
}
