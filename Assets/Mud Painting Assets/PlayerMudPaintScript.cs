using UnityEngine;

public class PlayerMudPaintScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Texture2D mudMaskTextureBase;
    [SerializeField] Texture2D eraserTexture;
    [SerializeField] Material material;
    [SerializeField] float eraserSize = 1.0f;

    private Texture2D mudMaskTexture;
    private float mudAmountTotal;
    private float totalGreenPixels;  // Total number of green pixels initially

    // Start is called before the first frame update
    void Start()
    {
        mudMaskTexture = new Texture2D(mudMaskTextureBase.width, mudMaskTextureBase.height, TextureFormat.RGBA32, false);
        mudMaskTexture.SetPixels(mudMaskTextureBase.GetPixels());
        mudMaskTexture.filterMode = FilterMode.Point;
        mudMaskTexture.Apply();

        eraserTexture.filterMode = FilterMode.Point;

        material.SetTexture("_MudMask", mudMaskTexture);

        totalGreenPixels = CalculateGreenPixels();
    }

    void Update()
    {
        if (Physics.Raycast(player.position, Vector3.forward, out RaycastHit raycastHit, Mathf.Infinity, groundMask))
        {
            Vector2 textureCoord = raycastHit.textureCoord;
            int pixelX = (int)(textureCoord.x * mudMaskTexture.width);
            int pixelY = (int)(textureCoord.y * mudMaskTexture.height);

            ApplyEraserTexture(pixelX, pixelY);

            mudMaskTexture.Apply();
            material.SetTexture("_MudMask", mudMaskTexture);
        }
    }

    // Calculate the total amount of mud left in the texture (as a percentage of remaining green pixels)
    public float GetTotalMud()
    {
        float remainingGreenPixels = 0f;

        for (int x = 0; x < mudMaskTexture.width; x++)
        {
            for (int y = 0; y < mudMaskTexture.height; y++)
            {
                Color pixel = mudMaskTexture.GetPixel(x, y);
                remainingGreenPixels += pixel.g;
            }
        }

        // Calculate the percentage of green pixels remaining
        float percentage = (remainingGreenPixels / totalGreenPixels) * 100f;
        return percentage;
    }

    // Calculate the initial amount of green pixels (mud) in the texture
    private float CalculateGreenPixels()
    {
        float greenPixelCount = 0f;

        for (int x = 0; x < mudMaskTexture.width; x++)
        {
            for (int y = 0; y < mudMaskTexture.height; y++)
            {
                Color pixel = mudMaskTexture.GetPixel(x, y);
                greenPixelCount += pixel.g;  // Sum all the green channel values
            }
        }

        return greenPixelCount; // Return the initial total amount of green pixels
    }

    // Apply the eraser texture on the mud mask
    void ApplyEraserTexture(int pixelX, int pixelY)
    {
        int eraserWidth = Mathf.RoundToInt(eraserTexture.width * eraserSize);
        int eraserHeight = Mathf.RoundToInt(eraserTexture.height * eraserSize);
        float playerRotation = player.eulerAngles.y * Mathf.Deg2Rad;

        for (int x = 0; x < eraserWidth; x++)
        {
            for (int y = 0; y < eraserHeight; y++)
            {
                float normX = (float)x / eraserWidth;
                float normY = (float)y / eraserHeight;

                float centeredX = (normX - 0.5f) * eraserWidth;
                float centeredY = (normY - 0.5f) * eraserHeight;

                float rotatedX = centeredX * Mathf.Cos(playerRotation) - centeredY * Mathf.Sin(playerRotation);
                float rotatedY = centeredX * Mathf.Sin(playerRotation) + centeredY * Mathf.Cos(playerRotation);

                int targetX = pixelX + Mathf.RoundToInt(rotatedX);
                int targetY = pixelY + Mathf.RoundToInt(rotatedY);

                if (targetX >= 0 && targetX < mudMaskTexture.width && targetY >= 0 && targetY < mudMaskTexture.height)
                {
                    int eraserTextureX = Mathf.FloorToInt(normX * eraserTexture.width);
                    int eraserTextureY = Mathf.FloorToInt(normY * eraserTexture.height);

                    Color mudPixel = mudMaskTexture.GetPixel(targetX, targetY);
                    Color eraserPixel = eraserTexture.GetPixel(eraserTextureX, eraserTextureY);

                    if (eraserPixel.a > 0) // Only erase where the eraser texture has alpha
                    {
                        mudMaskTexture.SetPixel(targetX, targetY, new Color(0, 0, 0, mudPixel.a));
                    }
                }
            }
        }
    }
}
