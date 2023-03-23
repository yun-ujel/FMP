using UnityEngine;
public class RoomTrigger : MonoBehaviour
{
    private RoomService roomService;
    private int roomIndex;

    public void SetRoomService(RoomService roomService, int roomIndex)
    {
        this.roomService = roomService;
        this.roomIndex = roomIndex;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (roomService != null)
            {

            }
        }
    }
}
