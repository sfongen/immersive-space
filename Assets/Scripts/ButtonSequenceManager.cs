using System.Collections.Generic;
using UnityEngine;

public class ButtonSequenceManager : MonoBehaviour
{
    private List<List<ButtonGlowActivation>> sequences = new List<List<ButtonGlowActivation>>();
    private ButtonGlowActivation[] buttons;
    public AudioSource[] speakers;
    public ParticleSystem[] musicNoteParticleSystems;

    // Specific buttons
    private ButtonGlowActivation redButton, orangeButton, yellowButton, greenButton, indigoButton, violetButton;

    void Start()
{
    buttons = GetComponentsInChildren<ButtonGlowActivation>();

    foreach (var button in buttons)
    {
        switch (button.gameObject.name)
        {
            case "ButtonRed": redButton = button; break;
            case "ButtonOrange": orangeButton = button; break;
            case "ButtonYellow": yellowButton = button; break;
            case "ButtonGreen": greenButton = button; break;
            case "ButtonIndigo": indigoButton = button; break;
            case "ButtonViolet": violetButton = button; break;
            // Add case for "ButtonBlue": blueButton = button; break; if you have a blue button
            default: Debug.LogError("Unexpected button name: " + button.gameObject.name); break;
        }
    }

    // Define sequences with 3 unique colors each
    sequences.Add(new List<ButtonGlowActivation> { redButton, greenButton, indigoButton }); // Adjust if blueButton is not defined
    sequences.Add(new List<ButtonGlowActivation> { orangeButton, indigoButton, yellowButton });
    sequences.Add(new List<ButtonGlowActivation> { yellowButton, violetButton, redButton });
    sequences.Add(new List<ButtonGlowActivation> { greenButton, orangeButton, violetButton });
    sequences.Add(new List<ButtonGlowActivation> { indigoButton, redButton, yellowButton });
    sequences.Add(new List<ButtonGlowActivation> { violetButton, greenButton, indigoButton });

    InitializeAudioAndParticles();
}

    void Update()
    {
        CheckButtonCombinations();
    }

    private void InitializeAudioAndParticles()
    {
        foreach (var speaker in speakers)
        {
            if (speaker != null)
            {
                speaker.loop = true;
                speaker.mute = true;
                speaker.Play();
            }
        }

        foreach (var musicNotes in musicNoteParticleSystems)
        {
            if (musicNotes != null)
            {
                musicNotes.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    private void CheckButtonCombinations()
    {
        for (int i = 0; i < sequences.Count; i++)
        {
            if (IsSequenceActivated(sequences[i]))
            {
                TriggerEvent(i); // Pass the index of the activated sequence
                break;
            }
        }
    }

    private bool IsSequenceActivated(List<ButtonGlowActivation> sequence)
    {
        foreach (var button in sequence)
        {
            if (button == null || !button.IsActivated()) return false;
        }

        foreach (var button in buttons)
        {
            if (!sequence.Contains(button) && button.IsActivated()) return false;
        }

        return true;
    }

    private void TriggerEvent(int sequenceIndex)
    {
        if (sequenceIndex < speakers.Length)
        {
            // Activate the speaker and music notes for the corresponding sequence
            var speaker = speakers[sequenceIndex];
            var musicNotes = musicNoteParticleSystems[sequenceIndex];

            speaker.mute = false; // Unmute the speaker
            if (musicNotes != null) musicNotes.Play();
        }

        // Start the blink animation for each button in the activated sequence
        foreach (var button in sequences[sequenceIndex])
        {
            StartCoroutine(button.BlinkAndReset());
        }
    }

    public void ResetSequence()
    {
        foreach (var button in buttons)
        {
            if (button != null) button.ResetButton();
        }
    }
}
