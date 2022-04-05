﻿using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityCompare
{
    public class CompareInspector : EditorWindow
    {
        [MenuItem("Tools/Compare/CompareInspector")]
        public static CompareInspector GetWindow(CompareInfo info, UnityEngine.Object left, UnityEngine.Object right)
        {
            var window = GetWindow<CompareInspector>();
            window.titleContent = new GUIContent("Compare Inspector");
            window.Focus();
            window.Repaint();

            window.SetInfo(info);
            window.SetObject(left, right);
            return window;
        }

        [SerializeField]
        private UnityEngine.Object m_Left;

        [SerializeField]
        private UnityEngine.Object m_Right;

        [SerializeField]
        private Editor m_LeftEditor;

        [SerializeField]
        private Editor m_RightEditor;

        [SerializeField]
        private Vector2 m_ScrollPosition;

        [SerializeField]
        private string m_UnequalMessage;

        private void SetObject(UnityEngine.Object left, UnityEngine.Object right)
        {
            if(m_Left != left)
            {
                m_Left = left;

                if(m_Left != null)
                {
                    m_LeftEditor = Editor.CreateEditor(m_Left);
                }
                else
                {
                    m_LeftEditor = null;
                }
                
            }

            if (m_Right != right)
            {
                m_Right = right;

                if(m_Right != null)
                {
                    m_RightEditor = Editor.CreateEditor(m_Right);
                }
                else
                {
                    m_RightEditor = null;
                }
            }
        }

        private void SetInfo(CompareInfo info)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("no equal item:");

            builder.AppendLine(info.GetUnequalMessage());

            m_UnequalMessage = builder.ToString();
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();

            OnEditor(m_LeftEditor);

            EditorGUILayout.Separator();

            OnEditor(m_RightEditor);

            GUILayout.EndHorizontal();

            OnUnequalMessage();
        }

        private void OnEditor(Editor editor)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(this.position.width / 2 - 3));

            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);

            if (editor!= null)
            {
                editor.DrawHeader();
                editor.OnInspectorGUI();
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        private void OnUnequalMessage()
        {
            if (!string.IsNullOrWhiteSpace(m_UnequalMessage))
            {
                EditorGUILayout.HelpBox(m_UnequalMessage, MessageType.Error);
            }
        }
    }
}
