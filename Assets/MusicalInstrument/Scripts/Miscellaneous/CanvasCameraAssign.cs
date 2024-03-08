using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeutronCat.MusicalInstrument
{
    public class CanvasCameraAssign : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
            Destroy(this);
        }
    }
}