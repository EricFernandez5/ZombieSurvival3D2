using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("Salto y gravedad")]
    public float gravity = -9.81f;
    public float jumpHeight = 1.6f;
    public float groundedStick = -2f;

    [Header("Ground Check (opcional)")]
    public Transform groundCheck;
    public float groundRadius = 0.25f;
    public LayerMask groundMask; // arrastra aquí la capa del suelo si la usas

    private CharacterController cc;
    private Vector3 velocity;
    private bool isGrounded;
    private Camera cam;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        cam = Camera.main;

        // Crear GroundCheck si no existe
        if (!groundCheck)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.SetParent(transform);
            gc.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            groundCheck = gc.transform;
        }
    }

    void Update()
    {
        // Comprobar si está tocando el suelo
        if (groundMask.value != 0)
            isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask, QueryTriggerInteraction.Ignore);
        else
            isGrounded = cc.isGrounded;

        if (isGrounded && velocity.y < 0f)
            velocity.y = groundedStick;

        // Entrada de movimiento
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(h, 0f, v);
        input = Vector3.ClampMagnitude(input, 1f);

        // Movimiento relativo a la cámara
        Vector3 camForward = cam ? cam.transform.forward : Vector3.forward;
        Vector3 camRight = cam ? cam.transform.right : Vector3.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camRight * input.x + camForward * input.z).normalized;
        Vector3 move = moveDir * moveSpeed;

        // Salto
        if (isGrounded && Input.GetButtonDown("Jump"))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;

        // Aplicar movimiento al CharacterController
        cc.Move((move + velocity) * Time.deltaTime);

        // Girar suavemente hacia donde se mueve
        Vector3 planar = new Vector3(moveDir.x, 0f, moveDir.z);
        if (planar.sqrMagnitude > 0.001f)
        {
            Quaternion target = Quaternion.LookRotation(planar);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
