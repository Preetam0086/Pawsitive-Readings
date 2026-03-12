using UnityEngine;

public class StatusLight : MonoBehaviour
{
    public Renderer greenLight;
    public Renderer redLight;

    public Material greenOn;
    public Material greenOff;
    public Material redOn;
    public Material redOff;

    public float blinkInterval = 0.4f;
    public int blinkCount = 6;

    float timer;
    int blinksLeft;
    bool isBlinking;
    bool blinkingRed;
    bool lightOn;

    void Start()
    {
        SetGreen(false);
        SetRed(false);
    }

    void Update()
    {
        if (!isBlinking) return;

        timer += Time.deltaTime;

        if (timer >= blinkInterval)
        {
            timer = 0;

            lightOn = !lightOn;

            if (blinkingRed)
                SetRed(lightOn);
            else
                SetGreen(lightOn);

            blinksLeft--;

            if (blinksLeft <= 0)
            {
                isBlinking = false;

                if (blinkingRed)
                {
                    SetRed(false);
                    SetGreen(false);
                }
                else
                {
                    SetGreen(true);
                }
            }
        }
    }

    public void OnPuzzleFailed()
    {
        StopBlinking();

        blinkingRed = true;
        blinksLeft = blinkCount * 2;

        isBlinking = true;
        timer = 0;
    }

    public void OnPuzzleSolved()
    {
        StopBlinking();

        blinkingRed = false;
        blinksLeft = blinkCount * 2;

        isBlinking = true;
        timer = 0;
    }

    void StopBlinking()
    {
        isBlinking = false;

        SetGreen(false);
        SetRed(false);
    }

    void SetGreen(bool on)
    {
        if (greenLight != null)
            greenLight.material = on ? greenOn : greenOff;
    }

    void SetRed(bool on)
    {
        if (redLight != null)
            redLight.material = on ? redOn : redOff;
    }
}