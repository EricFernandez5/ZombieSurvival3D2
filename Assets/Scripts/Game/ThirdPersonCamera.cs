using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;              // arrastra el Player aquí
    public Vector3 offset = new Vector3(0, 1.6f, 0);
    public float distance = 3.5f;
    public float mouseSensitivity = 150f;
    public float minPitch = -30f;
    public float maxPitch = 70f;
    public float followDamp = 10f;

    float yaw, pitch;

    void Start()
    {
        if (!target) target = GameObject.FindWithTag("Player")?.transform;
        var a = transform.eulerAngles;
        yaw = a.y; pitch = a.x;
    }

    void LateUpdate()
    {
        if (!target) return;

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = target.position + rot * (offset + new Vector3(0, 0, -distance));

        transform.position = Vector3.Lerp(transform.position, desiredPos, 1 - Mathf.Exp(-followDamp * Time.deltaTime));
        transform.rotation = rot;
    }
}
