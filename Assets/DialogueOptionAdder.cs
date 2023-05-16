using DS;
using UnityEngine;

public class DialogueOptionAdder : MonoBehaviour
{
    private bool addingOption;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!addingOption)
        {
            if (collision.CompareTag("Player"))
            {
                addingOption = true;
                if (DSDialogueDisplay.Instance != null)
                {
                    DSDialogueDisplay.Instance.AddNextOption();
                }
                Destroy(gameObject);
            }
        }
    }
}
