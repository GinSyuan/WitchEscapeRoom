using UnityEngine;

public class MagicIngredient : MonoBehaviour
{
    [Header("Ingredient Settings")]
    public string ingredientName; // Give each item a unique name like "RedMushroom", "Bone", etc.

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    void Start()
    {
        // Save the starting position on the table (記住桌上的初始位置)
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    public void RespawnOnTable()
    {
        // Teleport back to the table and stop moving (傳送回桌上並停止移動)
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}