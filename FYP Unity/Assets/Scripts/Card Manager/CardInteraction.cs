using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour
{
    private Vector3 originalSize;
    private Vector3 modifiedSize;
    private Vector3 OriginalPosition;
    private Vector3 HoverOverPosition;
    private Vector3 HoveredOriginalPosition;
    public float SizeIncrease;
    Vector3 mousePositionOffset;
    [SerializeField] float ShowOffset;
    bool LookingAtCardInfo;
    bool HasClicked;
    bool IsOnDropZone;

    private Vector3 GetMousePosition()
    {
        Vector3 MousePos = Input.mousePosition;
        MousePos.z = Camera.main.transform.position.y;
        return Camera.main.ScreenToWorldPoint(MousePos);
    }

    private void Start()
    {
        OriginalPosition = gameObject.transform.position;
        HoveredOriginalPosition = gameObject.transform.GetChild(0).position;
        HoverOverPosition = HoveredOriginalPosition + new Vector3(0, 0, ShowOffset);
        originalSize = gameObject.transform.GetChild(0).localScale;
        modifiedSize = originalSize + new Vector3(SizeIncrease, 0, SizeIncrease);
        LookingAtCardInfo = true;
        HasClicked = false;
        IsOnDropZone = false;
    }


    private void OnMouseOver()
    {
        // if player is just hovering over it, make the card bigger
        if (LookingAtCardInfo)
        {
            gameObject.transform.GetChild(0).position = HoverOverPosition;
            gameObject.transform.GetChild(0).localScale = modifiedSize;
        }
        // if the player is dragging the card, make the card its original size
        else
        {
            gameObject.transform.GetChild(0).localScale = originalSize;
        }
    }

    private void OnMouseExit()
    {
        if (!HasClicked)
        {
            gameObject.transform.GetChild(0).position = HoveredOriginalPosition;
            gameObject.transform.position = OriginalPosition;
            gameObject.transform.GetChild(0).localScale = originalSize;
            LookingAtCardInfo = true;
        }
    }

    private void OnMouseUpAsButton()
    {
        HasClicked = false;
    }

    private void OnMouseDown()
    {
        if (!HasClicked)
        {
            HasClicked = true;
            gameObject.transform.GetChild(0).position = HoveredOriginalPosition;
            //gameObject.GetComponentInChildren<Transform>().position = new Vector3(0, 0, 0);
            mousePositionOffset = gameObject.transform.position - GetMousePosition();
            LookingAtCardInfo = false;
        }
    }

    private void OnMouseDrag()
    {
        gameObject.transform.position = GetMousePosition() + mousePositionOffset;
    }

    private void OnMouseUp()
    {
        CheckOnDropZone();
    }

    private void OnTriggerEnter(Collider other)
    {
        IsOnDropZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsOnDropZone = false;
    }

    private void CheckOnDropZone()
    {
        if (IsOnDropZone)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.transform.position = OriginalPosition;
        }
    }
}
