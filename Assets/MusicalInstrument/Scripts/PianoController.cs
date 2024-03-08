using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeutronCat.MusicalInstrument
{
    public enum KeyboardAction
    {
        KeyDown, KeyUp, PedalDown, PedalUp
    }

    public enum KeyNote
    {
        C0, C0S, D0, D0S, E0, F0, F0S, G0, G0S, A0, A0S, B0,
        C1, C1S, D1, D1S, E1, F1, F1S, G1, G1S, A1, A1S, B1,
        C2, C2S, D2, D2S, E2, F2, F2S, G2, G2S, A2, A2S, B2,
        C3, C3S, D3, D3S, E3, F3, F3S, G3, G3S, A3, A3S, B3,
        C4, C4S, D4, D4S, E4, F4, F4S, G4, G4S, A4, A4S, B4,
        C5, C5S, D5, D5S, E5, F5, F5S, G5, G5S, A5, A5S, B5,
        C6, C6S, D6, D6S, E6, F6, F6S, G6, G6S, A6, A6S, B6,
        C7, C7S, D7, D7S, E7, F7, F7S, G7, G7S, A7, A7S, B7,
        C8, C8S, D8, D8S, E8, F8, F8S, G8, G8S, A8, A8S, B8
    }

    public enum PianoPedal
    {
        Soft, Sostenuto, Sustain
    }

    public struct PianoCommand
    {
        public PianoCommand(KeyboardAction action, int arg)
        {
            Action = action;
            Arg = arg;
        }

        public KeyboardAction Action { get; }
        public int Arg { get; }
    }

    public class PianoController : MonoBehaviour
    {
        [Header("Keyboard")]
        public Transform[] keys;
        public KeyNote startKey = KeyNote.A0;
        public float keyDownSpeed = 20f;
        public float keyUpSpeed = 15f;
        public float keyDownDegree = 2.2f;
        [Header("Pedal")]
        public Transform[] pedals;
        public float pedalDownSpeed = 30f;
        public float pedalUpSpeed = 20f;
        public float pedalDownDegree = 7f;

        private List<PianoCommand> _commandList;

        public int CommandCount { get => _commandList.Count; }

        void Awake()
        {
            _commandList = new List<PianoCommand>();
        }

        void Update()
        {
            // execute existing commands
            for (int i = _commandList.Count - 1; i >= 0; i--)
            {
                var cmd = _commandList[i];

                if (cmd.Action == KeyboardAction.KeyDown)
                {
                    var note = cmd.Arg - (int)startKey;
                    if (note < 0 || note >= keys.Length)
                    {
                        _commandList.RemoveAt(i);
                        Debug.LogWarning("The note is out of range! Command aborted.");
                        continue;
                    }
                    var key = keys[note];
                    if (key == null)
                    {
                        _commandList.RemoveAt(i);
                        Debug.LogWarning("The key transform has not be assigned! Command aborted.");
                        continue;
                    }

                    var angle = Mathf.MoveTowardsAngle(key.localRotation.eulerAngles.x, -keyDownDegree, keyDownSpeed * Time.deltaTime);
                    if (Mathf.Approximately(angle, -keyDownDegree)) _commandList.RemoveAt(i);
                    key.localRotation = Quaternion.Euler(angle, 0f, 0f);
                }

                else if (cmd.Action == KeyboardAction.KeyUp)
                {
                    var note = cmd.Arg - (int)startKey;
                    if (note < 0 || note >= keys.Length)
                    {
                        _commandList.RemoveAt(i);
                        Debug.LogWarning("The note is out of range! Command aborted.");
                        continue;
                    }
                    var key = keys[note];
                    if (key == null)
                    {
                        _commandList.RemoveAt(i);
                        Debug.LogWarning("The key transform has not be assigned! Command aborted.");
                        continue;
                    }

                    var angle = Mathf.MoveTowardsAngle(key.localRotation.eulerAngles.x, 0f, keyUpSpeed * Time.deltaTime);
                    if (Mathf.Approximately(angle, 0f)) _commandList.RemoveAt(i);
                    key.localRotation = Quaternion.Euler(angle, 0f, 0f);
                }

                else if (cmd.Action == KeyboardAction.PedalDown)
                {
                    var p = cmd.Arg;
                    if (p < 0 || p >= pedals.Length)
                    {
                        _commandList.RemoveAt(i);
                        Debug.LogWarning("No such pedal! Command aborted.");
                        continue;
                    }
                    var pedal = pedals[p];
                    if (pedal == null)
                    {
                        _commandList.RemoveAt(i);
                        Debug.LogWarning("The pedal transform has not be assigned! Command aborted.");
                        continue;
                    }

                    var angle = Mathf.MoveTowardsAngle(pedal.localRotation.eulerAngles.x, -pedalDownDegree, pedalDownSpeed * Time.deltaTime);
                    if (Mathf.Approximately(angle, -pedalDownDegree)) _commandList.RemoveAt(i);
                    pedal.localRotation = Quaternion.Euler(angle, 0f, 0f);
                }

                else if (cmd.Action == KeyboardAction.PedalUp)
                {
                    var p = cmd.Arg;
                    if (p < 0 || p >= pedals.Length)
                    {
                        _commandList.RemoveAt(i);
                        Debug.LogWarning("No such pedal! Command aborted.");
                        continue;
                    }
                    var pedal = pedals[p];
                    if (pedal == null)
                    {
                        _commandList.RemoveAt(i);
                        Debug.LogWarning("The pedal transform has not be assigned! Command aborted.");
                        continue;
                    }

                    var angle = Mathf.MoveTowardsAngle(pedal.localRotation.eulerAngles.x, 0f, pedalUpSpeed * Time.deltaTime);
                    if (Mathf.Approximately(angle, 0f)) _commandList.RemoveAt(i);
                    pedal.localRotation = Quaternion.Euler(angle, 0f, 0f);
                }
            }
        }

        public void KeyDown(KeyNote note)
        {
            _commandList.Add(new PianoCommand(KeyboardAction.KeyDown, (int)note));
        }

        public void KeyUp(KeyNote note)
        {
            _commandList.Add(new PianoCommand(KeyboardAction.KeyUp, (int)note));
        }

        public void PedalDown(PianoPedal pedal)
        {
            _commandList.Add(new PianoCommand(KeyboardAction.PedalDown, (int)pedal));
        }

        public void PedalUp(PianoPedal pedal)
        {
            _commandList.Add(new PianoCommand(KeyboardAction.PedalUp, (int)pedal));
        }
    }
}