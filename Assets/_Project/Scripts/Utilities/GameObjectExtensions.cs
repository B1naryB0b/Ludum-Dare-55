using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static void DestroyAllChildren(this GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform)
        {
            Object.Destroy(child.gameObject);
        }
    }
}
