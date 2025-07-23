using Latios.Authoring;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CubeAuthoring : MonoBehaviour
{
}

public struct Cube : IComponentData
{
    public int cubeIndex;
}

public class CubeAuthoringBaker : Baker<CubeAuthoring>
{
    public override void Bake(CubeAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent<Cube>(entity);
    }
}