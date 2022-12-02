using System.Collections;
using System.Collections.Generic;
using Carum.Interior;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class FrameItem : MonoBehaviour
{
    private int frameNumber;
    private RawImage image;
    // Start is called before the first frame update
    void Start()
    {
        frameNumber = transform.GetSiblingIndex()+1;
        image = GetComponent<RawImage>();
        image.texture = Resources.Load<Texture>("thumbnail/base/" + frameNumber);
    }

    public void ClickItem()
    {
        InteriorManager.Instance?.LoadFrame(this.frameNumber);
    }
}
