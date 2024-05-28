using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TGame.Procedure
{
    /// <summary>
    /// 流程控制模块
    /// </summary>
    public partial class ProcedureModule : BaseGameModule
    {
        // Unity 编辑器中进行序列化。
        [SerializeField]
        private string[] proceduresNames = null; // 存放的流程名称
        [SerializeField]
        private string defaultProcedureName = null; // 游戏启动时的默认流程名称。
        // 当前活动的流程
        public BaseProcedure CurrentProcedure { get; private set; }
        // 流程模块是否正在运行
        public bool IsRunning { get; private set; }
        // 是否正在更改流程
        public bool IsChangingProcedure { get; private set; }
        // 用于按类型存储所有流程
        private Dictionary<Type, BaseProcedure> procedures;
        // 启动游戏的默认流程。
        private BaseProcedure defaultProcedure;
        // 更换流程请求池
        private ObjectPool<ChangeProcedureRequest> changeProcedureRequestPool = new ObjectPool<ChangeProcedureRequest>(null);
        // 流程变更请求队列
        private Queue<ChangeProcedureRequest> changeProcedureQ = new Queue<ChangeProcedureRequest>();
        
        /// <summary>
        /// 初始化模块
        /// </summary>
        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
            procedures = new Dictionary<Type, BaseProcedure>();
            // 判断是否找到默认流程
            bool findDefaultState = false;
            // 循环对流程初始化
            for (int i = 0; i < proceduresNames.Length; i++)
            {
                // 获取名称
                string procedureTypeName = proceduresNames[i];
                // 判断是否为空
                if (string.IsNullOrEmpty(procedureTypeName))
                    continue;
                // 反射获得属性
                Type procedureType = Type.GetType(procedureTypeName, true);
                if (procedureType == null)
                {
                    Debug.LogError($"Can't find procedure:`{procedureTypeName}`");
                    continue;
                }
                // 反射创建一个实例
                BaseProcedure procedure = Activator.CreateInstance(procedureType) as BaseProcedure;
                // 转换失败就为空
                // 判断是否为默认流程名称
                bool isDefaultState = procedureTypeName == defaultProcedureName;
                
                procedures.Add(procedureType, procedure);

                if (isDefaultState)
                {
                    defaultProcedure = procedure;
                    findDefaultState = true;
                }
            }
            // 是否找到默认流程
            if (!findDefaultState)
            {
                Debug.LogError($"You have to set a correct default procedure to start game");
            }
        }

        protected internal override void OnModuleStart()
        {
            base.OnModuleStart();
        }

        protected internal override void OnModuleStop()
        {
            base.OnModuleStop();
            // 停止清空池子, 清空队列, 更改状态
            changeProcedureRequestPool.Clear();
            changeProcedureQ.Clear();
            IsRunning = false;
        }

        protected internal override void OnModuleUpdate(float deltaTime)
        {
            base.OnModuleUpdate(deltaTime);
        }

        // 启动流程
        public async Task StartProcedure()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            // 从对象池中获取一个流程变更请求对象
            ChangeProcedureRequest changeProcedureRequest = changeProcedureRequestPool.Obtain();
            // 设置变更请求的目标流程为默认流程
            changeProcedureRequest.TargetProcedure = defaultProcedure;
            // 将变更请求加入到流程变更队列中
            changeProcedureQ.Enqueue(changeProcedureRequest);
            await ChangeProcedureInternal();
        }

        public async Task ChangeProcedure<T>() where T : BaseProcedure
        {
            await ChangeProcedure<T>(null);
        }

        public async Task ChangeProcedure<T>(object value) where T : BaseProcedure
        {
            if (!IsRunning)
                return;
            
            if (!procedures.TryGetValue(typeof(T), out BaseProcedure procedure))
            {
                UnityEngine.Debug.Log($"Change Procedure Failed, Can't find Proecedure:${typeof(T).FullName}");
                return;
            }
            // 从对象池中获取一个流程变更请求对象
            ChangeProcedureRequest changeProcedureRequest = changeProcedureRequestPool.Obtain();
            // 设置变更请求的目标流程和传递的参数值
            changeProcedureRequest.TargetProcedure = procedure;
           
            changeProcedureRequest.Value = value;
            // 将变更请求加入到流程变更队列中
            changeProcedureQ.Enqueue(changeProcedureRequest);
            // 如果当前没有在进行流程变更，则开始处理变更请求
            if (!IsChangingProcedure)
            {
                await ChangeProcedureInternal();
            }
        }
        /// <summary>
        /// 处理流程更改
        /// </summary>
        /// <returns></returns>
        private async Task ChangeProcedureInternal()
        {
            if (IsChangingProcedure)
                return;

            IsChangingProcedure = true;
            // 处理队列中的所有流程变更请求
            while (changeProcedureQ.Count > 0)
            {
                // 获取一个流程变更清流
                ChangeProcedureRequest request = changeProcedureQ.Dequeue();
                if (request == null || request.TargetProcedure == null)
                    continue;

                // 如果当前的有流程
                if (CurrentProcedure != null)
                {
                    // 调用返回的方法
                    await CurrentProcedure.OnLeaveProcedure();
                }
                // 更换到目标流程
                CurrentProcedure = request.TargetProcedure;
                // 异步执行更换后的方法
                await CurrentProcedure.OnEnterProcedure(request.Value);
            }
            IsChangingProcedure = false;
        }
    }

    // 用于表示流程变更请求
    public class ChangeProcedureRequest
    {
        public BaseProcedure TargetProcedure { get; set; }
        public object Value { get; set; }
    }
}