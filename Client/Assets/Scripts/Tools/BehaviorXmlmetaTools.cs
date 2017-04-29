using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System;



public class ExportInfo {
    public Type type;
    public List<string> methodNameList;
    public List<string> propertynamelist;
    public List<string> fieldnamelist;

    public ExportInfo (Type type) {
        methodNameList = new List<string>();
        propertynamelist = new List<string>();
        fieldnamelist = new List<string>();
        this.type = type;
        
    }

    public void AddMethod(string name) {
        methodNameList.Add( name );
    }

    public void AddProperty(string name) {
        propertynamelist.Add( name );
    }

    public void AddField (string name) {
        fieldnamelist.Add( name );
    }
}

public class XMLTypeInfo {
    public ExportInfo exportinfo;
    public static Dictionary<Type, XMLTypeInfo> EcportTypes = new Dictionary<Type, XMLTypeInfo>();
    public string stype;
    public string sNameScpeace;
    public string sBase {
        get {
            return _sBase;
        }
        set {
            if(value == "Object") {
                return;
            }
            if (value == "ValueType")
                return;
            _sBase = value;
        }
    }
    string _sBase;
    public string bIsRefType;
    public string IsCustomized;
    public string IsImplemented;
    public string sDisplayName;
    public string sDesc;
    public static string sFormat = "<struct Type=\"{0}\" Namespace=\"{1}\" Base=\"{2}\" IsRefType=\"{3}\" IsCustomized=\"{4}\" IsImplemented=\"{5}\" DisplayName=\"{6}\" Desc=\"{7}\"> \n {8}</struct>";

    public List<XMLMemberInfo> memberList = new List<XMLMemberInfo>();
    
    public XMLTypeInfo (ExportInfo exportinfo) {
        this.exportinfo = exportinfo;
        EcportTypes.Add( exportinfo.type, this );
    }


    public string tostring () {
        return string.Format( sFormat, stype,sNameScpeace,sBase,bIsRefType,IsCustomized,IsImplemented,sDisplayName,sDesc, GetMemberStrint());
    }

    public string GetMemberStrint () {
        string sRet = "";
        for (int i = 0; i < memberList.Count; i++) {
            sRet += memberList[i].tostring();
        }
        return sRet;
    }

    public void AddMemberInfo (XMLMemberInfo oinfo) {
        memberList.Add( oinfo );
    }

}

public class XMLMemberInfo {
    public string Name;
    public string DisplayName;
    public string Desc;
    public string Type;
    public string TypeFullName;
    public string Class;
    public string Public;
    public static string sFormat = "<Member Name=\"{0}\" DisplayName=\"{1}\" Desc=\"{2}\" Type=\"{3}\" TypeFullName=\"{4}\" Class=\"{5}\" Public=\"{6}\" /> \n";
    public XMLMemberInfo () {

    }

    public string tostring () {
        return string.Format( sFormat, Name, DisplayName, Desc, Type, TypeFullName, Class, Public );
    }

}
//导出工具
public class BehaviorXmlmetaTools : Editor {
    

    static List<ExportInfo> m_ExportClass {
        get {
            if(_ExportClass == null) {
                _ExportClass = new List<ExportInfo>();
                Init();
            }
            return _ExportClass;
        }
    }
    static List<ExportInfo> _ExportClass;

