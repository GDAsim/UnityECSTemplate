namespace TemplateProcessJob
{
    using Unity.Entities;
    using UnityEngine;

    /// <summary>
    /// Scene Authoring Script
    /// </summary>
    public class TemplateProcessJobAuthoring : MonoBehaviour
    {
        class Baker : Baker<TemplateProcessJobAuthoring>
        {
            public override void Bake(TemplateProcessJobAuthoring authoring)
            {
                // 1. Create the initial systems in the world
                var templateProcessJobSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<TemplateProcessJobSystem>();

                // 2. Find Existing SystemGroup to insert the system into
                var InitSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>();
                var SimSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();
                var PresentSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PresentationSystemGroup>();

                // 3. Add System to Appropriate Group

                // ========================  InitializationSystemGroup   ==============================

                // ===========================  SimulationSystemGroup       ===========================
                SimSG.AddSystemToUpdateList(templateProcessJobSystemHandle);

                // ===========================  PresentationSystemGroup  ===========================
            }
        }
    }
}