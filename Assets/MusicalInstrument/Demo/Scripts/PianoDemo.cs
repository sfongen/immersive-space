using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeutronCat.MusicalInstrument.Demo
{
    [RequireComponent(typeof(PianoController))]
    public class PianoDemo : MonoBehaviour
    {
        public PianoController piano;

        void Start()
        {
            if (piano == null) piano = GetComponent<PianoController>();

            StartCoroutine("PlayDemo");
        }

        IEnumerator PlayDemo()
        {
            while (true)
            {
                piano.PedalDown(PianoPedal.Soft);
                piano.KeyDown(KeyNote.C4);

                yield return new WaitForSeconds(.5f);

                piano.KeyDown(KeyNote.D4S);

                yield return new WaitForSeconds(.5f);

                piano.KeyDown(KeyNote.G4);

                yield return new WaitForSeconds(1f);

                piano.KeyUp(KeyNote.C4);
                piano.KeyUp(KeyNote.D4S);
                piano.KeyUp(KeyNote.G4);
                piano.PedalUp(PianoPedal.Soft);

                yield return new WaitForSeconds(1f);
            }
        }
    }
}