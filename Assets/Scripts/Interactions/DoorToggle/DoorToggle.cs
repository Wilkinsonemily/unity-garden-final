using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToggle : MonoBehaviour
{
   [Header("Assign Door States")]
    public GameObject doorClosed;
    public GameObject doorOpen;

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.JoystickButton0; 

    private bool isOpen = false;

    void Start()
    {
        SetDoorState(false); 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckDoorClick(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

    }

    void CheckDoorClick(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null)
        {
            if (!isOpen && hit.collider.gameObject == doorClosed)
            {
                SetDoorState(true);
            }
            else if (isOpen && hit.collider.gameObject == doorOpen)
            {
                SetDoorState(false);
            }
        }
    }

    void SetDoorState(bool open)
    {
        isOpen = open;
        doorClosed.SetActive(!open);
        doorOpen.SetActive(open);
    }
}
