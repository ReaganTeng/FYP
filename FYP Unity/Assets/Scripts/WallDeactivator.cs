using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDeactivator : MonoBehaviour
{
    public Transform player;
    public Camera cam;
    public LayerMask wallsLayer;

    private List<GameObject> disabledWalls = new List<GameObject>();

    void Update()
    {
        // Re-enable any previously disabled walls that are now visible
        for (int i = disabledWalls.Count - 1; i >= 0; i--)
        {
            GameObject wall = disabledWalls[i];
            if (!IsWallBlockingView(wall))
            {
                wall.SetActive(true);
                disabledWalls.RemoveAt(i);
            }
        }

        // Disable any walls that are blocking the view
        RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, player.position - cam.transform.position, Mathf.Infinity, wallsLayer);
        foreach (RaycastHit hit in hits)
        {
            GameObject wall = hit.collider.gameObject;
            if (!disabledWalls.Contains(wall))
            {
                wall.SetActive(false);
                disabledWalls.Add(wall);
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
