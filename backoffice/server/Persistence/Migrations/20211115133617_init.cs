using System;
using System.Collections.Generic;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.ExternalData;
using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ECCSA.BackOffice.Server.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "membership");

            migrationBuilder.EnsureSchema(
                name: "website");

            migrationBuilder.EnsureSchema(
                name: "services");

            migrationBuilder.EnsureSchema(
                name: "arbitration");

            migrationBuilder.EnsureSchema(
                name: "training");

            migrationBuilder.EnsureSchema(
                name: "surveying");

            migrationBuilder.EnsureSchema(
                name: "investment");

            migrationBuilder.EnsureSchema(
                name: "communication");

            migrationBuilder.EnsureSchema(
                name: "trade");

            migrationBuilder.EnsureSchema(
                name: "payment");

            migrationBuilder.EnsureSchema(
                name: "advocacy");

            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.EnsureSchema(
                name: "eventmanagement");

            migrationBuilder.EnsureSchema(
                name: "mande");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "security");

            migrationBuilder.CreateTable(
                name: "arbitration_service_types",
                schema: "arbitration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_eng = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    name_local = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    requires_third_party = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    description_eng = table.Column<string>(type: "text", nullable: true),
                    description_local = table.Column<string>(type: "text", nullable: true),
                    service_charge = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arbitration_service_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "business_information",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_business_information", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "businesses",
                schema: "investment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar", maxLength: 200, nullable: false),
                    sector = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    profile = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    address = table.Column<string>(type: "varchar", maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    telephone = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    tino = table.Column<string>(type: "varchar", maxLength: 20, nullable: false),
                    contact_person = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    products = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    establishment_year = table.Column<int>(type: "integer", nullable: false),
                    working_capital = table.Column<int>(type: "integer", nullable: false),
                    confirmation_link = table.Column<string>(type: "varchar", maxLength: 200, nullable: false),
                    link_expiry_date = table.Column<DateTime>(type: "date", nullable: false),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    sms_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    phone_confirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 883, DateTimeKind.Utc).AddTicks(2773)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 883, DateTimeKind.Utc).AddTicks(2997))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_businesses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "campaigns",
                schema: "communication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    purpose = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 663, DateTimeKind.Utc).AddTicks(2227)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 663, DateTimeKind.Utc).AddTicks(3670))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campaigns", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_categories_categories_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "training",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "commodity_types",
                schema: "trade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    name_local = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: " ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commodity_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contents",
                schema: "website",
                columns: table => new
                {
                    partner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content_type = table.Column<int>(type: "integer", nullable: false),
                    is_visible = table.Column<bool>(type: "boolean", nullable: false),
                    meta_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    page_title = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contents", x => x.partner_id);
                });

            migrationBuilder.CreateTable(
                name: "currencies",
                schema: "payment",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currencies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "currencies",
                schema: "trade",
                columns: table => new
                {
                    code = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    order_no = table.Column<string>(type: "varchar", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currencies", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "dialogs",
                schema: "advocacy",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    responsible_person = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    target_institution = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    outcome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    implementation_period = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    consultation_dates = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    remark = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 863, DateTimeKind.Utc).AddTicks(6753)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 863, DateTimeKind.Utc).AddTicks(7078))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dialogs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discount_categories",
                schema: "payment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discount_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "email_messages",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner = table.Column<int>(type: "integer", nullable: false),
                    subject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    recipients = table.Column<List<MessageDelivery>>(type: "jsonb", nullable: false),
                    attachment = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    reference_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "email_templates",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    place_holders = table.Column<List<EmailPlaceHolder>>(type: "jsonb", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_templates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "event_types",
                schema: "eventmanagement",
                columns: table => new
                {
                    event_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_types", x => x.event_type_id);
                });

            migrationBuilder.CreateTable(
                name: "exchange_rates",
                schema: "trade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    transaction_date = table.Column<DateTime>(type: "date", nullable: false),
                    currency = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    buying = table.Column<decimal>(type: "decimal", nullable: false),
                    selling = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exchange_rates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "export_agreements",
                schema: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    agreement_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    license_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    consignee = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    type_of_carriage = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    county_of_destination = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    port_of_destination = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    port_of_departure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name_of_carrier = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    term_of_delivery = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_locked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_export_agreements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "external_data_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    table_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    schema_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    caption = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    meta_data = table.Column<List<ExternalMetaData>>(type: "jsonb", nullable: false),
                    periods = table.Column<List<Period>>(type: "jsonb", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_external_data_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "flag_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    flag_type = table.Column<string>(type: "varchar", maxLength: 50, nullable: true),
                    flag_type_local = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flag_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "followup_result_types",
                schema: "arbitration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_eng = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name_local = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    description_eng = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    description_local = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_followup_result_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "import_exports",
                schema: "trade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hscode = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    cpcode = table.Column<string>(type: "varchar", maxLength: 10, nullable: false),
                    current_epc = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    previous_epc = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    apc = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    direction = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    origin_country = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    destination_country = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    consignment_country = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    net_weight_kg = table.Column<long>(type: "bigint", nullable: false),
                    trade_value = table.Column<decimal>(type: "numeric", nullable: false),
                    tax_paid = table.Column<decimal>(type: "numeric", nullable: false),
                    trades = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_import_exports", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "indicators",
                schema: "mande",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "varchar", maxLength: 20, nullable: false),
                    name_eng = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    name_local = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    responsible_unit = table.Column<int>(type: "integer", nullable: false),
                    indicator_type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_indicators", x => x.id);
                    table.ForeignKey(
                        name: "FK_indicators_indicators_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "mande",
                        principalTable: "indicators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                schema: "payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sub_system = table.Column<int>(type: "integer", nullable: false),
                    reference_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    reason = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    invoice_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_credit_service = table.Column<bool>(type: "boolean", nullable: false),
                    credit_due_days = table.Column<int>(type: "integer", nullable: false),
                    invoice_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "ETB"),
                    base_price = table.Column<decimal>(type: "numeric", nullable: false),
                    discount = table.Column<decimal>(type: "numeric", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    cancelled = table.Column<bool>(type: "boolean", nullable: false),
                    cancelled_by = table.Column<Guid>(type: "uuid", nullable: false),
                    date_cancelled = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "issue_types",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issue_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "legal_document_types",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    has_expiry = table.Column<bool>(type: "boolean", nullable: false),
                    allow_multiple = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legal_document_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "legal_profiles",
                schema: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    legal_document_type = table.Column<int>(type: "integer", nullable: false, defaultValue: 100),
                    document_number = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    expiry_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_locked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    file_extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, defaultValue: ""),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legal_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false),
                    level1_id = table.Column<Guid>(type: "uuid", nullable: true),
                    level1_name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    level1_name_local = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    level2_id = table.Column<Guid>(type: "uuid", nullable: true),
                    level2_name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    level2_name_local = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lookup_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    owner = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lookup_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "market_places",
                schema: "trade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    name_eng = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    name_local = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    location_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-0000000f4240")),
                    latitude = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    longitude = table.Column<float>(type: "real", nullable: false, defaultValue: 0f)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_market_places", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "media_agencies",
                schema: "communication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar", maxLength: 200, nullable: false),
                    media_type = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_local_agency = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ownership_type = table.Column<int>(type: "integer", nullable: false),
                    representative_name = table.Column<string>(type: "varchar", maxLength: 200, nullable: false),
                    responsible_personnel = table.Column<string>(type: "varchar", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "varchar", maxLength: 200, nullable: false),
                    mobile = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    telephone = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    url = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    remark = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 674, DateTimeKind.Utc).AddTicks(432)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 674, DateTimeKind.Utc).AddTicks(998))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_agencies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "meetings",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    meeting_date = table.Column<DateTime>(type: "date", nullable: false),
                    address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 873, DateTimeKind.Utc).AddTicks(8311)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 873, DateTimeKind.Utc).AddTicks(8574))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meetings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "members",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    member_type = table.Column<int>(type: "int", nullable: false),
                    membership_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address_type = table.Column<int>(type: "int", nullable: false),
                    location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address_line = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    contact_type = table.Column<int>(type: "int", nullable: false),
                    contact = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_members", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "need_assessments",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_need_assessments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "position_types",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    for_board = table.Column<bool>(type: "boolean", nullable: false),
                    for_staff = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_position_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "price_unit_rates",
                schema: "trade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    name_local = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_price_unit_rates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "profile_attribure",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    attribute_type = table.Column<int>(type: "integer", nullable: false),
                    attribute_max = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    attribute_min = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    attribute_source = table.Column<List<SimpleLookup>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_attribure", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                schema: "advocacy",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    planned_start_date = table.Column<DateTime>(type: "date", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    planned_end_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    remark = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 869, DateTimeKind.Utc).AddTicks(8597)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 869, DateTimeKind.Utc).AddTicks(8874))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "publications",
                schema: "advocacy",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    keywords = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    url = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    funding = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_public = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    remark = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 867, DateTimeKind.Utc).AddTicks(7141)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 867, DateTimeKind.Utc).AddTicks(7490))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "recipients",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    company = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    mobile = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    owner = table.Column<int>(type: "integer", nullable: false),
                    stakeholder_type = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "report",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    report_type_id = table.Column<int>(type: "integer", nullable: false),
                    source_name = table.Column<string>(type: "text", nullable: false),
                    subtitle = table.Column<string>(type: "text", nullable: true),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    group_name = table.Column<string>(type: "text", nullable: true),
                    sort_order = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    default_value_key = table.Column<string>(type: "text", nullable: true),
                    spatial_component = table.Column<int>(type: "integer", nullable: false),
                    trend_component = table.Column<int>(type: "integer", nullable: false),
                    spatial_source = table.Column<string>(type: "text", nullable: true),
                    trend_source = table.Column<string>(type: "text", nullable: true),
                    pk_field_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "researches",
                schema: "advocacy",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    conducted_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    target = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    finding = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    stage = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    remark = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 865, DateTimeKind.Utc).AddTicks(9381)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 865, DateTimeKind.Utc).AddTicks(9688))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_researches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resources",
                schema: "communication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "varchar", maxLength: 800, nullable: false),
                    author = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    publication_date = table.Column<DateTime>(type: "date", nullable: false),
                    url = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    digital_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    print_uri = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    print_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service_owners",
                schema: "services",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_owners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service_types",
                schema: "services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceOwner = table.Column<int>(type: "integer", nullable: false),
                    name_eng = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name_local = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    approval_required = table.Column<bool>(type: "boolean", nullable: false),
                    description_eng = table.Column<string>(type: "text", nullable: true),
                    description_local = table.Column<string>(type: "text", nullable: true),
                    rate_basis = table.Column<int>(type: "integer", nullable: false),
                    member_service_charge = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    public_service_charge = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    status = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: true),
                    end_date = table.Column<DateTime>(type: "date", nullable: true),
                    report_template = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 694, DateTimeKind.Utc).AddTicks(7043)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 694, DateTimeKind.Utc).AddTicks(7392))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "setting_variables",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    data_type = table.Column<int>(type: "int", nullable: false),
                    min_value = table.Column<int>(type: "int", nullable: false),
                    max_value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_setting_variables", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sms_messages",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    owner = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    recipients = table.Column<List<MessageDelivery>>(type: "jsonb", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sms_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sponsors",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    mobile = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sponsors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stakeholders",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    stakeholder_type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stakeholders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "survey_settings",
                schema: "surveying",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    responsible_unit = table.Column<int>(type: "integer", nullable: false),
                    Recurrence = table.Column<int>(type: "integer", nullable: false),
                    open_days = table.Column<int>(type: "integer", nullable: false),
                    publish_through = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 839, DateTimeKind.Utc).AddTicks(8504)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 839, DateTimeKind.Utc).AddTicks(8848))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task_templates",
                schema: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_eng = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    description_eng = table.Column<string>(type: "text", nullable: false),
                    description_local = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_templates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task_types",
                schema: "arbitration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_eng = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name_local = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    description_eng = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    description_local = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ServiceCharge = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_contacts",
                schema: "security",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_type = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_contacts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "yes_no",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    name_local = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_yes_no", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "arbitration_services",
                schema: "arbitration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    association_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tin = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    email_address = table.Column<string>(type: "text", nullable: true),
                    mobile_no = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    service_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    request_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    invoice_no = table.Column<string>(type: "varchar", maxLength: 20, nullable: true),
                    receipt_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    payment_method = table.Column<int>(type: "integer", nullable: false),
                    request_status = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 819, DateTimeKind.Utc).AddTicks(4534)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 819, DateTimeKind.Utc).AddTicks(4809))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arbitration_services", x => x.id);
                    table.ForeignKey(
                        name: "FK_arbitration_services_arbitration_service_types_service_type~",
                        column: x => x.service_type_id,
                        principalSchema: "arbitration",
                        principalTable: "arbitration_service_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topics",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    decription = table.Column<string>(type: "text", nullable: true),
                    decription_local = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 804, DateTimeKind.Utc).AddTicks(9760)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 805, DateTimeKind.Utc).AddTicks(122))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics", x => x.id);
                    table.ForeignKey(
                        name: "FK_topics_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "training",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "advertizements",
                schema: "website",
                columns: table => new
                {
                    partner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    company_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    owner = table.Column<int>(type: "integer", nullable: false),
                    show_in_section = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advertizements", x => x.partner_id);
                    table.ForeignKey(
                        name: "FK_advertizements_contents_partner_id",
                        column: x => x.partner_id,
                        principalSchema: "website",
                        principalTable: "contents",
                        principalColumn: "partner_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "keywords",
                schema: "website",
                columns: table => new
                {
                    keyword_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    word = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    located_in = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keywords", x => x.keyword_id);
                    table.ForeignKey(
                        name: "FK_keywords_contents_content_id",
                        column: x => x.content_id,
                        principalSchema: "website",
                        principalTable: "contents",
                        principalColumn: "partner_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news",
                schema: "website",
                columns: table => new
                {
                    partner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    brief = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    full = table.Column<string>(type: "text", nullable: true),
                    order_no = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    author = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news", x => x.partner_id);
                    table.ForeignKey(
                        name: "FK_news_contents_partner_id",
                        column: x => x.partner_id,
                        principalSchema: "website",
                        principalTable: "contents",
                        principalColumn: "partner_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "partners",
                schema: "website",
                columns: table => new
                {
                    partner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    company_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partners", x => x.partner_id);
                    table.ForeignKey(
                        name: "FK_partners_contents_partner_id",
                        column: x => x.partner_id,
                        principalSchema: "website",
                        principalTable: "contents",
                        principalColumn: "partner_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "events",
                schema: "eventmanagement",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: false),
                    owner = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.event_id);
                    table.ForeignKey(
                        name: "FK_events_event_types_event_type_id",
                        column: x => x.event_type_id,
                        principalSchema: "eventmanagement",
                        principalTable: "event_types",
                        principalColumn: "event_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "agreement_commodities",
                schema: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    agreement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    hs_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agreement_commodities", x => x.id);
                    table.ForeignKey(
                        name: "FK_agreement_commodities_export_agreements_agreement_id",
                        column: x => x.agreement_id,
                        principalSchema: "services",
                        principalTable: "export_agreements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "flags",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    flag_type_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar", maxLength: 50, nullable: true),
                    name_local = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flags", x => new { x.id, x.flag_type_id });
                    table.ForeignKey(
                        name: "FK_flags_flag_types_flag_type_id",
                        column: x => x.flag_type_id,
                        principalTable: "flag_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plans",
                schema: "mande",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    indicator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    plan_year = table.Column<int>(type: "integer", nullable: false),
                    plan_type = table.Column<int>(type: "integer", nullable: false),
                    phase = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    baseline = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    physical_target = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    physical_actual = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    financial_target = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    financial_actual = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    green = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    yello = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    main_plan_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plans", x => x.id);
                    table.ForeignKey(
                        name: "FK_plans_indicators_indicator_id",
                        column: x => x.indicator_id,
                        principalSchema: "mande",
                        principalTable: "indicators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_plans_plans_main_plan_id",
                        column: x => x.main_plan_id,
                        principalSchema: "mande",
                        principalTable: "plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "settlments",
                schema: "payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receipt_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    payment_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    payment_method = table.Column<int>(type: "integer", nullable: false),
                    amount_paid = table.Column<decimal>(type: "numeric", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settlments", x => x.id);
                    table.ForeignKey(
                        name: "FK_settlments_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalSchema: "payment",
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "issues",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    issue_date = table.Column<DateTime>(type: "date", nullable: false),
                    contact_email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    contact_phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    contact_person_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    group_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    action_owner = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    reporting_means = table.Column<int>(type: "integer", nullable: false),
                    remark = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issues", x => x.id);
                    table.ForeignKey(
                        name: "FK_issues_issue_types_type_id",
                        column: x => x.type_id,
                        principalSchema: "membership",
                        principalTable: "issue_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "applicants",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    application_date = table.Column<DateTime>(type: "date", nullable: false),
                    tin = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    trade_registration_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    association_id = table.Column<Guid>(type: "uuid", nullable: false),
                    application_fee = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    annual_payment = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    invoice_no = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    recepit_no = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    remark = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    application_status = table.Column<int>(type: "integer", nullable: false),
                    document_status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicants", x => x.id);
                    table.ForeignKey(
                        name: "FK_applicants_locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "membership",
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lookup_values",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    lookup_type_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lookup_values", x => x.id);
                    table.ForeignKey(
                        name: "FK_lookup_values_lookup_types_lookup_type_id",
                        column: x => x.lookup_type_id,
                        principalTable: "lookup_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "campaign_partner_agencies",
                schema: "communication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    campaign_id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_agency_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    communicated_through = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 666, DateTimeKind.Utc).AddTicks(4513)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 666, DateTimeKind.Utc).AddTicks(5084))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campaign_partner_agencies", x => x.id);
                    table.ForeignKey(
                        name: "FK_campaign_partner_agencies_campaigns_campaign_id",
                        column: x => x.campaign_id,
                        principalSchema: "communication",
                        principalTable: "campaigns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_campaign_partner_agencies_media_agencies_media_agency_id",
                        column: x => x.media_agency_id,
                        principalSchema: "communication",
                        principalTable: "media_agencies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "media_coverages",
                schema: "communication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_agency_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    duration_minutes = table.Column<int>(type: "int", nullable: false),
                    frequency = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    covered_in = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    about = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    remark = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 678, DateTimeKind.Utc).AddTicks(5316)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 678, DateTimeKind.Utc).AddTicks(5973))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_coverages", x => x.id);
                    table.ForeignKey(
                        name: "FK_media_coverages_media_agencies_media_agency_id",
                        column: x => x.media_agency_id,
                        principalSchema: "communication",
                        principalTable: "media_agencies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meeting_artifacts",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    meeting_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meeting_artifacts", x => x.id);
                    table.ForeignKey(
                        name: "FK_meeting_artifacts_meetings_meeting_id",
                        column: x => x.meeting_id,
                        principalSchema: "membership",
                        principalTable: "meetings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meeting_participants",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    meeting_id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meeting_participants", x => x.id);
                    table.ForeignKey(
                        name: "FK_meeting_participants_meetings_meeting_id",
                        column: x => x.meeting_id,
                        principalSchema: "membership",
                        principalTable: "meetings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "need_assessment_details",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assessment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    need_rating = table.Column<int>(type: "int", nullable: false),
                    best_before = table.Column<DateTime>(type: "date", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_need_assessment_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_need_assessment_details_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "training",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_need_assessment_details_members_member_id",
                        column: x => x.member_id,
                        principalSchema: "training",
                        principalTable: "members",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_need_assessment_details_need_assessments_assessment_id",
                        column: x => x.assessment_id,
                        principalSchema: "training",
                        principalTable: "need_assessments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "commodity_prices",
                schema: "trade",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    market_date = table.Column<DateTime>(type: "date", nullable: false),
                    commodity_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price_unit_rate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    market_place_id = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceType = table.Column<int>(type: "integer", nullable: false),
                    price_value = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commodity_prices", x => x.id);
                    table.ForeignKey(
                        name: "FK_commodity_prices_commodity_types_commodity_type_id",
                        column: x => x.commodity_type_id,
                        principalSchema: "trade",
                        principalTable: "commodity_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_commodity_prices_market_places_market_place_id",
                        column: x => x.market_place_id,
                        principalSchema: "trade",
                        principalTable: "market_places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_commodity_prices_price_unit_rates_price_unit_rate_id",
                        column: x => x.price_unit_rate_id,
                        principalSchema: "trade",
                        principalTable: "price_unit_rates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_followups",
                schema: "advocacy",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    planned_start_date = table.Column<DateTime>(type: "date", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    planned_end_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: true),
                    remark = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 871, DateTimeKind.Utc).AddTicks(6987)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 871, DateTimeKind.Utc).AddTicks(7248))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_followups", x => x.id);
                    table.ForeignKey(
                        name: "FK_project_followups_projects_project_id",
                        column: x => x.project_id,
                        principalSchema: "advocacy",
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "report_field",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    report_id = table.Column<int>(type: "integer", nullable: false),
                    field_name = table.Column<string>(type: "text", nullable: false),
                    caption = table.Column<string>(type: "text", nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: false),
                    is_viewable = table.Column<bool>(type: "boolean", nullable: false),
                    is_group_field = table.Column<bool>(type: "boolean", nullable: false),
                    is_percent = table.Column<bool>(type: "boolean", maxLength: 50, nullable: false),
                    is_numeric = table.Column<bool>(type: "boolean", nullable: false),
                    is_filter_field = table.Column<bool>(type: "boolean", nullable: false),
                    dropdown_source = table.Column<string>(type: "text", nullable: true),
                    field_type_id = table.Column<int>(type: "integer", nullable: false),
                    is_default = table.Column<int>(type: "integer", nullable: false),
                    filter_field_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_field", x => x.id);
                    table.ForeignKey(
                        name: "FK_report_field_report_report_id",
                        column: x => x.report_id,
                        principalSchema: "dbo",
                        principalTable: "report",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_requests",
                schema: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    tin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    mobile = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    tracing_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    requesting_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    request_date = table.Column<DateTime>(type: "date", nullable: false),
                    invoice_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    amount = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    remark = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false, defaultValue: ""),
                    receipt_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    signature = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, defaultValue: ""),
                    payment_method = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    request_status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    as_member = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    description = table.Column<string>(type: "text", nullable: true),
                    previous_step = table.Column<Guid>(type: "uuid", nullable: true),
                    current_step = table.Column<Guid>(type: "uuid", nullable: false),
                    operator_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: ""),
                    approver_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: ""),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 707, DateTimeKind.Utc).AddTicks(9344)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 707, DateTimeKind.Utc).AddTicks(9640))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_requests_service_types_service_type_id",
                        column: x => x.service_type_id,
                        principalSchema: "services",
                        principalTable: "service_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_requirements",
                schema: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    requirement_type = table.Column<int>(type: "integer", nullable: false),
                    order_no = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name_eng = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name_local = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description_eng = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description_local = table.Column<string>(type: "text", nullable: false),
                    input_method = table.Column<int>(type: "integer", nullable: false),
                    upper_limit = table.Column<int>(type: "integer", nullable: false),
                    lower_limit = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    lookup_type_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_requirements", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_requirements_service_types_service_type_id",
                        column: x => x.service_type_id,
                        principalSchema: "services",
                        principalTable: "service_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_type_tasks",
                schema: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_no = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name_eng = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    description_eng = table.Column<string>(type: "text", nullable: false),
                    description_local = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_type_tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_type_tasks_service_types_service_type_id",
                        column: x => x.service_type_id,
                        principalSchema: "services",
                        principalTable: "service_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address_type = table.Column<int>(type: "int", nullable: false),
                    location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address_line = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_addresses_stakeholders_member_id",
                        column: x => x.member_id,
                        principalSchema: "membership",
                        principalTable: "stakeholders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "associations",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    member_since = table.Column<DateTime>(type: "date", nullable: false),
                    association_type = table.Column<int>(type: "integer", nullable: false),
                    association_level = table.Column<int>(type: "integer", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    location_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    registration_fee = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    renewal_fee = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    profile = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    logo = table.Column<byte[]>(type: "bytea", nullable: true),
                    extension = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: ""),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_associations", x => x.id);
                    table.ForeignKey(
                        name: "FK_associations_locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "membership",
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_associations_stakeholders_id",
                        column: x => x.id,
                        principalSchema: "membership",
                        principalTable: "stakeholders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contacts",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_type = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contacts", x => x.id);
                    table.ForeignKey(
                        name: "FK_contacts_stakeholders_member_id",
                        column: x => x.member_id,
                        principalSchema: "membership",
                        principalTable: "stakeholders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "individuals",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    member_since = table.Column<DateTime>(type: "date", nullable: false),
                    tin = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    trade_registration_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_individuals", x => x.id);
                    table.ForeignKey(
                        name: "FK_individuals_locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "membership",
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_individuals_stakeholders_id",
                        column: x => x.id,
                        principalSchema: "membership",
                        principalTable: "stakeholders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "representatives",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: ""),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    position = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_representatives", x => x.id);
                    table.ForeignKey(
                        name: "FK_representatives_stakeholders_member_id",
                        column: x => x.member_id,
                        principalSchema: "membership",
                        principalTable: "stakeholders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "survey_questions",
                schema: "surveying",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    setting_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    question = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    question_type = table.Column<int>(type: "integer", nullable: false),
                    min_range = table.Column<decimal>(type: "numeric", nullable: true),
                    max_range = table.Column<decimal>(type: "numeric", nullable: true),
                    cascade = table.Column<bool>(type: "boolean", nullable: false),
                    parent_choice_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey_questions", x => x.id);
                    table.ForeignKey(
                        name: "FK_survey_questions_survey_settings_setting_id",
                        column: x => x.setting_id,
                        principalSchema: "surveying",
                        principalTable: "survey_settings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "surveys",
                schema: "surveying",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    setting_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    analysis_description = table.Column<string>(type: "text", nullable: false),
                    attachment = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 829, DateTimeKind.Utc).AddTicks(7808)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 829, DateTimeKind.Utc).AddTicks(8107))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_surveys", x => x.id);
                    table.ForeignKey(
                        name: "FK_surveys_survey_settings_setting_id",
                        column: x => x.setting_id,
                        principalSchema: "surveying",
                        principalTable: "survey_settings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "arbitration_documents",
                schema: "arbitration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    document_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    date_submitted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    reviewd_by = table.Column<Guid>(type: "uuid", nullable: true),
                    date_reviewed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    remark = table.Column<string>(type: "text", nullable: true),
                    review_status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arbitration_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_arbitration_documents_arbitration_services_service_id",
                        column: x => x.service_id,
                        principalSchema: "arbitration",
                        principalTable: "arbitration_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "followups",
                schema: "arbitration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    planned_date = table.Column<DateTime>(type: "date", nullable: false),
                    followup_result_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    remark = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 814, DateTimeKind.Utc).AddTicks(468)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 814, DateTimeKind.Utc).AddTicks(927))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_followups", x => x.id);
                    table.ForeignKey(
                        name: "FK_followups_arbitration_services_service_id",
                        column: x => x.service_id,
                        principalSchema: "arbitration",
                        principalTable: "arbitration_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_followups_followup_result_types_followup_result_id",
                        column: x => x.followup_result_id,
                        principalSchema: "arbitration",
                        principalTable: "followup_result_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_followups_task_types_task_type_id",
                        column: x => x.task_type_id,
                        principalSchema: "arbitration",
                        principalTable: "task_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "related_parties",
                schema: "arbitration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_no = table.Column<string>(type: "varchar", maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_related_parties", x => x.id);
                    table.ForeignKey(
                        name: "FK_related_parties_arbitration_services_service_id",
                        column: x => x.service_id,
                        principalSchema: "arbitration",
                        principalTable: "arbitration_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resources",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    resource_type = table.Column<int>(type: "int", nullable: false),
                    resource = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resources", x => x.id);
                    table.ForeignKey(
                        name: "FK_resources_topics_topic_id",
                        column: x => x.topic_id,
                        principalSchema: "training",
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topic_settings",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    setting_variable_id = table.Column<int>(type: "int", nullable: false),
                    setting_value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_topic_settings_setting_variables_setting_variable_id",
                        column: x => x.setting_variable_id,
                        principalSchema: "training",
                        principalTable: "setting_variables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_topic_settings_topics_topic_id",
                        column: x => x.topic_id,
                        principalSchema: "training",
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trainings",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    instructor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    instruction_media = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    rate_basis = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    member_fee = table.Column<decimal>(type: "decimal", nullable: false),
                    non_member_fee = table.Column<decimal>(type: "decimal", nullable: false),
                    schedule = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 810, DateTimeKind.Utc).AddTicks(1527)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 810, DateTimeKind.Utc).AddTicks(1812))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trainings", x => x.id);
                    table.ForeignKey(
                        name: "FK_trainings_topics_topic_id",
                        column: x => x.topic_id,
                        principalSchema: "training",
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_details",
                schema: "eventmanagement",
                columns: table => new
                {
                    detail_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    servie_type_id = table.Column<Guid>(type: "uuid", nullable: true),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_details", x => x.detail_id);
                    table.ForeignKey(
                        name: "FK_event_details_events_event_id",
                        column: x => x.event_id,
                        principalSchema: "eventmanagement",
                        principalTable: "events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "progresses",
                schema: "mande",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    plan_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    physical = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    financial = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_progresses_plans_plan_id",
                        column: x => x.plan_id,
                        principalSchema: "mande",
                        principalTable: "plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "applicant_documents",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    applicant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_type_id = table.Column<int>(type: "int", nullable: false),
                    document = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    remark = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicant_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_applicant_documents_applicants_applicant_id",
                        column: x => x.applicant_id,
                        principalSchema: "membership",
                        principalTable: "applicants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applicant_documents_legal_document_types_document_type_id",
                        column: x => x.document_type_id,
                        principalSchema: "membership",
                        principalTable: "legal_document_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "operator_history",
                schema: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operator_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_operator_history_service_requests_service_id",
                        column: x => x.service_id,
                        principalSchema: "services",
                        principalTable: "service_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_responses",
                schema: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    requirement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    text_response = table.Column<string>(type: "text", nullable: true),
                    date_response = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    decimal_response = table.Column<decimal>(type: "numeric", nullable: true),
                    int_response = table.Column<int>(type: "integer", nullable: true),
                    date_submitted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lookup_id = table.Column<Guid>(type: "uuid", nullable: true),
                    invoice_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    invoice_amount = table.Column<decimal>(type: "numeric", nullable: true),
                    invoice_date = table.Column<DateTime>(type: "date", nullable: true),
                    receipt_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    amount_paid = table.Column<decimal>(type: "numeric", nullable: true),
                    date_paid = table.Column<DateTime>(type: "date", nullable: true),
                    reviewed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    date_reviewed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    remark = table.Column<string>(type: "text", nullable: false),
                    review_status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_responses", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_responses_service_requests_service_request_id",
                        column: x => x.service_request_id,
                        principalSchema: "services",
                        principalTable: "service_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_service_responses_service_requirements_requirement_id",
                        column: x => x.requirement_id,
                        principalSchema: "services",
                        principalTable: "service_requirements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_followups",
                schema: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    duration_hr = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    description = table.Column<string>(type: "text", nullable: false),
                    extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    is_last_task = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    next_task_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 713, DateTimeKind.Utc).AddTicks(7174)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 713, DateTimeKind.Utc).AddTicks(7478))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_followups", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_followups_service_requests_service_id",
                        column: x => x.service_id,
                        principalSchema: "services",
                        principalTable: "service_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_service_followups_service_type_tasks_task_type_id",
                        column: x => x.task_type_id,
                        principalSchema: "services",
                        principalTable: "service_type_tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "association_settings",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    association_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    account_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sub_system = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_association_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_association_settings_associations_association_id",
                        column: x => x.association_id,
                        principalSchema: "membership",
                        principalTable: "associations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "board_members",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    position_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    association_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_board_members", x => x.id);
                    table.ForeignKey(
                        name: "FK_board_members_associations_association_id",
                        column: x => x.association_id,
                        principalSchema: "membership",
                        principalTable: "associations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_board_members_position_types_position_type_id",
                        column: x => x.position_type_id,
                        principalSchema: "membership",
                        principalTable: "position_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: ""),
                    name_eng = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_local = table.Column<string>(type: "text", nullable: false),
                    position_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    association_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, defaultValue: ""),
                    mobile = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.id);
                    table.ForeignKey(
                        name: "FK_employees_associations_association_id",
                        column: x => x.association_id,
                        principalSchema: "membership",
                        principalTable: "associations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employees_position_types_position_type_id",
                        column: x => x.position_type_id,
                        principalSchema: "membership",
                        principalTable: "position_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "membership_enrollment",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    parent_association_id = table.Column<Guid>(type: "uuid", nullable: false),
                    member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_of_enrollment = table.Column<DateTime>(type: "date", nullable: false),
                    membership_status = table.Column<int>(type: "integer", nullable: false),
                    renewal_status = table.Column<int>(type: "integer", nullable: false),
                    document_status = table.Column<int>(type: "integer", nullable: false),
                    last_renewal_date = table.Column<DateTime>(type: "date", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "date", nullable: false),
                    application_fee = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    annual_payment = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    invoice_no = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    bank_recepit_no = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    remark = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    last_renewal_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_membership_enrollment", x => x.id);
                    table.ForeignKey(
                        name: "FK_membership_enrollment_associations_parent_association_id",
                        column: x => x.parent_association_id,
                        principalSchema: "membership",
                        principalTable: "associations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_membership_enrollment_stakeholders_member_id",
                        column: x => x.member_id,
                        principalSchema: "membership",
                        principalTable: "stakeholders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "membership_requirements",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    association_id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_type_id = table.Column<int>(type: "int", nullable: false),
                    application_requirement_level = table.Column<int>(type: "integer", nullable: false),
                    renewal_requirement_level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_membership_requirements", x => x.id);
                    table.ForeignKey(
                        name: "FK_membership_requirements_associations_association_id",
                        column: x => x.association_id,
                        principalSchema: "membership",
                        principalTable: "associations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_membership_requirements_legal_document_types_document_type_~",
                        column: x => x.document_type_id,
                        principalSchema: "membership",
                        principalTable: "legal_document_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "business_profile",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    individual_id = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_attribure_id = table.Column<int>(type: "int", nullable: false),
                    attribute_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_business_profile", x => x.id);
                    table.ForeignKey(
                        name: "FK_business_profile_individuals_individual_id",
                        column: x => x.individual_id,
                        principalSchema: "membership",
                        principalTable: "individuals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_business_profile_profile_attribure_profile_attribure_id",
                        column: x => x.profile_attribure_id,
                        principalSchema: "membership",
                        principalTable: "profile_attribure",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "legal_documents",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    individual_id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_type_id = table.Column<int>(type: "int", nullable: false),
                    document = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    remark = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false, defaultValue: ""),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legal_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_legal_documents_individuals_individual_id",
                        column: x => x.individual_id,
                        principalSchema: "membership",
                        principalTable: "individuals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_legal_documents_legal_document_types_document_type_id",
                        column: x => x.document_type_id,
                        principalSchema: "membership",
                        principalTable: "legal_document_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "representative_contacts",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    representative_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_type = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_representative_contacts", x => x.id);
                    table.ForeignKey(
                        name: "FK_representative_contacts_representatives_representative_id",
                        column: x => x.representative_id,
                        principalSchema: "membership",
                        principalTable: "representatives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "survey_choices",
                schema: "surveying",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    setting_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    choice_text = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey_choices", x => x.id);
                    table.ForeignKey(
                        name: "FK_survey_choices_survey_questions_question_id",
                        column: x => x.question_id,
                        principalSchema: "surveying",
                        principalTable: "survey_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_survey_choices_survey_settings_setting_id",
                        column: x => x.setting_id,
                        principalSchema: "surveying",
                        principalTable: "survey_settings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attachments",
                schema: "surveying",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    survey_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 841, DateTimeKind.Utc).AddTicks(5388)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 841, DateTimeKind.Utc).AddTicks(5730))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachments", x => x.id);
                    table.ForeignKey(
                        name: "FK_attachments_surveys_survey_id",
                        column: x => x.survey_id,
                        principalSchema: "surveying",
                        principalTable: "surveys",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "survey_audiences",
                schema: "surveying",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    survey_id = table.Column<Guid>(type: "uuid", nullable: false),
                    stakeholder_type = table.Column<int>(type: "integer", nullable: false),
                    association_type = table.Column<int>(type: "integer", nullable: false),
                    national = table.Column<bool>(type: "boolean", nullable: false),
                    regional = table.Column<bool>(type: "boolean", nullable: false),
                    woreda = table.Column<bool>(type: "boolean", nullable: false),
                    include_members = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey_audiences", x => x.id);
                    table.ForeignKey(
                        name: "FK_survey_audiences_surveys_survey_id",
                        column: x => x.survey_id,
                        principalSchema: "surveying",
                        principalTable: "surveys",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "survey_respondents",
                schema: "surveying",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    survey_id = table.Column<Guid>(type: "uuid", nullable: false),
                    respondent_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 835, DateTimeKind.Utc).AddTicks(3226)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 835, DateTimeKind.Utc).AddTicks(3516))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey_respondents", x => x.id);
                    table.ForeignKey(
                        name: "FK_survey_respondents_surveys_survey_id",
                        column: x => x.survey_id,
                        principalSchema: "surveying",
                        principalTable: "surveys",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "training_sponsors",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reference_no = table.Column<string>(type: "text", nullable: true),
                    training_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sponsor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attachment = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_training_sponsors", x => x.id);
                    table.ForeignKey(
                        name: "FK_training_sponsors_sponsors_sponsor_id",
                        column: x => x.sponsor_id,
                        principalSchema: "training",
                        principalTable: "sponsors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_training_sponsors_trainings_training_id",
                        column: x => x.training_id,
                        principalSchema: "training",
                        principalTable: "trainings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "renewal_history",
                schema: "membership",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    enrollment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    application_date = table.Column<DateTime>(type: "date", nullable: false),
                    renewal_status = table.Column<int>(type: "integer", nullable: false),
                    renewal_date = table.Column<DateTime>(type: "date", nullable: false),
                    fee = table.Column<decimal>(type: "decimal", nullable: false),
                    invoice_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    invoice_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    receipt_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    payment_method = table.Column<int>(type: "integer", nullable: false),
                    payment_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_renewal_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_renewal_history_membership_enrollment_enrollment_id",
                        column: x => x.enrollment_id,
                        principalSchema: "membership",
                        principalTable: "membership_enrollment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "selected_associations",
                schema: "surveying",
                columns: table => new
                {
                    audience_id = table.Column<Guid>(type: "uuid", nullable: false),
                    association_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_selected_associations", x => new { x.audience_id, x.association_id });
                    table.ForeignKey(
                        name: "FK_selected_associations_associations_association_id",
                        column: x => x.association_id,
                        principalSchema: "membership",
                        principalTable: "associations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_selected_associations_survey_audiences_audience_id",
                        column: x => x.audience_id,
                        principalSchema: "surveying",
                        principalTable: "survey_audiences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "survey_responses",
                schema: "surveying",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    respondent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    survey_id = table.Column<Guid>(type: "uuid", nullable: false),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    numeric_response = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    textual_response = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey_responses", x => x.id);
                    table.ForeignKey(
                        name: "FK_survey_responses_survey_respondents_respondent_id",
                        column: x => x.respondent_id,
                        principalSchema: "surveying",
                        principalTable: "survey_respondents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_survey_responses_surveys_survey_id",
                        column: x => x.survey_id,
                        principalSchema: "surveying",
                        principalTable: "surveys",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "registrations",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    training_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_id = table.Column<Guid>(type: "uuid", nullable: false),
                    training_sponsor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email_address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    mobile_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    registration_date = table.Column<DateTime>(type: "date", nullable: false),
                    registration_status = table.Column<int>(type: "int", nullable: false),
                    discount_category = table.Column<int>(type: "integer", nullable: false),
                    training_rating = table.Column<int>(type: "int", nullable: false),
                    training_feedback = table.Column<string>(type: "text", nullable: true),
                    certificate_attachment = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    invoice_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    invoice_rate = table.Column<decimal>(type: "numeric", nullable: true),
                    as_member = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    remark = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false, defaultValue: ""),
                    receipt_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    payment_method = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 794, DateTimeKind.Utc).AddTicks(3688)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 794, DateTimeKind.Utc).AddTicks(3965))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registrations", x => x.id);
                    table.ForeignKey(
                        name: "FK_registrations_training_sponsors_training_sponsor_id",
                        column: x => x.training_sponsor_id,
                        principalSchema: "training",
                        principalTable: "training_sponsors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_registrations_trainings_training_id",
                        column: x => x.training_id,
                        principalSchema: "training",
                        principalTable: "trainings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "selected_choices",
                schema: "surveying",
                columns: table => new
                {
                    response_id = table.Column<Guid>(type: "uuid", nullable: false),
                    choice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rank = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_selected_choices", x => new { x.response_id, x.choice_id });
                    table.ForeignKey(
                        name: "FK_selected_choices_survey_choices_choice_id",
                        column: x => x.choice_id,
                        principalSchema: "surveying",
                        principalTable: "survey_choices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_selected_choices_survey_responses_response_id",
                        column: x => x.response_id,
                        principalSchema: "surveying",
                        principalTable: "survey_responses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assessments",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    training_id = table.Column<Guid>(type: "uuid", nullable: false),
                    registration_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    result_score = table.Column<int>(type: "int", nullable: false),
                    result_pass = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    create_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 780, DateTimeKind.Utc).AddTicks(6466)),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    update_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2021, 11, 15, 13, 36, 16, 780, DateTimeKind.Utc).AddTicks(6731))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessments", x => x.id);
                    table.ForeignKey(
                        name: "FK_assessments_registrations_registration_id",
                        column: x => x.registration_id,
                        principalSchema: "training",
                        principalTable: "registrations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assessments_trainings_training_id",
                        column: x => x.training_id,
                        principalSchema: "training",
                        principalTable: "trainings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attendances",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    training_id = table.Column<Guid>(type: "uuid", nullable: false),
                    registration_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attendance_date = table.Column<DateTime>(type: "date", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendances", x => x.id);
                    table.ForeignKey(
                        name: "FK_attendances_registrations_registration_id",
                        column: x => x.registration_id,
                        principalSchema: "training",
                        principalTable: "registrations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attendances_trainings_training_id",
                        column: x => x.training_id,
                        principalSchema: "training",
                        principalTable: "trainings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resource_delivery",
                schema: "training",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    delivery_method = table.Column<int>(type: "int", nullable: false),
                    resource_id = table.Column<Guid>(type: "uuid", nullable: false),
                    registration_id = table.Column<Guid>(type: "uuid", nullable: false),
                    delivery_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resource_delivery", x => x.id);
                    table.ForeignKey(
                        name: "FK_resource_delivery_registrations_registration_id",
                        column: x => x.registration_id,
                        principalSchema: "training",
                        principalTable: "registrations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_resource_delivery_resources_resource_id",
                        column: x => x.resource_id,
                        principalSchema: "training",
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_addresses_member_id",
                schema: "membership",
                table: "addresses",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_agreement_commodities_agreement_id",
                schema: "services",
                table: "agreement_commodities",
                column: "agreement_id");

            migrationBuilder.CreateIndex(
                name: "IX_applicant_documents_applicant_id",
                schema: "membership",
                table: "applicant_documents",
                column: "applicant_id");

            migrationBuilder.CreateIndex(
                name: "IX_applicant_documents_document_type_id",
                schema: "membership",
                table: "applicant_documents",
                column: "document_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_applicants_location_id",
                schema: "membership",
                table: "applicants",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_arbitration_documents_service_id",
                schema: "arbitration",
                table: "arbitration_documents",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_arbitration_services_service_type_id",
                schema: "arbitration",
                table: "arbitration_services",
                column: "service_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_assessments_registration_id",
                schema: "training",
                table: "assessments",
                column: "registration_id");

            migrationBuilder.CreateIndex(
                name: "IX_assessments_training_id",
                schema: "training",
                table: "assessments",
                column: "training_id");

            migrationBuilder.CreateIndex(
                name: "IX_association_settings_association_id",
                schema: "membership",
                table: "association_settings",
                column: "association_id");

            migrationBuilder.CreateIndex(
                name: "IX_associations_location_id",
                schema: "membership",
                table: "associations",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_attachments_survey_id",
                schema: "surveying",
                table: "attachments",
                column: "survey_id");

            migrationBuilder.CreateIndex(
                name: "IX_attendances_registration_id",
                schema: "training",
                table: "attendances",
                column: "registration_id");

            migrationBuilder.CreateIndex(
                name: "IX_attendances_training_id",
                schema: "training",
                table: "attendances",
                column: "training_id");

            migrationBuilder.CreateIndex(
                name: "IX_board_members_association_id",
                schema: "membership",
                table: "board_members",
                column: "association_id");

            migrationBuilder.CreateIndex(
                name: "IX_board_members_position_type_id",
                schema: "membership",
                table: "board_members",
                column: "position_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_business_profile_individual_id",
                schema: "membership",
                table: "business_profile",
                column: "individual_id");

            migrationBuilder.CreateIndex(
                name: "IX_business_profile_profile_attribure_id",
                schema: "membership",
                table: "business_profile",
                column: "profile_attribure_id");

            migrationBuilder.CreateIndex(
                name: "IX_campaign_partner_agencies_campaign_id",
                schema: "communication",
                table: "campaign_partner_agencies",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_campaign_partner_agencies_media_agency_id",
                schema: "communication",
                table: "campaign_partner_agencies",
                column: "media_agency_id");

            migrationBuilder.CreateIndex(
                name: "IX_categories_parent_id",
                schema: "training",
                table: "categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_commodity_prices_commodity_type_id",
                schema: "trade",
                table: "commodity_prices",
                column: "commodity_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_commodity_prices_market_place_id",
                schema: "trade",
                table: "commodity_prices",
                column: "market_place_id");

            migrationBuilder.CreateIndex(
                name: "IX_commodity_prices_price_unit_rate_id",
                schema: "trade",
                table: "commodity_prices",
                column: "price_unit_rate_id");

            migrationBuilder.CreateIndex(
                name: "IX_contacts_member_id",
                schema: "membership",
                table: "contacts",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_association_id",
                schema: "membership",
                table: "employees",
                column: "association_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_position_type_id",
                schema: "membership",
                table: "employees",
                column: "position_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_details_event_id",
                schema: "eventmanagement",
                table: "event_details",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_events_event_type_id",
                schema: "eventmanagement",
                table: "events",
                column: "event_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_flags_flag_type_id",
                table: "flags",
                column: "flag_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_followups_followup_result_id",
                schema: "arbitration",
                table: "followups",
                column: "followup_result_id");

            migrationBuilder.CreateIndex(
                name: "IX_followups_service_id",
                schema: "arbitration",
                table: "followups",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_followups_task_type_id",
                schema: "arbitration",
                table: "followups",
                column: "task_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_indicators_parent_id",
                schema: "mande",
                table: "indicators",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_individuals_location_id",
                schema: "membership",
                table: "individuals",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_issues_type_id",
                schema: "membership",
                table: "issues",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_keywords_content_id",
                schema: "website",
                table: "keywords",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_documents_document_type_id",
                schema: "membership",
                table: "legal_documents",
                column: "document_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_documents_individual_id",
                schema: "membership",
                table: "legal_documents",
                column: "individual_id");

            migrationBuilder.CreateIndex(
                name: "IX_lookup_values_lookup_type_id",
                table: "lookup_values",
                column: "lookup_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_media_coverages_media_agency_id",
                schema: "communication",
                table: "media_coverages",
                column: "media_agency_id");

            migrationBuilder.CreateIndex(
                name: "IX_meeting_artifacts_meeting_id",
                schema: "membership",
                table: "meeting_artifacts",
                column: "meeting_id");

            migrationBuilder.CreateIndex(
                name: "IX_meeting_participants_meeting_id",
                schema: "membership",
                table: "meeting_participants",
                column: "meeting_id");

            migrationBuilder.CreateIndex(
                name: "IX_membership_enrollment_member_id",
                schema: "membership",
                table: "membership_enrollment",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_membership_enrollment_parent_association_id",
                schema: "membership",
                table: "membership_enrollment",
                column: "parent_association_id");

            migrationBuilder.CreateIndex(
                name: "IX_membership_requirements_association_id",
                schema: "membership",
                table: "membership_requirements",
                column: "association_id");

            migrationBuilder.CreateIndex(
                name: "IX_membership_requirements_document_type_id",
                schema: "membership",
                table: "membership_requirements",
                column: "document_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_need_assessment_details_assessment_id",
                schema: "training",
                table: "need_assessment_details",
                column: "assessment_id");

            migrationBuilder.CreateIndex(
                name: "IX_need_assessment_details_category_id",
                schema: "training",
                table: "need_assessment_details",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_need_assessment_details_member_id",
                schema: "training",
                table: "need_assessment_details",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_operator_history_service_id",
                schema: "services",
                table: "operator_history",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_plans_indicator_id",
                schema: "mande",
                table: "plans",
                column: "indicator_id");

            migrationBuilder.CreateIndex(
                name: "IX_plans_main_plan_id",
                schema: "mande",
                table: "plans",
                column: "main_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_progresses_plan_id",
                schema: "mande",
                table: "progresses",
                column: "plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_followups_project_id",
                schema: "advocacy",
                table: "project_followups",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_training_id",
                schema: "training",
                table: "registrations",
                column: "training_id");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_training_sponsor_id",
                schema: "training",
                table: "registrations",
                column: "training_sponsor_id");

            migrationBuilder.CreateIndex(
                name: "IX_related_parties_service_id",
                schema: "arbitration",
                table: "related_parties",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_renewal_history_enrollment_id",
                schema: "membership",
                table: "renewal_history",
                column: "enrollment_id");

            migrationBuilder.CreateIndex(
                name: "IX_report_field_report_id",
                schema: "dbo",
                table: "report_field",
                column: "report_id");

            migrationBuilder.CreateIndex(
                name: "IX_representative_contacts_representative_id",
                schema: "membership",
                table: "representative_contacts",
                column: "representative_id");

            migrationBuilder.CreateIndex(
                name: "IX_representatives_member_id",
                schema: "membership",
                table: "representatives",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_resource_delivery_registration_id",
                schema: "training",
                table: "resource_delivery",
                column: "registration_id");

            migrationBuilder.CreateIndex(
                name: "IX_resource_delivery_resource_id",
                schema: "training",
                table: "resource_delivery",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_resources_topic_id",
                schema: "training",
                table: "resources",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "IX_selected_associations_association_id",
                schema: "surveying",
                table: "selected_associations",
                column: "association_id");

            migrationBuilder.CreateIndex(
                name: "IX_selected_choices_choice_id",
                schema: "surveying",
                table: "selected_choices",
                column: "choice_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_followups_service_id",
                schema: "services",
                table: "service_followups",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_followups_task_type_id",
                schema: "services",
                table: "service_followups",
                column: "task_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_requests_service_type_id",
                schema: "services",
                table: "service_requests",
                column: "service_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_requirements_service_type_id",
                schema: "services",
                table: "service_requirements",
                column: "service_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_responses_requirement_id",
                schema: "services",
                table: "service_responses",
                column: "requirement_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_responses_service_request_id",
                schema: "services",
                table: "service_responses",
                column: "service_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_type_tasks_service_type_id",
                schema: "services",
                table: "service_type_tasks",
                column: "service_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_settlments_invoice_id",
                schema: "payment",
                table: "settlments",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_survey_audiences_survey_id",
                schema: "surveying",
                table: "survey_audiences",
                column: "survey_id");

            migrationBuilder.CreateIndex(
                name: "IX_survey_choices_question_id",
                schema: "surveying",
                table: "survey_choices",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_survey_choices_setting_id",
                schema: "surveying",
                table: "survey_choices",
                column: "setting_id");

            migrationBuilder.CreateIndex(
                name: "IX_survey_questions_setting_id",
                schema: "surveying",
                table: "survey_questions",
                column: "setting_id");

            migrationBuilder.CreateIndex(
                name: "IX_survey_respondents_survey_id",
                schema: "surveying",
                table: "survey_respondents",
                column: "survey_id");

            migrationBuilder.CreateIndex(
                name: "IX_survey_responses_respondent_id",
                schema: "surveying",
                table: "survey_responses",
                column: "respondent_id");

            migrationBuilder.CreateIndex(
                name: "IX_survey_responses_survey_id",
                schema: "surveying",
                table: "survey_responses",
                column: "survey_id");

            migrationBuilder.CreateIndex(
                name: "IX_surveys_setting_id",
                schema: "surveying",
                table: "surveys",
                column: "setting_id");

            migrationBuilder.CreateIndex(
                name: "IX_topic_settings_setting_variable_id",
                schema: "training",
                table: "topic_settings",
                column: "setting_variable_id");

            migrationBuilder.CreateIndex(
                name: "IX_topic_settings_topic_id",
                schema: "training",
                table: "topic_settings",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "IX_topics_category_id",
                schema: "training",
                table: "topics",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_training_sponsors_sponsor_id",
                schema: "training",
                table: "training_sponsors",
                column: "sponsor_id");

            migrationBuilder.CreateIndex(
                name: "IX_training_sponsors_training_id",
                schema: "training",
                table: "training_sponsors",
                column: "training_id");

            migrationBuilder.CreateIndex(
                name: "IX_trainings_topic_id",
                schema: "training",
                table: "trainings",
                column: "topic_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addresses",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "advertizements",
                schema: "website");

            migrationBuilder.DropTable(
                name: "agreement_commodities",
                schema: "services");

            migrationBuilder.DropTable(
                name: "applicant_documents",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "arbitration_documents",
                schema: "arbitration");

            migrationBuilder.DropTable(
                name: "assessments",
                schema: "training");

            migrationBuilder.DropTable(
                name: "association_settings",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "attachments",
                schema: "surveying");

            migrationBuilder.DropTable(
                name: "attendances",
                schema: "training");

            migrationBuilder.DropTable(
                name: "board_members",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "business_information",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "business_profile",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "businesses",
                schema: "investment");

            migrationBuilder.DropTable(
                name: "campaign_partner_agencies",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "commodity_prices",
                schema: "trade");

            migrationBuilder.DropTable(
                name: "contacts",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "currencies",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "currencies",
                schema: "trade");

            migrationBuilder.DropTable(
                name: "dialogs",
                schema: "advocacy");

            migrationBuilder.DropTable(
                name: "discount_categories",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "email_messages",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "email_templates",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "employees",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "event_details",
                schema: "eventmanagement");

            migrationBuilder.DropTable(
                name: "exchange_rates",
                schema: "trade");

            migrationBuilder.DropTable(
                name: "external_data_types");

            migrationBuilder.DropTable(
                name: "flags");

            migrationBuilder.DropTable(
                name: "followups",
                schema: "arbitration");

            migrationBuilder.DropTable(
                name: "import_exports",
                schema: "trade");

            migrationBuilder.DropTable(
                name: "issues",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "keywords",
                schema: "website");

            migrationBuilder.DropTable(
                name: "legal_documents",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "legal_profiles",
                schema: "services");

            migrationBuilder.DropTable(
                name: "lookup_values");

            migrationBuilder.DropTable(
                name: "media_coverages",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "meeting_artifacts",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "meeting_participants",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "membership_requirements",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "need_assessment_details",
                schema: "training");

            migrationBuilder.DropTable(
                name: "news",
                schema: "website");

            migrationBuilder.DropTable(
                name: "operator_history",
                schema: "services");

            migrationBuilder.DropTable(
                name: "partners",
                schema: "website");

            migrationBuilder.DropTable(
                name: "progresses",
                schema: "mande");

            migrationBuilder.DropTable(
                name: "project_followups",
                schema: "advocacy");

            migrationBuilder.DropTable(
                name: "publications",
                schema: "advocacy");

            migrationBuilder.DropTable(
                name: "recipients",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "related_parties",
                schema: "arbitration");

            migrationBuilder.DropTable(
                name: "renewal_history",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "report_field",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "representative_contacts",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "researches",
                schema: "advocacy");

            migrationBuilder.DropTable(
                name: "resource_delivery",
                schema: "training");

            migrationBuilder.DropTable(
                name: "resources",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "selected_associations",
                schema: "surveying");

            migrationBuilder.DropTable(
                name: "selected_choices",
                schema: "surveying");

            migrationBuilder.DropTable(
                name: "service_followups",
                schema: "services");

            migrationBuilder.DropTable(
                name: "service_owners",
                schema: "services");

            migrationBuilder.DropTable(
                name: "service_responses",
                schema: "services");

            migrationBuilder.DropTable(
                name: "settlments",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "sms_messages",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "task_templates",
                schema: "services");

            migrationBuilder.DropTable(
                name: "topic_settings",
                schema: "training");

            migrationBuilder.DropTable(
                name: "user_contacts",
                schema: "security");

            migrationBuilder.DropTable(
                name: "yes_no");

            migrationBuilder.DropTable(
                name: "export_agreements",
                schema: "services");

            migrationBuilder.DropTable(
                name: "applicants",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "profile_attribure",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "campaigns",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "commodity_types",
                schema: "trade");

            migrationBuilder.DropTable(
                name: "market_places",
                schema: "trade");

            migrationBuilder.DropTable(
                name: "price_unit_rates",
                schema: "trade");

            migrationBuilder.DropTable(
                name: "position_types",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "events",
                schema: "eventmanagement");

            migrationBuilder.DropTable(
                name: "flag_types");

            migrationBuilder.DropTable(
                name: "followup_result_types",
                schema: "arbitration");

            migrationBuilder.DropTable(
                name: "task_types",
                schema: "arbitration");

            migrationBuilder.DropTable(
                name: "issue_types",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "individuals",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "lookup_types");

            migrationBuilder.DropTable(
                name: "media_agencies",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "meetings",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "legal_document_types",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "members",
                schema: "training");

            migrationBuilder.DropTable(
                name: "need_assessments",
                schema: "training");

            migrationBuilder.DropTable(
                name: "contents",
                schema: "website");

            migrationBuilder.DropTable(
                name: "plans",
                schema: "mande");

            migrationBuilder.DropTable(
                name: "projects",
                schema: "advocacy");

            migrationBuilder.DropTable(
                name: "arbitration_services",
                schema: "arbitration");

            migrationBuilder.DropTable(
                name: "membership_enrollment",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "report",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "representatives",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "registrations",
                schema: "training");

            migrationBuilder.DropTable(
                name: "resources",
                schema: "training");

            migrationBuilder.DropTable(
                name: "survey_audiences",
                schema: "surveying");

            migrationBuilder.DropTable(
                name: "survey_choices",
                schema: "surveying");

            migrationBuilder.DropTable(
                name: "survey_responses",
                schema: "surveying");

            migrationBuilder.DropTable(
                name: "service_type_tasks",
                schema: "services");

            migrationBuilder.DropTable(
                name: "service_requests",
                schema: "services");

            migrationBuilder.DropTable(
                name: "service_requirements",
                schema: "services");

            migrationBuilder.DropTable(
                name: "invoices",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "setting_variables",
                schema: "training");

            migrationBuilder.DropTable(
                name: "event_types",
                schema: "eventmanagement");

            migrationBuilder.DropTable(
                name: "indicators",
                schema: "mande");

            migrationBuilder.DropTable(
                name: "arbitration_service_types",
                schema: "arbitration");

            migrationBuilder.DropTable(
                name: "associations",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "training_sponsors",
                schema: "training");

            migrationBuilder.DropTable(
                name: "survey_questions",
                schema: "surveying");

            migrationBuilder.DropTable(
                name: "survey_respondents",
                schema: "surveying");

            migrationBuilder.DropTable(
                name: "service_types",
                schema: "services");

            migrationBuilder.DropTable(
                name: "locations",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "stakeholders",
                schema: "membership");

            migrationBuilder.DropTable(
                name: "sponsors",
                schema: "training");

            migrationBuilder.DropTable(
                name: "trainings",
                schema: "training");

            migrationBuilder.DropTable(
                name: "surveys",
                schema: "surveying");

            migrationBuilder.DropTable(
                name: "topics",
                schema: "training");

            migrationBuilder.DropTable(
                name: "survey_settings",
                schema: "surveying");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "training");
        }
    }
}
