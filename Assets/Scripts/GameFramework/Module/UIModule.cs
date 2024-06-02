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
    // 定义一个 UIModule 类，继承自 BaseGameModule
    public partial class UIModule : BaseGameModule
    {
        public Transform normalUIRoot; // 普通 UI 的根节点
        public Transform modalUIRoot;  // 模态 UI 的根节点
        public Transform closeUIRoot;  // 关闭状态的 UI 根节点
        public Image imgMask;          // UI 的蒙版图像
        public QuantumConsole prefabQuantumConsole; // 量子控制台的预制件

        // 字典，用于存储 UIViewID 与对应 Mediator 和 Asset 类型的映射
        private static Dictionary<UIViewID, Type> MEDIATOR_MAPPING;
        private static Dictionary<UIViewID, Type> ASSET_MAPPING;

        // 列表，用于存储当前正在使用的 Mediator 实例
        private readonly List<UIMediator> usingMediators = new List<UIMediator>();
        // 字典，用于存储空闲的 Mediator 实例，以类型为键进行分组
        private readonly Dictionary<Type, Queue<UIMediator>> freeMediators = new Dictionary<Type, Queue<UIMediator>>();

        // 定义一个 UI 对象池
        private readonly GameObjectPool<GameObjectAsset> uiObjectPool = new GameObjectPool<GameObjectAsset>();
        // 存储 Quantum Console 实例的引用
        private QuantumConsole quantumConsole;

        // 模块初始化方法
        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
            // 实例化并将其父节点设置为当前对象
            quantumConsole = Instantiate(prefabQuantumConsole);
            quantumConsole.transform.SetParentAndResetAll(transform);
            // 订阅量子控制台的激活和停用事件
            quantumConsole.OnActivate += OnConsoleActive;
            quantumConsole.OnDeactivate += OnConsoleDeactive;
        }

        // 模块停止方法
        protected internal override void OnModuleStop()
        {
            base.OnModuleStop();
            // 取消订阅量子控制台的激活和停用事件
            quantumConsole.OnActivate -= OnConsoleActive;
            quantumConsole.OnDeactivate -= OnConsoleDeactive;
        }

        // 静态方法，用于缓存 UIViewID 与 Mediator 和 Asset 类型的映射
        private static void CacheUIMapping()
        {
            // 如果映射已经存在，则直接返回
            if (MEDIATOR_MAPPING != null)
                return;

            // 初始化映射字典
            MEDIATOR_MAPPING = new Dictionary<UIViewID, Type>();
            ASSET_MAPPING = new Dictionary<UIViewID, Type>();

            // 获取 UIView 类的类型
            Type baseViewType = typeof(UIView);
            // 遍历程序集中的所有类型
            foreach (var type in baseViewType.Assembly.GetTypes())
            {
                // 跳过抽象类型
                if (type.IsAbstract)
                    continue;

                // 如果该类型是 UIView 的子类
                if (baseViewType.IsAssignableFrom(type))
                {
                    // 获取该类型上的 UIViewAttribute 属性 
                    object[] attrs = type.GetCustomAttributes(typeof(UIViewAttribute), false);
                    // 如果没有找到属性，则输出警告并继续
                    if (attrs.Length == 0)
                    {
                        Debug.Log($"{type.FullName} 没有绑定 Mediator，请使用UIMediatorAttribute绑定一个Mediator以正确使用");
                        continue;
                    }

                    // 将属性中的 ID 和 MediatorType 添加到映射字典中
                    foreach (UIViewAttribute attr in attrs)
                    {
                        MEDIATOR_MAPPING.Add(attr.ID, attr.MediatorType);
                        ASSET_MAPPING.Add(attr.ID, type);
                        break;
                    }
                }
            }
        }

        // 模块更新方法，每帧调用一次
        protected internal override void OnModuleUpdate(float deltaTime)
        {
            base.OnModuleUpdate(deltaTime);
            // 更新 UI 对象池的加载请求
            uiObjectPool.UpdateLoadRequests();
            // 更新所有正在使用的 Mediator
            foreach (var mediator in usingMediators)
            {
                mediator.Update(deltaTime);
            }
            // 更新蒙版图像的透明度
            UpdateMask(deltaTime);
        }

        // 量子控制台激活时的回调方法
        private void OnConsoleActive()
        {
            // 这里可以添加当量子控制台激活时的逻辑
            // 比如禁用游戏中的输入
        }

        // 量子控制台停用时的回调方法
        private void OnConsoleDeactive()
        {
            // 这里可以添加当量子控制台停用时的逻辑
            // 比如重新启用游戏中的输入
        }

        // 获取指定模式下的最高排序
        private int GetTopMediatorSortingOrder(UIMode mode)
        {
            // 初始化索引
            int lastIndexMediatorOfMode = -1;
            // 从后向前遍历正在使用的 Mediator 列表
            for (int i = usingMediators.Count - 1; i >= 0; i--)
            {
                UIMediator mediator = usingMediators[i];
                // 如果模式不匹配，则跳过
                if (mediator.UIMode != mode)
                    continue;

                // 记录最后一个匹配的 Mediator 的索引
                lastIndexMediatorOfMode = i;
                break;
            }

            // 如果没有找到匹配的 Mediator，则返回默认排序值
            if (lastIndexMediatorOfMode == -1)
                return mode == UIMode.Normal ? 0 : 1000;

            // 返回找到的 Mediator 的排序值
            return usingMediators[lastIndexMediatorOfMode].SortingOrder;
        }

        // 获取指定 UIViewID 的 Mediator
        private UIMediator GetMediator(UIViewID id)
        {
            // 缓存 UI 映射
            CacheUIMapping();

            // 尝试从映射中获取对应的 Mediator 类型
            if (!MEDIATOR_MAPPING.TryGetValue(id, out Type mediatorType))
            {
                Debug.Log($"找不到 {id} 对应的Mediator");
                return null;
            }

            // 尝试从空闲的 Mediator 字典中获取对应类型的队列
            if (!freeMediators.TryGetValue(mediatorType, out Queue<UIMediator> mediatorQ))
            {
                mediatorQ = new Queue<UIMediator>();
                freeMediators.Add(mediatorType, mediatorQ);
            }

            // 从队列中获取一个 Mediator 实例，如果队列为空则创建一个新的实例
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

        // 回收 Mediator
        private void RecycleMediator(UIMediator mediator)
        {
            // 如果 Mediator 为 null，则直接返回
            if (mediator == null)
                return;

            // 获取 Mediator 的类型
            Type mediatorType = mediator.GetType();
            // 尝试从空闲的 Mediator 字典中获取对应类型的队列
            if (!freeMediators.TryGetValue(mediatorType, out Queue<UIMediator> mediatorQ))
            {
                mediatorQ = new Queue<UIMediator>();
                freeMediators.Add(mediatorType, mediatorQ);
            }
            // 将 Mediator 放回队列中
            mediatorQ.Enqueue(mediator);
        }
        // 获取当前正在打开的 UIMediator
        public UIMediator GetOpeningUIMediator(UIViewID id)
        {
            // 根据 id 获取 UI 配置信息
            UIConfig uiConfig = UIConfig.ByID((int)id);
            if (uiConfig.IsNull)
                return null;

            // 获取对应的 Mediator
            UIMediator mediator = GetMediator(id);
            if (mediator == null)
                return null;

            // 获取 Mediator 的类型
            Type requiredMediatorType = mediator.GetType();
            // 遍历所有正在使用的 Mediator
            foreach (var item in usingMediators)
            {
                // 如果找到同类型的 Mediator，则返回该 Mediator
                if (item.GetType() == requiredMediatorType)
                    return item;
            }
            return null;
        }

        // 将指定的 UI 提到最前面
        public void BringToTop(UIViewID id)
        {
            // 获取当前正在打开的 UIMediator
            UIMediator mediator = GetOpeningUIMediator(id);
            if (mediator == null)
                return;

            // 获取指定模式下的最高排序值
            int topSortingOrder = GetTopMediatorSortingOrder(mediator.UIMode);
            if (mediator.SortingOrder == topSortingOrder)
                return;

            // 更新 Mediator 的排序值
            int sortingOrder = topSortingOrder + 10;
            mediator.SortingOrder = sortingOrder;

            // 将 Mediator 移到列表末尾
            usingMediators.Remove(mediator);
            usingMediators.Add(mediator);

            // 更新 Canvas 的排序值
            Canvas canvas = mediator.ViewObject.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.sortingOrder = sortingOrder;
            }
        }

        // 判断指定的 UI 是否已打开
        public bool IsUIOpened(UIViewID id)
        {
            return GetOpeningUIMediator(id) != null;
        }

        // 打开单个 UI，如果已经打开则直接返回
        public UIMediator OpenUISingle(UIViewID id, object arg = null)
        {
            // 获取当前正在打开的 UIMediator
            UIMediator mediator = GetOpeningUIMediator(id);
            if (mediator != null)
                return mediator;

            // 打开 UI
            return OpenUI(id, arg);
        }

        // 打开 UI
        public UIMediator OpenUI(UIViewID id, object arg = null)
        {
            // 根据 id 获取 UI 配置信息
            UIConfig uiConfig = UIConfig.ByID((int)id);
            if (uiConfig.IsNull)
                return null;

            // 获取对应的 Mediator
            UIMediator mediator = GetMediator(id);
            if (mediator == null)
                return null;

            // 加载 UI 资源
            GameObject uiObject = (uiObjectPool.LoadGameObject(uiConfig.Asset, (obj) =>
            {
                UIView newView = obj.GetComponent<UIView>();
                mediator.InitMediator(newView);
            })).gameObject;

            // 当 UI 资源加载完成后处理
            return OnUIObjectLoaded(mediator, uiConfig, uiObject, arg);
        }

        // 异步打开单个 UI
        public IEnumerator OpenUISingleAsync(UIViewID id, object arg = null)
        {
            // 如果 UI 没有打开，则异步打开
            if (!IsUIOpened(id))
            {
                yield return OpenUIAsync(id, arg);
            }
        }

        // 异步打开 UI
        public IEnumerator OpenUIAsync(UIViewID id, object arg = null)
        {
            // 根据 id 获取 UI 配置信息
            UIConfig uiConfig = UIConfig.ByID((int)id);
            if (uiConfig.IsNull)
                yield break;

            // 获取对应的 Mediator
            UIMediator mediator = GetMediator(id);
            if (mediator == null)
                yield break;

            // 异步加载 UI 资源
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

            // 等待加载完成
            while (!loadFinish)
            {
                yield return null;
            }
            yield return null;
            yield return null;
        }

        // UI 资源加载完成后的处理
        private UIMediator OnUIObjectLoaded(UIMediator mediator, UIConfig uiConfig, GameObject uiObject, object obj)
        {
            if (uiObject == null)
            {
                Debug.Log($"加载UI失败:{uiConfig.Asset}");
                RecycleMediator(mediator);
                return null;
            }

            // 获取 UI 视图组件
            UIView view = uiObject.GetComponent<UIView>();
            if (view == null)
            {
                Debug.Log($"UI Prefab不包含UIView脚本:{uiConfig.Asset}");
                RecycleMediator(mediator);
                uiObjectPool.UnloadGameObject(view.gameObject);
                return null;
            }

            // 设置 Mediator 的模式和排序值
            mediator.UIMode = uiConfig.Mode;
            int sortingOrder = GetTopMediatorSortingOrder(uiConfig.Mode) + 10;

            // 将 Mediator 添加到使用中的列表
            usingMediators.Add(mediator);

            // 设置 Canvas 的属性
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

            // 设置排序值
            mediator.SortingOrder = sortingOrder;
            canvas.sortingOrder = sortingOrder;

            // 激活 UI 并显示
            uiObject.SetActive(true);
            mediator.Show(uiObject, obj);
            return mediator;
        }

        // 关闭 UI
        public void CloseUI(UIMediator mediator)
        {
            if (mediator != null)
            {
                // 回收 UI 视图对象
                uiObjectPool.UnloadGameObject(mediator.ViewObject);
                mediator.ViewObject.transform.SetParentAndResetAll(closeUIRoot);

                // 回收 Mediator
                mediator.Hide();
                RecycleMediator(mediator);

                // 从使用中的列表中移除
                usingMediators.Remove(mediator);
            }
        }

        // 关闭所有 UI
        public void CloseAllUI()
        {
            for (int i = usingMediators.Count - 1; i >= 0; i--)
            {
                CloseUI(usingMediators[i]);
            }
        }

        // 根据 ID 关闭 UI
        public void CloseUI(UIViewID id)
        {
            UIMediator mediator = GetOpeningUIMediator(id);
            if (mediator == null)
                return;

            CloseUI(mediator);
        }

        // 设置所有普通 UI 的可见性
        public void SetAllNormalUIVisibility(bool visible)
        {
            normalUIRoot.gameObject.SetActive(visible);
        }

        // 设置所有模态 UI 的可见性
        public void SetAllModalUIVisibility(bool visible)
        {
            modalUIRoot.gameObject.SetActive(visible);
        }

        // 显示蒙版
        public void ShowMask(float duration = 0.5f)
        {
            destMaskAlpha = 1;
            maskDuration = duration;
        }

        // 隐藏蒙版
        public void HideMask(float? duration = null)
        {
            destMaskAlpha = 0;
            if (duration.HasValue)
            {
                maskDuration = duration.Value;
            }
        }

        // 更新蒙版的透明度
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

        // 显示量子控制台
        public void ShowConsole()
        {
            quantumConsole.Activate();
        }
    }
}

// 自定义属性，用于标记 UIView 类型
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed class UIViewAttribute : Attribute
{
    // UIViewID 和 Mediator 类型
    public UIViewID ID { get; }
    public Type MediatorType { get; }

    // 构造函数
    public UIViewAttribute(Type mediatorType, UIViewID id)
    {
        ID = id;
        MediatorType = mediatorType;
    }
}
