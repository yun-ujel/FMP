using UnityEngine;
using DS;

public class Phone : MonoBehaviour
{
    [SerializeField] private DSDialogue dialogueToQueue;
    [SerializeField] private Animator animator;

    [Space]

    [SerializeField] private InputController worldController;

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 200;

        animator.SetBool("Calling", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WorldPlayer"))
        {
            animator.SetBool("Selected", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WorldPlayer"))
        {
            animator.SetBool("Selected", false);
        }
    }

    private void Update()
    {
        if (animator.GetBool("Selected") && worldController.GetInteractPressed())
        {
            DSDialogueDisplay.Instance.QueueDialogue(dialogueToQueue);
            Destroy(gameObject);
        }
    }
}
