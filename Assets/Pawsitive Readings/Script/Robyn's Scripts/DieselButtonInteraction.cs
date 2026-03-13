using UnityEngine;

public class DieselButtonInteraction : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        Debug.Log("Play Animation");
        gameObject.GetComponent<Animator>().Play("Push");
        gameObject.GetComponent<AudioSource>().Play();
    }//end Interact


}//end of diesel button interaction
