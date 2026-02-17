import axios from "axios";
import { setAuthHeader } from "../utils/axiosHeaders";

const config = {
    clientId: "react.client",
    getUserSurveyUri: `${process.env.REACT_APP_API_URL}/api/ApiSurvey/GetUserSurvey`,
    getSurveyListUri: `${process.env.REACT_APP_API_URL}/api/ApiSurvey/GetSurveyList`,
    saveResponseUri: `${process.env.REACT_APP_API_URL}/api/ApiSurvey/SaveResponse`,
    scopes: [],
};

export const  getUserSurvey = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getUserSurveyUri+ "/" + id);
    /* } else {
        return null;
    } */
};


export const  getSurveyList = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.getSurveyListUri);
    /* } else {
        return null;
    } */
};

export const  saveResponse = (id, data) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.saveResponseUri + "/" + id, data);
    /* } else {
        return null;
    } */
};