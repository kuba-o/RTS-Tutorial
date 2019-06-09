using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float panSpeed;
    public float rotateSpeed;
    public float rotateAmount;

    private Rect selectBox;

    private float panDetect = 15f;
    private Quaternion rotation;
    private Vector3 position;
    public float minHeight;
    public float maxHeight;
    public float maxRight;
    public float maxLeft;
    public float maxForward;
    public float maxBackward;
    public ObjectInfo selectedInfo;

    private Vector2 boxStart;
    private Vector2 boxEnd;
    public Texture boxText;


    public GameObject primary;
    public GameObject[] units;

    private float altitude;
    private float oldAltitude;

    void Start()
    {
        primary = null;
        rotation = Camera.main.transform.rotation;
        position = Camera.main.transform.position;
        oldAltitude = Terrain.activeTerrain.SampleHeight(Camera.main.transform.position);
    }

    void Update()
    {

        

        if (primary != null)
        {
            selectedInfo = primary.GetComponent<ObjectInfo>();
        }

        MoveCamera();
        RotateCamera();
        altitude = Terrain.activeTerrain.SampleHeight(Camera.main.transform.position);

        if (altitude > oldAltitude)
        {
            Camera.main.transform.position += new Vector3(0, altitude - oldAltitude, 0);
            oldAltitude = altitude;

        }
        else if (altitude < oldAltitude)
        {
            Camera.main.transform.position -= new Vector3(0, oldAltitude - altitude, 0);
            oldAltitude = altitude;

        }

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
        }

        if (Input.GetMouseButtonDown(0))
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
                    if (primary == null)
                    {
                        unit.GetComponent<ObjectInfo>().isPrimary = true;
                        unit.GetComponent<ObjectInfo>().iconCam.SetActive(true);
                        primary = unit;

                    }
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
                if (selectedInfo != null)
                {
                    selectedInfo.isPrimary = false;
                    selectedInfo.isSelected = false;
                    selectedInfo.iconCam.SetActive(false);
                    Debug.Log("Deselected " + primary.GetComponent<ObjectInfo>().objectName);

                }

                primary = null;
                selectedInfo = null;
                
            } else if (hit.collider.tag == "Selectable")
            {
                primary = hit.collider.gameObject;
                selectedInfo = primary.GetComponent<ObjectInfo>();
                selectedInfo.isSelected = true;
                selectedInfo.iconCam.SetActive(true);
                selectedInfo.isPrimary = true;
                Debug.Log("selected " + primary.GetComponent<ObjectInfo>().objectName);
            }

        }
    }

    void MoveCamera()
    {
        float moveX = 0;
        float moveY = 0;
        float moveZ = 0;

        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        if (xPos > 0 && xPos < panDetect)
        {
            if (Camera.main.transform.position.x >= -maxLeft)
            {
                moveX = -panSpeed;
            } else
            {
                moveX = 0;
            }
        }
        else if (xPos <= Screen.width && xPos >= Screen.width - panDetect)
        {
            if (Camera.main.transform.position.x <= maxRight)
            {
                moveX = panSpeed;
            } else
            {
                moveX = 0;
            }
        }

        if (yPos <= Screen.height && yPos >= Screen.height - panDetect)
        {
            if (Camera.main.transform.position.z <= maxForward)
            {
                moveZ = panSpeed;
            } else
            {
                moveZ = 0;
            }
        } else if (yPos >= 0 && yPos <= panDetect)
        {
            if (Camera.main.transform.position.z >= -maxBackward)
            {
                moveZ = -panSpeed;
            } else
            {
                moveZ = 0;
            }
        }



        float y2 = Camera.main.transform.position.y;
        Vector3 newPos = new Vector3(moveX, moveY, moveZ);
        Vector3 initPos = Camera.main.transform.position;
        Camera.main.transform.Translate(newPos * Time.deltaTime);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, y2, Camera.main.transform.position.z);
        Vector3 pos = Camera.main.transform.position;
        if (pos.x > maxRight || pos.x <- maxLeft || pos.z > maxForward || pos.z <- maxBackward)
        {
            Camera.main.transform.position = initPos;
        }

        float rise = 15;
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.transform.position.y >= maxHeight)
            {
                rise = 0;
            }


            Camera.main.transform.position += new Vector3(0, rise, 0) * Time.deltaTime;
        } else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.transform.position.y <= minHeight)
            {
                rise = 0;
            }
            Camera.main.transform.position -= new Vector3(0, rise, 0) * Time.deltaTime;
        }
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
            GUI.DrawTexture(selectBox, boxText);
        }
    }
}
