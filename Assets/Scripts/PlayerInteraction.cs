﻿using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private Vector3 interactionRayPoint;
    [SerializeField] private float interactionRayDistance;
    private Interactable currentInteractable;

    private Camera _mainCamera;

    #endregion

    #region UNITY EVENT FUNCTIONS

    private void Start()
    {
        _mainCamera = Camera.main;

    }

    private void Update()
    {
        HandleInteractionCheck();

        if (Input.GetKeyDown(interactionKey)) HandleInteractionInput();
    }

    #endregion

    #region METHOODS

    //Interaction

    private void HandleInteractionCheck()
    {
        Debug.DrawRay(_mainCamera.ViewportPointToRay(interactionRayPoint).origin, _mainCamera.ViewportPointToRay(interactionRayPoint).direction * interactionRayDistance, Color.red);
        if (Physics.Raycast(_mainCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hitInfo, interactionRayDistance, interactionLayer))
        {
            if (currentInteractable != null && hitInfo.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID())
            {
                currentInteractable.OnLoseFocus();
                currentInteractable = null;
            }

            if (currentInteractable == null)
            {
                hitInfo.collider.TryGetComponent(out currentInteractable);

                if (currentInteractable) currentInteractable.OnFocus();
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }

    private void HandleInteractionInput()
    {
        if (currentInteractable != null && Physics.Raycast(_mainCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hitInfo, interactionRayDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
    }

    #endregion
}

#region Backup

/*#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            private PlayerInput _playerInput;
    #endif*/

#region Is Current Device Mouse
/* {
     get
     {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
         return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                 return false;
#endif
     }
 }*/
#endregion

#endregion