using System;
using System.Collections.Generic;
using Latios;
using Latios.Authoring;
using Unity.Entities;

[UnityEngine.Scripting.Preserve]
public class LatiosBakingBootstrap : ICustomBakingBootstrap
{
    public void InitializeBakingForAllWorlds(ref CustomBakingBootstrapContext context)
    {
        //Latios.Authoring.CoreBakingBootstrap.ForceRemoveLinkedEntityGroupsOfLength1(ref context);
        Latios.Transforms.Authoring.TransformsBakingBootstrap.InstallLatiosTransformsBakers(ref context);
        Latios.Psyshock.Authoring.PsyshockBakingBootstrap.InstallUnityColliderBakers(ref context);
        Latios.Kinemation.Authoring.KinemationBakingBootstrap.InstallKinemation(ref context);
        Latios.Unika.Authoring.UnikaBakingBootstrap.InstallUnikaEntitySerialization(ref context);
    }
}

[UnityEngine.Scripting.Preserve]
public class LatiosEditorBootstrap : ICustomEditorBootstrap
{
    public World Initialize(string defaultEditorWorldName)
    {
        var world = new LatiosWorld(defaultEditorWorldName, WorldFlags.Editor);

        var systems = DefaultWorldInitialization.GetAllSystemTypeIndices(WorldSystemFilterFlags.Default, true);
        BootstrapTools.InjectUnitySystems(systems, world, world.simulationSystemGroup);

        Latios.Transforms.TransformsBootstrap.InstallTransforms(world, world.simulationSystemGroup);
        Latios.Kinemation.KinemationBootstrap.InstallKinemation(world);
        Latios.Calligraphics.CalligraphicsBootstrap.InstallCalligraphics(world);

        BootstrapTools.InjectUserSystems(systems, world, world.simulationSystemGroup);

        return world;
    }
}

[UnityEngine.Scripting.Preserve]
public class LatiosBootstrap : ICustomBootstrap
{
    public bool Initialize(string defaultWorldName)
    {
        var world                             = new LatiosWorld(defaultWorldName);
        World.DefaultGameObjectInjectionWorld = world;

        var systems = DefaultWorldInitialization.GetAllSystemTypeIndices(WorldSystemFilterFlags.Default);
        BootstrapTools.InjectUnitySystems(systems, world, world.simulationSystemGroup);

        //Latios.CoreBootstrap.InstallSceneManager(world);
        Latios.Transforms.TransformsBootstrap.InstallTransforms(world, world.simulationSystemGroup);
        Latios.Myri.MyriBootstrap.InstallMyri(world);
        Latios.Kinemation.KinemationBootstrap.InstallKinemation(world);
        Latios.Calligraphics.CalligraphicsBootstrap.InstallCalligraphics(world);
        Latios.Calligraphics.CalligraphicsBootstrap.InstallCalligraphicsAnimations(world);
        Latios.Unika.UnikaBootstrap.InstallUnikaEntitySerialization(world);
        //Latios.LifeFX.LifeFXBootstrap.InstallLifeFX(world);

        BootstrapTools.InjectUserSystems(systems, world, world.simulationSystemGroup);

        world.initializationSystemGroup.SortSystems();
        world.simulationSystemGroup.SortSystems();
        world.presentationSystemGroup.SortSystems();

        ScriptBehaviourUpdateOrder.AppendWorldToCurrentPlayerLoop(world);
        return true;
    }
}
