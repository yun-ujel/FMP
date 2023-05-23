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
        [System.Serializable]
        private class Darkener
        {
            [SerializeField] private SpriteRenderer spriteRenderer;

            public float Alpha
            {
                get
                {
                    return alpha;
                }

                set
                {
                    spriteRenderer.color = new Color(0f, 0f, 0f, value);
                    alpha = value;
                }
            }
            private float alpha;

            public bool IsDarkened { get; set; }
        }

        [Header("Darkeners")]
        [SerializeField] private Darkener mindDarkener;
        [SerializeField] private Darkener worldDarkener;
        private float maxDelta;
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
            UpdateDarkeners();
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
                    mindDarkener.IsDarkened = false;
                    worldDarkener.IsDarkened = true;
                    break;

                case ControlMode.world:
                    mindDarkener.IsDarkened = true;
                    worldDarkener.IsDarkened = false;
                    break;

                case ControlMode.UI:
                    mindDarkener.IsDarkened = true;
                    worldDarkener.IsDarkened = true;
                    break;

                default:
                    mindDarkener.IsDarkened = false;
                    worldDarkener.IsDarkened = false;
                    break;
            }
        }

        private void UpdateDarkeners()
        {
            maxDelta = Time.deltaTime * 3f;
            if (mindDarkener.IsDarkened && mindDarkener.Alpha != 0.8f)
            {
                mindDarkener.Alpha = Mathf.MoveTowards(mindDarkener.Alpha, 0.8f, maxDelta);
            }
            else if (!mindDarkener.IsDarkened && mindDarkener.Alpha != 0f)
            {
                mindDarkener.Alpha = Mathf.MoveTowards(mindDarkener.Alpha, 0f, maxDelta);
            }


            if (worldDarkener.IsDarkened && worldDarkener.Alpha != 0.8f)
            {
                worldDarkener.Alpha = Mathf.MoveTowards(worldDarkener.Alpha, 0.8f, maxDelta);
            }
            else if (!worldDarkener.IsDarkened && worldDarkener.Alpha != 0f)
            {
                worldDarkener.Alpha = Mathf.MoveTowards(worldDarkener.Alpha, 0f, maxDelta);
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
