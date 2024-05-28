using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TGame.Procedure
{
    /// <summary>
    /// ���̿���ģ��
    /// </summary>
    public partial class ProcedureModule : BaseGameModule
    {
        // Unity �༭���н������л���
        [SerializeField]
        private string[] proceduresNames = null; // ��ŵ���������
        [SerializeField]
        private string defaultProcedureName = null; // ��Ϸ����ʱ��Ĭ���������ơ�
        // ��ǰ�������
        public BaseProcedure CurrentProcedure { get; private set; }
        // ����ģ���Ƿ���������
        public bool IsRunning { get; private set; }
        // �Ƿ����ڸ�������
        public bool IsChangingProcedure { get; private set; }
        // ���ڰ����ʹ洢��������
        private Dictionary<Type, BaseProcedure> procedures;
        // ������Ϸ��Ĭ�����̡�
        private BaseProcedure defaultProcedure;
        // �������������
        private ObjectPool<ChangeProcedureRequest> changeProcedureRequestPool = new ObjectPool<ChangeProcedureRequest>(null);
        // ���̱���������
        private Queue<ChangeProcedureRequest> changeProcedureQ = new Queue<ChangeProcedureRequest>();
        
        /// <summary>
        /// ��ʼ��ģ��
        /// </summary>
        protected internal override void OnModuleInit()
        {
            base.OnModuleInit();
            procedures = new Dictionary<Type, BaseProcedure>();
            // �ж��Ƿ��ҵ�Ĭ������
            bool findDefaultState = false;
            // ѭ�������̳�ʼ��
            for (int i = 0; i < proceduresNames.Length; i++)
            {
                // ��ȡ����
                string procedureTypeName = proceduresNames[i];
                // �ж��Ƿ�Ϊ��
                if (string.IsNullOrEmpty(procedureTypeName))
                    continue;
                // ����������
                Type procedureType = Type.GetType(procedureTypeName, true);
                if (procedureType == null)
                {
                    Debug.LogError($"Can't find procedure:`{procedureTypeName}`");
                    continue;
                }
                // ���䴴��һ��ʵ��
                BaseProcedure procedure = Activator.CreateInstance(procedureType) as BaseProcedure;
                // ת��ʧ�ܾ�Ϊ��
                // �ж��Ƿ�ΪĬ����������
                bool isDefaultState = procedureTypeName == defaultProcedureName;
                
                procedures.Add(procedureType, procedure);

                if (isDefaultState)
                {
                    defaultProcedure = procedure;
                    findDefaultState = true;
                }
            }
            // �Ƿ��ҵ�Ĭ������
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
            // ֹͣ��ճ���, ��ն���, ����״̬
            changeProcedureRequestPool.Clear();
            changeProcedureQ.Clear();
            IsRunning = false;
        }

        protected internal override void OnModuleUpdate(float deltaTime)
        {
            base.OnModuleUpdate(deltaTime);
        }

        // ��������
        public async Task StartProcedure()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            // �Ӷ�����л�ȡһ�����̱���������
            ChangeProcedureRequest changeProcedureRequest = changeProcedureRequestPool.Obtain();
            // ���ñ�������Ŀ������ΪĬ������
            changeProcedureRequest.TargetProcedure = defaultProcedure;
            // �����������뵽���̱��������
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
            // �Ӷ�����л�ȡһ�����̱���������
            ChangeProcedureRequest changeProcedureRequest = changeProcedureRequestPool.Obtain();
            // ���ñ�������Ŀ�����̺ʹ��ݵĲ���ֵ
            changeProcedureRequest.TargetProcedure = procedure;
           
            changeProcedureRequest.Value = value;
            // �����������뵽���̱��������
            changeProcedureQ.Enqueue(changeProcedureRequest);
            // �����ǰû���ڽ������̱������ʼ����������
            if (!IsChangingProcedure)
            {
                await ChangeProcedureInternal();
            }
        }
        /// <summary>
        /// �������̸���
        /// </summary>
        /// <returns></returns>
        private async Task ChangeProcedureInternal()
        {
            if (IsChangingProcedure)
                return;

            IsChangingProcedure = true;
            // ��������е��������̱������
            while (changeProcedureQ.Count > 0)
            {
                // ��ȡһ�����̱������
                ChangeProcedureRequest request = changeProcedureQ.Dequeue();
                if (request == null || request.TargetProcedure == null)
                    continue;

                // �����ǰ��������
                if (CurrentProcedure != null)
                {
                    // ���÷��صķ���
                    await CurrentProcedure.OnLeaveProcedure();
                }
                // ������Ŀ������
                CurrentProcedure = request.TargetProcedure;
                // �첽ִ�и�����ķ���
                await CurrentProcedure.OnEnterProcedure(request.Value);
            }
            IsChangingProcedure = false;
        }
    }

    // ���ڱ�ʾ���̱������
    public class ChangeProcedureRequest
    {
        public BaseProcedure TargetProcedure { get; set; }
        public object Value { get; set; }
    }
}