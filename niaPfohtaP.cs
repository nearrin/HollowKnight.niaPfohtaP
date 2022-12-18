using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using System.Collections.Generic;

namespace niaPfohtaP
{
    public class Settings
    {
        public bool on = true;
    }
    public class niaPfohtaP : Mod
    {
        public Settings settings_ = new Settings();
        public bool ToggleButtonInsideMenu => true;
        private GameObject platform;
        public niaPfohtaP() : base("niaPfohtaP")
        {
        }
        public override string GetVersion() => "1.0.0.0";
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
        {
            ("White_Palace_03_hub", "wp_plat_float_05 (1)")
        };
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            ModHooks.Instance.HeroUpdateHook += ModHooks_HeroUpdateHook;
            platform = preloadedObjects["White_Palace_03_hub"]["wp_plat_float_05 (1)"];
            UnityEngine.Object.Destroy(platform.GetComponent<SpriteRenderer>());
        }
        List<GameObject> GetAllObjectsInScene()
        {
            List<GameObject> objectsInScene = new List<GameObject>();
            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
                    continue;
                objectsInScene.Add(go);
            }
            return objectsInScene;
        }
        private void Reverse(UnityEngine.SceneManagement.Scene scene, float height)
        {
            Dictionary<string, Vector3> oldPos = new Dictionary<string, Vector3>();
            foreach (var g in GetAllObjectsInScene())
            {
                if (g.name.StartsWith("CameraLockArea"))
                {
                    UnityEngine.Object.Destroy(g);
                }
                oldPos[g.name] = g.transform.position;
            }
            foreach (var g in scene.GetRootGameObjects())
            {
                Log("Reverse " + g.name);
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
            foreach (var g in GetAllObjectsInScene())
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
                foreach (var g in GetAllObjectsInScene())
                {
                    if (g.name == "right1")
                    {
                        g.transform.Translate(-2, 0, 0);
                    }
                    else if (g.name == "Hazard Respawn Trigger v2 (12)")
                    {
                        UnityEngine.Object.Destroy(g);
                    }
                    else if (g.name == "wp_saw (18)")
                    {
                        var saw = UnityEngine.Object.Instantiate(g, g.transform.parent);
                        saw.transform.position = new Vector3(
                            165,
                            80,
                            saw.transform.position.z
                        );
                    }
                }
                var p = UnityEngine.Object.Instantiate(platform);
                p.transform.position = new Vector3(
                    204.9f,
                    11.05f,
                    p.transform.position.z
                );
                p.transform.localScale = new Vector3(
                    p.transform.localScale.x,
                    0.05f,
                    p.transform.localScale.z
                );
                p.SetActive(true);
            }
            else if (arg1.name == "White_Palace_17")
            {
                Reverse(arg1, 115);
                foreach (var g in GetAllObjectsInScene())
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
                var p = UnityEngine.Object.Instantiate(platform);
                p.transform.position = new Vector3(
                    27.3f,
                    47.41f,
                    p.transform.position.z
                );
                p.transform.localScale = new Vector3(
                    p.transform.localScale.x,
                    0.05f,
                    p.transform.localScale.z
                );
                p.SetActive(true);
            }
            else if (arg1.name == "White_Palace_19")
            {
                Reverse(arg1, 165);
                foreach (var g in GetAllObjectsInScene())
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
                foreach (var g in GetAllObjectsInScene())
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
                    else if (g.name == "wp_saw (64)")
                    {
                        var saw = UnityEngine.Object.Instantiate(g, g.transform.parent);
                        saw.transform.position = new Vector3(
                            231,
                            26.07f,
                            saw.transform.position.z
                        );
                        saw = UnityEngine.Object.Instantiate(g, g.transform.parent);
                        saw.transform.position = new Vector3(
                            231,
                            35.07f,
                            saw.transform.position.z
                        );
                        saw = UnityEngine.Object.Instantiate(g, g.transform.parent);
                        saw.transform.position = new Vector3(
                            231,
                            44.07f,
                            saw.transform.position.z
                        );
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
    }
}