import axios from "axios";
import { setAuthHeader } from "../utils/axiosHeaders";

const config = {
  clientId: "react.client",
  newsUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiNews/LoadNews`,
  newsByIdUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiNews/GetNewsById`,
  newsHighlightUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiNews/NewsHighlight`,
  newsForSliderUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiNews/NewsForSlider`,
  scopes: [],
};

export const getNews = () => {
  return axios.get(config.newsUri);
};

export const getNewsById = (id) => {
  return axios.get(config.newsByIdUri + "/" + id);
};

export const getNewsHighlight = () => {
  return axios.get(config.newsHighlightUri);
};

export const getNewsForSlider = () => {
  return axios.get(config.newsForSliderUrl);
};
