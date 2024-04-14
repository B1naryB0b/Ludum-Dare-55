using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeSO : ScriptableObject
{
    public ItemSO[] ingredients = new ItemSO[3];
    public ItemSO summon;
}
