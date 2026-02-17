import axios from "axios";

const config = {
  clientId: "react.client",
  photoAlbumsUri: `${process.env.REACT_APP_API_URL}/api/ApiGallery/PhotoAlbums`,
  getAlbumPhotosUri: `${process.env.REACT_APP_API_URL}/api/ApiGallery/GetAlbumPhotos`,
  videoGalleriesUri: `${process.env.REACT_APP_API_URL}/api/ApiGallery/VideoGalleries`,
  scopes: [],
};

export const getPhotoAlbums = () => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.photoAlbumsUri);
  /* } else {
        return null;
    } */
};

export const getAlbumPhotos = (id) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
    axios.defaults.headers.common["Authorization"] = token
      ? "Bearer " + token
      : "";
    return axios.get(config.getAlbumPhotosUri + "/" + id);
    /* } else {
          return null;
      } */
  };

export const getVideoGalleries = () => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.videoGalleriesUri);
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