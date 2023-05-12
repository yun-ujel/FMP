using UnityEngine;
using DS;

namespace PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        [SerializeField] private InputController mindController;
        [SerializeField] private InputController worldController;

        [Header("UI")]
        [SerializeField] private InputController uIController;

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

            DisableAllControllers();
        }

        private void DisableAllControllers()
        {
            mindController.Enabled = false;
            uIController.Enabled = false;
            worldController.Enabled = false;
            DSDialogueDisplay.Instance.SetOptionsOpened(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DisableAllControllers();
                mindController.Enabled = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                DisableAllControllers();
                worldController.Enabled = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                DisableAllControllers();
                DSDialogueDisplay.Instance.SetOptionsOpened(true);
                uIController.Enabled = true;
            }
        }
    }
}
