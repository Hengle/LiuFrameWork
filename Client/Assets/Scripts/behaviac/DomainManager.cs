/////////////////////////////////////////////////////////////////////////
///作    者：  Liu
///文件说明:   用于Unity编辑模式开发
///功能说明:   InitializeOnLoad 使该程序能在Unity启动和重编译的时候能够被创建出来，
///            所以我们根据此项写出可以在编辑模式下运行的脚本管理器。所有需要在编辑
///            模式运行的程序，可以继承自 IEditorTool 接口，就可以被管理器通过反射
///            加入进来。
using System.Collections.Generic;
using UnityEditor;
[InitializeOnLoad]
public class EditorToolManager {
    #region Public Attributes
    public enum eEditorState {
        eEditor,
        ePlaying,
        eMax,
    }
    static eEditorState m_eState {
        get {
            return _eState;
        }
        set {
            if (_eState == value) {
                return;
            }
            _eState = value;
            switch (_eState) {
                case eEditorState.eEditor:
                    OnEnterEditor();
                    break;
                case eEditorState.ePlaying:
                    OnEnterPlaying();
                    break;
                default:
                    break;
            }
        }
    }
    static eEditorState _eState = eEditorState.eMax;
    #endregion
    #region Private Attributes
    private static List<IEditorTool> m_crows = new List<IEditorTool>();
    #endregion
    #region Public Methods
    static EditorToolManager () {
        EditorApplication.update = Update;
        if (m_eState == eEditorState.eMax) {
            EnterUnity();
        }
    }
    ~EditorToolManager () {
        //Debug.LogError( "Stop Unity" );
    }
    static void OnEnterEditor () {
        for (int Index = 0; Index < m_crows.Count; Index++) {
            m_crows[Index].EnterEditor();
        }
        //Debug.LogError( "Enter Editor" );
    }
    static void OnEnterPlaying () {
        for (int Index = 0; Index < m_crows.Count; Index++) {
            m_crows[Index].EnterPlaying();
        }
        //Debug.LogError( "Enter Playing" );
    }
    static void EnterUnity () {
        m_crows = new List<IEditorTool>( GetAllImplementTypes<IEditorTool>( System.AppDomain.CurrentDomain ) );
        for (int Index = 0; Index < m_crows.Count; Index++) {
            m_crows[Index].EnterUnity();
        }
        //Debug.LogError( "Enter Unity" );
    }
    static void Update () {
        switch (m_eState) {
            case eEditorState.eEditor:
                for (int Index = 0; Index < m_crows.Count; Index++) {
                    m_crows[Index].UpdateEditor();
                }
                break;
            case eEditorState.ePlaying:
                for (int Index = 0; Index < m_crows.Count; Index++) {
                    m_crows[Index].UpdatePlaying();
                }
                break;
            default:
                break;
        }
        if (EditorApplication.isPlaying) {
            m_eState = eEditorState.ePlaying;
        } else {
            m_eState = eEditorState.eEditor;
        }
    }
    #endregion
    //可以使用工具类
    public static T[] GetAllImplementTypes<T> (System.AppDomain aAppDomain) where T : class {
        var result = new List<T>();
        var assemblies = aAppDomain.GetAssemblies();
        foreach (var assembly in assemblies) {
            var types = assembly.GetTypes();
            foreach (var type in types) {
                if (typeof( T ).IsAssignableFrom( type )) {
                    if (!type.IsAbstract) {
                        var tar = assembly.CreateInstance( type.FullName ) as T;
                        result.Add( tar );
                    }
                }
            }
        }
        return result.ToArray();
    }
}
public interface IEditorTool {
    IEditorTool GetInstance ();
    void EnterUnity ();
    void UpdateEditor ();
    void EnterEditor ();
    void UpdatePlaying ();
    void EnterPlaying ();
    void ExitUnity ();
}