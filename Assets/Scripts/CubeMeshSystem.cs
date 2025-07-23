using Latios;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Latios.Transforms;
using static Unity.Entities.SystemAPI;
[BurstCompile]
public partial struct CubeMeshSystem : ISystem
{
    LatiosWorldUnmanaged latiosWorld;
    EntityQuery cubeQuery;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        latiosWorld = state.GetLatiosWorldUnmanaged();
        cubeQuery = state.Fluent().With<Cube>().Build();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new SpawnCubesJob
        {
            icb = latiosWorld.syncPoint.CreateInstantiateCommandBuffer<WorldTransform, Cube>().AsParallelWriter(),
            transformLookup = GetComponentLookup<WorldTransform>(true),
        }.ScheduleParallel();
        new CubePositionJob
        {
            maxCubes = cubeQuery.CalculateEntityCount(),
            elapsedTime = (float)Time.ElapsedTime,
        }.ScheduleParallel();
    }
    [BurstCompile]
    partial struct SpawnCubesJob : IJobEntity
    {
        //Instead of an instantiation function and IMMEDIATELY creating a new object
        // We instead DECLARE that we want to spawn something AT SOME POINT
        //  We load up that declaration with information (the data we want the object to have)
        //      Then at some point NEXT FRAME when the jobs system thinks it's best, it will spawn that object
        public InstantiateCommandBuffer<WorldTransform, Cube>.ParallelWriter icb;
        //Some "boilerplate" we have to LOOKUP the transform information in memory, we can't just get it via reference anymore
        [ReadOnly] public ComponentLookup<WorldTransform> transformLookup;
        public void Execute([ChunkIndexInQuery] int chunkIndexInQuery, ref CubeMesh cubeMesh, CubePrefab cubePrefab)
        {
            //If we run this job infinitely the editor will crash.
            if (cubeMesh.amountSpawned > cubeMesh.maxAmount)
                return;
            //This is like getting a reference to a prefab for use of Instantiate()
            var prefabEntity = cubePrefab.prefab;
            //This gives us information about the transform of the PREFAB,
            //i.e. what size should the object we spawn be?
            //      where should we spawn the object?
            var prefabTransform = transformLookup[cubePrefab.prefab];
            var cube = new Cube
            {
                cubeIndex = cubeMesh.amountSpawned, //So we can tell which cube is which
            };
            //This is essentially GameObject.Instantiate(PREFAB, POSITION, QUATERNION.IDENTITY);
            icb.Add(prefabEntity, prefabTransform, cube, chunkIndexInQuery);
            cubeMesh.amountSpawned++;
        }
    }
//We may not actually have any important info stored on the cube, the tag is how we ensure the following job only runs on cubes and not just every entity
    [BurstCompile, WithAll(typeof(CubeTag))] 
    partial struct CubePositionJob : IJobEntity
    {
        public int maxCubes;
        //Here is some "external data" we shove in at the time we schedule the job
        public float elapsedTime;
        public void Execute(TransformAspect transformAspect, Cube cube)
        {
            //TransformAspect is basically Transform
            // use transformAspect.worldPosition to set where in the world you want the cubes to be
            float cubeIndexNormalized = (float)cube.cubeIndex / (float)maxCubes;
            float3 newPosition = float3.zero;
            newPosition.x = math.lerp(-30,30,cubeIndexNormalized);
            newPosition.y = 10*math.sin(cubeIndexNormalized - elapsedTime);
            transformAspect.worldPosition = newPosition;
        }
    }
}