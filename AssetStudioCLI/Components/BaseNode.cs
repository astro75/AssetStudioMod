using System.Collections.Generic;

namespace AssetStudioCLI
{
    internal class BaseNode
    {
        public List<BaseNode> nodes = new List<BaseNode>();
        public string FullPath = "";
        public readonly string Text;

        public BaseNode(string name)
        {
            Text = name;
        }
    }
}
