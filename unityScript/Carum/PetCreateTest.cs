using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetCreateTest : MonoBehaviour
{
    public GameObject DinoPrefab;
    public GameObject WhalePrefab;
    public string color;//색상
    public string emotion;//감정 종류
    public string type;//펫 종류


    private void Awake() {
        
    }
    private void Start(){
        //PetManager.Instance.MakePet();   
    }
    public Transform startingPoint;
    // Start is called before the first frame update
    
    public GameObject MakePet(){
        GameObject prePet=GameObject.FindWithTag("Player");
        if(prePet!=null){
            Destroy(prePet);
        }

        GameObject pet=null;
        if(type=="DINO"){
            pet=Instantiate(DinoPrefab);
            pet.name="DINO";
        }else if(type=="WHALE"){
            pet=Instantiate(WhalePrefab);
            pet.name="WHALE";
        }

        pet.transform.position=startingPoint.position;
        
        pet.GetComponent<VisualController>().newColor=color;
        pet.GetComponent<VisualController>().newFace=emotion;
        pet.GetComponent<VisualController>().setDefaultFace(emotion);
        return pet;
    }

    public GameObject MakePet(string color, string emotion, string type){
        this.color=color;
        this.emotion=emotion;
        this.type=type;
        GameObject prePet=GameObject.FindWithTag("Player");
        if(prePet!=null){
            Destroy(prePet);
        }

        GameObject pet=null;
        if(type=="DINO"){
            pet=Instantiate(DinoPrefab);
            pet.name="DINO";
        }else if(type=="WHALE"){
            pet=Instantiate(WhalePrefab);
            pet.name="WHALE";
        }

        pet.transform.position=startingPoint.position;
        //Debug.Log(pet.transform.position);
        pet.GetComponent<VisualController>().newColor=color;
        pet.GetComponent<VisualController>().newFace=emotion;
        pet.GetComponent<VisualController>().setDefaultFace(emotion);

        return pet;
        
    }
}
