using UnityEngine;

public class savePoint : MonoBehaviour
{
    public int savePointID;

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 5);
    }
}
