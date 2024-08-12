using AssetStudio;

namespace AssetStudioCLI
{
    internal class GameObjectNode : BaseNode
    {
        public GameObject gameObject;

        public GameObjectNode(GameObject gameObject) : base(gameObject.m_Name)
        {
            this.gameObject = gameObject;
        }
    }
}
