using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECCSA.BackOffice.Server.Model.Trade;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Trade
{
    public class CertificateOfOriginEntityTypeConfiguration :
        IEntityTypeConfiguration<CertificateOfOrigin>
    {
        public void Configure(EntityTypeBuilder<CertificateOfOrigin> config)
        {
            config.ToTable("certificate_of_origin", "trade");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .UseIdentityByDefaultColumn()
            .HasColumnName("id")
            .IsRequired();

            config.Property(o => o.LPCONo)
            .HasColumnName("lpco_no");

            config.Property(o => o.ApfRefNo)
            .HasColumnName("apf_ref_no");

            config.Property(o => o.ApfMNo)
            .HasColumnName("apf_m_no");

            config.Property(o => o.RegDate)
            .HasColumnName("reg_date");

            config.Property(o => o.CbraCd)
            .HasColumnName("cbracd");
            config.Property(o => o.CbraBrnchCd)
            .HasColumnName("cbra_brnch_cd");
            config.Property(o => o.TraderTIN)
            .HasColumnName("trader_tin");
            config.Property(o => o.TraderPartnerCode)
            .HasColumnName("trader_partner_code");
            config.Property(o => o.AplcTinAgencyID)
            .HasColumnName("aplc_tin_agency_id");
            config.Property(o => o.AplcCompTin)
            .HasColumnName("aplc_comp_tin");
            config.Property(o => o.AplcIdAgencyID)
            .HasColumnName("aplic_id_agency_id");
            config.Property(o => o.ApplicantId)
            .HasColumnName("applicant_id");
            config.Property(o => o.CarrierName)
            .HasColumnName("carrier_name");
            config.Property(o => o.CarrierNameCode)
            .HasColumnName("carrier_name_code");
            config.Property(o => o.AdditionalDeclarationComment)
            .HasColumnName("additional_declaration_comment");
            config.Property(o => o.AdditionalDeclarationCode)
            .HasColumnName("additional_declaration_code");
            config.Property(o => o.DptrTrmnCd)
            .HasColumnName("dptr_trnm_cd");
            config.Property(o => o.CostAmount)
            .HasColumnName("cost_amount");
            config.Property(o => o.CostTypeCurrencyCode)
            .HasColumnName("cost_type_currenty_code");
            config.Property(o => o.FreightForeignAmount)
            .HasColumnName("freight_foreign_amount");
            config.Property(o => o.FreightCurrencyCode)
            .HasColumnName("freight_currency_code");

            config.Property(o => o.InsuranceAmount)
            .HasColumnName("insurance_amount");
            config.Property(o => o.InsuranceCurrencyCode)
            .HasColumnName("insurance_currenty_code");
            config.Property(o => o.OtherCharge)
            .HasColumnName("other_charge");
            config.Property(o => o.OtherChargeCode)
            .HasColumnName("other_charge_code");
            config.Property(o => o.InvoiceNumber)
            .HasColumnName("invoice_number");
            config.Property(o => o.InvoiceDate)
            .HasColumnName("invoice_date");
            config.Property(o => o.InvoiceTypeCode)
            .HasColumnName("invoice_type_code");
            config.Property(o => o.TermsOfDelivery)
            .HasColumnName("terms_of_delivery");
            config.Property(o => o.PortOfExitCode)
            .HasColumnName("port_of_exit_code");
            config.Property(o => o.LicenseNo)
            .HasColumnName("license_no");
            config.Property(o => o.TradeLicenseType)
            .HasColumnName("trade_license_type");
            config.Property(o => o.TraderPartnerSequenceNo)
            .HasColumnName("trade_partner_sequence_no");
            config.Property(o => o.CountryOfDestinationCode)
            .HasColumnName("country_of_destination_code");
            config.Property(o => o.PortOfDestinationCode)
            .HasColumnName("port_of_destination_code");
            config.Property(o => o.CarriageName)
            .HasColumnName("carriage_name");
            config.Property(o => o.DestinationCountryName)
            .HasColumnName("destination_country_name");
            config.Property(o => o.PortOfDestinationName)
            .HasColumnName("port_destination_name");
            config.Property(o => o.PortOfExit)
            .HasColumnName("port_of_exit");
            config.Property(o => o.Status)
            .HasColumnName("status");

            config.Property(o => o.ConsigneeCompany)
            .HasColumnName("consignee_company");
            config.Property(o => o.ConsigneeName)
            .HasColumnName("consignee_name");
            config.Property(o => o.ConsigneeTelNo)
            .HasColumnName("consignee_tel_no");
            config.Property(o => o.ConsigneeAddress)
            .HasColumnName("consignee_address");
            config.Property(o => o.ConsigneeHouseNo)
            .HasColumnName("consignee_house_no");
            config.Property(o => o.ConsigneePoBox)
            .HasColumnName("consignee_pobox");

            config.Property(o => o.ConsignorCompany)
            .HasColumnName("consignor_company");
            config.Property(o => o.ConsignorCountryName)
            .HasColumnName("consignor_country_name");
            config.Property(o => o.ConsignorAddress)
            .HasColumnName("consignor_address");
            config.Property(o => o.ConsignorPlaceCode)
            .HasColumnName("consignor_place_code");
            config.Property(o => o.ConsignorCityName)
            .HasColumnName("consignor_city_name");
            config.Property(o => o.ConsignorTelephone)
            .HasColumnName("consignor_telephone");
            config.Property(o => o.ConsignorEmail)
            .HasColumnName("consignor_email");


            config.Property(o => o.ImportDate)
            .HasColumnName("import_date");

            config.HasMany(o => o.CommodityList)
                 .WithOne(s => s.CertificateOfOrigin)
                 .HasForeignKey(s => s.CertificateOfOriginId);
        }
    }

    public class CommodityEntityTypeConfiguration :
        IEntityTypeConfiguration<Commodity>
    {
        public void Configure(EntityTypeBuilder<Commodity> config)
        {
            config.ToTable("certificate_of_origin_commodities", "trade");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .UseIdentityByDefaultColumn()
            .HasColumnName("id")
            .IsRequired();

            config.Property(o => o.CertificateOfOriginId)
            .HasColumnName("certificate_of_origin_id");

            config.Property(o => o.SerialNo)
            .HasColumnName("serial_no");

            config.Property(o => o.OriginalSequenceNumber)
            .HasColumnName("original_sequence_number");
            config.Property(o => o.GoodsDescription)
            .HasColumnName("goods_description");
            config.Property(o => o.Quantity)
            .HasColumnName("quantity");
            config.Property(o => o.QuantityUnitCode)
            .HasColumnName("quantity_unit_code");
            config.Property(o => o.CommonName)
            .HasColumnName("common_name");
            config.Property(o => o.CommodityAmount)
            .HasColumnName("commodity_amount");
            config.Property(o => o.HSCode)
            .HasColumnName("hs_Code");
            config.Property(o => o.SpecificationCode)
            .HasColumnName("specification_code");
            config.Property(o => o.GrossWeight)
            .HasColumnName("gross_weight");
            config.Property(o => o.GrossWeightCode)
            .HasColumnName("gross_weight_code");
            config.Property(o => o.NetWeight)
            .HasColumnName("net_weight");
            config.Property(o => o.NetWeightCode)
            .HasColumnName("net_weight_code");
            config.Property(o => o.InvoiceAmount)
            .HasColumnName("invoice_amount");
            config.Property(o => o.InvoiceAmountCode)
            .HasColumnName("invoice_amount_code");
            config.Property(o => o.InvoiceAmountEtb)
            .HasColumnName("invoice_amount_etb");
            config.Property(o => o.InvoiceAmountEtbCode)
            .HasColumnName("invoice_amount_etb_code");
            config.Property(o => o.OrcyCd)
            .HasColumnName("orcy_cd");
            config.Property(o => o.PkgCnt)
            .HasColumnName("pkg_cnt");

            config.Property(o => o.PkgTpCd)
            .HasColumnName("pkg_tp_cd");
            config.Property(o => o.OriginCrtrn)
            .HasColumnName("origin_crtrn");
        }
    }

    public class ESWRequestEntityTypeConfiguration :
       IEntityTypeConfiguration<ESWRequest>
    {
        public void Configure(EntityTypeBuilder<ESWRequest> config)
        {
            config.ToTable("esw_requests", "trade");

            config.HasKey(o => o.RequestDate);

            config.Property(o => o.RequestDate)
            .HasColumnType("date")    
            .HasColumnName("request_date")
            .IsRequired();

            config.Property(o => o.Success)
            .HasColumnName("success");

            config.Property(o => o.Message)
            .HasColumnName("message");

            

           
        }
    }
}
