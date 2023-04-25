using UnityEngine;

public class WorldCharacterInfo : CharacterInfo
{
    [SerializeField] private IsometricMovement move;

    private void Update()
    {
        if (Mathf.Abs(move.Direction.x) > 0f)
        {
            Facing = move.Direction.x > 0 ? 1f : -1f;
        }
    }
}
