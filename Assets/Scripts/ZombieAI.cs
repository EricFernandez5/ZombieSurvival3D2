using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieAI : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Ranges")]
    public float detectionRange = 40f;   // distancia a la que empieza a perseguir
    public float attackRange    = 1.7f;  // distancia a la que ataca
    public float repathInterval = 0.15f; // cada cuánto re-calcula destino

    [Header("Attack (animación solo)")]
    public float attackWindup   = 0.35f; // tiempo antes del “impacto”
    public float attackCooldown = 0.9f;  // tiempo entre ataques

    NavMeshAgent agent;
    Animator anim;
    float timer;
    bool attacking;

    int hashSpeed;
    int hashIsAttacking;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        anim  = GetComponentInChildren<Animator>();
        hashSpeed       = Animator.StringToHash("Speed");
        hashIsAttacking = Animator.StringToHash("IsAttacking");
    }

    void Start() {
        if (!target) {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
        }

        // Que el agente pare un poco antes del rango de ataque
        if (agent) agent.stoppingDistance = Mathf.Max(attackRange - 0.1f, 0.5f);
    }

    void Update() {
        if (!target) return;

        timer += Time.deltaTime;
        float dist = Vector3.Distance(transform.position, target.position);

        // Persecución (si no está atacando)
        if (!attacking && timer >= repathInterval) {
            timer = 0f;
            if (dist <= detectionRange) {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
        }

        // Entrar en ataque cuando esté en rango
        if (!attacking && dist <= attackRange + 0.05f) {
            StartCoroutine(AttackRoutine());
        }

        // Actualiza locomoción (Blend Tree)
        if (anim) anim.SetFloat(hashSpeed, agent.velocity.magnitude);
    }

    IEnumerator AttackRoutine() {
        attacking = true;
        agent.isStopped = true;

        // Mirar al jugador
        if (target) {
            Vector3 look = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(look);
        }

        // Dispara la anim de ataque (Animator)
        if (anim) anim.SetBool(hashIsAttacking, true);

        // “Viento” del golpe y momento de impacto (sin daño real)
        yield return new WaitForSeconds(attackWindup);
        TryAttackHit(); // solo log, sin salud del jugador

        // Enfriamiento antes de siguiente ataque
        yield return new WaitForSeconds(attackCooldown);

        if (anim) anim.SetBool(hashIsAttacking, false);
        attacking = false;
        agent.isStopped = false;
    }

    // Aquí NO tocamos salud; solo confirmamos que estaba en rango y registramos el golpe.
    void TryAttackHit() {
        if (!target) return;
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= attackRange + 0.2f) {
            Debug.Log("[Zombie] ¡Ataque realizado! (sin aplicar daño)");
        }
    }

    // Si prefieres precisión exacta, añade un Animation Event en el clip Z_Attack
    // y llama a este método en el frame del impacto.
    public void AnimEvent_Hit() { TryAttackHit(); }
}
