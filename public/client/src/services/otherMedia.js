import axios from "axios";
import { setAuthHeader } from "../utils/axiosHeaders";

const config = {
  clientId: "react.client",
  getTextMediaListUri: `${process.env.REACT_APP_API_URL}/api/ApiPress/List`,
  getTextMediaUri: `${process.env.REACT_APP_API_URL}/api/ApiPress/ById`,
  getTextMediaUri: `${process.env.REACT_APP_API_URL}/api/ApiPress/ById`,
  addCommentUri: `${process.env.REACT_APP_API_URL}/api/ApiPress/AddComment`,
  scopes: [],
};

export const getTextMediaList = (type) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getTextMediaListUri + "?mediaType=" + type);
  /* } else {
        return null;
    } */
};

export const getTextMedia = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.getTextMediaUri + "/" + id);
  /* } else {
        return null;
    } */
};

export const addComment = (id, text) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.addCommentUri + "/" + id + "/" + text);
  /* } else {
        return null;
    } */
};

export const textMediaTypes = [
  { value: 1, text: "Announcement" },
  { value: 2, text: "Press Release" },
 /* { value: 3, text: "Speech" }, */
  { value: 4, text: "Blog" },
];
