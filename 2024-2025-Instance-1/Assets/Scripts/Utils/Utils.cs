using UnityEngine;
using System;
using System.Linq;
using random = System.Random;
using System.Collections.Generic;

public static class Utils
{
    public static float RandomFloat(float min, float max, random rand)
    {
        return min + ((max - min) * (float) rand.NextDouble());
    }

    public static float RandomFloat(float min, float max)
    {
        return RandomFloat(min, max, new random());
    }

    // https://stackoverflow.com/questions/273313/randomize-a-listt
    public static void Shuffle<T>(List<T> list)  
    {  
        random rand = new random();
        int n = list.Count;  
        
        while (n > 1) {  
            n--;  
            int k = rand.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public static T Choice<T>(List<T> list)
    {
        return list[new random().Next(list.Count)];
    }

    public static IEnumerable<float> Range(float start, float stop, float step)
    {
        float x = start;

        Func<bool> predicate = start < stop ? (() => (x < stop)) : (() => (x > stop));

        while (predicate())
        {
            yield return x;
            x += step;
        }
    }

    public static IEnumerable<float> Range(float start, float stop)
    {
        return Range(start, stop, 1);
    }

    public static IEnumerable<float> Range(float stop)
    {
        return Range(0, stop, 1);
    }

    public static T WeightedChoice<T>((T, float)[] values)
    {
        float totalWeight = values.Select(tuple => tuple.Item2).Sum();
        float upto = 0f;
        float r = RandomFloat(0, totalWeight);

        foreach (var (choice, weight) in values)
        {
            upto += weight;
            if (upto >= r)
                return choice;
        }

        throw new Exception("we shouldn't have gotten here");
    }

    // https://stackoverflow.com/a/47176199
    public static float ToNearestMultiple(float value, float multipleOf) 
    {
        return (float) Math.Round((decimal) value / (decimal) multipleOf, MidpointRounding.AwayFromZero) * multipleOf;
    }

    public static IEnumerable<WaitForSeconds> InvokeAfter(Action callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    public static IEnumerable<WaitForSecondsRealtime> InvokeAfterUnscaled(Action callback, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        callback();
    }

    public static IEnumerable<WaitForEndOfFrame> InvokeAfterFrame(Action callback)
    {
        yield return new WaitForEndOfFrame();
        callback();
    }
}