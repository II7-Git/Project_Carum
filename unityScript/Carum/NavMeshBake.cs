using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBake : MonoBehaviour
{

    //public NavMeshSurface[] surfaces;
    public static GameObject [] arr;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Bake();
    }


    public static void Bake(){
        arr=GameObject.FindGameObjectsWithTag("RoomBase");
        //surfaces=;
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].GetComponent<NavMeshSurface>().BuildNavMesh();
            //surfaces[i].BuildNavMesh();
        }
    } 
}
