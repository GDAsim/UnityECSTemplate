namespace TemplateAlternate
{
    using Unity.Entities;
    using UnityEngine;

    /// <summary>
    /// Alternative way to setup
    /// </summary>
    public class TemplateAlternateAuthoring : MonoBehaviour
    {
        void Start()
        {
            // 1. Create the initial systems in the world
            var templateSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<TemplateSystem>();

            // 2. Find Existing SystemGroup to insert the system into
            var InitSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>();
            var SimSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();
            var PresentSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PresentationSystemGroup>();

            // 3. Add System to Appropriate Group

            // ========================  InitializationSystemGroup   ==============================
            InitSG.AddSystemToUpdateList(templateSystemHandle);

            // ===========================  SimulationSystemGroup       ===========================
            SimSG.AddSystemToUpdateList(templateSystemHandle);

            // ===========================  PresentationSystemGroup  ===========================
            PresentSG.AddSystemToUpdateList(templateSystemHandle);
        }
    }
}