namespace SSO.Domain.Models
{
    public class RealmUser
    {
        public Guid RealmId { get; set; }
        public string UserId { get; set; }

        public virtual Realm Realm { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
