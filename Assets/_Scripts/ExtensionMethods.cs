using System;
using System.Collections;
using UnityEngine;

static class ExtensionMethods
{
    public static Coroutine ExecuteDelayed(this MonoBehaviour go, Action function, float delay)
    {
        return go.StartCoroutine(executeDelayed(function, delay));
    }

    static IEnumerator executeDelayed(Action funtion, float delay)
    {
        yield return new WaitForSeconds(delay);
        funtion();
    }

    public static string GetName(this Enum e)
    {
        return Enum.GetName(e.GetType(), e);
    }
}
