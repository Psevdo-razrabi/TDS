namespace Game.Player.PlayerStateMashine
{
    public class Name
    {
        public static readonly string IsCrouch = "IsCrouch";
        public static readonly string IsMove = "IsMove";
        public static readonly string IsAim = "IsAim";
        public static readonly string IsAiming = "IsAiming";
        public static readonly string IsDashing = "IsDashing";
        public static readonly string IsLookAtObstacle = "IsLookAtObstacle";
        public static readonly string IsClimbing = "IsClimbing";
        public static readonly string IsGrounded = "IsGrounded";
        public static readonly string IsLockAim = "IsLockAim";
        public static readonly string IsPlayerInObstacle = "IsPlayerInObstacle";
        public static readonly string Climb = "Climb";
        public static readonly string Landing = "Landing";
        public static readonly string ObstacleConfig = "ObstacleConfig";
        public static readonly string PlayerMoveConfig = "PlayerMoveConfig";
        public static readonly string Rotation = "Rotation";
        public static readonly string Movement = "Movement";
        public static readonly string TargetDirectionY = "TargetDirectionY";
    }


    public class NameAIKeys
    {
        public static readonly string Animator = "AIAnimator";
        public static readonly string Agent = "NavMeshAgent";
        public static readonly string HealthAI = "Health";
        public static readonly string PatrolPoints = "PatrolPoints";
        public static readonly string FoodPoint = "FoodPoint";
        public static readonly string ChillPoint = "ChillPoint";
        public static readonly string TransformAI = "Transform";
    }

    public class NameExperts
    {
        public static readonly string Movement = "Movement";
        public static readonly string MovementPredicate = "MovementPredicate";
        public static readonly string HealthStats = "HealthStats";
        public static readonly string HealthStatsLowPredicate = "HealthPredicate";
        public static readonly string HealthStatsLow = "HealthStatsLow";
        public static readonly string HealthStatsPredicate = "HealthLowPredicate";
        public static readonly string StaminaStats = "StaminaStats";
        public static readonly string StaminaStatsPredicate = "StaminaPredicate";
        public static readonly string EyesSensor = "Eyes";
        public static readonly string LocationFood = "LocationFood";
        public static readonly string LocationFoodPredicate = "LocationFoodPredicate";
        public static readonly string LocationChillZone = "LocationChill";
        public static readonly string LocationChillZonePredicate = "LocationChillPredicate";
    }
}