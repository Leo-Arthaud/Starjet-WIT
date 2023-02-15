#if PLAYMAKER

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Retrieve one or more parameters of any NGUI component.")]
	public class NGUIGetWidgetValues : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The target GameObject with the NGUI component attached.")]
		public FsmOwnerDefault target;

		[UIHint(UIHint.Variable)]
		[Tooltip("Returns if the GameObject contains any widgets.")]
		public FsmBool hasWidget;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the attached widget, if any. Includes UIPanel, UISprite, UIWidget and the like.")]
		public FsmObject storeWidget;

		[UIHint(UIHint.Variable)]
		[Tooltip("Get the current widgets alpha value.")]
		public FsmFloat widgetAlpha;

		[UIHint(UIHint.Variable)]
		[Tooltip("Get the depth of the attached widget.")]
		public FsmInt widgetDepth;

		[UIHint(UIHint.Variable)]
		[Tooltip("Returns whether the widget is anchored.")]
		public FsmBool isAnchored;

		[Tooltip("Get the values every frame.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
		  target = null;
		  hasWidget = false;
		  storeWidget = null;
		  widgetAlpha = null;
			widgetDepth = null;
			isAnchored = false;
			everyFrame = false;
		}

		public override void OnEnter()
    {
			GetInfo();

			if (!everyFrame.Value) Finish();
		}

		public override void OnUpdate()
		{
			GetInfo();
		}

		void GetInfo()
		{
			var go = Fsm.GetOwnerDefaultTarget(target);

			if (go == null)
			{
				LogError("GameObject in " + this.Name + " is null!");
				return;
			}

			// get the widget of the current GameObject
      UIWidget widget = go.GetComponent<UIWidget>();

      // exit if there's no widget
      if (widget == null)
      {
        hasWidget.Value = false;
        LogWarning(go.name + " does not contain any UIWidget Component!");
      }
      else
      {
        hasWidget.Value = true;
        storeWidget.Value = widget;
        widgetAlpha.Value = widget.alpha;
        widgetDepth.Value = widget.depth;
        isAnchored.Value = widget.isAnchored;
      }
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