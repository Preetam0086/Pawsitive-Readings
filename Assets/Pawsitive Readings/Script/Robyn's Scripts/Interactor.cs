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
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactorRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }//end if
            }//end if
        }//end if

    }//end Update

}//end Interactor class
