using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoVisualController : VisualController
{
    //얼굴 표정
    public  Dictionary<string, string> faceDict = new Dictionary<string, string>(){
        {"HAPPY","Dino_Face03"},
        {"SAD","Dino_Face09"},
        {"ANGRY","Dino_Face11"},
        {"SURPRISE","Dino_Face24"},
        {"WORRY","Dino_Face13"},//15
        {"PEACE","Dino_Face07"},
        {"NORMAL","Dino_Face01"},
        {"STUN","Dino_Face18"},
        {"HEART","Dino_Face19"},
        {"CONFUSE","Dino_Face14"}
    };

    // Start is called before the first frame update
    void Start()
    {
        meshObject=transform.GetChild(0);
        //newColor="01";
        //preColor="01";

        faceObject=transform.GetChild(0).Find("Face_A");
        //newFace="normal";
        //preFace="normal";
    }
    
    //각각 이거 오버라이딩
    public override void FaceChange(){
        if(newFace!=preFace){
            preFace=newFace;
            //Debug.Log(faceObject.GetComponent<SkinnedMeshRenderer>().materials[0]);
            if(faceDict.ContainsKey(preFace)){
                //Debug.Log(faceDict[preFace]);
                //Debug.Log(Resources.Load("Dino_Face/"+faceDict[preFace], typeof(Material)) as Material);

                //Material 자체 변경
                Material[] newMaterial=new Material[1];
                newMaterial[0]=Resources.Load("Dino_Face/"+faceDict[preFace], typeof(Material)) as Material;
                //newMaterial[0]=Resources.Load(faceLink+faceDict[preFace], typeof(Material)) as Material;
                faceObject.GetComponent<SkinnedMeshRenderer>().materials = newMaterial;

                //텍스쳐만 변경
                //faceObject.GetComponent<SkinnedMeshRenderer>().materials[0].mainTexture = Resources.Load("Dino_Face/"+faceDict[preFace], typeof(Texture)) as Texture;
            }
        }
    }

    protected override void ColorChange(){
        if(newColor!= preColor){
            preColor=newColor;
            Material newMat=Resources.Load("Dino_Color/Dino_"+preColor, typeof(Material)) as Material;
            if(newMat==null) return;
            //Material 자체 변경
            Material[] newMaterial=new Material[1];
            newMaterial[0]=newMat;

           for (int i = 0; i < 7; i++)
           {
            if(i==1)continue;//face 파트
            meshObject.GetChild(i).GetComponent<SkinnedMeshRenderer>().materials = newMaterial;
           }

             
        }
    }
}
