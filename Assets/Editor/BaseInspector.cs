using UnityEditor;

namespace TGame.Editor.Inspector
{
    /// <summary>
    /// 作者: Teddy
    /// 时间: 2018/03/05
    /// 功能: 
    /// </summary>
	public class BaseInspector : UnityEditor.Editor
    {
        protected virtual bool DrawBaseGUI { get { return true; } }

        private bool isCompiling = false;
        protected virtual void OnInspectorUpdateInEditor() { }

        private void OnEnable()
        {
            OnInspectorEnable();
            EditorApplication.update += UpdateEditor;
        }
        protected virtual void OnInspectorEnable() { }

        private void OnDisable()
        {
            EditorApplication.update -= UpdateEditor;
            OnInspectorDisable();
        }
        protected virtual void OnInspectorDisable() { }

        private void UpdateEditor()
        {
            if (!isCompiling && EditorApplication.isCompiling)
            {
                isCompiling = true;
                OnCompileStart();
            }
            else if (isCompiling && !EditorApplication.isCompiling)
            {
                isCompiling = false;
                OnCompileComplete();
            }
            OnInspectorUpdateInEditor();
        }

        public override void OnInspectorGUI()
        {
            if (DrawBaseGUI)
            {
                base.OnInspectorGUI();
            }
        }

        protected virtual void OnCompileStart() { }
        protected virtual void OnCompileComplete() { }
    }
}