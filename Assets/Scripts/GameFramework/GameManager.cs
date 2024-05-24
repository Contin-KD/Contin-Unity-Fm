using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Unity.Properties;
using UnityEngine.Rendering.VirtualTexturing;

public class GameManager : MonoBehaviour
{
    [Module(6)]
    public static MessageModule Message { get => TGameFrameWork.Instance.GetModule<MessageModule>(); }
    private bool activing;
    private void Awake()
    {
        if (TGameFrameWork.Instance != null)
        {
            Destroy(gameObject);
        }

        // ��ʾ��Ϸ������
        activing = true;

        //��ת��ɾ��
        DontDestroyOnLoad(gameObject);

        // Application.logMessageReceived += OnReceiveLog;

        // ��ʼ����Ϸ���
        TGameFrameWork.Initialize();

        StartupModules();

        TGameFrameWork.Instance.InitModules();
    }

    private void StartupModules()
    {
        List<ModuleAttribute> moduleAttrs = new List<ModuleAttribute>();
        PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
        // ����һ��BaseGameModule������
        Type baseCompType = typeof(BaseGameModule);
        // ���������õ�����ģ��
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

        moduleAttrs.Sort((a, b) =>
        {
            return a.Priority - b.Priority;
        });

        for (int i = 0; i < moduleAttrs.Count; i++)
        {
            TGameFrameWork.Instance.AddModule(moduleAttrs[i].Module);
        }
    }
    private void Start()
    {
        TGameFrameWork.Instance.StartModules();
        // Procedure.StartProcedure().Coroutine();
    }

    private void Update()
    {
        TGameFrameWork.Instance.Update();
    }

    private void LateUpdate()
    {
        TGameFrameWork.Instance.LateUpdate();
    }

    private void FixedUpdate()
    {
        TGameFrameWork.Instance.FixedUpdate();
    }

    private void OnDestroy()
    {
        // �ű�������ʱ����
        // �ڽű�����ʱactiving�ᱻ��ture��ʾ������
        if (activing)
        {
            //// ȡ����־��
            //Application.logMessageReceived -= OnReceiveLog;
            // ����TGameFrameWorkʵ��
            TGameFrameWork.Instance.Destroy();
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ModuleAttribute : Attribute, IComparable<ModuleAttribute>
    {
        // ���ȼ�
        public int Priority { get; private set; }
        // ģ�����(����Unity�������ڷ�����һЩ�Զ����������ڷ���,�Ա�����ʹ��ģ��)
        public BaseGameModule Module { get; set; }
        public ModuleAttribute(int priority)
        {
            Priority = priority;
        }
        /// <summary>
        /// �Ƚ���
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int CompareTo(ModuleAttribute other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }
    //private void OnReceiveLog(string condition, string stackTrace, LogType type)
    //{

    //}
}
