using UnityEngine;
using Cinemachine;

public class RoomCameraManager : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public RoomTrigger trigger;
        public CinemachineVirtualCamera virtualCam;

        public bool IsEntered;
        public bool IsFocused;
    }

    [field: SerializeField] public Room[] Rooms { get; private set; }
    [SerializeField] private int focusedRoomOnStart;

    private bool hasFocusedRoomOnThisUpdate;

    public int CurrentRoomIndex { get; private set; }

    [SerializeField] private Rigidbody2D playerRigidbody;

    [Space]
    [SerializeField] private CinemachineBrain cinemachineBrain;

    private void Awake()
    {
        for (int i = 0; i < Rooms.Length; i++)
        {
            if (Rooms[i].trigger != null && Rooms[i].virtualCam != null)
            {
                Rooms[i].trigger.SetRoomCameraManager(this, i);
                Rooms[i].virtualCam.gameObject.SetActive(false);

                continue;
            }
            Debug.LogError("Room " + i + " Is missing a component!");
        }
    }

    private void Start()
    {
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
        Rooms[index].IsEntered = true;
    }
    public void OnRoomExit(int index)
    {
        Rooms[index].IsEntered = false;
    }

    

    private bool IsInFocusedRoom()
    {
        for (int i = 0; i < Rooms.Length; i++)
        {
            if (Rooms[i].IsFocused && Rooms[i].IsEntered)
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

        for (int i = 0; i < Rooms.Length; i++)
        {
            if (!Rooms[i].IsEntered) // De-Focus un-entered rooms
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
            Rooms[i].virtualCam.gameObject.SetActive(true);
            Rooms[i].IsFocused = true;

            CurrentRoomIndex = i;
            hasFocusedRoomOnThisUpdate = true;

            ShakeCamera.Instance.StopTime(cinemachineBrain.m_DefaultBlend.BlendTime);
        }
        else
        {
            Rooms[i].virtualCam.gameObject.SetActive(false);
            Rooms[i].IsFocused = false;
        }
    }

    private void RespawnPlayerInRoom(int index)
    {
        Rooms[index].trigger.RoomReset();

        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.transform.position = Rooms[CurrentRoomIndex].trigger.StartPosition;
    }

    public void LoadRoom(int index)
    {
        for (int i = 0; i < Rooms.Length; i++)
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
