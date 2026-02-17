import axios from "axios";
const apiController = "/api/ApiBusinessInformation";
const config = {
    clientId: "react.client",
    getBusinessInformationListUrl: `${process.env.REACT_APP_API_URL}${apiController}/BusinessInformationList`,
    getPastBusinessInformationListUrl: `${process.env.REACT_APP_API_URL}${apiController}/PastBusinessInformationList`,
    getBusinessInformationUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetBusinessInformation`,
    getSectorsUrl: `${process.env.REACT_APP_API_URL}${apiController}/Sectors`,
    getSectorCompaniesUrl: `${process.env.REACT_APP_API_URL}${apiController}/SectorCompanies`,
    getCompaniesByKeyUrl: `${process.env.REACT_APP_API_URL}${apiController}/CompaniesByKey`,
    getInvestmentProfilesListUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetInvestmentProfilesList`,
    getInvestmentProfileUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetInvestmentProfile`,
    scopes: [],
};

export const  getInvestmentProfilesList = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getInvestmentProfilesListUrl);
    /* } else {
        return null;
    } */
};
export const  getInvestmentProfile = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getInvestmentProfileUrl + "/" + id);
    /* } else {
        return null;
    } */
};
export const  getSectorsList = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getSectorsUrl);
    /* } else {
        return null;
    } */
};
export const  getSectorCompanies = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getSectorCompaniesUrl + "/" + id);
    /* } else {
        return null;
    } */
};
export const  getCompaniesByKey = (key) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getCompaniesByKeyUrl + "/" + key);
    /* } else {
        return null;
    } */
};
export const  getBusinessInformationList = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getBusinessInformationListUrl);
    /* } else {
        return null;
    } */
};
export const  getPastBusinessInformationList = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getPastBusinessInformationListUrl);
    /* } else {
        return null;
    } */
};
export const  getBusinessInformation = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getBusinessInformationUrl + "/" + id);
    /* } else {
        return null;
    } */
};
