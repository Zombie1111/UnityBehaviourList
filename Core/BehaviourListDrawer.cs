//UnityBehaviourList by David Westberg https://github.com/Zombie1111/UnityBehaviourList
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace behLists
{
    public static class BehListDrawGlob
    {
        public static readonly float spacing = EditorGUIUtility.standardVerticalSpacing;
        public static readonly float bigSpace = EditorGUIUtility.standardVerticalSpacing * 5.0f;
        public const float panelSideShift = 30.0f;
        public static readonly Color ogColor = EditorGUIUtility.isProSkin
        ? new Color32(56, 56, 56, 255)
        : new Color32(194, 194, 194, 255);
    }

    [CustomPropertyDrawer(typeof(behLists.BehaviourList))]
    public class BehaviourListDrawer : PropertyDrawer
    {
        private bool showRoots = false;
        private bool showBehList = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            //Showlist drop down
            var positionFold = position;
            positionFold.height = EditorGUIUtility.singleLineHeight;
            showBehList = EditorGUI.Foldout(positionFold, showBehList, "Show Behaviour List", true);
            if (showBehList == false) goto SkipDrawStuff;

            position.y += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;
            Color ogCol = BehListDrawGlob.ogColor;

            // Draw ListDataAsset
            SerializedProperty listDataAsset = property.FindPropertyRelative("listDataAsset");
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, listDataAsset, new GUIContent("List Data Asset"));
            position.y += position.height + BehListDrawGlob.spacing;
            EditorGUI.DrawRect(position, ogCol * 1.3f);

            // Draw Roots with Foldout
            SerializedProperty roots = property.FindPropertyRelative("roots");
            //showRoots = EditorGUI.Foldout(position, showRoots, "Roots");
            showRoots = true;

            position.height = EditorGUIUtility.singleLineHeight;
            if (GUI.Button(position, "Add Root"))
            {
                roots.arraySize++;
            }
            position.y += position.height + BehListDrawGlob.spacing;

            if (showRoots)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < roots.arraySize; i++)
                {
                    position.height = EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;
                    EditorGUI.DrawRect(position, ogCol * 1.15f);
                    position.height = EditorGUIUtility.singleLineHeight;
                    SerializedProperty root = roots.GetArrayElementAtIndex(i);

                    if (GUI.Button(position, "Remove Root " + root.FindPropertyRelative("rootId").stringValue))
                    {
                        roots.DeleteArrayElementAtIndex(i);
                        i--;
                        break;
                    }
                    position.y += position.height + BehListDrawGlob.spacing;


                    position.height = EditorGUI.GetPropertyHeight(root, true);
                    float ogX = position.x;
                    position.x += BehListDrawGlob.panelSideShift;
                    EditorGUI.DrawRect(position, ogCol * 1.15f);
                    position.x = ogX;
                    EditorGUI.PropertyField(position, root, new GUIContent($"Root {i}"), true);
                    position.y += position.height + (BehListDrawGlob.bigSpace * 4.0f);
                }

                EditorGUI.indentLevel--;
            }

        SkipDrawStuff:;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (showBehList == false) return EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;

            float height = EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; // ListDataAsset
            height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; // Foldout

            if (showRoots)
            {
                SerializedProperty roots = property.FindPropertyRelative("roots");
                for (int i = 0; i < roots.arraySize; i++)
                {
                    SerializedProperty root = roots.GetArrayElementAtIndex(i);
                    height += EditorGUI.GetPropertyHeight(root, true) + (BehListDrawGlob.bigSpace * 4.0f);
                    height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;
                }

                height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; // Add Root button
            }

            return height;
        }
    }

    [CustomPropertyDrawer(typeof(behLists.BehaviourList.Root))]
    public class RootDrawer : PropertyDrawer
    {
        private bool showBranches = false;
        private float spacing = EditorGUIUtility.standardVerticalSpacing;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.x += BehListDrawGlob.panelSideShift;
            position.width -= BehListDrawGlob.panelSideShift;
            Color ogCol = BehListDrawGlob.ogColor * 1.15f;
            EditorGUI.BeginProperty(position, label, property);

            //Draw Root ID
            SerializedProperty rootId = property.FindPropertyRelative("rootId");
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, rootId, new GUIContent("Root ID"));
            position.y += position.height + BehListDrawGlob.spacing;

            //Draw Active Branch ID
            SerializedProperty activeBranchId = property.FindPropertyRelative("activeBranchId");
            EditorGUI.PropertyField(position, activeBranchId, new GUIContent("Active Branch ID"));
            position.y += position.height + BehListDrawGlob.spacing;
            EditorGUI.DrawRect(position, ogCol * 1.3f);

            //Draw Branches Foldout
            SerializedProperty branches = property.FindPropertyRelative("branches");
            //showBranches = EditorGUI.Foldout(position, showBranches, "Branches");
            showBranches = true;
            position.height = EditorGUIUtility.singleLineHeight;
            if (GUI.Button(position, "Add Branch"))
            {
                branches.arraySize++;
            }
            position.y += position.height + BehListDrawGlob.spacing;

            if (showBranches)
            {
                EditorGUI.indentLevel++;

                for (int i = 0; i < branches.arraySize; i++)
                {
                    position.height = EditorGUIUtility.singleLineHeight + spacing;
                    EditorGUI.DrawRect(position, ogCol * 1.15f);
                    position.height = EditorGUIUtility.singleLineHeight;
                    SerializedProperty branch = branches.GetArrayElementAtIndex(i);

                    if (GUI.Button(position, "Remove Branch " + branch.FindPropertyRelative("branchId").stringValue))
                    {
                        branches.DeleteArrayElementAtIndex(i);
                        i--;
                        break;
                    }

                    position.y += position.height + BehListDrawGlob.spacing;
                    position.height = EditorGUI.GetPropertyHeight(branch, true);
                    EditorGUI.PropertyField(position, branch, new GUIContent($"Branch {i}"), true);
                    position.y += position.height + (BehListDrawGlob.bigSpace * 4.0f);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; // Root ID
            height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; // Active Branch ID
            height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; // Foldout

            if (showBranches)
            {
                SerializedProperty branches = property.FindPropertyRelative("branches");
                for (int i = 0; i < branches.arraySize; i++)
                {
                    SerializedProperty branch = branches.GetArrayElementAtIndex(i);
                    height += EditorGUI.GetPropertyHeight(branch, true) + (BehListDrawGlob.bigSpace * 4.0f);
                    height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;
                }

                height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; // Add Branch button
            }

            return height;
        }
    }

    [CustomPropertyDrawer(typeof(behLists.BehaviourList.Root.Branch))]
    public class BranchDrawer : PropertyDrawer
    {
        private bool showLeafs = false;
        private float spacing = EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.x += BehListDrawGlob.panelSideShift;
            position.width -= BehListDrawGlob.panelSideShift;
            EditorGUI.BeginProperty(position, label, property);
            Color ogCol = BehListDrawGlob.ogColor * 1.15f;

            //Draw Branch ID
            SerializedProperty branchId = property.FindPropertyRelative("branchId");
            //position.y += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;
            EditorGUI.DrawRect(position, ogCol * 1.15f);
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), branchId, new GUIContent("Branch ID"));

            //Draw branch behaviour
            position.y += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;
            SerializedProperty branchBeh = property.FindPropertyRelative("branchBehaviourAsset");
            //EditorGUI.DrawRect(position, ogCol * 1.15f);
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), branchBeh, new GUIContent("Branch Behaviour Asset"));

            //Draw leaf behaviour
            position.y += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;
            SerializedProperty leafBeh = property.FindPropertyRelative("leafBehaviour");
            //EditorGUI.DrawRect(position, ogCol * 1.15f);
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), leafBeh, new GUIContent("Leaf Behaviour"));

            //Draw Leafs
            SerializedProperty leafs = property.FindPropertyRelative("leafs");
            position.height = EditorGUIUtility.singleLineHeight;
            position.y += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;
            EditorGUI.DrawRect(position, ogCol * 1.45f);
            //showLeafs = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), showLeafs, "Leafs");
            showLeafs = true;
            if (GUI.Button(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), "Add Leaf"))
            {
                leafs.arraySize++;
            }
            position.y += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;

            if (showLeafs)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < leafs.arraySize; i++)
                {
                    position.height = EditorGUIUtility.singleLineHeight + spacing;
                    EditorGUI.DrawRect(position, BehListDrawGlob.ogColor * 1.3f * 1.15f);
                    position.height = EditorGUIUtility.singleLineHeight;
                    if (GUI.Button(position, "Remove Leaf"))
                    {
                        leafs.DeleteArrayElementAtIndex(i);
                        i--;
                        if (i < 0) break;
                    }

                    SerializedProperty leaf = leafs.GetArrayElementAtIndex(i);
                    position.y += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;
                    EditorGUI.PropertyField(position, leaf, new GUIContent($"Leaf {i}"), true);
                    position.y += EditorGUI.GetPropertyHeight(leaf) + BehListDrawGlob.spacing;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; // Branch ID
            height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; // Foldout
            height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; //Beh asset
            height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; //leaf beh

            if (showLeafs)
            {
                SerializedProperty leafs = property.FindPropertyRelative("leafs");
                for (int i = 0; i < leafs.arraySize; i++)
                {
                    SerializedProperty leaf = leafs.GetArrayElementAtIndex(i);
                    height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;
                    height += EditorGUI.GetPropertyHeight(leaf) + BehListDrawGlob.spacing;
                    //height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing;
                }

                //height += EditorGUIUtility.singleLineHeight + BehListDrawGlob.spacing; // Add Leaf button
            }

            return height;
        }
    }

    [CustomPropertyDrawer(typeof(behLists.BehaviourList.Root.Branch.Leaf))]
    public class LeafDrawer : PropertyDrawer
    {
        private bool showConditions = false;
        private bool showTops = false;
        private float spacing = EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.x += BehListDrawGlob.panelSideShift;
            position.width -= BehListDrawGlob.panelSideShift;
            EditorGUI.BeginProperty(position, label, property);
            Color ogCol = BehListDrawGlob.ogColor * 1.3f;

            // Draw Reverse
            SerializedProperty reverse = property.FindPropertyRelative("reverse");

            position.height = EditorGUIUtility.singleLineHeight + spacing;
            EditorGUI.DrawRect(position, ogCol * 1.15f);
            EditorGUI.PropertyField(position, reverse, new GUIContent("Reverse"));
            position.y += position.height;

            // Draw Requirement
            SerializedProperty requirement = property.FindPropertyRelative("requirement");
            EditorGUI.DrawRect(position, ogCol * 1.15f);
            EditorGUI.PropertyField(position, requirement, new GUIContent("Requirement"));
            position.y += position.height;

            // Draw Trigger
            SerializedProperty trigger = property.FindPropertyRelative("trigger");
            EditorGUI.DrawRect(position, ogCol * 1.15f);
            EditorGUI.PropertyField(position, trigger, new GUIContent("Trigger"));
            position.y += position.height;

            // Draw Leaf Conditions Foldout
            SerializedProperty leafConditionAssets = property.FindPropertyRelative("leafConditionAssets");
            //showConditions = EditorGUI.Foldout(position, showConditions, "Leaf Conditions");
            showConditions = true;

            if (showConditions)
            {
                EditorGUI.indentLevel++;
                position.height += spacing;
                var posB = position;
                posB.height = EditorGUIUtility.singleLineHeight;
                posB.width *= 0.49f;
                EditorGUI.DrawRect(position, ogCol * 1.3f);
                if (GUI.Button(posB, "Add Condition"))
                {
                    leafConditionAssets.arraySize++;
                }
                posB.x += posB.width + spacing;
                if (GUI.Button(posB, "Remove Condition") && leafConditionAssets.arraySize > 0)
                {
                    leafConditionAssets.arraySize--;
                }
                position.y += position.height;

                for (int i = 0; i < leafConditionAssets.arraySize; i++)
                {
                    SerializedProperty condition = leafConditionAssets.GetArrayElementAtIndex(i);
                    position.height = EditorGUI.GetPropertyHeight(condition, true);
                    EditorGUI.DrawRect(position, ogCol * 1.2f);
                    EditorGUI.PropertyField(position, condition, new GUIContent($"Condition {i}"), true);
                    position.y += position.height + (spacing * 2.0f);
                }

                EditorGUI.indentLevel--;
            }

            // Draw Tops Foldout
            SerializedProperty tops = property.FindPropertyRelative("tops");
            //showTops = EditorGUI.Foldout(position, showTops, "Tops");
            showTops = true;

            if (showTops)
            {
                EditorGUI.indentLevel++;

                var posB = position;
                posB.height = EditorGUIUtility.singleLineHeight + spacing;
                posB.width *= 0.49f;
                EditorGUI.DrawRect(position, ogCol * 1.3f);
                if (GUI.Button(posB, "Add Top"))
                {
                    tops.arraySize++;
                }
                posB.x += posB.width + spacing;
                if (GUI.Button(posB, "Remove Top") && tops.arraySize > 0)
                {
                    tops.arraySize--;
                }
                position.y += position.height;
                if (leafConditionAssets.arraySize > 0) position.y += spacing * 2.0f;

                for (int i = 0; i < tops.arraySize; i++)
                {
                    SerializedProperty top = tops.GetArrayElementAtIndex(i);
                    position.height = EditorGUI.GetPropertyHeight(top, true);
                    EditorGUI.DrawRect(position, ogCol * 1.2f);
                    EditorGUI.PropertyField(position, top, new GUIContent($"Top {i}"), true);
                    position.y += position.height + (spacing * 2.0f);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight + spacing; // Reverse
            height += EditorGUIUtility.singleLineHeight + spacing; // Requirement
            height += EditorGUIUtility.singleLineHeight + spacing; // Leaf Conditions Foldout

            if (showConditions)
            {
                SerializedProperty leafConditionAssets = property.FindPropertyRelative("leafConditionAssets");
                for (int i = 0; i < leafConditionAssets.arraySize; i++)
                {
                    SerializedProperty condition = leafConditionAssets.GetArrayElementAtIndex(i);
                    height += EditorGUI.GetPropertyHeight(condition, true) + (spacing * 2.0f);
                }
                height += EditorGUIUtility.singleLineHeight + spacing; // Add Condition button
            }

            height += EditorGUIUtility.singleLineHeight + spacing; // Trigger
            height += EditorGUIUtility.singleLineHeight + spacing; // Tops Foldout

            if (showTops)
            {
                SerializedProperty tops = property.FindPropertyRelative("tops");
                for (int i = 0; i < tops.arraySize; i++)
                {
                    SerializedProperty top = tops.GetArrayElementAtIndex(i);
                    height += EditorGUI.GetPropertyHeight(top, true) + (spacing * 2.0f);
                }

                height += EditorGUIUtility.singleLineHeight + spacing; // Add Condition button
            }

            return height;
        }
    }


    [CustomPropertyDrawer(typeof(behLists.BehaviourList.Root.Branch.Leaf.Top))]
    public class TopDrawer : PropertyDrawer
    {
        private float spacing = EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw Root ID to Set
            SerializedProperty rootIdToSet = property.FindPropertyRelative("rootIdToSet");
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, rootIdToSet, new GUIContent("Root ID to Set"));
            position.y += position.height + spacing;

            // Draw Root ID New Active Branch ID
            SerializedProperty rootIdNewActiveBranchId = property.FindPropertyRelative("rootIdNewActiveBranchId");
            EditorGUI.PropertyField(position, rootIdNewActiveBranchId, new GUIContent("New Active Branch ID"));
            position.y += position.height + spacing;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight + spacing; // Root ID to Set
            height += EditorGUIUtility.singleLineHeight + spacing; // Root ID New Active Branch ID

            return height;
        }
    }
}

#endif





