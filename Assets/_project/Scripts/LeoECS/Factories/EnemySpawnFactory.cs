using _project.Scripts.Configs;
using _project.Scripts.Configs.Enemies;
using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Audio;
using _project.Scripts.LeoECS.Components.Tags;
using _project.Scripts.LeoECS.Mono;
using AB_Utility.FromSceneToEntityConverter;
using EasyPoolKit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Factories
{
    public interface IEnemySpawnFactory
    {
        void Spawn(Transform target, Vector3 position);
    }

    public class EnemySpawnFactory : IEnemySpawnFactory, IEcsSystem
    {
        private readonly EcsPoolInject<TransformComponent> _transformPool;
        private readonly EcsPoolInject<MoveDirectionComponent> _moveDirectionPool;
        private readonly EcsPoolInject<HealthComponent> _healthPool;
        private readonly EcsPoolInject<TargetFollowComponent> _targetFollowPool;
        private readonly EcsPoolInject<DamageComponent> _damagePool;
        private readonly EcsPoolInject<TeamComponent> _teamPool;
        private readonly EcsPoolInject<EcsMonoObjectComponent> _ecsMonoObjectPool;
        private readonly EcsPoolInject<DropComponent> _dropPool;
        private readonly EcsPoolInject<HitAudioComponent> _hitAudioPool;
        private readonly EcsPoolInject<AttackAudioComponent> _attackAudioPool;
        private readonly EcsPoolInject<DeathAudioComponent> _deathAudioPool;
        private readonly EcsPoolInject<PoolObjectTag> _poolObjectTagPool;

        private readonly EcsSharedInject<SharedData> _shared;
        private readonly EcsWorldInject _world;

        public void Spawn(Transform target, Vector3 position)
        {
            var enemyConfig = _shared.Value.EnemyConfigProvider.GetRandomConfig();

            var enemyGO = SimpleGOPoolKit.Instance.SimpleSpawn(enemyConfig.Prefab.gameObject);

            enemyGO.transform.SetParent(_shared.Value.EnemyContainer);
            enemyGO.transform.position = position;

            Init(enemyGO, target, enemyConfig);
        }

        private void Init(GameObject enemyGO, Transform target, EnemyConfig enemyConfig)
        {
            if (!enemyGO.TryGetComponent<EcsMonoObject>(out var ecsMonoObject)) return;

            var enemyEntity = _world.Value.NewEntity();
            ecsMonoObject.Init(enemyEntity, _world.Value);

            _targetFollowPool.Value.Add(enemyEntity).Target = target;
            _teamPool.Value.Add(enemyEntity).Team = TeamType.EnemyTeam;
            _ecsMonoObjectPool.Value.Add(enemyEntity).EcsMonoObject = ecsMonoObject;
            _transformPool.Value.Add(enemyEntity).Transform = ecsMonoObject.transform;
            _moveDirectionPool.Value.Add(enemyEntity).MoveSpeed = enemyConfig.MoveSpeed;
            _damagePool.Value.Add(enemyEntity).Damage = enemyConfig.Damage;
            _hitAudioPool.Value.Add(enemyEntity).Clip = enemyConfig.AudioClipHit;
            _attackAudioPool.Value.Add(enemyEntity).Clip = enemyConfig.AudioClipAttack;
            _deathAudioPool.Value.Add(enemyEntity).Clip = enemyConfig.AudioClipDeath;
            _poolObjectTagPool.Value.Add(enemyEntity);

            ref var healthComponent = ref _healthPool.Value.Add(enemyEntity);
            healthComponent.MaxHealth = enemyConfig.Health;
            healthComponent.CurrentHealth = enemyConfig.Health;

            ref var dropComponent = ref _dropPool.Value.Add(enemyEntity);
            dropComponent.ID = enemyConfig.ItemDrop.ID;
            dropComponent.Count = Random.Range(enemyConfig.MinDrop, enemyConfig.MaxDrop + 1);

            if (enemyGO.TryGetComponent<ComponentsContainer>(out var componentsContainer))
            {
                var converters = componentsContainer.Converters;

                for (int i = 0; i < converters.Length; i++)
                {
                    var converter = converters[i];
                    converter.Convert(_world.Value.PackEntityWithWorld(enemyEntity));
                }
            }
        }
    }
}