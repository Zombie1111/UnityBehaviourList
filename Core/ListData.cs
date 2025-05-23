//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using UnityEngine;

namespace behLists
{
    public abstract class ListData : ScriptableObject
    {
        /// <summary>
        /// Called before Init and after OnWillDestroy, this may be called on the shared asset and not on the same instance of the asset as Init
        /// </summary>
        public virtual AssetInstanceMode GetInstanceMode()
        {
            return AssetInstanceMode.OneInstancePerReference;//There can only be one ListData per BehaviourList so AssetInstanceMode.OneInstancePerBehaviourList has no effect
        }

        /// <summary>
        /// Called on BehaviourList initilization (ListData is the first thing that gets initialized)
        /// </summary>
        /// <param name="behList">The BehaviourList this ListData is a part of</param>
        /// <param name="trans">The transform the BehaviourList is attatched to</param>
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
        /// Called when the BehaviourList should be reseted, can only be called if initilized
        /// </summary>
        public virtual void OnReset()
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
