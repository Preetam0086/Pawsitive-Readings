using UnityEngine;

public class ValvePuzzleManager : MonoBehaviour
{
    [Header("Tank Values")]
    public int tankA;
    public int tankB;
    public int tankC;

    [Header("Target Values")]
    public int targetA = 5;
    public int targetB = 4;
    public int targetC = 1;

    [Header("Settings")]
    public int maxAmount = 10;

    [Header("Valve References")]
    public Valve valve1;
    public Valve valve2;
    public Valve valve3;

    [Header("Elevator Door")]
    public SkinnedMeshRenderer elevatorRenderer;
    public int blendShapeIndex = 0;
    public float openValue = 100f;

    private bool puzzleSolved = false;

    void Start()  // Stays as Start() — Valves have already run Awake() by now
    {
        ResetPuzzle();
    }

    public void RegisterValveTurn(int valveID)
    {
        if (puzzleSolved) return;

        switch (valveID)
        {
            case 1:
                tankA += 2;
                break;
            case 2:
                tankA += 1;
                tankB += 1;
                break;
            case 3:
                tankB += 1;
                tankC += 1;
                break;
        }

        UpdateLiquids();
        CheckState();
    }

    void UpdateLiquids()
    {
        if (valve1 != null)
            valve1.SetLiquidLevel(tankA, maxAmount);
        if (valve2 != null)
            valve2.SetLiquidLevel(tankB, maxAmount);
        if (valve3 != null)
            valve3.SetLiquidLevel(tankC, maxAmount);
    }

    void CheckState()
    {
        // WIN
        if (tankA == targetA &&
            tankB == targetB &&
            tankC == targetC)
        {
            SolvePuzzle();
            return;
        }

        // OVERFLOW
        if (tankA > maxAmount ||
            tankB > maxAmount ||
            tankC > maxAmount)
        {
            Debug.Log("Overflow! Resetting puzzle...");
            ResetPuzzle();
        }
    }

    void SolvePuzzle()
    {
        puzzleSolved = true;
        Debug.Log("Puzzle Solved!");

        if (elevatorRenderer != null)
            elevatorRenderer.SetBlendShapeWeight(blendShapeIndex, openValue);
    }

    void ResetPuzzle()
    {
        tankA = 0;
        tankB = 0;
        tankC = 0;

        UpdateLiquids();

        // Safe to call ResetValve() now since Awake() has already set baseRotation
        if (valve1 != null) valve1.ResetValve();
        if (valve2 != null) valve2.ResetValve();
        if (valve3 != null) valve3.ResetValve();
    }
}