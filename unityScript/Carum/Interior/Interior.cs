using System;
using Carum.Room;
using UnityEngine;

// ReSharper disable Unity.InefficientPropertyAccess

namespace Carum.Interior
{
    [RequireComponent(typeof(Outline))]
    public class Interior : MonoBehaviour, IPuttable
    {
        private long _interiorId;
        private long _furnitureId;
        private Outline _outline;
        private LayerMask _layerMask;
        public InteriorManageState action = InteriorManageState.NONE;

        public static Interior Instantiate(GameObject furnitureObject)
        {
            GameObject instance = Instantiate(furnitureObject, Vector3.zero, Quaternion.Euler(0, 0, 0),GameObject.FindWithTag("Interior").transform);
            instance.AddComponent<Outline>().enabled = false;
            return instance.AddComponent<Interior>();
        }

        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Interior");
            _outline = GetComponent<Outline>();
        }

        private void Start()
        {
            _layerMask = LayerMask.GetMask("PickedInterior");
        }

        public void Init(long furnitureId)
        {
            _furnitureId = furnitureId;
            _interiorId = 0;
            action = InteriorManageState.ADD;
            _outline = GetComponent<Outline>();
        }

        public void Init(InteriorDto interiorDto)
        {
            _furnitureId = interiorDto.furnitureId;
            _interiorId = interiorDto.interiorId;
            transform.position = new Vector3(interiorDto.x, interiorDto.y, interiorDto.z);
            transform.rotation = Quaternion.Euler(interiorDto.rotX, interiorDto.rotY, interiorDto.rotZ);
            action = InteriorManageState.NONE;
            _outline = GetComponent<Outline>();
        }

        /// <summary>
        /// 해당 위치로 옮기기
        /// </summary>
        /// <param name="position">옮길 위치</param>
        public void Put(Vector3 position)
        {
            transform.SetPositionAndRotation(position, transform.rotation);
        }

        /// <summary>
        /// 레이캐스트가 맞은 지점에 배치
        /// </summary>
        /// <param name="hit">레이캐스트 지점</param>
        public void PutFromRaycast(RaycastHit hit)
        {
            // 히트지점 노멀벡터
            Vector3 hitNormal = hit.normal;
            Transform t = transform;
            Vector3 pivotPoint = t.position;

            Debug.DrawRay(pivotPoint - hitNormal * 5f, hitNormal * 5f, Color.green);

            Ray ray = new Ray(pivotPoint - hitNormal * 15f, hitNormal);

            if (Physics.SphereCast(ray, 10f, out var hitInfo, 20f, _layerMask))
            {
                Vector3 delta = t.position - hitInfo.point;

                Put(hit.point + delta);
            }
        }

        /// <summary>
        /// 방향벡터만큼 이동
        /// </summary>
        /// <param name="moveVector">이동할 방향</param>
        /// <param name="cameraTransform">카메라의 Transform</param>
        public void Move(Vector3 moveVector, Transform cameraTransform)
        {
            Vector3 cr = cameraTransform.right;
            float angle = (Mathf.Atan2(cr.z, cr.x) * Mathf.Rad2Deg+90)*Mathf.Deg2Rad;
            Vector3 cf = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            moveVector = Vector3.up * moveVector.y + cameraTransform.right * moveVector.x + cf * moveVector.z;

            transform.SetPositionAndRotation(transform.position + moveVector, transform.rotation);
        }

        /// <summary>
        /// 각도벡터만큼 각도 변경
        /// </summary>
        /// <param name="rotationVector">변경할 각도</param>
        public void Rotate(Vector3 rotationVector)
        {
            transform.Rotate(rotationVector, Space.World);
        }


        public void SetOutline(bool active)
        {
            if (_outline)
                _outline.enabled = active;
        }

        public InteriorDto ToDto()
        {
            Vector3 pos = transform.position;
            Vector3 rot = transform.rotation.eulerAngles;
            InteriorDto dto = new(_interiorId, _furnitureId, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z
                , action);

            return dto;
        }
    }
}