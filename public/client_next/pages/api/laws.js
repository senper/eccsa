import axios from "axios";

const config = {
  clientId: "react.client",
  listUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiLaws/List`,
  downloadUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiLaws/Download`,
  scopes: [],
};

export const lawDownloadUrl = config.downloadUri;

export const getLawsList = (category, type) => {
  return axios.get(
    config.listUri + "?category=" + category + "&lawType=" + type
  );
};

export const downloadLaw = (id, isLocal) => {
  return axios.get(config.downloadUri + "/" + id + "/" + isLocal);
};

export const lawTypes = [
  { value: 1, text: "Proclamation", textLocal: "አዋጅ" },
  { value: 2, text: "Regulation", textLocal: "ደንብ" },
  { value: 3, text: "Directive", textLocal: "መመሪያ" },
];

export const lawCategories = [
  { value: 1, text: "Trade", textLocal: "የንግድ" },
  { value: 2, text: "Investment", textLocal: "የኢንቨስትመንት" },
  { value: 3, text: "Customs", textLocal: "የጉምሩክ" },
  { value: 4, text: "Domestic", textLocal: "የየሀገር ውስጥ" },
  { value: 5, text: "Land", textLocal: "የመሬት" },
  { value: 99, text: "Other", textLocal: "የሌላ" },
];
