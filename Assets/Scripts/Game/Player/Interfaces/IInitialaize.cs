using UI.Storage;

namespace Game.Player.Interfaces
{
    public interface IInitialaize
    { 
        void InitModel(ValueCountStorage<float> valueCountStorage);
    }
}