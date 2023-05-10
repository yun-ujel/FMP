using UnityEngine;
using TMPro;
using UI.Options;
using UnityEngine.UI;

namespace DS
{
    using ScriptableObjects;
    public class DSDialogueDisplay : DSDialogue
    {
        public static DSDialogueDisplay Instance { get; private set; }

        [Header("Display Portrait")]
        [SerializeField] private RawImage portrait;
        [SerializeField] private Image portraitBox;
        private Color defaultPortraitBoxColour;

        [Header("Display Text")]
        [SerializeField] private TextMeshProUGUI uGUI;
        public DSDialogueSO CurrentDialogue { get; private set; }

        [Header("Options")]
        [SerializeField] private InputController input;
        [SerializeField] private OptionsNavigation optionsNav;
        private int numberOfAddedChoices;

        [Space]

        [SerializeField] private GameObject thoughtPrefab;

        [Header("Box")]
        [SerializeField] private Animator boxAnimator;

        private void Start()
        {
            Instance = this;
            UpdateDialogue(startDialogue);
        }
        private void Update()
        {
            if (input.GetInteractPressed() || input.GetJumpPressed())
            {
                if ((numberOfAddedChoices > 0 && CurrentDialogue.DialogueType == Enumerations.DSDialogueType.MultipleChoice) || CurrentDialogue.DialogueType == Enumerations.DSDialogueType.SingleChoice)
                {
                    DSDialogueSO dialogue = CurrentDialogue.GetChoice(optionsNav.CurrentSelected, out _);
                    if (dialogue != null)
                    {
                        UpdateDialogue(dialogue);

                        if (dialogue.DialogueType == Enumerations.DSDialogueType.MultipleChoice)
                        {
                            for (int i = 0; i < CurrentDialogue.Choices.Count; i++)
                            {
                                Instantiate(thoughtPrefab, new Vector3(Random.Range(-13f, 0.5f), Random.Range(-5.5f, -3.5f)), Quaternion.identity);
                            }
                        }
                    }
                }
            }
        }
        private void UpdateDialogue(DSDialogueSO dialogue)
        {
            CurrentDialogue = dialogue;
            uGUI.text = CurrentDialogue.Text;

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

            optionsNav.CreateOptions();

            numberOfAddedChoices = 0;
        }

        public void SetOptions(bool setting)
        {
            boxAnimator.SetBool("IsOpen", setting);
        }

        public void AddOptionFromIndex(int index)
        {
            CurrentDialogue.GetChoice(index, out string choice);
            optionsNav.AddOption(choice);
        }

        public void AddNextOption()
        {
            if (numberOfAddedChoices < CurrentDialogue.Choices.Count)
            {
                AddOptionFromIndex(numberOfAddedChoices);
                numberOfAddedChoices += 1;
            }
        }
    }
}
