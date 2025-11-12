using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
public class ZombieSpawner : MonoBehaviour
{
    [Header("Asignaciones")]
    public GameObject zombiePrefab;
    public Transform player;

    [Header("Configuración")]
    public int maxAlive = 8;
    public float spawnInterval = 3f;
    public bool spawnOnlyIfPlayerInside = true;
    public float sampleRadius = 4f;

    BoxCollider zone;
    List<GameObject> alive = new();
    bool playerInside;

    void Awake() {
        zone = GetComponent<BoxCollider>();
        zone.isTrigger = true;
    }

    void Start() {
        if (!player) {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop() {
        while (true) {
            yield return new WaitForSeconds(spawnInterval);

            if (alive.Count >= maxAlive) continue;
            if (spawnOnlyIfPlayerInside && !playerInside) continue;

            Vector3 pos = GetRandomPointInZone();

            if (NavMesh.SamplePosition(pos, out var hit, sampleRadius, NavMesh.AllAreas))
                pos = hit.position;

            var z = Instantiate(zombiePrefab, pos, Quaternion.identity);
            alive.Add(z);

            var health = z.GetComponent<ZombieHealth>();
            if (health) health.onDied += () => { alive.Remove(z); };

            var ai = z.GetComponent<ZombieAI>();
            if (ai && player) ai.target = player;
        }
    }

    Vector3 GetRandomPointInZone() {
        var center = zone.center;
        var size = zone.size;
        var local = new Vector3(
            center.x + (Random.value - 0.5f) * size.x,
            center.y + (Random.value - 0.5f) * size.y,
            center.z + (Random.value - 0.5f) * size.z
        );
        return transform.TransformPoint(local);
    }

    void OnTriggerEnter(Collider other) {
        if ((player && other.transform == player) || other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit(Collider other) {
        if ((player && other.transform == player) || other.CompareTag("Player"))
            playerInside = false;
    }
}
