using UnityEngine;

namespace DS
{
    using ScriptableObjects;
    public class DSDialogue : MonoBehaviour
    {
        /* Dialogue Scriptable Objects */
        [SerializeField, HideInInspector] protected DSDialogueContainerSO dialogueContainer;
        [SerializeField, HideInInspector] protected DSDialogueGroupSO dialogueGroup;
        [SerializeField, HideInInspector] protected DSDialogueSO startDialogue;

        /* Filters */
        [SerializeField, HideInInspector] protected bool groupedDialogues;
        [SerializeField, HideInInspector] protected bool startingDialoguesOnly;

        /* Indexes */
        [SerializeField, HideInInspector] protected int selectedDialogueGroupIndex;
        [SerializeField, HideInInspector] protected int selectedDialogueIndex;

        public virtual void SetDSDialogue
        (
            DSDialogueContainerSO dialogueContainerSO,
            DSDialogueGroupSO dialogueGroupSO,
            DSDialogueSO startDialogueSO,
            bool groupedDialogues,
            bool startingDialoguesOnly,
            int selectedDialogueGroupIndex,
            int selectedDialogueIndex
        )
        {
            dialogueContainer = dialogueContainerSO;
            dialogueGroup = dialogueGroupSO;
            startDialogue = startDialogueSO;

            this.groupedDialogues = groupedDialogues;
            this.startingDialoguesOnly = startingDialoguesOnly;

            this.selectedDialogueGroupIndex = selectedDialogueGroupIndex;
            this.selectedDialogueIndex = selectedDialogueIndex;
        }
    }
}