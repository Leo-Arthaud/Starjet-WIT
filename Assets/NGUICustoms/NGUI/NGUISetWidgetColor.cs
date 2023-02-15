#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Sets the color of an UIWidget. Can also alter the gradient colors "
	       + "if the UIWidget contains any (like UISprite, UILabel, ...).")]
	public class NGUISetWidgetColor : FsmStateActionUpdate
	{
		[RequiredField]
		[CheckForComponent(typeof(UIWidget))]
		[Tooltip("UIWidget to update.")]
		public FsmOwnerDefault widget;

		[Tooltip("The new color to assign to the widget. Set as 'None' to ignore.")]
		public FsmColor color;

		[Tooltip("The color for the top gradient of the UISprite or UILabel. Set as 'none' to ignore.")]
		public FsmColor gradientTop;

		[Tooltip("The color for the bottom gradient of the UISprite or UILabel. Set as 'none' to ignore.")]
		public FsmColor gradientBottom;

		public override void Reset()
		{
			base.Reset();

			widget = null;
			color = Color.white;
			gradientTop = Color.white;
			gradientBottom = new Color(0.7f, 0.7f, 0.7f, 1f);
		}

		public override void OnEnter()
		{
			DoSetWidgetColor();

			if(updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			DoSetWidgetColor();
		}

		private void DoSetWidgetColor()
		{
			//exit if no GameObject
			if (widget == null) {
			  LogError("GameObject is null!");
			  return;
			}

			GameObject go = Fsm.GetOwnerDefaultTarget(widget);

			// get the object as a widget
			UIWidget widgetComp = go.GetComponent<UIWidget>();

			// exit if no widget
			if (widgetComp == null) {
			  LogError(go.name + " has no supported NGUI component.");
			  return;
			}

			// set color value
			if (!color.IsNone) widgetComp.color = color.Value;

			UISprite sprite = go.GetComponent<UISprite>();
      UILabel label = go.GetComponent<UILabel>();

			if (sprite != null)
			{
				if(!gradientTop.IsNone) sprite.gradientTop = gradientTop.Value;
				if(!gradientTop.IsNone) sprite.gradientBottom = gradientBottom.Value;
			}
			
			if (label != null)
			{
				if(!gradientTop.IsNone) label.gradientTop = gradientTop.Value;
				if(!gradientTop.IsNone) label.gradientBottom = gradientBottom.Value;
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