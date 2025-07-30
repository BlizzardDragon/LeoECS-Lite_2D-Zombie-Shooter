using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Tags;
using LeoECS.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems.Player
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag>> _playerFilter;

        private readonly EcsPoolInject<EcsMonoObjectComponent> _ecsMonoObjectPool;
        private readonly EcsPoolInject<MoveDirectionComponent> _moveDirectionPool;
        private readonly EcsPoolInject<HealthComponent> _healthPool;
        private readonly EcsPoolInject<TeamComponent> _teamPool;
        private readonly EcsPoolInject<AmmoComponent> _ammoPool;
        private readonly EcsPoolInject<ReloadComponent> _reloadPool;

        private readonly EcsSharedInject<SharedData> _shared;

        public void Init(IEcsSystems systems)
        {
            _playerFilter.TryGetSingleEntity(out var entity);
            var playerConfig = _shared.Value.PlayerConfig;

            _ecsMonoObjectPool.Value.Get(entity).EcsMonoObject.Init(entity, systems.GetWorld());
            _teamPool.Value.Add(entity).Team = TeamType.PlayerTeam;
            _ammoPool.Value.Add(entity).AmmoCount = playerConfig.StartAmmoCount;
            _reloadPool.Value.Add(entity).ReloadDuration = playerConfig.ReloadDuration;

            ref var moveDirectionComponent = ref _moveDirectionPool.Value.Add(entity);
            moveDirectionComponent.MoveSpeed = playerConfig.MoveSpeed;
            moveDirectionComponent.LastMoveDirection = 1;

            ref var healthComponent = ref _healthPool.Value.Add(entity);
            healthComponent.CurrentHealth = playerConfig.StartHealth;
            healthComponent.MaxHealth = playerConfig.StartHealth;
        }
    }
}