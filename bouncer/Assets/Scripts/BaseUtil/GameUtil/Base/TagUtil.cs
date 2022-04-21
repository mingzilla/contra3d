using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;
using BaseUtil.GameUtil.Types;
using System;
using UnityEditor;

namespace BaseUtil.GameUtil.Base
{
    public static class TagUtil
    {
        public static void AddTagsIfNotPresent(List<string> tagNames)
        {
            AddPropertiesIfNotPresent("tags", tagNames);
        }

        public static void AddLayersIfNotPresent(List<string> layerNames)
        {
            AddPropertiesIfNotPresent("layers", layerNames);
        }

        /// <summary>
        /// Add tags or layers at Runtime, and they are only available when the game is running
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValues"></param>
        private static void AddPropertiesIfNotPresent(string propertyName, List<string> propertyValues)
        {
            UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if ((asset != null) && (asset.Length > 0))
            {
                SerializedObject so = new SerializedObject(asset[0]);
                SerializedProperty tags = so.FindProperty(propertyName);

                SelectedIds existingTags = SelectedIds.Create();

                for (int i = 0; i < tags.arraySize; ++i)
                {
                    existingTags = existingTags.SelectId(tags.GetArrayElementAtIndex(i).stringValue);
                }

                SelectedIds incomingTags = SelectedIds.Create().SelectAll(propertyValues);
                SelectedIds tagsToAdd = incomingTags.RemoveAll(existingTags.GetIds());

                tagsToAdd.GetIds().ForEach(t =>
                {
                    int nextIndex = tags.arraySize;
                    tags.InsertArrayElementAtIndex(nextIndex);
                    tags.GetArrayElementAtIndex(nextIndex).stringValue = t;
                });

                so.ApplyModifiedProperties();
                so.Update();
            }
        }
    }
}