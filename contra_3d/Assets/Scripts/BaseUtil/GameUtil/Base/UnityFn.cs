using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace BaseUtil.GameUtil.Base
{
    /**
     * General function handling objects from Unity Engine.
     */
    public static class UnityFn
    {
        public static GameObject GetParent(GameObject obj)
        {
            if (obj == null) return null;
            if (obj.transform == null) return null;
            if (obj.transform.parent == null) return null;
            return obj.transform.parent.gameObject;
        }

        /**
         * If parent matches tag, return parent, otherwise return self.
         */
        public static GameObject GetTaggedParentOrSelf(GameObject obj, string tag)
        {
            if (obj == null) return null;
            if (obj.CompareTag(tag)) return obj;
            GameObject parentObj = GetParent(obj);

            // max of 5 searches - unity crashes if using a while loop here to 
            for (int i = 0; i < 5; i++)
            {
                if (parentObj == null) return null;
                if (parentObj.CompareTag(tag)) return parentObj;
                parentObj = GetParent(obj);
            }

            return obj;
        }

        public static void HandleTaggedParentOrSelf(GameObject obj, string rootTag, Action<GameObject> destroyFn)
        {
            if (obj == null) return;
            GameObject taggedObj = GetTaggedParentOrSelf(obj, rootTag);
            if (taggedObj != null) destroyFn(taggedObj);
            if (taggedObj == null) destroyFn(obj);
        }

        public static void SafeDestroy(GameObject obj)
        {
            if (obj == null) return;
            Object.Destroy(obj);
        }

        public static void SafeDestroy(GameObject obj, float lifeTime)
        {
            if (obj == null) return;
            Object.Destroy(obj, lifeTime);
        }

        /// <summary>
        /// Used inside GameManagerController.Start(), so that mouse input is disabled, and players can't use mouse click on buttons
        /// </summary>
        public static void PreventMouseEventAtGameManagerStart()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public static void CreateEffect(GameObject effectPreFab, Vector3 position, float lifeTime)
        {
            if (effectPreFab != null)
            {
                GameObject copy = Object.Instantiate(effectPreFab, position, Quaternion.identity); // Quaternion.identity means no rotation
                SafeDestroy(copy, lifeTime);
            }
        }

        public static bool IsInRange(Transform unit, Transform target, float range)
        {
            if (unit == null) return false;
            if (target == null) return false;
            return Vector3.Distance(unit.position, target.position) <= range;
        }

        /**
         * Desire rotation means this is the expected rotation. 
         * If currently it's not this value, it takes time * turnSpeed to get to this desire rotation.
         */
        public static Quaternion GetDesireRotation2D(Transform unit, Transform target)
        {
            Vector3 direction = unit.position - target.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            return rotation;
        }

        /**
         * Rotation of this frame when turning unit towards target
         */
        public static Quaternion GetRotation2D(Transform unit, Transform target, float turnSpeed, float deltaTime)
        {
            // for more info, refer to unity course flyer section e.g. L35
            Quaternion desireRotation = GetDesireRotation2D(unit, target);
            return Quaternion.Slerp(unit.rotation, desireRotation, turnSpeed * deltaTime);
        }

        public static Quaternion GetImmediateRotation3D(Vector3 unitPosition, Vector3 targetPosition)
        {
            return Quaternion.LookRotation(targetPosition - unitPosition);
        }

        /// <summary>
        /// Rotate towards target, without looking up or down (rotates Y axis only)
        /// </summary>
        public static Quaternion LookXZ(Transform unit, Transform target)
        {
            if (!target) return unit.rotation;
            Quaternion rotation = Quaternion.LookRotation(target.position - unit.position);
            unit.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
            return unit.rotation;
        }

        /// <summary>
        /// Rotate towards target, allow looking up or down
        /// </summary>
        public static Quaternion LookXYZ(Transform unit, Transform target)
        {
            if (!target) return unit.rotation;
            unit.rotation = Quaternion.LookRotation(target.position - unit.position);
            return unit.rotation;
        }

        public static Quaternion LookX(Transform unit, bool toRightSide)
        {
            Vector3 targetPosition = toRightSide ? new Vector3(1f, 0f, 0f) : new Vector3(-1f, 0f, 0f);
            Quaternion rotation = Quaternion.LookRotation(targetPosition);
            unit.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
            return unit.rotation;
        }

        /**
         * Used for e.g. health bar in 3D inside LateUpdate() loop, so that it always faces the player.
         */
        public static void LookAtPlayer(Transform unit, Transform camera)
        {
            unit.LookAt(unit.position + camera.forward);
        }

        /**
         * Position of this frame when moving unit towards target
         */
        public static Vector3 GetPosition(Transform unit, Transform target, float moveSpeed, float deltaTime)
        {
            return Vector3.MoveTowards(unit.position, target.position, moveSpeed * deltaTime);
        }

        /**
         * Position of this frame when moving unit towards target, excluding Y, so that unit doesn't move vertically
         */
        public static Vector3 GetPositionXZ(Transform unit, Transform target, float moveSpeed, float deltaTime)
        {
            if (!target) return unit.position;
            float originalY = unit.position.y;
            Vector3 position = GetPosition(unit, target, moveSpeed, deltaTime);
            return new Vector3(position.x, originalY, position.z);
        }

        /// <summary>
        /// Provides the delta for e.g. bullet movement when only knowing the shooting point and destination, considering bullet won't stop at destination.  
        /// </summary>
        /// <returns>Position delta, which can be added to currentPosition and targetPosition. Adding to target is generally needed so that the bullet never stop at the original target</returns>
        public static Vector3 GetFramePositionDelta(Vector3 currentPosition, Vector3 targetPosition, float moveSpeed, float deltaTime)
        {
            Vector3 nextPosition = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * deltaTime);
            return nextPosition - currentPosition;
        }

        public static List<Transform> FindChildrenWithTag(Transform transform, string tagName)
        {
            var children = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.gameObject.CompareTag(tagName))
                {
                    children.Add(child);
                }
            }
            return children;
        }

        public static List<GameObject> FindChildObjectsWithTag(Transform transform, string tagName)
        {
            List<Transform> children = FindChildrenWithTag(transform, tagName);
            return Fn.Map((x) => x.gameObject, children);
        }

        public static List<T> FindComponentsFromChildrenWithTag<T>(Transform transform, string tagName)
        {
            List<Transform> children = FindChildrenWithTag(transform, tagName);
            return Fn.Map((x) => x.GetComponent<T>(), children);
        }

        public static float SpeedX(Rigidbody2D theRb)
        {
            return Mathf.Abs(theRb.velocity.x);
        }

        public static List<SpriteRenderer> EnableSpriteRenderer(List<SpriteRenderer> spriteRenderers)
        {
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.enabled = true;
            }
            return spriteRenderers;
        }

        public static List<SpriteRenderer> ToggleSpriteRenderer(List<SpriteRenderer> spriteRenderers)
        {
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.enabled = !sr.enabled;
            }
            return spriteRenderers;
        }

        /// <summary>
        /// Won't work if time is stopped (e.g. game is paused)
        /// IMPORTANT:
        /// If controller is AppResource.instance, intervalState needs to be reset using intervalReset observable. Otherwise intervalState.canRun is false so it stops functional.
        /// Use "this" (preferred) if each object needs to have it's own interval. If using AppResource.instance, every object share the same interval (because AppResource is the one to track timer)
        /// 1) Used inside Update loop. If interval is 3, fn runs every 3 seconds. 
        /// 2) Used in events, to allow executing fn only once only within an interval. 
        /// </summary>
        public static void RunWithInterval(MonoBehaviour controller, IntervalState intervalState, Action fn)
        {
            if (!intervalState.canRun) return;
            intervalState.canRun = false;
            fn();
            SetTimeout(controller, intervalState.interval, () => intervalState.canRun = true);
        }

        /// <summary>
        /// IMPORTANT:
        /// If controller is AppResource.instance, intervalState needs to be reset using intervalReset observable. Otherwise intervalState.canRun is false so it stops functional.
        /// Use "this" (preferred) if each object needs to have it's own interval. If using AppResource.instance, every object share the same interval (because AppResource is the one to track timer)
        /// When using "this" as controller, need to safe check first, because object may have been destroyed when timeout content is run
        /// When using a global controller, may get error of accessing destroyed object even when checking e.g. isBroken, because the coroutine is run by something else
        /// </summary>
        public static void RepeatWithInterval(MonoBehaviour controller, IntervalState intervalState, int repeatTimes, float repeatDelay, Action fn)
        {
            RunWithInterval(controller, intervalState, () =>
            {
                SetTimeoutWithRepeat(controller, repeatTimes, repeatDelay, fn);
            });
        }

        /// <returns>The coroutine that can be used to stop the timeout event</returns>
        public static IEnumerator SetTimeout(MonoBehaviour controller, float delay, Action fn)
        {
            IEnumerator coroutine = TimeOutDelayFn(delay, fn);
            Fn.SafeRun(() => controller.StartCoroutine(coroutine));
            return coroutine; // return a copy so that it can be stopped if needed
        }

        /// <summary>
        /// Run fn repeatedly, with specified repeat times and repeat delay
        /// </summary>
        public static void SetTimeoutWithRepeat(MonoBehaviour controller, int repeatTimes, float repeatDelay, Action fn)
        {
            foreach (int i in Enumerable.Range(0, repeatTimes))
            {
                SetTimeout(controller, i * repeatDelay, fn);
            }
        }

        private static IEnumerator TimeOutDelayFn(float delay, Action fn)
        {
            yield return new WaitForSeconds(delay);
            fn();
        }

        public static IEnumerator WaitUntil(MonoBehaviour controller, Func<bool> conditionFn, Action fn)
        {
            IEnumerator coroutine = WaitUntilDelayFn(conditionFn, fn);
            controller.StartCoroutine(coroutine); // run if condition returns true
            return coroutine; // return a copy so that it can be stopped if needed
        }

        private static IEnumerator WaitUntilDelayFn(Func<bool> conditionFn, Action fn)
        {
            yield return new WaitUntil(conditionFn);
            fn();
        }

        public static T GetClosestTarget<T>(Vector3 source, List<T> targets, Func<T, Vector3> getPositionFn)
        {
            int count = targets.Count;
            if (count == 0) throw new ArgumentException("Need to have at least one target");

            T closestTarget = targets[0];
            float shortestDistance = Vector3.Distance(source, getPositionFn(targets[0]));
            foreach (T target in targets)
            {
                float distance = Vector3.Distance(source, getPositionFn(target));
                if (distance < shortestDistance)
                {
                    closestTarget = target;
                    shortestDistance = distance;
                }
            }
            return closestTarget;
        }

        public static Vector3 GetMeanVector3(List<Vector3> positions)
        {
            int count = positions.Count;
            if (count == 0) return Vector3.zero;
            if (count == 1) return positions[0];
            float x = 0f;
            float y = 0f;
            float z = 0f;
            foreach (Vector3 pos in positions)
            {
                x += pos.x;
                y += pos.y;
                z += pos.z;
            }
            return new Vector3(x / count, y / count, z / count);
        }

        public static Vector3 GetMaxVector3Distance(List<Vector3> positions)
        {
            int count = positions.Count;
            if (count == 0) return Vector3.zero;
            if (count == 1) return positions[0];

            float minX = positions.Min(it => it.x);
            float minY = positions.Min(it => it.y);
            float minZ = positions.Min(it => it.z);
            float maxX = positions.Max(it => it.x);
            float maxY = positions.Max(it => it.y);
            float maxZ = positions.Max(it => it.z);

            return new Vector3((maxX - minX), (maxY - minY), (maxZ - minZ));
        }

        /**
         * Keeps a controller as singleton and make sure the gameObject is not destroyed when going to another scene.
         * When the next scene is loaded, an object with such mark won't run OnEnable() or Start()
         */
        public static void MarkSingletonAndKeepAlive<T>(T instance, GameObject gameObj, Action setInstanceToThisFn)
        {
            if (instance == null)
            {
                setInstanceToThisFn();
                KeepAlive(gameObj);
            }
            else
            {
                Object.Destroy(gameObj);
            }
        }

        public static void KeepAlive(GameObject gameObj)
        {
            gameObj.transform.parent = null; // only root level object can be set to not destroyable
            Object.DontDestroyOnLoad(gameObj);
        }

        public static T InstantiateCharacterObject<T>(GameObject prefab, bool isActive, Vector3 initialPosition)
        {
            GameObject obj = Object.Instantiate(prefab, initialPosition, Quaternion.identity);
            if (!isActive) obj.SetActive(false);
            return obj.GetComponent<T>();
        }

        public static T InstantiateObjectWith<T>(GameObject prefab, Vector3 position)
        {
            GameObject obj = Object.Instantiate(prefab, position, Quaternion.identity);
            return obj.GetComponent<T>();
        }

        public static void SetActive(GameObject gameObject, Action beforeSetActiveFn = null)
        {
            if (!gameObject.activeSelf)
            {
                beforeSetActiveFn?.Invoke();
                gameObject.SetActive(true);
            }
        }

        public static void SetInactive(GameObject gameObject, Action beforeSetInactiveFn = null)
        {
            if (gameObject.activeSelf)
            {
                beforeSetInactiveFn?.Invoke();
                gameObject.SetActive(false);
            }
        }

        public static void FastSetActive(GameObject gameObject, bool value)
        {
            if (gameObject == null) return;
            if (gameObject.activeSelf != value)
            {
                gameObject.SetActive(value);
            }
        }

        public static void SetAllActivate(List<GameObject> gameObjectsToSetInactive)
        {
            foreach (GameObject item in gameObjectsToSetInactive)
            {
                SetActive(item, Fn.DoNothing);
            }
        }

        public static void SetControllersActive<T>(IEnumerable<T> controllers, bool isActive) where T : MonoBehaviour
        {
            foreach (T controller in controllers)
            {
                if (controller) FastSetActive(controller.gameObject, isActive);
            }
        }

        public static void SetControllerActive<T>(T controller, bool isActive) where T : MonoBehaviour
        {
            if (controller) FastSetActive(controller.gameObject, isActive);
        }

        public static void DestroyReferenceIfPresent(MonoBehaviour controller, Action postFn)
        {
            if (controller != null)
            {
                Object.Destroy(controller.gameObject);
                postFn();
            }
        }

        public static void DeActivateReferenceIfPresent(MonoBehaviour controller, Action postFn)
        {
            if (controller != null)
            {
                SetInactive(controller.gameObject);
                postFn();
            }
        }

        /// <summary>
        /// Only deactivate others when setting the desire object from inactive to active.
        /// </summary>
        /// <param name="gameObjectToSetActive"></param>
        /// <param name="gameObjectsToSetInactive"></param>
        public static void SetActiveAndDeActivateOthers(GameObject gameObjectToSetActive, List<GameObject> gameObjectsToSetInactive)
        {
            SetActive(gameObjectToSetActive, () =>
            {
                SetAllInactivate(gameObjectsToSetInactive);
            });
        }

        public static void SetAllInactivate(List<GameObject> gameObjectsToSetInactive)
        {
            foreach (GameObject item in gameObjectsToSetInactive)
            {
                SetInactive(item, Fn.DoNothing);
            }
        }

        public static Rigidbody GetOrAddRigidbody(GameObject gameObject, bool useGravity, bool freezeZ)
        {
            Rigidbody existingRb = gameObject.GetComponent<Rigidbody>();
            Rigidbody rb = existingRb ? existingRb : gameObject.AddComponent<Rigidbody>();
            rb.useGravity = useGravity;
            if (freezeZ) rb.constraints = (RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation);
            return rb;
        }

        /// <summary>
        /// Apply this on a controlled character to prevent jittery.
        /// </summary>
        public static Rigidbody GetOrAddInterpolateRigidbody(GameObject gameObject, bool useGravity, bool freezeZ)
        {
            Rigidbody rb = GetOrAddRigidbody(gameObject, useGravity, freezeZ);
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            return rb;
        }

        public static SphereCollider AddSphereCollider(GameObject gameObject, float radius, bool isTrigger)
        {
            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.radius = radius;
            collider.isTrigger = isTrigger;
            return collider;
        }

        public static CapsuleCollider AddCapsuleCollider(GameObject gameObject, float radius, float height, bool isTrigger)
        {
            CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
            collider.radius = radius;
            collider.height = height;
            collider.isTrigger = isTrigger;
            return collider;
        }

        public static T AddCollider<T>(GameObject gameObject, bool isTrigger) where T : Collider
        {
            T collider = gameObject.AddComponent<T>();
            collider.isTrigger = isTrigger;
            return collider;
        }

        public static void RemoveForce(Rigidbody rb)
        {
            rb.velocity = Vector3.zero; // remove collision force
            rb.angularVelocity = Vector3.zero;
        }

        public static T MakeInvisible<T>(GameObject gameObject) where T : Renderer
        {
            T meshRenderer = gameObject.GetComponent<T>();
            meshRenderer.enabled = false;
            return meshRenderer;
        }

        public static T AddNoFrictionCollider<T>(GameObject gameObject) where T : Collider
        {
            T collider = gameObject.AddComponent<T>();
            collider.material = CreateNoFrictionMaterial();
            return collider;
        }

        public static T AddNoFrictionMaterialToCollider<T>(GameObject gameObject) where T : Collider
        {
            T collider = gameObject.GetComponent<T>();
            collider.material = CreateNoFrictionMaterial();
            return collider;
        }

        public static T AddFrictionMaterialToCollider<T>(GameObject gameObject) where T : Collider
        {
            T collider = gameObject.GetComponent<T>();
            collider.material = CreateFrictionMaterial();
            return collider;
        }

        public static PhysicMaterial CreateNoFrictionMaterial()
        {
            PhysicMaterial material = new PhysicMaterial();
            material.dynamicFriction = 0f;
            material.staticFriction = 0f;
            material.bounciness = 0f;
            material.frictionCombine = PhysicMaterialCombine.Minimum;
            return material;
        }

        public static PhysicMaterial CreateFrictionMaterial()
        {
            PhysicMaterial material = new PhysicMaterial();
            material.dynamicFriction = 1f;
            material.staticFriction = 1f;
            material.bounciness = 0f;
            material.frictionCombine = PhysicMaterialCombine.Maximum;
            return material;
        }

        /// <summary>
        /// Apply force once only to throw an object. This is not used in Update() loop to continuously apply force. 
        /// </summary>
        /// <param name="rb">Of the object to throw</param>
        /// <param name="xForce">positive goes right</param>
        /// <param name="yForce">positive goes up</param>
        /// <param name="zForce">positive goes into</param>
        public static void Throw(Rigidbody rb, float xForce, float yForce, float zForce)
        {
            Vector3 forceToAdd = new Vector3(xForce, yForce, zForce);
            rb.AddRelativeForce(forceToAdd, ForceMode.Impulse); // Impulse is used to apply force once only, so don't put it in Update
        }

        public static void RotateOverTimeWithRandomRotation(Transform transform, float rotationSpeed)
        {
            Random random = new Random();
            bool x = FnVal.RandomBool(random);
            bool y = FnVal.RandomBool(random);
            bool z = FnVal.RandomBool(random);
            RotateOverTime(transform, x, y, z, rotationSpeed);
        }

        public static void RotateOverTime(Transform transform, bool x, bool y, bool z, float rotationSpeed)
        {
            float speed = rotationSpeed * Time.deltaTime;
            float xAngle = x ? speed : 0;
            float yAngle = y ? speed : 0;
            float zAngle = z ? speed : 0;
            transform.Rotate(xAngle, yAngle, zAngle);
        }

        public static void LoadNextScene()
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            if (index < sceneCount) LoadScene(index + 1);
        }

        public static void LoadScene(int index)
        {
            UnPause(); // game can be paused when a player decides to load a scene, this makes sure time is not stopped
            SceneManager.LoadScene(index);
        }

        public static void ReloadCurrentScene()
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            LoadScene(index);
        }

        /// <summary>
        /// Destroy all players and quit to menu
        /// </summary>
        /// <typeparam name="T">Player Controller, which represents user input control</typeparam>
        public static void QuitToMenu<T>(int menuSceneIndex = 0) where T : MonoBehaviour
        {
            T[] objects = Object.FindObjectsOfType<T>();
            Fn.EachInArray(x => Object.Destroy(x.gameObject), objects);
            LoadScene(menuSceneIndex);
        }

        public static string[] GetSceneNamesInBuildSettings()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            string[] scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = GetSceneNameByIndexInBuildSettings(i);
            }
            return scenes;
        }

        public static string GetSceneNameByIndexInBuildSettings(int index)
        {
            return Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(index));
        }

        public static Dictionary<string, int> GetSceneNameAndIndexDictInBuildSettings()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            string[] scenes = GetSceneNamesInBuildSettings();
            for (int i = 0; i < scenes.Length; i++)
            {
                dictionary[scenes[i]] = i;
            }
            return dictionary;
        }

        /// <summary>
        /// Move to button and highlight it
        /// </summary>
        public static void MoveToButton(Button button)
        {
            button.Select(); // Or EventSystem.current.SetSelectedGameObject(myButton.gameObject) - Select Button
            button.OnSelect(null); // Or myButton.OnSelect(new BaseEventData(EventSystem.current)) - Highlight Button
        }

        public static void DeSelectButtons(List<Button> buttons)
        {
            buttons.ForEach(b => b.OnDeselect(null));
        }

        /// <summary>
        /// Buttons need to have onClick.AddListener(), then this can trigger the click event
        /// </summary>
        public static void TriggerButton(Button button)
        {
            button.onClick.Invoke();
        }

        public static void Pause()
        {
            Time.timeScale = 0f;
        }

        public static void UnPause()
        {
            Time.timeScale = 1f;
        }

        /**
         * @param blastRange generally needs to be 2. Too small may not overlap with other layer so may not trigger damage.
         */
        public static void DealDamage(Vector3 position, float blastRange, LayerMask affectedLayers, Action<GameObject> damagingFn)
        {
            Collider[] affectedObjects = Physics.OverlapSphere(position, blastRange, affectedLayers, QueryTriggerInteraction.Ignore);
            foreach (var item in affectedObjects)
            {
                damagingFn(item.gameObject);
            }
        }

        /// <param name="playerPosition"></param>
        /// <param name="playerToGroundDistance">Not visible, so need to create an empty object on the UI, and calculate the distance to adjust</param>
        /// <param name="groundLayers"></param>
        /// <returns></returns>
        public static bool IsOnGround(Vector3 playerPosition, float playerToGroundDistance, LayerMask groundLayers)
        {
            Vector3 position = new Vector3(playerPosition.x, playerPosition.y - playerToGroundDistance, playerPosition.z);
            // check: within circle close to groundPoint, is there any ground
            // .2f is a good value 
            return Physics.CheckSphere(position, .2f, groundLayers);
        }

        public static void HandleJump(Rigidbody rb, float jumpForce)
        {
            Vector3 velocity = rb.velocity;
            rb.velocity = new Vector3(velocity.x, jumpForce, velocity.z);
        }

        /// <summary>
        /// Change current to target over smoothTime
        /// </summary>
        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, float smoothTime)
        {
            Vector3 smoothVelocity = Vector3.zero;
            return Vector3.SmoothDamp(current, target, ref smoothVelocity, smoothTime);
        }

        /// <summary>
        /// When using materials as an array, setting to a position doesn't work, need to replace an element and use a whole new array
        /// </summary>
        public static Material[] UpdateMaterialAt(Material[] originalItems, int index, Material materialToUse)
        {
            return Fn.MapArrayWithIndex((x, i) => (i == index) ? materialToUse : x, originalItems);
        }
    }
}