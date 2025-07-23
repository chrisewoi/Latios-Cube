using Latios;
using Latios.Authoring;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

    public class CubeMeshAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public int maxAmount;
    }

public struct CubeMesh : IComponentData
{
    public int maxAmount;
    public int amountSpawned;
}
public struct CubePrefab : IComponentData
{
    public EntityWith<Prefab> prefab;
}
    public class CubeMeshAuthoringBaker : Baker<CubeMeshAuthoring>
    {
        public override void Bake(CubeMeshAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CubePrefab
            {
                prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
            });
            AddComponent(entity, new CubeMesh
            {
                maxAmount = authoring.maxAmount,
            });
        }
    }