using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnlockedItems : MonoBehaviour
{
    
    private List<ItemSO> _unlockedItems = new List<ItemSO>();

    private SummonFactory _summonFactory;
    private ItemWindow _itemWindow;
    
    // Start is called before the first frame update
    void Start()
    {
        _itemWindow = GetComponent<ItemWindow>();
        if (_itemWindow == null)
        {
            Debug.LogError("ItemWindow component not found on the GameObject.");
            return;
        }

        _summonFactory = GetComponent<SummonFactory>();
        if (_summonFactory == null)
        {
            Debug.LogError("SummonFactory component not found on the GameObject.");
            return;
        }

        if (_summonFactory.StartingItems == null)
        {
            Debug.LogError("StartingItems in SummonFactory is null.");
            return;
        }

        foreach (var item in _summonFactory.StartingItems)
        {
            if (item == null)
            {
                Debug.LogError("An item in StartingItems is null.");
                continue;
            }

            if (!_unlockedItems.Contains(item))
            {
                _unlockedItems.Add(item);
            }

            _summonFactory.ConstructItem(item);
        }

        _itemWindow.LoadItems(_unlockedItems);
        _itemWindow.DrawItems();
    }


    
    public void CheckIfUnlocked(ItemSO item)
    {
        if (_unlockedItems.Contains(item)) return;
        
        _unlockedItems.Add(item);
        
        _itemWindow.LoadItems(_unlockedItems);
        _itemWindow.DrawItems();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
