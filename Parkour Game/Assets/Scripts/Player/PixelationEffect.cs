using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelationEffect : MonoBehaviour
{
    [Range(1, 64)]
    public int pixelSize; // Adjust this value to change the pixelation level

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width / pixelSize;
        int height = source.height / pixelSize;

        RenderTexture temporaryTexture = RenderTexture.GetTemporary(width, height, 0);
        temporaryTexture.filterMode = FilterMode.Point;

        Graphics.Blit(source, temporaryTexture);
        Graphics.Blit(temporaryTexture, destination);

        RenderTexture.ReleaseTemporary(temporaryTexture);
    }
}
