using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
   [Header("Visual/Narrative")]
   public Sprite sprite;
   [TextArea]
   public string flavourText;

   [Header("Movement")]
   [Range(0f, 1f)]
   public float velocityScaling = 0.5f;
   public float maxVelocity = 10f;
   public float defaultGravity = 1f;

   [Header("Audio")] 
   public AudioClip impactSound;
}
