import axios from "axios";

const config = {
  clientId: "react.client",
  getSubscriptionsUrl: `${process.env.REACT_APP_API_URL}/api/Home/GetSubscriptions`,
  saveSubscriptionUrl: `${process.env.REACT_APP_API_URL}/api/Home/SaveSubscription`,
  scopes: [],
};
export const getSubscriptions = (keyword) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getSubscriptionsUrl);
  /* } else {
        return null;
    } */
};

export const  saveSubscription = (data) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
        axios.defaults.headers.common["Authorization"] = token
            ? "Bearer " + token
            : "";
        return axios.post(config.saveSubscriptionUrl, data);
    /* } else {
        return null;
    } */
};