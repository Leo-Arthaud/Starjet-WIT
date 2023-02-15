#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Store the value of a given UISlider component.")]
	public class NGUISliderGetValue : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UISlider))]
		[Tooltip("The GameObject with the UISlider component attached.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("The value of the given UISlider component.")]
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

			UISlider compSlider = go.GetComponent<UISlider>();

			if (compSlider != null)
			{
				result.Value = compSlider.value;
			} else LogError("No UISlider component attached to " + go.name + "!");
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