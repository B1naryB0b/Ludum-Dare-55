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
        _summonFactory = GetComponent<SummonFactory>();
        
        foreach (var item in _summonFactory.StartingItems)
        {
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
