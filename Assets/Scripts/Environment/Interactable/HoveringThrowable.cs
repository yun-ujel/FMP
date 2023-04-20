using UnityEngine;

public class HoveringThrowable : HoldableObject
{
    [SerializeField, Range(0f, 5f)] private float hoverTime = 1f;
    private float hoverTimeLeft;
    private bool isHovering;

    [SerializeField, Range(0f, 10f)] private float hoverDrag = 4f;
    private float startDrag;

    public override void Awake()
    {
        base.Awake();
        startDrag = body.drag;
    }    

    public override void Update()
    {
        base.Update();

        if (hoverTimeLeft <= 0f && isHovering)
        {
            SetGravity(true);

            body.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);

            isHovering = false;
        }

        hoverTimeLeft -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hoverTimeLeft = 0f;
    }

    public override void Throw()
    {
        base.Throw();

        isHovering = true;
        hoverTimeLeft = hoverTime;
    }

    public override void SetStateToHeld()
    {
        IsBeingHeld = true;        
        body.transform.rotation = Quaternion.identity;

        SetGravity(false);
    }

    public override void SetStateToThrown()
    {
        IsBeingHeld = false;
        throwPending = false;

        SetGravity(false);
    }

    private void SetGravity(bool setting)
    {
        body.gravityScale = setting ? startingGravityScale : 0f;
        body.drag = setting ? startDrag : hoverDrag;

        if (TryGetComponent(out GravityMultiplier gravity)) { gravity.enabled = setting; }
    }
}
