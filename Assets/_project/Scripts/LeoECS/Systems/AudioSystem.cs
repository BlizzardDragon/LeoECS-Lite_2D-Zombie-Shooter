using _project.Scripts.Audio;
using _project.Scripts.LeoECS.Components.Audio;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems
{
    public class AudioSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<AudioEvent>> _audioFilter;

        private readonly EcsCustomInject<IAudioPlayer> _audioPlayer;

        public void Run(IEcsSystems systems)
        {
            var entities = _audioFilter.Value.GetRawEntities();
            var count = _audioFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];

                _audioPlayer.Value.Play(_audioFilter.Pools.Inc1.Get(entity).Clip);
            }
        }
    }
}