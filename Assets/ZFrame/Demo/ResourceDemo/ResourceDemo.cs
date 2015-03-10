using UnityEngine;
using ZFrame.Base.MonoBase;
using ZFrame.IO.ResourceSystem;

public class ResourceDemo : MonoBehaviour
{
    private void AsyncDemo()
    {
        ResourcePool.InstantiateAsync<GameObject>("DemoResource/demosphere",
            go =>
                ResourcePool.LoadAsync<Material>("DemoResource/demomat",
                    material =>
                    {
                        go.GetComponent<MeshRenderer>().material = material;
                        go.AddComponent<DelegateMonoMini>().UpdateHandler += () => go.transform.RotateAround(transform.position, Vector3.up, 5f);
                    }));
    }

    private void Demo()
    {
        GameObject sphere = ResourcePool.Instantiate<GameObject>("DemoResource/demosphere");
        Material mat = ResourcePool.Load<Material>("DemoResource/demomat");

        sphere.GetComponent<MeshRenderer>().material = mat;
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Demo"))
        {
            Demo();
        }

        if (GUILayout.Button("Async Demo"))
        {
            AsyncDemo();
        }
    }
}