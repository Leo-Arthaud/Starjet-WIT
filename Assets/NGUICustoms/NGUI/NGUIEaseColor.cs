#if PLAYMAKER

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Ease the color of an NGUI UIWidget between the from and to value. Set any of these two values to 'None' to use the color of the widget when starting this action.")]
	public class NGUIEaseColor : EaseFsmAction
	{
    public enum WidgetColorType
    {
      Color,
      GradientTop,
      GradientBottom
    }

    [ObjectType(typeof(WidgetColorType))]
    public FsmEnum colorType;
    
    [RequiredField]
    [CheckForComponent(typeof(UIWidget))]
    [Tooltip("NGUI widget to update. Can be UIWidget, UISprite, UIPanel, ...")]
    public FsmOwnerDefault target;

    [Tooltip("From which value the widgets should be easing. Leave as 'None' to use the current widget color value.")]
    public FsmColor fromValue;

    [Tooltip("To which value the widgets should be easing. Leave as 'None' to use the current widget color value.")]
    public FsmColor toValue;

    [UIHint(UIHint.Variable)]
    [Tooltip("Store the eased float value in a variable.")]
    public FsmColor colorVariable;

    private Color srcCol, dstCol;
    private UIWidget widgetComp;
    private UISprite sprite;
    private UILabel label;
		private bool finishInNextStep;
		
		public override void Reset()
    {
			base.Reset();

      colorType = WidgetColorType.Color;
      target = null;
      fromValue = new FsmColor {UseVariable = true};
      toValue = new FsmColor {UseVariable = true};
      colorVariable = null;

      widgetComp = null;
      finishInNextStep = false;
		}

    public override void OnEnter()
		{
			base.OnEnter();
      
      // get the UIWidget component
      GameObject go = Fsm.GetOwnerDefaultTarget(target);

      if (go == null)
      {
        Debug.LogError("GameObject is null!");
        Finish();
      }

      widgetComp = go.GetComponent<UIWidget>();

      if (widgetComp == null)
      {
        Debug.LogError(go.name + " does not contain any Widget Component!");
        Finish();
      }
      
      sprite = go.GetComponent<UISprite>();
      label = go.GetComponent<UILabel>();

      if (fromValue.IsNone)
      {
	      if ((WidgetColorType) colorType.Value == WidgetColorType.GradientTop)
	      {
					if(sprite != null) srcCol = sprite.gradientTop;
					if(label != null) srcCol = label.gradientTop;
	      }
	      else if ((WidgetColorType) colorType.Value == WidgetColorType.GradientBottom)
	      {
		      if(sprite != null) srcCol = sprite.gradientBottom;
		      if(label != null) srcCol = label.gradientBottom;
	      }
        else srcCol = widgetComp.color;
      }
      else srcCol = fromValue.Value;
      
      if (toValue.IsNone)
      {
	      if ((WidgetColorType) colorType.Value == WidgetColorType.GradientTop)
	      {
		      if(sprite != null) dstCol = sprite.gradientTop;
		      if(label != null) dstCol = label.gradientTop;
	      }
	      else if ((WidgetColorType) colorType.Value == WidgetColorType.GradientBottom)
	      {
		      if(sprite != null) dstCol = sprite.gradientBottom;
		      if(label != null) dstCol = label.gradientBottom;
	      }
        else dstCol = widgetComp.color;
      }
      else dstCol = toValue.Value;
      
			fromFloats = new float[4];
			fromFloats[0] = srcCol.r;
			fromFloats[1] = srcCol.g;
			fromFloats[2] = srcCol.b;
			fromFloats[3] = srcCol.a;
      
			toFloats = new float[4];
			toFloats[0] = dstCol.r;
			toFloats[1] = dstCol.g;
			toFloats[2] = dstCol.b;
			toFloats[3] = dstCol.a;
      
			resultFloats = new float[4];
			finishInNextStep = false;
      
		  colorVariable.Value = srcCol;
      ApplyColor();
		}

    public override void OnUpdate() {
			base.OnUpdate();
      
			if (isRunning) {
				colorVariable.Value = new Color(resultFloats[0],resultFloats[1],resultFloats[2], resultFloats[3]);
        ApplyColor();
      }
			
			if (finishInNextStep) {
				Finish();
				if (finishEvent != null)	Fsm.Event(finishEvent);
			}
			
      if (!finishAction || finishInNextStep) return;
      
      colorVariable.Value = new Color(reverse.IsNone ? dstCol.r : reverse.Value ? srcCol.r : dstCol.r, 
                                      reverse.IsNone ? dstCol.g : reverse.Value ? srcCol.g : dstCol.g,
                                      reverse.IsNone ? dstCol.b : reverse.Value ? srcCol.b : dstCol.b,
                                      reverse.IsNone ? dstCol.a : reverse.Value ? srcCol.a : dstCol.a);
      
      finishInNextStep = true;
		}

    private void ApplyColor()
    {
	    if ((WidgetColorType) colorType.Value == WidgetColorType.GradientTop)
	    {
		    if(sprite != null) sprite.gradientTop = colorVariable.Value;
		    if(label != null) label.gradientTop = colorVariable.Value;
	    }
      else if ((WidgetColorType) colorType.Value == WidgetColorType.GradientBottom)
	    {
		    if(sprite != null) sprite.gradientBottom = colorVariable.Value;
		    if(label != null) label.gradientBottom = colorVariable.Value;
	    }
      else widgetComp.color = colorVariable.Value;
    }
    
#if UNITY_EDITOR
    public override string AutoName()
    {
      var go = Fsm.GetOwnerDefaultTarget(target);
      return "NGUIEaseColor - " + go.name + ": " + fromValue.Value + " -> " + toValue.Value;
    }
#endif
	}
}

#endif