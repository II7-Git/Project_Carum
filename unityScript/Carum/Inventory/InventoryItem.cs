using System;
using System.Collections;
using Carum.Interior;
using Carum.UI;
using Carum.Util;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Carum.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        public RawImage image;
        public TMP_Text tmPro;

        private Furniture _furniture;
        private GameObject _itemObject;
        
        private bool _previewLoading;

        private Transform myT;
        private void Start()
        {
            myT = transform;
            myT.localScale = Vector3.one;
            myT.localRotation = Quaternion.Euler(0,0,0);
            var localPosition = myT.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y,
                0);
            myT.localPosition = localPosition;
        }

        /// <summary>
        /// 아이템 선택
        /// </summary>
        public void PickItem()
        {
            if (!_itemObject) _itemObject = Resources.Load<GameObject>(_furniture.GetResourcePath());

            Interior.Interior interior = Interior.Interior.Instantiate(_itemObject);
            interior.Init(_furniture.id);


            InteriorManager interiorManager = InteriorManager.Instance;
            interiorManager.SetPickedInterior(interior);
            interiorManager.ChangeMode(PutMode.Raycast);
            interiorManager.AddToInteriorTracking(interior);
            interiorManager.inventoryController.ToggleInventory(false);
        }

        /// <summary>
        /// 가구 정보 설정
        /// </summary>
        /// <param name="furniture">가구 객체</param>
        public void SetItem(Furniture furniture)
        {
            _furniture = furniture;
            _itemObject = null;
        }




        /// <summary>
        /// 가구 프리팹 로드
        /// </summary>
        public IEnumerator LoadAsset()
        {
            if (_furniture == null) yield break;
            tmPro.SetText(_furniture.name);
            image.texture = Resources.Load<Texture>(_furniture.GetIconResourcePath());

        }


    }
}