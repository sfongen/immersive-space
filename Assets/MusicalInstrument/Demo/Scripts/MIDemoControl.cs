using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeutronCat.MusicalInstrument.Demo
{
    public class MIDemoControl : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float rotateSpeed = 2f;
        public bool canRotate = true;

        Transform _transform;
        GameObject _UIDemoCanvas;
        Vector2 _rotateAngles = Vector2.zero;

        void Start()
        {
            _transform = transform;
            _UIDemoCanvas = GameObject.Find("UIDemoCanvas");
        }

        void Update()
        {
            var _h = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            var _f = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
            var _v = (Input.GetKey(KeyCode.E) ? 1 : 0) + (Input.GetKey(KeyCode.Q) ? -1 : 0);
            _transform.position += (_transform.forward * _f + _transform.right * _h + _transform.up * _v) * moveSpeed * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.R))
                canRotate = !canRotate;
            if (canRotate)
            {
                _rotateAngles.y += Input.GetAxis("Mouse X") * rotateSpeed;
                _rotateAngles.x -= Input.GetAxis("Mouse Y") * rotateSpeed;
                _transform.rotation = Quaternion.Euler(_rotateAngles.x, _rotateAngles.y, 0f);
            }

            if (_UIDemoCanvas != null && Input.GetKeyDown(KeyCode.H))
            {
                _UIDemoCanvas.SetActive(!_UIDemoCanvas.activeInHierarchy);
            }
        }
    }
}