#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Iterates through all children of a given parent GameObject and disables all GameObjects which UIWidgets aren't " +
	         "visible by the given Camera and vice versa, so that only those are enabled, that are actually in sight.")]
	public class NGUIOnlyShowVisible : FsmStateActionUpdate
	{
		[RequiredField]
		[Tooltip("Parent GameObject with the children to check for visibility.")]
		public FsmOwnerDefault parent;

		[RequiredField]
		[CheckForComponent(typeof(Camera))]
		[Tooltip("The Camera that's supposed to see the sprites.")]
		public FsmGameObject camera;

		public override void Reset()
		{
			base.Reset();

			parent = null;
			camera = null;
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
			GameObject parentGO = Fsm.GetOwnerDefaultTarget(parent);

      for(int i = 0; i < parentGO.transform.childCount; i++) {
        GameObject nextChild = parentGO.transform.GetChild(i).gameObject;
        Camera targetCam = camera.Value.GetComponent<Camera>();

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(targetCam);
        Bounds bounds = NGUIMath.CalculateAbsoluteWidgetBounds(nextChild.transform);

        nextChild.SetActive(GeometryUtility.TestPlanesAABB(planes, bounds));
      }
		}

#if UNITY_EDITOR
    public override string AutoName()
    {
      return ActionHelpers.AutoName(this, parent.GameObject);
    }
#endif
	}
}

#endif