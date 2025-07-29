using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Despawn;
using _project.Scripts.LeoECS.Factories;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems
{
    public class ItemDropSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DropComponent, DespawnEvent>> _dropFilter;
        
        private readonly EcsCustomInject<IItemSpawnFactory> _itemSpawnFactory;

        public void Run(IEcsSystems systems)
        {
            var entities = _dropFilter.Value.GetRawEntities();
            var count = _dropFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var dropSourceEntity = entities[i];
                var spawnPosition = _dropFilter.Pools.Inc2.Get(dropSourceEntity).GameObject.transform.position;
                var dropComponent = _dropFilter.Pools.Inc1.Get(dropSourceEntity);

                _itemSpawnFactory.Value.Spawn(dropComponent.ID, dropComponent.Count, spawnPosition);
            }
        }
    }
}