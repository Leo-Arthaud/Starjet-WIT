#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Checks if a NGUI component is visible on Screen.")]
	public class NGUIIsVisible : FsmStateActionUpdate
	{
		[RequiredField]
		[Tooltip("GameObject with a UIWidget to check for visibility.")]
		[CheckForComponent(typeof(UIWidget))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[CheckForComponent(typeof(Camera))]
		[Tooltip("The Camera that's supposed to see the sprite.")]
		public FsmGameObject camera;

		[Tooltip("Event to send if the Sprite is visible.")]
		public FsmEvent trueEvent;

		[Tooltip("Event to send if the Sprite is not visible.")]
		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a bool variable.")]
		public FsmBool storeResult;

		public override void Reset()
		{
			base.Reset();

			gameObject = null;
			camera = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoCheck();

			if(updateType == UpdateType.Once) Finish();
		}

		public override void OnActionUpdate()
		{
			DoCheck();
		}

		private void DoCheck()
		{
			//exit if sprite is null
			if(gameObject == null) return;

			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

			bool isVisible = false;
			
			Camera targetCam = camera.Value.GetComponent<Camera>();

			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(targetCam);
			Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(go.transform);

			if(GeometryUtility.TestPlanesAABB(planes, bounds)) isVisible = true;

			storeResult.Value = isVisible;
			Fsm.Event(isVisible ? trueEvent : falseEvent);
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, gameObject.GameObject);
    }
#endif
	}
}

#endif