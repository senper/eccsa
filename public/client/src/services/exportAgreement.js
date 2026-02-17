import axios from "axios";
const apiController = "/api/Home";
const config = {
    clientId: "react.client",
    saveExportAgreementUri: `${process.env.REACT_APP_API_URL}${apiController}/SaveExportAgreement`,
    saveCommodityListUri: `${process.env.REACT_APP_API_URL}${apiController}/SaveCommodityList`,
    getExportAgreementsUri: `${process.env.REACT_APP_API_URL}${apiController}/GetExportAgreements`,
    getExportAgreementLookupUri: `${process.env.REACT_APP_API_URL}${apiController}/GetExportAgreementLookup`,
    getExportAgreementUri: `${process.env.REACT_APP_API_URL}${apiController}/GetExportAgreement`,
    removeExportAgreementUri: `${process.env.REACT_APP_API_URL}${apiController}/DeleteExportAgreement`,
    scopes: [],
};
export const  saveExportAgreement = (data) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.saveExportAgreementUri, data);
    /* } else {
        return null;
    } */
};


export const  saveCommodityList = (id, data) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.saveCommodityListUri + "/" + id, data);
    /* } else {
        return null;
    } */
};

export const  getExportAgreements = (status) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getExportAgreementsUri + "?status=" + status);
    /* } else {
        return null;
    } */
};

export const  getExportAgreementLookup = (status) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getExportAgreementLookupUri + "?status=" + status);
    /* } else {
        return null;
    } */
};
export const  getExportAgreement = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getExportAgreementUri + "/" + id);
    /* } else {
        return null;
    } */
};
export const  deleteExportAgreement = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.removeExportAgreementUri + "/" + id);
    /* } else {
        return null;
    } */
};