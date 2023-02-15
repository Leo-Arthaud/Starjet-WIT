#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Adjust the alpha value of a single or multiple widgets.")]
	public class NGUISetWidgetAlphaMulti : FsmStateActionUpdate
	{
		[RequiredField]
		[Tooltip("NGUI widgets to update. Can be UIWidget, UISprite, UIPanel, ...")]
		public FsmGameObject[] widgets;

		[RequiredField]
		[HasFloatSlider(0, 1)]
		[Tooltip("The new alpha to assign to the widgets. Set or ease between 0 and 1.")]
		public FsmFloat alpha;

		public override void Reset()
		{
			base.Reset();

			widgets = new FsmGameObject[1];
			alpha = null;
		}

		public override void OnEnter()
		{
			DoSetWidgetsAlpha();

			if(updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			DoSetWidgetsAlpha();
		}

		private void DoSetWidgetsAlpha()
		{
			// exit if objects are null
			if((widgets == null) || (widgets.Length == 0) || (alpha == null)) return;

			// handle each widget
			for(int i = 0; i < widgets.Length; i++)
			{
			//skip unset GameObjects
				if(!widgets[i].Value) continue;

				// get the UIWidget component
				UIWidget widget = widgets[i].Value.GetComponent<UIWidget>();

				if(widget == null)
				{
					UIPanel panel = widgets[i].Value.GetComponent<UIPanel>();
					if(panel == null)
						Debug.LogWarning(widgets[i].Value.name + " does not contain any Widget Component!");
					else panel.alpha = alpha.Value;
				} else widget.alpha = alpha.Value;
			}
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, alpha);
    }
#endif
	}
}

#endif