//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
using System.Collections.Generic;
using UnityEngine;

namespace behLists
{
    [System.Serializable]
    public class BehaviourList
    {
        #region BehaviourList
        [Tooltip("Assign a scriptableObject that inherit from ListData." +
            " A copy of this scriptableObject can be accessed from any BranchBehaviour or LeafCondition of this Behaviour List")]
        [SerializeField] private ListData listDataAsset = null;
        private ListData listData = null;
        [SerializeField] private List<Root> roots = new();

        private Dictionary<string, Root> rootIdToRoot = new();
        private float timeAtLastRootTick = 0.0f;
        private bool isInitilized = false;

        /// <summary>
        /// Initinilizes the behaviourList and returns the ListData copy (Null if listDataAsset was not assigned)
        /// </summary>
        public ListData Init()
        {
            if (isInitilized == true) return listData;
            isInitilized = true;

            timeAtLastRootTick = Time.time;

            if (listDataAsset != null)
            {
                listData = Object.Instantiate(listDataAsset);
                listData.Init(this);
            }
            else listData = null;

            foreach (Root root in roots)
            {
                if (rootIdToRoot.TryAdd(root.rootId, root) == false) Debug.LogError("More than one root has the rootId: " + root.rootId + ", this is not allowed");
                root.Init(this, listData);
            }

            return listData;
        }

        /// <summary>
        /// Call to properly destroy the behaviourList (Usually called in Monobehaviour.OnDestroy())
        /// </summary>
        public void Destroy()
        {
            if (isInitilized == false) return;
            isInitilized = false;

            foreach (Root root in roots)
            {
                root.Destroy();
            }

            rootIdToRoot.Clear();
            if (listData != null)
            {
                listData.OnWillDestroy();
                Object.Destroy(listData);
            }
        }

        /// <summary>
        /// Ticks the given rootId, throws exception if no root has the given rootId or Init() has not been called yet
        /// </summary>
        public void TickRoot(string rootId, float deltaTime)
        {
            float tickDelta = Time.time - timeAtLastRootTick;
            timeAtLastRootTick = Time.time;
            if (listData != null) listData.AnyRootTick(rootId, deltaTime, tickDelta);

            rootIdToRoot[rootId].Tick(deltaTime);
        }

        /// <summary>
        /// Sets the activeBranchId for the given rootId, throws exception if no root has the given rootId
        /// </summary>
        public void SetRootActiveBranchId(string rootId, string newActiveBranchId)
        {
            rootIdToRoot[rootId].SetActiveBranchId(newActiveBranchId);
        }

        /// <summary>
        /// Returns the activeBranchId for the given root, throws exception if no root has the given rootId
        /// </summary>
        public string GetRootActiveBranchId(string rootId)
        {
            return rootIdToRoot[rootId].GetActiveBranchId();
        }

        /// <summary>
        /// Returns array of bools (Null if no active branchId), true if Leaf conditions are met, one bool per Leaf (Does not trigger Leaf tops).
        /// Throws exception if no root has the given rootId
        /// </summary>
        public bool[] CheckRootActiveBranchConditions(string rootId, float deltaTime)
        {
            Root root = rootIdToRoot[rootId];//We dont allow choosing branchId since then Check() may be called on inactive branch
            return root.CheckBranchConditions(root.GetActiveBranchId(), deltaTime);
        }

        /// <summary>
        /// Returns true if any root has the given rootId
        /// </summary>
        public bool DoesRootIdExist(string rootId)
        {
            return rootIdToRoot.ContainsKey(rootId);
        }

        #endregion BehaviourList

        [System.Serializable]
        public class Root
        {
            #region Root
            [SerializeField] internal string rootId = "Main";
            [Tooltip("What branchId should be ticked when this root is ticked?")]
            [SerializeField] private string activeBranchId = "Base";
            [SerializeField] private List<Branch> branches = new();
            private Dictionary<string, Branch> branchIdToBranch = new();

