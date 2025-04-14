using UnityEngine;
using UnityEngine.InputSystem;

public class Stairs : MonoBehaviour
{
    private Interaction interact;
    private PlayerManager playerManager;
    [SerializeField] private Transform point0 = null;
    [SerializeField] private Transform point1 = null;
    private Transform next;
    [HideInInspector] public bool use = false;
    private void Start()
    {
       interact = gameObject.GetComponent<Interaction>();
       playerManager = FindAnyObjectByType<PlayerManager>();
       interact.Press.AddListener(TeleportTo);
        if (point0 == null)
            Debug.LogError("point not set");
        if (point1 == null)
            Debug.LogError("point not set");
        next = point1;
    }

    public void Update()
    {
    }
    public void TeleportTo()
    {
        CharacterController controller = playerManager.gameObject.GetComponent<CharacterController>();
        controller.enabled = false;

        if (next == point1)
        {
            playerManager.transform.position = new Vector3(point1.position.x, point1.position.y, 0f);
            next = point0;
            controller.enabled = true;
        }
        else if (next == point0)
        {
            playerManager.transform.position = new Vector3(point0.position.x, point0.position.y, 0f);
            next = point1;
            controller.enabled = true;
        }
        use = false;
    }

    public void UseStairs(InputAction.CallbackContext context)
    {
        if (context.started && playerManager.checkInput) 
        {
            if (!use)
                use = true;
        }
        
    }
}
    