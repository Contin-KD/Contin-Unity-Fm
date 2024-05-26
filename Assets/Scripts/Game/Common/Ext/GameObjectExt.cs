namespace UnityEngine
{
    public static class GameObjectExt
    {
        public static void SetLayerRecursive(this GameObject go, int layer)
        {
            foreach (var trans in go.GetComponentsInChildren<Transform>())
            {
                trans.gameObject.layer = layer;
            }
        }
    }
}
