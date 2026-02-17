import axios from "axios";
const apiController = "/api/ApiBusinessInterest";
const config = {
  clientId: "react.client",
  getSourcesListUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetSources`,
  searchSourcesListUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SearchSources`,
  getSourceDetailUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetSourceDetail`,
  getSectorsUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/Sectors`,
  saveInterestUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveInterest`,
  getUserMatchesUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/UserMatches`,
  removeInterestUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/RemoveInterest`,
  setInterestRemarkUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SetInterestRemark`,
  getUserMatchSourcesUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetUserMatchSources`,
  scopes: [],
};

export const deleteUserInterest = (id) => {
  return axios.post(config.removeInterestUri + "/" + id);
};

export const setInterestRemark = (id, remark) => {
  return axios.post(config.setInterestRemarkUri + "/" + id + "/" + remark);
};

export const saveInterest = (id) => {
  return axios.post(config.saveInterestUrl + "/" + id);
};

export const getUserMatches = () => {
  return axios.get(config.getUserMatchesUrl);
};

export const getUserMatchSources = () => {
  return axios.get(config.getUserMatchSourcesUri);
};

export const getSourcesList = (sectorId) => {
  return axios.get(config.getSourcesListUrl + "/" + sectorId);
};

export const getSourceDetail = (id) => {
  return axios.get(config.getSourceDetailUrl + "/" + id);
};

export const getSectorsList = (parentsOnly = false) => {
  return axios.get(config.getSectorsUrl + "/" + parentsOnly);
};

export const searchSourcesList = (businessType, key) => {
  return axios.get(
    config.searchSourcesListUrl + "/" + businessType + "/" + key
  );
};
