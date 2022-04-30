//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;
//using UnityEditor;

//public static class PropertyCopier
//{
//    public static void CreatePropertyClass<T>(this T self)
//    {
//        var t = typeof(T);
//        var strType = $"{typeof(T)}".Replace("UnityEngine.","");
//        string classHeader = $"public class {strType}PropCopier" + "\n{\n";
//        string publicStr = "    public ";
//        string getSettStr = " { get; set; }\n";
//        string body = "";
//        string closing = "}";

//        string ctrHeaderStr = $"    public {strType}PropCopier({typeof(T)} instance)\n" + "   {\n";
//        string ctrRowStart = "        this.";
//        string ctrRowMiddle = " = instance.";
//        string ctrRowEnd = ";\n";
//        string ctrEnd = "    }\n\n";
//        string ctrBody= ctrHeaderStr;


//        string methodHeaderStr = $"\n    public void CopyTo(ref {typeof(T)} other)\n" + "   {\n";
//        string methodBody = methodHeaderStr;
//        string rowStart = "       other.";
//        string rowMiddle = " = this.";
//        string rowEnd = ";\n";

//        var propertyInfos = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

//        Debug.Log("Property Count:" + propertyInfos.Length);

//        foreach (var propInfo in propertyInfos)
//        {
//            if (!propInfo.CanWrite) continue;
//            string propBody = publicStr + propInfo.PropertyType + " " + propInfo.Name + getSettStr;
//            string row = rowStart + propInfo.Name + rowMiddle + propInfo.Name + rowEnd;

//            string ctrRow = ctrRowStart + propInfo.Name + ctrRowMiddle + propInfo.Name + ctrRowEnd;

//            ctrBody += ctrRow;
//            methodBody += row;
//            body += propBody;
//        }

//        ctrBody += ctrEnd;
//        methodBody += " }\n";
//        string classStr = classHeader + body + ctrBody + methodBody + closing;

//        Debug.Log(classStr);

//        var targetPath = Application.dataPath + "/Mesut/Scripts/Test";

//        if (!Directory.Exists(targetPath))
//            Directory.CreateDirectory(targetPath);

//        string scriptName = $"/{strType}PropCopier.cs";
//        targetPath += scriptName;

//        File.WriteAllText(targetPath, classStr);
//        AssetDatabase.Refresh();
//    }
//}
