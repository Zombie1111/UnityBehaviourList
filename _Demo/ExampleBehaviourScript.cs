//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using UnityEngine;
using behLists;

namespace behListsExamples
{
    public class ExampleBehaviourScript : MonoBehaviour
    {
        [SerializeField] private Transform aiTrans = null;
        [SerializeField] private Transform attackTarget = null;
        [SerializeField] private BehaviourList behList = new();
        private ExampleAiData aiData = null;

        private void Awake()
        {
            aiData = (ExampleAiData)behList.Init();
        }

        private void OnDestroy()
        {
            behList.Destroy();
        }

        private void Update()
        {
            aiData.aiPos = aiTrans.position;
            aiData.targetPos = attackTarget.position;

#if ENABLE_LEGACY_INPUT_MANAGER//I think this is for old input system
            if (Input.GetKeyDown(KeyCode.F) == true) behList.TickRoot("OnDamaged", Time.deltaTime);//Simulate ai being attacked by player
#endif
            behList.TickRoot("Main", Time.deltaTime);
        }
    }
}
