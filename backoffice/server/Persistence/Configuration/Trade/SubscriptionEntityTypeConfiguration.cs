using ECCSA.BackOffice.Server.Model.Trade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class SubscriptionEntityTypeConfiguration :
		IEntityTypeConfiguration<Subscription>
	{
		public void Configure(EntityTypeBuilder<Subscription> config)
		{
			config.ToTable("subscriptions", "trade");

			config.HasKey(o => o.Id);
			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.UserId)
				.HasColumnName("user_id")
				.IsRequired();

			config.Property(o => o.TopicId)
				.HasColumnName("topic_id")
				.IsRequired();

			config.Property(o => o.Status)
				.HasColumnName("status")
				.IsRequired();

			config.Property(o => o.SubscriptionDate)
				.HasColumnName("subscription_date")
				.HasColumnType("date")
				.IsRequired();

			config.Property(o => o.UnsubscriptionDate)
				.HasColumnName("unsubscribe_date")
				.HasColumnType("date")
				.IsRequired(false);

			config.HasMany<SubscriptionDissimination>(o => o.Dissiminations)
				.WithOne(s => s.Subscription)
				.HasForeignKey(s => s.SubscriberId);

		}
	}
	public class SubscriptionDissiminationEntityTypeConfiguration :
		IEntityTypeConfiguration<SubscriptionDissimination>
	{
		public void Configure(EntityTypeBuilder<SubscriptionDissimination> config)
		{
			config.ToTable("subscription_dissiminations", "trade");

			config.HasKey(o => o.Id);
			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.SubscriberId)
				.HasColumnName("subscriber_id")
				.IsRequired();

			config.Property(o => o.NewsletterIssueId)
				.HasColumnName("newsletter_issue_id")
				.IsRequired();

			config.Property(o => o.DisseminationDate)
				.HasColumnName("dissemination_date")
				.HasColumnType("date")
				.IsRequired();

		}
	}

	public class NewsletterIssueEntityTypeConfiguration :
		IEntityTypeConfiguration<NewsletterIssue>
	{
		public void Configure(EntityTypeBuilder<NewsletterIssue> config)
		{
			config.ToTable("newsletters", "trade");

			config.HasKey(o => o.Id);
			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.TopicId)
				.HasColumnName("topic_id")
				.IsRequired();

			config.Property(o => o.Title)
				.HasColumnName("title")
				.HasMaxLength(250)
				.IsRequired();

            config.Property(o => o.TitleLocal)
                .HasColumnName("title_local")
                .HasMaxLength(250)
				.IsUnicode()
                .IsRequired(false);

            config.Property(o => o.IssueDate)
				.HasColumnName("issue_date")
				.HasColumnType("date")
				.IsRequired();

			config.Property(o => o.Extension)
				.HasColumnName("extension")
				.HasMaxLength(10)
				.IsRequired();
			config.Property(o => o.Brief)
				.HasColumnName("brief")
				.HasMaxLength(1000)
				.IsRequired()
				.IsUnicode();
            config.Property(o => o.BriefLocal)
                .HasColumnName("brief_local")
                .HasMaxLength(1000)
                .IsRequired(false)
                .IsUnicode();

            config.Property(o => o.DissiminationStatus)
				.HasColumnName("dissimination_status")
				.IsRequired();
		}
	}
}