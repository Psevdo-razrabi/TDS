using System;
using Game.Player.PlayerStateMashine;

namespace BlackboardScripts.Expert
{
    public class RegisterExperts
    {
        private BlackboardController _blackboardController;

        public RegisterExperts(BlackboardController blackboardController)
        {
            _blackboardController = blackboardController;
        }
        
        public void Initialize(Func<float, bool> predictionHealth, Func<float, bool> predictionHealthLow)
        {
            var movementKey = _blackboardController.GetBlackboard().GetOrRegisterKey(NameExperts.MovementPredicate);
            var healthKeyPredicate = _blackboardController.GetBlackboard().GetOrRegisterKey(NameExperts.HealthStatsPredicate);
            var healthKey = _blackboardController.GetBlackboard().GetOrRegisterKey(NameExperts.HealthStats);
            var healthKeyLow = _blackboardController.GetBlackboard().GetOrRegisterKey(NameExperts.HealthStatsLow);
            var locationFoodKeyPredicate = _blackboardController.GetBlackboard().GetOrRegisterKey(NameExperts.LocationFoodPredicate);
            var locationFoodKey = _blackboardController.GetBlackboard().GetOrRegisterKey(NameExperts.LocationFood);
            var locationChillKeyPredicate = _blackboardController.GetBlackboard().GetOrRegisterKey(NameExperts.LocationChillZonePredicate);
            var locationChillKey = _blackboardController.GetBlackboard().GetOrRegisterKey(NameExperts.LocationChillZone);
            _blackboardController.RegisterExpert(new MovementExpert(movementKey, _blackboardController));
            _blackboardController.RegisterExpert(new StatsExpert(healthKey, healthKeyPredicate, _blackboardController, 3, predictionHealth));
            _blackboardController.RegisterExpert(new StatsExpert(healthKeyLow, healthKeyPredicate, _blackboardController, 3, predictionHealthLow));
            _blackboardController.RegisterExpert(new LocationExpert(locationFoodKey, locationFoodKeyPredicate, _blackboardController));
            _blackboardController.RegisterExpert(new LocationExpert(locationChillKey, locationChillKeyPredicate, _blackboardController));
        }
    }
}