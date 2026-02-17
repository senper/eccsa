import axios from "axios";
const apiController = "/api/ApiBusinessInterest";
const config = {
  clientId: "react.client",
  getSourcesListUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetSources`,
  searchSourcesListUrl: `${process.env.REACT_APP_API_URL}${apiController}/SearchSources`,
  getSourceDetailUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetSourceDetail`,
  getSectorsUrl: `${process.env.REACT_APP_API_URL}${apiController}/Sectors`,
  saveInterestUrl: `${process.env.REACT_APP_API_URL}${apiController}/SaveInterest`,
  getUserMatchesUrl: `${process.env.REACT_APP_API_URL}${apiController}/UserMatches`,
  removeInterestUri: `${process.env.REACT_APP_API_URL}${apiController}/RemoveInterest`,
  setInterestRemarkUri: `${process.env.REACT_APP_API_URL}${apiController}/SetInterestRemark`,
  getUserMatchSourcesUri: `${process.env.REACT_APP_API_URL}${apiController}/GetUserMatchSources`,
  scopes: [],
};

export const deleteUserInterest = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.removeInterestUri + "/" + id);
  /* } else {
        return null;
    } */
};

export const setInterestRemark = (id, remark) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.setInterestRemarkUri + "/" + id + "/" + remark);
  /* } else {
        return null;
    } */
};

export const saveInterest = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.saveInterestUrl + "/" + id);
  /* } else {
        return null;
    } */
};
export const getUserMatches = () => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getUserMatchesUrl);
  /* } else {
        return null;
    } */
};

export const getUserMatchSources = () => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getUserMatchSourcesUri);
  /* } else {
        return null;
    } */
};
export const getSourcesList = (sectorId) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getSourcesListUrl + "/" + sectorId);
  /* } else {
        return null;
    } */
};
export const getSourceDetail = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getSourceDetailUrl + "/" + id);
  /* } else {
        return null;
    } */
};
export const getSectorsList = (parentsOnly = false) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getSectorsUrl + "/" + parentsOnly);
  /* } else {
        return null;
    } */
};
export const searchSourcesList = (businessType, key) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(
    config.searchSourcesListUrl + "/" + businessType + "/" + key
  );
  /* } else {
        return null;
    } */
};
