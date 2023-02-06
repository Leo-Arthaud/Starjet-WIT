//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	PoolKit PlayMaker Actions.cs
//	This script adds PoolKit Actions to PlayMaker
//
//	PoolKit For Unity, Created By Melli Georgiou
//	© 2018 Hell Tap Entertainment LTD
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using HellTap.PoolKit;

#if PLAYMAKER
namespace HutongGames.PlayMaker.Actions {

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//	CACHE / GET ACTIONS
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	// ============================================================================================================
	//	GET POOL
	//	Find a pool by name and store it in a variable
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Find a pool by name and store it in a variable.")]
	public class PoolKitGetPool : FsmStateAction 
	{

		[RequiredField]
		[Tooltip("Enter the name of the Pool to find.")]
		public FsmString findPool;

		//[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("This is the GameObject variable that will store the Pool.")]
		[CheckForComponent(typeof(HellTap.PoolKit.Pool))]
		public FsmGameObject storePool;

		// Reset -> Reset the used variables
		public override void Reset() {
			findPool = null;
			storePool = null;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// Cache Pool
			Pool pool = PoolKit.FindPool( findPool.Value );

			// Make sure we found the Pool
			if( pool != null ){

				storePool.Value = pool.gameObject;

			// Show warning message if something went wrong
			} else {
				Debug.LogWarning("POOLKIT PLAYMAKER: Could not get Pool because it could not be found!" );
			}

			// Done.
			Finish();
		}
	}

	// ============================================================================================================
	//	GET SPAWNER
	//	Find a Spawner by name and store it in a variable
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Find a Spawner by name and store it in a variable.")]
	public class PoolKitGetSpawner : FsmStateAction 
	{

		[RequiredField]
		[Tooltip("Enter the name of the Spawner to find.")]
		public FsmString findSpawner;

		//[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("This is the GameObject variable that will store the Spawner.")]
		[CheckForComponent(typeof(HellTap.PoolKit.Spawner))]
		public FsmGameObject storeSpawner;

		// Reset -> Reset the used variables
		public override void Reset() {
			findSpawner = null;
			storeSpawner = null;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// Cache Spawner
			Spawner spawner = PoolKit.FindSpawner( findSpawner.Value );

			// Make sure we found the Spawner
			if( spawner != null ){

				storeSpawner.Value = spawner.gameObject;

			// Show warning message if something went wrong
			} else {
				Debug.LogWarning("POOLKIT PLAYMAKER: Could not get Spawner because it could not be found!" );
			}

			// Done.
			Finish();
		}
	}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//	SPAWN INSTANCE ACTIONS
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	// ============================================================================================================
	//	SPAWN INSTANCE BY PREFAB
	//	Spawns a new instance by providing a prefab (and optionally a cached pool).
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Spawns a new instance by prefab.")]
	public class PoolKitSpawnInstanceByPrefab : FsmStateAction 
	{

		[RequiredField]
		[Tooltip("The prefab you want to make a new instance of.")]
		public FsmGameObject prefabToSpawn;

		[CheckForComponent(typeof(HellTap.PoolKit.Pool))]
		[Tooltip("Speed up spawning this instance by setting the reference of the Pool.")]
		[Title("Pool To Use (Optional)")]
		public FsmGameObject poolToUse;

		[UIHint(UIHint.Description)]
		public string setupDescription = "<b>Setup Instance</b>";
		
		[RequiredField]
		[Tooltip("The position where the new instance will be spawned.")]
		public FsmVector3 spawnPosition;

		[RequiredField]
		[Tooltip("The rotation of the new instance that will be spawned.")]
		public FsmQuaternion spawnRotation;

		//[RequiredField]
		[Tooltip("You can optionally choose to parent this instance to a new Transform.")]
		public FsmGameObject spawnParent;

		[RequiredField]
		[Tooltip("Should the instance be scaled when it is spawned?")]
		public FsmBool useScaling;

		[RequiredField]
		[Tooltip("The local scale of the new instance when spawned.")]
		public FsmVector3 spawnLocalScale;

		[UIHint(UIHint.Description)]
		public string outputDescription = "<b>Ouput</b>";

		//[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Set the new instance to a GameObject variable.")]
		public FsmGameObject storeInstance;

