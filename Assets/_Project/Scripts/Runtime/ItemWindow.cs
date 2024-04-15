using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemWindow : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject itemPrefab;

    [FormerlySerializedAs("gridOffset")] [SerializeField] private Vector2 offset;
    [FormerlySerializedAs("gridSpacing")] [SerializeField] private Vector2 spacing;

    [SerializeField] private GameObject toolTipPrefab;

    private SummonFactory _summonFactory;
    private List<ItemSO> _items;

    void Start()
    {
        _summonFactory = GetComponent<SummonFactory>();
    }

    public void LoadItems(List<ItemSO> items)
    {
        _items = items;
    }
    
    public void DrawItems()
    {
        content.DestroyAllChildren();
        
        for (int i = 0; i < _items.Count; i++)
        {
            int x = i % (int)(content.GetComponent<RectTransform>().rect.width / spacing.x);
            int y = i / (int)(content.GetComponent<RectTransform>().rect.width / spacing.x);

            Vector2 position = new Vector2(x * spacing.x + offset.x, -y * spacing.y - offset.y);

            GameObject itemObject = Instantiate(itemPrefab, content.transform);

            RectTransform rectTransform = itemObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position;

            Image image = itemObject.GetComponentsInChildren<Image>(true).FirstOrDefault(img => img.gameObject != itemObject);
            image.sprite = _items[i].sprite;
            
            Button button = itemObject.GetComponent<Button>();
            ItemSO currentItem = _items[i];
            button.onClick.AddListener(() => OnItemClick(currentItem));

            ToolTip toolTip = itemObject.AddComponent<ToolTip>();
            toolTip.item = _items[i];
            toolTip.toolTipBox = toolTipPrefab;
        }
    }

    private void OnItemClick(ItemSO item)
    {
        _summonFactory.ConstructItem(item);
    }
}