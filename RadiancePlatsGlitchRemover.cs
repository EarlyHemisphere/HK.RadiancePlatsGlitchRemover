using System.Diagnostics;
using System.Reflection;
using Modding;

namespace HK.RadiancePlatsGlitchRemover {
    public class RadiancePlatsGlitchRemover: Mod {
        public static RadiancePlatsGlitchRemover instance;

        public RadiancePlatsGlitchRemover(): base("Radiance Plats Glitch Remover") {}

        public override void Initialize() {
            instance = this;

            Log("Initalizing.");

            ModHooks.Instance.AfterSavegameLoadHook += AfterSaveGameLoad;
            ModHooks.Instance.NewGameHook += AddGlitchRemoverComponent;

            Log("Initialized.");
        }

        public override string GetVersion(){
            return FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(RadiancePlatsGlitchRemover)).Location).FileVersion;
        }

        private void AfterSaveGameLoad(SaveGameData _) {
            AddGlitchRemoverComponent();
        }

        private void AddGlitchRemoverComponent() {
            GameManager.instance.gameObject.AddComponent<PlatsGlitchRemover>();
        }
    }
}