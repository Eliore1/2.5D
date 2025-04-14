using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDetect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.parent.gameObject.GetComponent<Door>().PlayerOn = true;
            //other.gameObject.transform.parent.parent = transform.parent;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.parent.gameObject.GetComponent<Door>().PlayerOn = false;
            //other.gameObject.transform.parent.parent = null;
            //DontDestroyOnLoad(other.GetComponentInParent<Transform>().gameObject);
        }
    }
}
