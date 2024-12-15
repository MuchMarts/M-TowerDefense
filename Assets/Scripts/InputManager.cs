using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
public class InputManager : MonoBehaviour
{

    [SerializeField]
    private Camera playerCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayerMask, towerLayerMask;

    [SerializeField]
    private LayerMask pathLayerMask;

    public event Action OnClicked, OnExit;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public bool IsOverTower(Vector3 position)
    {
        Collider[] intersectingTowers = Physics.OverlapSphere(position, 0.01f, towerLayerMask);
        if (intersectingTowers.Length == 0) return true;
        return false;  
    }

    public bool IsOverPath(Vector3 position)
    {
        position.y = 0f;
        Collider[] intersectingPath = Physics.OverlapSphere(position, 0.01f, pathLayerMask);
        if (intersectingPath.Length == 0) return true;
        return false;
    }

    public bool IsPlacingTower { get; set; }

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = playerCamera.nearClipPlane;
        Ray ray = playerCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, placementLayerMask))
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
    
}
