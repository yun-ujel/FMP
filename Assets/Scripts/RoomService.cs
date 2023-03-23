using UnityEngine;

[System.Serializable]
public class Room 
{
    public RoomTrigger trigger;
    public GameObject virtualCam;
}

public class RoomService : MonoBehaviour
{
    [SerializeField] private Room[] rooms;

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
    }

    public void OnRoomTrigger()
    {

    }
}
