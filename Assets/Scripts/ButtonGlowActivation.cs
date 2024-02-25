using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGlowActivation : MonoBehaviour
{
    // public Material buttonMaterial; // You might not need this unless it's used elsewhere
    public Material glowMaterial; // Material to use when the capsule glows
    public Material darkMaterial; // Material to revert the capsule to when it's not glowing
    private bool isActivated = false; // Activation state of the button
    public ParticleSystem HitByParticles;

    void Start()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "dodgeBallActive")
        {
            ToggleGlow(!isActivated); // Toggle the activation state
        }
    }

    public void ToggleGlow(bool activate)
    {
        isActivated = activate;
        Transform capsule = transform.Find("Capsule"); // Ensure your capsules are named "Capsule"

        if (capsule != null)
        {
            Renderer capsuleRenderer = capsule.GetComponent<Renderer>();
            if (activate)
            {
                capsuleRenderer.material = glowMaterial; // Use the glow material
            }
            else
            {
                capsuleRenderer.material = darkMaterial; // Revert to the dark material
            }
        }

        if (activate && HitByParticles != null)
        {
            HitByParticles.Play(); // Play hit particles only when activated
        }
    }

    public bool IsActivated()
    {
        return isActivated;
    }

    public void ResetButton()
    {
        ToggleGlow(false);
    }

    public static IEnumerator BriefGreenLightAndResetAll(List<ButtonGlowActivation> allButtons)
    {
        Color lightGreen = new Color(0.5f, 1f, 0.5f); // Define light green color
        float duration = 0.5f; // Duration for light green state

        // Set all capsules to light green
        foreach (var button in allButtons)
        {
            Transform capsule = button.transform.Find("Capsule");
            if (capsule != null)
            {
                Renderer capsuleRenderer = capsule.GetComponent<Renderer>();
                capsuleRenderer.material.EnableKeyword("_EMISSION");
                capsuleRenderer.material.SetColor("_EmissionColor", lightGreen);
            }
        }

        yield return new WaitForSeconds(duration);

        // Reset all capsules to dark
        foreach (var button in allButtons)
        {
            button.ResetButton(); // This will now reset the capsule's color to dark
        }
    }
}
