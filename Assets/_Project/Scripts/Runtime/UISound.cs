using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    private AudioController _audioController;
    [SerializeField] private AudioClip UISFX;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioController = AudioController.Instance;
    }

    public void PlayUISFX()
    {
        _audioController.PlaySound(UISFX);
    }
}
