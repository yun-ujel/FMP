using PlayerInput;
using UnityEngine;

namespace PlayerInput
{
    public class ControlConsole : MonoBehaviour
    {
        [SerializeField] private InputManager.ControlMode controlMode;

        private InputController mindController;
        private InputController inputController;

        private bool playerNearby;
        private bool isControlling;

        private SpriteRenderer rend;

        private readonly Color fadedColour = new Color(1f, 1f, 1f, 0.6f);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerNearby = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerNearby = false;
            }
        }

        private void Start()
        {
            rend = GetComponent<SpriteRenderer>();

            mindController = InputManager.Instance.GetControls(InputManager.ControlMode.mind);
            inputController = InputManager.Instance.GetControls(controlMode);
        }
        private void Update()
        {
            if (playerNearby && !isControlling)
            {
                rend.color = fadedColour;
                if (mindController.GetInteractPressed())
                {
                    InputManager.Instance.EnableControls(controlMode);
                    isControlling = true;
                }
            }
            else if (inputController.GetBackPressed() && isControlling)
            {
                InputManager.Instance.EnableControls(InputManager.ControlMode.mind);
                isControlling = false;
            }
            else
            {
                rend.color = Color.white;
            }
        }
    }
}
