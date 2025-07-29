using _project.Scripts.Configs;
using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Despawn;
using _project.Scripts.LeoECS.Components.Tags;
using _project.Scripts.LeoECS.Mono;
using _project.Scripts.LeoECS.Services;
using EasyPoolKit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Factories
{
    public interface IBulletSpawnFactory
    {
        void Spawn(TeamType team, int moveDirection, Vector3 position);
    }

    public class BulletSpawnFactory : IBulletSpawnFactory, IEcsSystem
    {
        private readonly EcsPoolInject<TransformComponent> _transformPool;
        private readonly EcsPoolInject<MoveDirectionComponent> _moveDirectionPool;
        private readonly EcsPoolInject<DamageComponent> _damagePool;
        private readonly EcsPoolInject<TeamComponent> _teamPool;
        private readonly EcsPoolInject<DespawnWithDelayComponent> _despawnWithDelayPool;
        private readonly EcsPoolInject<EcsMonoObjectComponent> _ecsMonoObjectPool;
        private readonly EcsPoolInject<PoolObjectTag> _poolObjectTagPool;

        private readonly EcsCustomInject<ITimeService> _timeService;
        private readonly EcsSharedInject<SharedData> _shared;
        private readonly EcsWorldInject _world;

        public void Spawn(TeamType team, int moveDirection, Vector3 position)
        {
            var bulletConfig = _shared.Value.BulletConfig;

            var bulletGO = SimpleGOPoolKit.Instance.SimpleSpawn(bulletConfig.Prefab.gameObject);

            var bulletTransform = bulletGO.transform;
            bulletTransform.SetParent(_shared.Value.BulletContainer);
            bulletTransform.position = position;
            bulletTransform.localScale = new Vector3(moveDirection, 1, 1);

            Init(bulletGO, team, moveDirection, bulletConfig);
        }

        private void Init(GameObject bulletGO, TeamType team, int moveDirection, BulletConfig bulletConfig)
        {
            if (!bulletGO.TryGetComponent<EcsMonoObject>(out var ecsMonoObject)) return;

            var entity = _world.Value.NewEntity();
            ecsMonoObject.Init(entity, _world.Value);

            _teamPool.Value.Add(entity).Team = team;
            _ecsMonoObjectPool.Value.Add(entity).EcsMonoObject = ecsMonoObject;
            _damagePool.Value.Add(entity).Damage = bulletConfig.Damage;
            _transformPool.Value.Add(entity).Transform = bulletGO.transform;
            _poolObjectTagPool.Value.Add(entity);

            ref var moveDirectionComponent = ref _moveDirectionPool.Value.Add(entity);
            moveDirectionComponent.MoveSpeed = bulletConfig.MoveSpeed;
            moveDirectionComponent.MoveDirection = moveDirection;

            ref var despawnWithDelayComponent = ref _despawnWithDelayPool.Value.Add(entity);
            despawnWithDelayComponent.Delay = bulletConfig.DestroyDelay;
            despawnWithDelayComponent.GameObject = bulletGO.gameObject;
            despawnWithDelayComponent.StartTime = _timeService.Value.CurrentTime;
        }
    }
}