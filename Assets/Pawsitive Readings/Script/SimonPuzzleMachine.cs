using UnityEngine;
using System.Collections;
using TMPro;

public class SimonPuzzleMachine : MonoBehaviour
{
    [Header("Player Camera")]
    public Camera playerCamera;
    public float interactDistance = 3f;

    [Header("Buttons")]
    public SimonButton[] buttons;

    [Header("Lights")]
    public Renderer greenLight;
    public Renderer redLight;

    public Material greenOn;
    public Material greenOff;
    public Material redOn;
    public Material redOff;

    [Header("Code Display")]
    public GameObject codePanel;
    public TMP_Text codeText;
    public float codeDisplayTime = 4f;

    int[] sequence;
    int currentInput;

    bool sequencePlaying = false;
    bool playerTurn = false;

    void Start()
    {
        codePanel.SetActive(false);
        SetGreen(false);
        SetRed(false);
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
 

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (Input.GetMouseButtonDown(0))
            {
                SimonButton button = hit.collider.GetComponentInParent<SimonButton>();

                if (button != null)
                    button.PressButton();
            }
        }
    }

    public void ReceiveButtonInput(int buttonID)
    {
        // Start button
        if (buttonID == 4)
        {
            if (!sequencePlaying && !playerTurn)
                StartCoroutine(StartSequence());

            return;
        }

        // Reset button
        if (buttonID == 5)
        {
            ResetPuzzle();
            return;
        }

        if (!playerTurn) return;

        if (buttonID == sequence[currentInput])
        {
            currentInput++;

            if (currentInput >= sequence.Length)
                StartCoroutine(PuzzleSolved());
        }
        else
        {
            StartCoroutine(PuzzleFailed());
        }
    }

    IEnumerator StartSequence()
    {
        sequence = new int[4];

        for (int i = 0; i < sequence.Length; i++)
            sequence[i] = Random.Range(0, 4);

        sequencePlaying = true;

        yield return new WaitForSeconds(0.5f);

        foreach (int id in sequence)
        {
            buttons[id].PlayAnimation();
            yield return new WaitForSeconds(0.8f);
        }

        sequencePlaying = false;
        playerTurn = true;
        currentInput = 0;
    }

    IEnumerator PuzzleSolved()
    {
        playerTurn = false;

        for (int i = 0; i < 6; i++)
        {
            SetGreen(true);
            yield return new WaitForSeconds(0.3f);
            SetGreen(false);
            yield return new WaitForSeconds(0.3f);
        }

        ShowRandomCode();
    }

    IEnumerator PuzzleFailed()
    {
        playerTurn = false;

        for (int i = 0; i < 6; i++)
        {
            SetRed(true);
            yield return new WaitForSeconds(0.3f);
            SetRed(false);
            yield return new WaitForSeconds(0.3f);
        }
    }

    void ResetPuzzle()
    {
        sequencePlaying = false;
        playerTurn = false;
        currentInput = 0;

        SetGreen(false);
        SetRed(false);
    }

    void SetGreen(bool state)
    {
        greenLight.material = state ? greenOn : greenOff;
    }

    void SetRed(bool state)
    {
        redLight.material = state ? redOn : redOff;
    }

    void ShowRandomCode()
    {
        int code = Random.Range(1000, 9999);

        codeText.text = code.ToString();
        StartCoroutine(CodeDisplay());
    }

    IEnumerator CodeDisplay()
    {
        codePanel.SetActive(true);
        yield return new WaitForSeconds(codeDisplayTime);
        codePanel.SetActive(false);
    }
}