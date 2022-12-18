global using UnityEngine;
global using HutongGames.PlayMaker;
global using HutongGames.PlayMaker.Actions;
global using Modding;
global using Satchel;
namespace niaPfohtaP;
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
    private void Reverse(UnityEngine.SceneManagement.Scene scene, float height)
    {
        foreach (var g in scene.GetAllGameObjects())
        {
            if (g.name.StartsWith("CameraLockArea"))
            {
                UnityEngine.Object.Destroy(g);
            }
        }
        foreach (var g in scene.GetRootGameObjects())
        {
            g.transform.position = new Vector3(
                    g.transform.position.x,
                    height - g.transform.position.y,
                    g.transform.position.z
            );
            g.transform.rotation = Quaternion.Euler(
                g.transform.rotation.eulerAngles.x + 180,
                g.transform.rotation.eulerAngles.y,
                g.transform.rotation.eulerAngles.z
            );
            if (g.name.StartsWith("top"))
            {
                g.name = "bot" + g.name.Substring(3);
                g.GetComponent<TransitionPoint>().entryPoint = "top" + g.GetComponent<TransitionPoint>().entryPoint.Substring(3);
            }
            else if (g.name.StartsWith("bot"))
            {
                g.name = "top" + g.name.Substring(3);
                g.GetComponent<TransitionPoint>().entryPoint = "bot" + g.GetComponent<TransitionPoint>().entryPoint.Substring(3);
            }
        }
        foreach (var g in scene.GetAllGameObjects())
        {
            if (g.transform.parent == null)
            {
                continue;
            }
            g.transform.localPosition = new Vector3(
                g.transform.localPosition.x,
                g.transform.localPosition.y,
                -g.transform.localPosition.z
            );
        }
    }
    private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
    {
        if (!settings_.on)
        {
            return;
        }
        if (arg1.name == "White_Palace_18")
        {
            Reverse(arg1, 105);
            foreach (var g in arg1.GetRootGameObjects())
            {
                if (g.name == "right1")
                {
                    g.transform.Translate(-2, 0, 0);
                }
                else if (g.name == "Hazard Respawn Trigger v2 (12)")
                {
                    UnityEngine.Object.Destroy(g);
                }
            }
        }
        else if (arg1.name == "White_Palace_17")
        {
            Reverse(arg1, 115);
            foreach (var g in arg1.GetRootGameObjects())
            {
                if (g.name == "Hazard Respawn Trigger v2(5)")
                {
                    UnityEngine.Object.Destroy(g);
                }
                else if (g.name == "Hazard Respawn Trigger v2(9)")
                {
                    UnityEngine.Object.Destroy(g);
                }
                else if (g.name == "Hazard Respawn Trigger v2(11)")
                {
                    UnityEngine.Object.Destroy(g);
                }
            }
        }
        else if (arg1.name == "White_Palace_19")
        {
            Reverse(arg1, 165);
            foreach (var g in arg1.GetRootGameObjects())
            {
                if (g.name == "Hazard Respawn Trigger v2(8)")
                {
                    UnityEngine.Object.Destroy(g);
                }
            }
        }
        else if (arg1.name == "White_Palace_20")
        {
            Reverse(arg1, 185);
            foreach (var g in arg1.GetAllGameObjects())
            {
                if (g.name.StartsWith("Royal Gaurd"))
                {
                    g.GetComponent<Rigidbody2D>().gravityScale *= -1;
                }
                else if (g.name == "End Scene")
                {
                    UnityEngine.Object.Destroy(g.transform.Find("CamLock").gameObject);
                    g.GetComponent<BoxCollider2D>().offset = new Vector2(
                        g.GetComponent<BoxCollider2D>().offset.x,
                        g.GetComponent<BoxCollider2D>().offset.y + 11
                    );
                }
                else if (g.name == "Hazard Respawn Trigger v2")
                {
                    UnityEngine.Object.Destroy(g);
                }
            }
        }
    }
    private void ModHooks_HeroUpdateHook()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("White_Palace_20");
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
