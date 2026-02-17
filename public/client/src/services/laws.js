import axios from "axios";

const config = {
  clientId: "react.client",
  listUri: `${process.env.REACT_APP_API_URL}/api/ApiLaws/List`,
  downloadUri: `${process.env.REACT_APP_API_URL}/api/ApiLaws/Download`,
  scopes: [],
};

export const lawDownloadUrl = config.downloadUri;

export const getLawsList = (category, type) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.listUri + "?category=" + category + "&lawType=" + type);
  /* } else {
        return null;
    } */
};

export const downloadLaw = (id) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.downloadUri + "/" + id);
  /* } else {
        return null;
    } */
};

export const lawTypes = [
    { value: 1, text: "Proclamation" },
    { value: 2, text: "Regulation" },
    { value: 3, text: "Directive" },
  ];

  export const lawCategories = [
      { value: 1, text: "Trade" },
      { value: 2, text: "Investment" },
      { value: 3, text: "Customs" },
      { value: 4, text: "Domestic" },
      { value: 5, text: "Land" },
      { value: 99, text: "Other" },
    ];
