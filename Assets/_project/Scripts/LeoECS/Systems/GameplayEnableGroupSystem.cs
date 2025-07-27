using _project.Scripts.LeoECS.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;

namespace _project.Scripts.LeoECS.Systems
{
    public class GameplayEnableGroupSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<GameOverEvent>> _gameOverFilter;

        private readonly EcsPoolInject<EcsGroupSystemState> _ecsGroupSystemStatePool;

        private readonly EcsWorldInject _world;

        public void Run(IEcsSystems systems)
        {
            if (_gameOverFilter.Value.GetEntitiesCount() > 0)
            {
                var entity = _world.Value.NewEntity();

                ref var evt = ref _ecsGroupSystemStatePool.Value.Add(entity);
                evt.Name = SharedData.GameplayGroupName;
                evt.State = false;
            }
        }
    }
}