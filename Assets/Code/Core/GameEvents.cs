using System;
using ViaGen.Planet;

namespace ViaGen.Core
{
    public static class GameEvents
    {
        public static event Action<GameState, GameState> OnGameStateChanged;
        public static event Action<PlanetEmotion> OnPlanetLoaded;
        public static event Action<string> OnTechUnlocked;
        public static event Action<string> OnMemoryTriggered;

        public static void RaiseStateChanged(GameState prev, GameState current) =>
            OnGameStateChanged?.Invoke(prev, current);

        public static void RaisePlanetLoaded(PlanetEmotion emotion) =>
            OnPlanetLoaded?.Invoke(emotion);

        public static void RaiseTechUnlocked(string techId) =>
            OnTechUnlocked?.Invoke(techId);

        public static void RaiseMemoryTriggered(string memoryId) =>
            OnMemoryTriggered?.Invoke(memoryId);
    }
}
