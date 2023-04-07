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

    private static Quaternion GetQuaternionEulerZ(float rotationFloat)
    {
        int rotation = Mathf.RoundToInt(rotationFloat);
        rotation %= 360;
        if (rotation < 0) rotation += 360;

        return Quaternion.Euler(0, 0, rotation);
    }

    public static Mesh CreateQuad(float width, float height)
    {
        Mesh quad = new Mesh();

        Vector3[] vertices = new Vector3[4]; // 4 Vertices are created, one for each corner of the Quad.
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6]; // 2 Tris are created. Each Tri makes up 3 indexes on an array, meaning the array is of size 6.

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

    public static void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
    // Creates an Array of Vertices, UVs and Tris. These will always match the required size to make a quad, or a whole number of quads.
    {
        vertices = new Vector3[4 * quadCount];
        uvs = new Vector2[4 * quadCount];
        triangles = new int[6 * quadCount];
    }

    public static void AddArraysToMeshAsQuad
    // Takes Vertices, UVs and Tris of a full mesh, and relocates some of them in order to add a quad to the mesh.
    // Used for BGrid Meshes.
        (
            // The Arrays for the Full Mesh. Recommended to be created with CreateEmptyMeshArrays() method.
            Vector3[] vertices,
            Vector2[] uvs,
            int[] triangles,

            // The Position of the Quad being created.
            int index, // The Index is used to calculate which vertices, tris and UVs should be relocated
            Vector3 worldPosition,

            Vector3 quadSize,

            // The Bottom Left and Top Right of the UV That's going to display on the added quad.
            Vector2 uvMin,
            Vector2 uvMax
        )
    {
        // Relocate Vertices
        int startingVertIndex = index * 4;

        quadSize *= .5f;

        vertices[startingVertIndex + 0] = worldPosition + (GetQuaternionEulerZ( 90) * quadSize);
        vertices[startingVertIndex + 1] = worldPosition + (GetQuaternionEulerZ(180) * quadSize);
        vertices[startingVertIndex + 2] = worldPosition + (GetQuaternionEulerZ(270) * quadSize);
        vertices[startingVertIndex + 3] = worldPosition + (GetQuaternionEulerZ(  0) * quadSize);

        // Relocate UVs
        uvs[startingVertIndex + 0] = new Vector2(uvMin.x, uvMax.y);
        uvs[startingVertIndex + 1] = new Vector2(uvMin.x, uvMin.y);
        uvs[startingVertIndex + 2] = new Vector2(uvMax.x, uvMin.y);
        uvs[startingVertIndex + 3] = new Vector2(uvMax.x, uvMax.y);

        // Set Tris to Match Vertices
        int startingTriIndex = index * 6;

        triangles[startingTriIndex + 0] = startingVertIndex + 0;
        triangles[startingTriIndex + 1] = startingVertIndex + 3;
        triangles[startingTriIndex + 2] = startingVertIndex + 1;

        triangles[startingTriIndex + 3] = startingVertIndex + 1;
        triangles[startingTriIndex + 4] = startingVertIndex + 3;
        triangles[startingTriIndex + 5] = startingVertIndex + 2;
    }

    public static void DrawBox(Vector2 position, float radius, Color color)
    {
        // Left
        Debug.DrawLine(position + new Vector2(-radius, -radius), position + new Vector2(-radius, radius), color);

        // Top
        Debug.DrawLine(position + new Vector2(-radius, radius), position + new Vector2(radius, radius), color);

        // Right
        Debug.DrawLine(position + new Vector2(radius, radius), position + new Vector2(radius, -radius), color);

        // Bottom
        Debug.DrawLine(position + new Vector2(radius, -radius), position + new Vector2(-radius, -radius), color);
    }

    public static void DrawBox(Vector2 position, float radius)
    {
        DrawBox(position, radius, Color.white);
    }
}
