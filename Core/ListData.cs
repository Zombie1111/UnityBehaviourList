//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using UnityEngine;

namespace behLists
{
    public abstract class ListData : ScriptableObject
    {
        /// <summary>
        /// Called on BehaviourList initilization (ListData is the first thing that gets initialized)
        /// </summary>
        /// <param name="behList">The BehaviourList this ListData is a part of</param>
        /// <param name="trans">The transform the behaviour list is attatched to</param>
        public virtual void Init(BehaviourList behList, Transform trans)
        {

        }

        /// <summary>
        /// Called everytime any root in this BehaviourList gets ticked
        /// </summary>
        /// <param name="rootIdTicked">The rootId that was ticked</param>
        /// <param name="frameDelta">Scaled time in seconds from last frame to current frame (Usually Time.deltaTime)</param>
        /// <param name="tickDelta">Scaled time in seconds from last AnyRootTick to current AnyRootTick</param>
        public virtual void AnyRootTick(string rootIdTicked, float frameDelta, float tickDelta)
        {

        }

        /// <summary>
        /// Called just before BehaviourList destruction, Can only be called if Init() has been called before
        /// </summary>
        public virtual void OnWillDestroy()
        {

        }
    }
}
