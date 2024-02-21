using System;
using Registry;

namespace Model.Resource
{
    [Serializable]
    public record Asset : WithName
    {
        public SerializableGuid Guid;
    }
}
