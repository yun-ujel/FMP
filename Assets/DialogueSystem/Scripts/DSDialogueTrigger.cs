using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class DSDialogueTrigger : DSDialogue
    {
        public void TriggerDialogue()
        {
            DSDialogueDisplay.Instance.SetDSDialogue
            (
                dialogueContainer,
                dialogueGroup,
                startDialogue,

                groupedDialogues,
                startingDialoguesOnly,

                selectedDialogueGroupIndex,
                selectedDialogueIndex
            );
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                TriggerDialogue();
            }
        }
    }
}
