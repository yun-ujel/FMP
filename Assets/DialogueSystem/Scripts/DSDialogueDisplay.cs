using TMPro;
using UI.Options;
using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    using ScriptableObjects;
    using Enumerations;
    using PlayerInput;

    public class DSDialogueDisplay : MonoBehaviour
    {
        public static DSDialogueDisplay Instance { get; private set; }

        [System.Serializable]
        private class Portrait
        {
            public string[] GroupNames { get; set; }
            [field: SerializeField] public RawImage PortraitImage { get; set; }
            [field: SerializeField] public Image PortraitBox { get; set; }
        }

        [Header("Selected Dialogue")]
        [SerializeField] private DSDialogue selectedDSDialogue;

        [Header("Display Portraits")]
        [SerializeField] private Portrait portrait1;
        [SerializeField] private Portrait portrait2;

        [Header("Display Text")]
        [SerializeField] private TextMeshProUGUI nameUGUI;
        [SerializeField] private TextMeshProUGUI uGUI;
        public DSDialogueSO CurrentDialogue { get; private set; }

        private char[] textCharArray;
        private string displayText;
        private int displayTextProgress;

        [Space]
        [SerializeField, Range(1f, 100f)] private float lettersPerSecond;
        private float timeSinceLastLetterAdded;

        [Header("Options")]
        [SerializeField] private InputController input;
        [SerializeField] private OptionsNavigation optionsNav;
        private int numberOfAddedChoices;

        [Header("Box")]
        [SerializeField] private CanvasGroup boxCanvasGroup;
        [SerializeField] private Animator boxAnimator;

        [Header("Gameplay")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private MindLevelLoader levelLoader;

        [Space]

        [SerializeField] private DSDialogue callPhoneWhenDialogueReached;
        [SerializeField] private Phone phoneEvent;

        [Space]

        [SerializeField] private DSDialogue endGameWhenDialogueReached;
        [SerializeField] private FadeOut fadeOut;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            QueueDialogue(selectedDSDialogue);
        }

        private void Update()
        {
            if (displayTextProgress < textCharArray.Length)
            {
                UpdateDialogueText();

                if ((input.GetJumpPressed() || input.GetInteractPressed()) && InputManager.Instance.ControlSwitchCooldown <= 0f)
                {
                    FinishDialogueText();
                }
            }
            else
            {
                if ((input.GetJumpPressed() || input.GetInteractPressed()) && InputManager.Instance.ControlSwitchCooldown <= 0f)
                { /* Get Selected Dialogue Choice */
                    if
                        (
                            (optionsNav.IsCurrentSelectedOptionValid() && CurrentDialogue.DialogueType == DSDialogueType.MultipleChoice)
                            ||
                            CurrentDialogue.DialogueType == DSDialogueType.SingleChoice
                        )
                    {
                        DSDialogueSO dialogue = CurrentDialogue.GetChoice(optionsNav.CurrentSelected);
                        if (dialogue != null)
                        {
                            SetCurrentDialogue(dialogue);
                            UpdatePortraitImages();

                            if (callPhoneWhenDialogueReached.dialogue == dialogue)
                            {
                                phoneEvent.enabled = true;
                            }
                        }
                    }
                }
            }

            if (playerTransform.position.y > 2.5f)
            {
                boxCanvasGroup.alpha = 0.1f;
            }
            else
            {
                boxCanvasGroup.alpha = 1f;
            }
        }

        private void SetCurrentDialogue(DSDialogueSO dialogue)
        {
            CurrentDialogue = dialogue;
            //uGUI.text = CurrentDialogue.Text;
            textCharArray = CurrentDialogue.Text.ToCharArray();
            displayText = "";
            displayTextProgress = 0;

            uGUI.rectTransform.offsetMin = dialogue.Texture == null ? new Vector2(10f, 10f) : new Vector2(300f, 10f);

            numberOfAddedChoices = 0;

            optionsNav.CreateOptions();
            levelLoader.UnloadCurrentLevel();
        }

        #region Dialogue Text Display Methods
        private void FinishDialogueText()
        {
            uGUI.text = CurrentDialogue.Text;
            displayTextProgress = textCharArray.Length;


            if (CurrentDialogue.DialogueType == DSDialogueType.MultipleChoice)
            {
                optionsNav.CreateOptions(CurrentDialogue.GetChoicesAsStringArray());
                Debug.Log("Load Next Level");
                levelLoader.ProceedToNextLevel();
            }

            if (CurrentDialogue == endGameWhenDialogueReached.dialogue)
            {
                fadeOut.enabled = true;
            }
        }

        private void UpdateDialogueText()
        {
            if (timeSinceLastLetterAdded >= (1f / lettersPerSecond))
            {
                displayText += textCharArray[displayTextProgress];
                displayTextProgress++;

                uGUI.text = displayText;

                timeSinceLastLetterAdded = 0f;

                if (displayTextProgress >= textCharArray.Length)
                {
                    FinishDialogueText();
                }
            }
            else
            {
                timeSinceLastLetterAdded += Time.deltaTime;
            }
        }
        #endregion

        #region Options Methods
        public void SetOptionsOpened(bool setting)
        {
            boxAnimator.SetBool("Open", setting);
            optionsNav.SetOptionsOpened(setting);
        }

        public void AddOptionFromIndex(int index)
        {
            optionsNav.SetOptionNoise(index, false);
        }

        public void AddNextOption()
        {
            if (numberOfAddedChoices < CurrentDialogue.Choices.Count)
            {
                AddOptionFromIndex(numberOfAddedChoices);
                //Debug.Log($"Added Option {numberOfAddedChoices} to Dialogue");
                numberOfAddedChoices += 1;
            }
        }
        #endregion

        #region Portrait Methods
        private void UpdatePortraitSettings()
        {
            /* Sets Portrait Group Names and Animator's "single" bool */

            string[] groupNames = selectedDSDialogue.dialogueContainer.GetDialogueGroupNames().ToArray();

            if (groupNames.Length > 1)
            {
                int groupNamesSplit = Mathf.FloorToInt(groupNames.Length / 2f);

                portrait1.GroupNames = new string[groupNamesSplit];
                portrait2.GroupNames = new string[groupNamesSplit + (groupNames.Length % 2)];

                int portrait1Counter = 0;
                int portrait2Counter = 0;
                for (int i = 0; i < groupNames.Length; i++)
                {
                    if (i % 2 == 0 && portrait1.GroupNames.Length > portrait1Counter)
                    {
                        portrait1.GroupNames[portrait1Counter] = groupNames[i];
                        portrait1Counter++;
                        continue;
                    }
                    portrait2.GroupNames[portrait2Counter] = groupNames[i];
                    portrait2Counter++;
                }

                boxAnimator.SetBool("Single", false);
            }
            else if (groupNames.Length > 0)
            {
                portrait1.GroupNames = new string[] { groupNames[0] };
                portrait2.GroupNames = new string[] { "" };

                boxAnimator.SetBool("Single", true);
            }
        }

        private void UpdatePortraitImages()
        {
            string nodeGroupName = selectedDSDialogue.dialogueContainer.GetNodeGroupName(CurrentDialogue);
            nameUGUI.text = nodeGroupName;

            if (portrait2.GroupNames[0] != "")
            {
                portrait2.PortraitImage.color = Color.white;
                portrait2.PortraitBox.enabled = true;

                for (int i = 0; i < portrait2.GroupNames.Length; i++)
                {
                    if (portrait2.GroupNames[i] == nodeGroupName)
                    {
                        portrait2.PortraitImage.texture = CurrentDialogue.Texture;
                        return;
                    }
                }
            }
            else
            {
                portrait2.PortraitImage.color = Color.clear;
                portrait2.PortraitBox.enabled = false;
            }

            for (int i = 0; i < portrait1.GroupNames.Length; i++)
            {
                if (portrait1.GroupNames[i] == nodeGroupName)
                {
                    portrait1.PortraitImage.texture = CurrentDialogue.Texture;
                    return;
                }
            }
        }
        #endregion

        #region Dialogue Selection Methods
        public void QueueDialogue(DSDialogue dsDialogue)
        {
            selectedDSDialogue = dsDialogue;
            SetCurrentDialogue(dsDialogue.dialogue);

            UpdatePortraitSettings();
            UpdatePortraitImages();
        }

        #endregion
    }

}