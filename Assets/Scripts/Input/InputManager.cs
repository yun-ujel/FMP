using UnityEngine;
using DS;

namespace PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public enum ControlMode
        {
            mind, world, UI, none
        }

        private ControlMode queuedControlChange;

        [Header("Mind")]
        [SerializeField] private GameObject player;
        [Space, SerializeField] private InputController mindController;

        [Header("World")]
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
        }

        private void Start()
        {
            EnableControls(ControlMode.mind);
        }

        private void Update()
        {
            if (queuedControlChange != ControlMode.none)
            {
                if (queuedControlChange == ControlMode.mind && !mindController.GetInteractPressed())
                {
                    mindController.Enabled = true;
                    queuedControlChange = ControlMode.none;
                }
                else if (queuedControlChange == ControlMode.world && !mindController.GetInteractPressed())
                {
                    worldController.Enabled = true;
                    queuedControlChange = ControlMode.none;
                }
                else if (queuedControlChange == ControlMode.UI && !mindController.GetInteractPressed())
                {
                    SetOptionsOpened(true);
                    uIController.Enabled = true;
                    queuedControlChange = ControlMode.none;
                }
            }
        }

        private void DisableAllControllers()
        {
            mindController.Enabled = false;
            uIController.Enabled = false;
            worldController.Enabled = false;
            SetOptionsOpened(false);
        }

        private void OnDisable()
        {
            mindController.Enabled = true;
            uIController.Enabled = true;
            worldController.Enabled = true;
        }

        public void EnableControls(ControlMode mode)
        {
            DisableAllControllers();

            queuedControlChange = mode;
        }

        public InputController GetControls(ControlMode mode)
        {
            if (mode == ControlMode.mind)
            {
                return mindController;
            }
            else if (mode == ControlMode.world)
            {
                return worldController;
            }
            else
            {
                return uIController;
            }
        }

        #region Utility Methods
        private void SetOptionsOpened(bool setting)
        {
            if (DSDialogueDisplay.Instance == null)
            {
                return;
            }
            DSDialogueDisplay.Instance.SetOptionsOpened(setting);
        }
        #endregion
    }
}
