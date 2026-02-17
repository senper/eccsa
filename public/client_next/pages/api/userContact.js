import axios from "axios";
const apiController = "/api/Home";
const config = {
  clientId: "react.client",
  saveUserContactUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/SaveContact`,
  getUserContactsUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetContacts`,
  getUserContactUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/GetContact`,
  removeUserContactUri: `${process.env.NEXT_PUBLIC_API_URL}${apiController}/DeleteContact`,
  scopes: [],
};

export const saveUserContact = (data) => {
  return axios.post(config.saveUserContactUri, data);
};

export const getUserContacts = () => {
  return axios.get(config.getUserContactsUri);
};

export const deleteUserContact = (id) => {
  return axios.post(config.removeUserContactUri + "/" + id);
};

export const getUserContact = (id) => {
  return axios.get(config.getUserContactUri + "/" + id);
};
export const ContactTypes = [
  { name: "Mobile", value: 1 },
  { name: "Email", value: 2 },
  { name: "Website", value: 3 },
  { name: "Landline", value: 4 },
  { name: "Fax", value: 5 },
  { name: "Facebook", value: 6 },
  { name: "Twitter", value: 7 },
  { name: "Physical", value: 9 },
  { name: "Other", value: 0 },
];
