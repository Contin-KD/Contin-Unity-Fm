using System;
using System.Collections.Generic;
using UnityEngine; // ����ʹ�� UnityEngine �����ռ���������־��¼��ʱ�亯��

public class TGameFrameWork
{
    // TGameFrameWork �ĵ���ʵ��
    public static TGameFrameWork Instance { get; private set; }

    // ָʾ����Ƿ��ѳ�ʼ��
    public static bool Initialized { get; private set; }

    // �洢��Ϸģ����ֵ䣬��ģ��������Ϊ��
    public Dictionary<Type, BaseGameModule> modules = new Dictionary<Type, BaseGameModule>();

    // ��ʼ�����ʵ��
    public static void Initialize()
    {
        Instance = new TGameFrameWork();
    }

    // ��ʼ��������ע���ģ��
    internal void InitModules()
    {
        // ��ֹ�ظ���ʼ��
        if (Initialized)
        {
            return;
        }

        // ���Ϊ�ѳ�ʼ��
        Initialized = true;

        // ��������ģ�鲢�������ǵ� OnModuleInit ����
        foreach (var item in modules.Values)
        {
            item.OnModuleInit();
        }
    }

    /// <summary>
    /// ��ȡ����Ϊ T ��ģ�飨������ڣ���
    /// </summary>
    /// <typeparam name="T">Ҫ��ȡ��ģ������</typeparam>
    /// <returns>����ҵ��򷵻�ģ��ʵ�������򷵻� T ��Ĭ��ֵ</returns>
    public T GetModule<T>() where T : BaseGameModule
    {
        if (modules.TryGetValue(typeof(T), out BaseGameModule module))
        {
            return module as T;
        }

        return default(T);
    }

    /// <summary>
    /// ���ģ�鵽��ܽ���ͳһ����
    /// </summary>
    /// <param name="module">Ҫ��ӵ�ģ��ʵ��</param>
    internal void AddModule(BaseGameModule module)
    {
        Type moduleType = module.GetType();

        // ��ֹ����ظ���ģ��
        if (modules.ContainsKey(moduleType))
        {
            UnityEngine.Debug.Log($"ģ�����ʧ�ܣ��ظ����: {moduleType.Name}");
            return;
        }

        // ��ģ����ӵ��ֵ�
        modules.Add(moduleType, module);
    }

    // ��������ģ��
    public void StartModules()
    {
        // ���û��ģ���δ��ʼ���򷵻�
        if (modules == null || !Initialized)
            return;

        // ��������ģ�鲢�������ǵ� OnModuleStart ����
        foreach (var module in modules.Values)
        {
            module.OnModuleStart();
        }
    }

    // ��������ģ��
    public void Update()
    {
        // ���û��ģ���δ��ʼ���򷵻�
        if (modules == null || !Initialized)
            return;

        // ��ȡ��ǰ֡��ʱ��
        float deltaTime = UnityEngine.Time.deltaTime;

        // ��������ģ�鲢�������ǵ� OnModuleUpdate ����
        foreach (var module in modules.Values)
        {
            module.OnModuleUpdate(deltaTime);
        }
    }

    // LateUpdate ����ģ��
    public void LateUpdate()
    {
        // ���û��ģ���δ��ʼ���򷵻�
        if (modules == null || !Initialized)
            return;

        // ��ȡ��ǰ֡��ʱ��
        float deltaTime = UnityEngine.Time.deltaTime;

        // ��������ģ�鲢�������ǵ� OnModuleLateUpdate ����
        foreach (var module in modules.Values)
        {
            module.OnModuleLateUpdate(deltaTime);
        }
    }

    // FixedUpdate ����ģ��
    public void FixedUpdate()
    {
        // ���û��ģ���δ��ʼ���򷵻�
        if (modules == null || !Initialized)
            return;

        // ��ȡfixupdataÿ֡ʱ��
        float deltaTime = UnityEngine.Time.fixedDeltaTime;

        // ��������ģ�鲢�������ǵ� OnModuleFixedUpdate ����
        foreach (var module in modules.Values)
        {
            module.OnModuleFixedUpdate(deltaTime);
        }
    }

    // ���ٿ�ܲ�ֹͣ����ģ��
    public void Destroy()
    {
        // ���δ��ʼ���򷵻�
        if (!Initialized)
            return;

        // ���ʵ�����ǵ�ǰ�����򷵻�
        if (Instance != this)
            return;

        // ���û��ģ���򷵻�
        if (Instance.modules == null)
            return;

        // ��������ģ�鲢�������ǵ� OnModuleStop ����
        foreach (var module in Instance.modules.Values)
        {
            module.OnModuleStop();
        }

        // ����ʵ��
        Instance = null;
        Initialized = false;
    }
}
