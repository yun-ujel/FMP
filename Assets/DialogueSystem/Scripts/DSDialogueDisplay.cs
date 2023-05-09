using UnityEngine;
using TMPro;
using UI.Options;

namespace DS
{
    using ScriptableObjects;
    public class DSDialogueDisplay : DSDialogue
    {
        [Header("Display Text")]
        [SerializeField] private TextMeshProUGUI uGUI;
        private DSDialogueSO currentDialogue;

        [Header("Options")]
        [SerializeField] private InputController input;
        [SerializeField] private OptionsNavigation optionsNav;

        private void Start()
        {
            UpdateDialogue(startDialogue);
        }
        private void Update()
        {
            if (input.GetAttackPressed() || input.GetJumpPressed())
            {
                DSDialogueSO dialogue = currentDialogue.GetChoice(optionsNav.CurrentSelected, out _);
                if (dialogue != null)
                {
                    UpdateDialogue(dialogue);
                }
            }
        }
        private void UpdateDialogue(DSDialogueSO dialogue)
        {
            currentDialogue = dialogue;
            uGUI.text = currentDialogue.Text;

            if (dialogue.DialogueType == Enumerations.DSDialogueType.SingleChoice)
            {
                optionsNav.CreateOptions();
            }
            else
            {
                optionsNav.CreateOptions(currentDialogue.GetChoicesAsStringArray());
            }
        }
    }
}
