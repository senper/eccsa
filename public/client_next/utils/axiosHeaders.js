import axios from "axios";

export function setAuthHeader(token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";

  /*axios.interceptors.request.use(
    function (config) {
      // Do something before request is sent
      return config;
    },
    function (error) {
      // Do something with request error
      
      if (error.status===401 && error.config && 
        !error.config.__isRetryRequest) {
            return;
        }
      // console.log("axois error");
      return Promise.reject(error);
    }
  );

  axios.interceptors.response.use(
    function (response) {
      // Any status code that lie within the range of 2xx cause this function to trigger
      // Do something with response data
      return response;
    },
    function (error) {
      // Any status codes that falls outside the range of 2xx cause this function to trigger
      // Do something with response error
      // console.log("axois error");
      return Promise.reject(error);
    }
  ); */
}
