import axios from "axios";
const apiController = "/api/ApiBusinessInformation";
const config = {
  clientId: "react.client",
  getBusinessInformationListUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/BusinessInformationList`,
  getPastBusinessInformationListUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/PastBusinessInformationList`,
  getBusinessInformationUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetBusinessInformation`,
  getSectorsUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/Sectors`,
  getSectorCompaniesUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SectorCompanies`,
  getCompaniesByKeyUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/CompaniesByKey`,
  getInvestmentProfilesListUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetInvestmentProfilesList`,
  getInvestmentProfileUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetInvestmentProfile`,
  scopes: [],
};

export const getInvestmentProfilesList = () => {
  return axios.get(config.getInvestmentProfilesListUrl);
};
export const getInvestmentProfile = (id) => {
  return axios.get(config.getInvestmentProfileUrl + "/" + id);
};
export const getSectorsList = () => {
  return axios.get(config.getSectorsUrl);
};
export const getSectorCompanies = (id) => {
  return axios.get(config.getSectorCompaniesUrl + "/" + id);
};
export const getCompaniesByKey = (key) => {
  return axios.get(config.getCompaniesByKeyUrl + "/" + key);
};
export const getBusinessInformationList = () => {
  return axios.get(config.getBusinessInformationListUrl);
};
export const getPastBusinessInformationList = () => {
  return axios.get(config.getPastBusinessInformationListUrl);
};
export const getBusinessInformation = (id) => {
  return axios.get(config.getBusinessInformationUrl + "/" + id);
};
