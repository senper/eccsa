import axios from "axios";
const apiController = "/api/ApiUserBusinessInterest";
const config = {
  clientId: "react.client",
  getRootSectorsUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetRootSectors`,
  getChildSectorsUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/ChildSectors`,
  getBusinessInterestProductsUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetBusinessInterestProducts`,
  saveBusinessInterestUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveBusinessInterest`,
  getUserBusinessInterestUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetUserBusinessInterest`,
  removeBusinessInterestUrl: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/RemoveBusinessInterest`,
  scopes: [],
};

export const getRootSectors = () => {
  return axios.get(config.getRootSectorsUrl);
};

export const getChildSectors = (id) => {
  return axios.get(config.getChildSectorsUrl + "/" + id);
};

export const getBusinessInterestProducts = () => {
  return axios.get(config.getBusinessInterestProductsUrl);
};

export const getUserBusinessInterest = () => {
  return axios.get(config.getUserBusinessInterestUrl);
};

export const saveBusinessInterest = (data) => {
  return axios.post(config.saveBusinessInterestUrl, data);
};

export const removeBusinessInterest = () => {
  return axios.post(config.removeBusinessInterestUrl);
};

