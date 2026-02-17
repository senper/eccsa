import axios from "axios";

const config = {
  clientId: "react.client",
  adImagesUri: `${process.env.REACT_APP_API_URL}/api/ApiAdvert/GetAdImages`,
  scopes: [],
};

export const getAdImages = (type) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.adImagesUri + "?section=" + type);
  /* } else {
        return null;
    } */
};
