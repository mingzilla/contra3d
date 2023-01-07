using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using BaseUtil.GameUtil.Util3D;
using ProjectContent.Scripts.Data;
using ProjectContent.Scripts.Types;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameInputMapping mapping = GameInputMapping.Create();
    private PlayerAttribute playerAttribute = PlayerAttribute.CreateEmpty(1);

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
    private static readonly int isOnGroundKey = Animator.StringToHash("isOnGround");
    private static readonly int isMovingKey = Animator.StringToHash("isMoving");
    private readonly IntervalState takeDamageInterval = IntervalState.Create(1f);

    private readonly MoveSpeedModifier moveSpeedModifier = MoveSpeedModifier.Create();
    private PlayerWeaponState playerWeaponState = PlayerWeaponState.NONE;

    void Start()
    {
        playerId = 1;
        gameObject.layer = GameLayer.PLAYER.GetLayer();
        UnityFn.AddNoFrictionMaterialToCollider<CapsuleCollider>(gameObject.GetComponent<CapsuleCollider>().gameObject);
        rb = UnityFn.GetOrAddInterpolateRigidbody(gameObject, true, false);
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // for player only
        groundLayers = GameLayer.GetGroundLayerMask();
        animatorCtrl = gameObject.GetComponentInChildren<Animator>();
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        if (rb != null) HandlePlayerControl(userInput);
        UserInput.ResetTriggers(userInput);
    }

    private void HandlePlayerControl(UserInput userInput)
    {
        // UnitDisplayHandler3D.HandleInvincibility(meshRenderer, takeDamageInterval);
        bool isOnGround = UnityFn.IsOnGround(transform.position, playerAttribute.playerToGroundDistance, groundLayers);

        PlayerActionHandler3D.FighterMoveXZ(userInput, rb, playerAttribute.moveSpeed, moveSpeedModifier.speedModifier, isOnGround);
        animatorCtrl.SetBool(isMovingKey, userInput.IsMoving());
        UnitDisplayHandler3D.HandleXZFacing(transform, userInput);

        animatorCtrl.SetBool(isOnGroundKey, isOnGround);
        if (userInput.jump && isOnGround) animatorCtrl.SetTrigger(triggerJumpKey);
        if (userInput.jump) PlayerActionHandler3D.HandleJumpFromGround(isOnGround, rb, playerAttribute.jumpForce);
        PlayerActionHandler3D.HandleGravityModification(rb, playerAttribute.gravityMultiplier);

        PlayerWeaponState.HandleWeaponVisibility(playerWeaponState, swordMesh, staffMesh);
        if (userInput.swing) animatorCtrl.SetTrigger(triggerSwingKey);
        if (userInput.IsUsingMagic()) animatorCtrl.SetTrigger(triggerMagicKey);

        playerAttribute.inGameTransform = transform;
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
        if (context.started) userInput.isHoldingRt = true;
        if (context.canceled) userInput.isHoldingRt = false;
    }

    public void KeyboardAnyKey(InputAction.CallbackContext context)
    {
    }


    private void UpdatePadInput(InputAction.CallbackContext context, GameInputKey key)
    {
        GameInputAction action = mapping.GetGamePlayPadAction(key, userInput.isHoldingRb);
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
        if (action is {isMagic: true})
        {
            if (context.started)
            {
                playerWeaponState = PlayerWeaponState.STAFF;
                userInput = GameInputAction.UpdateMagicCommand(userInput, action);
                moveSpeedModifier.TemporarilyApplyModifier(this);
            }
        }
    }
}