            internal void SetActiveBranchId(string newActiveBranchId)
            {
                if (activeBranchId == newActiveBranchId) return;

                if (branchIdToBranch.TryGetValue(activeBranchId, out Branch branch) == true) branch.Deactivate(newActiveBranchId);
                if (branchIdToBranch.TryGetValue(newActiveBranchId, out branch) == true) branch.Activate(activeBranchId);
                activeBranchId = newActiveBranchId;
            }

            internal string GetActiveBranchId()
            {
                return activeBranchId;
            }

            internal void Init(BehaviourList behList, ListData listData)
            {
                if (rootId.Length == 0) Debug.LogError("A root has a rootId with lenght 0, this is not recommended");

                foreach (Branch branch in branches)
                {
                    if (branchIdToBranch.TryAdd(branch.branchId, branch) == false) Debug.LogError("More than one branch has the branchId: " + branch.branchId + ", this is not allowed");
                    branch.Init(behList, listData, rootId);
                }

                string ogActiveBranch = activeBranchId;
                activeBranchId = string.Empty;
                SetActiveBranchId(ogActiveBranch);
            }

            internal void Destroy()
            {
                SetActiveBranchId(string.Empty);

                foreach (Branch branch in branches)
                {
                    branch.Destroy();
                }

                branchIdToBranch.Clear();
            }

            internal void Tick(float deltaTime)
            {
                if (branchIdToBranch.TryGetValue(activeBranchId, out Branch branch) == false) return;
                branch.Tick(deltaTime);
            }

            internal bool[] CheckBranchConditions(string branchId, float deltaTime)
            {
                if (branchIdToBranch.TryGetValue(branchId, out Branch branch) == false) return null;
                return branch.CheckConditions(deltaTime);
            }

            #endregion Root

            [System.Serializable]
            public class Branch
            {
                #region Branch
                private enum LeafBehaviour
                {
                    checkAllAndTriggerFirstTrue = 0,
                    checkAllAndTriggerAllTrue = 1,
                    checkNextAndTriggerItIfTrue = 2,
                    checkRandomAndTriggerItIfTrue = 3
                }

                [SerializeField] internal string branchId = "Base";
                [Tooltip("Assign a scriptableObject that inherit from BranchBehaviour." +
                    " A copy of the assigned scriptableObject will recive callbacks depending on if this branchId is active or not")]
                [SerializeField] private BranchBehaviour branchBehaviourAsset = null;
                [Tooltip("How the leafs conditions and triggers will be checked and triggered, has no affect if this branch has less than 2 leafs")]
                [SerializeField] private LeafBehaviour leafBehaviour = LeafBehaviour.checkAllAndTriggerFirstTrue;
                [SerializeField] private List<Leaf> leafs = new();
                private BranchBehaviour branchBehaviour = null;
                private bool isActive = false;
                private float timeAtLastTick = 0.0f;
                private int lastCheckedLeafIndex = 0;
                [System.NonSerialized] private BehaviourList thisBehList = null;//Why do I need to specify NonSerialized??

                internal void Init(BehaviourList behList, ListData listData, string rootId)
                {
                    if (branchId.Length == 0) Debug.LogError("A branch has a branchId with lenght of 0, this is not recommended");

                    if (branchBehaviourAsset != null)
                    {
                        branchBehaviour = Object.Instantiate(branchBehaviourAsset);
                        branchBehaviour.Init(behList, listData, rootId, branchId);
                    }
                    else branchBehaviour = null;

                    thisBehList = behList;

                    foreach (Leaf leaf in leafs)
                    {
                        leaf.Init(behList, listData, rootId, branchId);
                    }
                }

                internal void Destroy()
                {
                    Deactivate(string.Empty);

                    foreach (Leaf leaf in leafs)
                    {
                        leaf.Destroy();
                    }

                    if (branchBehaviour == null) return;

                    branchBehaviour.OnWillDestroy();
                    Object.Destroy(branchBehaviour);
                }

