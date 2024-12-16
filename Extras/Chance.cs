//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using UnityEngine;
using behLists;

namespace behListsExtras
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Custom/BehaviourList/Leafs/Chance")]
    public class Chance : LeafCondition
    {
        [Tooltip("The chance, 0.6f == 60% chance")][SerializeField] private float chance = 0.5f;
        [Tooltip("If true, the chance is checked once on branch activated")][SerializeField] private bool singleChance = true;
        private bool singleWasTrue = false;

        public override void Init(BehaviourList behList, ListData listData, string rootId, string branchId)
        {

        }

        public override bool Check(float frameDelta, float tickDelta)
        {
            if (singleChance == true) return singleWasTrue;
            return UnityEngine.Random.Range(0.0f, 1.0f) < chance;
        }

        public override void OnWillDestroy()
        {

        }

        public override void OnActivated(string oldBranchId)
        {
            if (singleChance == false) return;
            singleWasTrue = UnityEngine.Random.Range(0.0f, 1.0f) < chance;
        }

        public override void OnDeactivated(string newBranchId)
        {

        }
    }
}
