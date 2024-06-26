using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SummonFactory : MonoBehaviour
{
    
    [SerializeField] private Transform summonPoint;
    [SerializeField] private List<ItemSO> startingItems;
    public List<ItemSO> StartingItems => startingItems;
    [SerializeField] private int maxItems;

    private Queue<GameObject> _activeItemObjects = new Queue<GameObject>();

    [SerializeField] private GameObject[] currentIngredients = new GameObject[3];

    private List<RecipeSO> _recipes;
    
    private Dictionary<GameObject, ItemSO> _itemObjectMap = new Dictionary<GameObject, ItemSO>();

    private UnlockedItems _unlockedItems;
    
    [SerializeField] private GameObject endScreen;
    [SerializeField] private AudioClip endAudio;
    
    void Start()
    {
        _unlockedItems = GetComponent<UnlockedItems>();
        if (_unlockedItems == null)
        {
            Debug.LogError("UnlockedItems component not found on the GameObject.");
            return;
        }

        if (summonPoint == null)
        {
            Debug.LogError("SummonPoint has not been assigned in the Inspector.");
            return;
        }

        _recipes = new List<RecipeSO>(Resources.LoadAll<RecipeSO>("Recipes"));
        if (_recipes == null || _recipes.Count == 0)
        {
            Debug.LogError("No RecipeSO objects found in Resources/Recipes.");
            return;
        }
    }

    
    public void UpdateIngredient(int index, GameObject ingredient)
    {
        if (index is > 2 or < 0)
        {
            Debug.LogError("Ingredient out of index range");
            return;
        }

        currentIngredients[index] = ingredient;
    }
    
    public void TriggerSummon()
    {
        RecipeSO matchedRecipe = CheckRecipe();
        if (matchedRecipe != null)
        {
            ClearIngredients(matchedRecipe);
            
            ConstructItem(matchedRecipe.summon);
            
            //Play success effect
        }
        else
        {
            //Play fail effect
        }
    }

    private void ClearIngredients(RecipeSO recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            ClearIngredient(ingredient);
        }
    
        // Local Functions
        void ClearIngredient(ItemSO ingredient)
        {
            for (int i = 0; i < currentIngredients.Length; i++)
            {
                if (IsIngredientMatch(currentIngredients[i], ingredient))
                {
                    DestroyIngredient(i);
                }
            }
        }

        bool IsIngredientMatch(GameObject ingredientObject, ItemSO ingredient)
        {
            return ingredientObject != null && _itemObjectMap.TryGetValue(ingredientObject, out var itemSO) && itemSO == ingredient;
        }

        void DestroyIngredient(int index)
        {
            GameObject ingredientObject = currentIngredients[index];
            if (ingredientObject != null)
            {
                Destroy(ingredientObject);
                _itemObjectMap.Remove(ingredientObject);
                currentIngredients[index] = null;
            }
        }
    }

    
    private RecipeSO CheckRecipe()
    {
        foreach (var recipe in _recipes)
        {
            Debug.Log(recipe.name);
            if (AllIngredientsMatch(recipe))
            {
                return recipe;
            }
        }

        return null;
    }

    private bool AllIngredientsMatch(RecipeSO recipe)
    {
        Dictionary<ItemSO, int> presentIngredientCount = currentIngredients
            .Where(ingredientObject => ingredientObject != null)
            .Select(ingredientObject => _itemObjectMap[ingredientObject])
            .GroupBy(itemSO => itemSO)
            .ToDictionary(group => group.Key, group => group.Count());

        Dictionary<ItemSO, int> requiredIngredientCount = recipe.ingredients
            .GroupBy(itemSO => itemSO)
            .ToDictionary(group => group.Key, group => group.Count());

        if (presentIngredientCount.Values.Sum() != recipe.ingredients.Count())
        {
            return false;
        }

        foreach (var present in presentIngredientCount)
        {
            Debug.Log("Present Ingredient: " + present.Key.name + ", Count: " + present.Value);
        }

        foreach (var required in requiredIngredientCount)
        {
            Debug.Log("Required Ingredient: " + required.Key.name + ", Count: " + required.Value);
        }

        foreach (var requiredIngredient in requiredIngredientCount)
        {
            if (!presentIngredientCount.TryGetValue(requiredIngredient.Key, out int presentCount) || presentCount < requiredIngredient.Value)
            {
                return false;
            }
        }

        return true;
    }



    private bool ContainsIngredient(ItemSO ingredient)
    {
        if (ingredient == null)
        {
            throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null.");
        }
    
        foreach (var ingredientObject in currentIngredients)
        {
            if (ingredientObject == null)
            {
                continue;
            }

            if (_itemObjectMap.TryGetValue(ingredientObject, out var itemSO) && itemSO == ingredient)
            {
                return true;
            }
        }
        return false;
    }



    private void ClearAllIngredients()
    {
        foreach (var ingredient in currentIngredients)
        {
            Destroy(ingredient);
            currentIngredients = new GameObject[3];
        }
    }
    
    public void ConstructItem(ItemSO item)
    {
        if (item == null || item.prefab == null)
        {
            Debug.LogError("ItemSO or its prefab is null.");
            return;
        }
        
        GameObject newItem = Instantiate(item.prefab, summonPoint.position, Quaternion.identity);
        newItem.name = item.name;
        
        newItem.tag = "Ingredient";

        SpriteRenderer spriteRenderer = newItem.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.sprite;
        
        Rigidbody2D rb2D = newItem.AddComponent<Rigidbody2D>();
        rb2D.gravityScale = item.defaultGravity;

        PolygonCollider2D col = newItem.AddComponent<PolygonCollider2D>();
        col.layerOverridePriority = 10;
        
        Drag dragComponent = newItem.AddComponent<Drag>();
        dragComponent.Configure(item);

        _itemObjectMap[newItem] = item;
        
        _activeItemObjects.Enqueue(newItem);
        CheckItemCount();
        
        _unlockedItems.CheckIfUnlocked(item);

        if (item.itemName == "Mjolnir")
        {
            StartCoroutine(Co_EndScreen());
        }
    }

    IEnumerator Co_EndScreen()
    {
        AudioController.Instance.PlaySound(endAudio);
        yield return new WaitForSeconds(5f);
        endScreen.SetActive(true);
    }

    private void CheckItemCount()
    {
        while (_activeItemObjects.Count > maxItems)
        {
            GameObject itemToRemove = _activeItemObjects.Dequeue();
            Destroy(itemToRemove);
        }
    }
    
}
