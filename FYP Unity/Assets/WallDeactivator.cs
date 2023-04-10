using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDeactivator : MonoBehaviour
{
    public Transform player;
    public Camera cam;
    public LayerMask wallsLayer;

    private List<Renderer> disabledRenderers = new List<Renderer>();

    void Update()
    {
        // Re-enable any previously disabled wall renderers that are now visible
        for (int i = disabledRenderers.Count - 1; i >= 0; i--)
        {
            Renderer renderer = disabledRenderers[i];
            if (!IsWallBlockingView(renderer.gameObject))
            {
                renderer.enabled = true;
                disabledRenderers.RemoveAt(i);
            }
        }

        // Disable any wall renderers that are blocking the view
        RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, player.position - cam.transform.position, Mathf.Infinity, wallsLayer);
        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null && !disabledRenderers.Contains(renderer))
            {
                renderer.enabled = false;
                disabledRenderers.Add(renderer);
            }
        }
    }

    bool IsWallBlockingView(GameObject wall)
    {
        // Check if the wall is blocking the view
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, player.position - cam.transform.position, out hit, Mathf.Infinity, wallsLayer))
        {
            if (hit.collider.gameObject == wall)
            {
                return true;
            }
        }
        return false;
    }
}






