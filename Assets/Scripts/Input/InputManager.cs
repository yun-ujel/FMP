using UnityEngine;

namespace PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputController mindController;
        [SerializeField] private InputController uIController;
        [SerializeField] private InputController worldController;

        private void Awake()
        {
            DisableAllControllers();
        }

        private void DisableAllControllers()
        {
            mindController.Enabled = false;
            uIController.Enabled = false;
            worldController.Enabled = false;
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
                uIController.Enabled = true;
            }
        }
    }
}
