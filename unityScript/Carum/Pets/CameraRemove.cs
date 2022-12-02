using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRemove : MonoBehaviour
{
    public List<GameObject> furnitures = new List<GameObject>();
    public List<GameObject> newAdded = new List<GameObject>();
    public GameObject pet;

bool isChange=false;
    void Start()
    {
        pet=GameObject.FindWithTag("Player");
    }
    void Update()
    {
        if(pet==null||pet.gameObject.activeSelf==false){
            isChange=true;
            // target=GameObject.FindWithTag("Destination").transform;
            // NavMeshBake.Bake();
        }

        if(isChange){
            if(GameObject.FindWithTag("Player")!=null){
                pet=GameObject.FindWithTag("Player");
                if(!pet) return;
                isChange=false;
            }
        }
        else{
        if(gameObject.GetComponent<CameraController>().cameraMode==1){
        float Distance = Vector3.Distance(transform.position, pet.transform.position);

        Vector3 Direction = (pet.transform.position - transform.position).normalized;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, Direction, Distance);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject != pet)
            {
                bool bFind = false;
                GameObject hitObject = hit.collider.gameObject;
                foreach (GameObject saved in furnitures)
                {
                    if (saved == hit.collider.gameObject)
                    {
                        //Debug.Log("이미 있어");
                        bFind = true;
                        newAdded.Add(hitObject);
                        break;
                    }
                }
                //새로운 장애물 발견
                if (bFind == false)
                { 
                    //hitObject.SetActive(false);
                    //hitObject.GetComponent<MeshRenderer>().materials[0].DOFade(0.0f,1.0f);
                    if(hitObject.GetComponent<MeshRenderer>()!=null)
                        hitObject.GetComponent<MeshRenderer>().enabled=false;
                    //hitObject.transform.DOScale(new Vector3(10.0f,10.0f,10.0f),1.0f);
                    //Debug.Log(hitObject.GetComponent<MeshRenderer>().materials[0]);
                    furnitures.Add(hitObject);
                    newAdded.Add(hitObject);
                }
            }
        }
        }
        // foreach (GameObject oldObject in furnitures)
        // {//기존 투명화됐던 물체들
        //  //Debug.Log(oldObject);
        //     bool bFind = false;
        //     foreach (GameObject newObject in newAdded)
        //     {
        //         if (newObject == oldObject)
        //         {
        //             bFind = true;
        //             break;
        //         }
        //     }
        //     if (bFind == false)
        //     {
        //         oldObject.SetActive(true);
        //         furnitures.Remove(oldObject);
        //     }
        // }
        // newAdded.Clear();
        int idx = 0;
        while (idx < furnitures.Count)
        {
            bool bFind = false;
            GameObject oldObject=furnitures[idx];
            foreach (GameObject newObject in newAdded)
            {
                if (newObject == oldObject)
                {
                    bFind = true;
                    break;
                }
            }

            //이번 충돌에 없다
            if (bFind == false)
            {
                //oldObject.GetComponent<MeshRenderer>().materials[0].DOFade(1.0f,1.0f);
                if(oldObject.GetComponent<MeshRenderer>()!=null)
                    oldObject.GetComponent<MeshRenderer>().enabled=true;
                furnitures.RemoveAt(idx);
            }else{
                idx++;
            }
        }
        newAdded.Clear();
        // if (Physics.Raycast(transform.position, Direction, out hits, Distance))

        // {
        //     Debug.Log("hit"+Hit.transform.gameObject);
        //     if(Hit.transform.gameObject!=Character)
        //         Hit.transform.gameObject.SetActive(false);
        //     // // 2.맞았으면 Renderer를 얻어온다.
        //     // ObstacleRenderer = Hit.transform.gameObject.GetComponentInChildren<Renderer>();

        //     // if (ObstacleRenderer != null)

        //     // {
        //     //     Debug.Log("hit"+Hit.transform.gameObject);
        //     //     // 3. Metrial의 Aplha를 바꾼다.

        //     //     Material Mat = ObstacleRenderer.material;

        //     //     Mat.shader=Shader.Find("Legacy Shaders/Transparent/Diffuse");

        //     //     Color matColor = Mat.color;

        //     //     matColor.a = 0.5f;

        //     //     Mat.color = matColor;

        //     // }
        // }
    }
    }
}
