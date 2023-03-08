using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCollider : MonoBehaviour
{
    [SerializeField] Collider boundaryCollider;
    [SerializeField] Rigidbody rb;
    
    void FixedUpdate()
    {
        Vector3 position = transform.position; // Get the player's current position
        if (!boundaryCollider.bounds.Contains(position))
        { // Check if the player is outside the boundary
            Vector3 clampedPosition = boundaryCollider.ClosestPoint(position); // Get the closest point on the boundary
            rb.MovePosition(clampedPosition); // Move the player to the closest point on the boundary
        }
    }
}
