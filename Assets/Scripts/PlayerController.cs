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

    CharacterController cc;
    Vector3 velocity;
    bool isGrounded;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (!groundCheck)
        {
            var gc = new GameObject("GroundCheck");
            gc.transform.SetParent(transform);
            gc.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            groundCheck = gc.transform;
        }
    }

    void Update()
    {
        // ¿tocando suelo? (usa mask si la asignas; si no, cc.isGrounded)
        if (groundMask.value != 0)
            isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask, QueryTriggerInteraction.Ignore);
        else
            isGrounded = cc.isGrounded;

        if (isGrounded && velocity.y < 0f) velocity.y = groundedStick;

        // entrada
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(h, 0, v);
        input = Vector3.ClampMagnitude(input, 1f);

        // mover en el espacio del player
        Vector3 move = transform.TransformDirection(input) * moveSpeed;

        // salto
        if (isGrounded && Input.GetButtonDown("Jump"))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // gravedad
        velocity.y += gravity * Time.deltaTime;

        // aplicar movimiento
        cc.Move((move + velocity) * Time.deltaTime);

        // girar hacia donde camina
        Vector3 planar = new Vector3(move.x, 0, move.z);
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
