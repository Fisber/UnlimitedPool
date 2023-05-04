using Enums;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace Models
{
    public class DiskManager
    {
        private Dictionary<int, Dictionary<int, int>> GroupndexValueHolder = new();
        private SerializeUtil<Dictionary<int, int>> SerializeUtil = new();
        private DataStore DataStore = new();
        private int MaXGroupCount;

        public void GenerateGroups(int maxValue)
        {
            MaXGroupCount = (int) Math.Ceiling((float) maxValue / Configuration.GROUP_SIZE);

            Dictionary<int, int> data = new Dictionary<int, int>();

            Random ran = new Random();

            int currentGroupId = 1;

            int memberCount = 0;
            for (int i = 0; i < maxValue; i++)
            {
                data.Add(i, ran.Next(0, 100));
                memberCount++;
                if (memberCount == Configuration.GROUP_SIZE || i + 1 == maxValue)
                {
                    var serializedData = SerializeUtil.Serialize(data);
                    DataStore.SaveData(Configuration.GROUP_PREFIX + currentGroupId, serializedData);
                    currentGroupId++;
                    memberCount = 0;
                    data.Clear();
                }
            }
        }

        public void PreloadData(int index)
        {
            int currentGroupId = (int) Math.Ceiling((float) index / Configuration.GROUP_SIZE);

            currentGroupId = currentGroupId == 0 ? 1 : currentGroupId;
            int preloadFrom = currentGroupId - 1;

            for (int i = preloadFrom; i <= preloadFrom + 2; i++) 
            {
                if (i <= 0 || i > MaXGroupCount || GroupndexValueHolder.ContainsKey(i))
                {
                    continue;
                }
                
                var data = DataStore.LoadData(Configuration.GROUP_PREFIX + i);

                var deserializedData = SerializeUtil.DeSerialize(data);

                GroupndexValueHolder.Add(i, deserializedData);
            }

       
        }

        public Dictionary<int, int> FetchData(int index)
        {
            int currentGroupId = (int) Math.Ceiling((float) index / Configuration.GROUP_SIZE);

            currentGroupId = currentGroupId == 0 ? 1 : currentGroupId;
            PreloadData(index);
            RemoveExtraLoadedData(currentGroupId);
            
            // PreloadData(index);
            for (int i = currentGroupId - 1; i < currentGroupId + 2; i++)
            {
                if (GroupndexValueHolder.ContainsKey(i))
                {
                    if (GroupndexValueHolder[i].ContainsKey(index))
                    {
                        return GroupndexValueHolder[i];
                    }
                }
            }

            Debug.LogError($"problems !!!! {index} 1-{currentGroupId}+1 ");
            return null;
        }

        private void RemoveExtraLoadedData(int currentGroupIndex)
        {
            int affectedGroupsTail = currentGroupIndex + 2;
            int affectedGroupsHead = currentGroupIndex - 2;

            if (GroupndexValueHolder.ContainsKey(affectedGroupsTail))
            {
                GroupndexValueHolder.Remove(affectedGroupsTail);
            }

            if (GroupndexValueHolder.ContainsKey(affectedGroupsHead))
            {
                GroupndexValueHolder.Remove(affectedGroupsHead);
            }
        }
    }
}