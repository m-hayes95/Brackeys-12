using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMudPaintScript : MonoBehaviour
{
    [SerializeField] Transform player;               // Player's transform to get position and rotation
    [SerializeField] LayerMask groundMask;           // Mask to detect where the raycast should hit
    [SerializeField] Texture2D mudMaskTextureBase;   // Base mask texture (initial state)
    [SerializeField] Texture2D eraserTexture;        // Eraser texture to apply
    [SerializeField] Material material;              // Material to update with the mask texture
    [SerializeField] float eraserSize = 1.0f;        // Size multiplier for the eraser texture

    private Texture2D mudMaskTexture;                // The working mud mask texture
    private float mudAmountTotal;                    // Total amount of mud in the texture

    // Start is called before the first frame update
    void Start()
    {
        // Create a new mud mask texture based on the base texture size
        mudMaskTexture = new Texture2D(mudMaskTextureBase.width, mudMaskTextureBase.height);
        mudMaskTexture.SetPixels(mudMaskTextureBase.GetPixels());
        mudMaskTexture.Apply();

        // Set the initial mud mask texture in the material
        material.SetTexture("_MudMask", mudMaskTexture);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(player.position, Vector3.forward, Color.red);
        // Perform a raycast from the player's position downward
        if (Physics.Raycast(player.position, Vector3.forward, out RaycastHit raycastHit, Mathf.Infinity, groundMask))
        {
            Vector2 textureCoord = raycastHit.textureCoord;

            // Convert the texture coordinates to pixel coordinates
            int pixelX = (int)(textureCoord.x * mudMaskTexture.width);
            int pixelY = (int)(textureCoord.y * mudMaskTexture.height);

            // Erase the mud mask at the hit position using the eraser texture
            ApplyEraserTexture(pixelX, pixelY);

            Debug.Log("UV: " + textureCoord + "; Pixels: " + new Vector2Int(pixelX, pixelY)); // Remove this once script is completed

            mudMaskTexture.Apply();

            // Update the material's texture with the modified mud mask
            material.SetTexture("_MudMask", mudMaskTexture);
        }

        // Calculate the total amount of mud based on the amount of green pixels on the mask texture
        mudAmountTotal = 0f;
        for (int x = 0; x < mudMaskTexture.width; x++)
        {
            for (int y = 0; y < mudMaskTexture.height; y++)
            {
                mudAmountTotal += mudMaskTexture.GetPixel(x, y).g;
            }
        }
    }

    // Apply the eraser texture at the given pixel position on the mud mask, adjusted by size and rotation
    void ApplyEraserTexture(int pixelX, int pixelY)
    {
        // Eraser size scaling
        int eraserWidth = Mathf.RoundToInt(eraserTexture.width * eraserSize);
        int eraserHeight = Mathf.RoundToInt(eraserTexture.height * eraserSize);

        // Get the player's rotation and convert it to radians
        float playerRotation = player.eulerAngles.y * Mathf.Deg2Rad;

        // Loop through the eraser texture pixels and apply them to the mud mask texture
        for (int x = 0; x < eraserWidth; x++)
        {
            for (int y = 0; y < eraserHeight; y++)
            {
                // Calculate normalized coordinates within the eraser texture (0 to 1)
                float normX = (float)x / eraserWidth;
                float normY = (float)y / eraserHeight;

                // Translate eraser coordinates to the center of the texture
                float centeredX = (normX - 0.5f) * eraserWidth;
                float centeredY = (normY - 0.5f) * eraserHeight;

                // Rotate the coordinates by the player's rotation angle
                float rotatedX = centeredX * Mathf.Cos(playerRotation) - centeredY * Mathf.Sin(playerRotation);
                float rotatedY = centeredX * Mathf.Sin(playerRotation) + centeredY * Mathf.Cos(playerRotation);

                // Translate back and map to pixel coordinates
                int targetX = pixelX + Mathf.RoundToInt(rotatedX);
                int targetY = pixelY + Mathf.RoundToInt(rotatedY);

                // Ensure the target position is within bounds of the mud mask texture
                if (targetX >= 0 && targetX < mudMaskTexture.width && targetY >= 0 && targetY < mudMaskTexture.height)
                {
                    // Get the original mud mask pixel and the eraser pixel
                    int eraserTextureX = Mathf.FloorToInt(normX * eraserTexture.width);
                    int eraserTextureY = Mathf.FloorToInt(normY * eraserTexture.height);

                    Color mudPixel = mudMaskTexture.GetPixel(targetX, targetY);
                    Color eraserPixel = eraserTexture.GetPixel(eraserTextureX, eraserTextureY);

                    // Blend the mud mask pixel with the eraser pixel (subtracting mud, simulating erasure)
                    // Assuming eraserPixel.a determines how much to erase (1 means full erase, 0 means no erase)
                    float eraseFactor = eraserPixel.a;  // How much to erase, based on the eraser texture's alpha
                    Color newMudPixel = new Color(
                        mudPixel.r * (1 - eraseFactor),  // Reduce red channel
                        mudPixel.g * (1 - eraseFactor),  // Reduce green channel (assuming this is mud amount)
                        mudPixel.b * (1 - eraseFactor),  // Reduce blue channel
                        mudPixel.a                       // Keep the original alpha
                    );

                    // Set the modified pixel back into the mud mask texture
                    mudMaskTexture.SetPixel(targetX, targetY, newMudPixel);
                }
            }
        }
    }

    // Get the total amount of mud left in the texture
    public float GetTotalMud()
    {
        return mudAmountTotal;
    }
}
