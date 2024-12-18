//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using UnityEngine;
using behLists;

namespace behListsExamples
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Custom/BehaviourList/ListData/ExampleAiData")]
    public class ExampleAiData : ListData
    {
        [System.NonSerialized] public Vector3 targetPos;
        [System.NonSerialized] public Vector3 aiPos;

        public override void Init(BehaviourList behList, Transform trans)
        {

        }

        public override void AnyRootTick(string rootIdTicked, float frameDelta, float tickDelta)
        {

        }

        public override void OnWillDestroy()
        {

        }
    }
}

