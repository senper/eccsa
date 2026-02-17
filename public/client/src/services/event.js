import axios from "axios";
import { setAuthHeader } from "../utils/axiosHeaders";

const config = {
    clientId: "react.client",
    eventsUri: `${process.env.REACT_APP_API_URL}/api/ApiEvent/Events`,
    pastEventsUri: `${process.env.REACT_APP_API_URL}/api/ApiEvent/PastEvents`,
    eventUri: `${process.env.REACT_APP_API_URL}/api/ApiEvent/GetEvent`,
    scopes: [],
};

export const  getEvents = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.eventsUri);
    /* } else {
        return null;
    } */
};
export const  getPastEvents = () => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.pastEventsUri);
    /* } else {
        return null;
    } */
};
export const  getEvent = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.get(config.eventUri + "/" + id);
    /* } else {
        return null;
    } */
};