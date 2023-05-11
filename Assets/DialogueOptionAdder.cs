using DS;
using UnityEngine;

public class DialogueOptionAdder : MonoBehaviour
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
