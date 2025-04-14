using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAmbientOnLoad : MonoBehaviour
{
    public string ambientTrackName;
    void Start()
    {
        AudioManager.Get.PlayAmbientAudio(ambientTrackName);
    }
}
