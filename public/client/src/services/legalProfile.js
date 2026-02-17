import axios from "axios";
const apiController = "/api/Home";
const config = {
  clientId: "react.client",
  saveLegalProfileUri: `${process.env.REACT_APP_API_URL}${apiController}/SaveLegalProfile`,
  getLegalProfilesUri: `${process.env.REACT_APP_API_URL}${apiController}/GetLegalProfiles`,
  getLegalProfileLookupUri: `${process.env.REACT_APP_API_URL}${apiController}/GetLegalProfileLookup`,
  getLegalProfileUri: `${process.env.REACT_APP_API_URL}${apiController}/GetLegalProfile`,
  removeLegalProfileUri: `${process.env.REACT_APP_API_URL}${apiController}/DeleteLegalProfile`,
  scopes: [],
};
export const saveLegalProfile = (data) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.saveLegalProfileUri, data);
  /* } else {
        return null;
    } */
};

export const getLegalProfiles = (status) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getLegalProfilesUri + "?status=" + status);
  /* } else {
        return null;
    } */
};

export const getLegalProfileLookup = (status) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getLegalProfileLookupUri + "?status=" + status);
  /* } else {
        return null;
    } */
};
export const getLegalProfile = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getLegalProfileUri + "/" + id);
  /* } else {
        return null;
    } */
};

export const deleteLegalProfile = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.removeLegalProfileUri + "/" + id);
  /* } else {
        return null;
    } */
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
