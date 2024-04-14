using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlTrigger : MonoBehaviour
{
    [SerializeField] private int bowlIndex;
    private SummonFactory _summonFactory;

    private void Start()
    {
        _summonFactory = FindObjectOfType<SummonFactory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ingredient"))
        {
            Debug.Log("Update");
            _summonFactory.UpdateIngredient(bowlIndex, other.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ingredient"))
        {
            _summonFactory.UpdateIngredient(bowlIndex, null);
        }
    }
}
