using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{
    [Min(1)] public int maxObs = 2;
    [Min(0)] public float radius = 2f;
    [Min(1)] public float angle = 90f;
    public float personalArea = 0.5f;
    public LayerMask obsMask;

    private Collider[] _colls;
    private Vector3 lastAvoidanceDir;

    private void Awake()
    {
        _colls = new Collider[maxObs];
    }

    public Vector3 GetDir(Vector3 currDir)
    {
        int count = Physics.OverlapSphereNonAlloc(Self, radius, _colls, obsMask);
        Collider nearColl = null;
        float nearCollDistance = 0f;
        Vector3 nearClosestPoint = Vector3.zero;

        for (int i = 0; i < count; i++)
        {
            Collider currColl = _colls[i];
            Vector3 closestPoint = currColl.ClosestPoint(Self);
            Vector3 dir = closestPoint - Self;
            float distance = dir.magnitude;

            float currAngle = Vector3.Angle(dir, currDir);
            if (currAngle > angle / 2f) continue;

            if (nearColl == null || distance < nearCollDistance)
            {
                nearColl = currColl;
                nearCollDistance = distance;
                nearClosestPoint = closestPoint;
            }
        }

        if (nearColl == null)
        {
            lastAvoidanceDir = Vector3.zero;
            return currDir;
        }

        Vector3 dirToColl = (nearClosestPoint - Self).normalized;
        Vector3 avoidanceDir = Vector3.Cross(Vector3.up, dirToColl).normalized;

        // Prefer misma dirección que el frame anterior
        if (Vector3.Dot(lastAvoidanceDir, avoidanceDir) < 0)
        {
            avoidanceDir = -avoidanceDir;
        }

        // Mezclar la dirección suavemente
        float weight = Mathf.Clamp01((radius - (nearCollDistance - personalArea)) / radius);
        Vector3 finalDir = Vector3.Lerp(currDir, avoidanceDir, weight).normalized;

#if UNITY_EDITOR
        Debug.DrawRay(Self, avoidanceDir * 2, Color.red);
        Debug.DrawRay(Self, currDir * 2, Color.green);
        Debug.DrawRay(Self, finalDir * 2, Color.cyan);
#endif

        lastAvoidanceDir = avoidanceDir;
        return finalDir;
    }

    public Vector3 Self => transform.position;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, personalArea);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * radius);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * radius);
    }
}
