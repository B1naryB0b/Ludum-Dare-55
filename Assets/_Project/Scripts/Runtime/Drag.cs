using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Drag : MonoBehaviour
{
    private float _velocityScaling;
    private float _maxVelocity;
    private float _defaultGravity;
    
    private Vector3 _offset;
    private bool _isDragging = false;

    private Rigidbody2D _rigidbody2D;
    private Camera _mainCamera;

    private CursorController _cc;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _cc = CursorController.Instance;
    }

    private void Update()
    {
        if (_isDragging)
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPosition = mousePosition + _offset;
            targetPosition.z = 0f;
            
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            float dampingFactor = Mathf.Clamp01(distanceToTarget * _velocityScaling);
            float scaledVelocity = dampingFactor * _maxVelocity;

            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, scaledVelocity * Time.deltaTime);
            _rigidbody2D.velocity = (newPosition - transform.position) / Time.deltaTime;
            transform.position = newPosition;
        }
    }


    public void Configure(ItemSO item)
    {
        _velocityScaling = item.velocityScaling;
        _maxVelocity = item.maxVelocity;
        _defaultGravity = item.defaultGravity;
    }

    private void OnMouseOver()
    {
        if (_cc == null) Debug.LogError("CC is null");
        if (_cc.hover == null) Debug.LogError("CC.hover is null");
        
        if (_isDragging) return;
        Cursor.SetCursor(_cc.hover, _cc.hotspot, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        if (_isDragging) return;
        Cursor.SetCursor(_cc.pointer, _cc.hotspot, CursorMode.Auto);
    }

    private void OnMouseDown()
    {
        if (Camera.main != null) _offset = transform.position - _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        _isDragging = true;

        _rigidbody2D.gravityScale = 0f;

        Cursor.SetCursor(_cc.grab, _cc.hotspot, CursorMode.Auto);
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        
        _rigidbody2D.gravityScale = _defaultGravity;
        
        Cursor.SetCursor(_cc.pointer, _cc.hotspot, CursorMode.Auto);

    }
}
