//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using UnityEngine;
using behLists;

namespace behListsExamples
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Custom/BehaviourList/Leafs/ExampleTargetClose")]
    public class ExampleLeaf_targetClose : LeafCondition
    {
        [Tooltip("How far the AI can see its target")][SerializeField] private float seeRadius = 3.0f;
        [Tooltip("If true we want the target to be far away")][SerializeField] private bool reverse = false;
        private ExampleAiData aiData;

        public override void Init(BehaviourList behList, ListData listData, Transform trans, string rootId, string branchId)
        {
            aiData = (ExampleAiData)listData;
        }

        public override bool Check(float frameDelta, float tickDelta)
        {
            if (reverse == true) return (aiData.aiPos - aiData.targetPos).magnitude > seeRadius;
            return (aiData.aiPos - aiData.targetPos).magnitude < seeRadius;
        }

        public override void OnWillDestroy()
        {

        }

        public override void OnActivated(string oldBranchId)
        {

        }

        public override void OnDeactivated(string newBranchId)
        {

        }
    }
}