                internal void Tick(float deltaTime)
                {
                    float tickDelta = Time.time - timeAtLastTick;
                    timeAtLastTick = Time.time;
                    if (branchBehaviour != null) branchBehaviour.Tick(deltaTime, tickDelta);

                    if (leafBehaviour == LeafBehaviour.checkNextAndTriggerItIfTrue)
                    {
                        lastCheckedLeafIndex++;
                        if (lastCheckedLeafIndex >= leafs.Count) lastCheckedLeafIndex = 0;
                        if (leafs[lastCheckedLeafIndex].Check(deltaTime) == false) return;
                        leafs[lastCheckedLeafIndex].TriggerTops(thisBehList);
                        return;
                    }

                    if (leafBehaviour == LeafBehaviour.checkRandomAndTriggerItIfTrue)
                    {
                        int rndLeafI = UnityEngine.Random.Range(0, leafs.Count);
                        if (leafs[rndLeafI].Check(deltaTime) == false) return;
                        leafs[rndLeafI].TriggerTops(thisBehList);
                        return;
                    }

                    foreach (Leaf leaf in leafs)
                    {
                        if (leaf.Check(deltaTime) == false) continue;

                        leaf.TriggerTops(thisBehList);
                        if (leafBehaviour == LeafBehaviour.checkAllAndTriggerFirstTrue) return;
                    }
                }

                internal bool[] CheckConditions(float deltaTime)
                {
                    bool[] bools = new bool[leafs.Count];

                    for (int i = 0; i < leafs.Count; i++)
                    {
                        bools[i] = leafs[i].Check(deltaTime);
                    }

                    return bools;
                }

                internal void Activate(string oldBranchId)
                {
                    if (isActive == true) return;
                    isActive = true;
                    timeAtLastTick = Time.time;
                    if (branchBehaviour != null) branchBehaviour.OnActivated(oldBranchId);

                    foreach (Leaf leaf in leafs)
                    {
                        leaf.Activate(oldBranchId);
                    }
                }

                internal void Deactivate(string newBranchId)
                {
                    if (isActive == false) return;

                    foreach (Leaf leaf in leafs)
                    {
                        leaf.Deactivate(newBranchId);
                    }

                    isActive = false;
                    if (branchBehaviour != null) branchBehaviour.OnDeactivated(newBranchId);
                }
                #endregion Branch

                [System.Serializable]
                public class Leaf
                {
                    #region Leaf
                    private enum Requirement
                    {
                        all = 0,
                        allOnce = 1,
                        any = 2
                    }

                    private enum Trigger
                    {
                        All = 0,
                        Random = 1,
                        RandomBranch = 2
                    }

                    [Tooltip("If true, conditions are reversed (false is true)")][SerializeField] private bool reverse = false;
                    [Tooltip("What Conditions needs to be true for the tops to trigger, if AllOnce, all conditions must have been true once after branch activision")]
                    [SerializeField] private Requirement requirement = Requirement.all;
                    [Tooltip("Assign a scriptableObject that inherit from LeafCondition. A copy of the assigned scriptableObject will recive callbacks to check its conditions")]
                    [SerializeField] private List<LeafCondition> leafConditionAssets = new();
                    private LeafCondition[] leafConditions = null;
                    [Tooltip("What tops will be triggered when the requirement is met," +
                        " if RandomBranch, there must be the same amount of rootIdNewActiveBranchId for every unique rootIdToSet")]
                    [SerializeField] private Trigger trigger = Trigger.All;
                    [SerializeField] private List<Top> tops = new();
                    private bool[] leafStates = null;
                    private Dictionary<string, List<string>> rootIdToBranchIds = new();
                    private float timeAtLastCheck = 0.0f;

                    internal void Init(BehaviourList behList, ListData listData, string rootId, string branchId)
                    {
                        leafStates = new bool[leafConditionAssets.Count];
                        leafConditions = new LeafCondition[leafConditionAssets.Count];

                        for (int i = 0; i < leafConditionAssets.Count; i++)
                        {
                            if (leafConditionAssets[i] == null) Debug.LogError("A LeafCondition in leafConditionAssets has not been assigned");
                            leafConditions[i] = Object.Instantiate(leafConditionAssets[i]);
                            leafConditions[i].Init(behList, listData, rootId, branchId);
                        }

                        foreach (Top top in tops)
                        {
                            if (top.rootIdNewActiveBranchId.Length == 0 || top.rootIdToSet.Length == 0)
                                Debug.LogError("A Top has a rootIdToSet or rootIdNewActiveBranchId with a lenght of 0, this is not recommended");

                            if (rootIdToBranchIds.TryGetValue(top.rootIdToSet, out var branchIds) == false)
                            {
                                branchIds = new();
                                rootIdToBranchIds.Add(top.rootIdToSet, branchIds);
                            }

                            branchIds.Add(top.rootIdNewActiveBranchId);
                        }
                    }

