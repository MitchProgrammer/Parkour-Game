using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelationEffect : MonoBehaviour
{
    public int pixelSize = 2; // Adjust this value to change the pixelation level

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
