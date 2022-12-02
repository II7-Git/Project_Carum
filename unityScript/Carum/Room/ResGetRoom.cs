using System;
using System.Collections.Generic;

namespace Carum.Room
{
    [Serializable]
    public class ResGetRoom
    {
        public int background;
        public int frame;
        public List<InteriorDto> interiorList;
    }
}