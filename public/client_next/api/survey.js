import axios from "axios";
import { setAuthHeader } from "../utils/axiosHeaders";

const config = {
  clientId: "react.client",
  getUserSurveyUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiSurvey/GetUserSurvey`,
  getSurveyListUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiSurvey/GetSurveyList`,
  userNotFinishedSurveysUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiSurvey/UserNotFinishedSurveys`,
  saveResponseUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiSurvey/SaveResponse`,
  scopes: [],
};

export const getUserSurvey = (id) => {
  return axios.get(config.getUserSurveyUri + "/" + id);
};

export const userNotFinishedSurveysUri = () => {
  return axios.get(config.userNotFinishedSurveysUri);
};

export const getSurveyList = () => {
  return axios.get(config.getSurveyListUri);
};
export const saveResponse = (id, data) => {
  return axios.post(config.saveResponseUri + "/" + id, data);
};
