global using UnityEngine;
global using HutongGames.PlayMaker;
global using HutongGames.PlayMaker.Actions;
global using Modding;
global using Satchel;
namespace niaPfohtaP;
[Serializable]
public class Settings
{
    public bool on = true;
}
public class niaPfohtaP : Mod, IGlobalSettings<Settings>, IMenuMod
{
    public Settings settings_ = new();
    public bool ToggleButtonInsideMenu => true;
    public niaPfohtaP() : base("niaPfohtaP")
    {
    }
    public override string GetVersion() => "1.0.0.0";
    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
    }
    private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
    {
        if (!settings_.on) return;
        if (arg1.name == "White_Palace_18")
        {
            var yTop = 105;
            foreach (var g in arg1.GetRootGameObjects())
            {
                g.transform.position = new Vector3(
                    g.transform.position.x,
                    yTop - g.transform.position.y,
                    g.transform.position.z
                );
                g.transform.rotation = Quaternion.Euler(
                    g.transform.rotation.eulerAngles.x + 180,
                    g.transform.rotation.eulerAngles.y,
                    g.transform.rotation.eulerAngles.z
                );
                if (g.name.StartsWith("CameraLockArea"))
                {
                    g.SetActive(false);
                }
                if (g.name == "wp_clouds")
                {
                    g.transform.Find("wp_clouds_0002_1 (61)").gameObject.SetActive(false);
                }
                if (g.name == "white_palace_floor_set_01 (49)")
                {
                    g.transform.Find("_0060_white (5)").gameObject.SetActive(false);
                }
                if (g.name == "right1")
                {
                    g.transform.Translate(-2, 0, 0);
                }
            }
        }
    }
    private void ModHooks_HeroUpdateHook()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("White_Palace_18");
        }
    }
    public void OnLoadGlobal(Settings settings) => settings_ = settings;
    public Settings OnSaveGlobal() => settings_;
    public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? menu)
    {
        List<IMenuMod.MenuEntry> menus = new();
        menus.Add(
            new()
            {
                Values = new string[]
                {
                    Language.Language.Get("MOH_ON", "MainMenu"),
                    Language.Language.Get("MOH_OFF", "MainMenu"),
                },
                Saver = i => settings_.on = i == 0,
                Loader = () => settings_.on ? 0 : 1
            }
        );
        return menus;
    }
}
