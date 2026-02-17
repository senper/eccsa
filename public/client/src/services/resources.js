import axios from "axios";

const config = {
  clientId: "react.client",
  resourcesUri: `${process.env.REACT_APP_API_URL}/api/ApiResources/Resources`,
  resourceUri: `${process.env.REACT_APP_API_URL}/api/ApiResources/GetResource`,
  dialogsListUri: `${process.env.REACT_APP_API_URL}/api/ApiResources/PPDialogCategories`,
  dialogUri: `${process.env.REACT_APP_API_URL}/api/ApiResources/Dialog`,
  downloadPPDialogAttUri: `${process.env.REACT_APP_API_URL}/api/ApiResources/DownloadPPDialogAttachment`,
  scopes: [],
};

export const getResources = (type) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.resourcesUri + "?type=" + type);
  /* } else {
        return null;
    } */
};

export const getResource = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.resourceUri + "/" + id);
  /* } else {
        return null;
    } */
};

export const resourceTypes = [
  { value: 1, text: "Books" },
  { value: 2, text: "Publications" },
  { value: 3, text: "Newspapers" },
  { value: 4, text: "External Links" },
  { value: 5, text: "Downloads" },
  { value: 6, text: "Laws and Directives" },
  { value: 99, text: "Other Materials" },
];


export const getDialogList = () => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.dialogsListUri);
  /* } else {
        return null;
    } */
};

export const getDialog = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.dialogUri + "/" + id);
  /* } else {
        return null;
    } */
};


export const downloadPPDialogAttachment = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.downloadPPDialogAttUri + "/" + id);
  /* } else {
        return null;
    } */
};