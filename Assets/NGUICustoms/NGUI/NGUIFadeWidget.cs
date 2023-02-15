#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
  [ActionCategory("NGUI")]
  [Tooltip("Fade the alpha value of a single or multiple widgets in or out, depending on the set from or to value. If any of those two values is set to 'None' it utilizes the initial alpha value of the widgets, to fade to/from the current alpha value. Also automatically enables the specified GameObject if it's disabled.")]
  public class NGUIFadeWidget : EaseFsmAction
  {
    [RequiredField]
    [CheckForComponent(typeof(UIRect))]
    [Tooltip("NGUI widget to update. Can be UIWidget, UISprite, UIPanel, ...")]
    public FsmOwnerDefault target;

    [Tooltip("From which value the widgets should be fading. Leave as 'None' to use the current widget alpha value.")]
    public FsmFloat fromValue;

    [Tooltip("To which value the widgets should be fading. Leave as 'None' to use the current widget alpha value.")]
    public FsmFloat toValue;

    [UIHint(UIHint.Variable)]
    [Tooltip("Store the eased float value in a variable.")]
    public FsmFloat floatVariable;

    private UIRect _widgetComp;
    private bool finishInNextStep;

    public override void Reset()
    {
      base.Reset();

      target = null;
      fromValue = new FsmFloat {UseVariable = true};
      toValue = new FsmFloat {UseVariable = true};
      floatVariable = null;

      _widgetComp = null;
      finishInNextStep = false;
    }

    public override void OnEnter()
    {
      base.OnEnter();

      // get the UIWidget component
      var go = Fsm.GetOwnerDefaultTarget(target);
      _widgetComp = go.GetComponent<UIRect>();

      //enable GameObject if it's disabled
      go.SetActive(true);

      fromFloats = new float[1];
      fromFloats[0] = fromValue.IsNone ? _widgetComp.alpha : fromValue.Value;
      floatVariable.Value = fromFloats[0];
      toFloats = new float[1];
      toFloats[0] = toValue.IsNone ? _widgetComp.alpha : toValue.Value;
      resultFloats = new float[1];
      finishInNextStep = false;

      //apply starting alpha initially to prevent flashing
      _widgetComp.alpha = fromFloats[0];
    }

    public override void OnUpdate()
    {
      base.OnUpdate();

      if (isRunning)
      {
        floatVariable.Value = resultFloats[0];
        _widgetComp.alpha = floatVariable.Value;
      }

      if (finishInNextStep)
      {
        Finish();
        if (finishEvent != null) Fsm.Event(finishEvent);
      }

      if (!finishAction || finishInNextStep) return;

      floatVariable.Value = reverse.IsNone ? toValue.Value : reverse.Value ? fromValue.Value : toValue.Value;
      finishInNextStep = true;
    }

#if UNITY_EDITOR
    public override string AutoName()
    {
      var go = Fsm.GetOwnerDefaultTarget(target);
      return "NGUIFadeWidget - " + go.name + ": " + fromValue.Value + " -> " + toValue.Value;
    }
#endif
  }
}

#endif
