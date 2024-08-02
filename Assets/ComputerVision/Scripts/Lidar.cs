using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lidar : MonoBehaviour
{
    public int numberOfRays = 360; // Number of rays to be cast
    public float maxDistance = 20f; // Max distance of the rays
    public int textureSize = 512; // Size of the texture
    public int pointSize = 5; // Size of the white point to be drawn
    public int borderSize = 2; // Size of the border area to be drawn
    public GameObject planeObject; // The Plane object where the texture will be applied
    public Color pointColor = Color.white; // Main point color
    public Color borderColor = Color.cyan; // Border color around the point

    private Texture2D texture;
    private Renderer planeRenderer;

    void Start()
    {
        // Create a new Texture2D
        texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);

        // Get the renderer of the plane and assign the texture
        planeRenderer = planeObject.GetComponent<Renderer>();
        planeRenderer.material.mainTexture = texture;

        // Initialize the texture with a clear color (black)
        ClearTexture();
    }

    void ClearTexture()
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, Color.black);
            }
        }
        texture.Apply();
    }

    void Update()
    {
        // Clear the texture at the start of each frame
        ClearTexture();

        // Cast rays in a circle
        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfRays;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                // Convert the hit point to texture coordinates
                Vector2 textureCoord = WorldToTextureCoord(hit.point);
                DrawPointWithBorder(textureCoord, pointSize, borderSize, pointColor, borderColor);
            }
        }

        texture.Apply(); // Apply all SetPixel changes
    }

    Vector2 WorldToTextureCoord(Vector3 worldPosition)
    {
        // Convert world position to Lidar's local space relative to xz plane
        Vector3 localPos = transform.InverseTransformPoint(worldPosition);

        // Normalize the local position to [0, 1] range relative to the maxDistance
        float u = (localPos.x / maxDistance) + 0.5f;
        float v = (localPos.z / maxDistance) + 0.5f;

        // Convert to texture coordinates
        int x = Mathf.Clamp((int)(u * texture.width), 0, texture.width - 1);
        int y = Mathf.Clamp((int)(v * texture.height), 0, texture.height - 1);

        return new Vector2(x, y);
    }

    void DrawPointWithBorder(Vector2 textureCoord, int pointSize, int borderSize, Color pointColor, Color borderColor)
    {
        int halfPointSize = pointSize / 2;

        for (int x = -halfPointSize - borderSize; x <= halfPointSize + borderSize; x++)
        {
            for (int y = -halfPointSize - borderSize; y <= halfPointSize + borderSize; y++)
            {
                int drawX = Mathf.Clamp((int)textureCoord.x + x, 0, texture.width - 1);
                int drawY = Mathf.Clamp((int)textureCoord.y + y, 0, texture.height - 1);

                // Draw the border area
                if (x < -halfPointSize || x > halfPointSize || y < -halfPointSize || y > halfPointSize)
                {
                    texture.SetPixel(drawX, drawY, borderColor);
                }
                else // Draw the main point
                {
                    texture.SetPixel(drawX, drawY, pointColor);
                }
            }
        }
    }
}
