using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Carum.Util
{
    public class PreviewUtil : MonoBehaviour
    {
        public static bool GetObjectPreview(GameObject targetObject)
        {
            if (!targetObject)
                return false;
            // 로딩중이 아니면 로드
            // 로딩중이면 false
#if UNITY_EDITOR == true

            if (AssetPreview.IsLoadingAssetPreview(targetObject.GetInstanceID()))
                return false;
            return AssetPreview.GetAssetPreview(targetObject);
#endif
            return false;
        }
    }
}