using System.Collections.Generic;
using UnityEngine;

public class AdditionalCharacterInfo : MonoBehaviour
{
    public List<Capability> Capabilities { get; private set; }


    [SerializeField] private GroundCheck ground;
    public float TimeSinceLastLanding { get; private set; }
    private bool wasMidairOnPreviousFrame;

    private void Awake()
    {
        Capabilities = new List<Capability>();
        Capabilities.AddRange(GetComponents<Capability>());

        if (ground == null)
        {
            CollisionCheck[] checks = GetComponent<CollisionRelay>().CollisionChecks;
            for (int i = 0; i < checks.Length; i++)
            {
                if (checks[i].GetType() == typeof(GroundCheck))
                {
                    ground = (GroundCheck)checks[i];
                    break;
                }
            }
        }

    }

    private void Update()
    {
        TimeSinceLastLanding += Time.deltaTime;

        if (wasMidairOnPreviousFrame && ground.OnGround)
        {
            //Debug.Log("Landed");
            TimeSinceLastLanding = 0f;
            wasMidairOnPreviousFrame = false;
        }

        wasMidairOnPreviousFrame = !ground.OnGround;
    }
}
