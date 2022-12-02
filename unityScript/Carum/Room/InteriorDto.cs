using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Carum.Room
{
    [Serializable]
    public class InteriorDto
    {
        public long interiorId;
        public long furnitureId;
        public string resource;
        public float x;
        public float y;
        public float z;
        public float rotX;
        public float rotY;
        public float rotZ;
        public string action;
        
        public InteriorDto(long interiorId, long furnitureId, float x, float y, float z, float rotX, float rotY, float rotZ, InteriorManageState action)
        {
            this.interiorId = interiorId;
            this.furnitureId = furnitureId;
            this.x = x;
            this.y = y;
            this.z = z;
            this.rotX = rotX;
            this.rotY = rotY;
            this.rotZ = rotZ;
            // ReSharper disable once HeapView.BoxingAllocation
            this.action = action.ToString();
        }

        public string GetResourcePath()
        {
            return "interiors/" + resource;
        }
    }
    public enum InteriorManageState
    {
        NONE,ADD,DEL,MOD
    }
}