using _project.Scripts.LeoECS.Components.Input;
using _project.Scripts.LeoECS.Components.Tags;
using LeoECS.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems.Input
{
    public class InputMouseSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InputTag>> _inputFilter;

        private readonly EcsPoolInject<LmbHoldComponent> _lmbHoldPool;

        public void Run(IEcsSystems systems)
        {
            if (!_inputFilter.TryGetSingleEntity(out var entity)) return;

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (!_lmbHoldPool.Value.Has(entity))
                {
                    _lmbHoldPool.Value.Add(entity);
                }
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                if (_lmbHoldPool.Value.Has(entity))
                {
                    _lmbHoldPool.Value.Del(entity);
                }
            }
        }
    }
}