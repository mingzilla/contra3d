using ProjectContent.Scripts.AppLiveResource;
using ProjectContent.Scripts.Data;
using ProjectContent.Scripts.Player.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectContent.Scripts.Player
{
    public class PlayerMono : MonoBehaviour, IControllable
    {
        private readonly GameInputMapping inputMapping = GameInputMapping.Create();
        private AppResource appResource;
        private GameStoreData gameStoreData;

        [SerializeField] public Vector3 initialPosition = Vector3.one;
        [SerializeField] public GameObject swordMesh;
        [SerializeField] public GameObject staffMesh;
        [SerializeField] public GameObject fireballPrefab;
        [SerializeField] public GameObject lighteningPrefab;
        [SerializeField] public GameObject iceSplashPrefab;
        [SerializeField] public string control;

        private int playerId;

        private ControlContext controlContext; 
        private PlayerInGameControl playerInGameControl;
        private PlayerSkillPanelControl playerSkillPanelControl;
        private AbstractControllable activeControl;

        void Start()
        {
            appResource = AppResource.instance;
            gameStoreData = appResource.storeData;
            PlayerInput playerInput = gameObject.GetComponent<PlayerInput>();
            playerId = playerInput.playerIndex;
            gameStoreData.AddPlayer(playerInput);

            controlContext = ControlContext.GetByName(control);
            playerInGameControl = PlayerInGameControl.Create(this, playerId);
            playerSkillPanelControl = PlayerSkillPanelControl.Create(this, playerId);
            SetActiveControl(controlContext);
        }

        /// <summary>
        /// Can be changed by activeControl instances
        /// </summary>
        public void SetActiveControl(ControlContext contextIn)
        {
            controlContext = contextIn;
            if (controlContext == ControlContext.IN_GAME) activeControl = playerInGameControl;
            if (controlContext == ControlContext.SKILL_PANEL) activeControl = playerSkillPanelControl;
        }

        void FixedUpdate()
        {
            activeControl.FixedUpdate();
        }

        /*------------------------------------------*/

        public void InputMove(InputAction.CallbackContext context)
        {
            activeControl.InputMove(context);
        }

        public void KeySelect(InputAction.CallbackContext context)
        {
            activeControl.KeySelect(context);
        }

        public void KeyStart(InputAction.CallbackContext context)
        {
            activeControl.KeyStart(context);
        }

        public void KeyA(InputAction.CallbackContext context)
        {
            activeControl.KeyA(context);
        }

        public void KeyB(InputAction.CallbackContext context)
        {
            activeControl.KeyB(context);
        }

        public void KeyX(InputAction.CallbackContext context)
        {
            activeControl.KeyX(context);
        }

        public void KeyY(InputAction.CallbackContext context)
        {
            activeControl.KeyY(context);
        }

        public void KeyPadUp(InputAction.CallbackContext context)
        {
            activeControl.KeyPadUp(context);
        }

        public void KeyPadDown(InputAction.CallbackContext context)
        {
            activeControl.KeyPadDown(context);
        }

        public void KeyPadLeft(InputAction.CallbackContext context)
        {
            activeControl.KeyPadLeft(context);
        }

        public void KeyPadRight(InputAction.CallbackContext context)
        {
            activeControl.KeyPadRight(context);
        }

        public void KeyLB(InputAction.CallbackContext context)
        {
            activeControl.KeyLB(context);
        }

        public void KeyRB(InputAction.CallbackContext context)
        {
            activeControl.KeyRB(context);
        }

        public void KeyLT(InputAction.CallbackContext context)
        {
            activeControl.KeyLT(context);
        }

        public void KeyRT(InputAction.CallbackContext context)
        {
            activeControl.KeyRT(context);
        }

        public void KeyboardAnyKey(InputAction.CallbackContext context)
        {
            activeControl.KeyboardAnyKey(context);
        }
    }
}