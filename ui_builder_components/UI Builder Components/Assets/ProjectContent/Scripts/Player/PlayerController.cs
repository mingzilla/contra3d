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
    private int playerId;
    [SerializeField] Vector3 initialPosition = Vector3.one;

    private Rigidbody rb;
    private SkinnedMeshRenderer meshRenderer;
    private LayerMask groundLayers;
    private UserInput userInput = UserInput.Create(1);

    private Animator animatorCtrl;
    private static readonly int triggerJumpKey = Animator.StringToHash("triggerJump");
    private static readonly int triggerSwingKey = Animator.StringToHash("triggerSwing");
    private static readonly int isOnGroundKey = Animator.StringToHash("isOnGround");
    private static readonly int isMovingKey = Animator.StringToHash("isMoving");
    private readonly IntervalState takeDamageInterval = IntervalState.Create(1f);

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
        PlayerActionHandler3D.MoveXZ(userInput, rb, playerAttribute.moveSpeed);
        animatorCtrl.SetBool(isMovingKey, userInput.IsMoving());
        UnitDisplayHandler3D.HandleXZFacing(transform, userInput);

        bool isOnGround = UnityFn.IsOnGround(transform.position, playerAttribute.playerToGroundDistance, groundLayers);
        animatorCtrl.SetBool(isOnGroundKey, isOnGround);
        if (userInput.jump && isOnGround) animatorCtrl.SetTrigger(triggerJumpKey);
        if (userInput.jump) PlayerActionHandler3D.HandleJumpFromGround(isOnGround, rb, playerAttribute.jumpForce);
        PlayerActionHandler3D.HandleGravityModification(rb, playerAttribute.gravityMultiplier);

        // if (userInput.fire1) SpawnBullets(playerAttribute.weaponType, userInput);
        if (userInput.fire1) animatorCtrl.SetTrigger(triggerSwingKey);

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
    }

    public void KeyRB(InputAction.CallbackContext context)
    {
    }

    public void KeyLT(InputAction.CallbackContext context)
    {
    }

    public void KeyRT(InputAction.CallbackContext context)
    {
    }

    public void KeyboardAnyKey(InputAction.CallbackContext context)
    {
    }


    private void UpdatePadInput(InputAction.CallbackContext context, GameInputKey key)
    {
        GameInputAction action = mapping.GetGamePlayPadAction(key);
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
            if (context.started) userInput.fire1 = true;
        }
    }
}