using Config;
using Koakuma.Game.UI;
using QFSW.QC;
using System;
using System.Collections;
using System.Collections.Generic;
using TGame.Asset;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

namespace TGame.UI
{
    // ����һ�� UIModule �࣬�̳��� BaseGameModule
    public partial class UIModule : BaseGameModule
    {
        public Transform normalUIRoot; // ��ͨ UI �ĸ��ڵ�
        public Transform modalUIRoot;  // ģ̬ UI �ĸ��ڵ�
        public Transform closeUIRoot;  // �ر�״̬�� UI ���ڵ�
        public Image imgMask;          // UI ���ɰ�ͼ��
        public QuantumConsole prefabQuantumConsole; // ���ӿ���̨��Ԥ�Ƽ�

        // �ֵ䣬���ڴ洢 UIViewID ���Ӧ Mediator �� Asset ���͵�ӳ��
        private static Dictionary<UIViewID, Type> MEDIATOR_MAPPING;
        private static Dictionary<UIViewID, Type> ASSET_MAPPING;

        // �б����ڴ洢��ǰ����ʹ�õ� Mediator ʵ��
        private readonly List<UIMediator> usingMediators = new List<UIMediator>();
        // �ֵ䣬���ڴ洢���е� Mediator ʵ����������Ϊ�����з���
        private readonly Dictionary<Type, Queue<UIMediator>> freeMediators = new Dictionary<Type, Queue<UIMediator>>();

        // ����һ�� UI �����
        private readonly GameObjectPool<GameObjectAsset> uiObjectPool = new GameObjectPool<GameObjectAsset>();
        // �洢 Quantum Console ʵ��������
        private QuantumConsole quantumConsole;

        // ģ���ʼ������
        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
            // ʵ���������丸�ڵ�����Ϊ��ǰ����
            quantumConsole = Instantiate(prefabQuantumConsole);
            quantumConsole.transform.SetParentAndResetAll(transform);
            // �������ӿ���̨�ļ����ͣ���¼�
            quantumConsole.OnActivate += OnConsoleActive;
            quantumConsole.OnDeactivate += OnConsoleDeactive;
        }

        // ģ��ֹͣ����
        protected internal override void OnModuleStop()
        {
            base.OnModuleStop();
            // ȡ���������ӿ���̨�ļ����ͣ���¼�
            quantumConsole.OnActivate -= OnConsoleActive;
            quantumConsole.OnDeactivate -= OnConsoleDeactive;
        }

        // ��̬���������ڻ��� UIViewID �� Mediator �� Asset ���͵�ӳ��
        private static void CacheUIMapping()
        {
            // ���ӳ���Ѿ����ڣ���ֱ�ӷ���
            if (MEDIATOR_MAPPING != null)
                return;

            // ��ʼ��ӳ���ֵ�
            MEDIATOR_MAPPING = new Dictionary<UIViewID, Type>();
            ASSET_MAPPING = new Dictionary<UIViewID, Type>();

            // ��ȡ UIView �������
            Type baseViewType = typeof(UIView);
            // ���������е���������
            foreach (var type in baseViewType.Assembly.GetTypes())
            {
                // ������������
                if (type.IsAbstract)
                    continue;

                // ����������� UIView ������
                if (baseViewType.IsAssignableFrom(type))
                {
                    // ��ȡ�������ϵ� UIViewAttribute ���� 
                    object[] attrs = type.GetCustomAttributes(typeof(UIViewAttribute), false);
                    // ���û���ҵ����ԣ���������沢����
                    if (attrs.Length == 0)
                    {
                        Debug.Log($"{type.FullName} û�а� Mediator����ʹ��UIMediatorAttribute��һ��Mediator����ȷʹ��");
                        continue;
                    }

                    // �������е� ID �� MediatorType ��ӵ�ӳ���ֵ���
                    foreach (UIViewAttribute attr in attrs)
                    {
                        MEDIATOR_MAPPING.Add(attr.ID, attr.MediatorType);
                        ASSET_MAPPING.Add(attr.ID, type);
                        break;
                    }
                }
            }
        }

