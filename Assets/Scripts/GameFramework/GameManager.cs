using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Unity.Properties;
using UnityEngine.Rendering.VirtualTexturing;
using TGame.Procedure;
using OpenCover.Framework.Model;
using TGame.UI;
using TGame.Asset;
using TGame.ECS;

namespace Koakuma.Game
{
    public class GameManager : MonoBehaviour
    {
        // ʹ��ModuleAttributeΪ��ͬģ���ṩ���ȼ�
        [Module(1)]
        public static AssetModule Asset { get => TGameFrameWork.Instance.GetModule<AssetModule>(); }
        [Module(2)]
        public static ProcedureModule Procedure { get => TGameFrameWork.Instance.GetModule<ProcedureModule>(); }
        [Module(3)]
        public static UIModule UI { get => TGameFrameWork.Instance.GetModule<UIModule>(); }

        [Module(4)]
        public static MessageModule Message { get => TGameFrameWork.Instance.GetModule<MessageModule>(); }

        [Module(5)]
        public static ECSModule ECS { get => TGameFrameWork.Instance.GetModule<ECSModule>(); }

        private bool activing;

        private void Awake()
        {
            // ���TGameFrameWorkʵ���Ƿ���ڣ�������������ٵ�ǰ��Ϸ����
            if (TGameFrameWork.Instance != null)
            {
                Destroy(gameObject);
            }

            // ��ʾ��Ϸ������
            activing = true;

            // ���ö����ڳ����л�ʱ��������
            DontDestroyOnLoad(gameObject);

            // ��ʼ����Ϸ���
            TGameFrameWork.Initialize();

            // ��������ģ��
            StartupModules();

            // ��ʼ������ģ��
            TGameFrameWork.Instance.InitModules();
        }

        private void StartupModules()
        {
            // ����һ�����ģ�����Ե��б�
            List<ModuleAttribute> moduleAttrs = new List<ModuleAttribute>();
            // ͨ�������ȡ���о�̬�ͷǹ���������
            PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            // ����һ��BaseGameModule������
            Type baseCompType = typeof(BaseGameModule);

            // ������������
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo property = propertyInfos[i];
                // ��������Ƿ���BaseGameModule�������BaseGameModule������
                if (!baseCompType.IsAssignableFrom(property.PropertyType))
                {
                    continue;
                }

                // ��ȡ���е�ModuleAttribute����
                object[] attrs = property.GetCustomAttributes(typeof(ModuleAttribute), false);
                if (attrs.Length <= 0)
                {
                    continue;
                }

                // ��ȡ���ö�����������л�ȡ�����
                Component comp = GetComponentInChildren(property.PropertyType);
                if (comp == null)
                {
                    Debug.Log("�Ҳ���Module" + property.PropertyType);
                    continue;
                }

                ModuleAttribute moduleAttribute = attrs[0] as ModuleAttribute;
                // ���Ը�ģ��������һ��
                moduleAttribute.Module = comp as BaseGameModule;
                moduleAttrs.Add(moduleAttribute);
            }

            // �����ȼ���ģ�����Խ�������
            moduleAttrs.Sort((a, b) =>
            {
                return a.Priority - b.Priority;
            });

            // ���ģ�鵽TGameFrameWorkʵ����
            for (int i = 0; i < moduleAttrs.Count; i++)
            {
                TGameFrameWork.Instance.AddModule(moduleAttrs[i].Module);
            }
        }

        private void Start()
        {
            // ��������ģ��
            TGameFrameWork.Instance.StartModules();
            // ��ʼ��������
            Procedure.StartProcedure().Coroutine();
        }

        private void Update()
        {
            // ÿ֡����TGameFrameWorkʵ��
            TGameFrameWork.Instance.Update();
        }

        private void LateUpdate()
        {
            // ÿ֡��Щʱ�����TGameFrameWorkʵ��
            TGameFrameWork.Instance.LateUpdate();
        }

        private void FixedUpdate()
        {
            // ÿ�̶�֡�ʸ���TGameFrameWorkʵ��
            TGameFrameWork.Instance.FixedUpdate();
        }

        private void OnDestroy()
        {
            // �ű�������ʱ����
            if (activing)
            {
                // ����TGameFrameWorkʵ��
                TGameFrameWork.Instance.Destroy();
            }
        }

        // �Զ���ModuleAttribute�࣬����Ϊģ���ṩ���ȼ�
        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        public sealed class ModuleAttribute : Attribute, IComparable<ModuleAttribute>
        {
            // ���ȼ�
            public int Priority { get; private set; }
            // ģ�����
            public BaseGameModule Module { get; set; }

            public ModuleAttribute(int priority)
            {
                Priority = priority;
            }

            // ʵ��IComparable�ӿڣ�����ģ����������
            public int CompareTo(ModuleAttribute other)
            {
                return Priority.CompareTo(other.Priority);
            }
        }
    }
}