		// Reset -> Reset the used variables
		public override void Reset() {
			prefabToSpawn = null;
			poolToUse = null;
			spawnPosition = Vector3.zero;
			spawnRotation = Quaternion.identity;
			spawnLocalScale = Vector3.one;
			spawnParent = null;
		}
		
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// -----------------
			// PREPARE THE POOL
			// -----------------

			// Prepare a pool to be used
			Pool pool = null;

			// If the Pool is set, cache it ...
			if( poolToUse.Value != null ){ pool = poolToUse.Value.GetComponent<Pool>(); }

			// Otherwise, try to find it based on the the prefab
			else { pool = PoolKit.FindPoolContainingPrefab( prefabToSpawn.Value ); }

			// -------------------
			// SPAWN THE INSTANCE
			// -------------------

			// Make sure we found the Pool
			if( pool != null ){

				// Use Scaling
				if( useScaling.Value == true ){

					storeInstance.Value = pool.SpawnGO( 	prefabToSpawn.Value, 
															spawnPosition.Value, 
															spawnRotation.Value,
															spawnLocalScale.Value,
															spawnParent.Value ? spawnParent.Value.transform : null );
				// Don't Use Scaling
				} else {

					storeInstance.Value = pool.SpawnGO( 	prefabToSpawn.Value, 
															spawnPosition.Value, 
															spawnRotation.Value,
															spawnParent.Value ? spawnParent.Value.transform : null );
				}
			
			// Show warning message if something went wrong
			} else {
				Debug.LogWarning("POOLKIT PLAYMAKER: Could not spawn an instance because the required pool or prefab couldn't be found!" );
			}

