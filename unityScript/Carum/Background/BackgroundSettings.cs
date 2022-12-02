using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Carum.Background
{
    [CreateAssetMenu(fileName = "backgroundData", menuName = "ScriptableObjects/BackgroundSettingData", order = 1)]
    public class BackgroundSettings : ScriptableObject
    {
        public Color backgroundColor;
        public Color skyColor;
        public Color equatorColor;
        public Color groundColor;
        // public ShadowsMidtonesHighlights MidtonesHighlights;
        //
        // public void Test()
        // {
        //     MidtonesHighlights.shadows = new Vector4Parameter()
        // }
    }
}