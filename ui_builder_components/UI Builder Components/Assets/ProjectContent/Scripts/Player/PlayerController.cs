using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using BaseUtil.GameUtil.Util3D;
using ProjectContent.Scripts.AppLiveResource;
using ProjectContent.Scripts.Data;
using ProjectContent.Scripts.Types;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private readonly GameInputMapping inputMapping = GameInputMapping.Create();
    private AppResource appResource;
    private GameStoreData gameStoreData;

    [SerializeField] private Vector3 initialPosition = Vector3.one;
    [SerializeField] private GameObject swordMesh;
    [SerializeField] private GameObject staffMesh;

    private int playerId;

    private Rigidbody rb;
    private SkinnedMeshRenderer meshRenderer;
    private LayerMask groundLayers;
    private UserInput userInput = UserInput.Create(1);

    private Animator animatorCtrl;
    private static readonly int triggerJumpKey = Animator.StringToHash("triggerJump");
    private static readonly int triggerSwingKey = Animator.StringToHash("triggerSwing");
    private static readonly int triggerMagicKey = Animator.StringToHash("triggerMagic");
    private static readonly int triggerRollKey = Animator.StringToHash("triggerRoll");
    private static readonly int isOnGroundKey = Animator.StringToHash("isOnGround");
    private static readonly int isMovingKey = Animator.StringToHash("isMoving");
    private readonly IntervalState takeDamageInterval = IntervalState.Create(1f);
    private readonly IntervalState dashInterval = IntervalState.Create(0.2f);

    public float dashForce = 15f;

    private readonly MoveSpeedModifier moveSpeedModifier = MoveSpeedModifier.Create();
    private PlayerWeaponState playerWeaponState = PlayerWeaponState.NONE;

    void Start()
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
        PlayerInput playerInput = gameObject.GetComponent<PlayerInput>();
        playerId = playerInput.playerIndex;
        gameStoreData.AddPlayer(playerInput);
    }

    void FixedUpdate()
    {
        if (rb != null) HandlePlayerControl(userInput);
        UserInput.ResetTriggers(userInput);
    }

    private void HandlePlayerControl(UserInput userInput)
    {
        PlayerAttribute playerAttribute = gameStoreData.GetPlayer(playerId);
        // UnitDisplayHandler3D.HandleInvincibility(meshRenderer, takeDamageInterval);
        bool isOnGround = UnityFn.IsOnGround(transform.position, playerAttribute.playerToGroundDistance, groundLayers);

        PlayerActionHandler3D.FighterMoveXZ(userInput, rb, playerAttribute.moveSpeed, moveSpeedModifier.speedModifier, isOnGround);
        animatorCtrl.SetBool(isMovingKey, moveSpeedModifier.CanUseMoveAnimation() && userInput.IsMoving());
        UnitDisplayHandler3D.HandleXZFacing(transform, userInput);

        animatorCtrl.SetBool(isOnGroundKey, isOnGround);
        if (userInput.jump && isOnGround) animatorCtrl.SetTrigger(triggerJumpKey);
        if (userInput.jump) PlayerActionHandler3D.HandleJumpFromGround(isOnGround, rb, playerAttribute.jumpForce);
        PlayerActionHandler3D.HandleGravityModification(rb, playerAttribute.gravityMultiplier);

        if (userInput.dash) UnityFn.HandleDash(rb, dashForce);
        if (userInput.dash) animatorCtrl.SetTrigger(triggerRollKey);

        PlayerWeaponState.HandleWeaponVisibility(playerWeaponState, swordMesh, staffMesh);
        if (userInput.swing) animatorCtrl.SetTrigger(triggerSwingKey);
        if (userInput.IsUsingMagic()) animatorCtrl.SetTrigger(triggerMagicKey);

        playerAttribute.inGameTransform = transform;
        gameStoreData.SetPlayer(playerAttribute);
    }

    /*------------------------------------------*/

    public void InputMove(InputAction.CallbackContext context)
    {
        userInput = UserInput.Move(userInput, context);
    }

    public void KeySelect(InputAction.CallbackContext context)
    {
        // if (context.started)
        // if (context.canceled)
    }

    public void KeyStart(InputAction.CallbackContext context)
    {
    }

    public void KeyA(InputAction.CallbackContext context)
    {
        UpdatePadInput(context, GameInputKey.A);
    }

    public void KeyB(InputAction.CallbackContext context)
    {
        UpdatePadInput(context, GameInputKey.B);
    }

    public void KeyX(InputAction.CallbackContext context)
    {
        UpdatePadInput(context, GameInputKey.X);
    }

    public void KeyY(InputAction.CallbackContext context)
    {
        UpdatePadInput(context, GameInputKey.Y);
    }

    public void KeyPadUp(InputAction.CallbackContext context)
    {
    }

    public void KeyPadDown(InputAction.CallbackContext context)
    {
    }

    public void KeyPadLeft(InputAction.CallbackContext context)
    {
    }

    public void KeyPadRight(InputAction.CallbackContext context)
    {
    }

    public void KeyLB(InputAction.CallbackContext context)
    {
        if (context.started) userInput.isHoldingLb = true;
        if (context.canceled) userInput.isHoldingLb = false;
    }

    public void KeyRB(InputAction.CallbackContext context)
    {
        if (context.started) userInput.isHoldingRb = true;
        if (context.canceled) userInput.isHoldingRb = false;
    }

    public void KeyLT(InputAction.CallbackContext context)
    {
        if (context.started) userInput.isHoldingLt = true;
        if (context.canceled) userInput.isHoldingLt = false;
    }

    public void KeyRT(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            userInput.isHoldingRt = true;
            moveSpeedModifier.ToggleIdle(true);
            playerWeaponState = PlayerWeaponState.STAFF;
        }
        if (context.canceled)
        {
            userInput.isHoldingRt = false;
            moveSpeedModifier.ToggleIdle(false);
            playerWeaponState = PlayerWeaponState.NONE;
        }
    }

    public void KeyboardAnyKey(InputAction.CallbackContext context)
    {
    }


    private void UpdatePadInput(InputAction.CallbackContext context, GameInputKey key)
    {
        GameInputAction action = inputMapping.GetGamePlayPadAction(key, userInput.isHoldingRt);
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
                playerWeaponState = PlayerWeaponState.SWORD;
                userInput.swing = true;
                moveSpeedModifier.TemporarilyApplyModifier(this);
            }
        }
        if (action == GameInputAction.DASH)
        {
            UnityFn.RunWithInterval(this, dashInterval, () =>
            {
                if (context.started)
                {
                    playerWeaponState = PlayerWeaponState.NONE;
                    userInput.dash = true;
                    moveSpeedModifier.CancelModifier(this);
                }
            });
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