using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;

namespace ProjectContra.Scripts.Util
{
    /// <summary>
    /// Similar to UnityFn, but this one has specific logic to this game
    /// </summary>
    public static class GameFn
    {
        public static GameStoreData HandlePause(GameStoreData storeData)
        {
            AppSfx.Play(AppSfx.instance.pause);
            AppMusic.instance.Pause();
            storeData.controlState = GameControlState.IN_GAME_PAUSED;
            UnityFn.Pause();
            return storeData;
        }

        public static GameStoreData HandleUnPause(GameStoreData storeData)
        {
            AppSfx.Play(AppSfx.instance.pause);
            AppMusic.instance.UnPause();
            storeData.controlState = GameControlState.IN_GAME;
            UnityFn.UnPause();
            return storeData;
        }
    }
}