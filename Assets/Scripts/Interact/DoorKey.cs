using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{

    public bool Picked = false;
    public MeshRenderer keyRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //keyRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !Picked)
        {
            AudioManager.Get.Play("take_key");
            Picked = true;
            keyRenderer.enabled = false;
        }
    }
}
