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

    [Header("Hover Offsets")]
    public float heightOffset = 0.2f;
    public float forwardOffset = 0.3f;

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


                grabbedObject.isKinematic = false; 
                                              

                grabbedObject.useGravity = false;
                originalDrag = grabbedObject.linearDamping;
                grabbedObject.linearDamping = 5f;

                currentGrabDistance = Vector3.Distance(wandTip.position, grabbedObject.position);
            }
        }
    }

    void LevitateObject()
    {
        float targetDistance = currentGrabDistance + forwardOffset;
        Vector3 targetPosition = wandTip.position + wandTip.forward * targetDistance;
        targetPosition += Vector3.up * heightOffset;

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