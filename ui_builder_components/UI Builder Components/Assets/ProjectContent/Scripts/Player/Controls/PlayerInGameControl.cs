using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContent.Scripts.AppLiveResource;
using ProjectContent.Scripts.Data;
using ProjectContent.Scripts.Player.Actions;
using ProjectContent.Scripts.Types;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectContent.Scripts.Player.Controls
{
    public class PlayerInGameControl : AbstractControllable
    {
        private readonly GameInputMapping inputMapping = GameInputMapping.Create();
        private AppResource appResource;
        private GameStoreData gameStoreData;

        [SerializeField] private Vector3 initialPosition = Vector3.one;
        [SerializeField] private GameObject swordMesh;
        [SerializeField] private GameObject staffMesh;
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private GameObject lighteningPrefab;
        [SerializeField] private GameObject iceSplashPrefab;

        private Rigidbody rb;
        private SkinnedMeshRenderer meshRenderer;
        private LayerMask groundLayers;
        private Animator animatorCtrl;

        private PlayerMoveAction moveAction;
        private PlayerJumpAction jumpAction;
        private PlayerDashAction dashAction;
        private PlayerAttackAction attackAction;

        private readonly IntervalState takeDamageInterval = IntervalState.Create(1f);

        public static PlayerInGameControl Create(PlayerMono mono, int playerId)
        {
            PlayerInGameControl item = BaseCreate<PlayerInGameControl>(mono, playerId);
            // SerializeField
            item.swordMesh = mono.swordMesh;
            item.staffMesh = mono.staffMesh;
            item.fireballPrefab = mono.fireballPrefab;
            item.lighteningPrefab = mono.lighteningPrefab;
            item.iceSplashPrefab = mono.iceSplashPrefab;
            item.Start();
            return item;
        }

        protected override void Start()
        {
            appResource = AppResource.instance;
            gameStoreData = appResource.storeData;
            gameObject.layer = GameLayer.PLAYER.GetLayer();
            UnityFn.AddNoFrictionMaterialToCollider<CapsuleCollider>(gameObject.GetComponent<CapsuleCollider>().gameObject);
            rb = UnityFn.GetOrAddInterpolateRigidbody(gameObject, true, false);
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // for player only
            groundLayers = GameLayer.GetGroundLayerMask();
            animatorCtrl = gameObject.GetComponentInChildren<Animator>();
            transform.position = initialPosition;
            gameObject.SetActive(true);

            moveAction = PlayerMoveAction.Create(animatorCtrl, rb);
            jumpAction = PlayerJumpAction.Create(animatorCtrl, rb);
            dashAction = PlayerDashAction.Create(animatorCtrl, rb);
            attackAction = PlayerAttackAction.Create(animatorCtrl, rb,
                swordMesh, staffMesh,
                fireballPrefab, lighteningPrefab, iceSplashPrefab);
        }

        public override void FixedUpdate()
        {
            if (rb != null) HandlePlayerControl(userInput);
            UserInput.ResetTriggers(userInput);
        }

        private void HandlePlayerControl(UserInput userInput)
        {
            PlayerAttribute playerAttribute = gameStoreData.GetPlayer(playerId);
            // UnitDisplayHandler3D.HandleInvincibility(meshRenderer, takeDamageInterval);
            bool isOnGround = UnityFn.IsOnGround(transform.position, playerAttribute.playerToGroundDistance, groundLayers);

            playerAttribute = moveAction.Perform(playerAttribute, userInput, transform, isOnGround);
            playerAttribute = jumpAction.Perform(playerAttribute, userInput, isOnGround);
            playerAttribute = dashAction.Perform(playerAttribute, userInput);
            playerAttribute = attackAction.Perform(playerAttribute, transform, new Vector3(0, 0.5f, 0), userInput);

            playerAttribute.inGameTransform = transform;
            gameStoreData.SetPlayer(playerAttribute);
        }

        /*------------------------------------------*/

        public override void InputMove(InputAction.CallbackContext context)
        {
            userInput = UserInput.Move(userInput, context);
        }

        public override void KeySelect(InputAction.CallbackContext context)
        {
            mono.SetActiveControl(ControlContext.SKILL_PANEL);
        }

        public override void KeyStart(InputAction.CallbackContext context)
        {
        }

        public override void KeyA(InputAction.CallbackContext context)
        {
            UpdatePadInput(context, GameInputKey.A);
        }

        public override void KeyB(InputAction.CallbackContext context)
        {
            UpdatePadInput(context, GameInputKey.B);
        }

        public override void KeyX(InputAction.CallbackContext context)
        {
            UpdatePadInput(context, GameInputKey.X);
        }

        public override void KeyY(InputAction.CallbackContext context)
        {
            UpdatePadInput(context, GameInputKey.Y);
        }

        public override void KeyPadUp(InputAction.CallbackContext context)
        {
        }

        public override void KeyPadDown(InputAction.CallbackContext context)
        {
        }

        public override void KeyPadLeft(InputAction.CallbackContext context)
        {
        }

        public override void KeyPadRight(InputAction.CallbackContext context)
        {
        }

        public override void KeyLB(InputAction.CallbackContext context)
        {
            if (context.started) userInput.isHoldingLb = true;
            if (context.canceled) userInput.isHoldingLb = false;
        }

        public override void KeyRB(InputAction.CallbackContext context)
        {
            if (context.started) userInput.isHoldingRb = true;
            if (context.canceled) userInput.isHoldingRb = false;
        }

        public override void KeyLT(InputAction.CallbackContext context)
        {
            if (context.started) userInput.isHoldingLt = true;
            if (context.canceled) userInput.isHoldingLt = false;
        }

        public override void KeyRT(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                userInput.isHoldingRt = true;
                moveAction.moveSpeedModifier.ToggleIdle(mono, true);
                attackAction.playerWeaponState = PlayerWeaponState.STAFF;
            }
            if (context.canceled)
            {
                userInput.isHoldingRt = false;
                moveAction.moveSpeedModifier.ToggleIdle(mono, false);
                attackAction.playerWeaponState = PlayerWeaponState.NONE;
            }
        }

        public override void KeyboardAnyKey(InputAction.CallbackContext context)
        {
        }


        private void UpdatePadInput(InputAction.CallbackContext context, GameInputKey key)
        {
            GameInputAction action = inputMapping.GetInGameGamePadAction(key, userInput.isHoldingRt);
            UpdateInput(context, action);
        }

        private void UpdateInput(InputAction.CallbackContext context, GameInputAction action)
        {
            if (action == GameInputAction.JUMP)
            {
                if (context.started) userInput.jump = true;
                if (context.canceled) userInput.jumpCancelled = true;
            }
            if (action == GameInputAction.ATTACK)
            {
                if (context.started)
                {
                    attackAction.playerWeaponState = PlayerWeaponState.SWORD;
                    userInput.swing = true;
                    moveAction.moveSpeedModifier.TemporarilyApplyModifier(mono);
                }
            }
            if (action == GameInputAction.DASH)
            {
                if (context.started)
                {
                    UnityFn.RunWithInterval(mono, dashAction.dashInterval, () =>
                    {
                        attackAction.playerWeaponState = PlayerWeaponState.NONE;
                        userInput.dash = true;
                        moveAction.moveSpeedModifier.CancelModifier(mono);
                    });
                }
            }
            if (action is {isMagic: true})
            {
                if (context.started)
                {
                    userInput = GameInputAction.UpdateMagicCommand(userInput, action);
                }
            }
        }
    }
}