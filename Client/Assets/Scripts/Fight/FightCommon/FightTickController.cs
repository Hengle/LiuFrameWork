using System.Collections.Generic;
using UnityEngine;


public class FightTickController {
    protected Dictionary<int, IFightTick> m_oFightTickList = new Dictionary<int, IFightTick>();
    protected Dictionary<int,FightTickChangeInfo> m_oChangeList = new Dictionary<int, FightTickChangeInfo>();
    public virtual void FightTick (float nDeltaTime) {
        ChangeFightTickList();
        Dictionary<int, IFightTick>.Enumerator oEnumerator = m_oFightTickList.GetEnumerator();
        while (oEnumerator.MoveNext()) {
            oEnumerator.Current.Value.FightTick( nDeltaTime );
        }
        oEnumerator.Dispose();
    }

    public virtual void AddFightTickObj (IFightTick oObj) {
        int nInstanceId = oObj.GetInstanceID();
        FightTickChangeInfo oInfo;
        if (m_oChangeList.TryGetValue(nInstanceId, out oInfo )) {
            m_oChangeList.Remove( nInstanceId );
            ChangeInfoFactory.Instance.GiveChangeInfo( oInfo );
        } else {
            oInfo = ChangeInfoFactory.Instance.GetChangeInfo();
            oInfo.m_eChangeType = eChangeType.Add;
            oInfo.m_oFightTickObj = oObj;
            oInfo.m_nInstanceId = oObj.GetInstanceID();
            m_oChangeList.Add( oInfo.m_nInstanceId, oInfo );
        }
        
    }

    public virtual void RemoveFightTickObj (IFightTick oObj) {
        int nInstanceId = oObj.GetInstanceID();
        FightTickChangeInfo oInfo;
        if (m_oChangeList.TryGetValue( nInstanceId, out oInfo )) {
            m_oChangeList.Remove( nInstanceId );
            ChangeInfoFactory.Instance.GiveChangeInfo( oInfo );
        } else {
            oInfo = ChangeInfoFactory.Instance.GetChangeInfo();
            oInfo.m_eChangeType = eChangeType.Remove;
            oInfo.m_oFightTickObj = oObj;
            oInfo.m_nInstanceId = oObj.GetInstanceID();
            m_oChangeList.Add( oInfo.m_nInstanceId, oInfo );
        }
    }
    

    private void ChangeFightTickList () {
        if (m_oChangeList.Count == 0)
            return;
        Dictionary<int, FightTickChangeInfo>.Enumerator oEnumerator = m_oChangeList.GetEnumerator();
        while (oEnumerator.MoveNext()) {
            FightTickChangeInfo oInfo = oEnumerator.Current.Value;
            if (oInfo.m_eChangeType == eChangeType.Add) {
                ReallyAddFightTickObj( oInfo );
            } else {
                ReallyRemoveFightTickObj( oInfo );
            }
            ChangeInfoFactory.Instance.GiveChangeInfo( oInfo );
        }
        oEnumerator.Dispose();
        //for (int Index = 0; Index < m_oChangeList.Count; Index++) {
        //    FightTickChangeInfo oInfo = m_oChangeList[Index];
        //    if (m_oChangeList[Index].m_eChangeType == eChangeType.Add) {
        //        ReallyAddFightTickObj( oInfo );
        //    } else {
        //        ReallyRemoveFightTickObj( oInfo );
        //    }
        //    ChangeInfoFactory.Instance.GiveChangeInfo( oInfo );
        //}
        m_oChangeList.Clear();
    }

    private void ReallyAddFightTickObj (FightTickChangeInfo oInfo) {
        m_oFightTickList.Add( oInfo.m_nInstanceId, oInfo.m_oFightTickObj );
    }

    private void ReallyRemoveFightTickObj (FightTickChangeInfo oInfo) {
        m_oFightTickList.Remove( oInfo.m_nInstanceId );
    }
}