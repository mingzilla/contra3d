using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;
using BaseUtil.GameUtil.Types;
using System;
using System.Linq;
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

        public static void LogErrorIfTagsNotPresent(List<string> tagNames)
        {
            LogErrorIfNotPresent("tags", tagNames);
        }

        public static void LogErrorIfLayersIfNotPresent(List<string> layerNames)
        {
            LogErrorIfNotPresent("layers", layerNames);
        }

        /// <summary>
        /// Add tags or layers at Runtime, and they are only available when the game is running
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValues"></param>
        private static void AddPropertiesIfNotPresent(string propertyName, List<string> propertyValues)
        {
            HandlePropertiesIfNotPresent(propertyName, propertyValues, (so, tags, missingItems) =>
            {
                missingItems.GetIds().ForEach(t =>
                {
                    int nextIndex = tags.arraySize;
                    tags.InsertArrayElementAtIndex(nextIndex);
                    tags.GetArrayElementAtIndex(nextIndex).stringValue = t;
                });

                so.ApplyModifiedProperties();
                so.Update();
            });
        }

        /// <summary>
        /// Add tags or layers at Runtime, and they are only available when the game is running
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValues"></param>
        private static void LogErrorIfNotPresent(string propertyName, List<string> propertyValues)
        {
            HandlePropertiesIfNotPresent(propertyName, propertyValues, (so, tags, missingItems) =>
            {
                if (!missingItems.IsEmpty()) Debug.LogError(string.Join(", ", missingItems.GetIds()));
            });
        }

        /// <summary>
        /// Add tags or layers at Runtime, and they are only available when the game is running
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValues"></param>
        private static void HandlePropertiesIfNotPresent(string propertyName, List<string> propertyValues, Action<SerializedObject, SerializedProperty, SelectedIds> handleMissingItemsFn)
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
                SelectedIds missingItems = incomingTags.RemoveAll(existingTags.GetIds());

                handleMissingItemsFn(so, tags, missingItems);
            }
        }
    }
}