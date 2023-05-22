using UnityEngine;
using DS;

namespace PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        #region Variables
        public static InputManager Instance { get; private set; }

        public enum ControlMode
        {
            mind, world, UI, none
        }
        private ControlMode queuedControlChange;

        #region Controllers
        [System.Serializable]
        private class Controllers
        {
            [field: SerializeField] public InputController Mind { get; set; }
            [field: SerializeField] public InputController World { get; set; }
            [field: SerializeField] public InputController UI { get; set; }
        };

        [Header("Controllers"),SerializeField] private Controllers controllers;
        #endregion

        #region Display
        [Header("Darkeners")]
        [SerializeField] private SpriteRenderer mindDarkener;
        [SerializeField] private SpriteRenderer worldDarkener;

        private Color faded = new Color(0f, 0f, 0f, 0.70588235f);
        #endregion

        #endregion

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
                PlayQueuedControlChanges();
            }
        }

        private void PlayQueuedControlChanges()
        {
            if (queuedControlChange == ControlMode.mind && !controllers.Mind.GetInteractPressed())
            {
                SetDarkenerMode(ControlMode.mind);
                controllers.Mind.Enabled = true;
                queuedControlChange = ControlMode.none;
            }
            else if (queuedControlChange == ControlMode.world && !controllers.Mind.GetInteractPressed())
            {
                SetDarkenerMode(ControlMode.world);
                controllers.World.Enabled = true;
                queuedControlChange = ControlMode.none;
            }
            else if (queuedControlChange == ControlMode.UI && !controllers.Mind.GetInteractPressed())
            {
                SetDarkenerMode(ControlMode.UI);
                SetOptionsOpened(true);
                controllers.UI.Enabled = true;
                queuedControlChange = ControlMode.none;
            }
        }

        #region Controller Methods
        private void DisableAllControllers()
        {
            controllers.Mind.Enabled = false;
            controllers.World.Enabled = false;
            controllers.UI.Enabled = false;
            SetOptionsOpened(false);
        }

        private void OnDisable()
        {
            controllers.Mind.Enabled = true;
            controllers.World.Enabled = true;
            controllers.UI.Enabled = true;
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
                return controllers.Mind;
            }
            else if (mode == ControlMode.world)
            {
                return controllers.World;
            }
            else
            {
                return controllers.UI;
            }
        }
        #endregion

        #region Display Methods
        private void SetDarkenerMode(ControlMode controlMode)
        {
            switch (controlMode)
            {
                case ControlMode.mind:
                    mindDarkener.color = Color.clear;
                    worldDarkener.color = faded;
                    break;

                case ControlMode.world:
                    mindDarkener.color = faded;
                    worldDarkener.color = Color.clear;
                    break;

                case ControlMode.UI:
                    mindDarkener.color = faded;
                    worldDarkener.color = faded;
                    break;

                default:
                    mindDarkener.color = Color.clear;
                    worldDarkener.color = Color.clear;
                    break;
            }
        }
        #endregion

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