        // ģ����·�����ÿ֡����һ��
        protected internal override void OnModuleUpdate(float deltaTime)
        {
            base.OnModuleUpdate(deltaTime);
            // ���� UI ����صļ�������
            uiObjectPool.UpdateLoadRequests();
            // ������������ʹ�õ� Mediator
            foreach (var mediator in usingMediators)
            {
                mediator.Update(deltaTime);
            }
            // �����ɰ�ͼ���͸����
            UpdateMask(deltaTime);
        }

        // ���ӿ���̨����ʱ�Ļص�����
        private void OnConsoleActive()
        {
            // ���������ӵ����ӿ���̨����ʱ���߼�
            // ���������Ϸ�е�����
        }

        // ���ӿ���̨ͣ��ʱ�Ļص�����
        private void OnConsoleDeactive()
        {
            // ���������ӵ����ӿ���̨ͣ��ʱ���߼�
            // ��������������Ϸ�е�����
        }

        // ��ȡָ��ģʽ�µ��������
        private int GetTopMediatorSortingOrder(UIMode mode)
        {
            // ��ʼ������
            int lastIndexMediatorOfMode = -1;
            // �Ӻ���ǰ��������ʹ�õ� Mediator �б�
            for (int i = usingMediators.Count - 1; i >= 0; i--)
            {
                UIMediator mediator = usingMediators[i];
                // ���ģʽ��ƥ�䣬������
                if (mediator.UIMode != mode)
                    continue;

                // ��¼���һ��ƥ��� Mediator ������
                lastIndexMediatorOfMode = i;
                break;
            }

            // ���û���ҵ�ƥ��� Mediator���򷵻�Ĭ������ֵ
            if (lastIndexMediatorOfMode == -1)
                return mode == UIMode.Normal ? 0 : 1000;

            // �����ҵ��� Mediator ������ֵ
            return usingMediators[lastIndexMediatorOfMode].SortingOrder;
        }

        // ��ȡָ�� UIViewID �� Mediator
        private UIMediator GetMediator(UIViewID id)
        {
            // ���� UI ӳ��
            CacheUIMapping();

            // ���Դ�ӳ���л�ȡ��Ӧ�� Mediator ����
            if (!MEDIATOR_MAPPING.TryGetValue(id, out Type mediatorType))
            {
                Debug.Log($"�Ҳ��� {id} ��Ӧ��Mediator");
                return null;
            }

            // ���Դӿ��е� Mediator �ֵ��л�ȡ��Ӧ���͵Ķ���
            if (!freeMediators.TryGetValue(mediatorType, out Queue<UIMediator> mediatorQ))
            {
                mediatorQ = new Queue<UIMediator>();
                freeMediators.Add(mediatorType, mediatorQ);
            }

            // �Ӷ����л�ȡһ�� Mediator ʵ�����������Ϊ���򴴽�һ���µ�ʵ��
            UIMediator mediator;
            if (mediatorQ.Count == 0)
            {
                mediator = Activator.CreateInstance(mediatorType) as UIMediator;
            }
            else
            {
                mediator = mediatorQ.Dequeue();
            }

            return mediator;
        }

        // ���� Mediator
        private void RecycleMediator(UIMediator mediator)
        {
            // ��� Mediator Ϊ null����ֱ�ӷ���
            if (mediator == null)
                return;

            // ��ȡ Mediator ������
            Type mediatorType = mediator.GetType();
            // ���Դӿ��е� Mediator �ֵ��л�ȡ��Ӧ���͵Ķ���
            if (!freeMediators.TryGetValue(mediatorType, out Queue<UIMediator> mediatorQ))
            {
                mediatorQ = new Queue<UIMediator>();
                freeMediators.Add(mediatorType, mediatorQ);
            }
            // �� Mediator �Żض�����
            mediatorQ.Enqueue(mediator);
        }
        // ��ȡ��ǰ���ڴ򿪵� UIMediator
        public UIMediator GetOpeningUIMediator(UIViewID id)
        {
            // ���� id ��ȡ UI ������Ϣ
            UIConfig uiConfig = UIConfig.ByID((int)id);
            if (uiConfig.IsNull)
                return null;

            // ��ȡ��Ӧ�� Mediator
            UIMediator mediator = GetMediator(id);
            if (mediator == null)
                return null;

            // ��ȡ Mediator ������
            Type requiredMediatorType = mediator.GetType();
            // ������������ʹ�õ� Mediator
            foreach (var item in usingMediators)
            {
                // ����ҵ�ͬ���͵� Mediator���򷵻ظ� Mediator
                if (item.GetType() == requiredMediatorType)
                    return item;
            }
            return null;
        }

