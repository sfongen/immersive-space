using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGlowActivation : MonoBehaviour
{
    public Material glowMaterial; // Material to use when the capsule glows
    public Material darkMaterial; // Material to revert the capsule to when it's not glowing
    public Material lightGreenMaterial; // Material to use for the brief green light effect
    private bool isActivated = false; // Activation state of the button
    public ParticleSystem HitByParticles;

    public GameObject associatedCapsule; // Direct reference to the associated capsule GameObject

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "dodgeBallActive" && associatedCapsule != null)
        {
            HitByParticles.Play(); // Play particle effect on hit
            ToggleActivation(); // Toggle the activation state of the button
        }
    }

    void ToggleActivation()
    {
        isActivated = !isActivated; // Toggle the activation state
        ToggleCapsuleGlow(associatedCapsule, isActivated); // Change the capsule's material based on the new state
    }

    public void ToggleCapsuleGlow(GameObject capsule, bool activate)
    {
        Renderer renderer = capsule.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = activate ? glowMaterial : darkMaterial;
        }
    }

    public bool IsActivated()
    {
        return isActivated;
    }

    public void ResetButton()
    {
        isActivated = false; // Reset the activation state
        ToggleCapsuleGlow(associatedCapsule, false); // Reset the capsule to dark material
    }

    public static IEnumerator BriefGreenLightAndResetAll(List<ButtonGlowActivation> allButtons, Material lightGreenMaterial, Material darkMaterial)
    {
        // Apply light green material to all capsules
        foreach (var button in allButtons)
        {
            if (button.associatedCapsule != null)
            {
                Renderer capsuleRenderer = button.associatedCapsule.GetComponent<Renderer>();
                capsuleRenderer.material = lightGreenMaterial; // Use the light green material
            }
        }

        yield return new WaitForSeconds(0.5f); // Wait for half a second

        // Reset all buttons and capsules to their default states
        foreach (var button in allButtons)
        {
            button.ResetButton(); // This will reset the activation state and apply the dark material
        }
    }
}
