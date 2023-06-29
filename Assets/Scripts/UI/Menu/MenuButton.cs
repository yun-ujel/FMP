using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler
    {
        private enum OnClickFunction
        {
            start,
            quit
        }
        [SerializeField] private OnClickFunction onClick;

        private Graphic graphic;
        [Space, SerializeField] private Color selectedColour = Color.gray;
        [SerializeField] private CanvasGroup darkenerCanvasGroup;

        public bool Selected { get; set; }

        private void Start()
        {
            graphic = GetComponent<Graphic>();
        }

        private void Update()
        {
            if (Selected)
            {
                graphic.color = selectedColour;
            }
            else
            {
                graphic.color = Color.white;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            MenuButtonManager.Instance.PointerOver(this);
        }

        public void OnClick()
        {
            if (onClick == OnClickFunction.start)
            {
                if (darkenerCanvasGroup != null) { darkenerCanvasGroup.alpha = 1f; }
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
