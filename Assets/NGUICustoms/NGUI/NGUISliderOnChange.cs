#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Send an event if the value of a UISlider has changed.")]
	public class NGUISliderOnChange : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UISlider))]
		[Tooltip("The GameObject with the UISlider component attached.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Send when the slider changes.")]
    public FsmEvent changeEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the current value of the given UISlider component.")]
		public FsmFloat storeResult;

		private UISlider compSlider;

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

      compSlider = go.GetComponent<UISlider>();

      if (compSlider != null)
      {
        storeResult.Value = compSlider.value;
      } else LogError("No UISlider component attached to " + go.name + "!");

      CheckValue();

			if (updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			CheckValue();
		}

		void CheckValue()
		{
			if(compSlider.value != storeResult.Value) {
			  storeResult.Value = compSlider.value;
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