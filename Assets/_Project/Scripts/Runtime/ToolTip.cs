using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public ItemSO item;
    public GameObject toolTipBox;
    
    private float _hoverTimeToActivate = 1f; // Time in seconds to wait before showing tooltip
    private float _mouseMovementThreshold = 0.1f; // Threshold for mouse movement

    private bool _isToolTipActive;
    private Vector3 _lastMousePosition;
    private Coroutine _hoverCoroutine;

    private void Awake()
    {
        gameObject.AddComponent<BoxCollider2D>();
    }

    private void OnMouseEnter()
    {
        Debug.Log("Enter");
        _lastMousePosition = Input.mousePosition;
        _hoverCoroutine = StartCoroutine(Co_ShowToolTipAfterDelay());
    }

    private void OnMouseExit()
    {
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
        yield return new WaitForSeconds(_hoverTimeToActivate);

        if (Vector3.Distance(_lastMousePosition, Input.mousePosition) < _mouseMovementThreshold)
        {
            ShowToolTip();
        }
    }

    private void ShowToolTip()
    {
        toolTipBox.SetActive(true);
        _isToolTipActive = true;
    }

    private void HideToolTip()
    {
        toolTipBox.SetActive(false);
        _isToolTipActive = false;
    }
}
