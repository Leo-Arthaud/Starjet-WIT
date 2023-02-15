#if PLAYMAKER

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[Tooltip("Destroys multiple GameObjects with NGUI components (removes the object from the parent's hirarchy before destroying). " 
         + "If the specified GameObjects doesn't have any NGUI component attached, they are being destroyed the normal way. "
         + "Optionally reposition afterwards and/or only destroy the children of the GameObjects.")]
	public class NGUIDestroyMulti : FsmStateAction
	{
		[RequiredField]
		[ArrayEditor(VariableType.GameObject)]
		[Tooltip("The GameObjects to destroy.")]
		public FsmGameObject[] gameObjects;

		[Tooltip("Reposition any UITable or UIGrid the GameObjects are children of.")]
		public FsmBool reposition;

		[Tooltip("Only destroys the children of the specified GameObjects.")]
		public FsmBool onlyDestroyChildren;

		public override void Reset()
		{
			gameObjects = new FsmGameObject[1];
			reposition = true;
			onlyDestroyChildren = false;
		}

		public override void OnEnter()
		{
			if (gameObjects.Length > 0) DestroyMulti();

			Finish();
		}

		private void DestroyMulti() {
		  foreach (var go in gameObjects)
      {
        if (go.Value == null) continue;

        GameObject currParent = null;
        Transform trans = go.Value.transform;

        if (trans.parent == null)
        {
          reposition.Value = false;
          Log("Couldn't reposition \"" + go.Value.name + "\" since it has no parent.");
        }
        else currParent = trans.parent.gameObject;

        if (go.Value.GetComponent(typeof(UIWidgetContainer)) == null)
        {
          Log("GameObject \"" + go.Value.name + "\" doesn't contain any NGUI Component. Destroying it the normal way...");

          if (onlyDestroyChildren.Value)
          {
            for (int i = 0; i < trans.childCount; i++)
              GameObject.Destroy(trans.GetChild(i).gameObject);
          }
          else GameObject.Destroy(go.Value);
        }
        else
        {
          if (onlyDestroyChildren.Value) trans.DestroyChildren();
          else NGUITools.Destroy(go.Value);
        }

        //reposition Table or Grid if prevalent
        if (currParent != null && reposition.Value)
        {
          UITable mTable = NGUITools.FindInParents<UITable>(currParent);
          if (mTable != null) mTable.repositionNow = true;

          UIGrid mGrid = NGUITools.FindInParents<UIGrid>(currParent);
          if (mGrid != null) mGrid.repositionNow = true;
        }
      }
		}
	}
}

#endif