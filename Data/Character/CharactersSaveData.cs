using System;
using System.Collections.Generic;

namespace Core.Services
{
    [Serializable]
    public class CharactersSaveData
    {
        public List<CharacterData> Characters = new();
    }
}