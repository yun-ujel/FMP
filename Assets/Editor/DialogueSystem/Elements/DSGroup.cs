using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Elements
{
    public class DSGroup : Group
    {
        public string previousTitle;

        private Color defaultBorderColor;
        private float defaultBorderWidth;

        public DSGroup(string groupTitle, Vector2 position)
        {
            title = groupTitle;
            previousTitle = groupTitle;

            SetPosition(new Rect(position, Vector2.zero));

            defaultBorderColor = contentContainer.style.borderBottomColor.value;
            defaultBorderWidth = contentContainer.style.borderBottomWidth.value;
        }

        public void SetErrorStyle(Color color)
        {
            contentContainer.style.borderBottomColor = color;
            contentContainer.style.borderBottomWidth = 2f;
        }

        public void ResetStyle()
        {
            contentContainer.style.borderBottomColor = defaultBorderColor;
            contentContainer.style.borderBottomWidth = defaultBorderWidth;
        }
    }
}