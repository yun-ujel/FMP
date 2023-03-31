using UnityEngine;

[System.Serializable]
public class Checkpoint
{
    public Transform transform;

    public Vector3 startingPosition = Vector3.zero;
    public Vector3 startingEulers = Vector3.zero;
    public Vector3 startingScale = Vector3.one;

    public void Reset()
    {
        transform.position = startingPosition;
        transform.rotation = Quaternion.Euler(startingEulers);
        transform.localScale = startingScale;
    }
}
public class RoomTrigger : MonoBehaviour
{
    private RoomCameraManager roomCameraManager;
    private int roomIndex;

    [Header("Checkpoints")]

    [SerializeField] private Vector2 playerStartPosition;

    [Space]

    [SerializeField] private Color checkpointPreviewColour = Color.red;
    [SerializeField] private Checkpoint[] checkpoints;
    public void RoomReset()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].Reset();
        }
    }

    public Vector2 StartPosition => playerStartPosition;

    private void OnDrawGizmosSelected()
    {
        playerStartPosition.DrawBox(0.5f);

        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].startingPosition.DrawBox(0.25f, checkpointPreviewColour);
        }
    }

    public void SetRoomCameraManager(RoomCameraManager roomCameraManager, int roomIndex)
    {
        this.roomCameraManager = roomCameraManager;
        this.roomIndex = roomIndex;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (roomCameraManager != null)
            {
                roomCameraManager.OnRoomEnter(roomIndex);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (roomCameraManager != null)
            {
                roomCameraManager.OnRoomExit(roomIndex);
            }
        }
    }
}
