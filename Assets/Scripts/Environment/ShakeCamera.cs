using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(RoomCameraManager))]
public class ShakeCamera : MonoBehaviour
{
    public static ShakeCamera Instance { get; private set; }

    private RoomCameraManager roomCameraManager;
    private CinemachineBasicMultiChannelPerlin currentVirtualCamNoise;

    private int currentRoomIndex;

    private float shakeTimer;
    private float stopTimer;

    private void Awake()
    {
        roomCameraManager = GetComponent<RoomCameraManager>();
        Instance = this;
    }

    private void Start()
    {
        UpdateNoiseComponent();
    }

    private void UpdateNoiseComponent()
    {
        currentRoomIndex = roomCameraManager.CurrentRoomIndex;

        currentVirtualCamNoise =
            roomCameraManager
            .Rooms[currentRoomIndex]
            .virtualCam
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.unscaledDeltaTime;
        }
        else if (shakeTimer <= 0f)
        {
            if (currentVirtualCamNoise.m_AmplitudeGain > 0f)
            {
                currentVirtualCamNoise.m_AmplitudeGain = 0f;
            }
        }

        if (stopTimer > 0f)
        {
            stopTimer -= Time.unscaledDeltaTime;
        }
        else if (stopTimer <= 0f)
        {
            Time.timeScale = 1.0f;
        }
    }

    public void Shake(float intensity, float time)
    {
        if (currentRoomIndex != roomCameraManager.CurrentRoomIndex)
        {
            UpdateNoiseComponent();
        }

        currentVirtualCamNoise.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    public void StopTime(float time)
    {
        Time.timeScale = 0.0f;
        stopTimer = time;
    }
}
