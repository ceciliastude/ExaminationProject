using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 5, -10); // position offset relative to player
    private Vector3 rotationOffsetEuler = new Vector3(10, 0, 0); // rotation offset in degrees
    public float smoothSpeed = 0.125f;

    private Quaternion rotationOffset;

    void Start()
    {
        // Convert Euler angles to Quaternion for offset
        rotationOffset = Quaternion.Euler(rotationOffsetEuler);
    }

    void LateUpdate()
    {
        // Desired position behind the player
        Vector3 desiredPosition = player.position + player.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply rotation offset relative to the player
        transform.rotation = player.rotation * rotationOffset;
    }
}
