using UnityEngine;

public class PlayerHairFollower : MonoBehaviour
{
    [SerializeField] private PlayerHairPosition hairPosition;
    [SerializeField] private Move move;

    private void Update()
    {
        transform.position = hairPosition.transform.position + new Vector3(hairPosition.Offset.x * move.Facing, hairPosition.Offset.y);
    }
}
