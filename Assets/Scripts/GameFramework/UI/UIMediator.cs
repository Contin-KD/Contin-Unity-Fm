using Config;
using UnityEngine;

namespace TGame.UI
{
    public abstract class UIMediator<T> : UIMediator where T : UIView
    {
        protected T view;

        protected override void OnShow(object arg)
        {
            base.OnShow(arg);
            view = ViewObject.GetComponent<T>();
        }
        protected override void OnHide()
        {
            view = default;
            base.OnHide();
        }

        protected void Close()
        {
            TGameFrameWork.Instance.GetModule<UIModule>().CloseUI(this);
        }

        public override void InitMediator(UIView view)
        {
            base.InitMediator(view);
            OnInit(view as T);
        }

        protected virtual void OnInit(T view) { }
    }

    public abstract class UIMediator
    {

        public event System.Action OnMediatorHide;
        public GameObject ViewObject { get; set; }
        public int SortingOrder { get; set; }
        public UIMode UIMode { get; set; }

        public virtual void InitMediator(UIView view) { }

        public void Show(GameObject viewObject, object arg)
        {
            ViewObject = viewObject;
            OnShow(arg);
        }
        protected virtual void OnShow(object arg) { }

        public void Hide()
        {
            OnHide();
            OnMediatorHide?.Invoke();
            OnMediatorHide = null;
            ViewObject = default;
        }

        protected virtual void OnHide() { }

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        protected virtual void OnUpdate(float deltaTime) { }
    }
}
