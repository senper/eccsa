import axios from "axios";
const apiController = "/api/ApiMembership";
const config = {
    clientId: "react.client",
    getMembershipInfoUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetMembershipInfo`,
    getECCSAAssociationsUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetECCSAAssociations`,
    getAssociationUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetAssociation`,
    getAssociationsUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetAssociationsForApply`,
    getApplicationsUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetApplications`,
    saveMembershipApplicationUri: `${process.env.REACT_APP_API_URL}${apiController}/Save`,
    getAssociationForApplyUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetAssociationForApply`,
    getMembershipRequestUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetApplication`,
    getUserApplicationUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetApplication`,
    submitApplicationUrl: `${process.env.REACT_APP_API_URL}${apiController}/SubmitApplication`,
    applicationDocSubmitUri: `${process.env.REACT_APP_API_URL}${apiController}/SubmitApplicationDocument`,
    applicationSavePaymentUri: `${process.env.REACT_APP_API_URL}${apiController}/SaveApplicationPayment`,
    printInvoiceUrl: `${process.env.REACT_APP_API_URL}${apiController}/PrintApplicationInvoice`,
    printReceiptUrl: `${process.env.REACT_APP_API_URL}${apiController}/PrintApplicationReceipt`,

    scopes: [],
};

export const  getMembershipInfo = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getMembershipInfoUrl);
    /* } else {
        return null;
    } */
};

export const  getECCSAAssociations = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getECCSAAssociationsUrl);
    /* } else {
        return null;
    } */
};

export const  getAssociations = (type) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getAssociationsUrl + "/" + type);
    /* } else {
        return null;
    } */
};

export const  getApplications = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getApplicationsUrl);
    /* } else {
        return null;
    } */
};

export const getAssociation = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
    axios.defaults.headers.common["Authorization"] = token
      ? "Bearer " + token
      : "";
    return axios.get(config.getAssociationUrl + "/" + id);
    /* } else {
          return null;
      } */
  };
  
export const getAssociationForApply = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
    axios.defaults.headers.common["Authorization"] = token
      ? "Bearer " + token
      : "";
    return axios.get(config.getAssociationForApplyUrl + "/" + id);
    /* } else {
          return null;
      } */
  };

export const getMembershipRequest = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
    axios.defaults.headers.common["Authorization"] = token
      ? "Bearer " + token
      : "";
    return axios.get(config.getMembershipRequestUrl + "/" + id);
    /* } else {
          return null;
      } */
  };
  
export const  saveMembershipApplication = (data) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.saveMembershipApplicationUri, data);
    /* } else {
        return null;
    } */
};


export const getUserApplication = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
    axios.defaults.headers.common["Authorization"] = token
      ? "Bearer " + token
      : "";
    return axios.get(config.getUserApplicationUrl + "/" + id);
    /* } else {
          return null;
      } */
  };

  
export const  submitApplication = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.submitApplicationUrl + "/" + id);
    /* } else {
        return null;
    } */
};

export const  submitApplicationDocument = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.applicationDocSubmitUri + "/" + id);
    /* } else {
        return null;
    } */
};

export const  saveApplicationPayment = (id, data) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.applicationSavePaymentUri, data);
    /* } else {
        return null;
    } */
};
export const  printApplicationInvoice = (id, invoiceNo) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.printInvoiceUrl + `?referenceId=${id}&invoiceNo=${invoiceNo}`, {responseType: 'blob'});
    /* } else {
        return null;
    } */
};
export const  printApplicationReceipt = (id, invoiceNo) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.printReceiptUrl + `?referenceId=${id}&invoiceNo=${invoiceNo}`, {responseType: 'blob'});
    /* } else {
        return null;
    } */
};