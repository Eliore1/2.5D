using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampToZPlayer : MonoBehaviour
{

    public float clampUpdateRate = 0.1f;
    public float Zpos = 0;

    PlayerManager player;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ClampToZCoroutine());
        player = GameManager.Get.playerManager;
    }

    IEnumerator ClampToZCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(clampUpdateRate);
            if (transform.position.z != Zpos && !player.hiding)
                transform.position = new Vector3(transform.position.x, transform.position.y, Zpos);
        }
    }
}
