import axios from "axios";
const apiController = "/api/Home";
const config = {
  clientId: "react.client",
  saveLegalProfileUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveLegalProfile`,
  getLegalProfilesUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetLegalProfiles`,
  getLegalProfileLookupUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetLegalProfileLookup`,
  getLegalProfileUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetLegalProfile`,
  removeLegalProfileUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/DeleteLegalProfile`,
  scopes: [],
};
export const saveLegalProfile = (data) => {
  return axios.post(config.saveLegalProfileUri, data);
};

export const getLegalProfiles = (status) => {
  var token = localStorage.getItem("access_token");
  return axios.get(config.getLegalProfilesUri + "?status=" + status);
};

export const getLegalProfileLookup = (status) => {
  var token = localStorage.getItem("access_token");
  return axios.get(config.getLegalProfileLookupUri + "?status=" + status);
};
export const getLegalProfile = (id) => {
  var token = localStorage.getItem("access_token");
  return axios.get(config.getLegalProfileUri + "/" + id);
};

export const deleteLegalProfile = (id) => {
  return axios.post(config.removeLegalProfileUri + "/" + id);
};
export const LegalDocumentTypes = [
  { name: "Trade Registration", value: 1 },
  { name: "Trade License", value: 2 },
  { name: "TIN Certificate", value: 3 },
  { name: "VAT Certificate", value: 4 },
  { name: "Export License", value: 5 },
  { name: "Article Of Incorporate", value: 6 },
  { name: "Article Of Constitution", value: 7 },
  { name: "Audit Report", value: 8 },
  { name: "Agreement", value: 9 },
  { name: "Other", value: 99 },
];
