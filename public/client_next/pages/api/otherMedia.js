import axios from "axios";

const config = {
  clientId: "react.client",
  getTextMediaListUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiPress/List`,
  getTextMediaUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiPress/ById`,
  getTextMediaUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiPress/ById`,
  addCommentUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiPress/AddComment`,
  scopes: [],
};


export const getTextMediaList = (type) => {
  return axios.get(config.getTextMediaListUri + "?mediaType=" + type);
};

export const getTextMedia = (id) => {
  return axios.get(config.getTextMediaUri + "/" + id);
};

export const addComment = (id, text) => {
  return axios.post(config.addCommentUri + "/" + id + "/" + text);
};

export const textMediaTypes = [
  { value: 1, text: "Announcement" },
  { value: 2, text: "Press Release" },
 /* { value: 3, text: "Speech" }, */
  { value: 4, text: "Blog" },
];
