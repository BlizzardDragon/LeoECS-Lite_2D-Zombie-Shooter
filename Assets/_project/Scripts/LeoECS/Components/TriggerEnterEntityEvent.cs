using Leopotam.EcsLite;

namespace _project.Scripts.LeoECS.Components
{
    public struct TriggerEnterEntityEvent
    {
        public EcsPackedEntity FirstPackedEntity;
        public EcsPackedEntity SecondPackedEntity;
    }
}