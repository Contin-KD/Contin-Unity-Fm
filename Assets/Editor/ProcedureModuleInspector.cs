using System;
using System.Collections.Generic;
using TGame.Common;
using TGame.Procedure;
using UnityEditor;
using UnityEngine;

namespace TGame.Editor.Inspector
{
    [CustomEditor(typeof(ProcedureModule))]
    public class ProcedureModuleInspector : BaseInspector
    {
        // 用于访问和修改程序模块的流程名称数组
        private SerializedProperty proceduresProperty;
        // 用于访问和修改默认流程名称
        private SerializedProperty defaultProcedureProperty;
        // 所有流程类型的列表
        private List<string> allProcedureTypes;

        /// <summary>
        /// 启用Inspector调用
        /// </summary>
        protected override void OnInspectorEnable()
        {
            base.OnInspectorEnable();
            // 获取属性
            proceduresProperty = serializedObject.FindProperty("proceduresNames");
            defaultProcedureProperty = serializedObject.FindProperty("defaultProcedureName");

            UpdateProcedures();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();
            UpdateProcedures();
        }
        // 更新所有流程类型
        private void UpdateProcedures()
        {
            // 通过反射获得所有继承BaseProcedure类型, 添加到allProcedureTypes中     类型, 是否是抽象类 程序集  将每个Type对象转换为它的全名字符串
            allProcedureTypes = Utility.Types.GetAllSubclasses(typeof(BaseProcedure), false, Utility.Types.GAME_CSHARP_ASSEMBLY).ConvertAll((Type t) => { return t.FullName; });

            //移除不存在的procedure
            for (int i = proceduresProperty.arraySize - 1; i >= 0; i--)
            {
                string procedureTypeName = proceduresProperty.GetArrayElementAtIndex(i).stringValue;
                if (!allProcedureTypes.Contains(procedureTypeName))
                {
                    proceduresProperty.DeleteArrayElementAtIndex(i);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            {
                if (allProcedureTypes.Count > 0)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        for (int i = 0; i < allProcedureTypes.Count; i++)
                        {
                            GUI.changed = false;
                            // 查找当前流程类型在 proceduresProperty 数组中的索引
                            int? index = FindProcedureTypeIndex(allProcedureTypes[i]);
                            // 创建一个 ToggleLeft 控件，显示流程类型名称和选择状态
                            bool selected = EditorGUILayout.ToggleLeft(allProcedureTypes[i], index.HasValue);
                            if (GUI.changed)
                            {
                                if (selected)
                                {
                                    // 如果选中，将流程类型添加到 proceduresProperty 数组中
                                    AddProcedure(allProcedureTypes[i]);
                                }
                                else
                                {
                                    // 如果取消选中，从 proceduresProperty 数组中移除流程类型
                                    RemoveProcedure(index.Value);
                                }
                            }
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUI.EndDisabledGroup();

            // 如果没有选中的流程类型，显示提示信息
            if (proceduresProperty.arraySize == 0)
            {
                if (allProcedureTypes.Count == 0)
                {
                    EditorGUILayout.HelpBox("Can't find any procedure", UnityEditor.MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("Please select a procedure at least", UnityEditor.MessageType.Info);
                }
            }
            else
            {
                if (Application.isPlaying)
                {
                    //播放中显示当前状态
                    EditorGUILayout.LabelField("Current Procedure", TGameFrameWork.Instance.GetModule<ProcedureModule>().CurrentProcedure?.GetType().FullName);
                }
                else
                {
                    //显示默认状态
                    List<string> selectedProcedures = new List<string>();
                    for (int i = 0; i < proceduresProperty.arraySize; i++)
                    {
                        selectedProcedures.Add(proceduresProperty.GetArrayElementAtIndex(i).stringValue);
                    }
                    selectedProcedures.Sort();
                    int defaultProcedureIndex = selectedProcedures.IndexOf(defaultProcedureProperty.stringValue);
                    defaultProcedureIndex = EditorGUILayout.Popup("Default Procedure", defaultProcedureIndex, selectedProcedures.ToArray());
                    if (defaultProcedureIndex >= 0)
                    {
                        defaultProcedureProperty.stringValue = selectedProcedures[defaultProcedureIndex];
                    }
                }
            }
            // 应用所有修改
            serializedObject.ApplyModifiedProperties();
        }

        private void AddProcedure(string procedureType)
        {
            proceduresProperty.InsertArrayElementAtIndex(0);
            proceduresProperty.GetArrayElementAtIndex(0).stringValue = procedureType;
        }

        private void RemoveProcedure(int index)
        {
            string procedureType = proceduresProperty.GetArrayElementAtIndex(index).stringValue;
            if (procedureType == defaultProcedureProperty.stringValue)
            {
                Debug.LogWarning("Can't remove default procedure");
                return;
            }
            proceduresProperty.DeleteArrayElementAtIndex(index);
        }

        private int? FindProcedureTypeIndex(string procedureType)
        {
            for (int i = 0; i < proceduresProperty.arraySize; i++)
            {
                SerializedProperty p = proceduresProperty.GetArrayElementAtIndex(i);
                if (p.stringValue == procedureType)
                {
                    return i;
                }
            }
            return null;
        }
    }
}