    static public void Init () {
        ExportInfo oComponent = new ExportInfo( typeof( Component ) );
        oComponent.AddProperty( "gameObject" );
        oComponent.AddProperty( "transform" );
        oComponent.AddProperty( "tag" );
        _ExportClass.Add( oComponent );

        ExportInfo oGameObject = new ExportInfo( typeof( GameObject ) );
        oGameObject.AddProperty( "activeInHierarchy" );
        oGameObject.AddProperty( "layer" );
        oGameObject.AddProperty( "transform" );
        oGameObject.AddProperty( "name" );
        _ExportClass.Add( oGameObject );

        ExportInfo oTransforminfo = new ExportInfo( typeof( Transform ) );
        oTransforminfo.AddProperty( "eulerAngles" );
        oTransforminfo.AddProperty( "forward" );
        oTransforminfo.AddProperty( "position" );
        oTransforminfo.AddProperty( "rotation" );
        oTransforminfo.AddProperty( "parent" );
        _ExportClass.Add( oTransforminfo );

        ExportInfo oVector3info = new ExportInfo( typeof( Vector3 ) );
        oVector3info.AddField( "x" );
        oVector3info.AddField( "y" );
        oVector3info.AddField( "z" );
        _ExportClass.Add( oVector3info );
    }
       /// <summary>  
       /// 获取一个命名空间下的所有类  
       /// </summary>  
       /// <param name="name"></param>  
       /// <returns></returns>  
    [MenuItem( "Test/Test" )]
    public static void GetTypes () {
        string sAll = "";
        List<XMLTypeInfo> aList = new List<XMLTypeInfo>();
        foreach (var item in m_ExportClass) {
            XMLTypeInfo otypeinfo = new XMLTypeInfo( item );
            otypeinfo.stype = item.type.FullName.Replace(".","::") ;
            otypeinfo.sNameScpeace = item.type.Assembly.GetName().Name ;
            otypeinfo.sBase = item.type.BaseType.FullName.Replace( ".", "::" );
            otypeinfo.bIsRefType = (!item.type.IsValueType).ToString().ToLower();
            otypeinfo.IsCustomized = true.ToString().ToLower();
            otypeinfo.IsImplemented = true.ToString().ToLower();
            otypeinfo.sDisplayName = item.type.Name;
            otypeinfo.sDesc = item.type.Name;
            aList.Add( otypeinfo );
        }

        foreach (var otypeinfo in aList) {
            ExportInfo oExportinfo = otypeinfo.exportinfo;
            foreach (var propertyname in oExportinfo.propertynamelist) {
                XMLMemberInfo oinfo = new XMLMemberInfo();
                var property = oExportinfo.type.GetProperty( propertyname );
                oinfo.Name = property.Name;
                oinfo.DisplayName = property.Name;
                oinfo.Desc = property.Name;
                oinfo.Type = GetMemberTypeName( property.PropertyType );
                oinfo.TypeFullName = GetMemberFullTypeName( property.PropertyType );
                oinfo.Class = otypeinfo.stype;
                oinfo.Public = property.CanRead.ToString().ToLower();
                otypeinfo.AddMemberInfo( oinfo );
            }

            foreach (var propertyname in oExportinfo.fieldnamelist) {
                XMLMemberInfo oinfo = new XMLMemberInfo();
                var property = oExportinfo.type.GetField( propertyname );
                oinfo.Name = property.Name;
                oinfo.DisplayName = property.Name;
                oinfo.Desc = property.Name;
                oinfo.Type = GetMemberTypeName( property.FieldType );
                oinfo.TypeFullName = GetMemberFullTypeName( property.FieldType );
                oinfo.Class = otypeinfo.stype;
                oinfo.Public = property.IsPublic.ToString().ToLower();
                otypeinfo.AddMemberInfo( oinfo );
            }
            sAll += otypeinfo.tostring();
            
        }
        Debug.LogError( sAll );
    }


    static public string GetMemberTypeName ( Type oSelfType) {
        if (XMLTypeInfo.EcportTypes.ContainsKey( oSelfType )) {
            return XMLTypeInfo.EcportTypes[oSelfType].stype;
        }
        return GetTypeString( oSelfType.Name );
    }

    static public string GetMemberFullTypeName (Type oSelfType) {
        if (XMLTypeInfo.EcportTypes.ContainsKey( oSelfType )) {
            return XMLTypeInfo.EcportTypes[oSelfType].stype;
        }
        return oSelfType.FullName;
    }

    static public string GetTypeString(string sName) {
        switch (sName) {
            case "Int32":
                return "int";
            case "Single":
                return "float";
            case "Boolean":
                return "bool";
            case "String":
                return "string";
            default:
                break;
        }
        return sName;
    }
}
