#if PLAYMAKER

namespace HutongGames.PlayMaker.Actions
{
	public abstract class FsmStateActionUpdate : FsmStateAction
	{
    //Enum for toggling the method that is supposed to run on the derived class.
		public enum UpdateType {
			Once,
			OnUpdate,
			OnLateUpdate,
			OnFixedUpdate
		};

    //Variable for setting and retrieving the current UpdateType.
		[ActionSection("Update Type")]
		[Title("Execute")]
		public UpdateType updateType;


		public abstract void OnActionUpdate();

		public override void Reset()
		{
			updateType = UpdateType.Once;
		}

		public override void OnPreprocess()
		{
			if(updateType == UpdateType.OnFixedUpdate) Fsm.HandleFixedUpdate = true;

#if PLAYMAKER_1_8_5_OR_NEWER
			if(updateType == UpdateType.OnLateUpdate) Fsm.HandleLateUpdate = true;
#endif
		}

		public override void OnUpdate()
		{
			if(updateType == UpdateType.OnUpdate) OnActionUpdate();
			if(updateType == UpdateType.Once) Finish();
		}

		public override void OnLateUpdate()
		{
			if(updateType == UpdateType.OnLateUpdate) OnActionUpdate();
			if(updateType == UpdateType.Once) Finish();
		}

		public override void OnFixedUpdate()
		{
			if(updateType == UpdateType.OnFixedUpdate) OnActionUpdate();
			if(updateType == UpdateType.Once) Finish();
		}
	}
}

#endif