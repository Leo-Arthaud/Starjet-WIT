#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Sets the value of a NGUI UISlider component.")]
	public class NGUISliderSetValue : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UISlider))]
		[Tooltip("The GameObject with a UISlider component.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The new value for the UISlider.")]
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

			UISlider compSlider = go.GetComponent<UISlider>();

      if(compSlider == null)
      {
        LogError("GameObject " + go.name + " doesn't have a UISlider component.");
        return;
      }

      compSlider.Set(_value.Value);
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