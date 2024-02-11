using UnityEngine;

public class ButtonSequenceManager : MonoBehaviour
{
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
        foreach (Transform child in transform)
        {
            Debug.Log("Found child: " + child.name); // Log each child's name

            if (child.name == "ButtonRed")
                redButton = child.GetComponent<ButtonGlowActivation>();
            else if (child.name == "ButtonOrange")
                orangeButton = child.GetComponent<ButtonGlowActivation>();
            else if (child.name == "ButtonYellow")
                yellowButton = child.GetComponent<ButtonGlowActivation>();
            else if (child.name == "ButtonGreen")
                greenButton = child.GetComponent<ButtonGlowActivation>();
            else if (child.name == "ButtonIndigo")
                indigoButton = child.GetComponent<ButtonGlowActivation>();
            else if (child.name == "ButtonViolet")
                violetButton = child.GetComponent<ButtonGlowActivation>();

            // Check if the component was found and assigned
            if (child.GetComponent<ButtonGlowActivation>() == null)
                Debug.LogError("ButtonGlowActivation component not found on " + child.name);
        }
    }

    void Update()
    {
        CheckButtonCombinations();
    }

    void CheckButtonCombinations()
    {
        // Check if red, green, and indigo buttons are activated
        if (redButton != null && greenButton != null && indigoButton != null &&
            redButton.IsActivated() && greenButton.IsActivated() && indigoButton.IsActivated())
        {
            Debug.Log("Red, Green, and Indigo buttons activated");
            // Trigger event or action
            TriggerEvent("Red-Green-Indigo activated");
        }
    }

    void TriggerEvent(string combination)
    {
        Debug.Log($"Combination {combination} activated");

        // Implement the logic for what happens when a combination is activated
        // This could involve activating other GameObjects, playing sounds, etc.

        //TODO: Add animation here to indicate correct sequences
        ResetSequence(); //Reset the sequence to prepare for new sequence
    }

     public void ResetSequence()
    {
        // Reset each button
        if (redButton != null) redButton.ResetButton();
        if (orangeButton != null) orangeButton.ResetButton();
        if (yellowButton != null) yellowButton.ResetButton();
        if (greenButton != null) greenButton.ResetButton();
        if (indigoButton != null) indigoButton.ResetButton();
        if (violetButton != null) violetButton.ResetButton();

        // Allow the sequence to be triggered again
        // You might want to reset any other relevant state or variables here
    }
}
