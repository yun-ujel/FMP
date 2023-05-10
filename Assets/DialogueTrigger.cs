using DS;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DSDialogueDisplay.Instance.AddNextOption();
            Destroy(gameObject);
        }
    }
}
