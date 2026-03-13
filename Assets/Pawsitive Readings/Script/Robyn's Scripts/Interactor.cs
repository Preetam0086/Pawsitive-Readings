using System;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}//end IInteractable

public class Interactor : MonoBehaviour
{

    //variables
    public Transform interactorSource;
    public float interactorRange;
    //end variables

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Input");
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            Debug.DrawRay(interactorSource.position, interactorSource.forward * interactorRange, Color.red, 1.0f); // Visualize ray
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactorRange))
            {
                Debug.Log("Raycast Hit");
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    Debug.Log("Object Found");
                    interactObj.Interact();
                }//end if
            }//end if
        }//end if

    }//end Update

}//end Interactor class
