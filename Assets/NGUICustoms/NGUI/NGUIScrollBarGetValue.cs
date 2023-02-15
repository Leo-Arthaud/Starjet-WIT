#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Store the value of a given UIScrollBar component.")]
	public class NGUIScrollBarGetValue : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UIScrollBar))]
		[Tooltip("The GameObject with the UIScrollBar component attached.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("The value of the given UIScrollBar component.")]
		public FsmFloat result;

		public override void Reset()
		{
		  base.Reset();

		  gameObject = null;
		  result = null;
		}

		public override void OnEnter()
		{
			GetValue();

			if (updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			GetValue();
		}

		void GetValue()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);

			if (go == null)
			{
				LogError("GameObject in " + Name + " is null!");
				return;
			}

			UIScrollBar compScrollBar = go.GetComponent<UIScrollBar>();

			if (compScrollBar != null)
			{
				result.Value = compScrollBar.value;
			} else LogError("No UIScrollBar component attached to " + go.name + "!");
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