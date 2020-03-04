
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameplayConfig
{
    public const float ORIGIN_W = 1080f;
    public const float ORIGIN_H = 2280;
    public const float ORIGIN_W_WIDE = 1710f;

    private static System.Random mRandom = new System.Random();
    public static int EasyRandom(int range)
    {
        return mRandom.Next(range);
    }

    public static int EasyRandom(int min, int max)
    {
        return mRandom.Next(min, max);
    }

    public static float EasyRandom(float min, float max)
    {
        return UnityEngine.Random.RandomRange(min, max);
    }
}
