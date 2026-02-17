import axios from "axios";
const apiController = "/api/Home";
const config = {
    clientId: "react.client",
    saveUserContactUri: `${process.env.REACT_APP_API_URL}${apiController}/SaveContact`,
    getUserContactsUri: `${process.env.REACT_APP_API_URL}${apiController}/GetContacts`,
    getUserContactUri: `${process.env.REACT_APP_API_URL}${apiController}/GetContact`,
    removeUserContactUri: `${process.env.REACT_APP_API_URL}${apiController}/DeleteContact`,
    scopes: [],
};
export const  saveUserContact = (data) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.saveUserContactUri, data);
    /* } else {
        return null;
    } */
};

export const  getUserContacts = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getUserContactsUri);
    /* } else {
        return null;
    } */
};

export const  deleteUserContact = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.removeUserContactUri + "/" + id);
    /* } else {
        return null;
    } */
};

export const  getUserContact = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getUserContactUri + "/" + id);
    /* } else {
        return null;
    } */
};
export const ContactTypes =
    [
        { name: "Mobile", value: 1},
        { name: "Email", value: 2},
        { name: "Website", value: 3},
        { name: "Landline", value: 4},
        { name: "Fax", value: 5},
        { name: "Facebook", value: 6},
        { name: "Twitter", value: 7},
        { name: "Physical", value: 9},
        { name: "Other", value: 0},
    ];