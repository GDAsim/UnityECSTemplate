namespace TemplateAlternate
{
    using Unity.Entities;
    using UnityEngine;

    /// <summary>
    /// Alternative Way To Setup ECS, though monobehaviour instead of Authoring (not recommanded workflow)
    /// </summary>
    public class TemplateAlternateSetup : MonoBehaviour
    {
        void Start()
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
            //InitSG.AddSystemToUpdateList(templateSystemHandle);

            // ===========================  SimulationSystemGroup       ===========================
            SimSG.AddSystemToUpdateList(templateUnmanagedSystemHandle);
            SimSG.AddSystemToUpdateList(templateManagedSystemHandle);

            // ===========================  PresentationSystemGroup  ===========================
            //PresentSG.AddSystemToUpdateList(templateUnmanagedSystemHandle);
        }
    }
}