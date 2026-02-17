import axios from "axios";

const config = {
  clientId: "react.client",
  eventsUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiEvent/Events`,
  pastEventsUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiEvent/PastEvents`,
  eventUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiEvent/GetEvent`,
  scopes: [],
};

export const getEvents = () => {
  return axios.get(config.eventsUri);
};
export const getPastEvents = () => {
  return axios.get(config.pastEventsUri);
};
export const getEvent = (id) => {
  return axios.get(config.eventUri + "/" + id);
};
