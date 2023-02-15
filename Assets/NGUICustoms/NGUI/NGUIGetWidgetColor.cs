#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Get the color of an UIWidget. Can also store the gradient colors if the UIWidget contains any (like UISprite, UILabel, ...).")]
	public class NGUIGetWidgetColor : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UIWidget))]
		[Tooltip("UIWidget to update.")]
		public FsmOwnerDefault widget;

		[UIHint(UIHint.Variable)]
		[Tooltip("The current color of the widget.")]
		public FsmColor color;

		[UIHint(UIHint.Variable)]
		[Tooltip("The color for the top gradient of the UISprite or UILabel.")]
		public FsmColor gradientTop;

		[UIHint(UIHint.Variable)]
		[Tooltip("The color for the bottom gradient of the UISprite or UILabel.")]
		public FsmColor gradientBottom;

		public override void Reset()
		{
			base.Reset();

			widget = null;
			color = null;
			gradientTop = null;
			gradientBottom = null;
		}

		public override void OnEnter()
		{
			DoGetWidgetColor();

			if(updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			DoGetWidgetColor();
		}

		private void DoGetWidgetColor()
		{
			//exit if no GameObject
			if(widget == null) {
			  LogError("GameObject is null!");
			  return;
			}

			GameObject go = Fsm.GetOwnerDefaultTarget(widget);

			// get the object as a widget
			UIWidget widgetComp = go.GetComponent<UIWidget>();

			// exit if no widget
			if(widgetComp == null) {
			  LogError(go.name + " has no supported NGUI component.");
			  return;
			}

			// get color value
			if(!color.IsNone) color.Value = widgetComp.color;

			UISprite sprite = go.GetComponent<UISprite>();
      UILabel label = go.GetComponent<UILabel>();

			if(sprite != null)
			{
				if(!gradientTop.IsNone) gradientTop.Value = sprite.gradientTop;
				if(!gradientTop.IsNone) gradientBottom.Value = sprite.gradientBottom;
			}
			
			if(label != null)
			{
				if(!gradientTop.IsNone) gradientTop.Value = label.gradientTop;
				if(!gradientTop.IsNone) gradientBottom.Value = label.gradientBottom;
			}
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, widget.GameObject);
    }
#endif
	}
}

#endif