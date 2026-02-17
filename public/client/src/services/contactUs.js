import axios from "axios";
const apiController = "/api/ApiContactUs";
const config = {
    clientId: "react.client",
    getIssueTypesUrl: `${process.env.REACT_APP_API_URL}${apiController}/IssueTypes`,
    saveContactUsUrl: `${process.env.REACT_APP_API_URL}${apiController}/SaveContactUs`,
    saveIssueUrl: `${process.env.REACT_APP_API_URL}${apiController}/SaveIssue`,
    getCountriesUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetCountries`,
    getLookupValuesUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetLookupValues`,
    getIssueLocationsUrl: `${process.env.REACT_APP_API_URL}${apiController}/GetIssueLocations`,
    scopes: [],
};

export const  getIssueLocations = (country) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getIssueLocationsUrl + "/" + country);
    /* } else {
        return null;
    } */
};

export const  saveContactUs = (data) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.saveContactUsUrl, data);
    /* } else {
        return null;
    } */
};

export const  saveIssue = (data) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.saveIssueUrl, data);
    /* } else {
        return null;
    } */
};

export const  getIssueTypes = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getIssueTypesUrl);
    /* } else {
        return null;
    } */
};
export const  getCountries = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getCountriesUrl);
    /* } else {
        return null;
    } */
};

export const  getLookupValues = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getLookupValuesUrl + "/" + id);
    /* } else {
        return null;
    } */
};