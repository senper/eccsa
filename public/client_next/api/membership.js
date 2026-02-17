import axios from "axios";
const apiController = "/api/ApiMembership";
const config = {
  clientId: "react.client",
  getMembershipInfoUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetMembershipInfo`,
  getECCSAAssociationsUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetECCSAAssociations`,
  getAssociationUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetAssociation`,
  getAssociationsUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetAssociationsForApply`,
  getApplicationsUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetApplications`,
  saveMembershipApplicationUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/Save`,
  getAssociationForApplyUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetAssociationForApply`,
  getMembershipRequestUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetApplication`,
  getUserApplicationUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetApplication`,
  submitApplicationUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SubmitApplication`,
  applicationDocSubmitUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SubmitApplicationDocument`,
  applicationSavePaymentUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveApplicationPayment`,
  printInvoiceUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/PrintApplicationInvoice`,
  printReceiptUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/PrintApplicationReceipt`,

  scopes: [],
};

export const getMembershipInfo = () => {
  return axios.get(config.getMembershipInfoUrl);
};

export const getECCSAAssociations = () => {
  return axios.get(config.getECCSAAssociationsUrl);
};

export const getAssociations = (type) => {
  return axios.get(config.getAssociationsUrl + "/" + type);
};

export const getApplications = () => {
  return axios.get(config.getApplicationsUrl);
};

export const getAssociation = (id) => {
  return axios.get(config.getAssociationUrl + "/" + id);
};

export const getAssociationForApply = (id) => {
  return axios.get(config.getAssociationForApplyUrl + "/" + id);
};

export const getMembershipRequest = (id) => {
  return axios.get(config.getMembershipRequestUrl + "/" + id);
};

export const saveMembershipApplication = (data) => {
  return axios.post(config.saveMembershipApplicationUri, data);
};

export const getUserApplication = (id) => {
  return axios.get(config.getUserApplicationUrl + "/" + id);
};

export const submitApplication = (id) => {
  return axios.post(config.submitApplicationUrl + "/" + id);
};

export const submitApplicationDocument = (id) => {
  return axios.post(config.applicationDocSubmitUri + "/" + id);
};

export const saveApplicationPayment = (id, data) => {
  return axios.post(config.applicationSavePaymentUri, data);
};
export const printApplicationInvoice = (id, invoiceNo) => {
  return axios.get(
    config.printInvoiceUrl + `?referenceId=${id}&invoiceNo=${invoiceNo}`,
    { responseType: "blob" }
  );
};
export const printApplicationReceipt = (id, invoiceNo) => {
  return axios.get(
    config.printReceiptUrl + `?referenceId=${id}&invoiceNo=${invoiceNo}`,
    { responseType: "blob" }
  );
};
