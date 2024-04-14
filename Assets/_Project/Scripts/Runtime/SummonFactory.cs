using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SummonFactory : MonoBehaviour
{

    [SerializeField] private Transform summonPoint;
    [SerializeField] private List<ItemSO> startingItems;
    [SerializeField] private int maxItems;

    private Queue<GameObject> _activeItemObjects = new Queue<GameObject>();

    [SerializeField] private GameObject[] currentIngredients = new GameObject[3];

    private List<RecipeSO> _recipes;
    
    private Dictionary<GameObject, ItemSO> _itemObjectMap = new Dictionary<GameObject, ItemSO>();

    
    void Start()
    {
        _recipes = new List<RecipeSO>(Resources.LoadAll<RecipeSO>("Recipes"));
        Debug.Log(_recipes[0].name);
        
        foreach (var item in startingItems)
        {
            ConstructItem(item);
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
            bool allIngredientsMatch = true;
            foreach (var ingredientSO in recipe.ingredients)
            {
                if (!ContainsIngredient(ingredientSO))
                {
                    allIngredientsMatch = false;
                    break;
                }
            }

            if (allIngredientsMatch)
            {
                return recipe;
            }
        }

        return null;
    }

    private bool ContainsIngredient(ItemSO ingredient)
    {
        foreach (var ingredientObject in currentIngredients)
        {
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
        GameObject newItem = new GameObject(item.itemName);
        newItem.transform.position = summonPoint.position;

        newItem.tag = "Ingredient";

        SpriteRenderer spriteRenderer = newItem.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.sprite;

        newItem.AddComponent<PolygonCollider2D>();

        Rigidbody2D rb2D = newItem.AddComponent<Rigidbody2D>();
        rb2D.gravityScale = item.defaultGravity;

        Drag dragComponent = newItem.AddComponent<Drag>();
        dragComponent.Configure(item);

        _itemObjectMap[newItem] = item;
        
        _activeItemObjects.Enqueue(newItem);
        CheckItemCount();
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
