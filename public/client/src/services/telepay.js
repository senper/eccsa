import axios from "axios";

const config = {
  clientId: "react.client",
  initiatePay: `${process.env.REACT_APP_TELEAPI_URL}/payByTelebirr`,
  sendNotifyPay: `${process.env.REACT_APP_TELEAPI_URL}/notifyTelebirr`,
  scopes: [],
};

export const initiatePay = (invoiceNo, paymentReason, amount, returnUrl) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.post(config.initiatePay, { invoiceNo, paymentReason, amount, returnUrl });
  /* } else {
        return null;
    } */
};


export const sendNotify = (invoiceNo, tradeNo) => {
    var token = localStorage.getItem("access_token");
    //if (token) {
    axios.defaults.headers.common["Authorization"] = token
      ? "Bearer " + token
      : "";
    return axios.post(config.sendNotifyPay, { invoiceNo, tradeNo });
    /* } else {
          return null;
      } */
  };

