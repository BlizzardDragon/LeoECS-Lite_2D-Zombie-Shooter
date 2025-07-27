using System;
using Leopotam.EcsLite;

namespace _project.Scripts.LeoECS.Utils
{
    public static class EntitiesUnpackUtility
    {
        public static (int firstEntity, int secondEntity) Unpack(
            EcsPackedEntity first, EcsPackedEntity second, EcsWorld world)
        {
            var tuple = (-1, -1);

            if (first.Unpack(world, out var firstEntity) && second.Unpack(world, out var secondEntity))
            {
                tuple = (firstEntity, secondEntity);
                return tuple;
            }

            throw new Exception($"Entities not unpacked! firstEntity = {tuple.Item1}, secondEntity = {tuple.Item2}");
        }
    }
}