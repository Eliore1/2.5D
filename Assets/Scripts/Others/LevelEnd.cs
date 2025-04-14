using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{


    GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.Get;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            manager.NextLevel();
        }
    }
}
