//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using UnityEngine;
using behLists;

namespace behListsExtras
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Custom/BehaviourList/Leafs/Wait")]
    public class Wait : LeafCondition
    {
        [Tooltip("Min delay in seconds")][SerializeField] private float delayMin = 0.8f;
        [Tooltip("Max delay in seconds, should be higher than delayMin")][SerializeField] private float delayMax = 1.2f;
        private float timer = 0.0f;

        public override void Init(BehaviourList behList, ListData listData, Transform trans, string rootId, string branchId)
        {

        }

        public override bool Check(float frameDelta, float tickDelta)
        {
            timer -= tickDelta;
            return timer < 0.0f;
        }

        public override void OnWillDestroy()
        {

        }

        public override void OnActivated(string oldBranchId)
        {
            timer = UnityEngine.Random.Range(delayMin, delayMax);
        }

        public override void OnDeactivated(string newBranchId)
        {

        }
    }
}
