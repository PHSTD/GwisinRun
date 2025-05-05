using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//FieldOfView 스크립트용 커스텀 에디터
[CustomEditor (typeof (Monster))]
public class FieldOfViewSystemEditor : Editor
{
    void OnSceneGUI()
    {
        Monster fow = (Monster)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.ViewRadius);
    
        Vector3 viewAngleA = fow.DirFromAngle(-fow.ViewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.ViewAngle / 2, false);
    
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.ViewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.ViewRadius);
    
        Handles.color = Color.red;
        if(fow.CurrentTarget != null)
        {
            Handles.DrawLine(fow.transform.position, fow.CurrentTarget.position);
        }
    }

}
