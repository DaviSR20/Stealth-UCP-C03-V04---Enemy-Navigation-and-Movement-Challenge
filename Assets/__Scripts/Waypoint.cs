using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float waitTime = 2f; // tiempo de espera en segundos

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up, gameObject.name);
        #endif
    }
}