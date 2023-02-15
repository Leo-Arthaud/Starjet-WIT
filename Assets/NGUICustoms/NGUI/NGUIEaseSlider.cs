#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
  [ActionCategory("NGUI")]
  [Tooltip("Eases the value of a UISlider or UISprite fill amount slider. Set either the From Value or To Value to 'None' to utilize the current slider value for that parameter.")]
  public class NGUIEaseSlider : EaseFsmAction
  {
    [RequiredField]
    [CheckForComponent(typeof(UIRect))]
    [Tooltip("NGUI widget to update. Can be a UISlider or UISprite.")]
    public FsmOwnerDefault target;

    [Tooltip("From which value the widgets should be fading. Leave as 'None' to use the current widget alpha value.")]
    public FsmFloat fromValue;

    [Tooltip("To which value the widgets should be fading. Leave as 'None' to use the current widget alpha value.")]
    public FsmFloat toValue;

    [UIHint(UIHint.Variable)]
    [Tooltip("Store the eased float value in a variable.")]
    public FsmFloat floatVariable;

    private UISlider sliderComp;
    private UISprite spriteComp;
    private bool finishInNextStep;

    public override void Reset()
    {
      base.Reset();

      target = null;
      fromValue = new FsmFloat {UseVariable = true};
      toValue = new FsmFloat {UseVariable = true};
      floatVariable = null;

      sliderComp = null;
      spriteComp = null;
      finishInNextStep = false;
    }

    public override void OnEnter()
    {
      base.OnEnter();

      // get the UIWidget component
      var go = Fsm.GetOwnerDefaultTarget(target);
      sliderComp = go.GetComponent<UISlider>();
      spriteComp = go.GetComponent<UISprite>();

      if (sliderComp == null && spriteComp == null)
      {
        Debug.LogError("Neither a UISlider nor UISprite component attached to " + go.name + " to ease the fill amount of!");
      }

      float currSliderValue = sliderComp != null ? sliderComp.value : spriteComp.fillAmount;

      fromFloats = new float[1];
      fromFloats[0] = fromValue.IsNone ? currSliderValue : fromValue.Value;
      floatVariable.Value = fromFloats[0];
      toFloats = new float[1];
      toFloats[0] = toValue.IsNone ? currSliderValue : toValue.Value;
      resultFloats = new float[1];
      finishInNextStep = false;

      //apply starting alpha initially to prevent flashing
      if(sliderComp != null) sliderComp.value = fromFloats[0];
      if(spriteComp != null) spriteComp.fillAmount = fromFloats[0];
    }

    public override void OnUpdate()
    {
      base.OnUpdate();

      if (isRunning)
      {
        floatVariable.Value = resultFloats[0];
        if (sliderComp != null) sliderComp.value = floatVariable.Value;
        if (spriteComp != null) spriteComp.fillAmount = floatVariable.Value;
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
