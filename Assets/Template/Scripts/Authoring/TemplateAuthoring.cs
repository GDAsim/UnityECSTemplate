namespace Template
{
    using Unity.Entities;
    using UnityEngine;

    /// <summary>
    /// Scene Authoring Script
    /// </summary>
    public class TemplateAuthoring : MonoBehaviour
    {
        class Baker : Baker<TemplateAuthoring>
        {
            public override void Bake(TemplateAuthoring authoring)
            {
                // 1. Create the initial systems in the world
                var templateUnmanagedSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<TemplateUnmanagedSystem>();
                var templateManagedSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<TemplateManagedSystem>();

                // 2. Find Existing SystemGroup to insert the system into
                var InitSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>();
                var SimSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();
                var PresentSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PresentationSystemGroup>();

                // 3. Add System to Appropriate Group

                // ========================  InitializationSystemGroup   ==============================
                //InitSG.AddSystemToUpdateList(templateUnmanagedSystemHandle);

                // ===========================  SimulationSystemGroup       ===========================
                SimSG.AddSystemToUpdateList(templateManagedSystemHandle);
                SimSG.AddSystemToUpdateList(templateUnmanagedSystemHandle);

                // ===========================  PresentationSystemGroup  ===========================
                //PresentSG.AddSystemToUpdateList(templateUnmanagedSystemHandle);
            }
        }
    }
}