using System;
using System.Collections.Generic;

namespace Carum.Room
{
    [Serializable]
    public class ReqPutRoom
    {
        public int frame;
        public int background;
        public List<InteriorDto> interiorList = new();
    }
}