using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemWindow : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject itemPrefab;

    [SerializeField] private Vector2 gridOffset;
    [SerializeField] private Vector2 gridSpacing;

    private SummonFactory _summonFactory;
    private List<ItemSO> _items;

    void Start()
    {
        _summonFactory = FindObjectOfType<SummonFactory>();
        _items = new List<ItemSO>(Resources.LoadAll<ItemSO>("Items"));

        content.DestroyAllChildren();

        DrawItems(content, gridOffset, gridSpacing);
    }

    private void DrawItems(GameObject parent, Vector2 offset, Vector2 spacing)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            int x = i % (int)(parent.GetComponent<RectTransform>().rect.width / spacing.x);
            int y = i / (int)(parent.GetComponent<RectTransform>().rect.width / spacing.x);

            Vector2 position = new Vector2(x * spacing.x + offset.x, -y * spacing.y - offset.y);

            GameObject itemObject = Instantiate(itemPrefab, parent.transform);

            RectTransform rectTransform = itemObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position;

            Image image = itemObject.GetComponent<Image>();
            image.sprite = _items[i].sprite;
            
            Button button = itemObject.GetComponent<Button>();
            ItemSO currentItem = _items[i];
            button.onClick.AddListener(() => OnItemClick(currentItem));
        }
    }

    private void OnItemClick(ItemSO item)
    {
        _summonFactory.ConstructItem(item);
    }
}