using Volo.Abp.Domain.Entities;

namespace UserSystem.Domain.Account
{
    public class Level : Entity<string>
    {
        public string LevelName { get; set; }
        public int NeedIntegral { get; set; }
    }
}