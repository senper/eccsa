import axios from "axios";
import { setAuthHeader } from "../utils/axiosHeaders";

const config = {
    clientId: "react.client",
    newsUri: `${process.env.REACT_APP_API_URL}/api/ApiNews/LoadNews`,
    newsHighlightUri:`${process.env.REACT_APP_API_URL}/api/ApiNews/NewsHighlight`,
    newsForSliderUrl:`${process.env.REACT_APP_API_URL}/api/ApiNews/NewsForSlider`,
    scopes: [],
};

export const  getNews = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.newsUri);
    /* } else {
        return null;
    } */
};

export const  getNewsHighlight = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.newsHighlightUri);
    /* } else {
        return null;
    } */
};

export const  getNewsForSlider = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.newsForSliderUrl);
    /* } else {
        return null;
    } */
};




