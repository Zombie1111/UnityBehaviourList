//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using UnityEngine;

namespace behLists
{
    public abstract class BranchBehaviour : ScriptableObject
    {
        /// <summary>
        /// Called on BehaviourList initilization after ListData.Init()
        /// </summary>
        /// <param name="behList">The BehaviourList this BranchBehaviour is a part of</param>
        /// <param name="listData">The listData asset copy, global for all BranchBehaviours and LeafConditions in this BehaviourList</param>
        /// <param name="rootId">The rootId this BranchBehaviour has</param>
        /// <param name="branchId">The branchId this BranchBehaviour has</param>
        /// <param name="trans">The transform the BranchBehaviour is attatched to</param>
        public virtual void Init(BehaviourList behList, ListData listData, Transform trans, string rootId, string branchId)
        {

        }

        /// <summary>
        /// Called after AnyRootTick on branch tick
        /// </summary>
        /// <param name="frameDelta">Scaled time in seconds from last frame to current frame (Usually Time.deltaTime)</param>
        /// <param name="tickDelta">Scaled time in seconds from last Tick or last OnActivated to current Tick</param>
        public virtual void Tick(float frameDelta, float tickDelta)
        {

        }

        /// <summary>
        /// Called just before BehaviourList destruction, Can only be called if Init() has been called before
        /// </summary>
        public virtual void OnWillDestroy()
        {

        }

        /// <summary>
        /// Called when this branch gets activated. Always called after Init() but before first Tick()
        /// </summary>
        /// <param name="oldBranchId">The previously active branchId in this root, equals String.Empty if no branch was previously active</param>
        public virtual void OnActivated(string oldBranchId)
        {

        }

        /// <summary>
        /// Called when this branch gets deactivated. Always called before OnWillDestroy() but after last Tick()
        /// </summary>
        /// <param name="newBranchId">The next active branchId in this root, equals String.Empty if no new branch will become active</param>
        public virtual void OnDeactivated(string newBranchId)
        {

        }
    }
}
