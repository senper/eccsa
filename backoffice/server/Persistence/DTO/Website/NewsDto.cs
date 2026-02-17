using System;


namespace ECCSA.BackOffice.Server.Persistence.DTO.Website
{
    public record News(
		Guid partner_id,
		String title,
		String brief,
		String full,
		String order_no,
		String author,
        DateTime news_date
        );

	
}





