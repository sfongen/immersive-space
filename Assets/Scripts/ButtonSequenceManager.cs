using System.Collections.Generic;
using System;

using UnityEngine;

public class ButtonSequenceManager : MonoBehaviour
{
    private List<List<ButtonGlowActivation>> sequences = new List<List<ButtonGlowActivation>>();
    private ButtonGlowActivation[] buttons; // To hold references to all button scripts
    public AudioSource[] speakers; // Array to hold references to the speaker audio sources
    public ParticleSystem[] musicNoteParticleSystems; // Array to hold references to the music note Particle Systems


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

        // Initialize speakers and animations (assuming they are already set in the Inspector)
        foreach (var speaker in speakers)
        {
            if (speaker != null)
            {
                speaker.loop = true;
                speaker.mute = true; // Ensure this line is present and correct
                speaker.Play(); // This will start playing the audio in a muted state
                Debug.Log("Speaker muted: " + speaker.gameObject.name); // Add this line
            }
        }
        // Stop all music note Particle Systems at the start
        foreach (var musicNotes in musicNoteParticleSystems)
        {
            if (musicNotes != null)
            {
                musicNotes.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
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
        Debug.Log("Correct sequence activated. Starting blink animation, sound, and music notes.");

        // Start the blink animation for each button in the sequence
        foreach (var button in activatedSequence)
        {
            StartCoroutine(button.BlinkAndReset());
        }

        // Unmute speakers, start sound, and start music note Particle Systems for the activated sequence
        foreach (var button in activatedSequence)
        {
            int index = Array.IndexOf(buttons, button); // Get the index of the button in the global buttons array
            if (index != -1 && index < speakers.Length)
            {
                var speaker = speakers[index];
                var musicNotes = musicNoteParticleSystems[index];

                speaker.mute = false; // Unmute the speaker

                if (musicNotes != null)
                {
                    musicNotes.Play(); // Start the corresponding music note Particle System
                }
            }
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
