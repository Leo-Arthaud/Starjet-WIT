#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Send an event if the value of a UIScrollBar has changed.")]
	public class NGUIScrollBarOnChange : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UIScrollBar))]
		[Tooltip("The GameObject with the UIScrollBar component attached.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Send when the slider changes.")]
    public FsmEvent changeEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the current value of the given UIScrollBar component.")]
		public FsmFloat storeResult;

		private UIScrollBar compScrollBar;

		public override void Reset()
		{
		  base.Reset();

		  gameObject = null;
		  storeResult = null;
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);

      if (go == null)
      {
        LogError("GameObject in " + Name + " is null!");
        return;
      }

      compScrollBar = go.GetComponent<UIScrollBar>();

      if (compScrollBar != null)
      {
        storeResult.Value = compScrollBar.value;
      } else LogError("No UIScrollBar component attached to " + go.name + "!");

      CheckValue();

			if (updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			CheckValue();
		}

		void CheckValue()
		{
			if(compScrollBar.value != storeResult.Value) {
			  storeResult.Value = compScrollBar.value;
			  Fsm.Event(changeEvent);
			}
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