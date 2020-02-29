using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TextureFormatUtils
{
    private static int[] formatSize = new int[] { 32, 64, 128, 256, 512, 1024, 2048, 4096 };

    public static int FitSize(int width, int height)
    {
        int textureSize = Mathf.Max(height, width);
        return FitSize(textureSize);
    }

    public static int FitSize(int size)
    {
        foreach (var temp in formatSize)
        {
            if (size <= temp)
            {
                return temp;
            }
        }

        return 1024;
    }
}