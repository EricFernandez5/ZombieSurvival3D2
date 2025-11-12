using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieAI : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Ranges")]
    public float detectionRange = 40f;   
    public float attackRange    = 1.7f;  
    public float repathInterval = 0.15f; 

    [Header("Attack")]
    public float attackWindup   = 0.35f; 
    public float attackCooldown = 0.9f;  
    public int damagePerHit     = 10;    // 🔥 HACE 10 DE DAÑO AL JUGADOR

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

        if (agent) agent.stoppingDistance = Mathf.Max(attackRange - 0.1f, 0.5f);
    }

    void Update() {
        if (!target) return;

        timer += Time.deltaTime;
        float dist = Vector3.Distance(transform.position, target.position);

        if (!attacking && timer >= repathInterval) {
            timer = 0f;
            if (dist <= detectionRange) {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
        }

        if (!attacking && dist <= attackRange + 0.05f) {
            StartCoroutine(AttackRoutine());
        }

        if (anim) anim.SetFloat(hashSpeed, agent.velocity.magnitude);
    }

    IEnumerator AttackRoutine() {
        attacking = true;
        agent.isStopped = true;

        if (target) {
            Vector3 look = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(look);
        }

        if (anim) anim.SetBool(hashIsAttacking, true);

        yield return new WaitForSeconds(attackWindup);

        // 🔥 AQUÍ SE APLICA EL DAÑO REAL
        TryAttackHit();

        yield return new WaitForSeconds(attackCooldown);

        if (anim) anim.SetBool(hashIsAttacking, false);
        attacking = false;
        agent.isStopped = false;
    }

    // ============================
    //   ⬇️ DAÑO REAL AL JUGADOR ⬇️
    // ============================
    void TryAttackHit() {
        if (!target) return;

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= attackRange + 0.2f) {

            // Buscamos el script PlayerHealth en el jugador
            PlayerHealth ph = target.GetComponent<PlayerHealth>();

            if (ph != null)
            {
                ph.TakeDamage(damagePerHit);    // 💥 LE QUITA 10 DE VIDA
            }
        }
    }

    public void AnimEvent_Hit() { TryAttackHit(); }
}
