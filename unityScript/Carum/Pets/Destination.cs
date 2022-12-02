using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Destination : MonoBehaviour
{
    GameObject pet;

    public GameObject roomBase;

    private MeshCollider area;

    //private BoxCollider boxArea;
    private float timer;

    // Start is called before the first frame update
    ActionController actionController;

    private bool isPetChange = false;
    private bool isRoomChange = false;

    void Start()
    {
        // roomBase = GameObject.FindWithTag("RoomBase");
        // area = roomBase.GetComponent<MeshCollider>();
        //boxArea=roomBase.GetComponent<BoxCollider>();
        //pet=GameObject.FindWithTag("Player");
        //actionController = pet.GetComponent<ActionController>(); 
        //물체 충돌 검사
        StartCoroutine(CheckRay());
    }

    // Update is called once per frame
    void Update()
    {
        if (!(pet & roomBase)) return;
        if (pet == null || !pet.activeSelf)
        {
            isPetChange = true;
        }

        if (isPetChange)
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                pet = GameObject.FindWithTag("Player");
                actionController = pet.GetComponent<ActionController>();
            }

            isPetChange = false;
        }

        if (roomBase == null || !roomBase.activeSelf)
        {
            isRoomChange = true;
        }

        if (isRoomChange)
        {
            if (GameObject.FindWithTag("RoomBase") != null)
            {
                roomBase = GameObject.FindWithTag("RoomBase");
                area = roomBase.GetComponent<MeshCollider>();
                NavMeshBake.Bake();
            }
        }

        timer += Time.deltaTime;
        float distance = Vector3.Distance(pet.transform.position, transform.position);
        if (actionController.isMoving && (distance < 1f || timer > 5f))
        {
            timer = 0f;
            changePosition();
        }
    }

    private void LateUpdate()
    {
        if (!roomBase)
        {
            GameObject baseObject = GameObject.FindWithTag("RoomBase");
            if (baseObject)
                SetRoomBase(baseObject);
        }
    }

    IEnumerator CheckRay()
    {
        if (pet != null)
        {
            Ray ray = new Ray(pet.transform.position, pet.transform.forward);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, 5))
            {
                float hitDistance = hitData.distance;
                if (hitDistance < 0.5f)
                {
                    //Debug.Log(hitDistance);
                    changePosition();
                }
            }
        }

        //Debug.Log("Call");
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(CheckRay());
    }


    //특점 지점으로 destinaition 변경
    public void changePosition(Vector3 point)
    {
        Vector3 temp = this.transform.position;
        temp.x = point.x;
        temp.z = point.z;
        this.transform.position = temp;
    }

    void changePosition()
    {
        float xSize = 5f;
        float zSize = 10f;
        Vector3 basePosition = roomBase.transform.position;
        Vector3 size = area.bounds.size;
        //Debug.Log(basePosition);
        //Debug.Log(size);
        //Debug.Log(area.bounds);
        //float tempX=Random.Range(-size.x/2f, size.x/2f);
        //float tempZ=Random.Range(-size.z/2f, size.z/2f);
        float tempX = Random.Range(-1 * xSize, xSize);
        float tempZ = Random.Range(-1 * zSize, zSize);

        // while ((Mathf.Abs(tempX)+Mathf.Abs(tempZ))>(size.x/2f))
        // {
        //     tempX=Random.Range(-size.x/2f, size.x/2f);
        //     tempZ=Random.Range(-size.z/2f, size.z/2f);
        // }

        // while ((Mathf.Abs(tempX)+Mathf.Abs(tempZ))>(zSize))
        // {
        //     tempX=Random.Range(-1*xSize, xSize);
        //     tempZ=Random.Range(-1*zSize, zSize);

        // }
        while (true)
        {
            if (Physics.Raycast(new Vector3(tempX, 3.0f, tempZ), transform.up * -1)) break;
            tempX = Random.Range(-1 * xSize, xSize);
            tempZ = Random.Range(-1 * zSize, zSize);
        }

        float posX = basePosition.x + tempX;
        float posZ = basePosition.z + tempZ;

        Vector3 temp = this.transform.position;
        temp.x = posX;
        temp.z = posZ;
        this.transform.position = temp;
        actionController.setAction(false);
    }

    public void SetRoomBase(GameObject roomBase)
    {
        this.roomBase = roomBase;
        area = roomBase.GetComponent<MeshCollider>();
    }

    public void SetPet(GameObject pet)
    {
        this.pet = pet;
        actionController = pet.GetComponent<ActionController>();
    }
}