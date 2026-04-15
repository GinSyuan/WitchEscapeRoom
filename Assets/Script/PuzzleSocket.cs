using UnityEngine;

public class PuzzleSocket : MonoBehaviour
{
    [Header("Socket Settings")]
    public GameObject correctItem;
    public DoorPuzzleManager manager;

    [Header("Perfect Snap Adjustments")]
    public Vector3 positionOffset = new Vector3(0, 0.1f, 0);
    public Vector3 uprightRotation;

    [HideInInspector]
    public bool isCorrectItemPlaced = false;

    void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null && rb.useGravity == true && !rb.isKinematic)
        {
            other.transform.position = transform.position + positionOffset;
            other.transform.rotation = Quaternion.Euler(uprightRotation);

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            if (other.gameObject == correctItem && !isCorrectItemPlaced)
            {
                isCorrectItemPlaced = true;
                if (manager != null) manager.CheckIfPuzzleSolved();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == correctItem)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // THE FIX: Ignore the Unity glitch!
            // Only un-solve the socket if the Wand actually grabbed it back (isKinematic is false)

            if (rb != null && rb.isKinematic == false)
            {
                isCorrectItemPlaced = false;

                // Optional: Tell the manager to print the updated lower score!
                if (manager != null) manager.CheckIfPuzzleSolved();
            }
        }
    }
}