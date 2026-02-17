import axios from "axios";
const apiController = "/api/Home";
const config = {
  clientId: "react.client",
  saveExportAgreementUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveExportAgreement`,
  saveCommodityListUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveCommodityList`,
  getExportAgreementsUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetExportAgreements`,
  getExportAgreementLookupUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetExportAgreementLookup`,
  getExportAgreementUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetExportAgreement`,
  removeExportAgreementUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/DeleteExportAgreement`,
  scopes: [],
};
export const saveExportAgreement = (data) => {
  return axios.post(config.saveExportAgreementUri, data);
};

export const saveCommodityList = (id, data) => {
  return axios.post(config.saveCommodityListUri + "/" + id, data);
};

export const getExportAgreements = (status) => {
  return axios.get(config.getExportAgreementsUri + "?status=" + status);
};

export const getExportAgreementLookup = (status) => {
  return axios.get(config.getExportAgreementLookupUri + "?status=" + status);
};
export const getExportAgreement = (id) => {
  return axios.get(config.getExportAgreementUri + "/" + id);
};
export const deleteExportAgreement = (id) => {
  return axios.post(config.removeExportAgreementUri + "/" + id);
};
