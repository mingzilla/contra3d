using System;
using System.Collections;
using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Types;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public static GameObject GetUnitRootOrSelf(GameObject obj)
        {
            return GetTaggedParentOrSelf(obj, GameTag.UNIT_ROOT.name);
        }

        public static T GetComponentFromUnitRootOrSelf<T>(GameObject obj)
        {
            return GetUnitRootOrSelf(obj).GetComponent<T>();
        }

        public static void SetActiveOnTaggedRootOrSelf(GameObject obj, bool isActive)
        {
            HandleTaggedParentOrSelf(obj, GameTag.UNIT_ROOT.name, (o) => o.SetActive(isActive));
        }

        /**
         * Unit Root is used to destroy a unit, because generally for grouping purposes, a unit is put inside an empty object.
         * So we tag the root object as Unit Root, and we destroy that object rather than the child only.
         */
        public static void DestroyTaggedRootOrSelf(GameObject obj)
        {
            HandleTaggedParentOrSelf(obj, GameTag.UNIT_ROOT.name, SafeDestroy);
        }

        public static void DestroyRootObjectOverTime(GameObject obj, float lifeTime)
        {
            HandleTaggedParentOrSelf(obj, GameTag.UNIT_ROOT.name, (o) => SafeDestroy(o, lifeTime));
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

        public static void SetRotation3D(Transform unit, Transform target)
        {
            // makes sure unit faces target, in 3D
            unit.LookAt(target);
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

        public static void SetTimeout(MonoBehaviour controller, float delay, Action fn)
        {
            controller.StartCoroutine(TimeOutDelayFn(delay, fn));
        }

        private static IEnumerator TimeOutDelayFn(float delay, Action fn)
        {
            yield return new WaitForSeconds(delay);
            fn();
        }

        public static void WaitUntil(MonoBehaviour controller, Func<bool> conditionFn, Action fn)
        {
            controller.StartCoroutine(WaitUntilDelayFn(conditionFn, fn)); // run if condition returns true
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

        /**
         * Keeps a controller as singleton and make sure the gameObject is not destroyed when going to another scene.
         */
        public static void MarkSingletonAndKeepAlive<T>(T instance, GameObject gameObj, Action setInstanceToThisFn)
        {
            if (instance == null)
            {
                setInstanceToThisFn();
                gameObj.transform.parent = null; // only root level object can be set to not destroyable
                Object.DontDestroyOnLoad(gameObj);
            }
            else
            {
                Object.Destroy(gameObj);
            }
        }

        public static T InstantiateDisabledCharacterObject<T>(GameObject prefab)
        {
            GameObject obj = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            return obj.GetComponent<T>();
        }

        public static void SetActive(GameObject gameObject, Action beforeSetActiveFn)
        {
            if (!gameObject.activeSelf)
            {
                beforeSetActiveFn();
                gameObject.SetActive(true);
            }
        }

        public static void SetInactive(GameObject gameObject, Action beforeSetInactiveFn)
        {
            if (gameObject.activeSelf)
            {
                beforeSetInactiveFn();
                gameObject.SetActive(false);
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
                foreach (GameObject item in gameObjectsToSetInactive)
                {
                    SetInactive(item, Fn.DoNothing);
                }
            });
        }

        public static Rigidbody AddRigidBodyAndFreezeZ(GameObject gameObject)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = (RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation);
            return rb;
        }

        public static T AddCollider<T>(GameObject gameObject, bool isTrigger) where T : Collider
        {
            T collider = gameObject.AddComponent<T>();
            collider.isTrigger = isTrigger;
            return collider;
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

        public static PhysicMaterial CreateNoFrictionMaterial()
        {
            PhysicMaterial material = new PhysicMaterial();
            material.dynamicFriction = 0f;
            material.staticFriction = 0f;
            material.bounciness = 0f;
            material.frictionCombine = PhysicMaterialCombine.Minimum;
            return material;
        }
    }
}