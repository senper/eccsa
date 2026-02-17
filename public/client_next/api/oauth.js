import axios from "axios";
import { setAuthHeader } from "../utils/axiosHeaders";
import {
  storeUserError,
  storeUser,
  userSignedOut,
} from "../actions/authActions";

const config = {
  clientId: "react.client",
  accessTokenUri: `${process.env.NEXT_PUBLIC_API_URL}/api/Authenticate/login`,
  registerUri: `${process.env.NEXT_PUBLIC_API_URL}/api/Authenticate/register`,
  userInfoUri: `${process.env.NEXT_PUBLIC_API_URL}/api/Home/GetUserProfile`,
  updateProfileUri: `${process.env.NEXT_PUBLIC_API_URL}/api/Home/updateProfile`,
  changePasswordUri: `${process.env.NEXT_PUBLIC_API_URL}/api/Home/changePassword`,
  isLunchedUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/Authenticate/Launched`,
  setLunchedUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/Authenticate/Launch`,
  verifyEmailMobileUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/Home/verifyEmailMobile`,
  getEmailMobileVerificationUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/Home/getEmailMobileVerification`,
  getPasswordResetLinkUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/Home/getPasswordResetLink`,
  resetPasswordUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/Home/resetPassword`,
  undoEmailMobileChangeUrl: `${process.env.NEXT_PUBLIC_API_URL}/api/Home/undoEmailMobileChange`,
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
    //console.error(`User not found: ${e}`);
    store.dispatch(storeUserError());
    localStorage.removeItem("access_token");
  }
}

export const login = (params, store) => {
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
      // console.log(error);
    });
};


export const resetPassword = (params) => {
  return axios.post(config.resetPasswordUrl, params);
};
export const getPasswordResetLink = (params) => {
  return axios.post(config.getPasswordResetLinkUrl, params);
};

export const getEmailMobileVerification = (params) => {
  return axios.post(config.getEmailMobileVerificationUrl, params);
};

export const verifyEmailMobile = (params) => {
  return axios.post(config.verifyEmailMobileUrl, params);
};

export const registerUser = (params) => {
  return axios.post(config.registerUri, params);
};

export const updateProfile = (params) => {
  return axios.post(config.updateProfileUri, params);
};

export const undoEmailMobileChange = (params) => {
  return axios.post(config.undoEmailMobileChangeUrl, params);
};

export const changePassword = (params) => {
  return axios.post(config.changePasswordUri, params);
};

export const logoff = (store) => {
  setAuthHeader(null);
  localStorage.removeItem("access_token");
  store.dispatch(storeUser(null));
};

export const userProfile = () => {
  setAuthHeader(localStorage.getItem("access_token"));
  return axios.get(config.userInfoUri);
};

export const getIsLunched = () => {
  return axios.get(config.isLunchedUrl);
};

export const setLunched = (code) => {
  return axios.post(config.setLunchedUrl, { Code: code });
};
