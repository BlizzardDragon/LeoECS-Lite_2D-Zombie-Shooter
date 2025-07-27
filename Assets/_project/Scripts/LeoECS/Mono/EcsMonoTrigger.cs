using _project.Scripts.LeoECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace _project.Scripts.LeoECS.Mono
{
    public class EcsMonoTrigger : EcsMonoObject
    {
        private EcsPool<TriggerEnterEntityEvent> _triggerEnterEntityPool;

        protected override void OnInit()
        {
            base.OnInit();
            _triggerEnterEntityPool = _world.GetPool<TriggerEnterEntityEvent>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var attachedRigidbody = other.attachedRigidbody;
            if (attachedRigidbody)
            {
                if (attachedRigidbody.TryGetComponent<EcsMonoObject>(out var otherEcsMonoObject))
                {
                    ref var triggerEnterEntityComponent = ref _triggerEnterEntityPool.Add(_world.NewEntity());

                    triggerEnterEntityComponent.FirstPackedEntity = PackedEntity;
                    triggerEnterEntityComponent.SecondPackedEntity = otherEcsMonoObject.PackedEntity;

                    OnTriggerEnter2DAction(otherEcsMonoObject);
                }
            }
        }

        protected virtual void OnTriggerEnter2DAction(EcsMonoObject otherEcsMonoObject)
        {
        }
    }
}