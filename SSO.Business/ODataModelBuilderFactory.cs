using Microsoft.OData.ModelBuilder;
using SSO.Business.Applications;
using SSO.Business.Groups;
using SSO.Business.Users;

namespace SSO.Business
{
    public static class ODataModelBuilderFactory
    {
        public static ODataConventionModelBuilder Create()
        {
            var modelBuilder = new ODataConventionModelBuilder();

            modelBuilder.EntityType<ApplicationDto>().HasKey(x => x.ApplicationId);
            modelBuilder.EntitySet<ApplicationDto>("Application");

            modelBuilder.EntityType<UserDto>().HasKey(x => x.UserId);
            modelBuilder.EntitySet<UserDto>("User");

            modelBuilder.EntityType<UserDto>().HasKey(x => x.UserId);
            modelBuilder.EntitySet<UserDto>("ApplicationUser");

            modelBuilder.EntityType<GroupDto>().HasKey(x => x.GroupId);
            modelBuilder.EntitySet<GroupDto>("Group");

            modelBuilder.EntityType<UserDto>().HasKey(x => x.UserId);
            modelBuilder.EntitySet<UserDto>("GroupUser");

            modelBuilder.EntityType<GroupDto>().HasKey(x => x.GroupId);
            modelBuilder.EntitySet<GroupDto>("ApplicationGroup");

            modelBuilder.EnableLowerCamelCase();

            return modelBuilder;
        }
    }
}
