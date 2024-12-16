//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using UnityEngine;
using behLists;

namespace behListsExamples
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Custom/BehaviourList/BranchBeh/ExampleIdle")]
    public class ExampleBranchBehIdle : BranchBehaviour
    {
        public override void Init(BehaviourList behList, ListData listData, string rootId, string branchId)
        {

        }

        public override void Tick(float frameDelta, float tickDelta)
        {

        }

        public override void OnActivated(string oldBranchId)
        {
            Debug.Log("Idle started");
        }

        public override void OnDeactivated(string newBranchId)
        {
            Debug.Log("Idle stopped");
        }

        public override void OnWillDestroy()
        {

        }
    }
}
