using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Config
{
    internal class SocialMediaLinksConfigurations : IEntityTypeConfiguration<UserSocialMedia>
    {
        public void Configure(EntityTypeBuilder<UserSocialMedia> builder)
        {
            builder.HasKey(s => new { s.AppUserId, s.SocialMediaId });
        }
    }
}
