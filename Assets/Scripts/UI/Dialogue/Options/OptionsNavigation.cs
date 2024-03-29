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
        private List<Option> options;

        private bool opened;

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

        public bool IsCurrentSelectedOptionValid()
        {
            if (options == null || options.Count <= 0)
            {
                return false;
            }

            return !options[CurrentSelected].Noisy;
        }

        #region Creation
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
                options[i].Initialize(names[i], false);
                
                options[i].SetColours(new Color(137f / 255f, 81f / 255f, 255f / 255f));
                
                options[i].Noisy = true;
                options[i].SetOptionsOpened(opened);
            }

            SetOptionSelect(0);
        }

        public void SetOptionNoise(int index, bool noisy)
        {
            options[index].Noisy = noisy;
        }
        #endregion

        #region Selection
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

        public void SetOptionsOpened(bool setting)
        {
            opened = setting;
            for (int i = 0; i < options.Count; i++)
            {
                options[i].SetOptionsOpened(setting);
            }
        }

        #endregion

        #endregion

        #region Utility Methods
        private bool InputAxisPressed()
        {
            return inputYOnLastFrame == 0f;
        }
        #endregion
    }
}
