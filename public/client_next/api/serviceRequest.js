import axios from "axios";
const apiController = "/api/ApiServiceRequest";
const config = {
  clientId: "react.client",
  serviceRequestSaveUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/Save`,
  serviceRequestsUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetRequests`,
  serviceRequestUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetRequest`,
  serviceRequestSubmitUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SubmitRequest`,
  serviceRequestDocSubmitUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SubmitRequestDoc`,
  serviceRequestCloseUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/CloseRequest`,
  serviceRequestSavePaymentUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/MakePayment`,
  serviceRequestDocUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/DownLoadFile`,
  printInvoice: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/PrintInvoice`,
  printReceipt: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/PrintReceipt`,
  printCertificate: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/PrintCertificate`,
  verifyCertificateUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/VerifyCertificate`,
  makeRequirementPaymentUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/MakeRequirementPayment`,
  saveRequestAttachmentUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveAttachment`,
  deleteRequestAttachmentUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/DeleteAttachment`,
  saveOptionalEntryUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveOptionalEntry`,
  deleteOptionalEntryUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/DeleteOptionalEntry`,
  saveTeleOutTradeNoUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveTeleOutTradeNo`,
  getUserCertificateOfOriginUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetUserCertificateOfOrigin`,
  closeCertOfOriginDocumentUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/CloseUserCertificateOfOrigin`,
  getResourceRequestsUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetResourceRequests`,
  downloadResourceUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/DownloadResource`,
  getResourceUrl_Url: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetResourceUrl`,
  scopes: [],
};

export const verifyCertificate = (code) => {
  return axios.get(config.verifyCertificateUrl + `?code=${code}`);
};

export const printCertificate = (id, invoiceNo) => {
  return axios.get(config.printCertificate + `?serviceId=${id}`, {
    responseType: "blob",
  });
};
export const printInvoice = (id, invoiceNo) => {
  return axios.get(
    config.printInvoice + `?referenceId=${id}&invoiceNo=${invoiceNo}`,
    { responseType: "blob" }
  );
};
export const printReceipt = (id, invoiceNo) => {
  return axios.get(
    config.printReceipt + `?referenceId=${id}&invoiceNo=${invoiceNo}`,
    { responseType: "blob" }
  );
};
export const saveRequest = (data) => {
  return axios.post(config.serviceRequestSaveUri, data);
};

export const getServiceRequests = (status) => {
  return axios.get(config.serviceRequestsUri + "?status=" + status);
};

export const getServiceRequest = (id) => {
  return axios.get(config.serviceRequestUri + "/" + id);
};
export const getDocument = (path, title) => {
  return axios.get(
    config.serviceRequestDocUri + "?filePath=" + path + "&title=" + title
  );
};

export const submitRequest = (id) => {
  return axios.post(config.serviceRequestSubmitUri + "/" + id);
};

export const submitDocument = (id) => {
  return axios.post(config.serviceRequestDocSubmitUri + "/" + id);
};

export const closeDocument = (id) => {
  return axios.post(config.serviceRequestCloseUri + "/" + id);
};

export const closeCertOfOriginDocument = (id) => {
  return axios.post(config.closeCertOfOriginDocumentUrl + "/" + id);
};
export const savePayment = (id, data) => {
  return axios.post(config.serviceRequestSavePaymentUri, data);
};

export const saveRequirementPayment = (id, data) => {
  return axios.post(config.makeRequirementPaymentUrl, data);
};

export const saveTeleOutTradeNo = (id, data) => {
  return axios.post(config.saveTeleOutTradeNoUrl, data);
};

export const saveRequestAttachment = (data) => {
  return axios.post(config.saveRequestAttachmentUri, data);
};

export const deleteRequestAttachment = (id) => {
  return axios.post(config.deleteRequestAttachmentUrl + "/" + id);
};

export const saveOptionalEntry = (data) => {
  return axios.post(config.saveOptionalEntryUrl, data);
};

export const deleteOptionalEntry = (label, requestId) => {
  return axios.post(config.deleteOptionalEntryUrl + "/" + label + "/" + requestId);
};
export const getUserCertificateOfOrigin = (id) => {
  return axios.get(config.getUserCertificateOfOriginUrl + "/" + id);
};

export const getResourceRequests = (status) => {
  return axios.get(config.getResourceRequestsUrl + "?status=" + status);
};

export const downloadResource = (id) => {
  return axios.get(config.downloadResourceUrl + `?requestId=${id}`, {
    responseType: "blob",
  });
};

export const getResourceUrl = (id) => {
  return axios.get(config.getResourceUrl_Url + `?requestId=${id}`);
};

export function excapeCharsReplacer(value) {
  return value
    .replaceAll("\t", "")
    .replaceAll("\b", "")
    .replaceAll("\v", "")
    .replaceAll("\0", "")
    .replaceAll("\xFF", "")
    .replaceAll("'", "'")
    .replaceAll('"', '"')
    .replaceAll("\f", "")
    .replaceAll("\n", "")
    .replaceAll("\r", "")
    ;
}
