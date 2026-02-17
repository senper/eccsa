using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECCSA.BackOffice.Server.WebApp.Api.Model
{
    public class ApiUserBusinessInterestModel
    {
        public String Description { get; set; }

        public  List<UserBusinessInterestSourceSector> Sectors { get; set; }
        public List<UserBusinessInterestSourceDetail> Details { get; set; }
        public BusinessInterestSource ToBusinessInterestSource()
        {
            return new BusinessInterestSource
            {
                Description = Description ?? "",
                Sectors = Sectors?.Select(s => s.ToBusinessInterestSourceSector()).ToList() ??
                new List<BusinessInterestSourceSector>(),
                Details = Details?.Select(d => d.ToBusinessInterestSourceSector()).Distinct().ToList() ??
                new List<BusinessInterestSourceDetail>()
            };
        }
        public static ApiUserBusinessInterestModel FromBusinessInterestSource(BusinessInterestSource data)
        {
            return new ApiUserBusinessInterestModel
            {
                Description = data?.Description ?? "",
                Details = data?.Details?.Select(d =>
                    UserBusinessInterestSourceDetail.FromBusinessInterestSourceDetail(d)).ToList()
                    ?? new List<UserBusinessInterestSourceDetail>(),
                Sectors = data?.Sectors?.Select(s => 
                UserBusinessInterestSourceSector.FromBusinessInterestSourceSector(s)).ToList()
                ?? new List<UserBusinessInterestSourceSector>(),
            };
        }
    }
    public class UserBusinessInterestSourceSector
    {
        public Guid Id { get; set; }
        public Guid SectorId { get; set; }
        public string Name { get; set; }
        public BusinessInterestSourceSector ToBusinessInterestSourceSector()
        {
            return new BusinessInterestSourceSector
            {
                 SectorId = SectorId,
                 Id = Id
            };
        }
        public static UserBusinessInterestSourceSector FromBusinessInterestSourceSector
            (BusinessInterestSourceSector data)
        {
            return new UserBusinessInterestSourceSector
            {
                SectorId = data.SectorId,
                Name = data.BusinessSector.Name,
                Id = data.Id
            };
        }
    }
    public class UserBusinessInterestSourceDetail
    {
        public Guid Id { get; set; }
        public Guid? ProductTypeId { get; set; }
        public string ProductName { get; set; }
        public Guid BusinessType { get; set; }
        public String Description { get; set; }
        public BusinessInterestSourceDetail ToBusinessInterestSourceSector()
        {
            return new BusinessInterestSourceDetail
            {
                ProductTypeId = ProductTypeId,
                BusinessType = BusinessType,    
                Description = Description,
                DescriptionNormalized = Description.ToLowerInvariant(),
                Id = Id
            };
        }
        public static UserBusinessInterestSourceDetail FromBusinessInterestSourceDetail
            (BusinessInterestSourceDetail data)
        {
            return new UserBusinessInterestSourceDetail
            {
                ProductTypeId = data.ProductTypeId,
                BusinessType = data.BusinessType,
                Description = data.Description,
                ProductName = data.ProductType.Name,
                Id = data.Id
            };
        }
    }
}
