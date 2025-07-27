using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Events;
using _project.Scripts.LeoECS.Services;
using _project.Scripts.LeoECS.Systems;
using _project.Scripts.LeoECS.Systems.Input;
using _project.Scripts.LeoECS.Systems.Player;
using _project.Scripts.LeoECS.Systems.UI;
using AB_Utility.FromSceneToEntityConverter;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;

namespace _project.Scripts.LeoECS
{
    class Startup : MonoBehaviour
    {
        [SerializeField] private EcsUguiEmitter _ecsUguiEmitter;
        [SerializeField] private SharedData _sharedData;

        EcsWorld _world;
        IEcsSystems _systems;

        void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world, _sharedData);
            _systems
                .Add(new TimeSystem())
                // ===== Init    
                .Add(new InputInitSystem())
                .Add(new PlayerInitSystem())
                // ===== Groups    
                .AddGroup(SharedData.GameplayGroupName, true, null,
                    // ===== Input
                    new InputMouseSystem(),
                    new PlayerMoveInputSystem(),
                    // ===== Move    
                    new TargetFollowSystem(),
                    new MoveDirectionSystem(),
                    // ===== Spawn    
                    new BulletSpawnSystem(),
                    new EnemySpawnSystem(),
                    // ===== Damage    
                    new DamageSystem(),
                    new DeathSystem(),
                    // ===== GameOver
                    new GameOverSystem(),
                    // ===== Animations    
                    new MoveFlipAnimationSystem(),
                    new AnimationSystem(),
                    // ===== View
                    new HealthBarSystem(),
                    // ===== UI
                    new GameOverScreenSystem()
                )
                // ===== EnableGroups    
                .Add(new GameplayEnableGroupSystem())
                // ===== UI
                .Add(new ClickExitEventSystem())
                .Add(new ClickRestartEventSystem())
                // ===== Destroy    
                .Add(new DestroySystem())
                .Add(new DestroyWithDelaySystem())
                // ===== DeleteEvents    
                .DelHere<ShootAnimationEvent>()
                .DelHere<TriggerEnterEntityEvent>()
                .DelHere<DestroyEvent>()
                .DelHere<GameOverEvent>()
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem())
#endif
                .ConvertScene()
                .InjectUgui(_ecsUguiEmitter)
                .Inject((ITimeService) new TimeService())
                .Init();
        }

        void Update()
        {
            _systems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}