using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualController : MonoBehaviour
{
    //얼굴 표정//펫에 맞게 다시 작성
    // [Header("Emotion")]
    // public Dictionary<string, string> faceDict = new Dictionary<string, string>(){
    //     {"happy","Dino_Face23"},
    //     {"sad","Dino_Face05"},
    //     {"smile","Dino_Face03"},
    //     {"cry","Dino_Face09"},
    //     {"normal","Dino_Face01"}
    // };
    //Dino Color 는 01~25

    [Header("Color")]
    protected Transform meshObject;
    public string newColor;
    protected string preColor;

    [Header("Face")]
    protected Transform faceObject;
    public string newFace;
    protected string preFace;
    public string defaultFace;

    public void doDefaultFace(){
        newFace=defaultFace;
    }
    public void setDefaultFace(string emotion){
        defaultFace=emotion;
    }
    public void setEmotion(string emotion){
        newFace=emotion;
    }

    public void setColor(string color){
        newColor=color;
    }
    // Start is called before the first frame update
    //이 부분도 overRidding필요
    void Start()
    {
        meshObject=transform.GetChild(0);
        newColor="01";
        preColor="01";

        faceObject=transform.GetChild(0).Find("Face_A");
        newFace="normal";
        preFace="normal";
    }

    // Update is called once per frame
    void Update()
    {
        FaceChange();
        ColorChange();
    }


    //각각 이거 오버라이딩
    public virtual void FaceChange(){
        // if(newFace!=preFace){
        //     preFace=newFace;
        //     //Debug.Log(faceObject.GetComponent<SkinnedMeshRenderer>().materials[0]);
        //     if(faceDict.ContainsKey(preFace)){
        //         //Debug.Log(faceDict[preFace]);
        //         //Debug.Log(Resources.Load("Dino_Face/"+faceDict[preFace], typeof(Material)) as Material);

        //         //Material 자체 변경
        //         Material[] newMaterial=new Material[1];
        //         newMaterial[0]=Resources.Load("Dino_Face/"+faceDict[preFace], typeof(Material)) as Material;
        //         //newMaterial[0]=Resources.Load(faceLink+faceDict[preFace], typeof(Material)) as Material;
        //         faceObject.GetComponent<SkinnedMeshRenderer>().materials = newMaterial;

        //         //텍스쳐만 변경
        //         //faceObject.GetComponent<SkinnedMeshRenderer>().materials[0].mainTexture = Resources.Load("Dino_Face/"+faceDict[preFace], typeof(Texture)) as Texture;
        //     }
        // }
    }

    protected virtual void ColorChange(){
        // if(newColor!= preColor){
        //     preColor=newColor;
        //     Material newMat=Resources.Load("Dino_Color/Dino_"+preColor, typeof(Material)) as Material;
        //     if(newMat==null) return;
        //     //Material 자체 변경
        //     Material[] newMaterial=new Material[1];
        //     newMaterial[0]=newMat;

        //    for (int i = 0; i < 7; i++)
        //    {
        //     if(i==1)continue;//face 파트
        //     meshObject.GetChild(i).GetComponent<SkinnedMeshRenderer>().materials = newMaterial;
        //    }

             
        // }
    }
}
