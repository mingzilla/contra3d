using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContent.Scripts.AppLiveResource;
using ProjectContent.Scripts.Data;
using ProjectContent.Scripts.Player.Actions;
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
    private UserInput userInput;
    private Animator animatorCtrl;

    private PlayerMoveAction moveAction;
    private PlayerJumpAction jumpAction;
    private PlayerDashAction dashAction;
    private PlayerAttackAction attackAction;

    private readonly IntervalState takeDamageInterval = IntervalState.Create(1f);

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
        userInput = UserInput.Create(playerId);
        gameStoreData.AddPlayer(playerInput);

        moveAction = PlayerMoveAction.Create(animatorCtrl, rb);
        jumpAction = PlayerJumpAction.Create(animatorCtrl, rb);
        dashAction = PlayerDashAction.Create(animatorCtrl, rb);
        attackAction = PlayerAttackAction.Create(animatorCtrl, rb, swordMesh, staffMesh);
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

        playerAttribute = moveAction.Perform(playerAttribute, userInput, transform, isOnGround);
        playerAttribute = jumpAction.Perform(playerAttribute, userInput, isOnGround);
        playerAttribute = dashAction.Perform(playerAttribute, userInput);
        playerAttribute = attackAction.Perform(playerAttribute, userInput);

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
            moveAction.moveSpeedModifier.ToggleIdle(true);
            attackAction.playerWeaponState = PlayerWeaponState.STAFF;
        }
        if (context.canceled)
        {
            userInput.isHoldingRt = false;
            moveAction.moveSpeedModifier.ToggleIdle(false);
            attackAction.playerWeaponState = PlayerWeaponState.NONE;
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
                attackAction.playerWeaponState = PlayerWeaponState.SWORD;
                userInput.swing = true;
                moveAction.moveSpeedModifier.TemporarilyApplyModifier(this);
            }
        }
        if (action == GameInputAction.DASH)
        {
            if (context.started)
            {
                UnityFn.RunWithInterval(this, dashAction.dashInterval, () =>
                {
                    attackAction.playerWeaponState = PlayerWeaponState.NONE;
                    userInput.dash = true;
                    moveAction.moveSpeedModifier.CancelModifier(this);
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