			// Done.
			Finish();
		}
	}	

	// ============================================================================================================
	//	SPAWN INSTANCE BY NAME
	//	Spawns a new instance by providing the name of a prefab (and optionally a cached pool).
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Spawns a new instance by name.")]
	public class PoolKitSpawnInstanceByName : FsmStateAction 
	{

		[RequiredField]
		[Tooltip("The prefab you want to make a new instance of.")]
		public FsmString prefabNameToSpawn;

		[CheckForComponent(typeof(HellTap.PoolKit.Pool))]
		[Tooltip("Speed up spawning this instance by setting the reference of the Pool.")]
		[Title("Pool To Use (Optional)")]
		public FsmGameObject poolToUse;

		[UIHint(UIHint.Description)]
		public string setupDescription = "<b>Setup Instance</b>";
		
		[RequiredField]
		[Tooltip("The position where the new instance will be spawned.")]
		public FsmVector3 spawnPosition;

		[RequiredField]
		[Tooltip("The rotation of the new instance that will be spawned.")]
		public FsmQuaternion spawnRotation;

		//[RequiredField]
		[Tooltip("You can optionally choose to parent this instance to a new Transform.")]
		public FsmGameObject spawnParent;

		[RequiredField]
		[Tooltip("Should the instance be scaled when it is spawned?")]
		public FsmBool useScaling;

		[RequiredField]
		[Tooltip("The local scale of the new instance when spawned.")]
		public FsmVector3 spawnLocalScale;

		[UIHint(UIHint.Description)]
		public string outputDescription = "<b>Ouput</b>";

		//[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Set the new instance to a GameObject variable.")]
		public FsmGameObject storeInstance;

		// Reset -> Reset the used variables
		public override void Reset() {
			prefabNameToSpawn = null;
			poolToUse = null;
			spawnPosition = Vector3.zero;
			spawnRotation = Quaternion.identity;
			spawnLocalScale = Vector3.one;
			spawnParent = null;
		}
		
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// -----------------
			// PREPARE THE POOL
			// -----------------

			// Prepare a pool to be used
			Pool pool = null;

			// If the Pool is set, cache it ...
			if( poolToUse.Value != null ){ pool = poolToUse.Value.GetComponent<Pool>(); }

			// Otherwise, try to find it based on the the prefab
			else { pool = PoolKit.FindPoolContainingPrefab( prefabNameToSpawn.Value ); }

			// -------------------
			// SPAWN THE INSTANCE
			// -------------------

			// Make sure we found the Pool
			if( pool != null ){

				// Use Scaling
				if( useScaling.Value == true ){

					storeInstance.Value = pool.SpawnGO( 	prefabNameToSpawn.Value, 
															spawnPosition.Value, 
															spawnRotation.Value,
															spawnLocalScale.Value,
															spawnParent.Value ? spawnParent.Value.transform : null );
				// Don't Use Scaling
				} else {

					storeInstance.Value = pool.SpawnGO( 	prefabNameToSpawn.Value, 
															spawnPosition.Value, 
															spawnRotation.Value,
															spawnParent.Value ? spawnParent.Value.transform : null );
				}
			
			// Show warning message if something went wrong
			} else {
				Debug.LogWarning("POOLKIT PLAYMAKER: Could not spawn an instance because the required pool or prefab couldn't be found!" );
			}

			// Done.
			Finish();
		}
	}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//	DESPAWN ACTIONS
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	// ============================================================================================================
	//	DESPAWN INSTANCE
	//	Despawns an instance
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Despawn a specific instance.")]
	public class PoolKitDespawnInstance : FsmStateAction 
	{

		[RequiredField]
		[Tooltip("The gameObject you want to despawn.")]
		public FsmGameObject prefabToDespawn;

		[CheckForComponent(typeof(HellTap.PoolKit.Pool))]
		[Tooltip("Drastically speed up despawning this instance by setting the reference of the Pool.")]
		[Title("Pool To Use (Optional)")]
		public FsmGameObject poolToUse;

		// Reset -> Reset the used variables
		public override void Reset() {
			prefabToDespawn = null;
			poolToUse = null;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// -----------------
			// PREPARE THE POOL
			// -----------------

			// Prepare a pool to be used
			Pool pool = null;

			// If the Pool is set, cache it ...
			if( poolToUse.Value != null ){ pool = poolToUse.Value.GetComponent<Pool>(); }

			// Otherwise, try to find it based on the the instance
			else { pool = PoolKit.GetPoolContainingInstance( prefabToDespawn.Value ); }

			// ---------------------
			// DESPAWN THE INSTANCE
			// ---------------------

			// Make sure we found the Pool and the prefab to despawn is valid
			if( pool != null && prefabToDespawn.Value != null ){

				pool.Despawn( prefabToDespawn.Value );

			// Otherwise, show warnings ...
			} else {

				// If the Pool couldn't be found ...
				if( pool == null){ 
					Debug.LogWarning("POOLKIT PLAYMAKER: Could not despawn an instance because it's associated Pool couldn't be found" );
				}

				// If the Prefab is missing ...
				if( prefabToDespawn.Value == null){ 
					Debug.LogWarning("POOLKIT PLAYMAKER: Could not despawn an instance because it no longer exists!" );
				}

			}

			// Done.
			Finish();
		}
	}

	// ============================================================================================================
	//	DESPAWN ALL
	//	Despawns all instances across all pools
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Despawn all instances across all pools.")]
	public class PoolKitDespawnAllInstances : FsmStateAction 
	{
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			PoolKit.DespawnAll();

			// Done.
			Finish();
		}
	}

	// ============================================================================================================
	//	DESPAWN ALL INSTANCES IN A POOL
	//	Despawns an instance
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Despawns all instances controlled by a Pool.")]
	public class PoolKitDespawnAllInstancesInAPool : FsmStateAction 
	{

		[RequiredField]
		[CheckForComponent(typeof(HellTap.PoolKit.Pool))]
		[Tooltip("Select a Pool you want to despawn.")]
		[Title("Pool To Use (Optional)")]
		public FsmGameObject poolToUse;

		// Reset -> Reset the used variables
		public override void Reset() {
			poolToUse = null;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// ---------------------
			// DESPAWN THE POOL
			// ---------------------

			// Make sure the pool is valid
			if( poolToUse.Value != null && poolToUse.Value.GetComponent<Pool>() != null ){

				// Despawn the entire pool
				poolToUse.Value.GetComponent<Pool>().DespawnAll();

			// Otherwise, show warning ...
			} else {

				Debug.LogWarning("POOLKIT PLAYMAKER: Could not despawn a pool because it could not be found!" );
				
			}

			// Done.
			Finish();
		}
	}

	// ============================================================================================================
	//	DESPAWN ALL INSTANCES BY PREFAB
	//	Despawns all instances of a prefab
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Despawns all instances of a specific Prefab.")]
	public class PoolKitDespawnAllInstancesOfAPrefab: FsmStateAction 
	{

		[RequiredField]
		[Tooltip("The prefab of the instances you want to despawn.")]
		public FsmGameObject prefabToDespawn;

		[CheckForComponent(typeof(HellTap.PoolKit.Pool))]
		[Tooltip("Drastically speed up despawning by setting the reference of the Pool.")]
		[Title("Pool To Use (Optional)")]
		public FsmGameObject poolToUse;

		// Reset -> Reset the used variables
		public override void Reset() {
			prefabToDespawn = null;
			poolToUse = null;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// -----------------
			// PREPARE THE POOL
			// -----------------

			// Prepare a pool to be used
			Pool pool = null;

			// If the Pool is set, cache it ...
			if( poolToUse.Value != null ){ pool = poolToUse.Value.GetComponent<Pool>(); }

			// Otherwise, try to find it based on the the prefab
			else { pool = PoolKit.GetPoolContainingPrefab( prefabToDespawn.Value ); }

			// ---------------------
			// DESPAWN THE INSTANCE
			// ---------------------

			// Make sure we found the Pool and the prefab to despawn is valid
			if( pool != null && prefabToDespawn.Value != null ){

				pool.DespawnAll( prefabToDespawn.Value );

			// Otherwise, show warnings ...
			} else {

				// If the Pool couldn't be found ...
				if( pool == null){ 
					Debug.LogWarning("POOLKIT PLAYMAKER: Could not despawn all instances because it's associated Pool couldn't be found" );
				}

				// If the Prefab is missing ...
				if( prefabToDespawn.Value == null){ 
					Debug.LogWarning("POOLKIT PLAYMAKER: Could not despawn an instance because it no longer exists!" );
				}

			}

			// Done.
			Finish();
		}
	}

	// ============================================================================================================
	//	DESPAWN ALL INSTANCES BY PREFAB
	//	Despawns all instances of a prefab
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Despawns all instances of a specific Prefab By Name.")]
	public class PoolKitDespawnAllInstancesOfAPrefabByName: FsmStateAction 
	{

		[RequiredField]
		[Tooltip("The prefab name of the instances you want to despawn.")]
		public FsmString nameOfPrefab;

		[CheckForComponent(typeof(HellTap.PoolKit.Pool))]
		[Tooltip("Drastically speed up despawning by setting the reference of the Pool.")]
		[Title("Pool To Use (Optional)")]
		public FsmGameObject poolToUse;

		// Reset -> Reset the used variables
		public override void Reset() {
			nameOfPrefab = System.String.Empty;
			poolToUse = null;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// -----------------
			// PREPARE THE POOL
			// -----------------

			// Prepare a pool to be used
			Pool pool = null;

			// If the Pool is set, cache it ...
			if( poolToUse.Value != null ){ pool = poolToUse.Value.GetComponent<Pool>(); }

			// Otherwise, try to find it based on the the prefab name
			else { pool = PoolKit.GetPoolContainingPrefab( nameOfPrefab.Value ); }

			// ---------------------
			// DESPAWN THE INSTANCE
			// ---------------------

			// Make sure we found the Pool and the prefab to despawn is valid
			if( pool != null ){

				pool.DespawnAll( nameOfPrefab.Value );

			// Otherwise, show warnings ...
			} else {

				// If the Pool couldn't be found ...
				if( pool == null){ 
					Debug.LogWarning("POOLKIT PLAYMAKER: Could not despawn any instances because it's associated Pool couldn't be found." );
				}

				// If the Prefab is missing ...
				if( nameOfPrefab.Value == ""){ 
					Debug.LogWarning("POOLKIT PLAYMAKER: Could not despawn any instances because the prefab name is empty." );
				}

			}

			// Done.
			Finish();
		}
	}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//	DESPAWNER ACTIONS
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	// ============================================================================================================
	//	DESPAWNER TRIGGER
	//	Tell the despawner to trigger early and use chain-spawning
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Tell a despawner to despawn early (this also triggers chain-Spawning!)")]
	public class PoolKitDespawnerTrigger : FsmStateAction 
	{

		[RequiredField]
		[Tooltip("This is the GameObject variable that will store the Despawner.")]
		[CheckForComponent(typeof(HellTap.PoolKit.Despawner))]
		public FsmGameObject despawnerToUse;

		[RequiredField]
		[Tooltip("The delay before triggering the despawner")]
		[CheckForComponent(typeof(HellTap.PoolKit.Despawner))]
		public FsmFloat delay;

		// Reset -> Reset the used variables
		public override void Reset() {
			despawnerToUse = null;
			delay = 0f;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// If the Despawner is valid, trigger it!
			if( despawnerToUse.Value != null && despawnerToUse.Value.GetComponent<Despawner>() != null ){ 
				despawnerToUse.Value.GetComponent<Despawner>().Despawn( delay.Value ); 
			}

			// Done.
			Finish();
		}
	}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//	SPAWNER ACTIONS
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

	// ============================================================================================================
	//	SPAWNER PLAY
	//	Send an action to the Spawner
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Tell a Spawner To Start playing.")]
	public class PoolKitSpawnerPlay : FsmStateAction 
	{

		//[RequiredField]
		[Tooltip("This is the GameObject variable that will store the Spawner.")]
		[CheckForComponent(typeof(HellTap.PoolKit.Spawner))]
		public FsmGameObject spawnerToUse;

		//[RequiredField]
		[Tooltip("You can optionally find the Spawner at runtime instead of using a variable.")]
		public FsmString orFindSpawner;

		// Reset -> Reset the used variables
		public override void Reset() {
			spawnerToUse = null;
			orFindSpawner = System.String.Empty;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// --------------------
			// PREPARE THE SPAWNER
			// --------------------

			// Prepare a Spawner to be used
			Spawner spawner = null;

			// If the Spawner is set, cache it ...
			if( spawnerToUse.Value != null && spawnerToUse.Value.GetComponent<Spawner>() != null ){ 
				spawner = spawnerToUse.Value.GetComponent<Spawner>(); 
			}

			// Otherwise, try to find it based on the the Spawner name
			else { spawner = PoolKit.GetSpawner( orFindSpawner.Value ); }

			// --------------------
			// SEND THE ACTION
			// --------------------
			
			if( spawner != null ){
				spawner.Play();
			}

			// Done.
			Finish();
		}
	}	

	// ============================================================================================================
	//	SPAWNER STOP
	//	Send an action to the Spawner
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Tell a Spawner To Stop playing.")]
	public class PoolKitSpawnerStop : FsmStateAction 
	{

		//[RequiredField]
		[Tooltip("This is the GameObject variable that will store the Spawner.")]
		[CheckForComponent(typeof(HellTap.PoolKit.Spawner))]
		public FsmGameObject spawnerToUse;

		//[RequiredField]
		[Tooltip("You can optionally find the Spawner at runtime instead of using a variable.")]
		public FsmString orFindSpawner;

		// Reset -> Reset the used variables
		public override void Reset() {
			spawnerToUse = null;
			orFindSpawner = System.String.Empty;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// --------------------
			// PREPARE THE SPAWNER
			// --------------------

			// Prepare a Spawner to be used
			Spawner spawner = null;

			// If the Spawner is set, cache it ...
			if( spawnerToUse.Value != null && spawnerToUse.Value.GetComponent<Spawner>() != null ){ 
				spawner = spawnerToUse.Value.GetComponent<Spawner>(); 
			}

			// Otherwise, try to find it based on the the Spawner name
			else { spawner = PoolKit.GetSpawner( orFindSpawner.Value ); }

			// --------------------
			// SEND THE ACTION
			// --------------------
			
			if( spawner != null ){
				spawner.Stop();
			}

			// Done.
			Finish();
		}
	}

	// ============================================================================================================
	//	SPAWNER RESTART AND PLAY
	//	Send an action to the Spawner
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Tell a Spawner To restart and play.")]
	public class PoolKitSpawnerRestartAndPlay : FsmStateAction 
	{

		//[RequiredField]
		[Tooltip("This is the GameObject variable that will store the Spawner.")]
		[CheckForComponent(typeof(HellTap.PoolKit.Spawner))]
		public FsmGameObject spawnerToUse;

		//[RequiredField]
		[Tooltip("You can optionally find the Spawner at runtime instead of using a variable.")]
		public FsmString orFindSpawner;

		// Reset -> Reset the used variables
		public override void Reset() {
			spawnerToUse = null;
			orFindSpawner = System.String.Empty;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// --------------------
			// PREPARE THE SPAWNER
			// --------------------

			// Prepare a Spawner to be used
			Spawner spawner = null;

			// If the Spawner is set, cache it ...
			if( spawnerToUse.Value != null && spawnerToUse.Value.GetComponent<Spawner>() != null ){ 
				spawner = spawnerToUse.Value.GetComponent<Spawner>(); 
			}

			// Otherwise, try to find it based on the the Spawner name
			else { spawner = PoolKit.GetSpawner( orFindSpawner.Value ); }

			// --------------------
			// SEND THE ACTION
			// --------------------
			
			if( spawner != null ){
				spawner.RestartAndPlay();
			}

			// Done.
			Finish();
		}
	}

	// ============================================================================================================
	//	SPAWNER PAUSE
	//	Send an action to the Spawner
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Tell a Spawner to pause.")]
	public class PoolKitSpawnerPause : FsmStateAction 
	{

		//[RequiredField]
		[Tooltip("This is the GameObject variable that will store the Spawner.")]
		[CheckForComponent(typeof(HellTap.PoolKit.Spawner))]
		public FsmGameObject spawnerToUse;

		//[RequiredField]
		[Tooltip("You can optionally find the Spawner at runtime instead of using a variable.")]
		public FsmString orFindSpawner;

		// Reset -> Reset the used variables
		public override void Reset() {
			spawnerToUse = null;
			orFindSpawner = System.String.Empty;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// --------------------
			// PREPARE THE SPAWNER
			// --------------------

			// Prepare a Spawner to be used
			Spawner spawner = null;

			// If the Spawner is set, cache it ...
			if( spawnerToUse.Value != null && spawnerToUse.Value.GetComponent<Spawner>() != null ){ 
				spawner = spawnerToUse.Value.GetComponent<Spawner>(); 
			}

			// Otherwise, try to find it based on the the Spawner name
			else { spawner = PoolKit.GetSpawner( orFindSpawner.Value ); }

			// --------------------
			// SEND THE ACTION
			// --------------------
			
			if( spawner != null ){
				spawner.Pause();
			}

			// Done.
			Finish();
		}
	}

	// ============================================================================================================
	//	SPAWNER RESUME
	//	Send an action to the Spawner
	// ============================================================================================================

	[ActionCategory("PoolKit")]
	[Tooltip("Tell a Spawner to resume.")]
	public class PoolKitSpawnerResume : FsmStateAction 
	{

		//[RequiredField]
		[Tooltip("This is the GameObject variable that will store the Spawner.")]
		[CheckForComponent(typeof(HellTap.PoolKit.Spawner))]
		public FsmGameObject spawnerToUse;

		//[RequiredField]
		[Tooltip("You can optionally find the Spawner at runtime instead of using a variable.")]
		public FsmString orFindSpawner;

		// Reset -> Reset the used variables
		public override void Reset() {
			spawnerToUse = null;
			orFindSpawner = System.String.Empty;
		}
		
		// On Enter -> Run the Action
		public override void OnEnter(){

			// --------------------
			// PREPARE THE SPAWNER
			// --------------------

			// Prepare a Spawner to be used
			Spawner spawner = null;

			// If the Spawner is set, cache it ...
			if( spawnerToUse.Value != null && spawnerToUse.Value.GetComponent<Spawner>() != null ){ 
				spawner = spawnerToUse.Value.GetComponent<Spawner>(); 
			}

			// Otherwise, try to find it based on the the Spawner name
			else { spawner = PoolKit.GetSpawner( orFindSpawner.Value ); }

			// --------------------
			// SEND THE ACTION
			// --------------------
			
			if( spawner != null ){
				spawner.Resume();
			}

			// Done.
			Finish();
		}
	}

}
#endif
