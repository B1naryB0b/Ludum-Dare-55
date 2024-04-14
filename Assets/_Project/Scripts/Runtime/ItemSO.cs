using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
   public string itemName;
   
   [Header("Visual/Narrative")]
   public Sprite sprite;
   [TextArea]
   public string flavourText;

   [Header("Movement")]
   public float velocityScaling;
   public float maxVelocity;
   public float defaultGravity = 1f;
}
