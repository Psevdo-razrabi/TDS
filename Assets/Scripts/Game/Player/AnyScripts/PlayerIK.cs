namespace Game.Player.AnyScripts
{
    public class PlayerIK
    {
        public IKSystem IKSystem { get; private set; }
        
        public PlayerIK(IKSystem ikSystem)
        {
            IKSystem = ikSystem;
        }
    }
}