using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZFrame.IO.ResourceSystem
{
    public class ResourceAsset : ScriptableObject
    {
        [HideInInspector] public List<Group> groups;

        [Serializable]
        public class Resource
        {
            public string resourceKey;
            public string desc;
            public Object resource;
        }

        [Serializable]
        public class Group
        {
            public string groupName;
            public string desc;
            public List<Resource> resources;
        }
    }
}