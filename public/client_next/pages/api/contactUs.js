import axios from "axios";
const apiController = "/api/ApiContactUs";
const config = {
    clientId: "react.client",
    getIssueTypesUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/IssueTypes`,
    saveContactUsUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveContactUs`,
    saveIssueUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveIssue`,
    getCountriesUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetCountries`,
    getLookupValuesUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetLookupValues`,
    getIssueLocationsUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetIssueLocations`,
    getIssuesSummaryUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetIssuesSummary`,
    scopes: [],
};

export const  getIssueLocations = (country) => {
        return axios.get(config.getIssueLocationsUrl + "/" + country);
};

export const  saveContactUs = (data) => {
        return axios.post(config.saveContactUsUrl, data);
};

export const  saveIssue = (data) => {
        return axios.post(config.saveIssueUrl, data);
};

export const  getIssueTypes = () => {
        return axios.get(config.getIssueTypesUrl);
};
export const  getCountries = () => {
        return axios.get(config.getCountriesUrl);
};
export const  getIssuesSummary = () => {
        return axios.get(config.getIssuesSummaryUrl);
};

export const  getLookupValues = (id) => {
        return axios.get(config.getLookupValuesUrl + "/" + id);
};