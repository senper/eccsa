import axios from "axios";
const apiController = "/api/ApiUserBusinessInterest";
const config = {
  clientId: "react.client",
  getRootSectorsUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetRootSectors`,
  getChildSectorsUrl: `${process.env.REACT_APP_API_URL}${apiController}/ChildSectors`,
  getBusinessInterestProductsUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetBusinessInterestProducts`,
  saveBusinessInterestUrl: `${process.env.REACT_APP_API_URL}${apiController}/SaveBusinessInterest`,
  getUserBusinessInterestUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetUserBusinessInterest`,
  removeBusinessInterestUrl: `${process.env.REACT_APP_API_URL}${apiController}/RemoveBusinessInterest`,
  scopes: [],
};

export const getRootSectors = () => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getRootSectorsUrl);
  /* } else {
        return null;
    } */
};

export const getChildSectors = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getChildSectorsUrl + "/" + id);
  /* } else {
        return null;
    } */
};

export const getBusinessInterestProducts = () => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getBusinessInterestProductsUrl);
  /* } else {
        return null;
    } */
};

export const getUserBusinessInterest = () => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getUserBusinessInterestUrl);
  /* } else {
        return null;
    } */
};

export const saveBusinessInterest = (data) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.saveBusinessInterestUrl, data);
  /* } else {
        return null;
    } */
};

export const removeBusinessInterest = () => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.removeBusinessInterestUrl);
  /* } else {
        return null;
    } */
};

