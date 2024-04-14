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
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isDragging)
        {
            var position = transform.position;
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPosition = mousePosition + _offset;
            targetPosition.z = position.z;

            float distanceToTarget = Vector3.Distance(position, targetPosition);
            float scaledVelocity = Mathf.Clamp(distanceToTarget * _velocityScaling, 0f, _maxVelocity);

            _rigidbody2D.velocity = Vector3.MoveTowards(_rigidbody2D.velocity, (targetPosition - position).normalized * scaledVelocity, scaledVelocity * Time.deltaTime);
        }
    }

    public void Configure(ItemSO item)
    {
        _velocityScaling = item.velocityScaling;
        _maxVelocity = item.maxVelocity;
        _defaultGravity = item.defaultGravity;
    }

    private void OnMouseDown()
    {
        if (Camera.main != null) _offset = transform.position - _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        _isDragging = true;

        _rigidbody2D.gravityScale = 0f;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        
        _rigidbody2D.gravityScale = _defaultGravity;
    }
}
