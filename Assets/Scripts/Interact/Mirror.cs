using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mirror : MonoBehaviour
{

    Interaction interactScript;
    PlayerManager plr_M;
    GameObject MirrorPart;
    GameObject MirrorPartDir;
    GameObject LightHalo;
    Transform EyePoint;
    MeshRenderer mirrorRenderer;
    BoxCollider LightBox;
    [SerializeField] float[] angles;
    int angleState = 1;
    bool inBlock = false;
    public LayerMask ignore;
    public LayerMask ignore2;
    Vector3 defaultScale;
    public UnityEvent LightOff;
    

    List<GameObject> LightContainder = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        MirrorPart = transform.Find("RotatingPart").gameObject;
        MirrorPartDir = MirrorPart.transform.Find("RotatingPart2").gameObject;
        interactScript = transform.Find("Interact").GetComponent<Interaction>();
        interactScript.Press.AddListener(MirrorRotate);
        LightHalo = MirrorPart.transform.Find("Cylinder").gameObject;
        mirrorRenderer = LightHalo.GetComponent<MeshRenderer>();
        defaultScale = LightHalo.transform.localScale;
        LightBox = LightHalo.GetComponent<BoxCollider>();
        plr_M = GameManager.Get.playerManager;
        plr_M.lampController.LampTurnedOff.AddListener(DeActivate);
        EyePoint = transform.Find("Eye");
    }
    void DeActivate()
    {
        if (LightContainder.Count == 0)
        {
            inBlock = false;
        }

    }

    void MirrorRotate()
    {
        if (angleState > 2)
        {
            angleState = 0;
        }
        MirrorPart.transform.eulerAngles = new Vector3(MirrorPart.transform.eulerAngles.x, MirrorPart.transform.eulerAngles.y, angles[angleState]);
        angleState++;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "light")
        {
            Debug.Log("Light");
            inBlock = true;
            if (!LightContainder.Contains(other.gameObject))
            {
                LightContainder.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "light")
        {
            if (LightContainder.Count == 0)
            {
                inBlock = false;
            }

            if (LightContainder.Contains(other.gameObject))
            {
                LightContainder.Remove(other.gameObject);
            }
        }
    }

    bool CheckAWall()
    {
        EyePoint.LookAt(plr_M.gameObject.transform);
        RaycastHit hit;
        if (Physics.Raycast(EyePoint.position, EyePoint.forward, out hit, Vector3.Distance(EyePoint.position, plr_M.gameObject.transform.position), ~ignore2))
        {
            Debug.Log(gameObject.name + " ," + hit.transform.gameObject.name);
            if (hit.transform.gameObject.tag != "light")
            {
                if (hit.transform.gameObject.tag == "Player" || LightContainder.Count > 1 || !LightContainder.Contains(plr_M.transform.parent.Find("Spot Light (1)").Find("Cone").gameObject))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        return false;
    }

    void ScaleUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(MirrorPart.transform.position, MirrorPartDir.transform.forward, out hit, Mathf.Infinity, ~ignore))
        {
            if (hit.transform.gameObject.tag != "light")
            {

                float dist = Vector3.Distance(hit.point, MirrorPart.transform.position);
                LightHalo.transform.position = MirrorPart.transform.position;
                LightHalo.transform.Translate(Vector3.down * (dist / 2));
                LightHalo.transform.localScale = new Vector3(LightHalo.transform.localScale.x, (dist / 2) * defaultScale.y, LightHalo.transform.localScale.z);
            }
        }
    }

    void LightContainCheck()
    {
        foreach (GameObject a in LightContainder.ToArray())
        {
            if (a.GetComponent<MeshRenderer>().enabled == false || a.transform.parent.gameObject.activeSelf == false)
            {
                LightContainder.Remove(a);
            }
        }

        if (LightContainder.Count == 0)
        {
            inBlock = false;
        }

        if (LightContainder.Count == 1)
        {
            if (LightContainder[0])
            {
                if (LightContainder[0].transform.parent.name == "RotatingPart")
                {
                    LightContainder[0].transform.parent.parent.GetComponent<Mirror>().LightOff.AddListener(DeActivate);
                }
                else
                {
                    LightContainder[0].transform.parent.GetComponent<LampLookController>().LampTurnedOff.AddListener(DeActivate);
                }
            }

        }
        else
        {
            foreach (GameObject a in LightContainder.ToArray())
            {
                if (a.transform.parent.name == "RotatingPart")
                {
                    a.transform.parent.parent.GetComponent<Mirror>().LightOff.AddListener(DeActivate);
                }
                else
                {
                    a.transform.parent.GetComponent<LampLookController>().LampTurnedOff.AddListener(DeActivate);
                }
            }
        }
    }

    void CollisionCheck()
    {
        if (LightContainder.Count == 1 && LightContainder.Contains(plr_M.lampController.gameObject.transform.Find("Cone").gameObject))
        {
            if (inBlock && CheckAWall())
            {
                mirrorRenderer.enabled = true;
                LightBox.enabled = true;
            }
        }
        else
        {
            if (inBlock)
            {
                mirrorRenderer.enabled = true;
                LightBox.enabled = true;
            }
        }

        if (!inBlock || !CheckAWall())
        {
            LightOff.Invoke();
            mirrorRenderer.enabled = false;
            LightBox.enabled = false;
        }
    }


    void Update()
    {
        ScaleUp();
        LightContainCheck();
        CollisionCheck();
    }
}
