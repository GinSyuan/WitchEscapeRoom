using UnityEngine;

public class MagicWandGrabber : MonoBehaviour
{
    [Header("VR Input Settings")]
    public OVRInput.Controller controller = OVRInput.Controller.RTouch;
    public OVRInput.Button grabButton = OVRInput.Button.PrimaryIndexTrigger;

    [Header("Wand Settings")]
    public Transform wandTip;
    public float maxGrabDistance = 10f;
    public float moveSpeed = 10f;
    public LayerMask grabbableLayer;

    // NEW: Offset settings for the hover position
    // 新增：懸浮位置的偏移量設定
    [Header("Hover Offsets")]
    public float heightOffset = 0.2f;   // How much higher to float (懸浮時多高，單位：公尺)
    public float forwardOffset = 0.3f;  // How much further forward to float (懸浮時多往前，單位：公尺)

    private Rigidbody grabbedObject;
    private float originalDrag;
    private float currentGrabDistance;

    void Update()
    {
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
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                grabbedObject = rb;
                grabbedObject.useGravity = false;

                originalDrag = grabbedObject.linearDamping;
                grabbedObject.linearDamping = 5f;

                // Remember the original distance 
                currentGrabDistance = Vector3.Distance(wandTip.position, grabbedObject.position);
            }
        }
    }

    void LevitateObject()
    {
        // 1. Calculate the base distance + the extra forward offset
        float targetDistance = currentGrabDistance + forwardOffset;

        // 2. Find the position straight in front of the wand
        Vector3 targetPosition = wandTip.position + wandTip.forward * targetDistance;

        // 3. Add the height offset (Vector3.up ensures it always goes straight up towards the ceiling)
        targetPosition += Vector3.up * heightOffset;

        // 4. Move the object
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