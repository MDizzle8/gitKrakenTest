using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent()]
[RequireComponent(typeof(Collider))]

public class triggerVolume : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private bool _oneShot = true;

    [Header("Filters")]
    [SerializeField]
    private GameObject _specificTriggerObject = null;
    [SerializeField]
    private LayerMask _layersToDetect = -1; //defaults to everything

    [Space(10)]

    public UnityEvent OnEnterTrigger;

    private Collider _collider;

    private bool _alreadyEntered = false;

    [Header("Gizmo Settings")]
    [SerializeField]
    private bool _displayGizmos = false;
    [SerializeField]
    private bool _showOnlyWhileSelected = true;
    [SerializeField]
    private Color _gizmoColor = Color.green;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //validate object
        if (_oneShot && _alreadyEntered)
            return;
        // if we specified an object and it's NOT the same, return
        if (_specificTriggerObject != null
            && other.gameObject != _specificTriggerObject)
            return;
        //(bit shifting and layer masking)if gameobject is NOT in our specified valid layers, return
        if (_layersToDetect != (_layersToDetect | (1 << other.gameObject.layer)))
            return;

        OnEnterTrigger.Invoke();
        _alreadyEntered = true;
        Debug.Log("Entered");
    }

    private void OnDrawGizmos()
    {
        if (_displayGizmos == false)
            return;
        if (_showOnlyWhileSelected == true)
            return;
        if(_collider == null)
        {
            _collider = GetComponent<Collider>();
        }
        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(transform.position, _collider.bounds.size);
    }

    private void OnDrawGizmosSelected()
    {
        if (_displayGizmos == false)
            return;
        if (_showOnlyWhileSelected == false)
            return;
        if (_collider == null)
        {
            _collider = GetComponent<Collider>();
        }
        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(transform.position, _collider.bounds.size);
    }

    public void ResetTrigger()
    {
        _alreadyEntered = false;
    }
}
