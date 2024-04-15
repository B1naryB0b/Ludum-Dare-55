using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemSO item;
    public GameObject toolTipBox;
    public Button button;

    private const float HoverTimeToActivate = 0.5f;

    private bool _isToolTipActive;
    private Coroutine _hoverCoroutine;

    private void Awake()
    {
        if (button != null)
        {
            button.gameObject.AddComponent<EventTrigger>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        _hoverCoroutine = StartCoroutine(Co_ShowToolTipAfterDelay());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        if (_hoverCoroutine != null)
        {
            StopCoroutine(_hoverCoroutine);
            _hoverCoroutine = null;
        }
        HideToolTip();
    }

    private IEnumerator Co_ShowToolTipAfterDelay()
    {
        Debug.Log("Hover");
        yield return new WaitForSeconds(HoverTimeToActivate);
        Debug.Log("Trigger");
        ShowToolTip();
    }

    private void ShowToolTip()
    {
        TextMeshProUGUI[] text = toolTipBox.GetComponentsInChildren<TextMeshProUGUI>();
        text[0].text = item.itemName;
        text[1].text = item.type;
        text[2].text = item.flavourText;
        
        toolTipBox.SetActive(true);
        _isToolTipActive = true;
    }

    private void HideToolTip()
    {
        toolTipBox.SetActive(false);
        _isToolTipActive = false;
    }

}