using UnityEngine;
public static class ExtensionMethods
{
    public static float Remap(this float input, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return (input - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin;
    }

    public static TextMesh CreateWorldText
        (
            string text, 
            Transform parent = null, 
            Vector3 localPosition = default, 
            int fontSize = 40, 
            Color? color = null, 
            TextAnchor textAnchor = TextAnchor.UpperLeft, 
            TextAlignment textAlignment = TextAlignment.Left, 
            int sortingOrder = 5000
        )
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("WorldText", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static Mesh CreateQuad(float width, float height)
    {
        Mesh quad = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(0, 0);            // Bottom Left
        vertices[1] = new Vector3(0, height);       // Top Left
        vertices[2] = new Vector3(width, height);   // Top Right
        vertices[3] = new Vector3(width, 0);        // Bottom Right

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        // Tris will be front-facing, so vertex index moves clockwise
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        quad.vertices = vertices;
        quad.uv = uv;
        quad.triangles = triangles;

        return quad;
    }

    public static GameObject CreateWorldQuad(Transform parent = null, Vector3 localPosition = default, float width = 1f, float height = 1f)
    {
        GameObject gameObject = new GameObject("Quad", typeof(MeshFilter), typeof(MeshRenderer));
        Mesh quad = CreateQuad(width, height);

        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = localPosition;

        gameObject.GetComponent<MeshFilter>().mesh = quad;
        return gameObject;
    }
}
