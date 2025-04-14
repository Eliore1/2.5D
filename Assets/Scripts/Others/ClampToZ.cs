using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampToZ : MonoBehaviour
{

    public float clampUpdateRate = 0.1f;
    public float Zpos = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ClampToZCoroutine());
    }

    IEnumerator ClampToZCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(clampUpdateRate);
            if (transform.position.z != Zpos)
                transform.position = new Vector3(transform.position.x, transform.position.y, Zpos);
        }
    }
}
