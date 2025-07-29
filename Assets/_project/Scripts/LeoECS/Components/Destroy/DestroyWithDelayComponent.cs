using UnityEngine;

namespace _project.Scripts.LeoECS.Components.Destroy
{
    public struct DestroyWithDelayComponent
    {
        public GameObject GameObject;
        public float StartTime;
        public float Delay;
    }
}