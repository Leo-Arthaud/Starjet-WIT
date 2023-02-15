#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Sets the value of a NGUI UIScrollBar component.")]
	public class NGUIScrollBarSetValue : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UIScrollBar))]
		[Tooltip("The GameObject with a UIScrollBar component.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The new value for the UIScrollBar.")]
    public FsmFloat _value;

		public override void Reset()
		{
		  base.Reset();

			gameObject = null;
      _value = null;
		}

		public override void OnEnter()
		{
			SetValue();

			if(updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			SetValue();
		}

		void SetValue()
		{
		  GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

			if(!go)
      {
        LogError("GameObject in " + Owner.name + " (" + Fsm.Name + ") is null!");
        return;
      }

			UIScrollBar compScrollBar = go.GetComponent<UIScrollBar>();

      if(compScrollBar == null)
      {
        LogError("GameObject " + go.name + " doesn't have a UIScrollBar component.");
        return;
      }

      compScrollBar.value = _value.Value;
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, gameObject.GameObject);
    }
#endif
	}
}

#endif