using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioPathway : MonoBehaviour {
    public void PassAudio (string aud) {
        PlayAudio.PlaySound (aud);
    }
}