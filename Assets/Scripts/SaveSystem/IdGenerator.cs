using System;

namespace SaveSystem
{
    public class IdGenerator
    {
        public string GenerateId() => Guid.NewGuid().ToString();
    }
}