import axios from "axios";

const config = {
  clientId: "react.client",
  getSubscriptionsUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/Home/GetSubscriptions`,
  saveSubscriptionUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/Home/SaveSubscription`,
  scopes: [],
};
export const getSubscriptions = (keyword) => {
  return axios.get(config.getSubscriptionsUrl);
};

export const saveSubscription = (data) => {
  return axios.post(config.saveSubscriptionUrl, data);
};
