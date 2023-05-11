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

        public override void SetDSDialogue(DSDialogueContainerSO dialogueContainerSO, DSDialogueGroupSO dialogueGroupSO, DSDialogueSO startDialogueSO, bool groupedDialogues, bool startingDialoguesOnly, int selectedDialogueGroupIndex, int selectedDialogueIndex)
        {
            base.SetDSDialogue(dialogueContainerSO, dialogueGroupSO, startDialogueSO, groupedDialogues, startingDialoguesOnly, selectedDialogueGroupIndex, selectedDialogueIndex);
            UpdateDialogue(startDialogue);
        }

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

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
                                Instantiate(thoughtPrefab, new Vector3(Random.Range(-13f, 0.5f), Random.Range(-3.5f, -1.5f)), Quaternion.identity);
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

            uGUI.rectTransform.offsetMin = dialogue.Texture == null ? new Vector2(10f, 10f) : new Vector2(300f, 10f);

            optionsNav.CreateOptions();

            numberOfAddedChoices = 0;
        }

        public void SetOptions(bool setting)
        {
            boxAnimator.SetBool("Open", setting);
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
