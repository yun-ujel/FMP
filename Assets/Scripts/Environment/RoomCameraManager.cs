using UnityEngine;
using Cinemachine;

[System.Serializable]
public class Room 
{
    public RoomTrigger trigger;
    public CinemachineVirtualCamera virtualCam;

    public bool IsEntered;
    public bool IsFocused;
}

public class RoomCameraManager : MonoBehaviour
{
    [SerializeField] private Room[] rooms;
    [SerializeField] private int focusedRoomOnStart;

    private bool hasFocusedRoomOnThisUpdate;

    public Room[] Rooms => rooms;
    public int CurrentRoomIndex { get; private set; }

    private Rigidbody2D playerRigidbody;

    [Space]
    [SerializeField] private CinemachineBrain cinemachineBrain;

    private void Start()
    {
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].trigger != null && rooms[i].virtualCam != null)
            {
                rooms[i].trigger.SetRoomCameraManager(this, i);
                rooms[i].virtualCam.gameObject.SetActive(false);

                continue;
            }
            Debug.LogError("Room " + i + " Is missing a component!");
        }

        LoadRoom(focusedRoomOnStart);
    }
    private void Update()
    {
        if (!IsInFocusedRoom())
        {
            FocusNewRoom();
        }

        if (Input.GetKeyDown(KeyCode.Home))
        {
            RespawnPlayerInRoom(CurrentRoomIndex);
        }
    }

    public void OnRoomEnter(int index)
    {
        rooms[index].IsEntered = true;
    }
    public void OnRoomExit(int index)
    {
        rooms[index].IsEntered = false;
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
                SetRoomFocus(i, false);
            }
            else if (!hasFocusedRoomOnThisUpdate) // Focus a Room
            {
                SetRoomFocus(i, true);
            }
        }

        // Player death / out of bounds reset
        if (!hasFocusedRoomOnThisUpdate)
        {
            RespawnPlayerInRoom(CurrentRoomIndex);
        }
    }
    private void SetRoomFocus(int i, bool focus)
    {
        if (focus)
        {
            rooms[i].virtualCam.gameObject.SetActive(true);
            rooms[i].IsFocused = true;

            CurrentRoomIndex = i;
            hasFocusedRoomOnThisUpdate = true;

            ShakeCamera.Instance.StopTime(cinemachineBrain.m_DefaultBlend.BlendTime);
        }
        else
        {
            rooms[i].virtualCam.gameObject.SetActive(false);
            rooms[i].IsFocused = false;
        }
    }

    private void RespawnPlayerInRoom(int index)
    {
        rooms[index].trigger.RoomReset();

        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.transform.position = rooms[CurrentRoomIndex].trigger.StartPosition;
    }

    public void LoadRoom(int index)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (index != i)
            {
                SetRoomFocus(i, false);
            }
            else
            {
                SetRoomFocus(i, true);
            }
        }
        RespawnPlayerInRoom(index);
    }
}
