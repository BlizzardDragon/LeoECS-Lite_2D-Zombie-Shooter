using UnityEngine;

namespace _project.Scripts.LeoECS.Components.Despawn
{
    public struct DespawnWithDelayComponent
    {
        public GameObject GameObject;
        public float StartTime;
        public float Delay;
    }
}