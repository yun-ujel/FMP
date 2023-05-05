using UnityEngine;

namespace DS
{
    using ScriptableObjects;
    public class DSDialogue : MonoBehaviour
    {
        /* Dialogue Scriptable Objects */
        [SerializeField, HideInInspector] protected DSDialogueContainerSO dialogueContainer;
        [SerializeField, HideInInspector] protected DSDialogueGroupSO dialogueGroup;
        [SerializeField, HideInInspector] protected DSDialogueSO dialogue;

        /* Filters */
        [SerializeField, HideInInspector] protected bool groupedDialogues;
        [SerializeField, HideInInspector] protected bool startingDialoguesOnly;

        /* Indexes */
        [SerializeField, HideInInspector] protected int selectedDialogueGroupIndex;
        [SerializeField, HideInInspector] protected int selectedDialogueIndex;
    }
}