using UnityEngine;

public class MagicWandGrabber : MonoBehaviour
{
    [Header("VR Input Settings")]
    public OVRInput.Controller controller = OVRInput.Controller.RTouch;
    public OVRInput.Button grabButton = OVRInput.Button.PrimaryIndexTrigger;

    [Header("Wand Settings")]
    public Transform wandTip;
    public float maxGrabDistance = 10f;
    public float levitationDistance = 2f;
    public float moveSpeed = 10f;
    public LayerMask grabbableLayer;

    private Rigidbody grabbedObject;
    private float originalDrag;

    void Update()
    {
        // Changed to UnityEngine.Debug (已更改為 UnityEngine.Debug)
        if (wandTip != null)
        {
            UnityEngine.Debug.DrawRay(wandTip.position, wandTip.forward * maxGrabDistance, Color.red);
        }

        if (OVRInput.GetDown(grabButton, controller))
        {
            TryGrab();
        }
        else if (OVRInput.GetUp(grabButton, controller) && grabbedObject != null)
        {
            Release();
        }
    }

    void FixedUpdate()
    {
        if (grabbedObject != null)
        {
            LevitateObject();
        }
    }

    void TryGrab()
    {
        RaycastHit hit;

        if (Physics.Raycast(wandTip.position, wandTip.forward, out hit, maxGrabDistance, grabbableLayer))
        {
            // Changed to UnityEngine.Debug (已更改為 UnityEngine.Debug)
            UnityEngine.Debug.Log("Magic hit: " + hit.collider.gameObject.name);

            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                grabbedObject = rb;
                grabbedObject.useGravity = false;

                originalDrag = grabbedObject.linearDamping;
                grabbedObject.linearDamping = 5f;
            }
            else
            {
                // Changed to UnityEngine.Debug (已更改為 UnityEngine.Debug)
                UnityEngine.Debug.LogWarning("The object hit does not have a Rigidbody! (打到的物件沒有剛體！)");
            }
        }
    }

    void LevitateObject()
    {
        Vector3 targetPosition = wandTip.position + wandTip.forward * levitationDistance;
        Vector3 direction = targetPosition - grabbedObject.position;
        grabbedObject.linearVelocity = direction * moveSpeed;
    }

    void Release()
    {
        grabbedObject.useGravity = true;
        grabbedObject.linearDamping = originalDrag;
        grabbedObject = null;
    }
}