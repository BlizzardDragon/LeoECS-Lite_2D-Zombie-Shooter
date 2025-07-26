using _project.Scripts.LeoECS.Components.Tags;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems.Input
{
    public class InputInitSystem : IEcsInitSystem
    {
        private readonly EcsPoolInject<InputTag> _inputPool;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            
            _inputPool.Value.Add(entity);
        }
    }
}