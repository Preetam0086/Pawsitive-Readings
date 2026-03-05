using UnityEngine;

public class drawerinteraction : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public int blendShapeIndex = 0;
    public float openSpeed = 5f;

    public KeyCode interactKey = KeyCode.E;

    private float targetWeight = 0f;
   private bool isOpen = false;

    void Update()
    {
        // Smooth opening animation
        float currentWeight = skinnedMesh.GetBlendShapeWeight(blendShapeIndex);
        float newWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * openSpeed);
        skinnedMesh.SetBlendShapeWeight(blendShapeIndex, newWeight);

        // Interaction key
        if (Input.GetKeyDown(interactKey))
        {
            ToggleDrawer();
        }
    }

    void ToggleDrawer()
    {
        isOpen = !isOpen;

        if (isOpen)
            targetWeight = 100f; 
        else
            targetWeight = 0f;   
    }
}
