using Model.Resource;

namespace Factory
{
    public interface IAssetFactory
    {
        bool TryCreate(string name, out Asset asset);
        Asset GetOrCreate(string name);
    }
}
