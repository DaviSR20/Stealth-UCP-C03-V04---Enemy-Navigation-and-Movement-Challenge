using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public List<Waypoint> waypoints; // visible en inspector

    private NavMeshAgent agent;
    private int currentIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // control manual rotación
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            Waypoint wp = waypoints[currentIndex];

            // 1. Rotar hacia el siguiente waypoint
            yield return StartCoroutine(RotateTowards(wp.GetPosition()));

            // 2. Ir al waypoint
            agent.SetDestination(wp.GetPosition());

            while (agent.pathPending || agent.remainingDistance > 0.1f)
                yield return null;

            // 3. Rotar a la orientación del waypoint
            yield return StartCoroutine(RotateToRotation(wp.GetRotation()));

            // 4. Esperar
            yield return new WaitForSeconds(wp.waitTime);

            // siguiente waypoint
            currentIndex = (currentIndex + 1) % waypoints.Count;
        }
    }

    IEnumerator RotateTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dir);

        while (Quaternion.Angle(transform.rotation, targetRot) > 1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * 3f
            );
            yield return null;
        }
    }

    IEnumerator RotateToRotation(Quaternion targetRot)
    {
        while (Quaternion.Angle(transform.rotation, targetRot) > 1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * 3f
            );
            yield return null;
        }
    }
    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] == null) continue;

            Vector3 current = waypoints[i].transform.position;
            Vector3 next = waypoints[(i + 1) % waypoints.Count].transform.position;

            Gizmos.DrawLine(current, next);
        }
    }
}