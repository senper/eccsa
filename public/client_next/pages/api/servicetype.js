import axios from "axios";

const config = {
  clientId: "react.client",
  serviceTypesUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiServiceType`,
  serviceOwnersUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiServiceType/owners`,
  serviceOwnerUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiServiceType/GetOwner`,
  scopes: [],
};

export const getServiceTypes = () => {
  return axios.get(config.serviceTypesUri);
};

export const getServiceType = (id) => {
  return axios.get(config.serviceTypesUri + "/" + id);
};

export const getServiceOwners = () => {
  return axios.get(config.serviceOwnersUri);
};

export const getServiceOwner = (id) => {
  return axios.get(config.serviceOwnerUri + "/" + id);
};
export const CertificateOfOriginId = "d7571107-ab29-43b3-b95c-c0ae12505a61";

