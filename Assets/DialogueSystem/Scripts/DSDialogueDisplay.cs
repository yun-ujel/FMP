using UnityEngine;
using TMPro;
using UI.Options;
using UnityEngine.UI;

namespace DS
{
    using ScriptableObjects;
    public class DSDialogueDisplay : DSDialogue
    {
        [Header("Display Portrait")]
        [SerializeField] private RawImage portrait;
        [SerializeField] private Image portraitBox;
        private Color defaultPortraitBoxColour;

        [Header("Display Text")]
        [SerializeField] private TextMeshProUGUI uGUI;
        private DSDialogueSO currentDialogue;

        [Header("Options")]
        [SerializeField] private InputController input;
        [SerializeField] private OptionsNavigation optionsNav;

        [Header("Box")]
        [SerializeField] private Animator boxAnimator;

        private void Start()
        {
            UpdateDialogue(startDialogue);            
        }
        private void Update()
        {
            if (input.GetInteractPressed() || input.GetJumpPressed())
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

            if (dialogue.Texture == null)
            {
                portrait.color = Color.clear;
                portraitBox.enabled = false;

                uGUI.rectTransform.offsetMin = new Vector2(10f, 10f);
            }
            else
            {
                portraitBox.enabled = true;
                portrait.color = Color.white;
                portrait.texture = dialogue.Texture;

                uGUI.rectTransform.offsetMin = new Vector2(300f, 10f);
            }

            if (dialogue.DialogueType == Enumerations.DSDialogueType.SingleChoice)
            {
                optionsNav.CreateOptions();
            }
            else
            {
                optionsNav.CreateOptions(currentDialogue.GetChoicesAsStringArray());
            }
        }

        public void SetOptions(bool setting)
        {
            boxAnimator.SetBool("IsOpen", setting);
        }
    }
}
