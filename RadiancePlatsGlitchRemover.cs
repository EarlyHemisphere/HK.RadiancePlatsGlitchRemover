using System.Diagnostics;
using System.Reflection;
using Modding;
using Satchel.BetterMenus;

namespace HK.RadiancePlatsGlitchRemover {
    public class RadiancePlatsGlitchRemover: Mod, ICustomMenuMod, IGlobalSettings<GlobalSettings> {
        public static RadiancePlatsGlitchRemover instance;
        private Menu menuRef = null;

        public RadiancePlatsGlitchRemover(): base("Radiance Plats Glitch Remover") {}

        public static GlobalSettings globalSettings { get; private set; } = new();
        public void OnLoadGlobal(GlobalSettings s) => globalSettings = s;
        public GlobalSettings OnSaveGlobal() => globalSettings;

        public bool ToggleButtonInsideMenu => false;

        public override void Initialize() {
            instance = this;

            Log("Initalizing.");

            ModHooks.AfterSavegameLoadHook += AfterSaveGameLoad;
            ModHooks.NewGameHook += AddGlitchRemoverComponent;

            Log("Initialized.");
        }

        public override string GetVersion(){
            return FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(RadiancePlatsGlitchRemover)).Location).FileVersion;
        }
        
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) {
            menuRef ??= new Menu(
                "Radiance Plats Glitch Remover",
                new Element[] {
                    new HorizontalOption(
                        name: "Prevent Stuck Behavior",
                        description: "Prevents behavior where radiance gets \"stuck\" in one spot and doesn't do normal teleporting",
                        values: new [] {"false", "true"},
                        applySetting: val => globalSettings.preventStuckBehavior = val == 1,
                        loadSetting: () => globalSettings.preventStuckBehavior ? 1 : 0
                    ),
                    new HorizontalOption(
                        name: "Prevent Sword Wall Spam",
                        description: "Prevents bradiance doing multiple sword wall attacks in a row after spending ~6 minutes in plats practice",
                        values: new [] {"false", "true"},
                        applySetting: val => globalSettings.preventSwordWallSpam = val == 1,
                        loadSetting: () => globalSettings.preventSwordWallSpam ? 1 : 0
                    ),
                }
            );

            return menuRef.GetMenuScreen(modListMenu);
        }

        private void AfterSaveGameLoad(SaveGameData _) {
            AddGlitchRemoverComponent();
        }

        private void AddGlitchRemoverComponent() {
            GameManager.instance.gameObject.AddComponent<PlatsGlitchRemover>();
        }
    }

    public class GlobalSettings {
        public bool preventStuckBehavior = false;
        public bool preventSwordWallSpam = true;
    }
}