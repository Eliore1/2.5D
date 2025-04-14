using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnLoad : MonoBehaviour
{
	public string name;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Get.PlayMusic(name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
