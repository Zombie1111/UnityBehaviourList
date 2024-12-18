//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using UnityEngine;
using behLists;

namespace behListsExamples
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Custom/BehaviourList/BranchBeh/ExampleAttack")]
    public class ExampleBranchBehAttack : BranchBehaviour
    {
        public override void Init(BehaviourList behList, ListData listData, Transform trans, string rootId, string branchId)
        {

        }

        public override void Tick(float frameDelta, float tickDelta)
        {

        }

        public override void OnActivated(string oldBranchId)
        {
            Debug.Log("Attacking start");
        }

        public override void OnDeactivated(string newBranchId)
        {
            Debug.Log("Attacking stop");
        }

        public override void OnWillDestroy()
        {

        }
    }
}
