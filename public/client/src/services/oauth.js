import axios from "axios";
import { setAuthHeader } from "../utils/axiosHeaders";
import {
  storeUserError,
  storeUser,
  userSignedOut,
} from "../actions/authActions";
import store from "../store";

const config = {
  clientId: "react.client",
  accessTokenUri: `${process.env.REACT_APP_API_URL}/api/Authenticate/login`,
  registerUri: `${process.env.REACT_APP_API_URL}/api/Authenticate/register`,
  userInfoUri: `${process.env.REACT_APP_API_URL}/api/Home/GetUserProfile`,
  updateProfileUri: `${process.env.REACT_APP_API_URL}/api/Home/updateProfile`,
  changePasswordUri: `${process.env.REACT_APP_API_URL}/api/Home/changePassword`,
  isLunchedUrl: `${process.env.REACT_APP_API_URL}/api/Authenticate/Launched`,
  setLunchedUrl: `${process.env.REACT_APP_API_URL}/api/Authenticate/Launch`,
};

//const oauth = new ClientOAuth2(config);

export async function loadUserFromStorage(store) {
  try {
    let user = await userProfile();
    if (!user) {
      localStorage.removeItem("access_token");
      return store.dispatch(storeUserError());
    }
    store.dispatch(storeUser(user.data));
    return user;
  } catch (e) {
    console.error(`User not found: ${e}`);
    store.dispatch(storeUserError());
    localStorage.removeItem("access_token");
  }
}

export const login = (params) => {
  //return oauth.owner
  //  .getToken(params.email, params.password)
  //  .then((result) => {
  //    localStorage.setItem("access_token", result.accessToken);
  //    setAuthHeader(result.accessToken);

  //    //get and store user details
  //    loadUserFromStorage(store);

  //    return result;
  //  })
  //  .catch((error) => {
  //    localStorage.removeItem("access_token");
  //    console.log(error);
  //  });
  return axios
    .post(config.accessTokenUri, {
      userName: params.userName,
      password: params.password,
    })
    .then((result) => {
      localStorage.setItem("access_token", result.data.token);
      setAuthHeader(result.data.token);

      //get and store user details
      loadUserFromStorage(store);

      return result;
    })
    .catch((error) => {
      localStorage.removeItem("access_token");
      console.log(error);
    });
};

export const registerUser = (params) => {
  var token = localStorage.getItem("access_token");
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.registerUri, params);
};

export const updateProfile = (params) => {
  var token = localStorage.getItem("access_token");
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.updateProfileUri, params);
};

export const changePassword = (params) => {
  var token = localStorage.getItem("access_token");
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.changePasswordUri, params);
};

export const logoff = () => {
  userSignedOut();
};

export const userProfile = () => {
  var token = localStorage.getItem("access_token");
  if (token) {
    axios.defaults.headers.common["Authorization"] = token
      ? "Bearer " + token
      : "";
    return axios.get(config.userInfoUri);
  } else {
    return null;
  }
};

export const getIsLunched = () => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.isLunchedUrl);
  //} else {
  //  return null;
  // }
};

export const setLunched = (code) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.setLunchedUrl, { Code: code });
  //} else {
  //  return null;
  // }
};

/* export const validateToken = (token) => {
  return axios
    .post(config.introspectURL, { token: token })
    .then((result) => {
    })
    .catch((error) => {
      console.error(error);
    });
}; */

//export default oauth;