        // ��ָ���� UI �ᵽ��ǰ��
        public void BringToTop(UIViewID id)
        {
            // ��ȡ��ǰ���ڴ򿪵� UIMediator
            UIMediator mediator = GetOpeningUIMediator(id);
            if (mediator == null)
                return;

            // ��ȡָ��ģʽ�µ��������ֵ
            int topSortingOrder = GetTopMediatorSortingOrder(mediator.UIMode);
            if (mediator.SortingOrder == topSortingOrder)
                return;

            // ���� Mediator ������ֵ
            int sortingOrder = topSortingOrder + 10;
            mediator.SortingOrder = sortingOrder;

            // �� Mediator �Ƶ��б�ĩβ
            usingMediators.Remove(mediator);
            usingMediators.Add(mediator);

            // ���� Canvas ������ֵ
            Canvas canvas = mediator.ViewObject.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.sortingOrder = sortingOrder;
            }
        }

        // �ж�ָ���� UI �Ƿ��Ѵ�
        public bool IsUIOpened(UIViewID id)
        {
            return GetOpeningUIMediator(id) != null;
        }

        // �򿪵��� UI������Ѿ�����ֱ�ӷ���
        public UIMediator OpenUISingle(UIViewID id, object arg = null)
        {
            // ��ȡ��ǰ���ڴ򿪵� UIMediator
            UIMediator mediator = GetOpeningUIMediator(id);
            if (mediator != null)
                return mediator;

            // �� UI
            return OpenUI(id, arg);
        }

        // �� UI
        public UIMediator OpenUI(UIViewID id, object arg = null)
        {
            // ���� id ��ȡ UI ������Ϣ
            UIConfig uiConfig = UIConfig.ByID((int)id);
            if (uiConfig.IsNull)
                return null;

            // ��ȡ��Ӧ�� Mediator
            UIMediator mediator = GetMediator(id);
            if (mediator == null)
                return null;

            // ���� UI ��Դ
            GameObject uiObject = (uiObjectPool.LoadGameObject(uiConfig.Asset, (obj) =>
            {
                UIView newView = obj.GetComponent<UIView>();
                mediator.InitMediator(newView);
            })).gameObject;

            // �� UI ��Դ������ɺ���
            return OnUIObjectLoaded(mediator, uiConfig, uiObject, arg);
        }

        // �첽�򿪵��� UI
        public IEnumerator OpenUISingleAsync(UIViewID id, object arg = null)
        {
            // ��� UI û�д򿪣����첽��
            if (!IsUIOpened(id))
            {
                yield return OpenUIAsync(id, arg);
            }
        }

        // �첽�� UI
        public IEnumerator OpenUIAsync(UIViewID id, object arg = null)
        {
            // ���� id ��ȡ UI ������Ϣ
            UIConfig uiConfig = UIConfig.ByID((int)id);
            if (uiConfig.IsNull)
                yield break;

            // ��ȡ��Ӧ�� Mediator
            UIMediator mediator = GetMediator(id);
            if (mediator == null)
                yield break;

            // �첽���� UI ��Դ
            bool loadFinish = false;
            uiObjectPool.LoadGameObjectAsync(uiConfig.Asset, (asset) =>
            {
                GameObject uiObject = asset.gameObject;
                OnUIObjectLoaded(mediator, uiConfig, uiObject, arg);
                loadFinish = true;
            }, (obj) =>
            {
                UIView newView = obj.GetComponent<UIView>();
                mediator.InitMediator(newView);
            });

            // �ȴ��������
            while (!loadFinish)
            {
                yield return null;
            }
            yield return null;
            yield return null;
        }

        // UI ��Դ������ɺ�Ĵ���
        private UIMediator OnUIObjectLoaded(UIMediator mediator, UIConfig uiConfig, GameObject uiObject, object obj)
        {
            if (uiObject == null)
            {
                Debug.Log($"����UIʧ��:{uiConfig.Asset}");
                RecycleMediator(mediator);
                return null;
            }

            // ��ȡ UI ��ͼ���
            UIView view = uiObject.GetComponent<UIView>();
            if (view == null)
            {
                Debug.Log($"UI Prefab������UIView�ű�:{uiConfig.Asset}");
                RecycleMediator(mediator);
                uiObjectPool.UnloadGameObject(view.gameObject);
                return null;
            }

            // ���� Mediator ��ģʽ������ֵ
            mediator.UIMode = uiConfig.Mode;
            int sortingOrder = GetTopMediatorSortingOrder(uiConfig.Mode) + 10;

            // �� Mediator ��ӵ�ʹ���е��б�
            usingMediators.Add(mediator);

            // ���� Canvas ������
            Canvas canvas = uiObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            //canvas.worldCamera = GameManager.Camera.uiCamera;
            if (uiConfig.Mode == UIMode.Normal)
            {
                uiObject.transform.SetParentAndResetAll(normalUIRoot);
                canvas.sortingLayerName = "NormalUI";
            }
            else
            {
                uiObject.transform.SetParentAndResetAll(modalUIRoot);
                canvas.sortingLayerName = "ModalUI";
            }

            // ��������ֵ
            mediator.SortingOrder = sortingOrder;
            canvas.sortingOrder = sortingOrder;

            // ���� UI ����ʾ
            uiObject.SetActive(true);
            mediator.Show(uiObject, obj);
            return mediator;
        }

        // �ر� UI
        public void CloseUI(UIMediator mediator)
        {
            if (mediator != null)
            {
                // ���� UI ��ͼ����
                uiObjectPool.UnloadGameObject(mediator.ViewObject);
                mediator.ViewObject.transform.SetParentAndResetAll(closeUIRoot);

                // ���� Mediator
                mediator.Hide();
                RecycleMediator(mediator);

                // ��ʹ���е��б����Ƴ�
                usingMediators.Remove(mediator);
            }
        }

        // �ر����� UI
        public void CloseAllUI()
        {
            for (int i = usingMediators.Count - 1; i >= 0; i--)
            {
                CloseUI(usingMediators[i]);
            }
        }

        // ���� ID �ر� UI
        public void CloseUI(UIViewID id)
        {
            UIMediator mediator = GetOpeningUIMediator(id);
            if (mediator == null)
                return;

            CloseUI(mediator);
        }

        // ����������ͨ UI �Ŀɼ���
        public void SetAllNormalUIVisibility(bool visible)
        {
            normalUIRoot.gameObject.SetActive(visible);
        }

        // ��������ģ̬ UI �Ŀɼ���
        public void SetAllModalUIVisibility(bool visible)
        {
            modalUIRoot.gameObject.SetActive(visible);
        }

        // ��ʾ�ɰ�
        public void ShowMask(float duration = 0.5f)
        {
            destMaskAlpha = 1;
            maskDuration = duration;
        }

        // �����ɰ�
        public void HideMask(float? duration = null)
        {
            destMaskAlpha = 0;
            if (duration.HasValue)
            {
                maskDuration = duration.Value;
            }
        }

        // �����ɰ��͸����
        private float destMaskAlpha = 0;
        private float maskDuration = 0;
        private void UpdateMask(float deltaTime)
        {
            Color c = imgMask.color;
            c.a = maskDuration > 0 ? Mathf.MoveTowards(c.a, destMaskAlpha, 1f / maskDuration * deltaTime) : destMaskAlpha;
            c.a = Mathf.Clamp01(c.a);
            imgMask.color = c;
            imgMask.enabled = imgMask.color.a > 0;
        }

        // ��ʾ���ӿ���̨
        public void ShowConsole()
        {
            quantumConsole.Activate();
        }
    }
}

// �Զ������ԣ����ڱ�� UIView ����
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed class UIViewAttribute : Attribute
{
    // UIViewID �� Mediator ����
    public UIViewID ID { get; }
    public Type MediatorType { get; }

    // ���캯��
    public UIViewAttribute(Type mediatorType, UIViewID id)
    {
        ID = id;
        MediatorType = mediatorType;
    }
}
