using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIFlameManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image flame;
    [SerializeField] float maxSize;
    void Start()
    {
        if (flame == null)
            flame = GetComponent<UnityEngine.UI.Image>();
        if (!flame.enabled)
            flame.enabled = true;
        if (flame.transform.localScale != new Vector3(maxSize, maxSize, 1f))
            flame.transform.localScale = new Vector3(maxSize, maxSize, 1f);
    }

    void Update()
    {
        CheckPlayerLife();
    }

    public void CheckPlayerLife()
    {
        flame.transform.localScale = new Vector3((maxSize / GameManager.Get.playerManager.life) * 3f, (maxSize / GameManager.Get.playerManager.life) * 3f, 1f);       
    }
}
