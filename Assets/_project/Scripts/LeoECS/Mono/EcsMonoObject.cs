using Leopotam.EcsLite;
using UnityEngine;

namespace _project.Scripts.LeoECS.Mono
{
    public class EcsMonoObject : MonoBehaviour
    {
        protected EcsWorld _world;

        public EcsPackedEntity PackedEntity { get; private set; }

        public void Init(int entity, EcsWorld world)
        {
            _world = world;
            PackedEntity = world.PackEntity(entity);

            OnInit();
        }

        protected virtual void OnInit()
        {
        }
    }
}