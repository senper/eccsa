import axios from "axios";
import { setAuthHeader } from "../utils/axiosHeaders";

const config = {
    clientId: "react.client",
    serviceTypesUri: `${process.env.REACT_APP_API_URL}/api/ApiServiceType`,
    serviceOwnersUri: `${process.env.REACT_APP_API_URL}/api/ApiServiceType/owners`,
    serviceOwnerUri: `${process.env.REACT_APP_API_URL}/api/ApiServiceType/GetOwner`,
    scopes: [],
};

export const  getServiceTypes = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.serviceTypesUri);
    /* } else {
        return null;
    } */
};

export const  getServiceType = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.serviceTypesUri + "/" + id);
    /* } else {
        return null;
    } */
};

export const  getServiceOwners = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.serviceOwnersUri);
    /* } else {
        return null;
    } */
};

export const  getServiceOwner = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.serviceOwnerUri + "/" + id);
    /* } else {
        return null;
    } */
};
export const CertificateOfOriginId = "d7571107-ab29-43b3-b95c-c0ae12505a61";