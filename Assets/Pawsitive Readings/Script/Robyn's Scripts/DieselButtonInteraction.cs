using UnityEngine;

public class DieselButtonInteraction : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        gameObject.GetComponent<Animator>().Play("Push");
        gameObject.GetComponent<AudioSource>().Play();
    }//end Interact


}//end of diesel button interaction
