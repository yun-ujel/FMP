using UnityEngine;
using TMPro;

namespace DS
{
    using ScriptableObjects;
    public class DSDialogueDisplay : DSDialogue
    {
        [SerializeField] private TextMeshProUGUI uGUI;
        private DSDialogueSO currentDialogue;

        private void Start()
        {
            currentDialogue = startDialogue;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentDialogue = currentDialogue.GetChoice(0, out _);
                Debug.Log(currentDialogue.Text);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log(dialogueContainer.GetNodeGroupName(currentDialogue));
            }
        }
    }
}
