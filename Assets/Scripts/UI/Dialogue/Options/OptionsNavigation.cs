using UnityEngine;
using System.Collections.Generic;

namespace UI.Options
{
    public class OptionsNavigation : MonoBehaviour
    {
        [Header("Controls")]
        [SerializeField] private InputController input;
        private float inputY;
        private float inputYOnLastFrame;

        [Space]

        [SerializeField, Range(0f, 1f)] private float selectionSwitchCooldown = 0.1f;
        private float timeSinceSelectionSwitch;

        public int CurrentSelected { get; set; }

        [Header("Options")]
        [SerializeField] private GameObject optionPrefab;
        
        [Space]

        [SerializeField] private Transform optionsParent;
        [SerializeField] private List<Option> options;

        private void Start()
        {
            
        }

        private void Update()
        {
            if (input.GetInteractPressed() || input.GetJumpPressed())
            {

            }
            else
            {
                inputY = input.GetVerticalInput();

                if (inputY >= 1f && (timeSinceSelectionSwitch <= 0f || InputAxisPressed()))
                {
                    CurrentSelected -= 1;
                    SetOptionSelect(CurrentSelected);
                    timeSinceSelectionSwitch = selectionSwitchCooldown;
                }
                else if (inputY <= -1f && (timeSinceSelectionSwitch <= 0f || InputAxisPressed()))
                {
                    CurrentSelected += 1;
                    SetOptionSelect(CurrentSelected);
                    timeSinceSelectionSwitch = selectionSwitchCooldown;
                }
                else
                {
                    timeSinceSelectionSwitch -= Time.deltaTime;
                }
            }

            inputYOnLastFrame = inputY;
        }

        #region Options Methods
        public void CreateOptions(params string[] names)
        {
            /* Clear Existing Options */

            if (options != null)
            {
                for (int i = 0; i < options.Count; i++)
                {
                    Destroy(options[i].gameObject);
                }
            }

            options = new List<Option>();

            /* Add new Options */

            for (int i = 0; i < names.Length; i++)
            {
                options.Add(Instantiate(optionPrefab, optionsParent).GetComponent<Option>());
                options[i].Initialize(names[i]);
            }

            SetOptionSelect(0);
        }

        public void AddOption(string name)
        {
            options.Add(Instantiate(optionPrefab, optionsParent).GetComponent<Option>());
            options[options.Count - 1].Initialize(name);
            SetOptionSelect(CurrentSelected);
        }

        private void SetOptionSelect(int selection)
        {
            if (selection > options.Count - 1)
            {
                selection = 0;
            }
            else if (selection < 0)
            {
                selection = options.Count - 1;
            }
            CurrentSelected = selection;

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selection)
                {
                    options[i].SetSelected(true);
                    continue;
                }
                options[i].SetSelected(false);
            }
        }
        #endregion

        #region Utility Methods
        private bool InputAxisPressed()
        {
            return inputYOnLastFrame == 0f;
        }
        #endregion
    }
}
