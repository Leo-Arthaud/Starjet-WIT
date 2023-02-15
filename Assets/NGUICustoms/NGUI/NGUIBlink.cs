#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Turns a UIWidget on/off in a regular repeating pattern by switching between its initial and no alpha value.")]
	public class NGUIBlink : FsmStateAction
	{
		[RequiredField]
    [CheckForComponent(typeof(UIWidget))]
    [Tooltip("The GameObject with the UIWidget component to blink on/off.")]
		public FsmOwnerDefault gameObject;

		[HasFloatSlider(0, 5)]
    [Tooltip("Time to stay off in seconds.")]
		public FsmFloat timeOff;
		
    [HasFloatSlider(0, 5)]
    [Tooltip("Time to stay on in seconds.")]
    public FsmFloat timeOn;
    
    [Tooltip("Total duration in seconds. Set to 'None' to keep blinking till the action has been exited.")]
    public FsmFloat duration;

    [Tooltip("Should the widget start in the visible state?")]
    public FsmBool startState;
    
    [Tooltip("Should the widget stop in the visible state?")]
		public FsmBool stopState;
		
		[Tooltip("Send an event after given Duration has passed.")]
		public FsmEvent finishEvent;

		[Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public FsmBool realTime;

		private UIWidget widget;
		private float startTime, timer, totalStartTime, totalTimer, initAlpha = 1f;
		private bool blinkOn;
		
		public override void Reset()
		{
			gameObject = null;
			timeOff = 0.05f;
			timeOn = 0.05f;
			duration = new FsmFloat { UseVariable = true };
			startState = new FsmBool { UseVariable = true };
			stopState = new FsmBool { UseVariable = true };
			finishEvent = null;
			realTime = false;
		}
	
		public override void OnEnter()
		{
		  var go = Fsm.GetOwnerDefaultTarget(gameObject);

		  if(go == null) {
		    LogError("GameObject is null!");
		    return;
		  }

		  widget = go.GetComponent<UIWidget>();
		  if(widget != null) initAlpha = widget.alpha;

		  totalStartTime = startTime = FsmTime.RealtimeSinceStartup;
		  totalTimer = timer = 0f;
		  
		  if (duration.Value <= 0)
		  {
			  Fsm.Event(finishEvent);
			  Finish();
			  return;
		  }
			
			if (!startState.Value) UpdateBlinkState(startState.Value);
		}

		public override void OnExit()
		{
		  if (!stopState.IsNone) UpdateBlinkState(stopState.Value);
		}
		
		public override void OnUpdate()
		{
			//update time
			if (realTime.Value)
			{
				totalTimer = FsmTime.RealtimeSinceStartup - totalStartTime;
				timer = FsmTime.RealtimeSinceStartup - startTime;
			}
			else
			{
				totalTimer += Time.deltaTime;
				timer += Time.deltaTime;
			}
			
			if (totalTimer >= duration.Value)
			{
				Fsm.Event(finishEvent);
				Finish();
				return;
			}
			
			//update blinkage
			if (blinkOn && timer > timeOn.Value) UpdateBlinkState(false);
			if (blinkOn == false && timer > timeOff.Value) UpdateBlinkState(true);
		}
			
		private void UpdateBlinkState(bool state)
		{
			widget.alpha = state ? initAlpha : 0f;
			
			blinkOn = state;
			
			// reset timer
			startTime = FsmTime.RealtimeSinceStartup;
			timer = 0f;
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
	    if (Fsm == null || gameObject == null) return "";
    
	    var go = Fsm.GetOwnerDefaultTarget(gameObject);
	    string name = go == null ? "<null>" : go.name;
      return GetType().Name + " - " + name + " : " + timeOn + " <-> " + timeOff;
    }
#endif
	}
}

#endif