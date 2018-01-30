/*******************************************************************************
* 版权声明：北京润尼尔网络科技有限公司，保留所有版权
* 版本声明：v1.0.0
* 项目名称：
* 类 名 称：EventTriggerListenerEditor
* 创建日期：2018-01-16 10:28:19
* 作者名称：王冠南
* CLR 版本：4.0.30319.42000
* 功能描述：
* 修改记录：
* 日期 描述 更新功能
*
******************************************************************************/

//includes for Unity
using UnityEngine;
using UnityEditor;

//includes for System
using System.Collections;
using System;
using MyUnitySDK.Listener;

/// <summary>
///
/// </summary>
[CustomEditor(typeof(EventTriggerListener))]
public class EventTriggerListenerEditor : Editor
{
    /// <summary>
    ///
    /// </summary>
    public override void OnInspectorGUI()
    {
        EventTriggerListener listener = (EventTriggerListener)target;
        base.OnInspectorGUI();

        Labe(listener.onClickLeft, "onClickLeft", () => listener.onClickLeft = null);
        Labe(listener.onClickRight, "onClickRight", () => listener.onClickRight = null);
        Labe(listener.onClickMiddle, "onClickMiddle", () => listener.onClickMiddle = null);
        Labe(listener.onUpLeft, "onUpLeft", () => listener.onUpLeft = null);
        Labe(listener.onUpRight, "onUpRight", () => listener.onUpRight = null);
        Labe(listener.onUpMiddle, "onUpMiddle", () => listener.onUpMiddle = null);
        Labe(listener.onDownLeft, "onDownLeft", () => listener.onDownLeft = null);
        Labe(listener.onDownRight, "onDownRight", () => listener.onDownRight = null);
        Labe(listener.onDownMiddle, "onDownMiddle", () => listener.onDownMiddle = null);
        Labe(listener.onDoubleClickLeft, "onDoubleClickLeft", () => listener.onDoubleClickLeft = null);
        Labe(listener.onDoubleClickRight, "onDoubleClickRight", () => listener.onDoubleClickRight = null);
        Labe(listener.onDoubleClickMiddle, "onDoubleClickMiddle", () => listener.onDoubleClickMiddle = null);
        Labe(listener.onEnter, "onEnter", () => listener.onEnter = null);
        Labe(listener.onExit, "onExit", () => listener.onExit = null);
        Labe(listener.onHover, "onHover", () => listener.onHover = null);
        Labe(listener.onBeginDragLeft, "onBeginDragLeft", () => listener.onBeginDragLeft = null);
        Labe(listener.onBeginDragRight, "onBeginDragRight", () => listener.onBeginDragRight = null);
        Labe(listener.onBeginDragMiddle, "onBeginDragMiddle", () => listener.onBeginDragMiddle = null);
        Labe(listener.onEndDragLeft, "onEndDragLeft", () => listener.onEndDragLeft = null);
        Labe(listener.onEndDragRight, "onEndDragRight", () => listener.onEndDragRight = null);
        Labe(listener.onEndDragMiddle, "onEndDragMiddle", () => listener.onEndDragMiddle = null);
        Labe(listener.onDragLeft, "onDragLeft", () => listener.onDragLeft = null);
        Labe(listener.onDragRight, "onDragRight", () => listener.onDragRight = null);
        Labe(listener.onDragMiddle, "onDragMiddle", () => listener.onDragMiddle = null);
        Labe(listener.onScroll, "onScroll", () => listener.onScroll = null);
        Labe(listener.onDrop, "onDrop", () => listener.onDrop = null);
        Labe(listener.onMove, "onMove", () => listener.onMove = null);
        Labe(listener.onSelect, "onSelect", () => listener.onSelect = null);
        Labe(listener.onDeselect, "onDeselect", () => listener.onDeselect = null);
        Labe(listener.onUpdateselect, "onUpdateselect", () => listener.onUpdateselect = null);
        Labe(listener.onSumit, "onSumit", () => listener.onSumit = null);
        Labe(listener.onCancel, "onCancel", () => listener.onCancel = null);
    }

    private void Labe(EventTriggerListener.VoidDelegate vo, string name, Action clearAction)
    {
        if (vo != null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name);
            if (GUILayout.Button("Clear"))
            {
                clearAction();
                EditorGUILayout.EndHorizontal();
                return;
            }
            EditorGUILayout.EndHorizontal();
            BeginContents();
            for (int i = 0; i < vo.GetInvocationList().Length; i++)
            {
                EditorGUILayout.LabelField(vo.GetInvocationList()[i].Method.Name);
            }
            EndContents();
        }
    }

    private void BeginContents()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(4f);
        EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
        GUILayout.BeginVertical();
        GUILayout.Space(2f);
    }

    /// <summary>
    /// End drawing the content area.
    /// </summary>

    private void EndContents()
    {
        GUILayout.Space(3f);
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(3f);
        GUILayout.EndHorizontal();
        GUILayout.Space(3f);
    }
}