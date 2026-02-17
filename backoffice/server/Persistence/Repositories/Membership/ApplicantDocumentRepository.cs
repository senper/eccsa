using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class ApplicantDocumentRepository 
	{
		private readonly EccsaContext _context;
		public ApplicantDocumentRepository(EccsaContext context)
		{
			_context = context;
		}

		public IQueryable<ApplicantDocument> ApplicantDocuments(Guid applicantId)
		{
			throw new NotImplementedException();
			;
		}

		

		public async Task Delete(ApplicantDocument data)
		{
			var tmp = await _context.ApplicantDocuments.FirstOrDefaultAsync(r => r.Id == data.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
			_context.ApplicantDocuments.Remove(tmp);
			await _context.SaveChangesAsync();
		}
	}
}