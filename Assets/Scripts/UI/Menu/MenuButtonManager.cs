using UnityEngine;

namespace UI.Menu
{
    public class MenuButtonManager : MonoBehaviour
    {
        public static MenuButtonManager Instance { get; set; }

        [SerializeField] private MenuButton[] menuButtons = new MenuButton[2];
        private int currentSelectedButtonIndex;

        [Header("Controls")]
        [SerializeField] private InputController input;
        private float inputY;
        private float inputYOnLastFrame;

        [Space]
        [SerializeField, Range(0f, 1f)] private float selectionSwitchCooldown = 0.1f;
        private float timeSinceSelectionSwitch;

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
        }

        private void Start()
        {
            input.Enabled = true;
        }

        private void Update()
        {
            inputY = input.GetVerticalInput();

            if (input.GetInteractPressed())
            {
                menuButtons[currentSelectedButtonIndex].OnClick();
            }
            else
            {
                if (inputY >= 1f && (timeSinceSelectionSwitch <= 0f || InputAxisPressed()))
                {
                    currentSelectedButtonIndex -= 1;
                    SetSelectedButton(currentSelectedButtonIndex);
                    timeSinceSelectionSwitch = selectionSwitchCooldown;
                }
                else if (inputY <= -1f && (timeSinceSelectionSwitch <= 0f || InputAxisPressed()))
                {
                    currentSelectedButtonIndex += 1;
                    SetSelectedButton(currentSelectedButtonIndex);
                    timeSinceSelectionSwitch = selectionSwitchCooldown;
                }
                else
                {
                    timeSinceSelectionSwitch -= Time.deltaTime;
                }
            }

            inputYOnLastFrame = inputY;
        }

        private void SetSelectedButton(int selection)
        {
            if (selection > menuButtons.Length - 1)
            {
                selection = menuButtons.Length - 1;
            }
            else if (selection < 0)
            {
                selection = 0;
            }

            currentSelectedButtonIndex = selection;

            for (int i = 0; i < menuButtons.Length; i++)
            {
                if (i == selection)
                {
                    menuButtons[i].Selected = true;
                    continue;
                }

                menuButtons[i].Selected = false;
            }
        }

        public void PointerOver(MenuButton menuButton)
        {
            for (int i = 0; i < menuButtons.Length; i++)
            {
                if (menuButtons[i] == menuButton)
                {
                    SetSelectedButton(i);
                }
            }
        }

        #region Utility Methods
        private bool InputAxisPressed()
        {
            return inputYOnLastFrame == 0f;
        }
        #endregion
    }
}
