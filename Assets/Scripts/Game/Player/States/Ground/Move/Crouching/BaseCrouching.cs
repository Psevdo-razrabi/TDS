using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.StateHandle;
using UnityEngine;

namespace Game.Player.States.Crouching
{
    public class BaseCrouching : BaseMove
    {
        public BaseCrouching(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
            
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            CheckPlaceToCrouch(Player.PlayerConfigs.CrouchingPlace);
            Player.StateChain.HandleState<PlayerSitDownCrouchHandle>();
        }

        private void CheckPlaceToCrouch(CrouchingPlace place)
        {
            var transformPlayerStep = Player.PlayerStep.transform;
            if (Physics.Raycast(transformPlayerStep.position, transformPlayerStep.forward, place.DistanceToCrouchPlace,
                    place.LayerMaskObjectToCrouch) && Data.IsPlayerSitDown == false)
            {
                Data.IsPlayerSitDown = true;
            }
        }
    }
}