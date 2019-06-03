﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float panSpeed;
    public float rotateSpeed;
    public float rotateAmount;

    public GameObject selectedObject;

    private float panDetect = 15f;
    private Quaternion rotation;
    private Vector3 position;
    private float minHeight = 10f;
    private float maxHeight = 100f;
    private ObjectInfo selectedInfo;

    // Start is called before the first frame update
    void Start()
    {
        rotation = Camera.main.transform.rotation;
        position = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        RotateCamera();

        if (Input.GetMouseButton(0))
        {
            LeftClick();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.transform.rotation = rotation;
            Camera.main.transform.position = position;
        }
    }

    public void LeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("sup");
            if (hit.collider.tag == "Ground")
            {
                selectedInfo.isSelected = false;
                selectedObject = null;
                Debug.Log("Deselected ");
            } else if (hit.collider.tag == "Selectable")
            {
                selectedObject = hit.collider.gameObject;
                selectedInfo = selectedObject.GetComponent<ObjectInfo>();
                selectedInfo.isSelected = true;
                Debug.Log("selected " + selectedInfo.objectName);
            }

        }
    }

    void MoveCamera()
    {
        float moveX = Camera.main.transform.position.x;
        float moveY = Camera.main.transform.position.y;
        float moveZ = Camera.main.transform.position.z;

        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        if (xPos > 0 && xPos < panDetect)
        {
            moveX -= panSpeed;
        }
        else if (xPos <= Screen.width && xPos >= Screen.width - panDetect)
        {
            moveX += panSpeed;
        }

        if (yPos <= Screen.height && yPos >= Screen.height - panDetect)
        {
            moveZ += panSpeed;
        } else if (yPos >= 0 && yPos <= panDetect)
        {
            moveZ -= panSpeed;
        }

        moveY -= Input.GetAxis("Mouse ScrollWheel") * (panSpeed * 20);
        moveY = Mathf.Clamp(moveY, minHeight, maxHeight);

        Vector3 newPos = new Vector3(moveX, moveY, moveZ);
        Camera.main.transform.position = newPos;
    }

    void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        if (Input.GetMouseButton(2))
        {
            destination.x -= Input.GetAxis("Mouse Y") * rotateAmount;
            destination.y += Input.GetAxis("Mouse X") * rotateAmount;
        }

        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }
    }
}
