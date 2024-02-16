using System.Collections.Generic;
using UnityEngine;

public class ButtonSequenceManager : MonoBehaviour
{
    private List<List<ButtonGlowActivation>> sequences = new List<List<ButtonGlowActivation>>();
    private ButtonGlowActivation[] buttons; // To hold references to all button scripts

    // Specific buttons
    private ButtonGlowActivation redButton;
    private ButtonGlowActivation orangeButton;
    private ButtonGlowActivation yellowButton;
    private ButtonGlowActivation greenButton;
    private ButtonGlowActivation indigoButton;
    private ButtonGlowActivation violetButton;

    void Start()
    {
        buttons = GetComponentsInChildren<ButtonGlowActivation>();

        foreach (var button in buttons)
        {
            Debug.Log("Found child: " + button.gameObject.name); // Log each child's name

            switch (button.gameObject.name)
            {
                case "ButtonRed":
                    redButton = button;
                    break;
                case "ButtonOrange":
                    orangeButton = button;
                    break;
                case "ButtonYellow":
                    yellowButton = button;
                    break;
                case "ButtonGreen":
                    greenButton = button;
                    break;
                case "ButtonIndigo":
                    indigoButton = button;
                    break;
                case "ButtonViolet":
                    violetButton = button;
                    break;
                default:
                    Debug.LogError("Unexpected button name: " + button.gameObject.name);
                    break;
            }
        }

        // Define sequences
        sequences.Add(new List<ButtonGlowActivation> { redButton, greenButton, indigoButton }); // Sequence 1
        sequences.Add(new List<ButtonGlowActivation> { orangeButton, violetButton, greenButton }); // Sequence 2
        sequences.Add(new List<ButtonGlowActivation> { yellowButton, indigoButton, redButton }); // Sequence 3
    }

    void Update()
    {
        CheckButtonCombinations();
    }

    void CheckButtonCombinations()
    {
        foreach (var sequence in sequences)
        {
            if (IsSequenceActivated(sequence))
            {
                Debug.Log("A sequence is activated");
                TriggerEvent(sequence);
                break; // Stop checking other sequences once one is found to be activated
            }
        }
    }

    bool IsSequenceActivated(List<ButtonGlowActivation> sequence)
    {
        // Check if all buttons in the sequence are activated
        foreach (var button in sequence)
        {
            if (button == null || !button.IsActivated())
                return false; // If any button in the sequence is not activated or null, the sequence is not activated
        }

        // Check that no other buttons are activated
        foreach (var button in buttons)
        {
            if (!sequence.Contains(button) && button.IsActivated())
                return false; // If any button outside the sequence is activated, the sequence is not activated
        }

        return true; // All checks passed, the sequence is activated
    }

    void TriggerEvent(List<ButtonGlowActivation> activatedSequence)
        {
            Debug.Log("Correct sequence activated. Starting blink animation.");

            // Start the blink animation for each button in the sequence
            foreach (var button in activatedSequence)
            {
                Debug.Log("Blink coroutine started");
                StartCoroutine(button.BlinkAndReset());
            }
        }

    public void ResetSequence()
    {
        // Reset each button
        foreach (var button in buttons)
        {
            if (button != null) button.ResetButton();
        }
    }
}
