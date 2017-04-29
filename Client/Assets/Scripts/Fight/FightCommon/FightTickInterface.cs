using System.Collections.Generic;
using Ranger;
public enum eUpdateQueue {
    Area = 0,  //区域
    LevelObj,  //LevelObj
    Sencond,   //玩家和怪物
    Gun,       //枪
    Skill,     //技能
    Object,    //Object
    Buff,      //Buff
    Bullet,    //子弹
    eMax,
}

public enum eChangeType {
    Add = 0,
    Remove,
}
public interface IFightTick {
    void FightTick (float nDeltaTime);

    int GetInstanceID ();
}

public class FightTickChangeInfo {
    public eChangeType m_eChangeType;

    public IFightTick m_oFightTickObj;

    public int m_nInstanceId;
}

public class ChangeInfoFactory:Singleton<ChangeInfoFactory> {
    private Queue<FightTickChangeInfo> m_oChangeInfoQueue = new Queue<FightTickChangeInfo>();
    public FightTickChangeInfo GetChangeInfo () {
        FightTickChangeInfo oOut;
        if (m_oChangeInfoQueue.Count > 0) {
            oOut = m_oChangeInfoQueue.Dequeue();
        }else {
            oOut = new FightTickChangeInfo();
        }
        return oOut;
    }

    public void GiveChangeInfo (FightTickChangeInfo oObject) {
        if (oObject == null)
            return;
        m_oChangeInfoQueue.Enqueue( oObject );
    }
}