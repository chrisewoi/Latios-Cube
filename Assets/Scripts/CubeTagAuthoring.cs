using Latios.Authoring;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

    public class CubeTagAuthoring : MonoBehaviour
    {
    }
public struct CubeTag : IComponentData{}
    public class CubeTagAuthoringBaker : Baker<CubeTagAuthoring>
    {
        public override void Bake(CubeTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<CubeTag>(entity);
        }
    }