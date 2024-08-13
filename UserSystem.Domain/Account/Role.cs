using Volo.Abp.Domain.Entities;

namespace UserSystem.Domain.Account
{
    public class Role : Entity<long>
    {
        public string RoleName { get; set; }
    }
}