using UnityEngine;

[System.Serializable]
public class Room 
{
    public RoomTrigger trigger;
    public GameObject virtualCam;
    public bool IsEntered { get; set; }
    public bool IsFocused { get; set; }
}

public class RoomService : MonoBehaviour
{
    [SerializeField] private Room[] rooms;
    private bool focusedRoomOnThisUpdate;

    private void Start()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].trigger != null && rooms[i].virtualCam != null)
            {
                rooms[i].trigger.SetRoomService(this, i);

                continue;
            }
            Debug.LogError("Room " + i + " Is missing a component!");
        }

        CheckRooms();
    }

    public void OnRoomEnter(int index)
    {
        rooms[index].IsEntered = true;
    }

    public void OnRoomExit(int index)
    {
        rooms[index].IsEntered = false;
    }

    private void Update()
    {
        CheckRooms();
    }

    private void CheckRooms()
    {
        focusedRoomOnThisUpdate = false;
        
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].IsFocused && rooms[i].IsEntered)
            {
                // If player hasn't fully moved out of a room,
                // don't change anything
                break;
            }

            if (!rooms[i].IsEntered) // De-Focus un-entered rooms
            {
                rooms[i].virtualCam.SetActive(false);
            }
            else if (!focusedRoomOnThisUpdate) // Focus a Room
            {
                rooms[i].virtualCam.SetActive(true);
                focusedRoomOnThisUpdate = true;
            }
        }
    }
}
