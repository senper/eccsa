const serviceTypes = [
  {
    id: "1",
    name: "Health Certificate",
  },
  {
    id: "2",
    name: "Commercial Invoice",
  },
  {
    id: "3",
    name: "Track way bill",
  },
  {
    id: "4",
    name: "Container list",
  },
  {
    id: "5",
    name: "Proforma Invoice",
  },
  {
    id: "6",
    name: "Document authentication",
  },
  {
    id: "7",
    name: "Recommendation letter",
  },
  {
    id: "8",
    name: "Free sale certificate",
  },
  {
    id: "9",
    name: "ICO",
  },
];

const responsibleUnits = [
  {
    id: 1,
    name: "Chamber Academy",
  },
  {
    id: 2,
    name: "Ethiopian Chamber Arbitration Center",
  },
  {
    id: 3,
    name: "Event Management",
  },
  {
    id: 4,
    name: "Export and Trade Promotion",
  },
  {
    id: 5,
    name: "ICT Services",
  },
  {
    id: 6,
    name: "Investment and Trade Promotion",
  },
  {
    id: 7,
    name: "Membership and Investment",
  },
  {
    id: 8,
    name: "Trade Information",
  },
];

const mediaTypes = [
  {
    id: 1,
    name: "SMS",
  },
  {
    id: 2,
    name: "Email",
  },
  {
    id: 3,
    name: "Website",
  },
  {
    id: 4,
    name: "Telegram",
  },
];

const topics = [
  {
    id: 1,
    name: "Training Capacity",
  },
  {
    id: 2,
    name: "Training Fee",
  },
];

const messageTypes = [
  {
    id: 1,
    name: "FYI",
  },
  {
    id: 2,
    name: "Please Reply",
  },
];

const placeholders = [
  {
    id: 1,
    name: "Current Date",
  },
  {
    id: 2,
    name: "Month",
  },
  {
    id: 3,
    name: "Month Et",
  },
  {
    id: 4,
    name: "Quarter",
  },
  {
    id: 5,
    name: "Year",
  },
  {
    id: 6,
    name: "Year Et",
  },
  {
    id: 7,
    name: "Fiscal Year",
  },
  {
    id: 8,
    name: "Fiscal Year Et",
  },
  {
    id: 9,
    name: "Telephone",
  },
  {
    id: 10,
    name: "Email",
  },
  {
    id: 11,
    name: "Member Company Name",
  },
  {
    id: 12,
    name: "Primary Representative Name",
  },
  {
    id: 13,
    name: "Contact Name",
  },
];

const messageTemplates = [
  {
    id: "1",
    message: "Dear member {[MemberName]}, please pay training fee",
    media_type: "SMS",
    topic: "Training Fee",
    message_type: "Please Reply",
    is_recurrent: "No",
  },
  {
    id: "2",
    message: "Yearly survey under taking. Please visit our web site ",
    media_type: "SMS",
    topic: "Survey",
    message_type: "FYI",
    is_recurrent: "No",
  },
];

const AdministrationLevels = [
  {
    id: 0,
    name: "Federal",
  },
  {
    id: 1,
    name: "Regional",
  },
  {
    id: 0,
    name: "Woreda",
  },
];

const Modules = [
  {
    id: 8,
    name: "Academy",
    layout: "/academy",
  },
  {
    id: 16,
    name: "Arbitration",
    layout: "/arbitration",
  },
  {
    id: 2,
    name: "Communication",
    layout: "/communication",
  },
  {
    id: 4,
    name: "Investment",
    layout: "/investment",
  },
  {
    id: 1,
    name: "Membership",
    layout: "/membership",
  },
  {
    id: 32,
    name: "Research",
    layout: "/research",
  },
  {
    id: 64,
    name: "Trade Promotion",
    layout: "/tradePromotion",
  },
  {
    id: 128,
    name: "Administration",
    layout: "/configuration",
  },
  {
    id: 256,
    name: "Reporting",
    layout: "/reporting",
  },
];

const statuses = [
  {
    id: 1,
    name: "Active",
  },
  {
    id: 2,
    name: "Inactive",
  },
];

export {
  serviceTypes,
  responsibleUnits,
  mediaTypes,
  messageTemplates,
  topics,
  messageTypes,
  placeholders,
  AdministrationLevels,
  Modules,
  statuses,
};
