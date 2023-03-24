using UnityEngine;

[System.Serializable]
public class Room 
{
    public RoomTrigger trigger;
    public GameObject virtualCam;
    public bool IsEntered;
    public bool IsFocused;
}

public class RoomService : MonoBehaviour
{
    [SerializeField] private Room[] rooms;
    private bool hasFocusedRoomOnThisUpdate;

    private void Start()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].trigger != null && rooms[i].virtualCam != null)
            {
                rooms[i].trigger.SetRoomService(this, i);
                rooms[i].virtualCam.SetActive(false);

                continue;
            }
            Debug.LogError("Room " + i + " Is missing a component!");
        }

        rooms[0].virtualCam.SetActive(true);
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
        if (!IsInFocusedRoom())
        {
            FocusNewRoom();
        }
    }

    private bool IsInFocusedRoom()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].IsFocused && rooms[i].IsEntered)
            {
                // If player hasn't fully moved out of a room,
                // don't change anything
                return true;
            }
        }

        return false;
    }

    private void FocusNewRoom()
    {
        hasFocusedRoomOnThisUpdate = false;

        for (int i = 0; i < rooms.Length; i++)
        {
            if (!rooms[i].IsEntered) // De-Focus un-entered rooms
            {
                rooms[i].virtualCam.SetActive(false);
                rooms[i].IsFocused = false;
            }
            else if (!hasFocusedRoomOnThisUpdate) // Focus a Room
            {
                rooms[i].virtualCam.SetActive(true);
                rooms[i].IsFocused = true;

                hasFocusedRoomOnThisUpdate = true;
            }
        }
    }
}
