using System;
using Game.Player.AnimatorScripts;
using Game.Player.Interfaces;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        public InputSystemPC InputSystem { get; private set; }
        public IPlayerAim PlayerAim { get; private set; }
        public AnimatorController AnimatorController { get; private set; }
        public CharacterController CharacterController { get; private set; }
        [Inject] public PlayerConfigs PlayerConfigs { get; private set; }
        [Inject] public StateHandleChain StateChain { get; private set; }

        private InitializationStateMachine _initializationStateMachine;
        
        
        [Inject]
        private void Construct(IPlayerAim playerAim, InputSystemPC inputSystemPC, AnimatorController animatorController, InitializationStateMachine stateMachine)
        {
            PlayerAim = playerAim;
            InputSystem = inputSystemPC;
            AnimatorController = animatorController;

            CharacterController = GetComponent<CharacterController>();

            _initializationStateMachine = stateMachine;
        }

        private void Update()
        {
            if(!_initializationStateMachine.PlayerStateMachine.isUpdate) return;
            
            _initializationStateMachine.PlayerStateMachine.currentStates.OnUpdateBehaviour();
        }
    }
}