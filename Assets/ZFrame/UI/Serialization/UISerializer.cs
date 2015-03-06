using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using ZFrame.Base.MonoBase;

namespace ZFrame.UI.Serialization
{
    public class UISerializer : MonoSingleton<UISerializer>
    {
        //public static void Serialize<T>(T target) where T : Component
        //{
        //    if (target is RectTransform)
        //    {
        //        Debug.Log();
        //    }
        //}

        public static string Serialize(RectTransform target)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            Type type = target.GetType();
            XmlSerializer sz = new XmlSerializer(type);
            sz.Serialize(tw, target);
            tw.Close();
            return sb.ToString();
        }
    }
}