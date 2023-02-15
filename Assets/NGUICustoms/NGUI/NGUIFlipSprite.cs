#if PLAYMAKER

namespace HutongGames.PlayMaker.Actions
{
  [ActionCategory("NGUI")]
  [Tooltip("Changes the flip drection of a UISprite component.")]
  public class NGUIFlipSprite : FsmStateAction
  {
    [RequiredField]
    [CheckForComponent(typeof(UISprite))]
    [Tooltip("The target GameObject with the UISprite component attached.")]
    public FsmOwnerDefault target;

    [Tooltip("The flip direction of the UISprite component.")]
    public UIBasicSprite.Flip flipDirection;
    
    [Tooltip("Whether to flip the sprite in the opposite direction it's currently in (essentially toggling it).")]
    public FsmBool flipOpposite;
    
    [Tooltip("Specify whether it should apply the specified flip direction. Useful if you wan't to toggle the direction with a bool variable. When disabled, resets the flip direction of the sprite to 'Nothing'. Set this field to 'None' to prevent this from happening.")]
    public FsmBool doFlip;

    [Tooltip("Select a random flip direction.")]
    public FsmBool random;

    [Tooltip("Apply changes every frame.")]
    public FsmBool everyFrame;

    private UIBasicSprite.Flip chosenFlipDirection;

    public override void Reset()
    {
      target = null;
      flipDirection = UIBasicSprite.Flip.Horizontally;
      flipOpposite = false;
      doFlip = true;
      random = false;
      everyFrame = false;
    }

    public override void OnEnter()
    {
      DoSetFlipDirection();

      if (!everyFrame.Value) Finish();
    }

    public override void OnUpdate()
    {
      DoSetFlipDirection();
    }

    private void DoSetFlipDirection()
    {
      var _go = Fsm.GetOwnerDefaultTarget(target);
      if (_go == null) return;

      UISprite nSprite = _go.GetComponent<UISprite>();

      chosenFlipDirection = random.Value ? RandomEnum<UIBasicSprite.Flip>() : flipDirection;

      if (flipOpposite.Value)
      {
        if (!doFlip.Value || nSprite.flip == chosenFlipDirection && nSprite.flip != UIBasicSprite.Flip.Nothing) nSprite.flip = UIBasicSprite.Flip.Nothing;
        else nSprite.flip = chosenFlipDirection;
      }
      else nSprite.flip = doFlip.Value ? chosenFlipDirection : UIBasicSprite.Flip.Nothing;
      
      nSprite.MarkAsChanged();
    }

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, target.GameObject) + " -> " + (random.Value ? "Random" : System.Enum.GetName(flipDirection.GetType(), flipDirection));
    }
#endif
    
    private static T RandomEnum<T>()
    {
      var val = System.Enum.GetValues(typeof (T));
      return (T)val.GetValue(UnityEngine.Random.Range(0, val.Length));
    }
  }
}

#endif