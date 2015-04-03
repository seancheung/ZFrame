using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Xml.Serialization;
using Microsoft.CSharp;
using UnityEditor;
using UnityEngine;

public class CodeBeach
{
    [MenuItem("GameObject/Script")]
    public static ICodeGenerator Create()
    {
        string @class = "NewMono";
        string fileName = "Assets/" + @class + ".cs";
        TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Create));
        ICodeGenerator codeGenerator = new CSharpCodeProvider().CreateGenerator();
        CodeGeneratorOptions options = new CodeGeneratorOptions {BlankLinesBetweenMembers = true, BracingStyle = "C"};
        CodeTypeDeclaration cls = new CodeTypeDeclaration(@class);
        cls.BaseTypes.Add(typeof (MonoBehaviour));
        cls.IsClass = true;
        cls.Attributes = MemberAttributes.Public;
        CodeMemberMethod meth = new CodeMemberMethod
        {
            Name = "Start",
            ReturnType = new CodeTypeReference(typeof (void)),
            Attributes = MemberAttributes.Private
        };
        cls.Members.Add(meth);
        codeGenerator.GenerateCodeFromType(cls, tw, options);
        tw.Flush();
        tw.Close();

        XmlSerializer xs = new XmlSerializer(typeof(ClassData));
        TextWriter xtw = new StreamWriter(new FileStream("Assets/data.xml", FileMode.Create));
        var mono = new ClassData();
        mono.Name = "Mono";
        xs.Serialize(xtw,mono);
        xtw.Flush();
        xtw.Close();
        AssetDatabase.Refresh();
        return codeGenerator;
    }

    [XmlType("Method")]
    public class MethodData
    {
        public string Name = "NewMethod";
        public MemberAttributes Attribute = MemberAttributes.Private;
        public string ReturnType = "void";
        public string[] ParameterTypes;
        public string Commont;
        public string MethodBody;
    }

    [XmlType("Field")]
    public class FieldData
    {
        public string Name = "newField";
        public MemberAttributes Attribute = MemberAttributes.Private;
        public string Type = "string";
    }

    public class ClassData
    {
        public string Name = "NewClass";
        public MemberAttributes Attribute = MemberAttributes.Public;

        [XmlArray("Field")] public FieldData[] Fields;

        [XmlArray("Method")] public MethodData[] Methods;
    }
}