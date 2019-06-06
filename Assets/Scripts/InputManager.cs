using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float panSpeed;
    public float rotateSpeed;
    public float rotateAmount;

    public GameObject selectedObject;

    private Rect selectBox;

    private float panDetect = 15f;
    private Quaternion rotation;
    private Vector3 position;
    private float minHeight = 10f;
    private float maxHeight = 100f;
    private ObjectInfo selectedInfo;

    private Vector2 boxStart;
    private Vector2 boxEnd;
    public Texture boxText;

    private GameObject[] units;

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

        if (Input.GetMouseButton(0) && boxStart == Vector2.zero)
        {
            boxStart = Input.mousePosition;
        } else if (Input.GetMouseButton(0) && boxStart != Vector2.zero)
        {
            boxEnd = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            units = GameObject.FindGameObjectsWithTag("Selectable");
            MultiSelect();  
            //boxStart = Vector2.zero;
            //boxEnd = Vector2.zero;
        }

        if (Input.GetMouseButton(0))
        {
            LeftClick();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.transform.rotation = rotation;
            Camera.main.transform.position = position;
        }

        selectBox = new Rect(boxStart.x, Screen.height - boxStart.y, boxEnd.x - boxStart.x, -boxEnd.y + boxStart.y);
    }

    public void MultiSelect()
    {
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<ObjectInfo>().isUnit)
            {
                Vector2 unitPos = Camera.main.WorldToScreenPoint(unit.transform.position);
                if (selectBox.Contains(unitPos, true))
                {
                    unit.GetComponent<ObjectInfo>().isSelected = true;
                }
            }
        }

        boxStart = Vector2.zero;
        boxEnd = Vector2.zero;
    }

    public void LeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.tag == "Ground")
            {
                if (selectedInfo != null && selectedInfo.isSelected)
                {
                    selectedInfo.isSelected = false;

                }
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
        //float moveX = Camera.main.transform.position.x;
        //float moveY = Camera.main.transform.position.y;
        //float moveZ = Camera.main.transform.position.z;


        float moveX = 0;
        float moveY = 0;
        float moveZ = 0;

        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        if (xPos > 0 && xPos < panDetect)
        {
            //moveX -= panSpeed;
            moveX = -panSpeed;
        }
        else if (xPos <= Screen.width && xPos >= Screen.width - panDetect)
        {
            //moveX += panSpeed;
            moveX = panSpeed;
        }

        if (yPos <= Screen.height && yPos >= Screen.height - panDetect)
        {
            //moveZ += panSpeed;
            //moveX += Camera.main.transform.position.x;
            moveZ = panSpeed;
        } else if (yPos >= 0 && yPos <= panDetect)
        {
            Debug.Log("ypos: " + yPos);
            moveZ = -panSpeed;
            //moveX -= Camera.main.transform.position.x;
            //moveZ -= panSpeed;
        }

        Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        moveY -= Input.GetAxis("Mouse ScrollWheel") * (panSpeed * 20);
        //moveY = Mathf.Clamp(moveY, minHeight, maxHeight);
        float y2 = Camera.main.transform.position.y;

        Vector3 newPos = new Vector3(moveX, moveY, moveZ);
        //Vector3 newPos = new Vector3(moveX, moveY, moveZ);
        Camera.main.transform.Translate(newPos * Time.deltaTime);
        //Camera.main.gameObject.transform.position.z = moveZ;
        float x2 = Camera.main.transform.position.x;

        float z2 = Camera.main.transform.position.z;
        //Camera.main.transform.position += new Vector3(0 , y2, z2 + moveZ * Time.deltaTime);
        
        //= moveZ;
        //Camera.main.transform.position = newPos;
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

    void OnGUI()
    {
        if (boxStart != Vector2.zero && boxEnd != Vector2.zero)
        {
            Debug.Log("ASD");
            Debug.Log(boxStart);
            Debug.Log(boxEnd);
            GUI.DrawTexture(selectBox, boxText);
            //GUI.DrawTexture(new Rect(boxStart.x, Screen.height - boxStart.y, boxEnd.x - boxStart.x, boxStart.y - boxStart.y), boxText);
        }
    }

    
}
