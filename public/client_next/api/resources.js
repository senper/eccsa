import axios from "axios";

const config = {
  clientId: "react.client",
  resourcesUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiResources/Resources`,
  resourcesByTypeIdUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiResources/ResourcesByTypeId`,
  resourceUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiResources/GetResource`,
  dialogsListUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiResources/PPDialogCategories`,
  dialogUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiResources/Dialog`,
  downloadPPDialogAttUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiResources/DownloadPPDialogAttachment`,
  scopes: [],
};

export const getResources = (type) => {
  return axios.get(config.resourcesUri + "?type=" + type);
};
export const getResourcesByTypeId = (typeId) => {
  return axios.get(config.resourcesByTypeIdUrl + "?typeId=" + typeId);
};

export const getResource = (id) => {
  return axios.get(config.resourceUri + "/" + id);
};
/* 
export const resourceTypes = [
  { value: 1, text: "Books" },
  { value: 2, text: "Publications" },
  { value: 3, text: "Newspapers" },
  { value: 4, text: "External Links" },
  { value: 5, text: "Downloads" },
  { value: 6, text: "Laws and Directives" },
  { value: 99, text: "Other Materials" },
]; */

export const getDialogList = () => {
  return axios.get(config.dialogsListUri);
};

export const getDialog = (id) => {
  return axios.get(config.dialogUri + "/" + id);
};

export const downloadPPDialogAttachment = (id) => {
  return axios.get(config.downloadPPDialogAttUri + "/" + id);
};
