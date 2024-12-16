//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using UnityEngine;

namespace behLists
{
    public abstract class LeafCondition : ScriptableObject
    {
        /// <summary>
        /// Called on BehaviourList initilization after BranchBehaviour.Init()
        /// </summary>
        /// <param name="behList">The BehaviourList this LeafCondition is a part of</param>
        /// <param name="listData">The listData asset copy, global for all BranchBehaviours and LeafConditions in this BehaviourList</param>
        /// <param name="rootId">The rootId this LeafCondition has</param>
        /// <param name="branchId">The branchId this LeafCondition has</param>
        public virtual void Init(BehaviourList behList, ListData listData, string rootId, string branchId)
        {
            
        }

        /// <summary>
        /// Called after branch tick on leaf tick
        /// </summary>
        /// <param name="frameDelta">Scaled time in seconds from last frame to current frame (Usually Time.deltaTime)</param>
        /// <param name="checkDelta">Scaled time in seconds from last Check or last OnActivated to current Check</param>
        /// <returns>True if this condition is true</returns>
        public virtual bool Check(float frameDelta, float checkDelta)
        {
            return true;
        }

        /// <summary>
        /// Called just before BehaviourList destruction, Can only be called if Init() has been called before
        /// </summary>
        public virtual void OnWillDestroy()
        {

        }

        /// <summary>
        /// Called when this branch gets activated. Always called after Init() but before first Check()
        /// </summary>
        /// <param name="oldBranchId">The previously active branchId in this root, equals String.Empty if no branch was previously active</param>
        public virtual void OnActivated(string oldBranchId)
        {

        }

        /// <summary>
        /// Called when this branch gets deactivated. Always called before OnWillDestroy() but after last Check()
        /// </summary>
        /// <param name="newBranchId">The next active branchId in this root, equals String.Empty if no new branch will become active</param>
        public virtual void OnDeactivated(string newBranchId)
        {

        }
    }
}
