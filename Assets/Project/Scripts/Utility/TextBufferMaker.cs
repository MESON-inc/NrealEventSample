using System.Collections.Generic;
using UnityEngine;

namespace NrealEventSample.Utility
{
    public static class TextBufferMaker
    {
        public static void Make(Texture2D texture, int id, ref List<Vector4> outPositions, float alphaThreshold = 0.01f)
        {
            Color[] pixels = texture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                Color px = pixels[i];
                float avg = (px.r + px.g + px.b) / 3f;

                if (avg < alphaThreshold)
                {
                    int xi = i % texture.width;
                    int yi = i / texture.width;
                    float x = (float)xi / texture.width - 0.5f;
                    float y = (float)yi / texture.height - 0.5f;

                    Vector4 pos = new Vector4(x, 0, y, id);
                    outPositions.Add(pos);
                }
            }
        }
    }
}