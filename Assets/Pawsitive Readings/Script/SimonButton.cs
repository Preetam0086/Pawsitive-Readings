using UnityEngine;
using System.Collections;

public class SimonButton : MonoBehaviour
{
    public SimonPuzzleMachine puzzleMachine;

    [Header("Button ID")]
    public int buttonID;
    // 0 heart
    // 1 triangle
    // 2 square
    // 3 star
    // 4 start
    // 5 reset

    [Header("Blendshape Animation")]
    public SkinnedMeshRenderer machineRenderer;
    public int blendShapeIndex;

    bool isAnimating = false;

    public void PressButton()
    {
        if (isAnimating) return;

        StartCoroutine(PressAnimation());

        if (puzzleMachine != null)
            puzzleMachine.ReceiveButtonInput(buttonID);
    }

    public void PlayAnimation()
    {
        if (isAnimating) return;

        StartCoroutine(PressAnimation());
    }

    IEnumerator PressAnimation()
    {
        isAnimating = true;

        machineRenderer.SetBlendShapeWeight(blendShapeIndex, 100);
        yield return new WaitForSeconds(0.2f);
        machineRenderer.SetBlendShapeWeight(blendShapeIndex, 0);

        isAnimating = false;
    }
}