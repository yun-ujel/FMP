using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace UI.Options
{
    public class Option : MonoBehaviour
    {
        private Graphic buttonGraphic;
        private TextMeshProUGUI text;

        private Color defaultColour;
        private Color selectedColour;

        private Color textDefaultColour;
        private Color textSelectedColour;

        private bool selected;
        public bool Selected
        {
            get
            {
                return selected;
            }
        }

        public void Initialize(string displayText = "Option")
        {
            buttonGraphic = GetComponent<Graphic>();
            text = GetComponentInChildren<TextMeshProUGUI>();
            text.text = displayText;

            SetSelected(false);
            SetColours();
        }

        private void Update()
        {

        }

        public void SetColours(Color? defaultColour = null, Color? selectedColour = null, Color? textDefaultColour = null, Color? textSelectedColour = null)
        {
            if (defaultColour == null)
            {
                this.defaultColour = Color.white;
            }
            else
            {
                this.defaultColour = (Color)defaultColour;
            }

            if (selectedColour == null)
            {
                this.selectedColour = Color.black;
            }
            else
            {
                this.selectedColour = (Color)selectedColour;
            }

            if (textDefaultColour == null)
            {
                this.textDefaultColour = Color.black;
            }
            else
            {
                this.textDefaultColour = (Color)textDefaultColour;
            }

            if (textSelectedColour == null)
            {
                this.textSelectedColour = Color.white;
            }
            else
            {
                this.textSelectedColour = (Color)textSelectedColour;
            }
        }

        public void SetSelected(bool selected)
        {
            this.selected = selected;

            if (selected)
            {
                buttonGraphic.color = selectedColour;
                text.color = textSelectedColour;
                return;
            }

            buttonGraphic.color = defaultColour;
            text.color = textDefaultColour;
        }
    }
}
