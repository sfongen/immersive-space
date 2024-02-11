using UnityEngine;

public class ButtonGlowActivation : MonoBehaviour
{
    private Material buttonMaterial; // To store the button's material
    private Color originalEmissionColor; // To store the original emission color
    public Color glowColor = Color.white; // Glow color when activated
    public float glowIntensity = 1.5f; // Intensity of the glow
    private bool isActivated = false; // Activation state of the button
    public ParticleSystem HitByParticles;

    void Start()
    {
        buttonMaterial = GetComponent<Renderer>().material;
        originalEmissionColor = buttonMaterial.GetColor("_EmissionColor"); // Get the original emission color
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "dodgeBallActive")
        {
            // Toggle the button's state each time it's hit by an active dodgeball
            ToggleGlow(!isActivated); // Toggle the activation state
        }
    }
    public void ToggleGlow(bool activate)
    {
        isActivated = activate; // Update the activation state
        HitByParticles.Play(); // Play hit particles only when activated

        if (activate)
        {
            buttonMaterial.EnableKeyword("_EMISSION");
            buttonMaterial.SetColor("_EmissionColor", glowColor * glowIntensity); // Set the glow color
        }
        else
        {
            buttonMaterial.SetColor("_EmissionColor", originalEmissionColor); // Revert to the original emission color
            if (originalEmissionColor == Color.black) // If the original emission is black, disable emission
            {
                buttonMaterial.DisableKeyword("_EMISSION");
            }
        }

        // Update the renderer's emission color
        DynamicGI.SetEmissive(GetComponent<Renderer>(), buttonMaterial.GetColor("_EmissionColor"));
    }



    public bool IsActivated()
    {
        return isActivated;
    }

    // Optionally, add a method to reset the button's state
    public void ResetButton()
    {
        ToggleGlow(false);
    }
}
