using System;
using System.Collections.Generic;

namespace AlmostControl.DialogSystems
{
    [Serializable]
    public class AllDialogs
    {
        public Dictionary<string, string> RespawnDialogs;
        public Dictionary<string, string> Compliments;
    }

    public enum DialogType
    {
        Respawn,
        Compliment
    }
}