                    internal void Destroy()
                    {
                        foreach (LeafCondition leafCondition in leafConditions)
                        {
                            leafCondition.OnWillDestroy();
                            Object.Destroy(leafCondition);
                        }

                        rootIdToBranchIds.Clear();
                    }

                    private void SetAllLeafStates(bool newState)
                    {
                        for (int i = 0; i < leafStates.Length; i++)
                        {
                            leafStates[i] = newState;
                        }
                    }

                    internal void Activate(string oldBranchId)
                    {
                        if (requirement == Requirement.allOnce) SetAllLeafStates(false);
                        timeAtLastCheck = Time.time;

                        foreach (LeafCondition leafCondition in leafConditions)
                        {
                            leafCondition.OnActivated(oldBranchId);
                        }
                    }

                    internal void Deactivate(string newBranchId)
                    {
                        foreach (LeafCondition leafCondition in leafConditions)
                        {
                            leafCondition.OnDeactivated(newBranchId);
                        }
                    }

                    /// <summary>
                    /// Returns true if the leafConditions meets the requirement
                    /// </summary>
                    internal bool Check(float frameDelta)
                    {
                        if (requirement != Requirement.allOnce) SetAllLeafStates(false);
                        int trueCount = 0;//For simplicity we always check all previously false conditions
                        float checkDelta = Time.time - timeAtLastCheck;
                        timeAtLastCheck = Time.time;

                        for (int i = 0; i < leafConditions.Length; i++)
                        {
                            if (leafStates[i] == true)
                            {
                                trueCount++;
                                continue;//checkDelta will be fine since i wont be checked again before next Activate()
                            }

                            bool result = leafConditions[i].Check(frameDelta, checkDelta);
                            if (reverse == true) result = !result;
                            if (result == false) continue;

                            leafStates[i] = true;
                            trueCount++;
                        }

                        if (requirement == Requirement.any && trueCount > 0) return true;
                        return trueCount >= leafStates.Length;
                    }

                    /// <summary>
                    /// Triggers the tops, should only be called if Check() returned true
                    /// </summary>
                    internal void TriggerTops(BehaviourList behList)
                    {
                        if (tops.Count == 0) return;

                        if (trigger == Trigger.All)
                        {
                            foreach (Top top in tops)
                            {
                                if (behList.rootIdToRoot.TryGetValue(top.rootIdToSet, out Root root) == false) continue;
                                root.SetActiveBranchId(top.rootIdNewActiveBranchId);
                            }

                            return;
                        }

                        if (trigger == Trigger.Random)
                        {
                            var top = tops[UnityEngine.Random.Range(0, tops.Count)];
                            if (behList.rootIdToRoot.TryGetValue(top.rootIdToSet, out Root root) == false) return;
                            root.SetActiveBranchId(top.rootIdNewActiveBranchId);

                            return;
                        }

                        int rndBranchI = UnityEngine.Random.Range(0, rootIdToBranchIds[tops[0].rootIdToSet].Count);

                        foreach (var rootIdBranchIds in rootIdToBranchIds)
                        {
                            if (behList.rootIdToRoot.TryGetValue(rootIdBranchIds.Key, out Root root) == false) continue;
                            root.SetActiveBranchId(rootIdBranchIds.Value[rndBranchI]);
                        }
                    }

                    #endregion Leaf

                    [System.Serializable]
                    public class Top
                    {
                        #region Top

                        public string rootIdToSet = "Main";
                        [Tooltip("The root with rootIdToSet as ID will get this as active branchId when the top is triggered")]
                        public string rootIdNewActiveBranchId = "None";

                        #endregion Top
                    }
                }
            }
        }
    }
}
