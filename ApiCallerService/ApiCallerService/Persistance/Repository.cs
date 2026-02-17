using ApiCallerService.Model;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCallerService.Persistance
{
    public class Repository
    {
        private readonly Context _context;

        public Repository(Context context)
        {
            _context = context;
        }

        public async Task<List<CertificateOfOrigin>> CertificateOfOrigins(Guid userId)
        {
            return await _context.CertificateOfOrigins.ToListAsync();
        }
        public async Task<DateTime?> LastSuccessReadDate()
        {
            return (await _context.ESWRequests.Where(s => s.Success).OrderByDescending(s => s.RequestDate).FirstOrDefaultAsync())?.RequestDate;
        }
        public async Task SaveESWRequests(bool success, DateTime requestDate, string message)
        {
            try
            {
                if (!_context.ESWRequests.Any(d => d.RequestDate == requestDate))
                {
                    _context.ESWRequests.Add(new ESWRequest { RequestDate = requestDate, Success = success, Message = message });
                }
                else
                {
                    var eswReq = _context.ESWRequests.FirstOrDefault(d => d.RequestDate == DateTime.Today);
                    eswReq.Success = success;
                    eswReq.Message = message;
                    _context.ESWRequests.Update(eswReq);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // throw ex;
                Log.Logger.Warning(string.Format("Unable to save ESWRequests status for at time {0}", DateTime.UtcNow.ToString()), ex);
            }
        }
        public async Task<bool> Save(DataReadResult data)
        {
            var today = DateTime.Today;
            try
            {
                foreach (var certificate in data.List)
                {
                    certificate.ImportDate = today;
                    if (_context.CertificateOfOrigins.Any(s => s.LPCONo == certificate.LPCONo))
                    {
                        var oldCertificate = _context.CertificateOfOrigins.Include(s => s.CommodityList).FirstOrDefault(s => s.LPCONo == certificate.LPCONo);
                        certificate.Id = oldCertificate.Id;
                        _context.Entry(oldCertificate).CurrentValues.SetValues(certificate);
                        certificate.CommodityList.ForEach(s => s.CertificateOfOriginId = oldCertificate.Id);
                        _context.Commodities.RemoveRange(oldCertificate.CommodityList);
                        _context.CertificateOfOrigins.Update(oldCertificate);
                        _context.Commodities.AddRange(certificate.CommodityList);
                    }
                    else
                    {
                        _context.CertificateOfOrigins.Add(certificate);
                    }
                }
                //_context.CertificateOfOrigins.AddRange(data.List);
                data.List.ForEach(s => _context.Commodities.AddRange(s.CommodityList));

                await _context.SaveChangesAsync();

                await SaveESWRequests(true, today, "");

                return true;
            }
            catch (Exception ex)
            {
                // throw ex;
                Log.Logger.Warning("Unable to save data for schedule", ex);
                await SaveESWRequests(false, today, ex.Message);
                return false;
            }
        }
    }
}
