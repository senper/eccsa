import axios from "axios";
const apiController = "/api/ApiServiceRequest";
const config = {
  clientId: "react.client",
  serviceRequestSaveUri: `${process.env.REACT_APP_API_URL}${apiController}/Save`,
  serviceRequestsUri: `${process.env.REACT_APP_API_URL}${apiController}/GetRequests`,
  serviceRequestUri: `${process.env.REACT_APP_API_URL}${apiController}/GetRequest`,
  serviceRequestSubmitUri: `${process.env.REACT_APP_API_URL}${apiController}/SubmitRequest`,
  serviceRequestDocSubmitUri: `${process.env.REACT_APP_API_URL}${apiController}/SubmitRequestDoc`,
  serviceRequestCloseUri: `${process.env.REACT_APP_API_URL}${apiController}/CloseRequest`,
  serviceRequestSavePaymentUri: `${process.env.REACT_APP_API_URL}${apiController}/MakePayment`,
  serviceRequestDocUri: `${process.env.REACT_APP_API_URL}${apiController}/DownLoadFile`,
  printInvoice: `${process.env.REACT_APP_API_URL}${apiController}/PrintInvoice`,
  printReceipt: `${process.env.REACT_APP_API_URL}${apiController}/PrintReceipt`,
  printCertificate: `${process.env.REACT_APP_API_URL}${apiController}/PrintCertificate`,
  verifyCertificateUrl: `${process.env.REACT_APP_API_URL}${apiController}/VerifyCertificate`,
  makeRequirementPaymentUrl: `${process.env.REACT_APP_API_URL}${apiController}/MakeRequirementPayment`,
  saveRequestAttachmentUri: `${process.env.REACT_APP_API_URL}${apiController}/SaveAttachment`,
  deleteRequestAttachmentUrl: `${process.env.REACT_APP_API_URL}${apiController}/DeleteAttachment`,
  saveTeleOutTradeNoUrl: `${process.env.REACT_APP_API_URL}${apiController}/SaveTeleOutTradeNo`,
  getUserCertificateOfOriginUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetUserCertificateOfOrigin`,
  closeCertOfOriginDocumentUrl: `${process.env.REACT_APP_API_URL}${apiController}/CloseUserCertificateOfOrigin`,
  getResourceRequestsUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetResourceRequests`,
  downloadResourceUrl: `${process.env.REACT_APP_API_URL}${apiController}/DownloadResource`,
  getResourceUrl_Url: `${process.env.REACT_APP_API_URL}${apiController}/GetResourceUrl`,
  scopes: [],
};

export const verifyCertificate = (code) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.verifyCertificateUrl + `?code=${code}`);
  /* } else {
        return null;
    } */
};

export const printCertificate = (id, invoiceNo) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.printCertificate + `?serviceId=${id}`, {
    responseType: "blob",
  });
  /* } else {
        return null;
    } */
};
export const printInvoice = (id, invoiceNo) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(
    config.printInvoice + `?referenceId=${id}&invoiceNo=${invoiceNo}`,
    { responseType: "blob" }
  );
  /* } else {
        return null;
    } */
};
export const printReceipt = (id, invoiceNo) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(
    config.printReceipt + `?referenceId=${id}&invoiceNo=${invoiceNo}`,
    { responseType: "blob" }
  );
  /* } else {
        return null;
    } */
};
export const saveRequest = (data) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.serviceRequestSaveUri, data);
  /* } else {
        return null;
    } */
};

export const getServiceRequests = (status) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.serviceRequestsUri + "?status=" + status);
  /* } else {
        return null;
    } */
};

export const getServiceRequest = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.serviceRequestUri + "/" + id);
  /* } else {
        return null;
    } */
};
export const getDocument = (path, title) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(
    config.serviceRequestDocUri + "?filePath=" + path + "&title=" + title
  );
  /* } else {
        return null;
    } */
};

export const submitRequest = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.serviceRequestSubmitUri + "/" + id);
  /* } else {
        return null;
    } */
};

export const submitDocument = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.serviceRequestDocSubmitUri + "/" + id);
  /* } else {
        return null;
    } */
};

export const closeDocument = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.serviceRequestCloseUri + "/" + id);
  /* } else {
        return null;
    } */
};

export const closeCertOfOriginDocument = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.closeCertOfOriginDocumentUrl + "/" + id);
  /* } else {
        return null;
    } */
};
export const savePayment = (id, data) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.serviceRequestSavePaymentUri, data);
  /* } else {
        return null;
    } */
};

export const saveRequirementPayment = (id, data) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.makeRequirementPaymentUrl, data);
  /* } else {
        return null;
    } */
};

export const saveTeleOutTradeNo = (id, data) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.saveTeleOutTradeNoUrl, data);
  /* } else {
        return null;
    } */
};

export const saveRequestAttachment = (data) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.saveRequestAttachmentUri, data);
  /* } else {
          return null;
      } */
};

export const deleteRequestAttachment = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.deleteRequestAttachmentUrl + "/" + id);
  /* } else {
          return null;
      } */
};

export const getUserCertificateOfOrigin = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getUserCertificateOfOriginUrl + "/" + id);
  /* } else {
        return null;
    } */
};

export const getResourceRequests = (status) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getResourceRequestsUrl + "?status=" + status);
  /* } else {
        return null;
    } */
};

export const downloadResource = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.downloadResourceUrl + `?requestId=${id}`, {
    responseType: "blob",
  });
  /* } else {
        return null;
    } */
};

export const getResourceUrl = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getResourceUrl_Url + `?requestId=${id}`);
  /* } else {
        return null;
    } */
};

export function excapeCharsReplacer(value) {
  return value
    .replace("\t", "")
    .replace("\b", "")
    .replace("\v", "")
    .replace("\0", "")
    .replace("\xFF", "")
    .replace("'", "'")
    .replace('"', '"')
    .replace("\f", "")
    .replace("\n", "")
    .replace("\r", "");
}
