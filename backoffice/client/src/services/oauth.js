// import ClientOAuth2 from "client-oauth2";
// import axios from "axios";
// import { setAuthHeader } from "utils/axiosHeaders";
// import {
//   storeUserError,
//   storeUser,
//   userSignedOut,
// } from "../actions/authActions";
// import store from "store";

// const config = {
//   clientId: "react.client",
//   accessTokenUri: `${process.env.REACT_APP_AUTH_SERVER_URL}/connect/token`,
//   userInfoUri: `${process.env.REACT_APP_AUTH_SERVER_URL}/connect/userinfo`,
//   endsessionUri: `${process.env.REACT_APP_AUTH_SERVER_URL}/connect/endsession`,
//   introspectURL: `${process.env.REACT_APP_AUTH_SERVER_URL}/connect/introspect`,
//   scopes: [],
// };

// const config2 = {
//   clientId: "react.client",
//   accessTokenUri: `https://localhost:44317/api/Authenticate/login`,
//   userInfoUri: `https://localhost:44317/WeatherForecast/GetUserProfile`,
//   endsessionUri: `${process.env.REACT_APP_AUTH_SERVER_URL}/connect/endsession`,
//   introspectURL: `${process.env.REACT_APP_AUTH_SERVER_URL}/connect/introspect`,
//   scopes: [],
// };

// const oauth = new ClientOAuth2(config);

// export async function loadUserFromStorage(store) {
//   try {
//     let user = await userProfile();
//     if (!user) {
//       return store.dispatch(storeUserError());
//     }
//     store.dispatch(storeUser(user.data));
//     return user.data;
//   } catch (e) {
//     console.error(`User not found: ${e}`);
//     store.dispatch(storeUserError());
//     return "Error";
//   }
// }

// export const login = (params) => {
//   //return oauth.owner
//   //  .getToken(params.email, params.password)
//   //  .then((result) => {
//   //    localStorage.setItem("access_token", result.accessToken);
//   //    setAuthHeader(result.accessToken);

//   //    //get and store user details
//   //    loadUserFromStorage(store);

//   //    return result;
//   //  })
//   //  .catch((error) => {
//   //    localStorage.removeItem("access_token");
//   //    console.log(error);
//   //  });
//   return axios
//     .post(config2.accessTokenUri, {
//       email: params.email,
//       password: params.password,
//     })
//     .then((result) => {
//       localStorage.setItem("access_token", result.data.token);
//       setAuthHeader(result.data.token);

//       //get and store user details
//       loadUserFromStorage(store);

//       return result;
//     })
//     .catch((error) => {
//       localStorage.removeItem("access_token");
//       console.log(error);
//     });
// };

// export const logoff = () => {
//   userSignedOut();
// };

// export const userProfile = () => {
//   var token = localStorage.getItem("access_token");
//   console.log(token);
//   if (token) {
//     axios.defaults.headers.common["Authorization"] = token
//       ? "Bearer " + token
//       : "";
//     return axios.get(config2.userInfoUri);
//   } else {
//     return null;
//   }
// };

// export const validateToken = (token) => {
//   return axios
//     .post(config.introspectURL, { token: token })
//     .then((result) => {
//       console.log(result);
//     })
//     .catch((error) => {
//       console.error(error);
//     });
// };

// export default oauth;
