#if PLAYMAKER

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Set one or more parameters of any NGUI component.")]
	public class NGUISetWidgetValues : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The target GameObject with the NGUI component attached.")]
		public FsmOwnerDefault target;
		
		[Tooltip("Enable or disable the component.")]
		public FsmBool enabled;

		[Tooltip("Set a custom alpha value for the attached widget. (O = Invisible, 1=fully visible)")]
		[HasFloatSlider(0, 1)]
		public FsmFloat widgetAlpha;

		[Tooltip("Set a custom depth for the attached widget.")]
		public FsmInt widgetDepth;

		[Tooltip("Get the values every frame.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			target = null;
			enabled = new FsmBool { UseVariable = true };
			widgetDepth = new FsmInt { UseVariable = true };
			widgetAlpha = new FsmFloat { UseVariable = true };
			everyFrame = false;
		}

		public override void OnEnter()
    {
			SetInfo();

			if (!everyFrame.Value) Finish();
		}

		public override void OnUpdate()
		{
			SetInfo();
		}

		void SetInfo()
		{
			var go = Fsm.GetOwnerDefaultTarget(target);

			if (go == null)
			{
				Log("GameObject in " + Owner.name + " is null!");
				return;
			}

			// get the object as a widget
      UIWidget widget = go.GetComponent<UIWidget>();

      if (widget == null)
      {
        LogError(go.name + " does not contain any UIWidget Component!");
        return;
      }

      if(!enabled.IsNone) widget.enabled = enabled.Value;
      if(!widgetAlpha.IsNone) widget.alpha = widgetAlpha.Value;
      if(!widgetDepth.IsNone) widget.depth = widgetDepth.Value;
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, target.GameObject);
    }
#endif
	}
}

#endif