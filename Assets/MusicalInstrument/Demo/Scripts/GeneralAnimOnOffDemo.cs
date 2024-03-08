using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeutronCat.MusicalInstrument.Demo
{
    [RequireComponent(typeof(Animator))]
    public class GeneralAnimOnOffDemo : MonoBehaviour
    {
        public Animator animator;
        public float onDuration = .5f;
        public float offDuration = 1f;

        void Start()
        {
            if (animator == null) animator = GetComponent<Animator>();

            StartCoroutine("PlayDemo");
        }

        IEnumerator PlayDemo()
        {
            while (true)
            {
                animator.SetTrigger("On");
                yield return new WaitForSeconds(onDuration);
                animator.SetTrigger("Off");
                yield return new WaitForSeconds(offDuration);
            }
        }
    }
}