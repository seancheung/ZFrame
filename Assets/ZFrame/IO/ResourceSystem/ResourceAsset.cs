using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZFrame.IO.ResourceSystem
{
    public class ResourceAsset : ScriptableObject
    {
        [SerializeField] public List<Group> groups;

        [Serializable]
        public class Resource
        {
            public string resourceKey;
            public string desc;
            public Object resource;
            public string path;
            public ResourceType type;
        }

        [Serializable]
        public class Group
        {
            public string groupName;
            public string desc;
            [SerializeField] public List<Resource> resources;
            public GroupType type;
        }

        [Serializable]
        public enum GroupType
        {
            Automatic,
            Reference,
            PathLink
        }

        [Serializable]
        public enum ResourceType
        {
            Reference,
            PathLink
        }
    }
}