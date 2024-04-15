using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController Instance;

    public Texture2D pointer;
    public Texture2D hover;
    public Texture2D grab;

    public Vector2 hotspot;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
