using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleVisualController : VisualController
{
    //얼굴 표정
    public  Dictionary<string, string> faceDict = new Dictionary<string, string>(){
        {"HAPPY","Face06"},
        {"SAD","Face10"},
        {"ANGRY","Face14"},
        {"SURPRISE","Face09"},
        {"WORRY","Face25"},
        {"PEACE","Face15"},
        {"NORMAL","Face17"},
        {"STUN","Face28"},
        {"HEART","Face26"},
        {"CONFUSE","Face20"}
    };

    // Start is called before the first frame update
    void Start()
    {
        meshObject=transform.GetChild(1);
        //newColor="01";
        //preColor="01";

        //faceObject=transform.GetChild(0).Find("Face_A");
       // newFace="normal";
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
                Material[] newMaterial=meshObject.GetComponent<SkinnedMeshRenderer>().materials;
                newMaterial[0]=Resources.Load("Whale_Face/"+faceDict[preFace], typeof(Material)) as Material;
                //newMaterial[0]=Resources.Load(faceLink+faceDict[preFace], typeof(Material)) as Material;
                meshObject.GetComponent<SkinnedMeshRenderer>().materials = newMaterial;

                //텍스쳐만 변경
                //faceObject.GetComponent<SkinnedMeshRenderer>().materials[0].mainTexture = Resources.Load("Dino_Face/"+faceDict[preFace], typeof(Texture)) as Texture;
            }
        }
    }

    protected override void ColorChange(){
        if(newColor!= preColor){
            //Debug.Log(meshObject.GetComponent<SkinnedMeshRenderer>());
            preColor=newColor;
            
             Material[] newMaterial=meshObject.GetComponent<SkinnedMeshRenderer>().materials;
             
                newMaterial[1]=Resources.Load("Whale_Color/Whale"+preColor, typeof(Material)) as Material;
                //Debug.Log(newMaterial[1]);
                if(newMaterial[1]==null) return;
                //newMaterial[0]=Resources.Load(faceLink+faceDict[preFace], typeof(Material)) as Material;
                meshObject.GetComponent<SkinnedMeshRenderer>().materials = newMaterial;
   
        }
    }
}
