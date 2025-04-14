using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FxManager : MonoBehaviour
{

    Canvas uieffectcanvas;
    Image pulsate;
    GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        uieffectcanvas = transform.Find("UIFxCanvas").GetComponent<Canvas>();
        pulsate = uieffectcanvas.transform.Find("Pulsate").GetComponent<Image>();
        manager = GameManager.Get;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator flashAlpha()
    {
        for (float i = 0; i <= 255; i+=1)
        {
            pulsate.color = new Color(1, 0, 0, 1-i/255);
            yield return new WaitForSeconds(.0015f);
        }
    }

    public void InstanceEffect(string sequence,Vector3 root)
    {

        if (sequence == "BasicHit")
        {
            manager.playerManager.playerCamera.CamShake(1.2f);
            pulsate.color = new Color32(255, 0, 0, 255);
            StartCoroutine(flashAlpha());

        }
    }
}
